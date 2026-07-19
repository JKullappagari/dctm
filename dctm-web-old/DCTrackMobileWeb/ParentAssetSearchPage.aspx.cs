using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Drawing.Design;
using iAssetTrack.DALC;
using System.Drawing;
using iAssetTrack.BAL;
using iAssetTrackBAL;
using Infragistics.Web.UI.NavigationControls;
using Infragistics.Web.UI.GridControls;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Infragistics.WebUI.WebDataInput;

public partial class ParentAssetSearchPage : System.Web.UI.Page
{
    //v3.8
    private int assetID;
    private int mfgID = 0,siteID = 0,locID = 0;
    private iAssetTrack.BAL.BusinessUnitBAL objBU;
    private iAssetTrack.BAL.SitesBAL objSite;
    private iAssetTrack.BAL.CommonBAL objCommon;
    private iAssetTrack.BAL.SearchParentAssetBAL objSearchParentAsset;
    private ManufacturerBAL objMfg;
    protected WebImageButton ibCreate;
    protected DropDownList ddlBusinessUnit, ddlPrimarySite, ddlMfg;
    protected TextBox txtModel, txtParentLocation, txtRefNumber;
    protected string ddlPrimarySiteVal;
    protected string ddlMfgName = string.Empty;
    protected string ddlMfgID = string.Empty;
    protected string ddlBusinessUnitVal = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageHeader"] = "Parent Asset Search";

        ibCreate = (WebImageButton)this.webAssetSearch.Groups[0].Items[0].FindControl("ibCreate");
        ddlBusinessUnit = (DropDownList)this.webAssetSearch.Groups[0].Items[0].FindControl("ddlBusinessUnit");
        ddlPrimarySite = (DropDownList)this.webAssetSearch.Groups[0].Items[0].FindControl("ddlPrimarySite");
        ddlMfg = (DropDownList)this.webAssetSearch.Groups[0].Items[0].FindControl("ddlMfg");

        txtModel = (TextBox)this.webAssetSearch.Groups[0].Items[0].FindControl("txtModel");
        txtParentLocation = (TextBox)this.webAssetSearch.Groups[0].Items[0].FindControl("txtParentLocation");
        txtRefNumber = (TextBox)this.webAssetSearch.Groups[0].Items[0].FindControl("txtRefNumber");
       
        if (!string.IsNullOrEmpty(hdnModelName.Value))
            txtModel.Text = hdnModelName.Value;

        if (!string.IsNullOrEmpty(hdnLocName.Value))
            txtParentLocation.Text = hdnLocName.Value;


        

        if (!Page.IsPostBack)
        {
            if (Request.QueryString.Count > 0)
            {
                if (!string.IsNullOrEmpty(Request.QueryString.Get("AssetID")))
                {
                    assetID = int.Parse(Request.QueryString.Get("AssetID"));
                }

                if (!string.IsNullOrEmpty(Request.QueryString.Get("MFG")))
                {
                    mfgID = int.Parse(Request.QueryString.Get("MFG"));
                }
                //Location ID
                if (!string.IsNullOrEmpty(Request.QueryString.Get("LID")))
                {
                    locID = int.Parse(Request.QueryString.Get("LID"));
                }

                if (!string.IsNullOrEmpty(Request.QueryString.Get("SID")))
                {
                    siteID = int.Parse(Request.QueryString.Get("SID"));
                }

            }

            LoadBusinessUnits();
            populateManufacturerList();
            LoadSitesByBU();
        }

        //Business Unit ID for TreeList call
        if (ddlBusinessUnit != null && ddlBusinessUnit.Items.Count > 0)
            ddlBusinessUnitVal = ddlBusinessUnit.SelectedItem.Value;
        if (ddlMfg != null && ddlMfg.Items.Count > 0)
        {
            ddlMfgName = ddlMfg.SelectedItem.Text;
            ddlMfgID = ddlMfg.SelectedItem.Value;
        }


