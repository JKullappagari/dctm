/*
File Name   :	Group.aspx.cs

Description :	create User Group

Date created:	01 Oct 2006

Modification History:
***********************
CR		Name			Date			Description
New		srinivas	01/10/2006		File has been created.
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
using iAssetTrack.DALC;
using iAssetTrack.BAL;
using System.Data.SqlClient;
using iAssetTrackBAL;
using Infragistics.Web.UI.GridControls;


public partial class ASPX_Group : System.Web.UI.Page
{
    #region "Declarations"

    private iAssetTrack.BAL.GroupBAL objGroup;
    private iAssetTrack.BAL.CommonBAL objCommon;
    DataTable _dtRights;
    private iAssetTrack_WebDataGrid_Paging_CustomerPagerControl pagerControl;

    private const int GRID_COL_EDIT = 2;
    private const int GRID_COL_DELETE = 3;

    //Order the First Column in Sequence for maximum efficiency
    private String[,] saModuleRightsControlList = new String[3, 2]
    {
        {"Create", "ibCreateAsset"},
        {"Authorize", "uwlGroupUsers"},
        {"Authorize", "uwlSelectedUsers"}
    };
    private string tenantAssignedGroups = string.Empty;
    #endregion

    #region " Page Event Methods "
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        grdGroup.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(grdGroup_ItemCommand);
        pagerControl = grdGroup.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);
    }
    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdGroup.Behaviors.Paging.PageIndex = e.PageNumber;
        populateGrid();
    }
    protected void grdGroup_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControl.SetupPageList(this.grdGroup.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdGroup.Behaviors.Paging.PageIndex);
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
            pagerControl.SetupPageList(this.grdGroup.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdGroup.Behaviors.Paging.PageIndex);
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        populateGrid();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageHeader"] = "User Groups";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];
        lblMessage.Visible = false;
        _dtRights = (DataTable)(Session["Rights"]);

        bool blfoundPage = false;

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "Group.aspx";
            Response.Redirect("Login.aspx");
        }

        if (_dtRights.Select("Module = 'Group' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }

        if (_dtRights.Select("Module = 'Group' and Rights = '" + "Create" + "'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }

        if (_dtRights.Select("Module = 'Group' and Rights = '" + "Delete" + "'").Length != 0)
        {
            wibDelete.Visible = true;
        }
        else
        {
            wibDelete.Visible = false;
        }
        txtDesc.Attributes.Add("onkeydown", "if(this.value.length >=190) return false");
        this.grdGroup.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());
        populateGrid();
        if (!IsPostBack)
        {
            populateSiteList();
        }
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        if (wibDelete.Enabled)
        {
            iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();
            string sMessage = objCommon.displayMessage(MessageCodes.GROUP_JS_DELETE);
            hdnMessage.Value = sMessage;
        }

        string errMsg = GetLocalResourceObject("revDesc1Resource1.ErrorMessage").ToString();
        cvDesc.ErrorMessage = errMsg.Replace("{MAXLENGTH}", txtDesc.MaxLength.ToString());
    }

    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        //for tenant
        if (bool.Parse(Session["TenantUser"].ToString()))
        {
            UserBAL objUser = new UserBAL();
            objUser.UserID = Convert.ToInt32(Session["UserID"]);
            DataSet dsTenant = objUser.retrieveTenantDetails();
            if (dsTenant.Tables.Count > 0 && dsTenant.Tables[0].Rows.Count > 0)
            {
                if (!txtGroup.Text.ToLower().StartsWith(dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_SHORT_NAME].ToString().ToLower() + "."))
                {
                    lblMessage.Text = "Group name must be prefixed with Tenant short name followed by dot(.)";
                    lblMessage.Visible = true;
                    return;

                }
            }

        }
        objGroup = new iAssetTrack.BAL.GroupBAL();

        objGroup.Group = txtGroup.Text.Trim();
        objGroup.Description = txtDesc.Text.Trim();
        objGroup.Status = 1;
        objGroup.CreatedBy = Convert.ToInt32(Session["UserID"]);
        int intGroup = 0;
        objGroup.GroupID = this.hdnGrpID.Value == string.Empty ? intGroup : Convert.ToInt32(this.hdnGrpID.Value);
        intGroup = objGroup.exists();

        if (intGroup != -1 && intGroup != 0)
            objGroup.GroupID = intGroup;

        if (intGroup != -1)
        {
            objGroup.Persist(DALCOperation.Insert);
            clearFields();
            if (this.hdnGrpID.Value == string.Empty)
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
            this.hdnGrpID.Value = string.Empty;
        }
        else
        {
            objCommon = new CommonBAL();
            lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_I_EXISTS);
            lblMessage.Visible = true;
        }
        grdGroup.ClearDataSource();
        populateGrid();
    }

    protected void ibReset_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        clearFields();
        txtGroup.Focus();
        this.hdnGrpID.Value = string.Empty;
        populateGrid();
    }
    protected void ibDelete_Click(object sender, EventArgs e)
    {
        CheckBox chkDelete;
        int GroupId;
        string strIDs;

        strIDs = "";

        foreach (GridViewRow grdViewRow in grdGroup.Rows)
        {
            chkDelete = (CheckBox)(grdViewRow.FindControl("chkDelete"));
            if (chkDelete.Checked == true)
            {
                GroupId = Convert.ToInt32(((Label)(grdViewRow.FindControl("lblDeleteID"))).Text);
                strIDs += Convert.ToString(GroupId) + ",";
            }
        }


        if (strIDs != "")
        {
            strIDs = strIDs.Remove(strIDs.Length - 1, 1);
        }


        objGroup = new iAssetTrack.BAL.GroupBAL();
        objGroup.GroupIDs = strIDs;
        objGroup.Status = 0;
        objGroup.LastModifiedBy = Convert.ToInt32(Session["UserID"]);
        objGroup.Persist(DALCOperation.Delete);

        clearFields();
        objCommon = new CommonBAL();
        lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_DELETED);

        lblMessage.Visible = true;
        grdGroup.ClearDataSource();
        populateGrid();

    }
    protected void grdGroup_RowEditing(object sender, GridViewEditEventArgs e)
    {
        //populateGrid();
        //objGroup = new iAssetTrack.BAL.GroupBAL();
        //objGroup.GroupID = Convert.ToInt32(grdGroup.DataKeys[Convert.ToInt32(e.NewEditIndex)].Value);
        //DataSet dsGroup = objGroup.retrieve();
        //DataRow dr = dsGroup.Tables[0].Rows[0];
        //txtGroup.Text = dr[DBFields.DBFIELD_GROUP].ToString();
        //txtDesc.Text = dr[DBFields.DBFIELD_DESCRIPTION].ToString();

        //ddlDepartment.SelectedValue = dr[DBFields.DBFIELD_SITEID].ToString();

        //this.hdnGrpID.Value = objGroup.GroupID.ToString();
        //Session["GroupID"] = objGroup.GroupID;
    }

    protected void grdGroup_ItemCommand(object sender, HandleCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            objGroup = new iAssetTrack.BAL.GroupBAL();
            objGroup.GroupID = Convert.ToInt32(e.CommandArgument);
            DataSet dsGroup = objGroup.retrieve();
            DataRow dr = dsGroup.Tables[0].Rows[0];
            txtGroup.Text = dr[DBFields.DBFIELD_GROUP].ToString();
            txtDesc.Text = dr[DBFields.DBFIELD_DESCRIPTION].ToString();

            //ddlDepartment.SelectedValue = dr[DBFields.DBFIELD_SITEID].ToString();

            this.hdnGrpID.Value = objGroup.GroupID.ToString();

            if (_dtRights.Select("Module = 'Group' and Rights = '" + "Modify" + "'").Length != 0)
            {
                ibCreate.Enabled = true;
            }
            else
            {
                ibCreate.Enabled = false;
            }
        }
    }
    protected void grdGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //grdGroup.PageIndex = e.NewPageIndex;
        //populateGrid();

    }
    #endregion

    #region "User Defined Methods"


    /// <summary>
    /// fill user groups
    /// </summary>
    /// 
    private void populateGrid()
    {

        objGroup = new iAssetTrack.BAL.GroupBAL();
        DataTable dtGroup = objGroup.retrieve().Tables[0];

        if (bool.Parse(Session["TenantUser"].ToString()))
        {
            UserBAL objUser = new UserBAL();
            objUser.UserID = Convert.ToInt32(Session["UserID"]);
            DataSet dsTenant = objUser.retrieveTenantDetails();
            if (dsTenant.Tables.Count > 0 && dsTenant.Tables[0].Rows.Count > 0)
            {
                tenantAssignedGroups = dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_ASSIGNEDGROUPS].ToString();
            }

            DataTable tempTable = dtGroup.Clone();
            foreach (DataRow dr in dtGroup.Rows)
            {
                tempTable.Rows.Add(dr.ItemArray);
            }
            DataRow[] filteredRows = tempTable.Select("GroupID IN (" + tenantAssignedGroups + ")");
            dtGroup.Rows.Clear();
            if (filteredRows != null && filteredRows.Length > 0)
            {
                foreach (DataRow dr in filteredRows)
                {
                    dtGroup.Rows.Add(dr.ItemArray);
                }

            }

        }

        DataView dvSource = new DataView(dtGroup);


        Session["Source"] = dvSource;
        if (Session["sortorder"] == null)
            Session["sortorder"] = "Ascending";

        if (grdGroup.Rows.Count == 0)
        {
            grdGroup.DataSource = dtGroup;
            grdGroup.DataBind();
            grdGroup.Columns[GRID_COL_EDIT].Hidden = true;
            wibDelete.Visible = false;
        }
        else
        {
            grdGroup.DataSource = dtGroup;
            grdGroup.DataBind();
            if (_dtRights.Select("Rights = 'Delete' and Module = '" + Page.Title + "'").Length != 0)
            {
                grdGroup.Columns[GRID_COL_DELETE].Hidden = false;
                wibDelete.Visible = true;
            }
            else
            {

                grdGroup.Columns[GRID_COL_DELETE].Hidden = true;
                wibDelete.Visible = false;
            }

            if (_dtRights.Select("Rights = 'Modify' and Module = '" + Page.Title + "'").Length != 0)
            {
                grdGroup.Columns[GRID_COL_EDIT].Hidden = false;
            }
            else
            {
                grdGroup.Columns[GRID_COL_EDIT].Hidden = true;
            }
        }
        if (wibDelete.Visible == true)
        {
            int groupID;
            int iCount = 0;

            for (int i = 0; i < grdGroup.Rows.Count; i++)
            {
                groupID = Convert.ToInt32(((Label)(grdGroup.Rows[i].Items[GRID_COL_DELETE].FindControl("lblDeleteID"))).Text);
                grdGroup.Rows[i].Items[GRID_COL_DELETE].FindControl("chkDelete").Visible = true;
                objCommon = new iAssetTrack.BAL.CommonBAL();

                DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_GROUPID, groupID.ToString(), 0);
                foreach (DataTable tblCheck in dsCheck.Tables)
                {
                    if (tblCheck.Rows[0][0].ToString() != "0")
                    {
                        grdGroup.Rows[i].Items[GRID_COL_DELETE].FindControl("chkDelete").Visible = false;
                    }
                }
                if (grdGroup.Rows[i].Items[GRID_COL_DELETE].FindControl("chkDelete").Visible == false)
                {
                    iCount += 1;
                }

            }
            if (iCount == grdGroup.Rows.Count)
            {
                grdGroup.Columns[GRID_COL_DELETE].Hidden = true;
                wibDelete.Visible = false;
            }
        }
        if (grdGroup.Rows.Count > 0)
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
    /// check deleted groups
    /// </summary>
    /// 

    //private void checkDelete()
    //{
    //    if (wibDelete.Visible == true)
    //    {
    //        int GroupId;
    //        int iCount = 0;

    //        for (int i = 0; i < grdGroup.Rows.Count; i++)
    //        {

    //            GroupId = Convert.ToInt32(((Label)(grdGroup.Rows[i].Items[GRID_COL_DELETE].FindControl("lblDeleteID"))).Text);

    //            objCommon = new iAssetTrack.BAL.CommonBAL();

    //            DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_GROUPID, GroupId);
    //            foreach (DataTable tblCheck in dsCheck.Tables)
    //            {
    //                if (tblCheck.Rows[0][0].ToString() != "0")
    //                {
    //                    grdGroup.Rows[i].Items[GRID_COL_DELETE].FindControl("chkDelete").Visible = false;
    //                }
    //            }
    //            if (grdGroup.Rows[i].Items[GRID_COL_DELETE].FindControl("chkDelete").Visible == false)
    //            {
    //                iCount += 1;
    //            }

    //        }
    //        if (iCount == grdGroup.Rows.Count)
    //        {
    //            grdGroup.Columns[GRID_COL_DELETE].Hidden = true;
    //            wibDelete.Visible = false;
    //        }
    //    }

    //}

    /// <summary>
    /// clear fields
    /// </summary>
    /// 

    private void clearFields()
    {

        txtGroup.Text = "";
        txtDesc.Text = "";
        lblMessage.Visible = false;
        lblMessage.Text = "";
        //ddlDepartment.SelectedValue = "-1";
    }


    #endregion

    /// <summary>
    /// delete group
    /// </summary>
    /// 

    protected void wibDelete_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        CheckBox chkDelete;
        int GroupId;
        string strIDs;

        strIDs = "";

        foreach (Infragistics.Web.UI.GridControls.GridRecord grdViewRow in grdGroup.Rows)
        {
            chkDelete = (CheckBox)(grdViewRow.Items[3].FindControl("chkDelete"));
            if (chkDelete.Checked == true)
            {
                GroupId = Convert.ToInt32(((Label)(grdViewRow.Items[3].FindControl("lblDeleteID"))).Text);
                strIDs += Convert.ToString(GroupId) + ",";
            }
        }


        if (strIDs != "")
        {
            strIDs = strIDs.Remove(strIDs.Length - 1, 1);
            objGroup = new iAssetTrack.BAL.GroupBAL();
            objGroup.GroupIDs = strIDs;
            objGroup.Status = 0;
            objGroup.LastModifiedBy = Convert.ToInt32(Session["UserID"]);
            objGroup.Persist(DALCOperation.Delete);

            clearFields();
            objCommon = new CommonBAL();
            lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_DELETED);

            lblMessage.Visible = true;
            grdGroup.ClearDataSource();
            populateGrid();
        }



    }

    /// <summary>
    /// sorting grid
    /// </summary>
    /// 

    protected void grdGroup_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataView dv = (DataView)Session["Source"];

        if (Session["sortorder"].ToString() == "Ascending")
        {

            Session["sortorder"] = "Descending";

            dv.Sort = e.SortExpression + " ASC";

        }

        else
        {

            Session["sortorder"] = "Ascending";

            dv.Sort = e.SortExpression + " DESC";

        }

        Session["SavedSort"] = dv.Sort;
        this.grdGroup.DataSource = dv;
        this.grdGroup.DataBind();
        //this.checkDelete();

        /*
        string m_strSortExp =(string) ViewState["_SortExp_"];
        string m_SortDirection =(string) ViewState["_Direction_"];
        if (String.Empty != m_strSortExp)
        {
            if (String.Compare(e.SortExpression, m_strSortExp, true) == 0)
            {
                if (m_SortDirection.Equals(SortDirection.Ascending))
                    m_SortDirection = SortDirection.Descending.ToString();
                else
                    m_SortDirection = SortDirection.Ascending.ToString();

                   //(m_SortDirection.Equals(SortDirection.Ascending)) ? SortDirection.Descending : SortDirection.Ascending;
            }
        }
        ViewState["_Direction_"] = m_SortDirection;
        ViewState["_SortExp_"] = m_strSortExp = e.SortExpression;
        this.populateGrid();
        */
    }


    private void populateSiteList()
    {
        //if (!ddlDepartment.Visible) return;


        //ddlDepartment.Items.Clear();
        //BUSiteAssignmentBAL siteBAL = new BUSiteAssignmentBAL();
        //siteBAL.BusinessUnitID = pBusineddUnitId;

        //DepartmentsBAL deptBAL = new DepartmentsBAL();
        //DataTable dt = deptBAL.retrieve().Tables[0];

        //ListItem emptyitem = new ListItem("- Select -", "-1");
        //ddlDepartment.Items.Add(emptyitem);

        ////DataTable dt = siteBAL.retrieveAssignSite().Tables[0];

        //ddlDepartment.DataSource = dt;
        //ddlDepartment.DataTextField = dt.Columns["Department"].ToString();
        //ddlDepartment.DataValueField = dt.Columns["DepartmentID"].ToString();
        //ddlDepartment.DataBind();

        //iAssetTrack.BAL.SitesBAL siteBAL = new SitesBAL();
        //DataSet ds = siteBAL.retrieve();

        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //    {
        //        ListItem item = new ListItem(ds.Tables[0].Rows[i]["Site"].ToString(), ds.Tables[0].Rows[i]["SiteID"].ToString());
        //        ddlDepartment.Items.Add(item);
        //    }

        //}

    }
    protected void ibExportToExcel_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        this.eExporter.DataExportMode = DataExportMode.AllDataInDataSource;
        Infragistics.Documents.Excel.Workbook wBook = new Infragistics.Documents.Excel.Workbook(excelFormat);
        this.eExporter.Export(this.grdGroup, wBook);
    }
    protected void eExporter_CellExported(object sender, ExcelCellExportedEventArgs e)
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

    protected void grdGroup_InitializeRow(object sender, RowEventArgs e)
    {
        //if (wibDelete.Visible == true)
        //{
        //    int GroupId;
        //    int iCount = 0;

        //    //for (int i = 0; i < grdGroup.Rows.Count; i++)
        //    //{

        //        GroupId = Convert.ToInt32(((Label)(e.Row.Items[GRID_COL_DELETE].FindControl("lblDeleteID"))).Text);

        //        objCommon = new iAssetTrack.BAL.CommonBAL();

        //        DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_GROUPID, GroupId);
        //        foreach (DataTable tblCheck in dsCheck.Tables)
        //        {
        //            if (tblCheck.Rows[0][0].ToString() != "0")
        //            {
        //                e.Row.Items[GRID_COL_DELETE].FindControl("chkDelete").Visible = false;
        //            }
        //        }
        //    //    if (e.Row.Items[GRID_COL_DELETE].FindControl("chkDelete").Visible == false)
        //    //    {
        //    //        iCount += 1;
        //    //    }

        //    //}
        //    //if (iCount == grdGroup.Rows.Count)
        //    //{
        //    //    grdGroup.Columns[GRID_COL_DELETE].Hidden = true;
        //    //    wibDelete.Visible = false;
        //    //}
        //}
    }
}


