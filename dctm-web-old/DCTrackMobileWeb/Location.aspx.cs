/*
File Name   :	Location.aspx.cs

Description :	Used to create Location

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
using iAssetTrack.DALC;
using System.Data.SqlClient;
using iAssetTrackBAL;
using Infragistics.Web.UI.NavigationControls;

public partial class Location : System.Web.UI.Page
{
    #region "Declarations"
    private iAssetTrack.BAL.LocationBAL objLocation;
    private iAssetTrack.BAL.CommonBAL objCommon;
    private iAssetTrack.BAL.LocationTypeBAL objLocationType;
    DataTable _dtRights;
    protected int totalRecordCount = 0;
    private iAssetTrack_WebDataGrid_Paging_CustomerPagerControl pagerControl;
    protected string ddlLocTypeVal = string.Empty;
    protected string ddlMfgName = string.Empty;
    protected string ddlMfgID = string.Empty;
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
    private string tenantAssignedLocations = string.Empty;
    #endregion

    #region " Page Event Methods "
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        grdLocation.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(grdLocation_ItemCommand);
        pagerControl = grdLocation.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);
    }
    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdLocation.Behaviors.Paging.PageIndex = e.PageNumber;
        populateGrid();
    }
    protected void grdLocation_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControl.SetupPageList(this.grdLocation.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdLocation.Behaviors.Paging.PageIndex);
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
            pagerControl.SetupPageList(this.grdLocation.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdLocation.Behaviors.Paging.PageIndex);
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
        Session["PageHeader"] = "Location";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        lblMessage.Visible = false;

        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "Location.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        if (_dtRights.Select("Module = 'Location' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }

        if (_dtRights.Select("Module = 'Location' and Rights = '" + "Create" + "'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }

        if (_dtRights.Select("Module = 'Location' and Rights = '" + "Delete" + "'").Length != 0)
        {
            ibDelete.Visible = true;
        }
        else
        {
            ibDelete.Visible = false;
        }

        this.grdLocation.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());
        populateGrid();

        if (!string.IsNullOrEmpty(hdnLocation.Value))
        {
            txtParentLocation.Text = hdnLocation.Value;
        }

        if (!string.IsNullOrEmpty(hdnModelName.Value))
        {
            txtModel.Text = hdnModelName.Value;
        }

        EnableRackControls(false);

        if (!IsPostBack)
        {
            Session["LocationID"] = null;
            populateLocationType();
            int intUserID = 0;
            if (Session["UserID"] != null)
                intUserID = Convert.ToInt32(Session["UserID"].ToString());

            iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();
            DataTable dt = objCommon.FillDropDownListBU(StoredProcedures.SP_BUSINESSUNIT_LISTBYUSERID, "-Select-", intUserID);
            if (dt.Rows.Count > 1)
            {
                //first row will have 0,-Select-
                string businessUnit = dt.Rows[1][0].ToString();//BusinessUnitID
                hdnBusinessUnit.Value = businessUnit;
            }
            else
            {
                hdnBusinessUnit.Value = "0";
            }
            //Mfg
            PopulateMfgList();
            if (ddlMfg.SelectedIndex == 0)
            {
                hdnModelID.Value = "0";
                hdnModelName.Value = "-Select-";
                txtModel.Text = "-Select-";
            }
        }
        if (ddlLocationTypeList != null)
        {
            if (ddlLocationTypeList.SelectedItem.Text.CompareTo("Rack") == 0)
            {
                EnableRackControls(true);
            }
            else
            {
                EnableRackControls(false);
            }
        }
        //v3.8
        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "checkLocType();", true);

        if (ddlMfg != null && ddlMfg.Items.Count > 0)
        {
            ddlMfgName = ddlMfg.SelectedItem.Text;
            ddlMfgID = ddlMfg.SelectedItem.Value;
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
            string sMessage = objCommon.displayMessage(MessageCodes.LC_JS_DELETE);
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
    protected void grdLocation_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
    }

    /// <summary>
    /// Used to call upon grid row edits.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void grdLocation_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }

    /// <summary>
    /// Used to save information related BU.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        //location id and parent location id can't be same
        if (Session["LocationID"] != null)
        {
            int locationId;
            if (int.TryParse(Session["LocationID"].ToString(), out locationId) && locationId == int.Parse(hdnParentLocationID.Value))
            {
                lblMessage.Text = "Parent location can't be same as current location";
                lblMessage.Visible = true;
                return;
            }
        }
        // before creating a location, perform validations based
        // on location type selected
        switch (ddlLocationTypeList.SelectedItem.Text)
        {
            case "Room":
                if (txtParentLocation.Text.CompareTo("No Location") != 0)
                {
                    lblMessage.Text = "Parent location not allowed for Room";
                    lblMessage.Visible = true;
                    return;
                }
                break;
            case "Row":
                if (txtParentLocation.Text.CompareTo("No Location") == 0)
                {
                    lblMessage.Text = "Row must contain a parent location";
                    lblMessage.Visible = true;
                    return;
                }
                else
                {
                    LocationBAL obLoc = new LocationBAL();
                    obLoc.LocationID = Int32.Parse(hdnParentLocationID.Value);
                    DataSet dsLoc = obLoc.retrieve();
                    if (dsLoc.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = dsLoc.Tables[0].Rows[0];
                        if (dr["LocationType"].ToString().CompareTo("Room") != 0)
                        {
                            lblMessage.Text = "Parent location for a Row must be a Room";
                            lblMessage.Visible = true;
                            return;
                        }
                    }
                }
                break;
            case "Rack":
                if (txtParentLocation.Text.CompareTo("No Location") == 0)
                {
                    lblMessage.Text = "Rack must contain a parent location";
                    lblMessage.Visible = true;
                    return;
                }
                else
                {
                    LocationBAL obLoc = new LocationBAL();
                    obLoc.LocationID = Int32.Parse(hdnParentLocationID.Value);
                    DataSet dsLoc = obLoc.retrieve();
                    if (dsLoc.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = dsLoc.Tables[0].Rows[0];
                        if (dr["LocationType"].ToString().CompareTo("Rack") == 0)
                        {
                            lblMessage.Text = "Parent location for a Rack must be a Room or a Row";
                            lblMessage.Visible = true;
                            return;
                        }
                    }
                }
                if (Convert.ToInt32(ddlMfg.SelectedItem.Value.ToString()) > 0)
                {
                    if (!string.IsNullOrEmpty(hdnModelID.Value) && Convert.ToInt32(hdnModelID.Value) > 0)
                    {

                    }
                    else
                    {
                        lblMessage.Text = "Model Details are required for Rack";
                        lblMessage.Visible = true;
                        return;
                    }
                }
                else
                {
                    lblMessage.Text = "Manufacturer and Model details required for Rack";
                    lblMessage.Visible = true;
                    return;
                }
                break;

        }
        if (txtLocation.Text.ToLower().Contains("dispose") || txtLocation.Text.ToLower().Contains("decom"))
        {
            //location with key words like dispose or decom is not allowed
            lblMessage.Text = "Location Name with Dispose or Decom keywords is not allowed";
            lblMessage.Visible = true;
            return;
        }
        if (!Page.IsValid) return;
        objLocation = new iAssetTrack.BAL.LocationBAL();
        objLocation.Location = txtLocation.Text.Trim();
        objLocation.Description = txtDesc.Text.Trim();
        objLocation.IpAddress = "";


        //v3.8
        objLocation.FloorNo = txtFloor.Text.Trim();
        //--//

        if (!string.IsNullOrEmpty(txtParentLocation.Text.Trim()))
        {
            if (!txtParentLocation.Text.Equals("No Location"))
            {
                if (!string.IsNullOrEmpty(hdnParentLocationID.Value))
                    objLocation.ParentLocationID = int.Parse(hdnParentLocationID.Value.ToString());
                //else
                //    objLocation.ParentLocationID = 0;
            }
            else
            {
                objLocation.ParentLocationID = 0;
            }
        }

        if (ddlLocationTypeList.SelectedIndex > 0)
            objLocation.LocationTypeID = Convert.ToInt32(ddlLocationTypeList.SelectedValue);

        //IsExitDoor and IsCheckOutLocation values are hard coded
        objLocation.IsExitDoor = 0;
        objLocation.IsCheckOutLocation = 1;

        objLocation.Status = 1;
        objLocation.CreatedBy = Convert.ToInt32(Session["UserID"]);

        //4.5.0.7
        int modelID = 0;
        if (!string.IsNullOrEmpty(txtTag.Text.Trim()))
            objLocation.TagID = txtTag.Text.Trim();
        else
            objLocation.TagID = "";

        if (ddlLocationTypeList.SelectedItem.Text.CompareTo("Rack") == 0)
        {
            objLocation.SerialNumber = txtSerialNo.Text.Trim();
            objLocation.Manufacturer = ddlMfg.SelectedItem.Text;

            if (!string.IsNullOrEmpty(hdnModelID.Value))
                modelID = Convert.ToInt32(hdnModelID.Value);
            else
                modelID = 0;

            if (modelID > 0)
                objLocation.Model = txtModel.Text.Trim();
            else
                objLocation.Model = "";

            //get Height from Model Detais
            AssetModelBAL modelBAL = new AssetModelBAL();
            modelBAL.ModelID = modelID;
            DataSet dsModel = modelBAL.retrieve();
            if (dsModel != null && dsModel.Tables.Count > 0 && dsModel.Tables[0].Rows.Count > 0)
                objLocation.Height = Convert.ToInt32(dsModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_UHEIGHT].ToString());
            else
                objLocation.Height = 0;
        }
        else
        {
            objLocation.SerialNumber = "";
            objLocation.Manufacturer = "";
            objLocation.Model = "";
            modelID = 0;
        }

        objLocation.ModelID = modelID;
        objLocation.ExternalID = "";


        int intLocation = 0;
        objLocation.LocationID = Session["LocationID"] == null ? intLocation : (int)Session["LocationID"];
        intLocation = objLocation.exists();

        if (intLocation != -1 && intLocation != 0)
            objLocation.LocationID = intLocation;
        if (objLocation.hasChildren(objLocation.LocationID) && objLocation.ParentLocationID == 0)
        {
            objCommon = new CommonBAL();
            lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_I_REMOVECHILD);
            lblMessage.Visible = true;

        }
        else if (intLocation != -1)
        {
            try
            {
                objLocation.Persist(DALCOperation.Insert);
                clearFields();
                if (Session["LocationID"] == null)
                {
                    objCommon = new CommonBAL();
                    lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_INSERTED);
                }
                else
                {
                    objCommon = new CommonBAL();
                    lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_UPDATED);
                }
                Session["LocationID"] = null;
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
            lblMessage.Visible = true;
            grdLocation.ClearDataSource();
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

    private void PopulateMfgList()
    {
        ManufacturerBAL objMfg = new iAssetTrack.BAL.ManufacturerBAL();

        ddlMfg.Items.Clear();
        DataSet dsMfg = objMfg.retrieveByModelType("Rack");
        DataTable dtMfg = dsMfg.Tables[0];

        DataRow dr = dtMfg.NewRow();
        dr[0] = 0;
        dr[1] = "-Select-";
        dtMfg.Rows.InsertAt(dr, 0);

        ddlMfg.DataSource = dtMfg;
        ddlMfg.DataValueField = dtMfg.Columns[0].ToString();
        ddlMfg.DataTextField = dtMfg.Columns[1].ToString();
        ddlMfg.DataBind();
    }

    /// <summary>
    /// Used to clear form control value(s).
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibReset_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        clearFields();
        Session["LocationID"] = null;
        txtLocation.Focus();
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
        int LocationId;
        string strIDs;

        strIDs = "";
        foreach (Infragistics.Web.UI.GridControls.GridRecord grdViewRow in grdLocation.Rows)
        {
            chkDelete = (CheckBox)(grdViewRow.Items[6].FindControl("chkDelete"));
            if (chkDelete.Checked == true)
            {
                LocationId = Convert.ToInt32(((Label)(grdViewRow.Items[6].FindControl("lblDeleteID"))).Text);
                strIDs += Convert.ToString(LocationId) + ",";
            }
        }

        if (strIDs != "")
        {
            strIDs = strIDs.Remove(strIDs.Length - 1, 1);
        }

        objLocation = new iAssetTrack.BAL.LocationBAL();
        objLocation.LocationIDs = strIDs;
        objLocation.Status = 0;
        objLocation.LastModifiedBy = Convert.ToInt32(Session["UserID"]);
        objLocation.Persist(DALCOperation.Delete);

        clearFields();

        objCommon = new CommonBAL();
        lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_DELETED);
        lblMessage.Visible = true;
        grdLocation.ClearDataSource();
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


        objLocation = new LocationBAL();
        DataTable dtGrid = objLocation.retrieve().Tables[0];

        if (bool.Parse(Session["TenantUser"].ToString()))
        {
            UserBAL objUser = new UserBAL();
            objUser.UserID = Convert.ToInt32(Session["UserID"]);
            DataSet dsTenant = objUser.retrieveTenantDetails();
            if (dsTenant.Tables.Count > 0 && dsTenant.Tables[0].Rows.Count > 0)
            {
                tenantAssignedLocations = dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_ASSIGNEDLOCATIONS].ToString();
            }

            DataTable tempTable = dtGrid.Clone();
            foreach (DataRow dr in dtGrid.Rows)
            {
                tempTable.Rows.Add(dr.ItemArray);
            }
            DataRow[] filteredRows = tempTable.Select("LocationID IN (" + tenantAssignedLocations + ")");
            dtGrid.Rows.Clear();
            if (filteredRows != null && filteredRows.Length > 0)
            {
                foreach (DataRow dr in filteredRows)
                {
                    dtGrid.Rows.Add(dr.ItemArray);
                }

            }

        }


        totalRecordCount = dtGrid.Rows.Count;
        //grdLocation.DataSource = dtGrid;
        //grdLocation.DataBind();

        grdLocation.Behaviors.Paging.Enabled = false;
        if (totalRecordCount == grdLocation.Rows.Count)
            this.FilterCount = "";
        grdLocation.Behaviors.Paging.Enabled = true;

        if (dtGrid.Rows.Count == 0)
        {
            //grdLocation.DataSource = null;
            grdLocation.DataSource = dtGrid;
            grdLocation.DataBind();
            grdLocation.Columns[6].Hidden = true;
            ibDelete.Visible = false;
        }
        else //if (grdLocation.Rows.Count > 0)
        {
            grdLocation.DataSource = dtGrid;
            grdLocation.DataBind();
            if (_dtRights.Select("Rights = 'Delete' and Module = 'Location'").Length != 0)
            {
                grdLocation.Columns[6].Hidden = false;
                ibDelete.Visible = true;
            }
            else
            {
                grdLocation.Columns[6].Hidden = true;
                ibDelete.Visible = false;
            }

            if (_dtRights.Select("Rights = 'Modify' and Module = 'Location'").Length != 0)
            {
                grdLocation.Columns[5].Hidden = false;
            }
            else
            {
                grdLocation.Columns[5].Hidden = true;
            }

            if (ibDelete.Visible == true)
            {
                int LocationId;
                int iCount = 0;

                for (int i = 0; i < grdLocation.Rows.Count; i++)
                {
                    LocationId = Convert.ToInt32(((Label)(grdLocation.Rows[i].Items[6].FindControl("lblDeleteID"))).Text);
                    grdLocation.Rows[i].Items[6].FindControl("chkDelete").Visible = true;
                    objCommon = new iAssetTrack.BAL.CommonBAL();

                    DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_LOCATIONID, LocationId.ToString(), 0);
                    foreach (DataTable tblCheck in dsCheck.Tables)
                    {
                        if (tblCheck.Rows[0][0].ToString() != "0")
                        {
                            grdLocation.Rows[i].Items[6].FindControl("chkDelete").Visible = false;
                        }
                    }
                    if (grdLocation.Rows[i].Items[6].FindControl("chkDelete").Visible == false)
                    {
                        iCount = iCount + 1;
                    }

                }
                if (iCount == grdLocation.Rows.Count)
                {
                    grdLocation.Columns[6].Hidden = true;
                }
            }

            if (grdLocation.Rows.Count > 0)
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
    /// <summary>
    /// Reset fields
    /// </summary>    
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    private void clearFields()
    {
        txtLocation.Text = "";
        txtDesc.Text = "";
        lblMessage.Visible = false;
        lblMessage.Text = "";

        txtParentLocation.Text = "No Location";
        hdnLocation.Value = "No Location";
        hdnParentLocationID.Value = "0";
        ddlLocationTypeList.ClearSelection();
        //v3.8
        txtFloor.Text = string.Empty;
        txtFloor.Enabled = false;
        //v4.5.0.7
        txtSerialNo.Text = "";
        txtTag.Text = "";
        ddlMfg.ClearSelection();
        txtModel.Text = "-Select-";
        hdnModelName.Value = "-Select-";
        hdnModelID.Value = "0";
        EnableRackControls(false);
        hdnRackHasAssets.Value = "0";
        ddlLocationTypeList.Enabled = true;
        //--//
    }

    private void populateLocationType()
    {
        objLocationType = new iAssetTrack.BAL.LocationTypeBAL();
        DataSet dsLocType = objLocationType.retrieve();
        DataTable dtLocType = dsLocType.Tables[0];

        objCommon = new CommonBAL();
        objCommon.setDataSourceInfra(ddlLocationTypeList, dtLocType, "-Select-");
    }

    #endregion

    protected void grdLocation_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            populateGrid();
            objLocation = new iAssetTrack.BAL.LocationBAL();
            objLocation.LocationID = Convert.ToInt32(e.CommandArgument);
            DataSet dsLocation = objLocation.retrieve();
            DataRow dr = dsLocation.Tables[0].Rows[0];
            txtLocation.Text = dr[DBFields.DBFIELD_LOCATION].ToString();
            txtDesc.Text = dr[DBFields.DBFIELD_DESCRIPTION].ToString();
            if (dr[DBFields.DBFIELD_LOCATIONTYPEID].ToString() == "0" || dr[DBFields.DBFIELD_LOCATIONTYPEID].ToString() == "")
                ddlLocationTypeList.SelectedIndex = 0;
            else
                ddlLocationTypeList.SelectedValue = dr[DBFields.DBFIELD_LOCATIONTYPEID].ToString();

            //v3.8
            if (ddlLocationTypeList.SelectedItem.Text.CompareTo("Room") == 0)
            {
                txtFloor.Enabled = true;
                txtFloor.Text = dr[DBFields.DBFIELD_FLOOR_NO].ToString();
            }

            //v4.5.0.7
            if (ddlLocationTypeList.SelectedItem.Text.CompareTo("Rack") == 0)
            {
                EnableRackControls(true);
                if (objLocation.HasChildNodes(objLocation.LocationID) == 1)
                {
                    ddlMfg.Enabled = false;
                    hdnRackHasAssets.Value = "1";
                }
            }
            txtSerialNo.Text = dr[DBFields.DBFIELD_LOCATION_SERIAL_NO].ToString();
            txtTag.Text = dr[DBFields.DBFIELD_LOCATION_TAG_ID].ToString();
            ddlMfg.SelectedValue = dr[DBFields.DBFIELD_MFGID].ToString();

            if (ddlMfg != null && ddlMfg.Items.Count > 0)
            {
                ddlMfgName = ddlMfg.SelectedItem.Text;
                ddlMfgID = ddlMfg.SelectedItem.Value;
            }

            if (int.Parse(dr[DBFields.DBFIELD_MODELID].ToString()) == 0)
            {
                hdnModelID.Value = "0";
                hdnModelName.Value = "-Select-";
            }
            else
            {
                hdnModelName.Value = dr[DBFields.DBFIELD_MODELNAME].ToString();
                hdnModelID.Value = dr[DBFields.DBFIELD_MODELID].ToString();
            }
            txtModel.Text = hdnModelName.Value;

            //--//

            if (ddlLocationTypeList != null && ddlLocationTypeList.Items.Count > 0)
                ddlLocTypeVal = ddlLocationTypeList.SelectedItem.Text;

            DataSet AssignedLocations = objLocation.GetLocationBYSite();
            if (AssignedLocations.Tables[0].Select("LocationID=" + objLocation.LocationID).Length == 0)
            {
                // if this location is not assigned to any Site
                if (dr[DBFields.DBFIELD_PARENTLOCATIONID].ToString() == "0" || dr[DBFields.DBFIELD_PARENTLOCATIONID].ToString() == "")
                {
                    txtParentLocation.Text = "No Location";
                    hdnParentLocationID.Value = "0";
                    hdnLocation.Value = "No Location";
                }
                else
                {
                    txtParentLocation.Text = dr[DBFields.DBFIELD_PARENTLOCATION].ToString();
                    //Session["ParentLocationID"] = dr[DBFields.DBFIELD_PARENTLOCATIONID].ToString();
                    hdnParentLocationID.Value = dr[DBFields.DBFIELD_PARENTLOCATIONID].ToString();
                    hdnLocation.Value = dr[DBFields.DBFIELD_PARENTLOCATION].ToString();
                }
            }
            else
            {
                //ddlLocationList.CurrentValue = "Already Assigned to Site";
                //ddlLocationList.DisplayMode = Infragistics.Web.UI.ListControls.DropDownDisplayMode.ReadOnly;
                //txtParentLocation.Text = "Already Assigned to Site";
                // above line commented by kjb for v3.8 on 10 Feb 2014

            }

            Session["LocationID"] = objLocation.LocationID;

            //If a Room or Row has child location than disable location type cahnge
            // if a rack has assets than disable location type change
            if (bool.Parse(Session["TenantUser"].ToString()))
            {
                ddlLocationTypeList.Enabled = false;
            }
            else
            {
                if (objLocation.HasChildNodes(objLocation.LocationID) == 1)
                {
                    ddlLocationTypeList.Enabled = false;
                }
                else
                {
                    ddlLocationTypeList.Enabled = true;
                }
            }

            if (_dtRights.Select("Module = 'Location' and Rights = '" + "Modify" + "'").Length != 0)
            {
                ibCreate.Enabled = true;
            }
            else
            {
                ibCreate.Enabled = false;
            }
        }
    }



    protected void ibExportToExcel_Click(object sender, EventArgs e)
    {
        Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        this.eExporter.DataExportMode = Infragistics.Web.UI.GridControls.DataExportMode.AllDataInDataSource;
        Infragistics.Documents.Excel.Workbook wBook = new Infragistics.Documents.Excel.Workbook(excelFormat);
        this.eExporter.Export(this.grdLocation, wBook);
    }
    protected void eExporter_CellExported(object sender, Infragistics.Web.UI.GridControls.ExcelCellExportedEventArgs e)
    {
        int iRdex = e.CurrentRowIndex;
        int iCdex = e.CurrentColumnIndex;
        //Workbook wb = new Infragistics.Documents.Excel.Workbook();
        //wb.Worksheets.Contains.Columns(0).Width = len  
        //Size size;
        //using ( Font font = new Font( "Arial", (float)( workbook.DefaultFontHeight / 20.0 ) ));
        //size = TextRenderer.MeasureText( grfx, "0", font, Size.Empty, TextFormatFlags.NoPadding );
        //int width = size.Width;

        e.Worksheet.Columns[0].Width = 6000;
        e.Worksheet.Columns[3].Width = 4000;
        e.Worksheet.Columns[5].Width = 1;
        e.Worksheet.Columns[6].Width = 1;
        if (iRdex != 0)
        {
            if (iCdex == 0 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
            {
                string str = e.Worksheet.Rows[iRdex].Cells[iCdex].Value.ToString();
                char[] sep = { '<', '>' };
                Array a = str.Split(sep);
                if (a.Length > 2)
                    e.Worksheet.Rows[iRdex].Cells[iCdex].Value = a.GetValue(2);
            }
            if (iCdex == 3 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
            {
                string str = e.Worksheet.Rows[iRdex].Cells[iCdex].Value.ToString();
                char[] sep = { '<', '>' };
                Array a = str.Split(sep);
                if (a.Length > 2)
                    e.Worksheet.Rows[iRdex].Cells[iCdex].Value = a.GetValue(2);
            }

        }
        else
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

    protected void grdLocation_InitializeRow(object sender, Infragistics.Web.UI.GridControls.RowEventArgs e)
    {

        //int LocationId;
        //LocationId = Convert.ToInt32(((Label)(e.Row.Items[5].FindControl("lblDeleteID"))).Text);

        //objCommon = new iAssetTrack.BAL.CommonBAL();

        //DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_LOCATIONID, LocationId);
        //foreach (DataTable tblCheck in dsCheck.Tables)
        //{
        //    if (tblCheck.Rows[0][0].ToString() != "0")
        //    {
        //        e.Row.Items[5].FindControl("chkDelete").Visible = false;
        //    }
        //}

        //if (((Label)(grdLocation.Rows[i].Items[3].FindControl("IsExitDoor"))).Text.ToLower().Equals("true"))
        //    ((Label)(grdLocation.Rows[i].Items[3].FindControl("IsExitDoor"))).Text = "Yes";
        //else
        //    ((Label)(grdLocation.Rows[i].Items[3].FindControl("IsExitDoor"))).Text = "No";

        //if (((Label)(grdLocation.Rows[i].Items[2].FindControl("IsCheckOutLocation"))).Text.ToLower().Equals("true"))
        //    ((Label)(grdLocation.Rows[i].Items[2].FindControl("IsCheckOutLocation"))).Text = "Yes";
        //else
        //    ((Label)(grdLocation.Rows[i].Items[2].FindControl("IsCheckOutLocation"))).Text = "No";


    }

    protected void ddlMfg_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMfg != null && ddlMfg.Items.Count > 0)
        {
            ddlMfgName = ddlMfg.SelectedItem.Text;
            ddlMfgID = ddlMfg.SelectedItem.Value;
        }
        txtModel.Text = "-Select-";
        hdnModelID.Value = "0";
        hdnModelName.Value = "-Select-";
        //V000013
        //Added on 24-08-2012
        //ibExport.Visible = true;
        //ibExport.Enabled = true;
        //above line commented by kjb on 28 Feb 2013
    }

    private void EnableRackControls(bool OnOff)
    {
        txtSerialNo.Enabled = OnOff;
        ddlMfg.Enabled = OnOff;
        //v5.1 - below statement is disabled to support Tag functionality for all locations, i.e. Room / Row / Rack
        //txtTag.Enabled = OnOff;
    }

    protected void ddlLocationTypeList_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (bool.Parse(Session["TenantUser"].ToString()))
        {
            UserBAL objUser = new UserBAL();
            objUser.UserID = Convert.ToInt32(Session["UserID"]);
            DataSet dsTenant = objUser.retrieveTenantDetails();
            if (dsTenant.Tables.Count > 0 && dsTenant.Tables[0].Rows.Count > 0)
            {
                string tenantType = dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_TYPE].ToString().ToLower();
                switch (tenantType)
                {
                    case "room":
                        if (ddlLocationTypeList.SelectedItem.Text.ToLower().CompareTo("room") == 0)
                        {
                            lblMessage.Text = "Location type: Room not allowed for this tenant";
                            lblMessage.Visible = true;
                            ddlLocationTypeList.SelectedIndex = 0;
                            return;
                        }
                        break;
                    case "row":
                        if (ddlLocationTypeList.SelectedItem.Text.ToLower().CompareTo("room") == 0 || ddlLocationTypeList.SelectedItem.Text.ToLower().CompareTo("row") == 0)
                        {
                            lblMessage.Text = "Location type: Room or Row not allowed for this tenant";
                            lblMessage.Visible = true;
                            ddlLocationTypeList.SelectedIndex = 0;
                            return;
                        }
                        break;
                    case "rack":
                        if (ddlLocationTypeList.SelectedItem.Text.ToLower().CompareTo("room") == 0 ||
                            ddlLocationTypeList.SelectedItem.Text.ToLower().CompareTo("row") == 0 || ddlLocationTypeList.SelectedItem.Text.ToLower().CompareTo("rack") == 0)
                        {
                            lblMessage.Text = "Creation of new locations is not allowed for this tenant";
                            lblMessage.Visible = true;
                            ddlLocationTypeList.SelectedIndex = 0;
                            return;
                        }
                        break;
                }

                if (ddlLocationTypeList != null && ddlLocationTypeList.Items.Count > 0)
                    ddlLocTypeVal = ddlLocationTypeList.SelectedItem.Text;
                hdnLocation.Value = "No Location";
                hdnParentLocationID.Value = "0";
                txtParentLocation.Text = "No Location";
                //v3.8
                //check if room is selected as loc type than only floor 
                //text box will be enabled
                if (ddlLocationTypeList.SelectedItem.Text.CompareTo("Room") == 0)
                {
                    txtFloor.Enabled = true;
                }
                else
                {
                    txtFloor.Enabled = false;
                    txtFloor.Text = "";
                }

                if (ddlLocationTypeList.SelectedItem.Text.CompareTo("Rack") == 0)
                {
                    EnableRackControls(true);
                }
                else
                {
                    EnableRackControls(false);
                    txtSerialNo.Text = "";
                    txtTag.Text = "";
                    ddlMfg.SelectedIndex = 0;
                }

            }
        }
        else
        {
            if (ddlLocationTypeList != null && ddlLocationTypeList.Items.Count > 0)
                ddlLocTypeVal = ddlLocationTypeList.SelectedItem.Text;
            hdnLocation.Value = "No Location";
            hdnParentLocationID.Value = "0";
            txtParentLocation.Text = "No Location";
            //v3.8
            //check if room is selected as loc type than only floor 
            //text box will be enabled
            if (ddlLocationTypeList.SelectedItem.Text.CompareTo("Room") == 0)
            {
                txtFloor.Enabled = true;
            }
            else
            {
                txtFloor.Enabled = false;
                txtFloor.Text = "";
            }

            if (ddlLocationTypeList.SelectedItem.Text.CompareTo("Rack") == 0)
            {
                EnableRackControls(true);
            }
            else
            {
                EnableRackControls(false);
                txtSerialNo.Text = "";
                txtTag.Text = "";
                ddlMfg.SelectedIndex = 0;
            }
        }
    }
    protected void grdLocation_DataFiltered(object sender, Infragistics.Web.UI.GridControls.FilteredEventArgs e)
    {
        // Cast the WebDataGrid DataSource to a DataTable  
        DataTable dt = this.grdLocation.DataSource as DataTable;
        // Disable WebDataGrid Paging
        grdLocation.Behaviors.Paging.Enabled = false;
        this.FilterCount = grdLocation.Rows.Count.ToString();
        if (!string.IsNullOrEmpty(this.FilterCount))
        {
            if (totalRecordCount == int.Parse(this.FilterCount))
            {
                this.FilterCount = "";
            }

        }
        // Enable WebDataGrid Paging
        grdLocation.Behaviors.Paging.Enabled = true;

    }

}
