/*
File Name   :	AssetType.aspx.cs

Description :	Used to create Document Group

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
using System.Collections.Generic;
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

public partial class AssetType : System.Web.UI.Page //, ICallbackEventHandler
{
    #region "Declarations"
    private AssetGroupBAL objAssetGroup;
    private iAssetTrack.BAL.CommonBAL objCommon;
    private iAssetTrack_WebDataGrid_Paging_CustomerPagerControl pagerControl;
    DataTable _dtRights;
    protected int totalRecordCount = 0;
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

    #region " Page Event Methods "
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        grdAssetType.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(grdAssetType_ItemCommand);
        pagerControl = grdAssetType.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        if (pagerControl != null)
            pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);
    }
    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdAssetType.Behaviors.Paging.PageIndex = e.PageNumber;
        populateGrid();
    }
    protected void grdAssetType_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControl.SetupPageList(this.grdAssetType.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdAssetType.Behaviors.Paging.PageIndex);
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
            pagerControl.SetupPageList(this.grdAssetType.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdAssetType.Behaviors.Paging.PageIndex);
        }

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        populateGrid();
    }
    /// <summary>
    /// Used to load the page.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageHeader"] = "Asset Type";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        lblMessage.Visible = false;

        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "AssetType.aspx";
            Response.Redirect("Login.aspx");
        }

        // bool blfoundPage = _dtRights != null && _dtRights.Select("Module = 'Asset Type'").Length != 0;//Commented 03Mar2013
        bool blfoundPage = false;//Added on 03Mar2013

       
        if (_dtRights.Select("Module = 'Asset Type' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }
        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }
        if (_dtRights.Select("Module = 'Asset Type' and Rights = '" + "Create" + "'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }

        if (_dtRights.Select("Module = 'Asset Type' and Rights = '" + "Delete" + "'").Length != 0)
        {
            ibDelete.Visible = true;
        }
        else
        {
            ibDelete.Visible = false;
        }

        this.grdAssetType.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());

        if (!IsPostBack)
        {
            Session["AssetTypeID"] = null;
            //populateBusinessUnitList();
            //ddlBusinessUnit.Enabled = false; // commented by kjb on 27 Feb 2013
        }
        populateGrid();
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
            string sMessage = objCommon.displayMessage(MessageCodes.ASSETGROUP_JS_DELETE);
            hdnMessage.Value = sMessage;
        }
        txtDesc.Attributes.Add("onkeypress", "doKeypress(this," + txtDesc.MaxLength.ToString() + ");");
        txtDesc.Attributes.Add("onbeforepaste", "doBeforePaste(this," + txtDesc.MaxLength.ToString() + ");");
        txtDesc.Attributes.Add("onpaste", "doPaste(this," + txtDesc.MaxLength.ToString() + ");");
    }

    /// <summary>
    /// Used to call upon grid page index changes.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void grdAssetType_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //grdAssetType.PageIndex = e.NewPageIndex;
        // populateGrid();
    }


    /// <summary>
    /// Used to save information related BU.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {

        objAssetGroup = new AssetGroupBAL();
        objAssetGroup.AssetType = txtAssetType.Text;
        objAssetGroup.Description = txtDesc.Text;

        objAssetGroup.Status = 1;
        objAssetGroup.CreatedBy = Convert.ToInt32(Session["UserID"]);

        int intAssetType = 0;
        objAssetGroup.AssetTypeID = Session["AssetTypeID"] == null ? intAssetType : (int)Session["AssetTypeID"];
        intAssetType = objAssetGroup.exists();

        if (intAssetType != -1 && intAssetType != 0)
            objAssetGroup.AssetTypeID = intAssetType;

        if (intAssetType != -1)
        {
            objAssetGroup.Persist(DALCOperation.Insert);
            clearFields();
            if (Session["AssetTypeID"] == null)
            {
                objCommon = new CommonBAL();
                lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_INSERTED);
            }
            else
            {
                objCommon = new CommonBAL();
                lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_UPDATED);
            }
            lblMessage.Visible = true;
            grdAssetType.ClearDataSource();
            populateGrid();
            Session["AssetTypeID"] = null;
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
        Session["AssetTypeID"] = null;
        txtAssetType.Focus();
        //Added - 13-Nov-2006
        //Check to Show/Hide the delete button
        populateGrid();
    }

    /// <summary>
    /// Used to delete information related specific BU.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibDelete_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        CheckBox chkDelete;
        int AssetTypeId;
        string strIDs;

        strIDs = "";
        foreach (Infragistics.Web.UI.GridControls.GridRecord grdViewRow in grdAssetType.Rows)
        {
            chkDelete = (CheckBox)(grdViewRow.Items[3].FindControl("chkDelete"));
            if (chkDelete.Checked == true)
            {
                AssetTypeId = Convert.ToInt32(((Label)(grdViewRow.Items[3].FindControl("lblDeleteID"))).Text);
                strIDs += Convert.ToString(AssetTypeId) + ",";
            }
        }

        if (strIDs != "")
        {
            strIDs = strIDs.Remove(strIDs.Length - 1, 1);
        }

        objAssetGroup = new AssetGroupBAL();
        objAssetGroup.AssetTypeIDs = strIDs;
        objAssetGroup.Status = 0;
        objAssetGroup.LastModifiedBy = Convert.ToInt32(Session["UserID"]);
        objAssetGroup.Persist(DALCOperation.Delete);

        clearFields();

        objCommon = new CommonBAL();
        lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_DELETED);
        lblMessage.Visible = true;
        grdAssetType.ClearDataSource();
        populateGrid();
    }


    protected void ddlBusinessUnit_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    #endregion

    #region "User Defined Methods"

    //private void populateBusinessUnitList()
    //{
    //    int intUserID = 0;
    //    if (Session["UserID"] != null)
    //        intUserID = Convert.ToInt32(Session["UserID"].ToString());

    //    FillDropDownListBU(StoredProcedures.SP_BUSINESSUNIT_LISTBYUSERID, ref ddlBusinessUnit, intUserID);
    //}


    //private void FillDropDownListBU(string strStoredProc, ref DropDownList ddl, int id)
    //{

    //    //CommonBAL objCommonBAL = new CommonBAL();
    //    //ICommon svcBU = (ICommon)RemotingHelper.CreateProxy(typeof(ICommon));
    //    iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();

    //    DataTable dt = objCommon.FillDropDownListBU(strStoredProc, "-Select-", id);
    //    ddl.DataSource = dt;
    //    ddl.DataValueField = dt.Columns[0].ToString();
    //    ddl.DataTextField = dt.Columns[1].ToString();
    //    ddl.DataBind();

    //}



    /// <summary>
    /// Populate Business Unit data grid
    /// </summary>    
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    private void populateGrid()
    {

        grdAssetType.Rows.Clear();
        objAssetGroup = new AssetGroupBAL();

      
        DataTable dtGrid = objAssetGroup.retrieve().Tables[0];
        totalRecordCount = dtGrid.Rows.Count;

        grdAssetType.Behaviors.Paging.Enabled = false;
        if (totalRecordCount == grdAssetType.Rows.Count)
            this.FilterCount = "";
        grdAssetType.Behaviors.Paging.Enabled = true;

        if (dtGrid.Rows.Count == 0)
        {
            grdAssetType.DataSource = dtGrid;
            grdAssetType.DataBind();
            grdAssetType.Columns[3].Hidden = true;
            ibDelete.Visible = false;
        }
        else //if (grdAssetType.Rows.Count > 0)
        {
           // totalRecordCount = dtGrid.Rows.Count;
            grdAssetType.DataSource = dtGrid;
            grdAssetType.DataBind();
            if (_dtRights.Select("Rights = 'Delete' and Module = 'Asset Type'").Length != 0)
            {
                grdAssetType.Columns[3].Hidden = false;

                ibDelete.Visible = true;
            }
            else
            {
                grdAssetType.Columns[3].Hidden = true;
                ibDelete.Visible = false;
            }

            if (_dtRights.Select("Rights = 'Modify' and Module = 'Asset Type'").Length != 0)
            {
                grdAssetType.Columns[2].Hidden = false;
            }
            else
            {
                grdAssetType.Columns[2].Hidden = true;
            }


            if (ibDelete.Visible == true)
            {
                int AssetTypeId;
                int iCount = 0;

                for (int i = 0; i < grdAssetType.Rows.Count; i++)
                {
                    AssetTypeId = Convert.ToInt32(((Label)(grdAssetType.Rows[i].Items[3].FindControl("lblDeleteID"))).Text);
                    grdAssetType.Rows[i].Items[3].FindControl("chkDelete").Visible = true;
                    objCommon = new iAssetTrack.BAL.CommonBAL();

                    DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_ASSETGROUPID, AssetTypeId.ToString(), 0);
                    foreach (DataTable tblCheck in dsCheck.Tables)
                    {
                        if (tblCheck.Rows[0][0].ToString() != "0")
                        {
                            grdAssetType.Rows[i].Items[3].FindControl("chkDelete").Visible = false;
                        }
                    }
                    if (grdAssetType.Rows[i].Items[3].FindControl("chkDelete").Visible == false)
                    {
                        iCount += 1;
                    }

                }
                if (iCount == grdAssetType.Rows.Count)
                {
                    grdAssetType.Columns[3].Hidden = true;
                    ibDelete.Visible = false;
                }
            }
            if (grdAssetType.Rows.Count > 0)
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
    /// <summary>
    /// Reset fields
    /// </summary>    
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    private void clearFields()
    {
        txtAssetType.Text = "";
        txtDesc.Text = "";
        lblMessage.Visible = false;
        lblMessage.Text = "";
    }



    #endregion

    #region ICallbackEventHandler Members

    public string GetCallbackResult()
    {
        return "";
    }


    #endregion
    protected void grdAssetType_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            populateGrid();
            objAssetGroup = new AssetGroupBAL();
            objAssetGroup.AssetTypeID = Convert.ToInt32(e.CommandArgument);
            DataSet dsAssetType = objAssetGroup.retrieve();
            DataRow dr = dsAssetType.Tables[0].Rows[0];
            txtAssetType.Text = dr[DBFields.DBFIELD_ASSETGROUP].ToString();
            txtDesc.Text = dr[DBFields.DBFIELD_DESCRIPTION].ToString();

            Session["AssetTypeID"] = objAssetGroup.AssetTypeID;

            if (_dtRights.Select("Module = 'Asset Type' and Rights = '" + "Modify" + "'").Length != 0)
            {
                ibCreate.Enabled = true;
            }
            else
            {
                ibCreate.Enabled = false;
            }
        }

    }

    //private void PopulateBUByBUID(int BUID)
    //{
    //    BusinessUnitBAL objBU = new BusinessUnitBAL();
    //    objBU.BusinessUnitID = BUID;
    //    DataTable dtBU = objBU.retrieve().Tables[0];
    //    ddlBusinessUnit.DataValueField = dtBU.Columns[0].ToString();
    //    ddlBusinessUnit.DataTextField = dtBU.Columns[1].ToString();
    //    ddlBusinessUnit.DataSource = dtBU;
    //    ddlBusinessUnit.DataBind();
    //    ddlBusinessUnit.SelectedIndex = 0;
    //}

    protected void eExporter_CellExported(object sender, ExcelCellExportedEventArgs e)
    {

        int iRdex = e.CurrentRowIndex;
        int iCdex = e.CurrentColumnIndex;
        e.Worksheet.Columns[2].Width = 1;
        e.Worksheet.Columns[3].Width = 1;
        if (iRdex == 0)
        {

            if (iCdex == 2 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
            {
                e.Worksheet.Rows[iRdex].Cells[iCdex].Value = "";
            }
            if (iCdex == 3 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
            {
                e.Worksheet.Rows[iRdex].Cells[iCdex].Value = "";
            }
        }

    }
    protected void ibExportToExcel_Click(object sender, EventArgs e)
    {
        Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        this.eExporter.DataExportMode = DataExportMode.AllDataInDataSource;
        Infragistics.Documents.Excel.Workbook wBook = new Infragistics.Documents.Excel.Workbook(excelFormat);
        this.eExporter.Export(this.grdAssetType, wBook);
    }
    protected void eExporter_RowExported(object sender, ExcelRowExportedEventArgs e)
    {

    }
    protected void grdAssetType_DataFiltered(object sender, Infragistics.Web.UI.GridControls.FilteredEventArgs e)
    {
        // Cast the WebDataGrid DataSource to a DataTable  
        DataTable dt = this.grdAssetType.DataSource as DataTable;
        // Disable WebDataGrid Paging
        grdAssetType.Behaviors.Paging.Enabled = false;

        this.FilterCount = grdAssetType.Rows.Count.ToString();
        if (!string.IsNullOrEmpty(this.FilterCount))
        {
            if (totalRecordCount == int.Parse(this.FilterCount))
            {
                this.FilterCount = "";
            }
        }

        // Enable WebDataGrid Paging
        grdAssetType.Behaviors.Paging.Enabled = true;
    }
    protected void grdAssetType_InitializeRow(object sender, RowEventArgs e)
    {

        //int AssetTypeId;
        //AssetTypeId = Convert.ToInt32(((Label)(e.Row.Items[3].FindControl("lblDeleteID"))).Text);

        //objCommon = new iAssetTrack.BAL.CommonBAL();

        //DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_ASSETGROUPID, AssetTypeId);
        //foreach (DataTable tblCheck in dsCheck.Tables)
        //{
        //    if (tblCheck.Rows[0][0].ToString() != "0")
        //    {
        //        e.Row.Items[3].FindControl("chkDelete").Visible = false;
        //    }
        //}
        //if (ibDelete.Visible == true)
        //{
        //    int AssetTypeId;
        //    int iCount = 0;

        //    //for (int i = 0; i < grdAssetType.Rows.Count; i++)
        //    //{
        //        AssetTypeId = Convert.ToInt32(((Label)(e.Row.Items[3].FindControl("lblDeleteID"))).Text);

        //        objCommon = new iAssetTrack.BAL.CommonBAL();

        //        DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_ASSETGROUPID, AssetTypeId);
        //        foreach (DataTable tblCheck in dsCheck.Tables)
        //        {
        //            if (tblCheck.Rows[0][0].ToString() != "0")
        //            {
        //                e.Row.Items[3].FindControl("chkDelete").Visible = false;
        //            }
        //        }
        //        //if (e.Row.Items[3].FindControl("chkDelete").Visible == false)
        //        //{
        //        //    iCount += 1;
        //        //}

        //    //}
        //    //if (iCount == grdAssetType.Rows.Count)
        //    //{
        //    //    grdAssetType.Columns[3].Hidden = true;
        //    //    ibDelete.Visible = false;
        //    //}
        //}
    }
}