        //Check whether Site and Location info comes create or edit asset page
        if (hdnLocationID != null && !string.IsNullOrEmpty(hdnLocationID.Value))
        {
            if (int.Parse(hdnLocationID.Value) > 0)
                locID = int.Parse(hdnLocationID.Value);
        }
        if (siteID > 0)
        {
            CompareItem(ddlPrimarySite, siteID.ToString());
            if (locID > 0)
            {
                LocationBAL locBAL = new LocationBAL();
                locBAL.LocationID = locID;
                DataSet ds = locBAL.retrieve();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    hdnLocationID.Value = ds.Tables[0].Rows[0][0].ToString();
                    hdnLocName.Value = ds.Tables[0].Rows[0][1].ToString();
                    txtParentLocation.Text = hdnLocName.Value;
                }
            }

        }

        
        if (ddlPrimarySite != null && ddlPrimarySite.Items.Count > 0)
        {
            ddlPrimarySiteVal = ddlPrimarySite.SelectedItem.Value;
        }


    }

    private void CompareItem(DropDownList ddl, string itemOrigin)
    {
        string itemToCompare = string.Empty;
        foreach (ListItem item in ddl.Items)
        {
            itemToCompare = item.Value.ToLower();
            if (itemOrigin.ToLower().CompareTo(itemToCompare) == 0)
            {
                ddl.ClearSelection();
                item.Selected = true;
            }
        }
    }

    protected void wibSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        populateGrid();
        //wdgParentAssetSearch.Behaviors.ColumnFixing.FixedColumns.Add("BusinessUnit");
        //wdgParentAssetSearch.Behaviors.ColumnFixing.FixedColumns.Add("AssetGroup");
        //wdgParentAssetSearch.Behaviors.ColumnFixing.FixedColumns.Add("RefNumber");
        //wdgParentAssetSearch.Behaviors.ColumnFixing.FixedColumns.Add("MfgName");
        //wdgParentAssetSearch.Behaviors.ColumnFixing.FixedColumns.Add("ModelName");
        //wdgParentAssetSearch.Behaviors.ColumnFixing.FixedColumns.Add("RFID Card No");
    }
    protected void wibReset_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        ddlPrimarySite.SelectedIndex = 0;
        
        txtModel.Text = "(All)";
        txtParentLocation.Text = "(All)";
        hdnLocName.Value = "";
        hdnModelName.Value = "";
        hdnModelID.Value = "";
        hdnLocationID.Value = "";
        siteID = 0;
        locID = 0;

    }
    protected void ddlPrimarySite_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPrimarySite != null && ddlPrimarySite.Items.Count > 0)
        {
            ddlPrimarySiteVal = ddlPrimarySite.SelectedItem.Value;
            siteID = int.Parse(ddlPrimarySite.SelectedItem.Value);
            //location
            txtParentLocation.Text = "(All)";
            hdnLocationID.Value = "";
            hdnLocName.Value = "";
            locID = 0;
        }
    }
    private void LoadSitesByBU()
    {
        int intBusinessUnitID = 0;
        if (ddlBusinessUnit.SelectedValue != "")
            intBusinessUnitID = Convert.ToInt32(ddlBusinessUnit.SelectedValue);

        objSite = new iAssetTrack.BAL.SitesBAL();
        // objSite.BusinessUnitID = Convert.ToInt16(Session["BUID"]);
        DataSet dsSite = objSite.retrieveByBusinessUnitId(intBusinessUnitID);
        DataTable dtSite = dsSite.Tables[0];

        objCommon = new CommonBAL();
        objCommon.setDataSource(ddlPrimarySite, dtSite, "-Select-");


    }
    private void LoadBusinessUnits()
    {
        objBU = new iAssetTrack.BAL.BusinessUnitBAL();
        objBU.BusinessUnitID = Convert.ToInt16(Session["BUID"]);
        DataSet dsBU = objBU.retrieve();
        DataTable dtBU = dsBU.Tables[0];

        objCommon = new CommonBAL();
        objCommon.setDataSource(ddlBusinessUnit, dtBU, "-Select-");
        ddlBusinessUnit.SelectedIndex = 1;
        ddlBusinessUnit.Enabled = false;
    }


    private void FillDropDownList(string strStoredProc, ref DropDownList ddl, int id)
    {


        iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();
        DataTable dt = objCommon.FillDropDownListBU(strStoredProc, "", id);
        ddl.DataSource = dt;
        ddl.DataValueField = dt.Columns[0].ToString();
        ddl.DataTextField = dt.Columns[1].ToString();
        ddl.DataBind();



    }

    private void populateManufacturerList()
    {
        objMfg = new iAssetTrack.BAL.ManufacturerBAL();
        objMfg.MFGID = mfgID;
        DataSet dsMfg = objMfg.retrieve();
        DataTable dtMfg = dsMfg.Tables[0];

        DataRow dr = dtMfg.NewRow();
        dr[0] = 0;
        dr[1] = "-Select-";
        dtMfg.Rows.InsertAt(dr, 0);

        ddlMfg.DataSource = dtMfg;
        ddlMfg.DataValueField = dtMfg.Columns[0].ToString();
        ddlMfg.DataTextField = dtMfg.Columns[1].ToString();
        ddlMfg.DataBind();

        //if MFGID passed from Create or Edit Asset pages than show only  that manufacturer and disable the control.
        if (mfgID > 0 && dtMfg.Rows.Count == 2)
        {
            ddlMfg.SelectedIndex = 1;
            ddlMfg.Enabled = false;
        }
        else
        {
            ddlMfg.SelectedIndex = 0;
        }

    }
    protected void ddlMfg_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMfg != null && ddlMfg.Items.Count > 0)
        {
            ddlMfgName = ddlMfg.SelectedItem.Text;
            ddlMfgID = ddlMfg.SelectedItem.Value;
        }
        

    }
    private void populateGrid()
    {

        objSearchParentAsset = new iAssetTrack.BAL.SearchParentAssetBAL();

        objSearchParentAsset.BusinessUnitID = Convert.ToInt16(ddlBusinessUnit.SelectedValue);

        if (ddlMfg.SelectedIndex != -1)
        {
            objSearchParentAsset.MFGID = Convert.ToInt16(ddlMfg.SelectedValue);
        }
        else
        {
            objSearchParentAsset.MFGID = 0;
        }

        if (!string.IsNullOrEmpty(hdnLocationID.Value))
        {
            objSearchParentAsset.LocationID = Convert.ToInt16(hdnLocationID.Value);
        }
        if (!string.IsNullOrEmpty(hdnModelID.Value))
        {
            objSearchParentAsset.ModelID = Convert.ToInt16(hdnModelID.Value);
        }
        if (ddlPrimarySite.SelectedIndex != -1)
        {
            objSearchParentAsset.SiteID = Convert.ToInt32(ddlPrimarySite.SelectedValue);
        }
        else
        {
            objSearchParentAsset.SiteID = 1;
        }
        objSearchParentAsset.RefNumber = txtRefNumber.Text;
        DataSet dsSearchParentAsset = null;

        dsSearchParentAsset = objSearchParentAsset.ParentAssetSearch();
        DataTable dtGrid = objSearchParentAsset.ParentAssetSearch().Tables[0];
        // totalRecordCount = dtGrid.Rows.Count;
        this.wdgParentAssetSearch.ClearDataSource();
        //if (dsSearchParentAsset.Tables[0].Rows.Count > 0)
        //{
        //  this.wdgParentAssetSearch.DataSource = dsSearchParentAsset.Tables[0].DefaultView;

        //v3.8
        //Filers the data to eliminate same asset from the list, this is 
        // to aviod assigning self as parent.
        DataRow[] rows = dtGrid.Select("AssetID <> " + assetID.ToString());
        DataTable dt = new DataTable();
        if (rows.Length > 0)
        {
            dt = rows.CopyToDataTable();
        }
        else
        {

        }
        this.wdgParentAssetSearch.DataSource = dt;
        this.wdgParentAssetSearch.DataBind();
    }
    //*V3.8-Added on 23Oct2013- By Amar Vidya
    protected void wdgParentAssetSearch_InitializeRow(object sender, Infragistics.Web.UI.GridControls.RowEventArgs e)
    {
        //if (Convert.ToInt16(e.Row.Items.GetValue(20)) > 0)
        //{
        //    //e.Row.Items[4].FindControl("RefNumber").Visible = false;
        //    //  e.Row.Items[4].CssClass = "disabled";
        //    //e.Row.Items[4].Text=Convert.ToString(e.Row.Items.GetValue(4));
        //    // e.Row.Items[4].FindControl("RefNumber").

        //}


    }

}