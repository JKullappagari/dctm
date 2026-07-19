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

public partial class Purpose : System.Web.UI.Page
{
    #region "Declarations"
    private iAssetTrack.BAL.PurposesBAL objPurpose;
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
        grdPurpose.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(grdPurpose_ItemCommand);
        pagerControl = grdPurpose.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);
    }
    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdPurpose.Behaviors.Paging.PageIndex = e.PageNumber;
        populateGrid();
    }
    protected void grdPurpose_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControl.SetupPageList(this.grdPurpose.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdPurpose.Behaviors.Paging.PageIndex);
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
            pagerControl.SetupPageList(this.grdPurpose.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdPurpose.Behaviors.Paging.PageIndex);
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
        Session["PageHeader"] = "Purpose";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        lblMessage.Visible = false;

       
        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "Purpose.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        //if (_dtRights.Select("Module = 'Purpose'").Length != 0)
        //{
        //    blfoundPage = true;
        //}
        if (_dtRights.Select("Module = 'Purpose' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }


        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }
        if (_dtRights.Select("Module = 'Purpose' and Rights = '" + "Create" + "'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }

        if (_dtRights.Select("Module = 'Purpose' and Rights = '" + "Delete" + "'").Length != 0)
        {
            ibDelete.Visible = true;
        }
        else
        {
            ibDelete.Visible = false;
        }
       
        this.grdPurpose.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());
        populateGrid();

        

        if (!IsPostBack)
        {
            Session["PurposeID"] = null;

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
            string sMessage = objCommon.displayMessage(MessageCodes.PURPOSE_JS_DELETE);
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
        objPurpose = new iAssetTrack.BAL.PurposesBAL();
        objPurpose.Purpose = txtPurpose.Text.Trim();
        
        objPurpose.Description = txtDesc.Text.Trim();
        objPurpose.Status = 1;
        objPurpose.CreatedBy = Convert.ToInt32(Session["UserID"]);

        int intPurpose = 0;
        objPurpose.PurposeID = Session["PurposeID"] == null ? intPurpose : (int)Session["PurposeID"];
        intPurpose = objPurpose.exists();

        if (intPurpose != -1 && intPurpose != 0)
            objPurpose.PurposeID = intPurpose;

        if (intPurpose != -1)
        {
            objPurpose.Persist(DALCOperation.Insert);
            clearFields();
            if (Session["PurposeID"] == null)
            {
                objCommon = new CommonBAL();
                lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_INSERTED);
            }
            else
            {
                objCommon = new CommonBAL();
                lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_UPDATED);
            }

            Session["PurposeID"] = null;
            lblMessage.Visible = true;
            grdPurpose.ClearDataSource();
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
        txtPurpose.Focus();
        Session["PurposeID"] = null;
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
        int PurposeId;
        string strIDs;

        strIDs = "";

        foreach (Infragistics.Web.UI.GridControls.GridRecord grdViewRow in grdPurpose.Rows)
        {
            chkDelete = (CheckBox)(grdViewRow.Items[3].FindControl("chkDelete"));
            if (chkDelete.Checked == true)
            {
                PurposeId = Convert.ToInt32(((Label)(grdViewRow.Items[3].FindControl("lblDeleteID"))).Text);
                strIDs += Convert.ToString(PurposeId) + ",";
            }
        }

        if (strIDs != "")
        {
            strIDs = strIDs.Remove(strIDs.Length - 1, 1);
        }

        objPurpose = new iAssetTrack.BAL.PurposesBAL();
        objPurpose.PurposeIDs = strIDs;
        objPurpose.Status = 0;
        objPurpose.LastModifiedBy = Convert.ToInt32(Session["UserID"]);
        objPurpose.Persist(DALCOperation.Delete);
        clearFields();
        objCommon = new CommonBAL();
        lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_DELETED);
        lblMessage.Visible = true;
        grdPurpose.ClearDataSource();
        populateGrid();
    }
    /// <summary>
    /// Used to call upon grid page index changes.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void grdPurpose_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //grdPurpose.PageIndex = e.NewPageIndex;
        //populateGrid();
    }
    /// <summary>
    /// Used to call upon grid row edits.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void grdPurpose_RowEditing(object sender, GridViewEditEventArgs e)
    {

        //populateGrid();
        //iAssetTrack.BAL.PurposesBAL objEdit = new iAssetTrack.BAL.PurposesBAL();
        //objEdit.PurposeID = Convert.ToInt32(grdPurpose.DataKeys[Convert.ToInt32(e.NewEditIndex)].Value);
        //DataSet dsPurpose = objEdit.retrieve();
        //DataRow dr = dsPurpose.Tables[0].Rows[0];
        //txtPurpose.Text = dr[DBFields.DBFIELD_PURPOSE].ToString();
        //txtDesc.Text = dr[DBFields.DBFIELD_DESCRIPTION].ToString();
        //Session["PurposeID"] = objEdit.PurposeID;
    }
    #endregion
    #region "User Defined Methods"
    private void populateGrid()
    {
        //objPurpose = new iAssetTrack.BAL.PurposesBAL();
        //DataTable dtPurpose = objPurpose.retrieve().Tables[0];
        //grdPurpose.DataSource = dtPurpose;
        //grdPurpose.DataBind();
        objPurpose = new iAssetTrack.BAL.PurposesBAL();
        DataTable dtGrid = objPurpose.retrieve().Tables[0];
        totalRecordCount = dtGrid.Rows.Count;
        grdPurpose.DataSource = dtGrid;
        grdPurpose.DataBind();

        grdPurpose.Behaviors.Paging.Enabled = false;
        if (totalRecordCount == grdPurpose.Rows.Count)
            this.FilterCount = "";
        grdPurpose.Behaviors.Paging.Enabled = true;

        if (grdPurpose.Rows.Count == 0)
        {
           // grdPurpose.DataSource = null;
            grdPurpose.DataSource = dtGrid;
            grdPurpose.DataBind();
            grdPurpose.Columns[3].Hidden = true;
            ibDelete.Visible = false;
        }
        else if(grdPurpose.Rows.Count > 0)
        {

            if (_dtRights.Select("Rights = 'Delete' and Module = 'Purpose'").Length !=0)
            {
                grdPurpose.Columns[3].Hidden = false;
                ibDelete.Visible = true;
            }
            else
            {
                grdPurpose.Columns[3].Hidden = true;
                ibDelete.Visible = false;
            }
            if (_dtRights.Select("Rights = 'Modify' and Module = 'Purpose'").Length != 0)
            {
                grdPurpose.Columns[2].Hidden = false;
            }
            else
            {
                grdPurpose.Columns[2].Hidden = true;
            }
        }

        if (ibDelete.Visible == true)
        {
            int PurposeId;
            int iCount = 0;

            for (int i = 0; i < grdPurpose.Rows.Count; i++)
            {
                PurposeId = Convert.ToInt32(((Label)(grdPurpose.Rows[i].Items[3].FindControl("lblDeleteID"))).Text);
                grdPurpose.Rows[i].Items[3].FindControl("chkDelete").Visible = true;
                objCommon = new iAssetTrack.BAL.CommonBAL();

                DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_PURPOSEID, PurposeId.ToString(),0);
                foreach (DataTable tblCheck in dsCheck.Tables)
                {
                    if (tblCheck.Rows[0][0].ToString() != "0")
                    {
                        grdPurpose.Rows[i].Items[3].FindControl("chkDelete").Visible = false;
                    }
                }
                if (grdPurpose.Rows[i].Items[3].FindControl("chkDelete").Visible == false)
                {
                    iCount += 1;
                }

            }

            if (iCount == grdPurpose.Rows.Count)
            {
                grdPurpose.Columns[3].Hidden = true;
                ibDelete.Visible = false;
            }
        }
        if (grdPurpose.Rows.Count > 0)
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
    private void clearFields()
    {
        txtPurpose.Text = "";
        txtDesc.Text = "";
        lblMessage.Visible = false;
        lblMessage.Text = "";
    }
    #endregion
    protected void grdPurpose_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            populateGrid();
            iAssetTrack.BAL.PurposesBAL objEdit = new iAssetTrack.BAL.PurposesBAL();
            objEdit.PurposeID = Convert.ToInt32(e.CommandArgument);
            DataSet dsPurpose = objEdit.retrieve();
            DataRow dr = dsPurpose.Tables[0].Rows[0];
            txtPurpose.Text = dr[DBFields.DBFIELD_PURPOSE].ToString();
            txtDesc.Text = dr[DBFields.DBFIELD_DESCRIPTION].ToString();
            Session["PurposeID"] = objEdit.PurposeID;

            if (_dtRights.Select("Module = 'Purpose' and Rights = '" + "Modify" + "'").Length != 0)
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
        this.eExporter.Export(this.grdPurpose, wBook);
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
    protected void grdPurpose_DataFiltered(object sender, Infragistics.Web.UI.GridControls.FilteredEventArgs e)
    {
        // Cast the WebDataGrid DataSource to a DataTable  
        DataTable dt = this.grdPurpose.DataSource as DataTable;

        // Disable WebDataGrid Paging
        grdPurpose.Behaviors.Paging.Enabled = false;
        this.FilterCount = grdPurpose.Rows.Count.ToString();
        if (!string.IsNullOrEmpty(this.FilterCount))
        {
            if (totalRecordCount == int.Parse(this.FilterCount))
            {
                this.FilterCount = "";
            }
        }
        // Enable WebDataGrid Paging
        grdPurpose.Behaviors.Paging.Enabled = true;
    }
    protected void grdPurpose_InitializeRow(object sender, RowEventArgs e)
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