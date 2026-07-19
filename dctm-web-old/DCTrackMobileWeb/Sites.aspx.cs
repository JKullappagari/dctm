/*
File Name   :	Sites.aspx.cs

Description :	Used to assign location for shift

Date created:	27 March 2006

Modification History:
***********************
CR		Name			Date			Description
New		Venkatesan M	27/03/2006		File has been created.
*/

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
using iAssetTrack.BAL;
using iAssetTrack.DALC;
using System.Data.SqlClient;
using iAssetTrackBAL;
using Infragistics.Web.UI.GridControls;

public partial class Sites : System.Web.UI.Page
{
    #region "Declarations"
    private iAssetTrack.BAL.SitesBAL objSite;
    private iAssetTrack.BAL.CommonBAL objCommon;
    DataTable _dtRights;
    protected int totalRecordCount = 0;
    private iAssetTrack_WebDataGrid_Paging_CustomerPagerControl pagerControl;
    private const string PROP_FILTERCOUNT = "FilteredCount";

    public string FilterCount
    {
        get
        {
            return (ViewState[PROP_FILTERCOUNT] != null ? ViewState[PROP_FILTERCOUNT].ToString() : "");
        }
        set
        {
            ViewState[PROP_FILTERCOUNT] = value;
        }
    }
    #endregion

