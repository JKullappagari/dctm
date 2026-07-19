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

public partial class Manufacturer : System.Web.UI.Page
{
    #region "Declarations"
    private iAssetTrack.BAL.ManufacturerBAL objMfg;
    private iAssetTrack.BAL.CommonBAL objCommon;
    DataTable _dtRights;
    protected int totalRecordCount = 0;
    private iAssetTrack_WebDataGrid_Paging_CustomerPagerControl pagerControl;
    private const string PROP_FILTERCOUNT = "FilteredCount";
    #endregion

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
    #region "Page Event Methods"
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        grdMfg.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(grdMfg_ItemCommand);
        pagerControl = grdMfg.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);
    }
    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdMfg.Behaviors.Paging.PageIndex = e.PageNumber;
        populateGrid();
    }
    protected void grdMfg_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControl.SetupPageList(this.grdMfg.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdMfg.Behaviors.Paging.PageIndex);
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
            pagerControl.SetupPageList(this.grdMfg.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdMfg.Behaviors.Paging.PageIndex);
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
        Session["PageHeader"] = "Manufacturer";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        lblMessage.Visible = false;


        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "Manufacturer.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        if (_dtRights.Select("Module = 'Manufacturer'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }
        if (_dtRights.Select("Module = 'Manufacturer' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }
        if (_dtRights.Select("Module = 'Manufacturer' and Rights = '" + "Delete" + "'").Length != 0)
        {
            ibDelete.Visible = true;
        }
        else
        {
            ibDelete.Visible = false;
        }

        if (_dtRights.Select("Rights = 'Create' and Module = 'Manufacturer'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }

        this.grdMfg.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());
        populateGrid();



        if (!IsPostBack)
        {
            Session["MfgID"] = null;

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
            string sMessage = objCommon.displayMessage(MessageCodes.MFG_JS_DELETE);
            this.hdnMessage.Value = sMessage;
        }
        string errMsg = GetLocalResourceObject("revDesc1Resource1.ErrorMessage").ToString();
        cvDesc.ErrorMessage = errMsg.Replace("{MAXLENGTH}", txtDesc.MaxLength.ToString());
    }
    /// <summary>
    /// Used to save Master data for Department.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>

    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        objMfg = new iAssetTrack.BAL.ManufacturerBAL();
        objMfg.MfgName = txtMfg.Text.Trim();

        objMfg.Description = txtDesc.Text.Trim();
        objMfg.Status = 1;
        objMfg.CreatedBy = Convert.ToInt32(Session["UserID"]);

        int intMfg = 0;
        objMfg.MFGID = Session["MfgID"] == null ? intMfg : (int)Session["MfgID"];
        intMfg = objMfg.exists();

        if (intMfg != -1 && intMfg != 0)
            objMfg.MFGID = intMfg;

        if (intMfg != -1)
        {
            objMfg.Persist(DALCOperation.Insert);
            clearFields();
            if (Session["MfgID"] == null)
            {
                objCommon = new CommonBAL();
                lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_INSERTED);
            }
            else
            {
                objCommon = new CommonBAL();
                lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_UPDATED);
            }

            Session["MfgID"] = null;
            lblMessage.Visible = true;
            grdMfg.ClearDataSource();
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
        txtMfg.Focus();
        Session["MfgID"] = null;
        //Added - 13-Nov-2006
        //Check to Show/Hide the delete button
        populateGrid();
    }
    /// <summary>
    /// Used to delete information related to specific Department.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibDelete_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        CheckBox chkDelete;
        int mfgID;
        string strIDs;

        strIDs = "";

        foreach (Infragistics.Web.UI.GridControls.GridRecord grdViewRow in grdMfg.Rows)
        {
            chkDelete = (CheckBox)(grdViewRow.Items[3].FindControl("chkDelete"));
            if (chkDelete.Checked == true)
            {
                mfgID = Convert.ToInt32(((Label)(grdViewRow.Items[3].FindControl("lblDeleteID"))).Text);
                strIDs += Convert.ToString(mfgID) + ",";
            }
        }

        if (strIDs != "")
        {
            strIDs = strIDs.Remove(strIDs.Length - 1, 1);
        }

        objMfg = new iAssetTrack.BAL.ManufacturerBAL();
        objMfg.MFGIDs = strIDs;
        objMfg.Status = 0;
        objMfg.LastModifiedBy = Convert.ToInt32(Session["UserID"]);
        objMfg.Persist(DALCOperation.Delete);
        clearFields();
        objCommon = new CommonBAL();
        lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_DELETED);
        lblMessage.Visible = true;
        grdMfg.ClearDataSource();
        populateGrid();
    }
    ///// <summary>
    ///// Used to call upon grid page index changes.
    ///// </summary>
    ///// <author>Venkatesan</author>
    ///// <createdOn>27 March 2006</createdOn>
    //protected void grdPurpose_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    //grdPurpose.PageIndex = e.NewPageIndex;
    //    //populateGrid();
    //}
    ///// <summary>
    ///// Used to call upon grid row edits.
    ///// </summary>
    ///// <author>Venkatesan</author>
    ///// <createdOn>27 March 2006</createdOn>
    //protected void grdPurpose_RowEditing(object sender, GridViewEditEventArgs e)
    //{

    //    //populateGrid();
    //    //iAssetTrack.BAL.PurposesBAL objEdit = new iAssetTrack.BAL.PurposesBAL();
    //    //objEdit.PurposeID = Convert.ToInt32(grdPurpose.DataKeys[Convert.ToInt32(e.NewEditIndex)].Value);
    //    //DataSet dsPurpose = objEdit.retrieve();
    //    //DataRow dr = dsPurpose.Tables[0].Rows[0];
    //    //txtPurpose.Text = dr[DBFields.DBFIELD_PURPOSE].ToString();
    //    //txtDesc.Text = dr[DBFields.DBFIELD_DESCRIPTION].ToString();
    //    //Session["PurposeID"] = objEdit.PurposeID;
    //}
    #endregion
    #region "User Defined Methods"
    private void populateGrid()
    {
        //objMfg = new iAssetTrack.BAL.ManufacturerBAL();
        //DataTable dtMfg = objMfg.retrieve().Tables[0];
        //grdMfg.DataSource = dtMfg;
        //grdMfg.DataBind();

        objMfg = new iAssetTrack.BAL.ManufacturerBAL();
        DataTable dtGrid = objMfg.retrieve().Tables[0];
        totalRecordCount = dtGrid.Rows.Count;

        grdMfg.Behaviors.Paging.Enabled = false;
        if (totalRecordCount == grdMfg.Rows.Count)
            this.FilterCount = "";
        grdMfg.Behaviors.Paging.Enabled = true;

        if (dtGrid.Rows.Count == 0)
        {
            //grdMfg.DataSource = null;
            grdMfg.DataSource = dtGrid;
            grdMfg.DataBind();
            grdMfg.Columns[2].Hidden = true;
            grdMfg.Columns[3].Hidden = true;
            ibDelete.Visible = false;
        }
        else// if(dtGrid.Rows.Count > 0)
        {
            grdMfg.DataSource = dtGrid;
            grdMfg.DataBind();
            if (_dtRights.Select("Rights = 'Delete' and Module = 'Manufacturer'").Length != 0)
            {
                grdMfg.Columns[3].Hidden = false;
                ibDelete.Visible = true;
            }
            else
            {
                grdMfg.Columns[3].Hidden = true;
                ibDelete.Visible = false;
            }
            if (_dtRights.Select("Rights = 'Modify' and Module = 'Manufacturer'").Length != 0)
            {
                grdMfg.Columns[2].Hidden = false;
            }
            else
            {
                grdMfg.Columns[2].Hidden = true;
            }

            if (ibDelete.Visible == true)
            {
                int MfgId;
                int iCount = 0;

                for (int i = 0; i < grdMfg.Rows.Count; i++)
                {
                    MfgId = Convert.ToInt32(((Label)(grdMfg.Rows[i].Items[3].FindControl("lblDeleteID"))).Text);
                    grdMfg.Rows[i].Items[3].FindControl("chkDelete").Visible = true;
                    objCommon = new iAssetTrack.BAL.CommonBAL();

                    DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_MFGID, MfgId.ToString(), 0);
                    foreach (DataTable tblCheck in dsCheck.Tables)
                    {
                        if (tblCheck.Rows[0][0].ToString() != "0")
                        {
                            grdMfg.Rows[i].Items[3].FindControl("chkDelete").Visible = false;
                        }
                    }
                    if (grdMfg.Rows[i].Items[3].FindControl("chkDelete").Visible == false)
                    {
                        iCount += 1;
                    }

                }

                if (iCount == grdMfg.Rows.Count)
                {
                    grdMfg.Columns[3].Hidden = true;
                    ibDelete.Visible = false;
                }
            }
            if (grdMfg.Rows.Count > 0)
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
    private void clearFields()
    {
        txtMfg.Text = "";
        txtDesc.Text = "";
        lblMessage.Visible = false;
        lblMessage.Text = "";
    }
    #endregion
    protected void grdMfg_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            populateGrid();
            iAssetTrack.BAL.ManufacturerBAL objEdit = new iAssetTrack.BAL.ManufacturerBAL();
            objEdit.MFGID = Convert.ToInt32(e.CommandArgument);
            DataSet dsMfg = objEdit.retrieve();
            DataRow dr = dsMfg.Tables[0].Rows[0];
            txtMfg.Text = dr[DBFields.DBFIELD_MFGNAME].ToString();
            txtDesc.Text = dr[DBFields.DBFIELD_MFGDESCRIPTION].ToString();
            Session["MfgID"] = objEdit.MFGID;

            if (_dtRights.Select("Module = 'Manufacturer' and Rights = '" + "Modify" + "'").Length != 0)
            {
                ibCreate.Enabled = true;
            }
            else
            {
                ibCreate.Enabled = false;
            }
        }
    }
    protected void ibExportToExcel_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        this.eExporter.DataExportMode = DataExportMode.AllDataInDataSource;
        Infragistics.Documents.Excel.Workbook wBook = new Infragistics.Documents.Excel.Workbook(excelFormat);
        this.eExporter.Export(this.grdMfg, wBook);
    }



    protected void eExporter_CellExported(object sender, ExcelCellExportedEventArgs e)
    {
        int iWSdex = e.Worksheet.Index;
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
    protected void grdMfg_DataFiltered(object sender, Infragistics.Web.UI.GridControls.FilteredEventArgs e)
    {
        // Cast the WebDataGrid DataSource to a DataTable  
        DataTable dt = this.grdMfg.DataSource as DataTable;
        // Disable WebDataGrid Paging
        grdMfg.Behaviors.Paging.Enabled = false;
        this.FilterCount = grdMfg.Rows.Count.ToString();
        if (!string.IsNullOrEmpty(this.FilterCount))
        {
            if (totalRecordCount == int.Parse(this.FilterCount))
            {
                this.FilterCount = "";
            }
        }
        // Enable WebDataGrid Paging
        grdMfg.Behaviors.Paging.Enabled = true;
    }
    protected void grdMfg_InitializeRow(object sender, RowEventArgs e)
    {
        //if (ibDelete.Visible == true)
        //{
        //    int PurposeId;
        //    int iCount = 0;

        //    //for (int i = 0; i < grdPurpose.Rows.Count; i++)
        //    //{
        //        PurposeId = Convert.ToInt32(((Label)(e.Row.Items[3].FindControl("lblDeleteID"))).Text);

        //        objCommon = new iAssetTrack.BAL.CommonBAL();

        //        DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_PURPOSEID, PurposeId);
        //        foreach (DataTable tblCheck in dsCheck.Tables)
        //        {
        //            if (tblCheck.Rows[0][0].ToString() != "0")
        //            {
        //                e.Row.Items[3].FindControl("chkDelete").Visible = false;
        //            }
        //        }
        //    //    if (e.Row.Items[3].FindControl("chkDelete").Visible == false)
        //    //    {
        //    //        iCount += 1;
        //    //    }

        //    //}

        //    //if (iCount == grdPurpose.Rows.Count)
        //    //{
        //    //    grdPurpose.Columns[3].Hidden = true;
        //    //    ibDelete.Visible = false;
        //    //}
        //}
    }
}