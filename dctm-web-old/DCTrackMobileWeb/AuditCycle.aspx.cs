
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

using iAssetTrack.DALC;
using System.Data.SqlClient;
using iAssetTrackBAL;
using iAssetTrack.BAL;

public partial class AuditCycle : System.Web.UI.Page
{
    #region "Declarations"
    private iAssetTrack.BAL.AuditCycleBAL objAC;
    private iAssetTrack.BAL.BusinessUnitBAL objBU;
    private iAssetTrack.BAL.SitesBAL objSite;
    private iAssetTrack.BAL.SiteLocationAssignmentBAL objSiteLocation;
    // private iAssetTrack.BAL.AuditCycleBAL objAC;
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
        grdAuditCycle.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(grdAuditCycle_ItemCommand);
        pagerControl = grdAuditCycle.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        if (pagerControl != null)
            pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);
    }
    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdAuditCycle.Behaviors.Paging.PageIndex = e.PageNumber;
        populateGrid();
    }
    protected void grdAuditCycle_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControl.SetupPageList(this.grdAuditCycle.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdAuditCycle.Behaviors.Paging.PageIndex);
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
            pagerControl.SetupPageList(this.grdAuditCycle.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdAuditCycle.Behaviors.Paging.PageIndex);
        }


    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        populateGrid();
    }
    protected void Page_Load(object sender, EventArgs e)
    {

       
        //enable drop down list
        //ddlBU.Enabled = true;
        ddlSite.Enabled = true;
        ddlRoom.Enabled = true;

        CompareValidator2.ValueToCompare = DateTime.Now.Date.ToShortDateString();
        Session["PageHeader"] = "AuditCycle";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        lblMessage.Visible = false;

        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "AuditCycle.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = _dtRights != null && _dtRights.Select("Module = 'Audit Cycle' and Rights = '" + "View" + "'").Length != 0;

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }

        if (_dtRights.Select("Module = 'Audit Cycle' and Rights = '" + "Create" + "'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }

        if (_dtRights.Select("Module = 'Audit Cycle' and Rights = '" + "Delete" + "'").Length != 0)
        {
            ibDelete.Visible = true;
        }
        else
        {
            ibDelete.Visible = false;
        }

        this.grdAuditCycle.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());

        populateGrid();

        if (!IsPostBack)
        {
            wdcStartDate.MinValue = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            wdcStartDate.MaxValue = Convert.ToDateTime(DateTime.Now.AddYears(1).ToString("yyyy-MM-dd"));
            wdcEndDate.MaxValue = Convert.ToDateTime(DateTime.Now.AddYears(1).ToString("yyyy-MM-dd"));

            Session["AuditCycleID"] = null;
            objBU = new iAssetTrack.BAL.BusinessUnitBAL();
            DataSet dsBU = objBU.retrieve();
            DataTable dtBU = dsBU.Tables[0];

            objCommon = new CommonBAL();
            objCommon.setDataSource(ddlBU, dtBU, "-Select-");
            ddlBU.SelectedIndex = 1;
            ddlBU.Enabled = false;
            populateSite();


        }

    }
    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        if (ibDelete.Enabled)
        {
            iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();
            string sMessage = objCommon.displayMessage(MessageCodes.AC_JS_DELETE);
            hdnMessage.Value = sMessage;
        }
        //txtDesc.Attributes.Add("onkeypress", "doKeypress(this," + txtDesc.MaxLength.ToString() + ");");
        //txtDesc.Attributes.Add("onbeforepaste", "doBeforePaste(this," + txtDesc.MaxLength.ToString() + ");");
        //txtDesc.Attributes.Add("onpaste", "doPaste(this," + txtDesc.MaxLength.ToString() + ");");
    }
    protected void grdAuditCycle_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //grdBU.Pag = e.NewPageIndex;
        populateGrid();
    }
    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        objAC = new iAssetTrack.BAL.AuditCycleBAL();
        // objAC.AuditCycleCount = txtBU.Text;


        objAC.LocationID = Convert.ToInt32(ddlRoom.SelectedValue);

        //Start Date
        int stYear = Convert.ToDateTime(this.wdcStartDate.Text).Year;
        int stMonth = Convert.ToDateTime(this.wdcStartDate.Text).Month;
        int stDay = Convert.ToDateTime(this.wdcStartDate.Text).Day;
        int hour = DateTime.Now.Hour;
        int mins = DateTime.Now.Minute;
        int secs = DateTime.Now.Second;

        DateTime dtStartDate = new DateTime(stYear, stMonth, stDay, hour, mins, secs);
        objAC.StartDate = dtStartDate;

        //End Date
        int enYear = Convert.ToDateTime(this.wdcEndDate.Text).Year;
        int enMonth = Convert.ToDateTime(this.wdcEndDate.Text).Month;
        int enDay = Convert.ToDateTime(this.wdcEndDate.Text).Day;

        objAC.EndDate = new DateTime(enYear, enMonth, enDay, 23, 59, 59); // modified by kjb on 27 Feb 2013
        // to specify end date with 23:59:59 as time

        //objAC.StartDate = DateTime(wdcStartDate.Value.ToString()); 
        objAC.CreatedBy = Convert.ToInt32(Session["UserID"]);
        //objAC.EndDate = wdcEndDate.Value;

        int intAC = 0;
        objAC.ID = Session["AuditCycleID"] == null ? intAC : (int)Session["AuditCycleID"];
        intAC = objAC.exists();

        if (intAC != -1 && intAC != 0)
            objAC.ID = intAC;

        if (intAC != -1)
        {
            objAC.Persist(DALCOperation.Insert);
            clearFields();
            if (Session["AuditCycleID"] == null)
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
            grdAuditCycle.ClearDataSource();
            populateGrid();
            Session["AuditCycleID"] = null;
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
        Session["AuditCycleID"] = null;

        populateGrid();
    }
    protected void ibDelete_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        CheckBox chkDelete;
        int ACId;
        string strIDs;

        strIDs = "";
        foreach (Infragistics.Web.UI.GridControls.GridRecord grdViewRow in grdAuditCycle.Rows)
        {
            chkDelete = (CheckBox)(grdViewRow.Items[6].FindControl("chkDelete"));
            if (chkDelete.Checked == true)
            {
                ACId = Convert.ToInt32(((Label)(grdViewRow.Items[6].FindControl("lblDeleteID"))).Text);
                strIDs += Convert.ToString(ACId) + ",";
            }
        }

        if (strIDs != "")
        {
            strIDs = strIDs.Remove(strIDs.Length - 1, 1);
        }

        objAC = new iAssetTrack.BAL.AuditCycleBAL();
        objAC.AuditCycleIDs = strIDs;
        // objAC.Status = 0;
        //objAC.LastModifiedBy = Convert.ToInt32(Session["UserID"]);
        objAC.Persist(DALCOperation.Delete);

        clearFields();

        objCommon = new CommonBAL();
        lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_DELETED);
        lblMessage.Visible = true;
        grdAuditCycle.ClearDataSource();
        populateGrid();
    }
    #endregion
    private void populateGrid()
    {
        //objAC = new iAssetTrack.BAL.AuditCycleBAL();
        //grdAuditCycle.DataSource = objAC.retrieve().Tables[0];
        //grdAuditCycle.DataBind();

        objAC = new iAssetTrack.BAL.AuditCycleBAL();
        DataTable dtGrid = objAC.retrieve().Tables[0];
        totalRecordCount = dtGrid.Rows.Count;
        grdAuditCycle.DataSource = dtGrid;
        grdAuditCycle.DataBind();

        grdAuditCycle.Behaviors.Paging.Enabled = false;
        if (totalRecordCount == grdAuditCycle.Rows.Count)
            this.FilterCount = "";
        grdAuditCycle.Behaviors.Paging.Enabled = true;

        if (grdAuditCycle.Rows.Count == 0)
        {
            //grdAuditCycle.DataSource = null;
            grdAuditCycle.DataSource = dtGrid;
            grdAuditCycle.DataBind();
            grdAuditCycle.Columns[6].Hidden = true;
            ibDelete.Visible = false;
        }
        else if (grdAuditCycle.Rows.Count > 0)
        {
            if (_dtRights.Select("Rights = 'Delete' and Module = 'Audit Cycle'").Length != 0)
            {
                grdAuditCycle.Columns[6].Hidden = false;
                ibDelete.Visible = true;
            }
            else
            {
                grdAuditCycle.Columns[6].Hidden = true;
                ibDelete.Visible = false;
            }

            if (_dtRights.Select("Rights = 'Modify' and Module = 'Audit Cycle'").Length != 0)
            {
                grdAuditCycle.Columns[5].Hidden = false;
            }
            else
            {
                grdAuditCycle.Columns[5].Hidden = true;
            }
        }

        if (ibDelete.Visible == true)
        {
            int ACId;
            int iCount = 0;

            for (int i = 0; i < grdAuditCycle.Rows.Count; i++)
            {
                ACId = Convert.ToInt32(((Label)(grdAuditCycle.Rows[i].Items[6].FindControl("lblDeleteID"))).Text);
                grdAuditCycle.Rows[i].Items[6].FindControl("chkDelete").Visible = true;
                objCommon = new iAssetTrack.BAL.CommonBAL();

                DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_AUDITCYCLE_ID, ACId.ToString(), 0);
                foreach (DataTable tblCheck in dsCheck.Tables)
                {
                    if (tblCheck.Rows[0][0].ToString() != "0")
                    {
                        grdAuditCycle.Rows[i].Items[6].FindControl("chkDelete").Visible = false;
                    }
                }
                if (grdAuditCycle.Rows[i].Items[6].FindControl("chkDelete").Visible == false)
                {
                    iCount += 1;
                }

            }
            if (iCount == grdAuditCycle.Rows.Count)
            {
                grdAuditCycle.Columns[6].Hidden = true;
                ibDelete.Visible = false;
            }
        }
        if (grdAuditCycle.Rows.Count > 0)
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
        //ddlBU.SelectedIndex = 0;
        ddlRoom.SelectedIndex = 0;
        ddlSite.SelectedIndex = 0;
        lblMessage.Visible = false;
        wdcStartDate.Value = "";
        wdcEndDate.Value = "";
        lblMessage.Text = "";
    }

    protected void grdAuditCycle_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e)
    {
        //if (e.CommandName == "Edit")
        //{
        //    populateGrid();
        //    DataSet dsBU = objAC.retrieve();
        //    DataRow dr = dsBU.Tables[0].Rows[0];
        //    //ddlBU.SelectedValue = dr[DBFields.DBFIELD_BU].ToString();
        //    //ddlSite.SelectedValue = dr[DBFields.DBFIELD_Sites].ToString();
        //    //ddlRoom.SelectedValue = dr[DBFields.DBFIELD_Locatioon].ToString();
        //    wdcStartDate.Value = dr[DBFields.DBFIELD_STARTDATE].ToString();
        //    wdcEndDate.Value = dr[DBFields.DBFIELD_ENDDATE].ToString();
        //    //txtBU.Text = dr[DBFields.DBFIELD_BUSINESSUNIT].ToString();
        //    //txtDesc.Text = dr[DBFields.DBFIELD_DESCRIPTION].ToString();
        //    //txtCoPrefix.Text = dr[DBFields.DBFIELD_COPREFIX].ToString();
        //    Session["AuditCycleID"] = objAC.ID;
        //    objAC = new iAssetTrack.BAL.AuditCycleBAL();
        //    objAC.ID = Convert.ToInt32(e.CommandArgument);

        //}
        if (e.CommandName == "Edit")
        {
            populateGrid();
            // iAssetTrack.BAL.SitesBAL objEdit = new iAssetTrack.BAL.SitesBAL();
            objAC.ID = Convert.ToInt32(e.CommandArgument);
            DataSet dsBU = objAC.retrieve();
            DataRow dr = dsBU.Tables[0].Rows[0];
            ddlBU.SelectedValue = dr[DBFields.DBFIELD_BU].ToString();
            populateSite();
            //ddlSite.Items.Add(dr[DBFields.DBFIELD_Sites].ToString());
            // ddlSite.Items.Add("0", new ListItem(0, dr[DBFields.DBFIELD_Sites].ToString()));
            //lblSites.Text = dr[DBFields.DBFIELD_Sites].ToString();
            //lblRooms.Text = dr[DBFields.DBFIELD_Locatioon].ToString();
            // string v = "29";
            ddlSite.SelectedValue = dr[DBFields.DBFIELD_SitesID].ToString(); ;
            //ddlSite.SelectedValue = dr[DBFields.DBFIELD_Sites].ToString().Trim();DBFIELD_SitesID
            // ddlRoom.SelectedValue = dr[DBFields.DBFIELD_Locatioon].ToString();
            populate();
            // string v1 = "1906";
            ddlRoom.SelectedValue = dr[DBFields.DBFIELD_LocatioonID].ToString();
            // ddlRoom.SelectedValue = dr[DBFields.DBFIELD_Locatioon].ToString();DBFIELD_LocatioonID
            wdcStartDate.Value = dr[DBFields.DBFIELD_STARTDATE].ToString();
            wdcEndDate.Value = dr[DBFields.DBFIELD_ENDDATE].ToString();
            Session["AuditCycleID"] = objAC.ID;
            //disable drop down list
            ddlBU.Enabled = false;
            ddlSite.Enabled = false;
            ddlRoom.Enabled = false;
            CompareValidator2.Enabled = false;

            if (_dtRights.Select("Module = 'Audit Cycle' and Rights = '" + "Modify" + "'").Length != 0)
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
            pagerControl.SetCurrentPageNumber(grdAuditCycle.Behaviors.Paging.PageIndex);
        }
    }
    protected void ibExportToExcel_Click(object sender, EventArgs e)
    {
        Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        this.eExporter.DataExportMode = Infragistics.Web.UI.GridControls.DataExportMode.AllDataInDataSource;
        Infragistics.Documents.Excel.Workbook wBook = new Infragistics.Documents.Excel.Workbook(excelFormat);
        this.eExporter.Export(this.grdAuditCycle, wBook);
    }
    protected void eExporter_CellExported(object sender, Infragistics.Web.UI.GridControls.ExcelCellExportedEventArgs e)
    {
        int iWSdex = e.Worksheet.Index;
        int iRdex = e.CurrentRowIndex;
        int iCdex = e.CurrentColumnIndex;
        e.Worksheet.Columns[5].Width = 1;
        e.Worksheet.Columns[6].Width = 1;
        if (iWSdex == 0)
        {
            if (iRdex == 0)
            {

                if (iCdex == 5 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
                {
                    e.Worksheet.Rows[iRdex].Cells[iCdex].Value = "";


                }
                if (iCdex == 6 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
                {
                    e.Worksheet.Rows[iRdex].Cells[iCdex].Value = "";


                }

            }

        }
    }
    protected void grdAuditCycle_InitializeRow(object sender, Infragistics.Web.UI.GridControls.RowEventArgs e)
    {


    }
    protected void ddlBU_SelectedIndexChanged(object sender, EventArgs e)
    {
        populateSite();
    }
    //protected void ddlSite_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    populate();
    //}
    private void populateSite()
    {
        if (Convert.ToInt32(ddlBU.SelectedValue) != 0)
        {
            objSite = new iAssetTrack.BAL.SitesBAL();
            DataSet dsSite = objSite.retrieveByBusinessUnitId(Convert.ToInt32(ddlBU.SelectedValue));
            DataTable dtSite = dsSite.Tables[0];
            objCommon = new CommonBAL();
            objCommon.setDataSource(ddlSite, dtSite, "-Select-");
        }
        else
        {
            objSite = new iAssetTrack.BAL.SitesBAL();
            DataSet dsSite = objSite.retrieveByBusinessUnitId(-1);
            DataTable dtSite = dsSite.Tables[0];
            objCommon = new CommonBAL();
            objCommon.setDataSource(ddlSite, dtSite, "-Select-");
        }


    }
    protected void ddlSite_SelectedIndexChanged(object sender, EventArgs e)
    {
        populate();
    }
    protected void grdAuditCycle_DataFiltered(object sender, Infragistics.Web.UI.GridControls.FilteredEventArgs e)
    {
        // Cast the WebDataGrid DataSource to a DataTable  
        DataTable dt = this.grdAuditCycle.DataSource as DataTable;
        // Disable WebDataGrid Paging
        grdAuditCycle.Behaviors.Paging.Enabled = false;
        this.FilterCount = grdAuditCycle.Rows.Count.ToString();
        if (!string.IsNullOrEmpty(this.FilterCount))
        {
            if (totalRecordCount == int.Parse(this.FilterCount))
            {
                this.FilterCount = "";
            }
        }
        // Enable WebDataGrid Paging
        grdAuditCycle.Behaviors.Paging.Enabled = true;
    }
    private void populate()
    {
        objSiteLocation = new iAssetTrack.BAL.SiteLocationAssignmentBAL();

        objSiteLocation.BusinessUnitID = Convert.ToInt32(ddlBU.SelectedValue);
        //populateSite();

        //Commented - 10-Nov-2006  --Dummy value passed for Parameter
        //objSiteLocation.SiteAccessID = Convert.ToInt32(ddlSiteAccess.SelectedValue);
        objSiteLocation.SiteID = Convert.ToInt32(ddlSite.SelectedValue);

        //DataSet dsAvSite = objSiteLocation.retrieveAvailLocations();
        //lbAvLocation.DataTextField = dsAvSite.Tables[0].Columns[1].ColumnName;
        //lbAvLocation.DataValueField = dsAvSite.Tables[0].Columns[0].ColumnName;
        //lbAvLocation.DataSource = dsAvSite.Tables[0].DefaultView;
        //lbAvLocation.DataBind();

        DataSet dsAsSite = objSiteLocation.retrieveAssignLocationRoomsOnly();
        DataTable dtRoom = dsAsSite.Tables[0];
        objCommon = new CommonBAL();
        objCommon.setDataSource(ddlRoom, dtRoom, "-Select-");
        //lbAsLocation.DataValueField = dsAsSite.Tables[0].Columns[0].ColumnName;
        //lbAsLocation.DataSource = dsAsSite.Tables[0].DefaultView;
        //lbAsLocation.DataBind();
    }
}
