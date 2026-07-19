/*
File Name   :	CreateApplication.aspx.cs

Description :	Used to create Asset Model

Date created:	23 June 2011

Modification History:
***********************
CR		Name			Date			Description
New		Nayana M    	02 Aug 2011 	File has been created.
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
using Infragistics.Web.UI.NavigationControls;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

public partial class CreateApplication : System.Web.UI.Page
{
    #region "Declarations"

    private iAssetTrack.BAL.ApplicationBAL objAppl;
    private iAssetTrack.BAL.ApplicationTypeBAL objAT;
    private iAssetTrack.BAL.ApplicationCriticalityBAL objAC;
    private iAssetTrack.BAL.ManufacturerBAL objMfg;
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

        grdAppl.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(grdAppl_ItemCommand);
        pagerControl = grdAppl.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        if (pagerControl != null)
            pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);
    }
    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdAppl.Behaviors.Paging.PageIndex = e.PageNumber;
        populateGrid();
    }
    //Added on 19Apr2013
    protected void Page_PreRender(object sender, EventArgs e)
    {
        populateGrid();
    }
    /// <summary>
    /// Used to load the page.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageHeader"] = "Create Application";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        lblMessage.Visible = false;

        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "CreateApplication.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = _dtRights != null && _dtRights.Select("Module = 'Create Application' and Rights = '" + "View" + "'").Length != 0;

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }
        if (_dtRights.Select("Module = 'Create Application' and Rights = '" + "Create" + "'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }

        if (_dtRights.Select("Module = 'Create Application' and Rights = '" + "Delete" + "'").Length != 0)
        {
            ibDelete.Visible = true;
        }
        else
        {
            ibDelete.Visible = false;
        }



        this.grdAppl.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());
        populateGrid();
        if (!IsPostBack)
        {
            Session["ApplID"] = null;
            populateApplTypeList();
            populateApplCriticalityList();
            populateBusinessUnitList();
            populateManageList();
            populateStatusList();
            Session["ApplID"] = null;
            ddlBU.SelectedIndex = 1;
            ddlBU.Enabled = false;
        }
    }

    /// <summary>
    /// Used to call once page loading complete.
    /// </summary>
    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        if (ibDelete.Enabled)
        {
            iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();
            string sMessage = objCommon.displayMessage(MessageCodes.APPL_JS_DELETE);
            hdnMessage.Value = sMessage;
        }

        string errMsg = GetLocalResourceObject("revDesc1Resource1.ErrorMessage").ToString();
        cvDesc.ErrorMessage = errMsg.Replace("{MAXLENGTH}", txtDesc.MaxLength.ToString());
    }

    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        try
        {
            objAppl = new iAssetTrack.BAL.ApplicationBAL();
            objAppl.ApplName = txtApplicationName.Text;
            objAppl.Description = txtDesc.Text;
            objAppl.Status = 1;
            if (ddlApplType.SelectedIndex > 0)
                objAppl.ApplTypeID = Convert.ToInt32(ddlApplType.SelectedItem.Value);


            if (ddlAppStatus.SelectedIndex > 0)
                objAppl.AppStatusID = Convert.ToInt32(ddlAppStatus.SelectedItem.Value);
            else
                objAppl.AppStatusID = 0;

            if (ddlApplCriticality.SelectedIndex > 0)
                objAppl.ApplCriticality = Convert.ToInt32(ddlApplCriticality.SelectedItem.Value);
            if (ddlBU.SelectedIndex > 0)
                objAppl.BUID = Convert.ToInt32(ddlBU.SelectedItem.Value);
            if (!string.IsNullOrEmpty(this.hdnOwnerID.Value.Trim()))
                objAppl.OwnerID = Convert.ToInt32(this.hdnOwnerID.Value);
            else
                objAppl.OwnerID = 0;

            if (ddlApplManage.SelectedIndex > 0)
                objAppl.ApplManageID = Convert.ToInt32(ddlApplManage.SelectedItem.Value);


            if (Session["ApplID"] != null)
            {
                objAppl.ApplID = (int)Session["ApplID"];
            }
            else
            {
                objAppl.ApplID = objAppl.exists();
            }

            int intAppl = 0;
            objAppl.ApplID = Session["ApplID"] == null ? intAppl : (int)Session["ApplID"];
            intAppl = objAppl.exists();

            objAppl.CreatedBy = Convert.ToInt32(Session["UserID"]);

            if (intAppl != -1 && intAppl != 0)
                objAppl.ApplID = intAppl;

            if (intAppl != -1)
            {
                objAppl.Persist(DALCOperation.Insert);
                clearFields();
                if (Session["ApplID"] == null)
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
                grdAppl.ClearDataSource();
                populateGrid();
                Session["ApplID"] = null;
            }
            else
            {
                objCommon = new CommonBAL();
                lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_I_EXISTS);
                lblMessage.Visible = true;
                populateGrid();
            }
        }
        catch (Exception ex)
        {
            //lblMessage.Text = ex.Message;
            //lblMessage.Visible = true;
            ExceptionPolicy.HandleException(ex, "errDCTrack");
            throw ex;
        }
    }

    protected void ibReset_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        clearFields();
        Session["ApplID"] = null;
        txtApplicationName.Focus();

        populateGrid();
    }

    protected void ibDelete_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        CheckBox chkDelete;
        int ApplID;
        string strIDs;

        strIDs = "";
        foreach (Infragistics.Web.UI.GridControls.GridRecord grdViewRow in grdAppl.Rows)
        {
            chkDelete = (CheckBox)(grdViewRow.Items[5].FindControl("chkDelete"));
            if (chkDelete.Checked == true)
            {
                ApplID = Convert.ToInt32(((Label)(grdViewRow.Items[5].FindControl("lblDeleteID"))).Text);
                strIDs += Convert.ToString(ApplID) + ",";
            }
        }

        if (strIDs != "")
        {
            strIDs = strIDs.Remove(strIDs.Length - 1, 1);
        }

        objAppl = new iAssetTrack.BAL.ApplicationBAL();
        objAppl.ApplIDs = strIDs;
        objAppl.Status = 0;
        objAppl.LastModifiedBy = Convert.ToInt32(Session["UserID"]);
        objAppl.Persist(DALCOperation.Delete);

        clearFields();

        objCommon = new CommonBAL();
        lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_DELETED);
        lblMessage.Visible = true;
        grdAppl.ClearDataSource();
        populateGrid();
    }

    protected void grdAppl_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            populateGrid();
            objAppl = new iAssetTrack.BAL.ApplicationBAL();
            objAppl.ApplID = Convert.ToInt32(e.CommandArgument);
            Session["ApplID"] = Convert.ToInt32(e.CommandArgument).ToString();
            DataSet dsAppl = objAppl.retrieve();
            DataRow dr = dsAppl.Tables[0].Rows[0];
            txtApplicationName.Text = dr[DBFields.DBFIELD_APPLNAME].ToString();
            txtDesc.Text = dr[DBFields.DBFIELD_APPLDESC].ToString();

            //if(!string.IsNullOrEmpty(dr[DBFields.DBFIELD_MFGID].ToString()))
            //    ddlApplType.SelectedValue = dr[DBFields.DBFIELD_APPLTYPEID].ToString();
            //else
            //    ddlApplType.SelectedIndex=0;
            if (ddlApplType.Items.Count > 1)
            {
                if (!string.IsNullOrEmpty(dr[DBFields.DBFIELD_APPLTYPEID].ToString()))
                    ddlApplType.SelectedValue = dr[DBFields.DBFIELD_APPLTYPEID].ToString();
                else
                    ddlApplType.SelectedIndex = 0;
            }

            if (!string.IsNullOrEmpty(dr[DBFields.DBFIELD_APPLSTATUS].ToString().Trim()))
            {
                CompareItem(ddlAppStatus, dr[DBFields.DBFIELD_APPLSTATUS].ToString().Trim());
            }
            else
            {
                ddlAppStatus.SelectedIndex = 0;
            }

            if (ddlApplCriticality.Items.Count > 1)
            {
                if (!string.IsNullOrEmpty(dr[DBFields.DBFIELD_APPLCRITICALITYID].ToString()))
                    ddlApplCriticality.SelectedValue = dr[DBFields.DBFIELD_APPLCRITICALITYID].ToString();
                else
                    ddlApplCriticality.SelectedIndex = 0;
            }

            if (!string.IsNullOrEmpty(dr[DBFields.DBFIELD_APPLBUID].ToString()))
                ddlBU.SelectedValue = dr[DBFields.DBFIELD_APPLBUID].ToString();
            else
                ddlBU.SelectedIndex = 0;

            OwnerBAL ownerBAL = new OwnerBAL();
            ownerBAL.OwnerID = Convert.ToInt32(dr["OwnerID"].ToString());
            if (ownerBAL.OwnerID > 0)
            {
                DataSet dsOwner = ownerBAL.retrieve();
                hdnOwnerName.Value = dsOwner.Tables[0].Rows[0]["OwnerLastName"].ToString().Trim() + "," + dsOwner.Tables[0].Rows[0]["OwnerFirstName"].ToString().Trim();
                txtOwner.Text = dsOwner.Tables[0].Rows[0]["OwnerLastName"].ToString().Trim() + "," + dsOwner.Tables[0].Rows[0]["OwnerFirstName"].ToString().Trim();
                hdnOwnerID.Value = dr["OwnerID"].ToString();
            }
            else
            {
                hdnOwnerID.Value = dr["OwnerID"].ToString();
                hdnOwnerName.Value = "";
                txtOwner.Text = "";
            }
            if (ddlApplManage.Items.Count > 1)
            {
                if (!string.IsNullOrEmpty(dr[DBFields.DBFIELD_APPLMANAGEID].ToString()))
                    ddlApplManage.SelectedValue = dr[DBFields.DBFIELD_APPLMANAGEID].ToString();
                else
                    ddlApplManage.SelectedIndex = 0;
            }
            Session["ApplID"] = objAppl.ApplID;

            if (_dtRights.Select("Module = 'Create Application' and Rights = '" + "Modify" + "'").Length != 0)
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
            pagerControl.SetCurrentPageNumber(grdAppl.Behaviors.Paging.PageIndex);
        }
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

    protected void ibExportToExcel_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        this.eExporter.DataExportMode = Infragistics.Web.UI.GridControls.DataExportMode.AllDataInDataSource;
        Infragistics.Documents.Excel.Workbook wBook = new Infragistics.Documents.Excel.Workbook(excelFormat);
        this.eExporter.Export(this.grdAppl, wBook);
    }

    protected void eExporter_CellExported(object sender, Infragistics.Web.UI.GridControls.ExcelCellExportedEventArgs e)
    {
        int iWSdex = e.Worksheet.Index;
        int iRdex = e.CurrentRowIndex;
        int iCdex = e.CurrentColumnIndex;
        e.Worksheet.Columns[4].Width = 1;
        e.Worksheet.Columns[5].Width = 1;
        if (iWSdex == 0)
        {
            if (iRdex == 0)
            {
                if (iCdex == 4 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
                {
                    e.Worksheet.Rows[iRdex].Cells[iCdex].Value = "";
                }
                if (iCdex == 5 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
                {
                    e.Worksheet.Rows[iRdex].Cells[iCdex].Value = "";
                }
            }
        }
    }

    protected void grdAppl_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControl.SetupPageList(this.grdAppl.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdAppl.Behaviors.Paging.PageIndex);
        }

        Control postbackControlInstance = null;
        string postbackControlName = this.Request.Params.Get("__EVENTTARGET");
        if (postbackControlName != null && postbackControlName != string.Empty)
            postbackControlInstance = this.FindControl(postbackControlName);
        if (postbackControlInstance != null && postbackControlInstance.ID == "PagerPageList")
        {
            //do nothing
            //pagerControl.SetupPageList(this.grdModel.Behaviors.Paging.PageCount);
        }
        else
        {
            pagerControl.SetCurrentPageNumber(grdAppl.Behaviors.Paging.PageIndex);
            pagerControl.SetupPageList(this.grdAppl.Behaviors.Paging.PageCount);
        }
    }

    protected void ddlBU_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    #endregion

    #region "User Defined Methods"

    private void populateBusinessUnitList()
    {
        int intUserID = 0;
        if (Session["UserID"] != null)
            intUserID = Convert.ToInt32(Session["UserID"].ToString());

        FillDropDownListBU(StoredProcedures.SP_BUSINESSUNIT_LISTBYUSERID, ref ddlBU, intUserID);
    }


    private void FillDropDownListBU(string strStoredProc, ref DropDownList ddl, int id)
    {

        //CommonBAL objCommonBAL = new CommonBAL();
        //ICommon svcBU = (ICommon)RemotingHelper.CreateProxy(typeof(ICommon));
        iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();

        DataTable dt = objCommon.FillDropDownListBU(strStoredProc, "- Select -", id);
        ddl.DataSource = dt;
        ddl.DataValueField = dt.Columns[0].ToString();
        ddl.DataTextField = dt.Columns[1].ToString();
        ddl.DataBind();

    }


    /// <summary>
    /// Populate Business Unit data grid
    /// </summary>    
    //private void populateGrid()
    //{
    //    grdAppl.Rows.Clear();
    //objAppl = new iAssetTrack.BAL.ApplicationBAL();
    //    grdAppl.ClearDataSource();
    //    grdAppl.DataSource=objAppl.retrieve().Tables[0];
    //    grdAppl.DataBind();
    //    applyRights();
    //}

    //private void applyRights()
    //{
    //    if (grdAppl.Rows.Count != 0)
    //    {
    //        if (_dtRights.Select("Rights = 'Delete' and Module = 'Create Application'").Length != 0)
    //        {
    //            grdAppl.Columns[5].Hidden = false;
    //            ibDelete.Visible = true;
    //        }
    //        else
    //        {
    //            grdAppl.Columns[5].Hidden = true;
    //            ibDelete.Visible = false;
    //        }

    //        if (_dtRights.Select("Rights = 'Modify' and Module = 'Create Application'").Length != 0)
    //        {
    //            grdAppl.Columns[5].Hidden = false;
    //        }
    //        else
    //        {
    //            grdAppl.Columns[5].Hidden = true;
    //        }
    //    }

    //    if (ibDelete.Visible == true)
    //    {
    //        int modelID;
    //        int iCount = 0;

    //        for (int i = 0; i < grdAppl.Rows.Count; i++)
    //        {
    //            modelID = Convert.ToInt32(((Label)(grdAppl.Rows[i].Items[5].FindControl("lblDeleteID"))).Text);

    //            objCommon = new iAssetTrack.BAL.CommonBAL();

    //            DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_APPLID, modelID.ToString(),0);
    //            foreach (DataTable tblCheck in dsCheck.Tables)
    //            {
    //                if (tblCheck.Rows[0][0].ToString() != "0")
    //                {
    //                    grdAppl.Rows[i].Items[5].FindControl("chkDelete").Visible = false;
    //                }
    //            }
    //            if (grdAppl.Rows[i].Items[5].FindControl("chkDelete").Visible == false)
    //            {
    //                iCount += 1;
    //            }
    private void populateGrid()
    {

        objAppl = new iAssetTrack.BAL.ApplicationBAL();
        DataTable dtGrid = objAppl.retrieve().Tables[0];

        if (bool.Parse(Session["TenantUser"].ToString()))
        {
           string  tenantAssignedApps = string.Empty;
            UserBAL objUser = new UserBAL();
            objUser.UserID = Convert.ToInt32(Session["UserID"]);
            DataSet dsTenant = objUser.retrieveTenantDetails();
            if (dsTenant.Tables.Count > 0 && dsTenant.Tables[0].Rows.Count > 0)
            {
                TenantBAL objTenant = new TenantBAL();
                objTenant.TenantId = Convert.ToInt32(dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_ID].ToString());
                DataSet dsTD = objTenant.retrieveApplicationAssignmentList();
                if (dsTD.Tables.Count > 0 && dsTD.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsTD.Tables[0].Rows)
                    {
                        tenantAssignedApps = tenantAssignedApps + dr[DBFields.DBFIELD_APPLICATION_ID].ToString() + ",";
                    }
                    tenantAssignedApps = tenantAssignedApps.Trim(',');

                }
                if (!string.IsNullOrEmpty(tenantAssignedApps))
                {
                    DataTable tempTable = dtGrid.Clone();
                    foreach (DataRow dr in dtGrid.Rows)
                    {
                        tempTable.Rows.Add(dr.ItemArray);
                    }
                    DataRow[] filteredRows = tempTable.Select("ApplID IN (" + tenantAssignedApps + ")");
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

        totalRecordCount = dtGrid.Rows.Count;
        grdAppl.DataSource = dtGrid;
        grdAppl.DataBind();

        grdAppl.Behaviors.Paging.Enabled = false;
        if (totalRecordCount == grdAppl.Rows.Count)
            this.FilterCount = "";
        grdAppl.Behaviors.Paging.Enabled = true;

        if (grdAppl.Rows.Count == 0)
        {
            grdAppl.DataSource = dtGrid;
            grdAppl.DataBind();
            grdAppl.Columns[5].Hidden = true;
            ibDelete.Visible = false;
        }
        else
        {
            if (_dtRights.Select("Rights = 'Delete' and Module = 'Create Application'").Length != 0)
            {
                grdAppl.Columns[5].Hidden = false;
                ibDelete.Visible = true;
            }
            else
            {
                grdAppl.Columns[5].Hidden = true;
                ibDelete.Visible = false;
            }

            if (_dtRights.Select("Rights = 'Modify' and Module = 'Create Application'").Length != 0)
            {
                grdAppl.Columns[5].Hidden = false;
            }
            else
            {
                grdAppl.Columns[5].Hidden = true;
            }
        }

        if (ibDelete.Visible == true)
        {
            int modelID;
            int iCount = 0;

            for (int i = 0; i < grdAppl.Rows.Count; i++)
            {
                modelID = Convert.ToInt32(((Label)(grdAppl.Rows[i].Items[5].FindControl("lblDeleteID"))).Text);

                objCommon = new iAssetTrack.BAL.CommonBAL();

                //TODO: Check the DBFields.DBFIELD_APPLDIVISIONID
                DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_APPLID, modelID.ToString(), 0);
                foreach (DataTable tblCheck in dsCheck.Tables)
                {
                    if (tblCheck.Rows[0][0].ToString() != "0")
                    {
                        grdAppl.Rows[i].Items[5].FindControl("chkDelete").Visible = false;
                    }
                }
                if (grdAppl.Rows[i].Items[5].FindControl("chkDelete").Visible == false)
                {
                    iCount += 1;
                }

            }
            if (iCount == grdAppl.Rows.Count)
            {
                grdAppl.Columns[5].Hidden = true;
                ibDelete.Visible = false;
            }
        }
        if (grdAppl.Rows.Count > 0)
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

    //        }
    //        if (iCount == grdAppl.Rows.Count)
    //        {
    //            grdAppl.Columns[5].Hidden = true;
    //            ibDelete.Visible = false;
    //        }
    //    }
    //}
    protected void grdAppl_DataFiltered(object sender, Infragistics.Web.UI.GridControls.FilteredEventArgs e)
    {
        // Cast the WebDataGrid DataSource to a DataTable  
        DataTable dt = this.grdAppl.DataSource as DataTable;
        // Disable WebDataGrid Paging
        grdAppl.Behaviors.Paging.Enabled = false;
        this.FilterCount = grdAppl.Rows.Count.ToString();
        if (!string.IsNullOrEmpty(this.FilterCount))
        {
            if (totalRecordCount == int.Parse(this.FilterCount))
            {
                this.FilterCount = "";
            }
        }
        // Enable WebDataGrid Paging
        grdAppl.Behaviors.Paging.Enabled = true;
    }
    private void populateApplTypeList()
    {
        objAT = new iAssetTrack.BAL.ApplicationTypeBAL();
        DataSet dsAT = objAT.retrieve();
        DataTable dtAT = dsAT.Tables[0];

        DataRow dr = dtAT.NewRow();
        dr[0] = 0;
        dr[1] = "-Select-";
        dtAT.Rows.InsertAt(dr, 0);

        ddlApplType.DataSource = dtAT;
        ddlApplType.DataValueField = dtAT.Columns[0].ToString();
        ddlApplType.DataTextField = dtAT.Columns[1].ToString();
        ddlApplType.DataBind();
    }

    private void populateApplCriticalityList()
    {
        objAC = new iAssetTrack.BAL.ApplicationCriticalityBAL();
        DataSet dsAC = objAC.retrieve();
        DataTable dtAC = dsAC.Tables[0];

        DataRow dr = dtAC.NewRow();
        dr[0] = 0;
        dr[1] = "-Select-";
        dtAC.Rows.InsertAt(dr, 0);

        ddlApplCriticality.DataSource = dtAC;
        ddlApplCriticality.DataValueField = dtAC.Columns[0].ToString();
        ddlApplCriticality.DataTextField = dtAC.Columns[1].ToString();
        ddlApplCriticality.DataBind();
    }

    private void populateManageList()
    {
        objMfg = new iAssetTrack.BAL.ManufacturerBAL();
        DataSet dsMfg = objMfg.retrieve();
        DataTable dtMfg = dsMfg.Tables[0];

        DataRow dr = dtMfg.NewRow();
        dr[0] = 0;
        dr[1] = "-Select-";
        dtMfg.Rows.InsertAt(dr, 0);

        ddlApplManage.DataSource = dtMfg;
        ddlApplManage.DataValueField = dtMfg.Columns[0].ToString();
        ddlApplManage.DataTextField = dtMfg.Columns[1].ToString();
        ddlApplManage.DataBind();
    }

    private void populateStatusList()
    {
        AppStatusBAL objAS = new iAssetTrack.BAL.AppStatusBAL();
        DataSet dsAS = objAS.retrieve();
        DataTable dtAS = dsAS.Tables[0];

        DataRow dr = dtAS.NewRow();
        dr[0] = 0;
        dr[1] = "-Select-";
        dtAS.Rows.InsertAt(dr, 0);

        ddlAppStatus.DataSource = dtAS;
        ddlAppStatus.DataValueField = dtAS.Columns[0].ToString();
        ddlAppStatus.DataTextField = dtAS.Columns[1].ToString();
        ddlAppStatus.DataBind();
    }

    /// <summary>
    /// Reset fields
    /// </summary>    
    private void clearFields()
    {
        txtApplicationName.Text = "";
        txtDesc.Text = "";
        lblMessage.Visible = false;
        lblMessage.Text = "";
        ddlApplType.SelectedIndex = 0;
        ddlAppStatus.SelectedIndex = 0;
        ddlApplCriticality.SelectedIndex = 0;
        ddlApplManage.SelectedIndex = 0;
        this.txtOwner.Text = "";
        this.hdnOwnerID.Value = "0";
        hdnOwnerName.Value = "";
    }

    #endregion



}
