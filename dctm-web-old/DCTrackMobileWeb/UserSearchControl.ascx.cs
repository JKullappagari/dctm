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
using System.Text;
using iAssetTrackBAL;
using iAssetTrack.DALC;

public partial class UserSearchControl : System.Web.UI.UserControl
{

    public int BusinessUnit
    {
        get { return Convert.ToInt32(ViewState["BusinessUnitID"]); }
        set { ViewState["BusinessUnitID"] = value; }
    }

    public int DivisionID
    {
        get { return Convert.ToInt32(ViewState["DivisionID"]); }
        set { ViewState["DivisionID"] = value; }
    }

    public enum ReturnValueColumnVals
    {
        UserID,
        GroupMemberID
    }
    private ReturnValueColumnVals m_ReturnValueColumn = ReturnValueColumnVals.UserID;

    public ReturnValueColumnVals ReturnValueColumn
    {
        get { return (ReturnValueColumnVals)ViewState["ReturnValueColumn"]; }
        set { ViewState["ReturnValueColumn"] = value; }
    }


    public enum ReturnTextColumVals
    {
        LoginName,
        FirstNameCommaLastName,
        LastNameCommaFirstName,
        FirstNameSpaceLastName,
    }
    private ReturnTextColumVals m_ReturnTextColumn = ReturnTextColumVals.LoginName;

    public ReturnTextColumVals ReturnTextColumn
    {
        get { return (ReturnTextColumVals)ViewState["ReturnTextColumn"]; }
        set { ViewState["ReturnTextColumn"] = value; }
    }


    private iAssetTrack_WebDataGrid_Paging_CustomerPagerControl pagerControl;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        grdOwner.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(grdOwner_ItemCommand);
        pagerControl = grdOwner.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);
    }
    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdOwner.Behaviors.Paging.PageIndex = e.PageNumber;
        populateGrid();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.grdOwner.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());
        populateGrid();

        if (!Page.IsPostBack)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            LoadBusinessUnits();

            this.BusinessUnit = Convert.ToInt32(ViewState["BusinessUnitID"]);
            this.DivisionID = Convert.ToInt32(ViewState["DivisionID"]);


            if (Request.QueryString.Count > 0)
            {
                if (this.BusinessUnit > 0)
                { 
                    ddlBusinessUnit.SelectedValue = Convert.ToString(this.BusinessUnit);
                    populateDivision(Int32.Parse(ddlBusinessUnit.SelectedValue));
                    ddlBusinessUnit.Enabled = false;
                }

            }
        }

        populateGrid();
        this.txtName.Focus();
    }


    private void LoadBusinessUnits()
    {
        int intUserID = 0;
        if (Session["UserID"] != null)
            intUserID = Convert.ToInt32(Session["UserID"].ToString());

        FillDropDownListBU(StoredProcedures.SP_BUSINESSUNIT_LISTBYUSERID, ref ddlBusinessUnit, intUserID);
    }


    private void FillDropDownListBU(string strStoredProc, ref DropDownList ddl, int id)
    {

        iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();
        DataTable dt = objCommon.FillDropDownListBU(strStoredProc, "", id);
        ddl.DataSource = dt;
        ddl.DataValueField = dt.Columns[0].ToString();
        ddl.DataTextField = dt.Columns[1].ToString();
        ddl.DataBind();

        string mode = ConfigurationManager.AppSettings["Mode"];

    }

    private void populateDivision(int pBusinessUnitID)
    {

        if (!ddlDivision.Visible) return;
        if (pBusinessUnitID < 0) return;

        DivisionBAL divBAL = new DivisionBAL();
        DataTable dt = divBAL.retrieveByBusinessUnitId(pBusinessUnitID).Tables[0];

        if (bool.Parse(Session["TenantUser"].ToString()))
        {
            string tenantAssignedDivisions = string.Empty;
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
                    DataTable tempTable = dt.Clone();
                    foreach (DataRow drow in dt.Rows)
                    {
                        tempTable.Rows.Add(drow.ItemArray);
                    }
                    DataRow[] filteredRows = tempTable.Select("DivisionID IN (" + tenantAssignedDivisions + ")");
                    dt.Rows.Clear();
                    if (filteredRows != null && filteredRows.Length > 0)
                    {
                        foreach (DataRow drow in filteredRows)
                        {
                            dt.Rows.Add(drow.ItemArray);
                        }

                    }
                }
                else
                {
                    dt.Rows.Clear();
                }

            }

        }

        ddlDivision.Items.Clear();
        ListItem emptyitem = new ListItem("(All)", "0");
        ddlDivision.Items.Add(emptyitem);

        ddlDivision.DataSource = dt;
        ddlDivision.DataTextField = dt.Columns["Division"].ToString();
        ddlDivision.DataValueField = dt.Columns["DivisionID"].ToString();
        ddlDivision.DataBind();
    }


    protected void wibSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        populateGrid();
    }

    protected void grdOwner_InitializeRow(object sender, Infragistics.Web.UI.GridControls.RowEventArgs e)
    {
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
        }
        else
        {
            pagerControl.SetupPageList(this.grdOwner.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdOwner.Behaviors.Paging.PageIndex);
        }


    }

    protected void grdOwner_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            pagerControl.SetCurrentPageNumber(grdOwner.Behaviors.Paging.PageIndex + 1);
            populateGrid();
        }
    }

    private void populateGrid()
    {
        if (ddlBusinessUnit.SelectedValue == "")
            return;

        iAssetTrack.BAL.OwnerBAL objOwner = new iAssetTrack.BAL.OwnerBAL();
        //Pass Search values        
        objOwner.DivisionID = Int32.Parse(ddlDivision.SelectedValue == "0" ? "0" : ddlDivision.SelectedValue);
        objOwner.FirstName = this.txtName.Text.Trim();
        objOwner.LastName = this.txtName.Text.Trim();

        DataSet dsOwner = null;
        if (ddlBusinessUnit.SelectedValue != "")
        {
            dsOwner = objOwner.retrieveByOtherFields();
        }

        if (bool.Parse(Session["TenantUser"].ToString()))
        {
            string tenantAssignedOwners = string.Empty;
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
                    DataTable tempTable = dsOwner.Tables[0].Clone();
                    foreach (DataRow dr in dsOwner.Tables[0].Rows)
                    {
                        tempTable.Rows.Add(dr.ItemArray);
                    }
                    DataRow[] filteredRows = tempTable.Select("OwnerID IN (" + tenantAssignedOwners + ")");
                    dsOwner.Tables[0].Rows.Clear();
                    if (filteredRows != null && filteredRows.Length > 0)
                    {
                        foreach (DataRow dr in filteredRows)
                        {
                            dsOwner.Tables[0].Rows.Add(dr.ItemArray);
                        }

                    }
                }
                else
                {
                    dsOwner.Tables[0].Rows.Clear();
                }

            }

        }

        grdOwner.DataSource = dsOwner.Tables[0];
        grdOwner.DataBind();
    }


    protected void grdOwner_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        populateGrid();
    }


    protected void grdOwner_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }

    protected void wibReset_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        string strStartUpScript = "<script language='javascript' type='text/javascript'>selectOwner('','');</script>";
        Page.ClientScript.RegisterStartupScript(typeof(Page), "PopupScript", strStartUpScript);

    }


    protected void ddlBusinessUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        populateDivision(Int32.Parse(ddlBusinessUnit.SelectedValue));
    }
}
