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
using Infragistics.WebUI.WebDataInput;
using iAssetTrack.DALC;
using System.Data.SqlClient;
using iAssetTrackBAL;

public partial class ASPX_UserSearch : System.Web.UI.Page
{
    #region Declarations

    private iAssetTrack.BAL.ManageUsersBAL objUser;
    private iAssetTrack.BAL.CommonBAL objCommon;
    DataTable _dtRights;
    private iAssetTrack_WebDataGrid_Paging_CustomerPagerControl pagerControl;
    //private const string GRID_COL_KEY_RFIDTAGID = "RFIDTagID";
    //private const string GRID_COL_KEY_RFIDTAGICON = "RFIDTagIcon";
    //private const string RFID_IMAGE = "images/label_16x16.gif";
    private string tenantAssignedGroups = string.Empty;
    #endregion

    #region Page load events


    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        grdUser.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(grdUser_ItemCommand);
        pagerControl = grdUser.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);
    }
    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdUser.Behaviors.Paging.PageIndex = e.PageNumber;
        populateGrid();
    }
    protected void grdUser_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControl.SetupPageList(this.grdUser.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdUser.Behaviors.Paging.PageIndex);
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
            pagerControl.SetupPageList(this.grdUser.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdUser.Behaviors.Paging.PageIndex);
        }


    }

    /// <summary>
    /// Checking user rights and setting pagination to data grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageHeader"] = "User Search";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        lblMessage.Visible = false;

        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "UserSearch.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        if (_dtRights.Select("Module = 'User Search' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }

        populateGrid();



        this.grdUser.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());




        //if (_dtRights.Select("Module = 'User Search' and Rights = '" + "Reset Password" + "'").Length != 0)
        //    uwgUsers.Columns.FromKey("ResetPassword").Hidden = false;
        //else
        //    uwgUsers.Columns.FromKey("ResetPassword").Hidden = true;        

        this.txtSearch.Focus();
        //Form.DefaultButton = this.wibSearch.UniqueID;
        //Page.SetFocus(txtSearch);
        //Page.Form.DefaultButton = wibSearch.UniqueID;
        /*
        if (!string.IsNullOrEmpty(txtSearch.Text))
        {
            uwgUsers.DisplayLayout.Pager.CurrentPageIndex = 1;
            this.doBuild(0);
        }
        */
        if (!IsPostBack)
        {
            //if (Session["@@LoginName"] != null)
            //{
            //    //this.txtSearch.Text = Session["@@LoginName"].ToString();
            //    this.txtSearch.Text = Session["@@SearchText"].ToString();
            //    this.doBuild(0);
            //    //Session["@@LoginName"] = null;
            //    //Session["@@SearchText"] = null;
            //}
        }
    }


    protected void grdUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //grdBU.Pag = e.NewPageIndex;
        //populateGrid();
    }


    protected void grdUser_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }

    protected void grdUser_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            Context.Items.Add("@@UserID", e.CommandArgument.ToString());
            Session["@@UserID"] = e.CommandArgument.ToString();
            Server.Transfer("ManageUsers.aspx");
            //Response.Redirect("ManageUsers.aspx");
        }
        else if (e.CommandName == "Assign")
        {

            UserBAL objUser = new UserBAL();
            objUser.UserID = Convert.ToInt32(e.CommandArgument.ToString());
            DataTable dtUserSearch = objUser.retrieve().Tables[0];
            lblMessage.Visible = false;

            if (dtUserSearch.Rows[0]["Status"].ToString().Equals("Active"))
            {
                Session["@@UserID"] = dtUserSearch.Rows[0]["UserID"].ToString();
                Session["@@LoginName"] = dtUserSearch.Rows[0]["LoginName"].ToString();
                Session["@@Name"] = dtUserSearch.Rows[0]["Name"].ToString();
                Session["@@SearchText"] = this.txtSearch.Text.Trim();
                Response.Redirect("AssignRights.aspx");
            }
            else
                this.ShowMessage("User Status Disabled. Not allowed to Assign Rights");
            //Response.Write("<script language=javascript>alert('User Status Disabled. Not allowed to Assign Rights');</script>");

        }
        else if (e.CommandName == "Page")
        {
            pagerControl.SetCurrentPageNumber(grdUser.Behaviors.Paging.PageIndex);
        }
        //Added on 4-July-2012-V00001

        else if (e.CommandName == "Delete")
        {

            objUser = new ManageUsersBAL();
            objUser.UserID = Convert.ToInt16(e.CommandArgument);
            objUser.LastModifiedBy = Convert.ToInt32(Session["UserID"]);
            objUser.IsDeleted = 1;
            objUser.Status = 0;
            objUser.Persist(DALCOperation.Delete);

            objCommon = new CommonBAL();
            lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_DELETED);
            lblMessage.Visible = true;
            grdUser.ClearDataSource();
            populateGrid();
        }
    }

    //protected void ibExportToExcel_Click(object sender, EventArgs e)
    //{
    //    Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
    //    this.eExporter.DataExportMode = Infragistics.Web.UI.GridControls.DataExportMode.AllDataInDataSource;
    //    Infragistics.Documents.Excel.Workbook wBook = new Infragistics.Documents.Excel.Workbook(excelFormat);
    //    this.eExporter.Export(this.grdBU, wBook);
    //}
    //protected void eExporter_CellExported(object sender, Infragistics.Web.UI.GridControls.ExcelCellExportedEventArgs e)
    //{
    //    int iWSdex = e.Worksheet.Index;
    //    int iRdex = e.CurrentRowIndex;
    //    int iCdex = e.CurrentColumnIndex;
    //    e.Worksheet.Columns[3].Width = 1;
    //    e.Worksheet.Columns[5].Width = 1;
    //    if (iWSdex == 0)
    //    {
    //        if (iRdex == 0)
    //        {

    //            if (iCdex == 3 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
    //            {
    //                e.Worksheet.Rows[iRdex].Cells[iCdex].Value = "";


    //            }
    //            if (iCdex == 4 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
    //            {
    //                e.Worksheet.Rows[iRdex].Cells[iCdex].Value = "";


    //            }

    //        }

    //    }
    //}


    protected void grdUser_InitializeRow(object sender, Infragistics.Web.UI.GridControls.RowEventArgs e)
    {
        //
        //
    }



    //protected void uwgUsers_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    //{
    //    //if (e.Row.Cells.FromKey(GRID_COL_KEY_RFIDTAGID).Value != null)
    //    //{
    //    //    e.Row.Cells.FromKey(GRID_COL_KEY_RFIDTAGICON).Style.CustomRules = "Background-repeat:no-repeat";
    //    //    e.Row.Cells.FromKey(GRID_COL_KEY_RFIDTAGICON).Style.BackgroundImage = RFID_IMAGE;
    //    //    e.Row.Cells.FromKey(GRID_COL_KEY_RFIDTAGICON).Title = e.Row.Cells.FromKey(GRID_COL_KEY_RFIDTAGID).Text;
    //    }
    //}

    #endregion

    protected void wibSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        //this.doBuild(0);
    }


    private void populateGrid()
    {
        objUser = new ManageUsersBAL();
        objUser.Search = txtSearch.Text.Trim();
        DataTable dtUser = objUser.retrieve().Tables[0];

        if (bool.Parse(Session["TenantUser"].ToString()))
        {
            UserBAL objUserBAL = new UserBAL();
            ArrayList userIds = new ArrayList();
            objUserBAL.UserID = Convert.ToInt32(Session["UserID"]);
            DataSet dsTenant = objUserBAL.retrieveTenantDetails();
            if (dsTenant.Tables.Count > 0 && dsTenant.Tables[0].Rows.Count > 0)
            {
                tenantAssignedGroups = dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_ASSIGNEDGROUPS].ToString();
            }
            objUserBAL.GroupIDs = tenantAssignedGroups;
            DataSet dsUsers = objUserBAL.SearchWithAssetAuthorization(0);

            if (dsUsers.Tables.Count > 0 && dsUsers.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsUsers.Tables[0].Rows)
                {
                    userIds.Add(dr[DBFields.DBFIELD_USERID].ToString());
                }
            }

            DataTable tempTable = dtUser.Clone();
            foreach (DataRow dr in dtUser.Rows)
            {
                tempTable.Rows.Add(dr.ItemArray);
            }

            DataRow[] filteredRows = tempTable.Select("UserID IN (" + string.Join(",",userIds.ToArray()) + ")");
            dtUser.Rows.Clear();
            if (filteredRows != null && filteredRows.Length > 0)
            {
                foreach (DataRow dr in filteredRows)
                {
                    dtUser.Rows.Add(dr.ItemArray);
                }

            }

        }


        if (dtUser.Rows.Count == 0)
        {
            grdUser.DataSource = dtUser;
            grdUser.DataBind();
            grdUser.Columns[5].Hidden = true;
            grdUser.Columns[6].Hidden = true;
            grdUser.Columns[7].Hidden = true;
        }
        else
        {
            grdUser.DataSource = dtUser;
            grdUser.DataBind();
            //if (grdUser.Rows.Count > 0)
            //{
            //    ibExportToExcel.Enabled = true;
            //    ibExportToExcel.Visible = true;

            //}
            //else
            //{
            //    ibExportToExcel.Enabled = false;
            //    ibExportToExcel.Visible = false;
            //}

            if (_dtRights.Select("Module = 'User Search' and Rights = '" + "Modify User" + "'").Length != 0)
            {
                grdUser.Columns[5].Hidden = false;
            }
            else
                grdUser.Columns[5].Hidden = true;

            //added by kjb on 23 OCT 2012
            if (_dtRights.Select("Module = 'User Search' and Rights = '" + "Delete User" + "'").Length != 0)
            {
                grdUser.Columns[7].Hidden = false;
            }
            else
                grdUser.Columns[7].Hidden = true;


            if (_dtRights.Select("Module = 'User Search' and Rights = '" + "Assign Group to User" + "'").Length != 0)
                grdUser.Columns[6].Hidden = false;
            else
                grdUser.Columns[6].Hidden = true;
        }
    }

    /// <summary>
    /// Populate data grid.
    /// </summary>
    /// <param name="iPageIndex"></param>
    //private void doBuild(int iPageIndex)
    //{
    //    objUser = new ManageUsersBAL();
    //    objUser.Search = this.txtSearch.Text.Trim();
    //    //if (ddlUserType.SelectedValue != "(All)")
    //    //    objUser.UserType = Convert.ToInt32(ddlUserType.SelectedValue);
    //    //else
    //    //    objUser.UserType = -1;

    //    DataTable dtUserSearch = objUser.retrieve().Tables[0];
    //    lblMessage.Visible = false;
    //    uwgUsers.Visible = false;

    //    if (iPageIndex == 0)
    //        iPageIndex = 1;

    //    if (_dtRights == null)
    //        _dtRights = (DataTable)Session["Rights"];


    //    if (_dtRights.Select("Module = 'User Search' and Rights = '" + "Reset Password" + "'").Length != 0)
    //        uwgUsers.Columns.FromKey("ResetPassword").Hidden = true;
    //    else
    //        uwgUsers.Columns.FromKey("ResetPassword").Hidden = true;

    //    if (dtUserSearch.Rows.Count > 0)
    //    {
    //        uwgUsers.Visible = true;
    //        this.uwgUsers.DataSource = dtUserSearch;
    //        this.uwgUsers.DataBind();

    //        if (uwgUsers.Rows.Count <= 0 && uwgUsers.DisplayLayout.Pager.PageCount > 0)
    //            if (uwgUsers.DisplayLayout.Pager.CurrentPageIndex > 1)
    //            {
    //                uwgUsers.DisplayLayout.Pager.CurrentPageIndex = iPageIndex - 1;
    //                uwgUsers.DataBind();
    //            }


    //        // Disable paging if there aren't enough rows for more than one page
    //        if (dtUserSearch.Rows.Count <= Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString()))
    //            this.uwgUsers.DisplayLayout.Pager.AllowPaging = false;
    //    }
    //    else
    //    {
    //        lblMessage.Visible = true;
    //        lblMessage.Text = "No Records Found. Please expand your search criteria.";

    //    }
    //}

    protected void wibReset_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        Response.Redirect("UserSearch.aspx");
    }

    protected void ibEdit_Click(object sender, ImageClickEventArgs e)
    {
        //var imgEdit = (ImageButton)(sender);
        //var celItem = (CellItem)(imgEdit.NamingContainer);
        //UltraGridRow row = celItem.Cell.Row;
        //foreach (UltraGridRow gridItem in uwgUsers.Rows)
        //{
        //    if (gridItem.Selected)
        //    {
        //        row = gridItem;
        //        break;
        //    }
        //}
        //if (row != null)
        //{
        //    //Session["@@UserID"] = row.Cells.FromKey("UserID").Value.ToString();
        //    //Session["@@UserType"] = row.Cells.FromKey("UserType").Value.ToString();
        //    Context.Items.Add("@@UserID", row.Cells.FromKey("UserID").Value.ToString());
        //    Session["@@UserID"] = row.Cells.FromKey("UserID").Value.ToString(); // Added to get the selected userid for RFID Assignment
        //    Server.Transfer("ManageUsers.aspx");
        //    //Response.Redirect("ManageUsers.aspx");
        //}
    }

    //protected void uwgUsers_PageIndexChanged(object sender, Infragistics.WebUI.UltraWebGrid.PageEventArgs e)
    //{
    //    this.doBuild(e.NewPageIndex);
    //}

    /// <summary>
    /// Setting up session variables and transfer to assign rights page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibAssignRights_Click(object sender, ImageClickEventArgs e)
    {
        //ImageButton imgEdit = (ImageButton)(sender);
        //CellItem celItem = (CellItem)(imgEdit.NamingContainer);
        //UltraGridRow row = celItem.Cell.Row;
        //foreach (UltraGridRow gridItem in uwgUsers.Rows)
        //{
        //    if (gridItem.Selected)
        //    {
        //        row = gridItem;
        //        break;
        //    }
        //}
        //if (row != null)
        //{
        //    if (row.Cells.FromKey("Status").Value.Equals("Active"))
        //    {
        //        Session["@@UserID"] = row.Cells.FromKey("UserID").Value.ToString();
        //        Session["@@LoginName"] = row.Cells.FromKey("LoginName").Value.ToString();
        //        Session["@@Name"] = row.Cells.FromKey("Name").Value.ToString();
        //        Session["@@SearchText"] = this.txtSearch.Text.Trim();
        //        Response.Redirect("AssignRights.aspx");
        //    }
        //    else
        //        this.ShowMessage("User Status Disabled. Not allowed to Assign Rights");
        //    //Response.Write("<script language=javascript>alert('User Status Disabled. Not allowed to Assign Rights');</script>");
        //}
    }

    /// <summary>
    /// Register javascript to alert messages.
    /// </summary>
    /// <param name="mess"></param>
    private void ShowMessage(string mess)
    {
        string strScript = "<script type=\"text/javascript\">validNavigation = true;alert(\"" + mess + "\");</script>";
        if (!Page.ClientScript.IsStartupScriptRegistered("FORMMESSAGE"))
            Page.ClientScript.RegisterStartupScript(typeof(Page), "FORMMESSAGE", strScript);
    }

    /// <summary>
    /// Adding javascript function to reset password image button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void ibResetPassword_Init(object sender, EventArgs e)
    //{
    //    ImageButton imgBtn = (ImageButton)(sender);
    //    CellItem celItem = (CellItem)(imgBtn.NamingContainer);
    //    UltraGridRow row = celItem.Cell.Row;
    //    if (row != null)
    //    {
    //        if (row.Cells.FromKey("Status").Value.Equals("Active"))
    //        {
    //            //HP: Added UserGuid attribute to pass to ResetPassword dialog.
    //            //imgBtn.Attributes.Add("onclick", "openWin('ResetPassword','" + row.Cells.FromKey("UserGuid").Value + "','0');");
    //        }
    //        else
    //        {
    //            imgBtn.Attributes.Add("onclick", "alert('User Status Disabled. Not allowed to Reset Password');return false;");
    //        }
    //    }
    //    else
    //    {
    //        imgBtn.Visible = false;
    //    }
    //}
}
