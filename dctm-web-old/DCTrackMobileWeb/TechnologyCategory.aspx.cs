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

public partial class TechnologyCategory : System.Web.UI.Page
{
    #region "Declarations"
    private iAssetTrack.BAL.TechnologyCategoryBAL objTC;
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

    //#region "Page Event Methods"
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        grdTechnologyCategory.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(grdTechnologyCategory_ItemCommand);
        pagerControl = grdTechnologyCategory.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);
    }
    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdTechnologyCategory.Behaviors.Paging.PageIndex = e.PageNumber;
        populateGrid();
    }
    protected void grdTechnologyCategory_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControl.SetupPageList(this.grdTechnologyCategory.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdTechnologyCategory.Behaviors.Paging.PageIndex);
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
            pagerControl.SetupPageList(this.grdTechnologyCategory.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdTechnologyCategory.Behaviors.Paging.PageIndex);
        }

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        populateGrid();
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        Session["PageHeader"] = "Technology Category";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        lblMessage.Visible = false;
        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "TechnologyCategory.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        if (_dtRights.Select("Module = 'Technology Category' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }

        if (_dtRights.Select("Module = 'Technology Category' and Rights = '" + "Create" + "'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }

        if (_dtRights.Select("Module = 'Technology Category' and Rights = '" + "Delete" + "'").Length != 0)
        {
            ibDelete.Visible = true;
        }
        else
        {
            ibDelete.Visible = false;
        }

        this.grdTechnologyCategory.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());
        populateGrid();
        if (!IsPostBack)
        {
            // populateGrid();
            Session["TechID"] = null;
            //populateGrid();
        }
    }
    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        if (ibDelete.Enabled)
        {
            iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();
            string sMessage = objCommon.displayMessage(MessageCodes.TC_JS_DELETE);
            this.hdnMessage.Value = sMessage;
        }

        string errMsg = GetLocalResourceObject("revDesc1Resource1.ErrorMessage").ToString();
        cvDesc.ErrorMessage = errMsg.Replace("{MAXLENGTH}", txtDesc.MaxLength.ToString());
    }
    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        objTC = new iAssetTrack.BAL.TechnologyCategoryBAL();
        objTC.TechName = txtTechName.Text.Trim();

        objTC.Description = txtDesc.Text.Trim();
        objTC.Status = 1;
        objTC.CreatedBy = Convert.ToInt32(Session["UserID"]);

        int intTC = 0;
        objTC.TechID = Session["TechID"] == null ? intTC : (int)Session["TechID"];
        intTC = objTC.exists();

        if (intTC != -1 && intTC != 0)
            objTC.TechID = intTC;

        if (intTC != -1)
        {
            objTC.Persist(DALCOperation.Insert);
            clearFields();
            if (Session["TechID"] == null)
            {
                objCommon = new CommonBAL();
                lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_INSERTED);
            }
            else
            {
                objCommon = new CommonBAL();
                lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_UPDATED);
            }

            Session["TechID"] = null;
            lblMessage.Visible = true;
            grdTechnologyCategory.ClearDataSource();
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
    protected void ibReset_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        clearFields();
        txtTechName.Focus();
        Session["TechID"] = null;
       
        populateGrid();
    }
    protected void ibDelete_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        CheckBox chkDelete;
        int TechID;
        string strIDs;

        strIDs = "";

        foreach (Infragistics.Web.UI.GridControls.GridRecord grdViewRow in grdTechnologyCategory.Rows)
        {
            chkDelete = (CheckBox)(grdViewRow.Items[3].FindControl("chkDelete"));
            if (chkDelete.Checked == true)
            {
                TechID = Convert.ToInt32(((Label)(grdViewRow.Items[3].FindControl("lblDeleteID"))).Text);
                strIDs += Convert.ToString(TechID) + ",";
            }
        }

        if (strIDs != "")
        {
            strIDs = strIDs.Remove(strIDs.Length - 1, 1);
        }

        objTC = new iAssetTrack.BAL.TechnologyCategoryBAL();
        objTC.TechIDs = strIDs;
        objTC.Status = 0;
        objTC.LastModifiedBy = Convert.ToInt32(Session["UserID"]);
        objTC.Persist(DALCOperation.Delete);
        clearFields();
        objCommon = new CommonBAL();
        lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_DELETED);
        lblMessage.Visible = true;
        grdTechnologyCategory.ClearDataSource();
        populateGrid();
    }

    private void populateGrid()
    {
        objTC = new iAssetTrack.BAL.TechnologyCategoryBAL();
        DataTable dtGrid = objTC.retrieve().Tables[0];
        totalRecordCount = dtGrid.Rows.Count;
        //grdTechnologyCategory.DataSource = dtGrid;
        //grdTechnologyCategory.DataBind();

        grdTechnologyCategory.Behaviors.Paging.Enabled = false;
        if (totalRecordCount == grdTechnologyCategory.Rows.Count)
            this.FilterCount = "";
        grdTechnologyCategory.Behaviors.Paging.Enabled = true;


        if (dtGrid.Rows.Count == 0)
        {
            //grdTechnologyCategory.DataSource = null;
            grdTechnologyCategory.DataSource = dtGrid;
            grdTechnologyCategory.DataBind();
            grdTechnologyCategory.Columns[3].Hidden = true;
            ibDelete.Visible = false;
        }
        else
        {
            grdTechnologyCategory.DataSource = dtGrid;
            grdTechnologyCategory.DataBind();
            if (_dtRights.Select("Rights = 'Delete' and Module = 'Technology Category'").Length != 0)
            {
                grdTechnologyCategory.Columns[3].Hidden = false;
                ibDelete.Visible = true;
            }
            else
            {
                grdTechnologyCategory.Columns[3].Hidden = true;
                ibDelete.Visible = false;
            }
            if (_dtRights.Select("Rights = 'Modify' and Module = 'Technology Category'").Length != 0)
            {
                grdTechnologyCategory.Columns[2].Hidden = false;
            }
            else
            {
                grdTechnologyCategory.Columns[2].Hidden = true;
            }


            if (ibDelete.Visible == true)
            {
                int TechId;
                int iCount = 0;

                for (int i = 0; i < grdTechnologyCategory.Rows.Count; i++)
                {
                    TechId = Convert.ToInt32(((Label)(grdTechnologyCategory.Rows[i].Items[3].FindControl("lblDeleteID"))).Text);
                    grdTechnologyCategory.Rows[i].Items[3].FindControl("chkDelete").Visible = true;
                    objCommon = new iAssetTrack.BAL.CommonBAL();

                    DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_TECHID, TechId.ToString(), 0);
                    foreach (DataTable tblCheck in dsCheck.Tables)
                    {
                        if (tblCheck.Rows[0][0].ToString() != "0")
                        {
                            grdTechnologyCategory.Rows[i].Items[3].FindControl("chkDelete").Visible = false;
                        }
                    }
                    if (grdTechnologyCategory.Rows[i].Items[3].FindControl("chkDelete").Visible == false)
                    {
                        iCount += 1;
                    }

                }

                if (iCount == grdTechnologyCategory.Rows.Count)
                {
                    grdTechnologyCategory.Columns[3].Hidden = true;
                    ibDelete.Visible = false;
                }
            }
            if (grdTechnologyCategory.Rows.Count > 0)
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
        txtTechName.Text = "";
        txtDesc.Text = "";
        lblMessage.Visible = false;
        lblMessage.Text = "";
    }
    protected void grdTechnologyCategory_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            populateGrid();
            iAssetTrack.BAL.TechnologyCategoryBAL objEdit = new iAssetTrack.BAL.TechnologyCategoryBAL();
            objEdit.TechID = Convert.ToInt32(e.CommandArgument);
            DataSet dsTC = objEdit.retrieve();
            DataRow dr = dsTC.Tables[0].Rows[0];
            txtTechName.Text = dr[DBFields.DBFIELD_TECHNAME].ToString();
            txtDesc.Text = dr[DBFields.DBFIELD_TECHID_DESCRIPTION].ToString();
            Session["TechID"] = objEdit.TechID;

            if (_dtRights.Select("Module = 'Technology Category' and Rights = '" + "Modify" + "'").Length != 0)
            {
                ibCreate.Enabled = true;
            }
            else
            {
                ibCreate.Enabled = false;
            }
        }
    }
    protected void eExporter_CellExported(object sender, ExcelCellExportedEventArgs e)
    {
        int iWSdex = e.Worksheet.Index;
        int iRdex = e.CurrentRowIndex;
        int iCdex = e.CurrentColumnIndex;
        e.Worksheet.Columns[2].Width = 1;
        e.Worksheet.Columns[3].Width = 1;
        if (iWSdex == 0)
        {
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
    }

    protected void ibExportToExcel_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        this.eExporter.DataExportMode = DataExportMode.AllDataInDataSource;
        Infragistics.Documents.Excel.Workbook wBook = new Infragistics.Documents.Excel.Workbook(excelFormat);
        this.eExporter.Export(this.grdTechnologyCategory, wBook);
    }
    protected void grdTechnologyCategory_InitializeRow(object sender, RowEventArgs e)
    {
    }
    protected void grdTechnologyCategory_DataFiltered(object sender, Infragistics.Web.UI.GridControls.FilteredEventArgs e)
    {
        // Cast the WebDataGrid DataSource to a DataTable  
        DataTable dt = this.grdTechnologyCategory.DataSource as DataTable;

        // Disable WebDataGrid Paging
        grdTechnologyCategory.Behaviors.Paging.Enabled = false;

        this.FilterCount = grdTechnologyCategory.Rows.Count.ToString();
        if (!string.IsNullOrEmpty(this.FilterCount))
        {
            if (totalRecordCount == int.Parse(this.FilterCount))
            {
                this.FilterCount = "";
            }
        }
        // Enable WebDataGrid Paging
        grdTechnologyCategory.Behaviors.Paging.Enabled = true;
    }
}
