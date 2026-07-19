/*
File Name   :	Host.aspx.cs

Description :	Used to create business unit

Date created:	01 Sep 2011

Modification History:
***********************
CR		Name			    Date			Description
New		Jagadeesh Babu K	01/09/2011		File has been created.
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

public partial class Host : System.Web.UI.Page
{
    #region "Declarations"
    private iAssetTrackBAL.HostBAL objHost;
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
        grdHost.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(grdHost_ItemCommand);
        pagerControl = grdHost.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);
    }
    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdHost.Behaviors.Paging.PageIndex = e.PageNumber;
        populateGrid();
    }
    protected void grdHost_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControl.SetupPageList(this.grdHost.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdHost.Behaviors.Paging.PageIndex);
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
            pagerControl.SetupPageList(this.grdHost.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdHost.Behaviors.Paging.PageIndex);

        }

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        populateGrid();
    }
    /// <summary>
    /// Used to load the page.
    /// </summary>
    /// <author>Jagadeesh</author>
    /// <CreatedOn> </CreatedOn>

    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageHeader"] = "Host";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        lblMessage.Visible = false;

        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "Host.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        if (_dtRights.Select("Module = 'Host'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }
        if (_dtRights.Select("Module = 'Host' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }
        if (_dtRights.Select("Module = 'Host' and Rights = '" + "Create" + "'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }

        if (_dtRights.Select("Module = 'Host' and Rights = '" + "Delete" + "'").Length != 0)
        {
            ibDelete.Visible = true;
        }
        else
        {
            ibDelete.Visible = false;
        }

        this.grdHost.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());

        populateGrid();

        if (!IsPostBack)
        {

            Session["HostID"] = null;

        }
    }

    /// <summary>
    /// Used to call once page loading complete.
    /// </summary>
    /// <author>Jagadeesh</author>
    /// <createdOn>01 Sep 2011</createdOn>
    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        if (ibDelete.Enabled)
        {
            iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();
            string sMessage = objCommon.displayMessage(MessageCodes.HOST_JS_DELETE);
            hdnMessage.Value = sMessage;
        }

        string errMsg = GetLocalResourceObject("revDesc1Resource1.ErrorMessage").ToString();
        cvDesc.ErrorMessage = errMsg.Replace("{MAXLENGTH}", txtDesc.MaxLength.ToString());

    }

    /// <summary>
    /// Used to call upon grid page index changes.
    /// </summary>
    /// <author>Jagadeesh</author>
    /// <createdOn>01 Sep 2011</createdOn>
    protected void grdHost_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //grdHost.PageIndex = e.NewPageIndex;
        populateGrid();
    }

    /// <summary>
    /// Used to call upon grid row edits.
    /// </summary>
    /// <author>Jagadeesh</author>
    /// <createdOn>01 Sep 2011</createdOn>
    protected void grdHost_RowEditing(object sender, GridViewEditEventArgs e)
    {
        //populateGrid();
        //objHost = new HostBAL();
        //objHost.BusinessUnitID = Convert.ToInt32(grdHost.DataKeys[Convert.ToInt32(e.NewEditIndex)].Value);
        //DataSet dsHost = objHost.retrieve();
        //DataRow dr = dsHost.Tables[0].Rows[0];
        //txtHost.Text = dr[DBFields.DBFIELD_BUSINESSUNIT].ToString();
        //txtDesc.Text = dr[DBFields.DBFIELD_DESCRIPTION].ToString();
        //txtCoPrefix.Text = dr[DBFields.DBFIELD_COPREFIX].ToString();
        //Session["HostID"] = objHost.BusinessUnitID;
    }

    /// <summary>
    /// Used to save information related BU.
    /// </summary>
    /// <author>Jagadeesh</author>
    /// <createdOn>01 Sep 2011</createdOn>
    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        // objHost = new iAssetTrack.BAL.hSitesBAL();
        objHost = new iAssetTrackBAL.HostBAL();

        objHost.HostName = txtHost.Text.Trim();
        //if (Session["SiteID"] != null)
        //{
        //    objSite.SiteID = (int)Session["SiteID"];
        //}
        //else
        //{
        //    objSite.SiteID = objSite.exists();
        //}
        objHost.Description = txtDesc.Text.Trim();
        objHost.Status = 1;
        objHost.CreatedBy = Convert.ToInt32(Session["UserID"]);

        string strHost = string.Empty;
        objHost.HostID = Session["HostID"] == null ? strHost : (string)Session["HostID"];
        strHost = objHost.exists().ToString();
        //objHost.HostID = Session["HostID"] == null ? strHost : (int)Session["HostID"];
        //strHost = objHost.exists();


        if (strHost != "-1" && strHost != "0")
            objHost.HostID = strHost;
        if (strHost != "-1")
        {
            objHost.Persist(DALCOperation.Insert);
            clearFields();
            if (Session["HostID"] == null)
            {
                objCommon = new CommonBAL();
                lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_INSERTED);
            }
            else
            {
                objCommon = new CommonBAL();
                lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_UPDATED);
            }

            Session["HostID"] = null;
            lblMessage.Visible = true;
            grdHost.ClearDataSource();
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
    /// <author>Jagadeesh</author>
    /// <createdOn>01 Sep 2011</createdOn>
    protected void ibReset_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        clearFields();
        Session["HostID"] = null;
        txtHost.Focus();
        //Added - 13-Nov-2006
        //Check to Show/Hide the delete button
        populateGrid();
    }

    /// <summary>
    /// Used to delete information related specific BU.
    /// </summary>
    /// <author>Jagadeesh</author>
    /// <createdOn>01 Sep 2011</createdOn>
    protected void ibDelete_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        CheckBox chkDelete;
        string hostID;
        string strIDs;

        strIDs = "";
        foreach (Infragistics.Web.UI.GridControls.GridRecord grdViewRow in grdHost.Rows)
        {
            chkDelete = (CheckBox)(grdViewRow.Items[3].FindControl("chkDelete"));
            if (chkDelete.Checked == true)
            {
                hostID = ((Label)(grdViewRow.Items[3].FindControl("lblDeleteID"))).Text;
                strIDs += "'" + hostID + "',";
            }
        }

        if (strIDs != "")
        {
            strIDs = strIDs.Remove(strIDs.Length - 1, 1);
        }

        objHost = new HostBAL();
        objHost.HostIDs = strIDs;
        objHost.Status = 0;
        objHost.LastModifiedBy = Convert.ToInt32(Session["UserID"]);
        objHost.Persist(DALCOperation.Delete);

        clearFields();

        objCommon = new CommonBAL();
        lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_DELETED);
        lblMessage.Visible = true;
        grdHost.ClearDataSource();
        populateGrid();
    }
    #endregion

    #region "User Defined Methods"
    /// <summary>
    /// Populate Business Unit data grid
    /// </summary>    
    /// <author>Jagadeesh</author>
    /// <createdOn>01 Sep 2011</createdOn>
    //private void populateGrid()
    //{
    //    grdHost.ClearDataSource();
    //    objHost = new HostBAL();
    //    grdHost.DataSource = objHost.retrieve().Tables[0];
    //    grdHost.DataBind();
    //    applyRights();
    //    //if (nonDeletableRowCount == grdHost.Rows.Count)
    //    //{
    //    //    grdHost.Columns[3].Hidden = true; //Delete column
    //    //    ibDelete.Visible = false;
    //    //}
    //    //nonDeletableRowCount = 0;

    //}

    private void populateGrid()
    {
        //objHost = new HostBAL();
        //grdHost.DataSource = objHost.retrieve().Tables[0];
        //grdHost.DataBind();
        objHost = new HostBAL();
        DataTable dtGrid = objHost.retrieve().Tables[0];

        if (bool.Parse(Session["TenantUser"].ToString()))
        {
            ArrayList tenantAssignedHosts = new ArrayList();
            UserBAL objUser = new UserBAL();
            objUser.UserID = Convert.ToInt32(Session["UserID"]);
            DataSet dsTenant = objUser.retrieveTenantDetails();
            if (dsTenant.Tables.Count > 0 && dsTenant.Tables[0].Rows.Count > 0)
            {
                TenantBAL objTenant = new TenantBAL();
                objTenant.TenantId = Convert.ToInt32(dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_ID].ToString());
                DataSet dsTD = objTenant.retrieveHostAssignmentList();
                if (dsTD.Tables.Count > 0 && dsTD.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsTD.Tables[0].Rows)
                    {
                        tenantAssignedHosts.Add("CONVERT('" + dr[DBFields.DBFIELD_HOSTID].ToString() + "', 'System.Guid')");
                    }
                }
                if (tenantAssignedHosts.Count > 0)
                {
                    DataTable tempTable = dtGrid.Clone();
                    foreach (DataRow dr in dtGrid.Rows)
                    {
                        tempTable.Rows.Add(dr.ItemArray);
                    }
                    DataRow[] filteredRows = tempTable.Select("HostID IN (" + string.Join(",", tenantAssignedHosts.ToArray()) + ")");
                    dtGrid.Rows.Clear();
                    if (filteredRows != null && filteredRows.Length > 0)
                    {
                        foreach (DataRow dr in filteredRows)
                        {
                            dtGrid.Rows.Add(dr.ItemArray);
                        }

                    }
                }
                else
                {
                    dtGrid.Rows.Clear();
                }

            }

        }
        else
        {
            ArrayList tenantAssignedHosts = new ArrayList();
            TenantBAL objTenant = new TenantBAL();
            DataSet dsTD = objTenant.retrieveHostAssignmentList();
            if (dsTD.Tables.Count > 0 && dsTD.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsTD.Tables[0].Rows)
                {
                    tenantAssignedHosts.Add("CONVERT('" + dr[DBFields.DBFIELD_HOSTID].ToString() + "', 'System.Guid')");
                }
            }
            if (tenantAssignedHosts.Count > 0)
            {
                DataTable tempTable = dtGrid.Clone();
                foreach (DataRow dr in dtGrid.Rows)
                {
                    tempTable.Rows.Add(dr.ItemArray);
                }
                DataRow[] filteredRows = tempTable.Select("HostID NOT IN (" + string.Join(",", tenantAssignedHosts.ToArray()) + ")");
                dtGrid.Rows.Clear();
                if (filteredRows != null && filteredRows.Length > 0)
                {
                    foreach (DataRow dr in filteredRows)
                    {
                        dtGrid.Rows.Add(dr.ItemArray);
                    }

                }
            }
        }

        totalRecordCount = dtGrid.Rows.Count;

        grdHost.Behaviors.Paging.Enabled = false;
        if (totalRecordCount == grdHost.Rows.Count)
            this.FilterCount = "";
        grdHost.Behaviors.Paging.Enabled = true;

        if (dtGrid.Rows.Count == 0)
        {
            //grdHost.DataSource = null;
            grdHost.DataSource = dtGrid;
            grdHost.DataBind();
            grdHost.Columns[3].Hidden = true;
            ibDelete.Visible = false;
        }
        else// if (grdHost.Rows.Count > 0)
        {
            grdHost.DataSource = dtGrid;
            grdHost.DataBind();
            if (_dtRights.Select("Rights = 'Delete' and Module = 'Host'").Length != 0)
            {
                grdHost.Columns[3].Hidden = false;
                ibDelete.Visible = true;

            }
            else
            {
                grdHost.Columns[3].Hidden = true;
                ibDelete.Visible = false;
            }
            if (_dtRights.Select("Rights = 'Modify' and Module = 'Host'").Length != 0)
            {
                grdHost.Columns[2].Hidden = false;
            }
            else
            {
                grdHost.Columns[2].Hidden = true;
            }




            if (ibDelete.Visible == true)
            {
                string HostId;
                int iCount = 0;

                for (int i = 0; i < grdHost.Rows.Count; i++)
                {
                    //HostId = Convert.ToInt32(((Label)(grdHost.Rows[i].Items[3].FindControl("lblDeleteID"))).Text);
                    HostId = ((Label)(grdHost.Rows[i].Items[3].FindControl("lblDeleteID"))).Text;
                    grdHost.Rows[i].Items[3].FindControl("chkDelete").Visible = true;
                    objCommon = new iAssetTrack.BAL.CommonBAL();

                    DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_HOSTID, HostId, 1);
                    foreach (DataTable tblCheck in dsCheck.Tables)
                    {
                        if (tblCheck.Rows[0][0].ToString() != "0")
                        {
                            grdHost.Rows[i].Items[3].FindControl("chkDelete").Visible = false;
                        }
                    }
                    if (grdHost.Rows[i].Items[3].FindControl("chkDelete").Visible == false)
                    {
                        iCount += 1;
                    }

                }

                if (iCount == grdHost.Rows.Count)
                {
                    grdHost.Columns[3].Hidden = true;
                    ibDelete.Visible = false;
                }
            }
            if (grdHost.Rows.Count > 0)
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
        //if (grdHost.Rows.Count != 0)
        //{
        //    if (_dtRights.Select("Rights = 'Delete' and Module = 'Host'").Length != 0)
        //    {
        //        grdHost.Columns[3].Hidden = false;
        //        ibDelete.Visible = true;
        //    }
        //    else
        //    {
        //        grdHost.Columns[3].Hidden = true;
        //        ibDelete.Visible = false;
        //    }

        //    if (_dtRights.Select("Rights = 'Modify' and Module = 'Host'").Length != 0)
        //    {
        //        grdHost.Columns[2].Hidden = false;
        //    }
        //    else
        //    {
        //        grdHost.Columns[2].Hidden = true;
        //    }
        //}


    }

    /// <summary>
    /// Reset fields
    /// </summary>    
    /// <author>Jagadeesh</author>
    /// <createdOn>01 Sep 2011</createdOn>
    private void clearFields()
    {
        txtHost.Text = "";
        txtDesc.Text = "";
        lblMessage.Visible = false;
        lblMessage.Text = "";
    }
    #endregion
    protected void grdHost_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e)
    {
        //if (e.CommandName == "Edit")
        //{
        //    populateGrid();
        //    objHost = new HostBAL();
        //    objHost.HostID = Convert.ToString(e.CommandArgument);
        //    DataSet dsHost = objHost.retrieve();
        //    DataRow dr = dsHost.Tables[0].Rows[0];
        //    txtHost.Text = dr[DBFields.DBFIELD_HOST].ToString();
        //    txtDesc.Text = dr[DBFields.DBFIELD_DESCRIPTION].ToString();
        //    Session["HostID"] = objHost.HostID;
        //}
        if (e.CommandName == "Edit")
        {
            populateGrid();
            iAssetTrackBAL.HostBAL objEdit = new iAssetTrackBAL.HostBAL();
            objEdit.HostID = Convert.ToString(e.CommandArgument);
            DataSet dsHost = objEdit.retrieve();
            DataRow dr = dsHost.Tables[0].Rows[0];
            txtHost.Text = dr[DBFields.DBFIELD_HOST].ToString();
            txtDesc.Text = dr[DBFields.DBFIELD_DESCRIPTION].ToString();
            Session["HostID"] = objEdit.HostID;
            if (_dtRights.Select("Module = 'Host' and Rights = '" + "Modify" + "'").Length != 0)
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
            pagerControl.SetCurrentPageNumber(grdHost.Behaviors.Paging.PageIndex);
        }

    }

    protected void ibExportToExcel_Click(object sender, EventArgs e)
    {
        Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        this.eExporter.DataExportMode = Infragistics.Web.UI.GridControls.DataExportMode.AllDataInDataSource;
        Infragistics.Documents.Excel.Workbook wBook = new Infragistics.Documents.Excel.Workbook(excelFormat);
        this.eExporter.Export(this.grdHost, wBook);
    }
    protected void eExporter_CellExported(object sender, Infragistics.Web.UI.GridControls.ExcelCellExportedEventArgs e)
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
    protected void grdHost_DataFiltered(object sender, Infragistics.Web.UI.GridControls.FilteredEventArgs e)
    {
        // Cast the WebDataGrid DataSource to a DataTable  
        DataTable dt = this.grdHost.DataSource as DataTable;

        // Disable WebDataGrid Paging
        grdHost.Behaviors.Paging.Enabled = false;
        this.FilterCount = grdHost.Rows.Count.ToString();
        if (!string.IsNullOrEmpty(this.FilterCount))
        {
            if (totalRecordCount == int.Parse(this.FilterCount))
            {
                this.FilterCount = "";
            }
        }
        // Enable WebDataGrid Paging
        grdHost.Behaviors.Paging.Enabled = true;
    }

    protected void grdHost_InitializeRow(object sender, Infragistics.Web.UI.GridControls.RowEventArgs e)
    {
        //    string hostID;

        //    hostID =((Label)(e.Row.Items[3].FindControl("lblDeleteID"))).Text;

        //    objCommon = new iAssetTrack.BAL.CommonBAL();

        //    DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_HOSTID, hostID,1);
        //    foreach (DataTable tblCheck in dsCheck.Tables)
        //    {
        //        if (tblCheck.Rows[0][0].ToString() != "0")
        //        {
        //            e.Row.Items[3].FindControl("chkDelete").Visible = false;

        //        }
        //    }
        //    if (e.Row.Items[3].FindControl("chkDelete").Visible == false)
        //    {
        //        nonDeletableRowCount++;
        //    }
    }
}
