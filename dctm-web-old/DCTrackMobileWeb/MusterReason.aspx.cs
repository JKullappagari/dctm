/*
File Name   :	MusterReason.aspx.cs

Description :	Used to create Muster Reason

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
using System.Data.SqlClient;
using iAssetTrackBAL;
using Infragistics.Web.UI.GridControls;
using iAssetTrack.DALC; 

public partial class MusterReason : System.Web.UI.Page
{
    #region "Declarations"
    private iAssetTrack.BAL.MusterReasonBAL objMusterReason;
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

    #region " Page Event Methods "
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        grdMusteringReason.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(grdMusteringReason_ItemCommand);
        pagerControl = grdMusteringReason.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);
    }
    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdMusteringReason.Behaviors.Paging.PageIndex = e.PageNumber;
        populateGrid();
    }
    protected void grdMusteringReason_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControl.SetupPageList(this.grdMusteringReason.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdMusteringReason.Behaviors.Paging.PageIndex);
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
            pagerControl.SetupPageList(this.grdMusteringReason.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdMusteringReason.Behaviors.Paging.PageIndex);
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
        Session["PageHeader"] = "Asset Status Change - Reason (applicable to Decommission, Recommission and Write-Off)";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        lblMessage.Visible = false;


        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "MusterReason.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        if (_dtRights.Select("Module = 'Reason'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }
        if (_dtRights.Select("Module = 'Reason' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }


        if (_dtRights.Select("Module = 'Reason' and Rights = '" + "Create" + "'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }

        if (_dtRights.Select("Module = 'Reason' and Rights = '" + "Delete" + "'").Length != 0)
        {
            ibDelete.Visible = true;
        }
        else
        {
            ibDelete.Visible = false;
        }

        this.grdMusteringReason.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());
        populateGrid();
        if (!IsPostBack)
        {
            Session["MusterReasonID"] = null;

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
            string sMessage = objCommon.displayMessage(MessageCodes.DREASON_JS_DELETE);
            hdnMessage.Value = sMessage;
        }
        string errMsg = GetLocalResourceObject("revDesc1Resource1.ErrorMessage").ToString();
        cvDesc.ErrorMessage = errMsg.Replace("{MAXLENGTH}", txtDesc.MaxLength.ToString());
    }

    /// <summary>
    /// Used to call upon grid page index changes.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void grdMusteringReason_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //grdMusteringReason.PageIndex = e.NewPageIndex;
        //populateGrid();
    }

    /// <summary>
    /// Used to call upon grid row edits.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void grdMusteringReason_RowEditing(object sender, GridViewEditEventArgs e)
    {
        //populateGrid();
        //objMusterReason = new iAssetTrack.BAL.MusterReasonBAL();
        //objMusterReason.MusterReasonID = Convert.ToInt32(grdMusteringReason.DataKeys[Convert.ToInt32(e.NewEditIndex)].Value);
        //DataSet dsMusterReason = objMusterReason.retrieve();
        //DataRow dr = dsMusterReason.Tables[0].Rows[0];
        //txtMusterReason.Text = dr[DBFields.DBFIELD_MUSTERREASON].ToString();
        //txtDesc.Text = dr[DBFields.DBFIELD_DESCRIPTION].ToString();
        //Session["MusterReasonID"] = objMusterReason.MusterReasonID;
    }
    protected void grdMusteringReason_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            populateGrid();
            objMusterReason = new iAssetTrack.BAL.MusterReasonBAL();
            objMusterReason.MusterReasonID = Convert.ToInt32(e.CommandArgument);
            DataSet dsMusterReason = objMusterReason.retrieve();
            DataRow dr = dsMusterReason.Tables[0].Rows[0];
            txtMusterReason.Text = dr[DBFields.DBFIELD_MUSTERREASON].ToString();
            txtDesc.Text = dr[DBFields.DBFIELD_DESCRIPTION].ToString();
            Session["MusterReasonID"] = objMusterReason.MusterReasonID;

            if (_dtRights.Select("Module = 'Reason' and Rights = '" + "Modify" + "'").Length != 0)
            {
                ibCreate.Enabled = true;
            }
            else
            {
                ibCreate.Enabled = false;
            }
        }

    }


    /// <summary>
    /// Used to save information related BU.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        objMusterReason = new iAssetTrack.BAL.MusterReasonBAL();
        objMusterReason.MusterReason = txtMusterReason.Text.Trim();
        //if (Session["BusinessUnitID"] != null)
        //{
        //    objBU.BusinessUnitID = (int)Session["BusinessUnitID"];
        //}
        //else
        //{
        //    objBU.BusinessUnitID = objBU.exists();
        //}

        objMusterReason.Description = txtDesc.Text.Trim();
        objMusterReason.Status = 1;
        objMusterReason.CreatedBy = Convert.ToInt32(Session["UserID"]);

        int intMusterReason = 0;
        objMusterReason.MusterReasonID = Session["MusterReasonID"] == null ? intMusterReason : (int)Session["MusterReasonID"];
        intMusterReason = objMusterReason.exists();

        if (intMusterReason != -1 && intMusterReason != 0)
            objMusterReason.MusterReasonID = intMusterReason;

        if (intMusterReason != -1)
        {
            objMusterReason.Persist(DALCOperation.Insert);
            clearFields();
            if (Session["MusterReasonID"] == null)
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
            grdMusteringReason.ClearDataSource();
            populateGrid();
            Session["MusterReasonID"] = null;
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
        Session["MusterReasonID"] = null;
        txtMusterReason.Focus();
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
        int MusterReasonId;
        string strIDs;

        strIDs = "";
        foreach (Infragistics.Web.UI.GridControls.GridRecord grdViewRow in grdMusteringReason.Rows)
        {
            chkDelete = (CheckBox)(grdViewRow.Items[3].FindControl("chkDelete"));
            if (chkDelete.Checked == true)
            {
                MusterReasonId = Convert.ToInt32(((Label)(grdViewRow.Items[3].FindControl("lblDeleteID"))).Text);
                strIDs += Convert.ToString(MusterReasonId) + ",";
            }
        }

        if (strIDs != "")
        {
            strIDs = strIDs.Remove(strIDs.Length - 1, 1);
        }

        objMusterReason = new iAssetTrack.BAL.MusterReasonBAL();
        objMusterReason.MusterReasonIDs = strIDs;
        objMusterReason.Status = 0;
        objMusterReason.LastModifiedBy = Convert.ToInt32(Session["UserID"]);
        objMusterReason.Persist(DALCOperation.Delete);

        clearFields();

        objCommon = new CommonBAL();
        lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_DELETED);
        lblMessage.Visible = true;
        grdMusteringReason.ClearDataSource();
        populateGrid();
    }
    #endregion

    #region "User Defined Methods"
    /// <summary>
    /// Populate Business Unit data grid
    /// </summary>    
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    private void populateGrid()
    {
        ////objMusterReason = new iAssetTrack.BAL.MusterReasonBAL();

        ////grdMusteringReason.DataSource = objMusterReason.retrieve().Tables[0];
        ////grdMusteringReason.DataBind();
        objMusterReason = new iAssetTrack.BAL.MusterReasonBAL();
        DataTable dtGrid = objMusterReason.retrieve().Tables[0];
        totalRecordCount = dtGrid.Rows.Count;
        grdMusteringReason.DataSource = dtGrid;
        grdMusteringReason.DataBind();

        grdMusteringReason.Behaviors.Paging.Enabled = false;
        if (totalRecordCount == grdMusteringReason.Rows.Count)
            this.FilterCount = "";
        grdMusteringReason.Behaviors.Paging.Enabled = true;

        if (grdMusteringReason.Rows.Count == 0)
        {
            //grdMusteringReason.DataSource = null;
            grdMusteringReason.DataSource = dtGrid;
            grdMusteringReason.DataBind();
            grdMusteringReason.Columns[3].Hidden = true;
            ibDelete.Visible = false;
        }
        else //if(grdMusteringReason.Rows.Count > 0)
        {


            if (_dtRights.Select("Rights = 'Delete' and Module = 'Reason'").Length != 0)
            {
                grdMusteringReason.Columns[3].Hidden = false;
                ibDelete.Visible = true;
            }
            else
            {
                grdMusteringReason.Columns[3].Hidden = true;
                ibDelete.Visible = false;
            }

            if (_dtRights.Select("Rights = 'Modify' and Module = 'Reason'").Length != 0)
            {
                grdMusteringReason.Columns[2].Hidden = false;

            }
            else
            {
                grdMusteringReason.Columns[2].Hidden = true;
            }
        }

        if (ibDelete.Visible == true)
        {
            int MusterReasonId;
            int iCount = 0;

            for (int i = 0; i < grdMusteringReason.Rows.Count; i++)
            {
                MusterReasonId = Convert.ToInt32(((Label)(grdMusteringReason.Rows[i].Items[3].FindControl("lblDeleteID"))).Text);
                grdMusteringReason.Rows[i].Items[3].FindControl("chkDelete").Visible = true;
                objCommon = new iAssetTrack.BAL.CommonBAL();

                DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_MUSTERREASONID, MusterReasonId.ToString(),0);
                foreach (DataTable tblCheck in dsCheck.Tables)
                {
                    if (tblCheck.Rows[0][0].ToString() != "0")
                    {
                        grdMusteringReason.Rows[i].Items[3].FindControl("chkDelete").Visible = false;
                    }
                }
                if (grdMusteringReason.Rows[i].Items[3].FindControl("chkDelete").Visible == false)
                {
                    iCount += 1;
                }

            }
            if (iCount == grdMusteringReason.Rows.Count)
            {
                grdMusteringReason.Columns[3].Hidden = true;
                ibDelete.Visible = false;
            }
        }
        if (grdMusteringReason.Rows.Count > 0)
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

    /// <summary>
    /// Reset fields
    /// </summary>    
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    private void clearFields()
    {
        txtMusterReason.Text = "";
        txtDesc.Text = "";
        lblMessage.Visible = false;
        lblMessage.Text = "";
    }
    #endregion


    protected void eExporter_CellExported(object sender, Infragistics.Web.UI.GridControls.ExcelCellExportedEventArgs e)
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
    protected void ibExportToExcel_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        this.eExporter.DataExportMode = DataExportMode.AllDataInDataSource;
        Infragistics.Documents.Excel.Workbook wBook = new Infragistics.Documents.Excel.Workbook(excelFormat);
        this.eExporter.Export(this.grdMusteringReason, wBook);
    }
    protected void grdMusteringReason_DataFiltered(object sender, Infragistics.Web.UI.GridControls.FilteredEventArgs e)
    {
        // Cast the WebDataGrid DataSource to a DataTable  
        DataTable dt = this.grdMusteringReason.DataSource as DataTable;

        // Disable WebDataGrid Paging
        grdMusteringReason.Behaviors.Paging.Enabled = false;
        this.FilterCount = grdMusteringReason.Rows.Count.ToString();
        if (!string.IsNullOrEmpty(this.FilterCount))
        {
            if (totalRecordCount == int.Parse(this.FilterCount))
            {
                this.FilterCount = "";
            }
        }
        // Enable WebDataGrid Paging
        grdMusteringReason.Behaviors.Paging.Enabled = true;
    }

    protected void grdMusteringReason_InitializeRow(object sender, RowEventArgs e)
    {
        //if (ibDelete.Visible == true)
        //{
        //    int MusterReasonId;
        //    int iCount = 0;

        //    //for (int i = 0; i < grdMusteringReason.Rows.Count; i++)
        //    //{
        //        MusterReasonId = Convert.ToInt32(((Label)(e.Row.Items[3].FindControl("lblDeleteID"))).Text);

        //        objCommon = new iAssetTrack.BAL.CommonBAL();

        //        DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_MUSTERREASONID, MusterReasonId);
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
        //    //if (iCount == grdMusteringReason.Rows.Count)
        //    //{
        //    //    grdMusteringReason.Columns[3].Hidden = true;
        //    //    ibDelete.Visible = false;
        //    //}
        //}
    }
}
