
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
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

public partial class Tenant : System.Web.UI.Page
{
    #region "Declarations"

    private iAssetTrack.BAL.TenantBAL objTenant;
    private iAssetTrack.BAL.BusinessUnitBAL objBU;
    private iAssetTrack.BAL.SitesBAL objSite;
    private iAssetTrack.BAL.LocationBAL objLocation;
    private iAssetTrack.BAL.CommonBAL objCommon;
    DataTable _dtRights;
    protected int totalRecordCount = 0;
    private iAssetTrack_WebDataGrid_Paging_CustomerPagerControl pagerControl;
    private const string PROP_FILTERCOUNT = "FilteredCount";
    bool isImplicitChange = false;
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

        grdTenant.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(grdTenant_ItemCommand);
        pagerControl = grdTenant.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        if (pagerControl != null)
            pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);
    }
    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdTenant.Behaviors.Paging.PageIndex = e.PageNumber;
        populateGrid();
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        populateGrid();
    }
    /// <summary>
    /// Used to load the page.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageHeader"] = "Define Tenant";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        lblMessage.Visible = false;

        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "Tenant.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = _dtRights != null && _dtRights.Select("Module = 'Tenant' and Rights = '" + "View" + "'").Length != 0;

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }
        if (_dtRights.Select("Module = 'Tenant' and Rights = '" + "Create" + "'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }

        if (_dtRights.Select("Module = 'Tenant' and Rights = '" + "Delete" + "'").Length != 0)
        {
            ibDelete.Visible = true;
        }
        else
        {
            ibDelete.Visible = false;
        }

     

        this.grdTenant.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());
        populateGrid();
        if (!IsPostBack)
        {
            Session["TenantId"] = null;
            populateLocationTypes();
            if (Application["companyname"] != null)
            {
                BusinessUnitBAL objBU = new BusinessUnitBAL();
                DataSet dsBU = objBU.retrieve();
                if(dsBU.Tables.Count > 0 && dsBU.Tables[0].Rows.Count > 0)
                {
                    if(Application["companyname"].ToString().ToLower().CompareTo(dsBU.Tables[0].Rows[0][DBFields.DBFIELD_BUSINESSUNIT].ToString().ToLower()) != 0)
                    {
                        ShowMessageBox("Tenant license is not intended for this customer");
                    }
                }
                LoadLicenseData();
            }
            else
            {
                ShowMessageBox("Tenant license details not found");
            }

        }
        else
        {
            //if(!isImplicitChange)
            //{
            ReloadCheckedLocations();
            //}
        }
    }

    private void LoadLicenseData()
    {
        //Check tenant functionality license information
        if (Application["licensemode"] != null)
        {
            if (Application["licensemode"] != null && !string.IsNullOrEmpty(Application["licensemode"].ToString()))
            {
                if (Application["licensemode"].ToString().ToLower().CompareTo("date") == 0)
                {
                    DateTime dt = DateTime.ParseExact(Application["ValidThru"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces);
                    if (DateTime.Now.Date <= dt.Date)
                    {
                        Session["NoofTenants"] = Application["TenantCount"];
                    }
                    else
                    {
                        ShowMessageBox("Tenant configuration license is expired");
                    }
                }
                else if(Application["licensemode"].ToString().ToLower().CompareTo("perpetual") == 0)
                {
                    //do nothing
                }
                else
                {
                    ShowMessageBox("Some of the tenant license details are missing or incorrect");
                }
            }
            else
            {
                ShowMessageBox("Some of the tenant license details are missing");
            }
        }
    }

    private void ShowMessageBox(string Message)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("alert(\'");
        sb.Append(Message);
        sb.Append("\');");
        sb.Append("document.location = \'MainPage.aspx\';");

        var script = HttpUtility.JavaScriptStringEncode(sb.ToString(), false);
        ClientScript.RegisterStartupScript(this.GetType(), "tenantError", HttpUtility.HtmlDecode(script).Replace("\\u0027", "'"), true);
    }

    /// <summary>
    /// Used to call once page loading complete.
    /// </summary>
    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        if (ibDelete.Enabled)
        {
            iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();
            string sMessage = objCommon.displayMessage(MessageCodes.TENANT_JS_DELETE);
            hdnMessage.Value = sMessage;
        }
    }

    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        try
        {
            int selCount = 0;
            bool isRowParentExists = false;
            if (Session["TenantId"] == null)
            {
                TenantBAL objT = new TenantBAL();
                DataSet ds = objT.retrieve();
                if (ds.Tables.Count > 0  && ds.Tables[0].Rows.Count > 0)
                {
                    if(ds.Tables[0].Rows.Count == int.Parse(Session["NoofTenants"].ToString()))
                    {
                        lblMessage.Text = "Maxmimum number of tenants allowed for this license is reached";
                        lblMessage.Visible = true;
                        return;
                    }
                }
            }

            if (Session["TenantId"] != null && Convert.ToInt32(Session["TenantId"].ToString()) > 0)
            {
                string tenantType = ddlTenantType.SelectedItem.Text;
                foreach (DataTreeNode node in wdtLocSelection.CheckedNodes)
                {
                    if (node.Text.ToLower().Split('-')[0].ToLower().CompareTo(tenantType.ToLower()) == 0)
                        selCount++;
                }
            }
            else
            {
                foreach (DataTreeNode node in wdtLocSelection.CheckedNodes)
                {
                    if (ddlTenantType.SelectedItem.Text.ToLower().CompareTo("rack") == 0)
                    {
                        string locType = node.Text.ToLower().Split('-')[0].ToLower();
                        if (locType.CompareTo("row") == 0)
                        {
                            isRowParentExists = true;
                            //row
                            //TextBox ctrlTextBox = (TextBox) phTypeSize.FindControl("txt" + node.Value.ToString());
                            TextBox ctrlTextBox = (this.Form.FindControl("Master_ContentPlaceHolder").FindControl("phTypeSize").FindControl("dtxt" + node.Value.ToString()) as TextBox);
                            if (ctrlTextBox != null)
                            {
                                if (!String.IsNullOrEmpty(ctrlTextBox.Text))
                                    selCount = selCount + int.Parse(ctrlTextBox.Text);
                            }

                        }
                        else
                        {
                            //rack
                            selCount++;
                        }
                    }
                    else
                    {
                        selCount++;
                    }
                }
            }

            if (!string.IsNullOrEmpty(hdnLocCount.Value))
            {
                foreach (string str in hdnLocCount.Value.Split(','))
                {
                    if (str.Contains("#"))
                    {
                        selCount = selCount + Convert.ToInt32(str.Split('#')[1]);
                    }

                }
            }

            if (int.Parse(txtSize.Text) != selCount)
            {
                lblMessage.Text = "Tenant type size defined and number of locations specified is not matching";
                lblMessage.Visible = true;
                return;
            }

            //Get Permmission list for Admins and users groups
            objTenant.AdminPermissions = GetGroupPermissions("Admins");
            objTenant.UserPermissions = GetGroupPermissions("Users");

            int intTenantId = 0;
            objTenant.FullName = txtFullName.Text.Trim();
            objTenant.ShortName = txtShortName.Text.Trim();
            objTenant.TenantType = ddlTenantType.SelectedItem.Value.ToString();
            objTenant.TenantTypeSize = int.Parse(txtSize.Text.Trim());
            objTenant.TenantUserCount = int.Parse(txtTotalUsers.Text.Trim());


            ArrayList assignedLocs = new ArrayList();
            hdnLocCount.Value = hdnLocCount.Value.Replace("dtxt", "");
            foreach (DataTreeNode item in wdtLocSelection.CheckedNodes)
            {
                //if (!hdnLocCount.Value.Contains(item.Value) && ddlTenantType.SelectedItem.Text.ToLower().CompareTo(item.Text.Split('-')[0].ToLower()) == 0)
                //{
                if (!string.IsNullOrEmpty(hdnLocCount.Value))
                {
                    if (!hdnLocCount.Value.Contains(item.Value.ToString()))
                        assignedLocs.Add(item.Value.ToString());

                }
                else
                {
                    assignedLocs.Add(item.Value.ToString());
                }


                //}
            }
            if (!string.IsNullOrEmpty(hdnLocCount.Value))
            {
                foreach (string str in hdnLocCount.Value.Split(','))
                {
                    assignedLocs.Add(str);
                }
            }

            objTenant.TenantAssignedLocations = string.Join(",", assignedLocs.ToArray());
            objTenant.ContactFirstName = txtFirstName.Text.Trim();
            objTenant.ContactLastName = txtLastName.Text.Trim();
            objTenant.ContactEmail = txtEmail.Text.Trim();


            if (Session["TenantId"] != null)
            {
                objTenant.TenantId = (int)Session["TenantId"];
            }
            else
            {
                objTenant.TenantId = objTenant.exists();
            }

            objTenant.TenantId = Session["TenantId"] == null ? intTenantId : (int)Session["TenantId"];
            intTenantId = objTenant.exists();

            if (ddlTenantType.SelectedIndex > 0)
                objTenant.TenantType = ddlTenantType.SelectedItem.Text;


            objTenant.Status = 1;
            objTenant.CreatedBy = Convert.ToInt32(Session["UserID"]);

            if (intTenantId != -1 && intTenantId != 0)
                objTenant.TenantId = intTenantId;

            if (intTenantId != -1)
            {
                objTenant.Persist(DALCOperation.Insert);
                clearFields();
                if (Session["TenantId"] == null)
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
                grdTenant.ClearDataSource();
                populateGrid();
                Session["TenantId"] = null;
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
        Session["TenantId"] = null;
        txtFullName.Focus();
        populateGrid();
    }

    protected void ibDelete_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        CheckBox chkDelete;
        int ownerID;
        string strIDs;

        strIDs = "";
        foreach (Infragistics.Web.UI.GridControls.GridRecord grdViewRow in grdTenant.Rows)
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

        objTenant = new iAssetTrack.BAL.TenantBAL();
        objTenant.TenantIds = strIDs;
        objTenant.Status = 0;
        objTenant.LastModifiedBy = Convert.ToInt32(Session["UserID"]);
        objTenant.Persist(DALCOperation.Delete);

        clearFields();

        objCommon = new CommonBAL();
        lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_DELETED);
        lblMessage.Visible = true;
        grdTenant.ClearDataSource();
        populateGrid();
    }

    protected void grdTenant_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            populateGrid();
            objTenant = new iAssetTrack.BAL.TenantBAL();
            objTenant.TenantId = Convert.ToInt32(e.CommandArgument);
            DataSet dsTenant = objTenant.retrieve();
            DataRow dr = dsTenant.Tables[0].Rows[0];
            txtFullName.Text = dr[DBFields.DBFIELD_TENANT_FULL_NAME].ToString();
            txtShortName.Text = dr[DBFields.DBFIELD_TENANT_SHORT_NAME].ToString();
            txtSize.Text = dr[DBFields.DBFIELD_TENANT_TYPE_SIZE].ToString();
            txtTotalUsers.Text = dr[DBFields.DBFIELD_TENANT_USER_COUNT].ToString();
            txtFirstName.Text = dr[DBFields.DBFIELD_TENANT_CONTANCT_FNAME].ToString();
            txtLastName.Text = dr[DBFields.DBFIELD_TENANT_CONTANCT_LNAME].ToString();
            txtEmail.Text = dr[DBFields.DBFIELD_TENANT_CONTANCT_EMAIL].ToString();
            Session["TenantId"] = objTenant.TenantId;

            populateLocationTypes();

            if (ddlTenantType.Items.Count > 1)
            {
                if (!string.IsNullOrEmpty(dr[DBFields.DBFIELD_TENANT_TYPE].ToString()))
                {
                    CompareItem(ddlTenantType, dr[DBFields.DBFIELD_TENANT_TYPE].ToString());
                }
            }

            LoadLocations(ddlTenantType.SelectedItem.Text);

            string assignedLocs = dr[DBFields.DBFIELD_TENANT_ASSIGNEDLOCATIONS].ToString();
            //Assigned Locations Selection
            foreach (string locId in assignedLocs.Split(','))
            {
                AddLocation(int.Parse(locId), false);
            }
            foreach (DataTreeNode node in wdtLocSelection.AllNodes)
            {
                if (assignedLocs.Contains(node.Value.ToString()))
                {
                    node.CheckState = Infragistics.Web.UI.CheckBoxState.Checked;
                    //only Tenant type related nodes will be checked and in enabled state
                    //others will be disabled. Example: Tenant Type is Row than Row and its child (Racks) will be checked but Rack nodes will be disabled
                    // so that user can't make any change for these nodes. If Parent (Row) is unchecked than all child (Racks) will be unchecked by the system.
                    if (node.Text.ToLower().Split('-')[0].ToLower().CompareTo(ddlTenantType.SelectedItem.Text.ToLower()) != 0)
                        node.Enabled = false;

                }
            }
            //
            // disable few controls like Full Name, Short Name and tenant type for Edit operation.
            enableDisableCotrolsForEdit(false);
            if (_dtRights.Select("Module = 'Tenant' and Rights = '" + "Modify" + "'").Length != 0)
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
            pagerControl.SetCurrentPageNumber(grdTenant.Behaviors.Paging.PageIndex);
        }
    }

    protected void ibExportToExcel_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        this.eExporter.DataExportMode = Infragistics.Web.UI.GridControls.DataExportMode.AllDataInDataSource;
        Infragistics.Documents.Excel.Workbook wBook = new Infragistics.Documents.Excel.Workbook(excelFormat);
        this.eExporter.Export(this.grdTenant, wBook);
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

    protected void grdTenant_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControl.SetupPageList(this.grdTenant.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdTenant.Behaviors.Paging.PageIndex);
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
            pagerControl.SetCurrentPageNumber(grdTenant.Behaviors.Paging.PageIndex);
            pagerControl.SetupPageList(this.grdTenant.Behaviors.Paging.PageCount);
        }
    }


    #endregion

    #region "User Defined Methods"

    private void populateGrid()
    {

        objTenant = new iAssetTrack.BAL.TenantBAL();
        DataTable dtGrid = objTenant.retrieve().Tables[0];
        totalRecordCount = dtGrid.Rows.Count;
        grdTenant.DataSource = dtGrid;
        grdTenant.DataBind();

        grdTenant.Behaviors.Paging.Enabled = false;
        if (totalRecordCount == grdTenant.Rows.Count)
            this.FilterCount = "";
        grdTenant.Behaviors.Paging.Enabled = true;

        if (grdTenant.Rows.Count == 0)
        {
            grdTenant.DataSource = dtGrid;
            grdTenant.DataBind();
            grdTenant.Columns[5].Hidden = true;
            ibDelete.Visible = false;
        }
        else
        {
            if (_dtRights.Select("Rights = 'Delete' and Module = 'Owner'").Length != 0)
            {
                grdTenant.Columns[5].Hidden = false;
                ibDelete.Visible = true;
            }
            else
            {
                grdTenant.Columns[5].Hidden = true;
                ibDelete.Visible = false;
            }

            if (_dtRights.Select("Rights = 'Modify' and Module = 'Owner'").Length != 0)
            {
                grdTenant.Columns[4].Hidden = false;
            }
            else
            {
                grdTenant.Columns[4].Hidden = true;
            }
        }

        if (ibDelete.Visible == true)
        {
            int ownerID;
            int iCount = 0;

            for (int i = 0; i < grdTenant.Rows.Count; i++)
            {
                ownerID = Convert.ToInt32(((Label)(grdTenant.Rows[i].Items[5].FindControl("lblDeleteID"))).Text);

                objCommon = new iAssetTrack.BAL.CommonBAL();

                DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_OWNER_ID, ownerID.ToString(), 0);
                foreach (DataTable tblCheck in dsCheck.Tables)
                {
                    if (tblCheck.Rows[0][0].ToString() != "0")
                    {
                        grdTenant.Rows[i].Items[5].FindControl("chkDelete").Visible = false;
                    }
                }
                if (grdTenant.Rows[i].Items[5].FindControl("chkDelete").Visible == false)
                {
                    iCount += 1;
                }

            }
            if (iCount == grdTenant.Rows.Count)
            {
                grdTenant.Columns[5].Hidden = true;
                ibDelete.Visible = false;
            }
        }
        if (grdTenant.Rows.Count > 0)
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

    protected void grdTenant_DataFiltered(object sender, Infragistics.Web.UI.GridControls.FilteredEventArgs e)
    {
        // Cast the WebDataGrid DataSource to a DataTable  
        DataTable dt = this.grdTenant.DataSource as DataTable;

        // Disable WebDataGrid Paging
        grdTenant.Behaviors.Paging.Enabled = false;
        this.FilterCount = grdTenant.Rows.Count.ToString();
        if (!string.IsNullOrEmpty(this.FilterCount))
        {
            if (totalRecordCount == int.Parse(this.FilterCount))
            {
                this.FilterCount = "";
            }
        }
        // Enable WebDataGrid Paging
        grdTenant.Behaviors.Paging.Enabled = true;
    }


    private void populateLocationTypes()
    {
        iAssetTrack.BAL.LocationTypeBAL objLocType = new iAssetTrack.BAL.LocationTypeBAL();
        DataSet dsLocType = objLocType.retrieve();
        DataTable dtLocType = dsLocType.Tables[0];
        DataRow dr = dsLocType.Tables[0].NewRow();
        dr[0] = 0;
        dr[1] = "-Select-";
        dtLocType.Rows.InsertAt(dr, 0);

        ddlTenantType.DataSource = dtLocType;
        ddlTenantType.DataValueField = dtLocType.Columns[0].ToString();
        ddlTenantType.DataTextField = dtLocType.Columns[1].ToString();
        ddlTenantType.DataBind();
    }


    /// <summary>
    /// Reset fields
    /// </summary>    
    private void clearFields()
    {
        enableDisableCotrolsForEdit(true);
        txtFullName.Text = "";
        txtShortName.Text = "";
        txtSize.Text = "";
        ddlTenantType.SelectedIndex = 0;
        txtTotalUsers.Text = "";
        txtFirstName.Text = "";
        txtLastName.Text = "";
        txtEmail.Text = "";
        wdtLocSelection.ClearSelection();
        lblMessage.Visible = false;
        lblMessage.Text = "";
        wdtLocSelection.Nodes.Clear();
        hdnLocCount.Value = string.Empty;
        phTypeSize.Controls.Clear();

    }

    private void enableDisableCotrolsForEdit(bool OnOff)
    {
        txtFullName.Enabled = OnOff;
        txtShortName.Enabled = OnOff;
        ddlTenantType.Enabled = OnOff;
    }

    private void LoadLocations(string LocationType)
    {

        objTenant = new iAssetTrack.BAL.TenantBAL();
        DataSet dsTenantLoc = objTenant.retrieveByTenantType(LocationType);
        DataTable dtLoc = dsTenantLoc.Tables[0];

        WebDataTree loctree = (WebDataTree)this.wdtLocSelection;
        loctree.Nodes.Clear();
        using (DataTableReader dtLocReader = dtLoc.CreateDataReader())
        {
            while (dtLocReader.Read())
            {

                DataTreeNode locNode = new DataTreeNode();
                if (string.IsNullOrEmpty(dtLocReader.GetValue(2).ToString()))
                    locNode.Text = dtLocReader.GetValue(3).ToString() + "-" + dtLocReader.GetValue(1).ToString();
                else
                    locNode.Text = dtLocReader.GetValue(3).ToString() + "-" + dtLocReader.GetValue(1).ToString() + "(" + dtLocReader.GetValue(2).ToString() + ")";
                locNode.Value = dtLocReader.GetValue(0).ToString();
                loctree.Nodes.Add(locNode);
            }

        }
    }

    private void AddLocation(int locationId, bool CheckState)
    {
        string path = string.Empty;
        string site = string.Empty;
        LocationBAL objLocation = new iAssetTrack.BAL.LocationBAL();
        objLocation.LocationID = locationId;
        DataSet dsLocation = objLocation.retrieve();
        DataTable dtLoc = dsLocation.Tables[0];

        SiteLocationAssignmentBAL objSL = new SiteLocationAssignmentBAL();
        DataSet dsSL = objSL.retrieve();
        if (dsSL.Tables.Count > 0 && dsSL.Tables[0].Rows.Count > 0)
        {
            DataRow[] rows = dsSL.Tables[0].Select("LocationID = " + locationId.ToString());
            if (rows != null && rows.Length > 0)
            {
                site = rows[0][DBFields.DBFIELD_SITE].ToString();
            }
        }

        if (string.IsNullOrEmpty(dtLoc.Rows[0]["Path"].ToString()))
            path = site + " > " + dtLoc.Rows[0]["Location"].ToString();
        else
            path = site + " > " + dtLoc.Rows[0]["Path"].ToString();

        WebDataTree loctree = (WebDataTree)this.wdtLocSelection;
        DataTreeNode locNode = new DataTreeNode();

        locNode.Text = dtLoc.Rows[0]["LocationType"].ToString() + "-" + dtLoc.Rows[0]["Location"].ToString() + "(" + path + ")";
        locNode.Value = dtLoc.Rows[0]["LocationId"].ToString();
        if (CheckState)
        {
            locNode.CheckState = Infragistics.Web.UI.CheckBoxState.Checked;
            locNode.Enabled = false;
        }
        loctree.Nodes.Insert(0, locNode);
    }


    private void getChildLocations(DataTreeNode node, int LocationID)
    {
        DataSet dsLocationbyLocation = objLocation.GetLocationBYLocation(LocationID);
        DataTable dtLocationbyLocation = dsLocationbyLocation.Tables[0];

        using (DataTableReader dtLocationbyLocationRdr = dtLocationbyLocation.CreateDataReader())
        {
            while (dtLocationbyLocationRdr.Read())
            {
                DataTreeNode subLocationNode = new DataTreeNode();
                subLocationNode.Text = dtLocationbyLocationRdr.GetValue(1).ToString();
                subLocationNode.Value = dtLocationbyLocationRdr.GetValue(0).ToString();
                node.Nodes.Add(subLocationNode);
                node.Expanded = true;
                if (int.Parse(dtLocationbyLocationRdr.GetValue(5).ToString()) > 0) //Child location count
                    getChildLocations(subLocationNode, Convert.ToInt32(dtLocationbyLocationRdr.GetValue(0).ToString()));
            }
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

    #endregion

    protected void ddlTenantType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadLocations(ddlTenantType.SelectedItem.Text);
    }

    protected void wdtLocSelection_CheckBoxSelectionChanged(object sender, DataTreeCheckBoxSelectionEventArgs e)
    {
        ArrayList alUnCheckedNodes = new ArrayList();
        ArrayList alCheckedNodes = new ArrayList();
        ArrayList alChildLocs = new ArrayList();
        string tenantType = string.Empty;
        if (Session["TenantId"] != null && Convert.ToInt32(Session["TenantId"].ToString()) > 0)
        {
            foreach (DataTreeNode node in e.OldCheckedNodes)
            {
                if (!alUnCheckedNodes.Contains(node.Value))
                    alUnCheckedNodes.Add(node.Value);

                if (!alCheckedNodes.Contains(node.Value))
                    alCheckedNodes.Add(node.Value);
            }
            foreach (DataTreeNode newNode in e.NewCheckedNodes)
            {
                //OldCheckedNodes consist of all ceckeded nodels before slection change event
                //newCheckedNodes consists of only checked nodes after change
                //example: old has 3 and new has 2 only than one node is unchecked
                //example2: old has 3 and new has 4 than 1 new node is checked.
                if (alUnCheckedNodes.Contains(newNode.Value))
                    alUnCheckedNodes.Remove((object)newNode.Value);
                //newly checked nodes
                if (alCheckedNodes.Contains(newNode.Value))
                    alCheckedNodes.Remove((object)newNode.Value);
                else
                    alCheckedNodes.Add((object)newNode.Value);
            }

            //remove unchecked nodes from checked node list
            foreach (string val in alUnCheckedNodes)
            {
                if (alCheckedNodes.Contains(val))
                    alCheckedNodes.Remove((object)val);
            }
            //contains unchecked nodes only.
            //get all child locations
            tenantType = ddlTenantType.SelectedItem.Text;
            if (tenantType.ToLower().CompareTo("room") == 0 || tenantType.ToLower().CompareTo("row") == 0)
            {
                //unchecked nodes
                foreach (string locId in alUnCheckedNodes)
                {
                    objTenant.TenantId = Convert.ToInt32(Session["TenantId"].ToString());
                    DataSet dsLocs = objTenant.retrieveLocationAssignmentList(Convert.ToInt32(locId));
                    if (dsLocs.Tables.Count > 0 && dsLocs.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsLocs.Tables[0].Rows)
                        {
                            if (Convert.ToInt32(dr[DBFields.DBFIELD_LOCATIONID].ToString()) != Convert.ToInt32(locId))
                            {
                                wdtLocSelection.Nodes.Remove(wdtLocSelection.Nodes.FindNodeByValue(dr[DBFields.DBFIELD_LOCATIONID].ToString()));
                            }
                        }
                    }


                }
                //Checked nodes
                foreach (string locId in alCheckedNodes)
                {
                    objTenant.TenantId = Convert.ToInt32(Session["TenantId"].ToString());
                    DataSet dsLocs = objTenant.retrieveLocationAssignmentList(Convert.ToInt32(locId));
                    if (dsLocs.Tables.Count > 0 && dsLocs.Tables[0].Rows.Count > 0)
                    {
                        //already existing parent node unchecked and checked again that onl child loction list must be loaded
                        foreach (DataRow dr in dsLocs.Tables[0].Rows)
                        {
                            if (Convert.ToInt32(dr[DBFields.DBFIELD_LOCATIONID].ToString()) != Convert.ToInt32(locId))
                            {
                                AddLocation(Convert.ToInt32(dr[DBFields.DBFIELD_LOCATIONID].ToString()), true);
                            }
                        }
                    }


                }

            }


        }
        //isImplicitChange = true;
        //ReloadCheckedLocations();
    }

    private void ReloadCheckedLocations()
    {
        if (ddlTenantType.SelectedItem.Text.ToLower().CompareTo("rack") == 0)
        {
            foreach (DataTreeNode node in wdtLocSelection.CheckedNodes)
            {
                if (node.Text.ToLower().Split('-')[0].CompareTo("row") == 0)
                {
                    Label title = new Label();
                    title.Text = node.Text.ToLower().Split('(')[0].Trim().ToUpper();
                    title.ID = "lbl" + node.Value.ToString();
                    title.Width = new Unit("200px");

                    TextBox val = new TextBox();
                    val.Text = "";
                    val.Width = new Unit("30px");
                    val.MaxLength = 3;
                    val.ID = "dtxt" + node.Value.ToString();
                    val.Attributes.Add("onkeyup", "addCounts(this);");
                    RegularExpressionValidator rev = new RegularExpressionValidator();
                    rev.ID = "rev" + node.Value.ToString();
                    rev.ControlToValidate = "dtxt" + node.Value.ToString();
                    rev.CssClass = "ErrValStyle";
                    rev.ValidationExpression = "[0-9]";
                    rev.Display = ValidatorDisplay.Dynamic;
                    rev.ErrorMessage = "*";

                    Label dummy = new Label();
                    dummy.Text = "";
                    dummy.ID = "lblD" + node.Value.ToString();
                    dummy.Height = new Unit("6px");

                    phTypeSize.Controls.Add(title);
                    phTypeSize.Controls.Add(val);
                    phTypeSize.Controls.Add(rev);
                    phTypeSize.Controls.Add(new LiteralControl("<br>"));
                    phTypeSize.Controls.Add(dummy);
                    phTypeSize.Controls.Add(new LiteralControl("<br>"));
                }
            }
        }
    }

    private string GetGroupPermissions(string GroupName)
    {
        ArrayList alPerms = new ArrayList();
        string xmlFileName = HttpContext.Current.Request.PhysicalApplicationPath + "\\Tenant\\" + "ModuleRights.xml";

        GroupModuleRightBAL objGRM = new GroupModuleRightBAL();
        DataSet dsRights = objGRM.retrieveRights();

        StringBuilder result = new StringBuilder();
        foreach (XElement grp in XElement.Load(xmlFileName).Elements("ModuleRightsGroup"))
        {
            if (grp.Attribute("Group").Value.ToLower().CompareTo(GroupName.ToLower()) == 0)
            {
                foreach (XElement mainmodule in grp.Elements("MainModule"))
                {
                    foreach (XElement module in mainmodule.Elements("Module"))
                    {
                        string perm = string.Empty;
                        string perms = string.Empty;
                        perm = module.Attribute("name").Value;
                        foreach (XElement rights in module.Elements("Rights"))
                        {
                            string permVal = rights.Attribute("Description").Value;
                            List<int> rightsVal = (from r in dsRights.Tables[0].AsEnumerable()
                                                   where r.Field<string>("Description").ToLower().CompareTo(permVal.ToLower()) == 0
                                                   select r.Field<int>("RightsId")).ToList();

                            if (rightsVal.Count > 0)
                                perms = perms + rightsVal[0].ToString() + ",";
                        }
                        alPerms.Add(perm + "#" + perms.Trim(','));
                    }
                }
            }
        }

        return string.Join(";", alPerms.ToArray());
    }
}