    #region "Page Event Methods"
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        grdSite.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(grdSite_ItemCommand);
        pagerControl = grdSite.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);
    }
    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdSite.Behaviors.Paging.PageIndex = e.PageNumber;
        populateGrid();
    }
    protected void grdSite_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControl.SetupPageList(this.grdSite.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdSite.Behaviors.Paging.PageIndex);
        }
        Control postbackControlInstance = null;
        string postbackControlName = this.Request.Params.Get("__EVENTTARGET");
        if (postbackControlName != null && postbackControlName != string.Empty)
            postbackControlInstance = this.FindControl(postbackControlName);
        if (postbackControlInstance != null && postbackControlInstance.ID == "PagerPageList")
        {
            //do nothing
        }
        else
        {
            pagerControl.SetupPageList(this.grdSite.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdSite.Behaviors.Paging.PageIndex);
        }

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        populateGrid();
    }

    /// <summary>
    /// Used to call once page loading complete.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void Page_Load(object sender, EventArgs e)
    {

        Session["PageHeader"] = "Site";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        lblMessage.Visible = false;
        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "Sites.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        //if (_dtRights.Select("Module = 'Site'").Length != 0)
        //{
        //    blfoundPage = true;
        //}

        if (_dtRights.Select("Module = 'Site' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }
        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }
        if (_dtRights.Select("Module = 'Site' and Rights = '" + "Create" + "'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }
        if (_dtRights.Select("Module = 'Site' and Rights = '" + "Delete" + "'").Length != 0)
        {
            ibDelete.Visible = true;
        }
        else
        {
            ibDelete.Visible = false;
        }
        this.grdSite.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());
        populateGrid();
        if (!IsPostBack)
        {
            // populateGrid();
            Session["SiteID"] = null;
            //populateGrid();
        }
    }

    /// <summary>
    /// Used to call once page loading complete.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        if (ibDelete.Enabled)
        {
            iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();
            string sMessage = objCommon.displayMessage(MessageCodes.SITE_JS_DELETE);
            this.hdnMessage.Value = sMessage;
        }
        string errMsg = GetLocalResourceObject("revDesc1Resource1.ErrorMessage").ToString();
        cvDesc.ErrorMessage = errMsg.Replace("{MAXLENGTH}", txtDesc.MaxLength.ToString());

    }

    /// <summary>
    /// Used to save Master data for site.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        objSite = new iAssetTrack.BAL.SitesBAL();
        objSite.Site = txtSite.Text.Trim();
        //if (Session["SiteID"] != null)
        //{
        //    objSite.SiteID = (int)Session["SiteID"];
        //}
        //else
        //{
        //    objSite.SiteID = objSite.exists();
        //}
        objSite.Description = txtDesc.Text.Trim();
        objSite.Status = 1;
        objSite.CreatedBy = Convert.ToInt32(Session["UserID"]);

        int intSite = 0;
        objSite.SiteID = Session["SiteID"] == null ? intSite : (int)Session["SiteID"];
        intSite = objSite.exists();

        if (intSite != -1 && intSite != 0)
            objSite.SiteID = intSite;

        # region v3.8

        if (ddlCountry.SelectedIndex > 0)
            objSite.CountryID = int.Parse(ddlCountry.SelectedItem.Value);
        if (ddlCity.SelectedIndex > 0)
            objSite.CityID = int.Parse(ddlCity.SelectedItem.Value);
        # endregion

        if (intSite != -1)
        {
            objSite.Persist(DALCOperation.Insert);
            clearFields();
            if (Session["SiteID"] == null)
            {
                objCommon = new CommonBAL();
                lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_INSERTED);
            }
            else
            {
                objCommon = new CommonBAL();
                lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_UPDATED);
            }

            Session["SiteID"] = null;
            lblMessage.Visible = true;
            grdSite.ClearDataSource();
            populateGrid();
        }
        else
        {
            objCommon = new CommonBAL();
            lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_I_EXISTS);
            lblMessage.Visible = true;
            populateGrid();
        }
    }

    /// <summary>
    /// Used to clear form control value(s).
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibReset_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        clearFields();
        txtSite.Focus();
        Session["SiteID"] = null;
        populateGrid();

    }

    /// <summary>
    /// Used to delete information related to specific site.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibDelete_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        CheckBox chkDelete;
        int SiteId;
        string strIDs;

        strIDs = "";

        foreach (Infragistics.Web.UI.GridControls.GridRecord grdViewRow in grdSite.Rows)
        {
            chkDelete = (CheckBox)(grdViewRow.Items[4].FindControl("chkDelete"));
            if (chkDelete.Checked == true)
            {
                SiteId = Convert.ToInt32(((Label)(grdViewRow.Items[4].FindControl("lblDeleteID"))).Text);
                strIDs += Convert.ToString(SiteId) + ",";
            }
        }

        if (strIDs != "")
        {
            strIDs = strIDs.Remove(strIDs.Length - 1, 1);
        }

        objSite = new iAssetTrack.BAL.SitesBAL();
        objSite.SiteIDs = strIDs;
        objSite.Status = 0;
        objSite.LastModifiedBy = Convert.ToInt32(Session["UserID"]);
        objSite.Persist(DALCOperation.Delete);
        clearFields();
        objCommon = new CommonBAL();
        lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_DELETED);
        lblMessage.Visible = true;
        grdSite.ClearDataSource();
        populateGrid();
    }

    /// <summary>
    /// Used to call upon grid page index changes.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void grdSite_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //grdSite.PageIndex = e.NewPageIndex;
        //populateGrid();
    }

    /// <summary>
    /// Used to call upon grid row edits.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void grdSite_RowEditing(object sender, GridViewEditEventArgs e)
    {
        //populateGrid();
        //iAssetTrack.BAL.SitesBAL objEdit = new iAssetTrack.BAL.SitesBAL();
        //objEdit.SiteID = Convert.ToInt32(grdSite.DataKeyFields[Convert.ToInt32(e.NewEditIndex)].ToString());
        //DataSet dsSite = objEdit.retrieve();
        //DataRow dr = dsSite.Tables[0].Rows[0];
        //txtSite.Text = dr[DBFields.DBFIELD_SITE].ToString();
        //txtDesc.Text = dr[DBFields.DBFIELD_DESCRIPTION].ToString();
        //Session["SiteID"] = objEdit.SiteID;
    }
    #endregion

    #region "User Defined Methods"
    private void populateGrid()
    {
        objSite = new iAssetTrack.BAL.SitesBAL();
        DataTable dtGrid = objSite.retrieve().Tables[0];
        totalRecordCount = dtGrid.Rows.Count;

        grdSite.Behaviors.Paging.Enabled = false;
        if (totalRecordCount == grdSite.Rows.Count)
            this.FilterCount = "";
        grdSite.Behaviors.Paging.Enabled = true;

        if (grdSite.Rows.Count == 0)
        {
            grdSite.DataSource = dtGrid;
            grdSite.DataBind();
            grdSite.Columns[4].Hidden = true;
            ibDelete.Visible = false;
        }
        else //if (grdSite.Rows.Count > 0)
        {
            grdSite.DataSource = dtGrid;
            grdSite.DataBind();
            if (_dtRights.Select("Rights = 'Delete' and Module = 'Site'").Length != 0)
            {
                grdSite.Columns[4].Hidden = false;
                ibDelete.Visible = true;

            }
            else
            {
                grdSite.Columns[4].Hidden = true;
                ibDelete.Visible = false;
            }
            if (_dtRights.Select("Rights = 'Modify' and Module = 'Site'").Length != 0)
            {
                grdSite.Columns[3].Hidden = false;
            }
            else
            {
                grdSite.Columns[3].Hidden = true;
            }
            //   }



            if (ibDelete.Visible == true)
            {
                int SiteId;
                int iCount = 0;

                for (int i = 0; i < grdSite.Rows.Count; i++)
                {
                    SiteId = Convert.ToInt32(((Label)(grdSite.Rows[i].Items[4].FindControl("lblDeleteID"))).Text);

                    grdSite.Rows[i].Items[4].FindControl("chkDelete").Visible = true;
                    objCommon = new iAssetTrack.BAL.CommonBAL();

                    DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_SITEID, SiteId.ToString(), 0);
                    foreach (DataTable tblCheck in dsCheck.Tables)
                    {
                        if (tblCheck.Rows[0][0].ToString() != "0")
                        {
                            grdSite.Rows[i].Items[4].FindControl("chkDelete").Visible = false;
                        }


                    }
                    if (grdSite.Rows[i].Items[4].FindControl("chkDelete").Visible == false)
                    {
                        iCount += 1;
                    }

                }

                if (iCount == grdSite.Rows.Count)
                {
                    grdSite.Columns[4].Hidden = true;
                    ibDelete.Visible = false;
                }
            }
            if (grdSite.Rows.Count > 0)
            {
                ibExportToExcel.Enabled = true;
                ibExportToExcel.Visible = true;

            }
            else
            {
                ibExportToExcel.Enabled = false;
                ibExportToExcel.Visible = false;
            }

        }
    }
    private void applyRights()
    {

    }

    private void clearFields()
    {
        txtSite.Text = "";
        txtDesc.Text = "";
        lblMessage.Visible = false;
        lblMessage.Text = "";
        ddlCity.Items.Clear();
        ddlCountry.Items.Clear();
        if (ddlRegion.Items.Count > 0)
            ddlRegion.SelectedIndex = 0;
    }

    private void CompareItem(DropDownList ddl, string itemOrigin)
    {
        string itemToCompare = string.Empty;
        foreach (ListItem item in ddl.Items)
        {
            itemToCompare = item.Text.ToLower();
            if (itemOrigin.ToLower().CompareTo(itemToCompare) == 0)
            {
                ddl.ClearSelection();
                item.Selected = true;
            }
        }
    }

    #endregion

    protected void grdSite_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            populateGrid();
            iAssetTrack.BAL.SitesBAL objEdit = new iAssetTrack.BAL.SitesBAL();
            objEdit.SiteID = Convert.ToInt32(e.CommandArgument);
            DataSet dsSite = objEdit.retrieve();
            DataRow dr = dsSite.Tables[0].Rows[0];
            txtSite.Text = dr[DBFields.DBFIELD_SITE].ToString();
            txtDesc.Text = dr[DBFields.DBFIELD_DESCRIPTION].ToString();
            //v3.8
            ddlRegion.SelectedIndex = 0;
            CompareItem(ddlRegion, Convert.ToString(dr[DBFields.DBFIELD_REGION]));
            PopulateCountry();
            CompareItem(ddlCountry, Convert.ToString(dr[DBFields.DBFIELD_COUNTRY_NAME]));
            PopulateCity();
            CompareItem(ddlCity, Convert.ToString(dr[DBFields.DBFIELD_CITY_NAME]));
            //--//
            Session["SiteID"] = objEdit.SiteID;
            if (_dtRights.Select("Module = 'Site' and Rights = '" + "Modify" + "'").Length != 0)
            {
                ibCreate.Enabled = true;
            }
            else
            {
                ibCreate.Enabled = false;
            }
        }
        else if (e.CommandName == "Page")
        {
            pagerControl.SetCurrentPageNumber(grdSite.Behaviors.Paging.PageIndex);
        }

    }

    protected void eExporter_CellExported(object sender, ExcelCellExportedEventArgs e)
    {
        int iWSdex = e.Worksheet.Index;
        int iRdex = e.CurrentRowIndex;
        int iCdex = e.CurrentColumnIndex;
        e.Worksheet.Columns[3].Width = 1;
        e.Worksheet.Columns[4].Width = 1;
        if (iWSdex == 0)
        {
            if (iRdex == 0)
            {

                if (iCdex == 3 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
                {
                    e.Worksheet.Rows[iRdex].Cells[iCdex].Value = "";


                }
                if (iCdex == 4 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
                {
                    e.Worksheet.Rows[iRdex].Cells[iCdex].Value = "";
                }

            }
        }
    }

    protected void ibExportToExcel_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        this.eExporter.DataExportMode = DataExportMode.AllDataInDataSource;
        Infragistics.Documents.Excel.Workbook wBook = new Infragistics.Documents.Excel.Workbook(excelFormat);
        this.eExporter.Export(this.grdSite, wBook);
    }

    protected void grdSite_DataFiltered(object sender, Infragistics.Web.UI.GridControls.FilteredEventArgs e)
    {
        // Cast the WebDataGrid DataSource to a DataTable  
        DataTable dt = this.grdSite.DataSource as DataTable;

        // Disable WebDataGrid Paging
        grdSite.Behaviors.Paging.Enabled = false;
        this.FilterCount = grdSite.Rows.Count.ToString();
        if (!string.IsNullOrEmpty(this.FilterCount))
        {
            if (totalRecordCount == int.Parse(this.FilterCount))
            {
                this.FilterCount = "";
            }
        }
        // Enable WebDataGrid Paging
        grdSite.Behaviors.Paging.Enabled = true;
    }

    protected void grdSite_InitializeRow(object sender, RowEventArgs e)
    {
        //if (ibDelete.Visible == true)
        //{
        //    int SiteId;
        //    int iCount = 0;

        //    //for (int i = 0; i < grdSite.Rows.Count; i++)
        //    //{
        //        SiteId = Convert.ToInt32(((Label)(e.Row.Items[4].FindControl("lblDeleteID"))).Text);

        //        objCommon = new iAssetTrack.BAL.CommonBAL();

        //        DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_SITEID, SiteId);
        //        foreach (DataTable tblCheck in dsCheck.Tables)
        //        {
        //            if (tblCheck.Rows[0][0].ToString() != "0")
        //            {
        //                e.Row.Items[4].FindControl("chkDelete").Visible = false;
        //            }
        //        }
        //    //    if (e.Row.Items[4].FindControl("chkDelete").Visible == false)
        //    //    {
        //    //        iCount += 1;
        //    //    }

        //    //}

        //    //if (iCount == grdSite.Rows.Count)
        //    //{
        //    //    grdSite.Columns[4].Hidden = false;
        //    //    ibDelete.Visible = false;
        //    //}
        //}
    }
    protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateCountry();

    }

    private void PopulateCountry()
    {
        CountryBAL objCountryBAL = new CountryBAL();
        objCountryBAL.Region = ddlRegion.SelectedItem.Value;
        DataSet dsCountry = objCountryBAL.RetrieveByRegion();
        DataTable dtCountry = dsCountry.Tables[0];

        DataRow dr = dtCountry.NewRow();
        dr["CountryID"] = "0";
        dr["CountryName"] = "-Select-";

        dtCountry.Rows.InsertAt(dr, 0);

        ddlCountry.DataSource = dtCountry;
        ddlCountry.DataTextField = "CountryName";
        ddlCountry.DataValueField = "CountryID";
        ddlCountry.DataBind();

        ddlCountry.SelectedIndex = 0;
        
    }
    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateCity();
    }

    private void PopulateCity()
    {
        CityBAL objCityBAL = new CityBAL();
        objCityBAL.CountryID = int.Parse(ddlCountry.SelectedItem.Value);
        DataSet dsCity = objCityBAL.RetrieveByCountryID();
        DataTable dtCity = dsCity.Tables[0];

        DataRow dr = dtCity.NewRow();
        dr["CityID"] = "0";
        dr["CityName"] = "-Select-";

        dtCity.Rows.InsertAt(dr, 0);

        ddlCity.DataSource = dtCity;
        ddlCity.DataTextField = "CityName";
        ddlCity.DataValueField = "CityID";
        ddlCity.DataBind();

        ddlCity.SelectedIndex = 0;
    }
}