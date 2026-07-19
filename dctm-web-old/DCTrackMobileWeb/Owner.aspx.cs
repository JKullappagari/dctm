
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

public partial class Owner : System.Web.UI.Page
{
    #region "Declarations"

    private iAssetTrack.BAL.OwnerBAL objOwner;
    private iAssetTrack.BAL.DivisionBAL objDiv;
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
    private string tenantAssignedOwners= string.Empty;
    #endregion

    #region " Page Event Methods "

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        grdOwner.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(grdOwner_ItemCommand);
        pagerControl = grdOwner.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        if (pagerControl != null)
            pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);
    }
    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdOwner.Behaviors.Paging.PageIndex = e.PageNumber;
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
        Session["PageHeader"] = "Owner/Custodian";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        lblMessage.Visible = false;

        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "Owner.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = _dtRights != null && _dtRights.Select("Module = 'Owner' and Rights = '" + "View" + "'").Length != 0;

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }
        if (_dtRights.Select("Module = 'Owner' and Rights = '" + "Create" + "'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }

        if (_dtRights.Select("Module = 'Owner' and Rights = '" + "Delete" + "'").Length != 0)
        {
            ibDelete.Visible = true;
        }
        else
        {
            ibDelete.Visible = false;
        }



        this.grdOwner.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());
        populateGrid();
        if (!IsPostBack)
        {
            Session["OwnerID"] = null;

            int intUserID = 0;
            if (Session["UserID"] != null)
                intUserID = Convert.ToInt32(Session["UserID"].ToString());
            iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();
            DataTable dt = objCommon.FillDropDownListBU(StoredProcedures.SP_BUSINESSUNIT_LISTBYUSERID, "- Select -", intUserID);

            populateDivisionList(int.Parse(dt.Rows[1][0].ToString()));
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
            string sMessage = objCommon.displayMessage(MessageCodes.OWNER_JS_DELETE);
            hdnMessage.Value = sMessage;
        }
    }

    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        try
        {

            int intOwnerID = 0;
            objOwner.FirstName = txtOwnerFName.Text.Trim();
            objOwner.LastName = txtOwnerLName.Text.Trim();
            objOwner.Email = txtEmail.Text.Trim();

            if (Session["OwnerID"] != null)
            {
                objOwner.OwnerID = (int)Session["OwnerID"];
            }
            else
            {
                objOwner.OwnerID = objOwner.exists();
            }
            
            objOwner.OwnerID = Session["OwnerID"] == null ? intOwnerID : (int)Session["OwnerID"];
            intOwnerID = objOwner.exists();
            
            if (ddlDivison.SelectedIndex > 0)
                objOwner.DivisionID = int.Parse(ddlDivison.SelectedItem.Value.ToString());
            else
                objOwner.DivisionID = 0;

            objOwner.Status = 1;
            objOwner.CreatedBy = Convert.ToInt32(Session["UserID"]);

            if (intOwnerID != -1 && intOwnerID != 0)
                objOwner.OwnerID = intOwnerID;

            if (intOwnerID != -1)
            {
                objOwner.Persist(DALCOperation.Insert);
                clearFields();
                if (Session["OwnerID"] == null)
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
                grdOwner.ClearDataSource();
                populateGrid();
                Session["OwnerID"] = null;
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
        Session["OwnerID"] = null;
        txtOwnerFName.Focus();
        populateGrid();
    }

    protected void ibDelete_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        CheckBox chkDelete;
        int ownerID;
        string strIDs;

        strIDs = "";
        foreach (Infragistics.Web.UI.GridControls.GridRecord grdViewRow in grdOwner.Rows)
        {
            chkDelete = (CheckBox)(grdViewRow.Items[5].FindControl("chkDelete"));
            if (chkDelete.Checked == true)
            {
                ownerID = Convert.ToInt32(((Label)(grdViewRow.Items[5].FindControl("lblDeleteID"))).Text);
                strIDs += Convert.ToString(ownerID) + ",";
            }
        }

        if (strIDs != "")
        {
            strIDs = strIDs.Remove(strIDs.Length - 1, 1);
        }

        objOwner = new iAssetTrack.BAL.OwnerBAL();
        objOwner.OwnerIDs = strIDs;
        objOwner.Status = 0;
        objOwner.LastModifiedBy = Convert.ToInt32(Session["UserID"]);
        objOwner.Persist(DALCOperation.Delete);

        clearFields();

        objCommon = new CommonBAL();
        lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_DELETED);
        lblMessage.Visible = true;
        grdOwner.ClearDataSource();
        populateGrid();
    }

    protected void grdOwner_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            populateGrid();
            objOwner = new iAssetTrack.BAL.OwnerBAL();
            objOwner.OwnerID = Convert.ToInt32(e.CommandArgument);
            DataSet dsOwner = objOwner.retrieve();
            DataRow dr = dsOwner.Tables[0].Rows[0];
            txtOwnerFName.Text = dr[DBFields.DBFIELD_OWNER_FNAME].ToString();
            txtOwnerLName.Text = dr[DBFields.DBFIELD_OWNER_LNAME].ToString();
            txtEmail.Text = dr[DBFields.DBFIELD_OWNER_EMAIL].ToString();

            int intUserID = 0;
            if (Session["UserID"] != null)
                intUserID = Convert.ToInt32(Session["UserID"].ToString());
            iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();
            DataTable dt = objCommon.FillDropDownListBU(StoredProcedures.SP_BUSINESSUNIT_LISTBYUSERID, "- Select -", intUserID);

            populateDivisionList(int.Parse(dt.Rows[1][0].ToString()));

            if (ddlDivison.Items.Count > 1)
            {
                if (!string.IsNullOrEmpty(dr[DBFields.DBFIELD_DIVISION].ToString()))
                    ddlDivison.SelectedValue = dr[DBFields.DBFIELD_DIVISIONID].ToString();
                else
                    ddlDivison.SelectedIndex = 0;
            }

            Session["OwnerID"] = objOwner.OwnerID;

            if (_dtRights.Select("Module = 'Owner' and Rights = '" + "Modify" + "'").Length != 0)
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
            pagerControl.SetCurrentPageNumber(grdOwner.Behaviors.Paging.PageIndex);
        }
    }

    protected void ibExportToExcel_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        this.eExporter.DataExportMode = Infragistics.Web.UI.GridControls.DataExportMode.AllDataInDataSource;
        Infragistics.Documents.Excel.Workbook wBook = new Infragistics.Documents.Excel.Workbook(excelFormat);
        this.eExporter.Export(this.grdOwner, wBook);
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

    protected void grdOwner_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControl.SetupPageList(this.grdOwner.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdOwner.Behaviors.Paging.PageIndex);
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
            pagerControl.SetCurrentPageNumber(grdOwner.Behaviors.Paging.PageIndex);
            pagerControl.SetupPageList(this.grdOwner.Behaviors.Paging.PageCount);
        }
    }


    #endregion

    #region "User Defined Methods"



    private void FillDropDownListBU(string strStoredProc, ref DropDownList ddl, int id)
    {



    }


    private void populateGrid()
    {

        objOwner = new iAssetTrack.BAL.OwnerBAL();
        DataTable dtGrid = objOwner.retrieve().Tables[0];

        if (bool.Parse(Session["TenantUser"].ToString()))
        {
            tenantAssignedOwners = string.Empty;
            UserBAL objUser = new UserBAL();
            objUser.UserID = Convert.ToInt32(Session["UserID"]);
            DataSet dsTenant = objUser.retrieveTenantDetails();
            if (dsTenant.Tables.Count > 0 && dsTenant.Tables[0].Rows.Count > 0)
            {
                TenantBAL objTenant = new TenantBAL();
                objTenant.TenantId = Convert.ToInt32(dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_ID].ToString());
                DataSet dsTD = objTenant.retrieveOwnerAssignmentList();
                if (dsTD.Tables.Count > 0 && dsTD.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsTD.Tables[0].Rows)
                    {
                        tenantAssignedOwners = tenantAssignedOwners + dr[DBFields.DBFIELD_OWNER_ID].ToString() + ",";
                    }
                    tenantAssignedOwners = tenantAssignedOwners.Trim(',');

                }
                if (!string.IsNullOrEmpty(tenantAssignedOwners))
                {
                    DataTable tempTable = dtGrid.Clone();
                    foreach (DataRow dr in dtGrid.Rows)
                    {
                        tempTable.Rows.Add(dr.ItemArray);
                    }
                    DataRow[] filteredRows = tempTable.Select("OwnerID IN (" + tenantAssignedOwners + ")");
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
        grdOwner.DataSource = dtGrid;
        grdOwner.DataBind();

        grdOwner.Behaviors.Paging.Enabled = false;
        if (totalRecordCount == grdOwner.Rows.Count)
            this.FilterCount = "";
        grdOwner.Behaviors.Paging.Enabled = true;

        if (grdOwner.Rows.Count == 0)
        {
            grdOwner.DataSource = dtGrid;
            grdOwner.DataBind();
            grdOwner.Columns[5].Hidden = true;
            ibDelete.Visible = false;
        }
        else
        {
            if (_dtRights.Select("Rights = 'Delete' and Module = 'Owner'").Length != 0)
            {
                grdOwner.Columns[5].Hidden = false;
                ibDelete.Visible = true;
            }
            else
            {
                grdOwner.Columns[5].Hidden = true;
                ibDelete.Visible = false;
            }

            if (_dtRights.Select("Rights = 'Modify' and Module = 'Owner'").Length != 0)
            {
                grdOwner.Columns[4].Hidden = false;
            }
            else
            {
                grdOwner.Columns[4].Hidden = true;
            }
        }

        if (ibDelete.Visible == true)
        {
            int ownerID;
            int iCount = 0;

            for (int i = 0; i < grdOwner.Rows.Count; i++)
            {
                ownerID = Convert.ToInt32(((Label)(grdOwner.Rows[i].Items[5].FindControl("lblDeleteID"))).Text);

                objCommon = new iAssetTrack.BAL.CommonBAL();

                DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_OWNER_ID, ownerID.ToString(), 0);
                foreach (DataTable tblCheck in dsCheck.Tables)
                {
                    if (tblCheck.Rows[0][0].ToString() != "0")
                    {
                        grdOwner.Rows[i].Items[5].FindControl("chkDelete").Visible = false;
                    }
                }
                if (grdOwner.Rows[i].Items[5].FindControl("chkDelete").Visible == false)
                {
                    iCount += 1;
                }

            }
            if (iCount == grdOwner.Rows.Count)
            {
                grdOwner.Columns[5].Hidden = true;
                ibDelete.Visible = false;
            }
        }
        if (grdOwner.Rows.Count > 0)
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

    protected void grdOwner_DataFiltered(object sender, Infragistics.Web.UI.GridControls.FilteredEventArgs e)
    {
        // Cast the WebDataGrid DataSource to a DataTable  
        DataTable dt = this.grdOwner.DataSource as DataTable;

        // Disable WebDataGrid Paging
        grdOwner.Behaviors.Paging.Enabled = false;
        this.FilterCount = grdOwner.Rows.Count.ToString();
        if (!string.IsNullOrEmpty(this.FilterCount))
        {
            if (totalRecordCount == int.Parse(this.FilterCount))
            {
                this.FilterCount = "";
            }
        }
        // Enable WebDataGrid Paging
        grdOwner.Behaviors.Paging.Enabled = true;
    }


    private void populateDivisionList(int BusinessUnitID)
    {
        objDiv = new iAssetTrack.BAL.DivisionBAL();
        DataSet dsDiv = objDiv.retrieveByBusinessUnitId(BusinessUnitID);
        DataTable dtDiv = dsDiv.Tables[0];

        if (bool.Parse(Session["TenantUser"].ToString()))
        {
           string  tenantAssignedDivisions = string.Empty;
            UserBAL objUser = new UserBAL();
            objUser.UserID = Convert.ToInt32(Session["UserID"]);
            DataSet dsTenant = objUser.retrieveTenantDetails();
            if (dsTenant.Tables.Count > 0 && dsTenant.Tables[0].Rows.Count > 0)
            {
                TenantBAL objTenant = new TenantBAL();
                objTenant.TenantId = Convert.ToInt32(dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_ID].ToString());
                DataSet dsTD = objTenant.retrieveDivisionAssignmentList();
                if (dsTD.Tables.Count > 0 && dsTD.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drow in dsTD.Tables[0].Rows)
                    {
                        tenantAssignedDivisions = tenantAssignedDivisions + drow[DBFields.DBFIELD_DIVISIONID].ToString() + ",";
                    }
                    tenantAssignedDivisions = tenantAssignedDivisions.Trim(',');

                }
                if (!string.IsNullOrEmpty(tenantAssignedDivisions))
                {
                    DataTable tempTable = dtDiv.Clone();
                    foreach (DataRow drow in dtDiv.Rows)
                    {
                        tempTable.Rows.Add(drow.ItemArray);
                    }
                    DataRow[] filteredRows = tempTable.Select("DivisionID IN (" + tenantAssignedDivisions + ")");
                    dtDiv.Rows.Clear();
                    if (filteredRows != null && filteredRows.Length > 0)
                    {
                        foreach (DataRow drow in filteredRows)
                        {
                            dtDiv.Rows.Add(drow.ItemArray);
                        }

                    }
                }
                else
                {
                    dtDiv.Rows.Clear();
                }

            }

        }

        DataRow dr = dtDiv.NewRow();
        dr[0] = 0;
        dr[1] = "-Select-";
        dtDiv.Rows.InsertAt(dr, 0);

        ddlDivison.DataSource = dtDiv;
        ddlDivison.DataValueField = dtDiv.Columns[0].ToString();
        ddlDivison.DataTextField = dtDiv.Columns[1].ToString();
        ddlDivison.DataBind();
    }


    /// <summary>
    /// Reset fields
    /// </summary>    
    private void clearFields()
    {
        txtOwnerFName.Text = "";
        txtOwnerLName.Text = "";
        txtEmail.Text = "";
        lblMessage.Visible = false;
        lblMessage.Text = "";
        ddlDivison.SelectedIndex = 0;
    }

    #endregion



}
