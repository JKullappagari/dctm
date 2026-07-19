using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using iAssetTrack.BAL;
using iAssetTrack.DALC;
using iAssetTrackBAL;
using Infragistics.Web.UI.NavigationControls;
using System.Web.Services;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

public partial class CreateAssetPopup : System.Web.UI.Page
{
    protected string ddlSiteVal;
    protected string ddlMfgName = string.Empty;
    protected string ddlMfgID = string.Empty;
    protected string ddlBusinessUnitVal = string.Empty;
    protected bool isParent = false;
    public static ArrayList arrList = new ArrayList();
    public static ArrayList tempHosts = new ArrayList();
    DataTable _dtRights;
    private ManufacturerBAL objMfg;
    private const string PROP_NO_OF_RUS = "NoOfRUs";
    private const string PROP_MODEL_TYPE_ID = "ModelTypeID";
    private const string PROP_MODEL_TYPE = "ModelType";
    private const string PROP_MOUNT_TYPE = "MountType";
    private const string PROP_LOCATION_TYPE = "LocationType";
    private const string PROP_IS_BLADE = "IsBlade";
    private const string TAB_KEY_SUMMARY = "Summary";
    private const string TAB_KEY_AUTHORIZATION = "Authorization";
    private const string TAB_KEY_ASSOCIATION = "Association";

    private const string VIEWSTATE_AUTHORIZATION = "Authorization";
    private const string VIEWSTATE_ASSOCIATION = "Association";
    private const string VIEWSTATE_DOCUMENTID = "AssetID";

    private const string PROP_ENCL_FRONT_ROW_COUNT = "EnclFrontRowCount";
    private const string PROP_ENCL_FRONT_COL_COUNT = "EnclFrontColCount";
    private const string PROP_ENCL_REAR_ROW_COUNT = "EnclRearRowCount";
    private const string PROP_ENCL_REAR_COL_COUNT = "EnclRearColCount";
    private const string PROP_BLADE_ROW_COUNT = "BladeRowCount";
    private const string PROP_BLADE_COL_COUNT = "BladeColCount";
    private const string PROP_START_POSITION = "StartPosition";
    private const string PROP_ORIENTATION = "Orientation";

    private const string ISPARENT_IMAGE = "infragistics/images/Outlook2003/folders.gif";
    private const string ISCHILD_IMAGE = "infragistics/images/Outlook2003/folder.gif";

    private bool IsAuthorizationTabInitialized
    {
        get { return (ViewState[VIEWSTATE_AUTHORIZATION] != null ? Convert.ToBoolean(ViewState[VIEWSTATE_AUTHORIZATION]) : false); }
        set { ViewState[VIEWSTATE_AUTHORIZATION] = value; }
    }

    private bool IsAssociationTabInitialized
    {
        get { return (ViewState[VIEWSTATE_ASSOCIATION] != null ? Convert.ToBoolean(ViewState[VIEWSTATE_ASSOCIATION]) : false); }
        set { ViewState[VIEWSTATE_ASSOCIATION] = value; }
    }

    public bool IsBlade
    {
        get
        {
            return (ViewState[PROP_IS_BLADE] != null ? Convert.ToBoolean(ViewState[PROP_IS_BLADE].ToString()) : false);
        }
        set
        {
            ViewState[PROP_IS_BLADE] = value;
        }
    }

    public int NoOfRUs
    {
        get
        {
            return (ViewState[PROP_NO_OF_RUS] != null ? Convert.ToInt32(ViewState[PROP_NO_OF_RUS].ToString()) : 0);
        }
        set
        {
            ViewState[PROP_NO_OF_RUS] = value;
        }
    }

    public int ModelTypeID
    {
        get
        {
            return (ViewState[PROP_MODEL_TYPE_ID] != null ? Convert.ToInt32(ViewState[PROP_MODEL_TYPE_ID].ToString()) : 0);
        }
        set
        {
            ViewState[PROP_MODEL_TYPE_ID] = value;
        }
    }

    public string ModelType
    {
        get
        {
            return (ViewState[PROP_MODEL_TYPE] != null ? ViewState[PROP_MODEL_TYPE].ToString() : "");
        }
        set
        {
            ViewState[PROP_MODEL_TYPE] = value;
        }
    }

    public string MountType
    {
        get
        {
            return (ViewState[PROP_MOUNT_TYPE] != null ? ViewState[PROP_MOUNT_TYPE].ToString() : "");
        }
        set
        {
            ViewState[PROP_MOUNT_TYPE] = value;
        }
    }

    public string LocationType
    {
        get
        {
            return (ViewState[PROP_LOCATION_TYPE] != null ? ViewState[PROP_LOCATION_TYPE].ToString() : "");
        }
        set
        {
            ViewState[PROP_LOCATION_TYPE] = value;
        }
    }


    private int AssetID
    {
        get { return (ViewState[VIEWSTATE_DOCUMENTID] != null ? Convert.ToInt32(ViewState[VIEWSTATE_DOCUMENTID]) : 0); }
        set { ViewState[VIEWSTATE_DOCUMENTID] = value; }
    }

    public int EnclFrontRowCount
    {
        get
        {
            return (ViewState[PROP_ENCL_FRONT_ROW_COUNT] != null ? Convert.ToInt32(ViewState[PROP_ENCL_FRONT_ROW_COUNT].ToString()) : 0);
        }
        set
        {
            ViewState[PROP_ENCL_FRONT_ROW_COUNT] = value;
        }
    }

    public int EnclFrontColCount
    {
        get
        {
            return (ViewState[PROP_ENCL_FRONT_COL_COUNT] != null ? Convert.ToInt32(ViewState[PROP_ENCL_FRONT_COL_COUNT].ToString()) : 0);
        }
        set
        {
            ViewState[PROP_ENCL_FRONT_COL_COUNT] = value;
        }
    }

    public int EnclRearRowCount
    {
        get
        {
            return (ViewState[PROP_ENCL_REAR_ROW_COUNT] != null ? Convert.ToInt32(ViewState[PROP_ENCL_REAR_ROW_COUNT].ToString()) : 0);
        }
        set
        {
            ViewState[PROP_ENCL_REAR_ROW_COUNT] = value;
        }
    }

    public int EnclRearColCount
    {
        get
        {
            return (ViewState[PROP_ENCL_REAR_COL_COUNT] != null ? Convert.ToInt32(ViewState[PROP_ENCL_REAR_COL_COUNT].ToString()) : 0);
        }
        set
        {
            ViewState[PROP_ENCL_REAR_COL_COUNT] = value;
        }
    }

    public int BladeRowCount
    {
        get
        {
            return (ViewState[PROP_BLADE_ROW_COUNT] != null ? Convert.ToInt32(ViewState[PROP_BLADE_ROW_COUNT].ToString()) : 0);
        }
        set
        {
            ViewState[PROP_BLADE_ROW_COUNT] = value;
        }
    }

    public int BladeColCount
    {
        get
        {
            return (ViewState[PROP_BLADE_COL_COUNT] != null ? Convert.ToInt32(ViewState[PROP_BLADE_COL_COUNT].ToString()) : 0);
        }
        set
        {
            ViewState[PROP_BLADE_COL_COUNT] = value;
        }
    }

    public int StartPosition
    {
        get
        {
            return (ViewState[PROP_START_POSITION] != null ? Convert.ToInt32(ViewState[PROP_START_POSITION].ToString()) : 0);
        }
        set
        {
            ViewState[PROP_START_POSITION] = value;
        }
    }

    public string Orientation
    {
        get
        {
            return (ViewState[PROP_ORIENTATION] != null ? ViewState[PROP_ORIENTATION].ToString() : "");
        }
        set
        {
            ViewState[PROP_ORIENTATION] = value;
        }
    }




    private const string USER_IMAGE_FILE = "MSNExplorer/People.gif"; // "~/images/user.gif";

    //Order the First Column in Sequence for maximum efficiency
    private String[,] saModuleRightsControlList = new String[10, 3]
    {
        {"Create", "ddlBusinessUnit", "Enabled"},
        {"Create", "ddlSite", "Enabled"},
        {"Create", "txtSerialNo", "Enabled"},
        {"Create", "txtHostName", "Enabled"},
        {"Create", "ddlAssetGroup", "Enabled"},
        {"Create", "hlSearch", "Visible,Enabled"},
        {"Create", "wdcCreatedDate", "Enabled"},
        {"Create", "numPageCount", "Enabled"},
        //{"Create", "uwtCreateDocument.Summary", "Visible"}, -- Allow to see the summary, but do not allow editing
        {"Create", "ibCreateAsset", "Enabled"},
        {"Create", "ibCreateAsset2", "Enabled"},
        //{"Authorize", "uwtDistributionList", "Enabled"},
        //{"Authorize", "uwtCreateDocument.Authorization", "Visible"},
        //{"Associate", "ucxAssetSearchControl", "Visible"},
        //{"Associate", "uwtCreateDocument.Association", "Visible"}

    };

    [WebMethod(EnableSession = true)]
    public static string[] getHostNames(string prefixText, int count)
    {
        string[] arrHost;
        HostBAL objHost = new HostBAL();
        DataSet dsHost = objHost.retrieve();
        if (bool.Parse(HttpContext.Current.Session["TenantUser"].ToString()))
        {
            ArrayList tenantAssignedHosts = new ArrayList();
            UserBAL objUser = new UserBAL();
            objUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
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
                    DataTable tempTable = dsHost.Tables[0].Clone();
                    foreach (DataRow dr in dsHost.Tables[0].Rows)
                    {
                        tempTable.Rows.Add(dr.ItemArray);
                    }
                    DataRow[] filteredRows = tempTable.Select("HostID IN (" + string.Join(",", tenantAssignedHosts.ToArray()) + ")");
                    dsHost.Tables[0].Rows.Clear();
                    if (filteredRows != null && filteredRows.Length > 0)
                    {
                        foreach (DataRow dr in filteredRows)
                        {
                            dsHost.Tables[0].Rows.Add(dr.ItemArray);
                        }

                    }
                }
                else
                {
                    dsHost.Tables[0].Rows.Clear();
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
                DataTable tempTable = dsHost.Tables[0].Clone();
                foreach (DataRow dr in dsHost.Tables[0].Rows)
                {
                    tempTable.Rows.Add(dr.ItemArray);
                }
                DataRow[] filteredRows = tempTable.Select("HostID NOT IN (" + string.Join(",", tenantAssignedHosts.ToArray()) + ")");
                dsHost.Tables[0].Rows.Clear();
                if (filteredRows != null && filteredRows.Length > 0)
                {
                    foreach (DataRow dr in filteredRows)
                    {
                        dsHost.Tables[0].Rows.Add(dr.ItemArray);
                    }

                }
            }
        }
        arrList.Clear();
        //tempHosts.Clear();
        foreach (DataRow dr in dsHost.Tables[0].Rows)
        {
            if (dr[1].ToString().ToLower().StartsWith(prefixText.ToLower()))
            {
                arrList.Add(dr[1].ToString());
            }
        }
        arrHost = (string[])arrList.ToArray(typeof(string));

        return arrHost;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "CreateAssetPopup.aspx";
            Response.Redirect("Login.aspx");
        }

        this.wdcCreatedDate.MaxValue = DateTime.Now;

        if (SiteMap.CurrentNode == null)
        {
            Session["PageHeader"] = "Edit Asset";
        }
        else
        {
            Session["PageHeader"] = SiteMap.CurrentNode.Description;
        }
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        

        bool blfoundPage = false;
        if (_dtRights.Select("Module = 'Create Asset'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }

        if (_dtRights.Select("Module = 'Create Asset' and Rights = '" + "Create" + "'").Length != 0)
        {
            ibCreateAsset.Enabled = true;
        }
        else
        {
            ibCreateAsset.Enabled = false;
        }

        if (Request.QueryString["ID"] != null & Request.QueryString["ID"] != "")
        {
            //Edit Asset
            this.AssetID = Convert.ToInt32(Request.QueryString["ID"]);
            Session["AssetID"] = Request.QueryString["ID"];
            hdnAssetID.Value = Session["AssetID"].ToString();
        }

        if (Request.QueryString["Mode"] != null)
        {
            if (Request.QueryString["Mode"].ToString().Equals("E"))
            {
                this.ibClose.Enabled = true;
                this.ibClose.Visible = true;

                this.ibClose2.Enabled = true;
                this.ibClose2.Visible = true;
            }
            else
            {
                this.ibClose.Enabled = false;
                this.ibClose.Visible = false;

                this.ibClose2.Enabled = false;
                this.ibClose2.Visible = false;
            }
        }
        else
        {
            this.ibClose.Enabled = false;
            this.ibClose.Visible = false;

            this.ibClose2.Enabled = false;
            this.ibClose2.Visible = false;
        }

        if (!string.IsNullOrEmpty(hdnModelName.Value))
            txtModel.Text = hdnModelName.Value;

        if (!string.IsNullOrEmpty(hdnLocName.Value))
            txtLocation.Text = hdnLocName.Value;

        if (bool.Parse(Session["TenantUser"].ToString()))
        {
            ArrayList assignedAssets = new ArrayList();
            UserBAL objUser = new UserBAL();
            objUser.UserID = Convert.ToInt32(Session["UserID"]);
            DataSet dsTenant = objUser.retrieveTenantDetails();
            if (dsTenant.Tables.Count > 0 && dsTenant.Tables[0].Rows.Count > 0)
            {
                TenantBAL objTenant = new TenantBAL();
                objTenant.TenantId = Convert.ToInt32(dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_ID].ToString());
                DataSet dsTA = objTenant.retrieveAssetAssignmentList();
                if (dsTA.Tables.Count > 0 && dsTA.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drA in dsTA.Tables[0].Rows)
                    {
                        if (!assignedAssets.Contains((drA[DBFields.DBFIELD_ASSETID].ToString())))
                            assignedAssets.Add(drA[DBFields.DBFIELD_ASSETID].ToString());
                    }
                }
                if (assignedAssets.Count > 0 && assignedAssets.Contains(Session["AssetID"].ToString()))
                {
                    //do nothing
                }
                else
                {
                    Response.Redirect("~/LogoutMe.aspx");
                }

            }

        }

        if (!Page.IsPostBack)
        {
            //lblMessage.Visible = false;
            lblErrorMsg.Visible = false;
            ddlBusinessUnit.Enabled = false;
            populateOrientationList();
            populateBusinessUnitList();
            populateManufacturerList();
            PopulatetechList();
            LoadStartPositions(false, "", 0, "");
            FillAssetDetails(this.AssetID);
            this.txtSerialNo.Focus();
        }
        AddHostsToList();
        if (!string.IsNullOrEmpty(txtParentAsset.Text))
        {
            ddlSite.Enabled = false;
        }
        //Business Unit ID for TreeList call
        if (ddlBusinessUnit != null && ddlBusinessUnit.Items.Count > 0)
            ddlBusinessUnitVal = ddlBusinessUnit.SelectedItem.Value;
        if (ddlMfg != null && ddlMfg.Items.Count > 0)
        {
            ddlMfgName = ddlMfg.SelectedItem.Text;
            ddlMfgID = ddlMfg.SelectedItem.Value;
        }
        if (ddlSite != null && ddlSite.Items.Count > 0)
        {
            ddlSiteVal = ddlSite.SelectedItem.Value;
        }
        if (!string.IsNullOrEmpty(hdnParentAssetName.Value.ToString().Trim()))
        {
            txtParentAsset.Text = hdnParentAssetName.Value.ToString();
        }
        if (!string.IsNullOrEmpty(hdnLocName.Value.ToString().Trim()))
        {
            txtLocation.Text = hdnLocName.Value.ToString();
        }

        //disable all location specific controls as Location change is not allowed in Edit Asset
        // Site, Location selection, Removing or changing Parent Enclosure is not allowed.
        imgRemoveParent.Visible = false;
        HyperLink1.Visible = false;
        ddlSite.Enabled = false;
        //enable - disable Model selection icon based on current location of the asset
        // If asset in Room or Row than allow model change otherwise not allow.
        string strScript = string.Empty;
        strScript = "<script type=\"text/javascript\">setModelDisplay('" + this.LocationType.ToLower() + "').disabled = true;</script>";
        if (!Page.ClientScript.IsStartupScriptRegistered("ModelSelvalidation"))
            Page.ClientScript.RegisterStartupScript(typeof(Page), "ModelSelvalidation", strScript);
        if (this.LocationType.ToLower().Equals("rack"))
            ddlMfg.Enabled = false;
        else
            ddlMfg.Enabled = true;
    }

    private void populateManufacturerList()
    {
        if (ddlMfg.Items.Count > 0)
            ddlMfg.Items.Clear();

        objMfg = new iAssetTrack.BAL.ManufacturerBAL();
        DataSet dsMfg = objMfg.retrieve();
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

    #region Populate Controls


    private void populateBusinessUnitList()
    {
        int intUserID = 0;
        if (Session["UserID"] != null)
            intUserID = Convert.ToInt32(Session["UserID"].ToString());

        FillDropDownListBU(StoredProcedures.SP_BUSINESSUNIT_LISTBYUSERID, ref ddlBusinessUnit, intUserID);
    }

    private void FillDropDownListBU(string strStoredProc, ref DropDownList ddl, int id)
    {

        iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();

        DataTable dt = objCommon.FillDropDownListBU(strStoredProc, "- Select -", id);
        ddl.DataSource = dt;
        ddl.DataValueField = dt.Columns[0].ToString();
        ddl.DataTextField = dt.Columns[1].ToString();
        ddl.DataBind();

    }

    /// <summary>
    /// Populate the Department for the Selected BU
    /// </summary>
    /// <param name="pBusineddUnitId"></param>
    private void populateSites(int pBusineddUnitId)
    {
        ddlSite.Items.Clear();

        if (!ddlSite.Visible) return;
        if (pBusineddUnitId <= 0) return;

        //existing code
        ListItem emptyitem = new ListItem("- Select -", "0");
        ddlSite.Items.Add(emptyitem);

        SitesBAL objsite = new SitesBAL();

        DataSet ds = objsite.retrieveByBusinessUnitId(pBusineddUnitId);
        DataTable dt = ds.Tables[0];
        ddlSite.DataSource = dt;
        ddlSite.DataValueField = dt.Columns["SiteID"].ToString();
        ddlSite.DataTextField = dt.Columns["Site"].ToString();
        ddlSite.DataBind();
        if (ddlSite.SelectedIndex > 0)
            hdnSiteID.Value = ddlSite.SelectedItem.Value.ToString();
        else
            hdnSiteID.Value = "";

    }


    //protected void cusCustom_ServerValidate(object sender, ServerValidateEventArgs e)
    //{
    //    int val = Int32.Parse(e.Value);
    //    if (val > 0)
    //        e.IsValid = true;
    //    else
    //        e.IsValid = false;
    //}

    //protected void cusCustom_ServerValidate_ddlMfg(object sender, ServerValidateEventArgs e)
    //{
    //    int val = Int32.Parse(e.Value);
    //    if (val > 0)
    //        e.IsValid = true;
    //    else
    //        e.IsValid = false;
    //}



    #endregion Populate Controls

    #region Database Retrieval and Updates

    private void Create()
    {
        try
        {

            AssetBAL docBAL = new AssetBAL();
            docBAL.AssetID = this.AssetID;
            //serial no
            if (!string.IsNullOrEmpty(txtSerialNo.Text.Trim()))
                docBAL.RefNumber = this.txtSerialNo.Text.Trim();
            else
                docBAL.RefNumber = "0";
            //Host Name
            string strHosts = string.Empty;
            if (!string.IsNullOrEmpty(hdnHostNames.Value))
            {
                strHosts = hdnHostNames.Value.Trim(',');
            }
            docBAL.HostName = strHosts;
            //Asset Name
            docBAL.AssetName = txtAssetNameA.Text.Trim();
            //asset type or asset group
            docBAL.AssetTypeId = Convert.ToInt32(this.ModelTypeID);
            //ModelID
            if (!string.IsNullOrEmpty(hdnModelID.Value))
                docBAL.ModelID = Convert.ToInt32(hdnModelID.Value);
            else
                docBAL.ModelID = 0;
            //LocationiD
            int locationID = 0;
            if (!string.IsNullOrEmpty(hdnLocID.Value))
            {
                if (!string.IsNullOrEmpty(hdnLocID.Value))
                    locationID = int.Parse(hdnLocID.Value.ToString());
            }
            docBAL.LastSeenLocationID = locationID;
            docBAL.DefaultLocationID = locationID;
            //Tech Category
            docBAL.TechID = Convert.ToInt32(this.ddlTechCat.SelectedValue);
            //MOunt Type
            docBAL.RackOrStand = this.MountType;
            // BU
            docBAL.BusinessUnitID = Convert.ToInt32(this.ddlBusinessUnit.SelectedValue);
            //Site
            if (string.IsNullOrEmpty(hdnParentAssetName.Value))
            {
                docBAL.PrimarySiteID = Convert.ToInt32(this.ddlSite.SelectedValue);
            }
            else
            {
                docBAL.PrimarySiteID = Convert.ToInt32(hdnSiteID.Value);
            }
            //Create Date
            if (!string.IsNullOrEmpty(this.wdcCreatedDate.Text) && this.wdcCreatedDate.Text.CompareTo("-Select-") != 0)
                docBAL.AssetCreatedDate = Convert.ToDateTime(this.wdcCreatedDate.Text);
            //Created By
            docBAL.AssetCreatedBy = Convert.ToInt32(Session["UserID"].ToString());
            docBAL.UpdatedBy = Convert.ToInt32(Session["UserID"].ToString());
            //Owner
            if (!string.IsNullOrEmpty(hdnOwnerID.Value.ToString()))
                docBAL.CurrentOwnerID = Convert.ToInt32(this.hdnOwnerID.Value);
            else
                docBAL.CurrentOwnerID = 0;
            //Parent Asset
            if (!string.IsNullOrEmpty(hdnParentAssetID.Value))
                docBAL.AssetParentID = Convert.ToInt32(hdnParentAssetID.Value);
            else
                docBAL.AssetParentID = 0;
            //IsParent
            if (docBAL.ModelID > 0)
            {
                AssetModelBAL modelBAL = new AssetModelBAL();
                modelBAL.ModelID = docBAL.ModelID;
                DataSet dsmodel = modelBAL.retrieve();
                if (dsmodel != null && dsmodel.Tables.Count > 0 && dsmodel.Tables[0].Rows.Count > 0)
                {
                    docBAL.IsParent = (Convert.ToBoolean(dsmodel.Tables[0].Rows[0][DBFields.DBFIELD_IS_ENCLOSURE].ToString()) == true ? true : false);
                }
                else
                    docBAL.IsParent = false;
            }
            else
                docBAL.IsParent = false;
            //OS
            if (txtOS.Text != string.Empty)
                docBAL.OS = txtOS.Text.Trim();
            //CPU
            if (txtCPU.Text != string.Empty)
                docBAL.CPU = txtCPU.Text.Trim();
            //CPU COunt
            if (txtCpuCount.Text != string.Empty)
                docBAL.CPUCount = Int32.Parse(txtCpuCount.Text);
            //CPU Core
            if (txtCPUCore.Text != string.Empty)
                docBAL.CPUCore = txtCPUCore.Text;
            //Position
            if (ddlPosition.SelectedIndex > 0)
            {
                docBAL.StartPos = Convert.ToInt16(ddlPosition.SelectedValue);

                //if device is vertical mount or standalone than startp position will be zero
                if (this.MountType.Contains("Vertical") || this.MountType.Contains("Standalone"))
                {
                    docBAL.StartPos = 0;
                }

                if (!IsBlade)
                {
                    //if device is placed in a row or room
                    if (this.LocationType.ToLower().CompareTo("rack") != 0)
                    {
                        docBAL.StartPos = 0;
                    }
                }
                else
                {
                    if (docBAL.AssetParentID == 0)
                        docBAL.StartPos = 0;
                }

            }
            //No Of RUs
            docBAL.NoOfRUs = this.NoOfRUs;
            //Orientation
            if (ddlOrientation.SelectedIndex > 0)
            {
                docBAL.Orientation = ddlOrientation.SelectedItem.Text;
            }
            //
            docBAL.IsImport = false;
            docBAL.SerialNoModelCheck = (Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["ImportAssetUniqueFilter"].ToString()) == 1 ? true : false);
            //Asset tag
            docBAL.AssetTAG = txtBarcode.Text.Trim();
            //Derated Power
            float deratedPower;
            if (!string.IsNullOrEmpty(txtDerated.Text.Trim()))
            {
                if (float.TryParse(txtDerated.Text.Trim(), out deratedPower))
                    docBAL.DeratedPower = deratedPower;
                else
                    docBAL.DeratedPower = 0.0f;
            }
            else
                docBAL.DeratedPower = 0.0f;

            docBAL.Persist(DALCOperation.Update);

            if (docBAL.Result == 0)
            {
                if (this.AssetID == 0)
                {
                    //hdnDocId.Value = Convert.ToString(docBAL.AssetID);
                    //lblMessage.Text = new CommonBAL().displayMessage(MessageCodes.GEN_S_INSERTED);
                    //lblMessage.Visible = true;
                    //lblMessage.ForeColor = System.Drawing.Color.Black;

                    lblErrorMsg.Text = new CommonBAL().displayMessage(MessageCodes.GEN_S_INSERTED);
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    rfvLocation.Visible = false;//V3.8-Added on 23Oct 2013-By Amar Vidya
                }
                else
                {
                    //lblMessage.Text = new CommonBAL().displayMessage(MessageCodes.GEN_S_UPDATED);
                    //lblMessage.Visible = true;
                    //lblMessage.ForeColor = System.Drawing.Color.Black;

                    lblErrorMsg.Text = new CommonBAL().displayMessage(MessageCodes.GEN_S_UPDATED);
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                }
                //release earlier position.

                ResetForm();
                FillAssetDetails(this.AssetID);
            }
            else
            {
                if (docBAL.MessageCode.ToLower().CompareTo("position_is_in_use") == 0)
                {
                    lblErrorMsg.Text = "Asset can't be placed in selected position, choose another.";
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                else
                {
                    CommonBAL objCommon = new CommonBAL();
                    MessageBAL objMessage = new MessageBAL();
                    objMessage.MessageCode = docBAL.MessageCode;

                    if (!string.IsNullOrEmpty(docBAL.MessageCode))
                    {
                        string displayMsg = objMessage.retrieve();
                        if (!string.IsNullOrEmpty(displayMsg))
                            lblErrorMsg.Text = displayMsg;
                        else
                            lblErrorMsg.Text = docBAL.MessageCode;
                    }

                    //lblErrorMsg.Text = docBAL.MessageCode;
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            //lblErrorMsg.Text = ex.Message;
            //lblErrorMsg.Text = "Asset creation failed, check error log for more information";
            ExceptionPolicy.HandleException(ex, "errDCTrack");
            throw (ex);
        }
    }

    private void FillAssetDetails(int pDocumentId)
    {
        AssetBAL docBAL = new AssetBAL();
        LocationBAL ObjLocation = new iAssetTrack.BAL.LocationBAL();
        ManufacturerBAL objMfg = new ManufacturerBAL();
        AssetModelBAL objAM = new AssetModelBAL();
        TechCatBAL objTC = new TechCatBAL();

        docBAL.AssetID = pDocumentId;
        DataSet ds = docBAL.Retrieve();


        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtBarcode.Text = dr["CurrentRFIDCardNumber"].ToString();
            hdnAssetID.Value = pDocumentId.ToString();
            txtSerialNo.Text = dr["RefNumber"].ToString();
            //Host Name
            if (!string.IsNullOrEmpty(dr[DBFields.DBFIELD_HOST].ToString()))
                hdnHostNames.Value = dr[DBFields.DBFIELD_HOST].ToString() + ",";
            else
                hdnHostNames.Value = "";

            AddHostsToList();
            //BU
            ddlBusinessUnit.AutoPostBack = false;
            ddlBusinessUnit.SelectedValue = dr[DBFields.DBFIELD_BUSINESSUNITID].ToString();
            populateSites(Convert.ToInt32(dr[DBFields.DBFIELD_BUSINESSUNITID].ToString()));
            //Site
            ddlSite.SelectedValue = dr["PrimarySiteID"].ToString();
            //Asset name
            txtAssetNameA.Text = dr["AssetName"].ToString();
            //Location
            LocationBAL objLoc = new LocationBAL();
            objLoc.LocationID = Int32.Parse(dr["LastSeenLocationID"].ToString());
            DataSet dsLoc = objLoc.retrieve();
            if (dsLoc.Tables.Count > 3 && dsLoc.Tables[3].Rows.Count > 0)
            {
                DataRow drLoc = dsLoc.Tables[3].Rows[0];
                if (drLoc[DBFields.DBFIELD_LOCATIONID].ToString() == "0" || drLoc[DBFields.DBFIELD_LOCATIONID].ToString() == "")
                {
                    txtLocation.Text = "";
                    hdnLocID.Value = "";
                    hdnLocationName.Value = "";
                }
                else
                {
                    txtLocation.Text = drLoc[DBFields.DBFIELD_LOCATION].ToString();
                    hdnLocID.Value = drLoc[DBFields.DBFIELD_LOCATIONID].ToString();
                    hdnLocationName.Value = drLoc[DBFields.DBFIELD_LOCATION].ToString();
                    //Set location type
                    if (!string.IsNullOrEmpty(hdnLocID.Value))
                    {
                        if (dsLoc != null && dsLoc.Tables.Count > 1 && dsLoc.Tables[1].Rows.Count > 0)
                        {
                            DataRow[] rowsL = dsLoc.Tables[1].Select(" LocationId =" + hdnLocID.Value.ToString());
                            if (rowsL != null && rowsL.Length > 0)
                            {
                                int locTypeId = int.Parse(rowsL[0][DBFields.DBFIELD_LOCATIONTYPEID].ToString());
                                LocationTypeBAL objLT = new LocationTypeBAL();
                                objLT.LocationTypeID = locTypeId;
                                DataSet dsLT = objLT.retrieve();
                                if (dsLT != null && dsLT.Tables.Count > 0 && dsLT.Tables[0].Rows.Count > 0)
                                {
                                    this.LocationType = dsLT.Tables[0].Rows[0][DBFields.DBFIELD_LOCATIONTYPE].ToString();
                                }
                            }
                        }
                    }
                }
            }

            hdnLocationID.Value = dr["DefaultLocationID"].ToString();
            //MOdel and Mfg
            DataSet dsAM = objAM.retrieveByModelID(Int32.Parse(dr["ModelID"].ToString()));
            if (dsAM.Tables[0].Rows.Count > 0)
            {
                int mfgID = Int32.Parse(dsAM.Tables[0].Rows[0]["MfgID"].ToString());
                objMfg.MFGID = mfgID;
                DataSet dsMfg = objMfg.retrieve();
                if (dsMfg.Tables[0].Rows.Count > 0)
                {
                    ddlMfg.SelectedValue = dsMfg.Tables[0].Rows[0]["MfgID"].ToString();
                }

                if (dr[DBFields.DBFIELD_MODELID].ToString() == "0" || dr[DBFields.DBFIELD_MODELID].ToString() == "")
                    txtModel.Text = "";
                else
                {
                    txtModel.Text = dsAM.Tables[0].Rows[0][DBFields.DBFIELD_MODELNAME].ToString();
                    hdnModelID.Value = dsAM.Tables[0].Rows[0][DBFields.DBFIELD_MODELID].ToString();
                    hdnModelName.Value = dsAM.Tables[0].Rows[0][DBFields.DBFIELD_MODELNAME].ToString();
                    hdnIsBlade.Value = dsAM.Tables[0].Rows[0][DBFields.DBFIELD_IS_BLADE].ToString();//IsBlade
                }
            }


            // Technology Category
            if (!string.IsNullOrEmpty(dr[DBFields.DBFIELD_TECHID].ToString()))
            {
                ddlTechCat.SelectedValue = dr[DBFields.DBFIELD_TECHID].ToString();
                ddlTechCat.Items.FindByValue(dr[DBFields.DBFIELD_TECHID].ToString()).Attributes.Add("style", "background-color:LightBlue;");
            }
            else
            {
                DataSet dsTC = objTC.retrieveByAssetID(Int32.Parse(dr["AssetID"].ToString()));
                ddlTechCat.SelectedValue = dsTC.Tables[0].Rows[0][DBFields.DBFIELD_TECHID].ToString();
                ddlTechCat.Items.FindByValue(dsTC.Tables[0].Rows[0][DBFields.DBFIELD_TECHID].ToString()).Attributes.Add("style", "background-color:lightGreen;");
            }


            bool bIsParent = (dr["IsParent"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsParent"]));
            Session["IsParent"] = (dr["IsParent"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsParent"])).ToString();
            bool bIsChild = (dr["ParentAssetID"] != DBNull.Value);

            if (dr["ParentAssetID"] != null)
            {
                if (dr["ParentAssetID"].ToString().CompareTo("0") == 0)
                {
                    //non blade asset
                }
                else
                {
                    if (int.Parse(dr["ParentAssetID"].ToString()) > 0)
                    {
                        hdnParentAssetID.Value = dr["ParentAssetID"].ToString();
                        hdnParentAssetName.Value = dr["Parent"].ToString();
                        hdnSiteID.Value = dr["PrimarySiteID"].ToString();
                        txtParentAsset.Text = dr["Parent"].ToString();
                    }
                }
            }

            //if (!string.IsNullOrEmpty(txtParentAsset.Text))
            //{
            //    imgRemoveParent.Visible = true;
            //}
            //else
            //{
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "disableRemoveParentButton();", true);
            //}

            wdcCreatedDate.Value = Convert.ToDateTime(dr["AssetCreatedDate"].ToString());
            //Owner
            OwnerBAL ownerBAL = new OwnerBAL();
            ownerBAL.OwnerID = Convert.ToInt32(dr["CurrentOwnerID"].ToString());
            Session["CO"] = Convert.ToInt32(dr["CurrentOwnerID"].ToString());
            if (ownerBAL.OwnerID > 0)
            {
                DataSet dsOwner = ownerBAL.retrieve();
                hdnOwnerID.Value = dr["CurrentOwnerID"].ToString();
                hdnOwnerName.Value = dsOwner.Tables[0].Rows[0][DBFields.DBFIELD_OWNER_LNAME].ToString() + "," + dsOwner.Tables[0].Rows[0][DBFields.DBFIELD_OWNER_FNAME].ToString();
                txtOwner.Text = dsOwner.Tables[0].Rows[0][DBFields.DBFIELD_OWNER_LNAME].ToString() + "," + dsOwner.Tables[0].Rows[0][DBFields.DBFIELD_OWNER_FNAME].ToString();
            }
            else
            {
                hdnOwnerID.Value = "";
                hdnOwnerName.Value = "";
                txtOwner.Text = "";
            }
            //OS,CPU,Core,Count
            if (!string.IsNullOrEmpty(dr["OS"].ToString()))
            {
                txtOS.Text = dr["OS"].ToString();
            }
            if (!string.IsNullOrEmpty(dr["CPU"].ToString()))
            {
                txtCPU.Text = dr["CPU"].ToString();
            }
            if (!string.IsNullOrEmpty(dr["CPUCount"].ToString()))
            {
                txtCpuCount.Text = dr["CPUCount"].ToString();
            }
            if (!string.IsNullOrEmpty(dr["CPUCore"].ToString()))
            {
                txtCPUCore.Text = dr["CPUCore"].ToString();
            }
            //Start Position
            if (!string.IsNullOrEmpty(dr["StartPos"].ToString()))
            {
                int startPos;
                if (int.TryParse(dr["StartPos"].ToString(), out startPos))
                {
                    this.StartPosition = startPos;
                }
            }
            else
                this.StartPosition = 0;
            //update model realted info
            UpdateModelRelatedInfo();
            //Orientation 
            if (!string.IsNullOrEmpty(dr["Orientation"].ToString()))
            {
                this.Orientation = dr["Orientation"].ToString();
                CompareItem(ddlOrientation, dr["Orientation"].ToString());
            }
            //trigger ddlOrientation_selection changed event
            OrientationChanged();


        }


        ibCreateAsset.Visible = true;
        ibCreateAsset.Enabled = true;

        ibCreateAsset2.Visible = true;
        ibCreateAsset2.Enabled = true;

        ibResetDocument.Visible = true;
        ibResetDocument.Enabled = true;

        ibReset2.Visible = true;
        ibReset2.Enabled = true;

    }

    /// <summary>
    /// Adds Hostnames to List box from a comma separated string
    /// <created by> kjb</created>
    /// <Created on >06 Sep 2011</Created>
    /// </summary>
    private void AddHostsToList()
    {
        if (!string.IsNullOrEmpty(hdnHostNames.Value))
        {
            lstHosts.Items.Clear();
            string[] items = hdnHostNames.Value.Trim(',').Split(',');
            ListItem[] lItems = new ListItem[items.Length];
            int idx = 0;
            foreach (string item in items)
            {
                lItems[idx++] = new ListItem(item, item);
            }
            lstHosts.Items.AddRange(lItems);
        }
    }

    private void ResetForm()
    {
        this.IsAssociationTabInitialized = false;
        this.IsAuthorizationTabInitialized = false;

        ManageUsersBAL userBAL = new ManageUsersBAL();
        userBAL.UserID = Convert.ToInt32(Session["UserID"]);

        userBAL.retrieveUserObject();

        this.wdcCreatedDate.Value = DateTime.Today;
        this.txtSerialNo.Text = "";
        this.txtHostName.Text = "";
        hdnHostNames.Value = "";
        lstHosts.Items.Clear();
        this.txtOwner.Text = "";
        hdnOwnerID.Value = "";
        hdnOwnerName.Value = "";
        txtAssetNameA.Text = "";
        this.ddlBusinessUnit.SelectedValue = Convert.ToString(userBAL.BusinessUnitID);
        populateSites(userBAL.BusinessUnitID);
        this.ddlTechCat.SelectedIndex = 0;

        txtLocation.Text = "";
        hdnLocID.Value = "";
        hdnLocationName.Value = "";
        hdnLocName.Value = "";
        hdnLocationID.Value = "";

        //Manufacturer
        ddlMfg.SelectedIndex = 0;
        // model
        txtModel.Text = "";
        hdnModelID.Value = "";
        hdnModelName.Value = "";

        txtCpuCount.Text = "";
        txtCPUCore.Text = "";
        txtOS.Text = "";
        txtCPU.Text = "";
        txtParentAsset.Text = "";
        hdnParentAssetName.Value = "";
        hdnParentAssetID.Value = "";
        txtBarcode.Text = "";

        txtDerated.Text = "";
        txtMaxPower.Text = "";

        ddlOrientation.SelectedIndex = 0;
        ddlPosition.SelectedIndex = 0;

    }


    #endregion Database Retrieval and Updates

    protected void ddlBusinessUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Business Unit ID for TreeList call
        if (ddlBusinessUnit != null && ddlBusinessUnit.Items.Count > 0)
            ddlBusinessUnitVal = ddlBusinessUnit.SelectedItem.Value;

        int iBusinessUnitID = Int32.Parse(ddlBusinessUnit.SelectedValue);
        populateSites(iBusinessUnitID);

    }

    protected void ibCreateAsset_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {

        //check Mount type selected, if mount type = 'rack mount' than check whether start position and no of rus specified
        // if not return and ask user to specify start position and no of rus.
        //if mount type is 'standalone' than these values are optional
        //validate whether space available in selected location.
        //validation will be performed only for racks

        int locationID = 0;
        if (!string.IsNullOrEmpty(hdnLocID.Value))
        {
            locationID = int.Parse(hdnLocID.Value.ToString());
            LocationBAL locBAL = new LocationBAL();
            locBAL.LocationID = locationID;
            DataSet dsLoc = locBAL.retrieve();
            if (dsLoc.Tables.Count > 0 && dsLoc.Tables[3].Rows.Count > 0)
            {
                string locType = dsLoc.Tables[3].Rows[0][DBFields.DBFIELD_LOCATIONTYPE].ToString();
                if (locType.ToLower().CompareTo("rack") == 0)
                {
                    if (this.MountType.ToLower().CompareTo("rackmount") == 0 || this.MountType.ToLower().CompareTo("stack in rack") == 0)
                    {

                        //IsBlade
                        if (this.IsBlade)
                        {
                            //Asset to be creatd is a blade
                        }
                        else
                        {
                            //rack
                            locBAL.LocationID = locationID;
                            DataSet ds = locBAL.retrieveRackPositions();
                            string pos_string;
                            string orientation = ddlOrientation.SelectedItem.Text;
                            if (orientation.ToLower().CompareTo("front") == 0)
                            {
                                pos_string = ds.Tables[0].Rows[0][DBFields.DBFIELD_FRONT_POSITIONS].ToString();

                            }
                            else if (orientation.ToLower().CompareTo("rear") == 0)
                            {
                                pos_string = ds.Tables[0].Rows[0][DBFields.DBFIELD_REAR_POSITIONS].ToString();
                            }

                            AssetModelBAL modelBAL = new AssetModelBAL();
                            modelBAL.ModelID = Convert.ToInt32(hdnModelID.Value.ToString());
                            DataSet dsModel = modelBAL.retrieve();
                            if (dsModel != null && dsModel.Tables.Count > 0 && dsModel.Tables[0].Rows.Count > 0)
                            {
                                float modelWidth = float.Parse(dsModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_WIDTH].ToString());
                                float modelDepth = float.Parse(dsModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_DEPTH].ToString());
                                //release u-position occupied by this asset 
                                //if for some reason edit asset fails than reverse U-position back to  is failed 

                                // if (locBAL.ValidateRackPosition(locationID, 0, Convert.ToInt32(ddlPosition.SelectedItem.Text), this.NoOfRUs,
                                //Convert.ToInt32(ddlOrientation.SelectedItem.Value), modelWidth, modelDepth))
                                // {
                                //     //Asset can be placed in selected position

                                // }
                                // else
                                // {
                                //     //Asset can't be placed in slected position
                                //     lblErrorMsg.Visible = true;
                                //     lblErrorMsg.Text = "Asset can't be placed in selected position, choose another.";
                                //     return;

                                // }
                            }
                            else
                            {
                                //model info not found
                                lblErrorMsg.Visible = true;
                                lblErrorMsg.Text = "Invalid Model. Select again";
                                return;
                            }


                        }

                    }
                    else
                    {
                        //standalone, vertical mount are optional and do nothing.

                    }
                }
                else
                {
                    //Room or Row 
                    //check whether room is a Dispose or Decom room
                    //if (locBAL.IsPartOfDisposeDecomRooms(locationID))
                    //{
                    //    lblErrorMsg.Visible = true;
                    //    lblErrorMsg.Text = "Active Assets can not be created in Decom or Dispose Rooms";
                    //    return;
                    //}
                }
            }
            else
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "Invalid location. Select location";
                return;
            }
        }
        else
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "Invalid location. Select location";
            return;
        }

        Create();
        //hdnParentAssetID.Value = "";

    }

    protected void ibDeleteHost_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {

    }

    protected void ibAddHost_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {

    }

    protected void ibResetDocument_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.IsAssociationTabInitialized = false;
        this.IsAuthorizationTabInitialized = false;
        //if (hdnDocId.Value == "")
        if (this.AssetID == 0)
        {
            ResetForm();
        }
        else
        {
            ResetForm();
            FillAssetDetails(this.AssetID);
            lblErrorMsg.Visible = false;

        }

    }


    protected void ibClose_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.close()", true);
    }


    protected void txtSerialNo_TextChanged(object sender, EventArgs e)
    {
        lblErrorMsg.Visible = false;
        lblErrorMsg.Text = "";
    }
    protected void ddlMfg_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMfg != null && ddlMfg.Items.Count > 0)
        {
            ddlMfgName = ddlMfg.SelectedItem.Text;
            ddlMfgID = ddlMfg.SelectedItem.Value;
        }

        hdnModelID.Value = "";
        hdnModelName.Value = "";
        txtModel.Text = "";
    }



    private void getChildModels(DataTreeNode node, int ModelID)
    {
        AssetModelBAL objChildAM = new AssetModelBAL();
        DataSet dsChildAM = objChildAM.retrieveByParentModelID(ModelID);
        DataTable dtChildAM = dsChildAM.Tables[0];
        using (DataTableReader dtModelByModelRdr = dtChildAM.CreateDataReader())
        {
            while (dtModelByModelRdr.Read())
            {
                DataTreeNode ModelNode = new DataTreeNode();
                ModelNode.Text = dtModelByModelRdr.GetValue(1).ToString();
                ModelNode.Value = dtModelByModelRdr.GetValue(0).ToString();
                node.Nodes.Add(ModelNode);
                getChildModels(ModelNode, Convert.ToInt32(dtModelByModelRdr.GetValue(0).ToString()));
            }
        }

    }

    protected void PopulatetechList()
    {
        TechCatBAL objTechCal = new TechCatBAL();
        DataSet dsTechCal = objTechCal.retrieve();
        DataTable dtTechCal = dsTechCal.Tables[0];

        DataRow dr = dtTechCal.NewRow();
        dr[0] = 0;
        dr[1] = "-Select-";
        dtTechCal.Rows.InsertAt(dr, 0);

        ddlTechCat.DataSource = dtTechCal;
        ddlTechCat.DataValueField = dtTechCal.Columns[0].ToString();
        ddlTechCat.DataTextField = dtTechCal.Columns[1].ToString();
        ddlTechCat.DataBind();
        //ddlTechCat.SelectedValue = "Network";
        //ddlTechCat.Items[0].Attributes.Add("style", "color:gray;"); 

    }
    protected void ddlTechCat_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlSite_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSite != null && ddlSite.Items.Count > 0)
        {
            ddlSiteVal = ddlSite.SelectedItem.Value;
            hdnSiteID.Value = this.ddlSite.SelectedValue;
        }
        hdnLocID.Value = "";
        hdnLocName.Value = "";
        txtLocation.Text = "";
    }
    protected void txtLocation_TextChanged(object sender, EventArgs e)
    {

    }


    private bool CheckUPosition(int StartPos, int NoOfRUs, BitArray RackPositions)
    {
        bool positionExists = true;

        for (int i = StartPos - 1; i < (StartPos + NoOfRUs - 1); i++)
        {
            if (RackPositions.Get(i))
            {
                positionExists = false;
                break;
            }
        }
        return positionExists;
    }

    private void Reverse(BitArray array)
    {
        int length = array.Length;
        int mid = (length / 2);
        for (int i = 0; i < mid; i++)
        {
            bool bit = array[i];
            array[i] = array[length - i - 1];
            array[length - i - 1] = bit;
        }
    }

    protected void btnReload_Click(object sender, EventArgs e)
    {
        UpdateModelRelatedInfo();

        if (!string.IsNullOrEmpty(hdnLocID.Value))
        {
            LocationBAL locBAL = new LocationBAL();
            locBAL.LocationID = Convert.ToInt32(hdnLocID.Value.ToString());
            DataSet ds = locBAL.retrieve();
            if (ds != null && ds.Tables.Count > 3 && ds.Tables[3].Rows.Count > 0)
            {
                this.LocationType = ds.Tables[3].Rows[0][DBFields.DBFIELD_LOCATIONTYPE].ToString();
            }
        }

    }

    private void UpdateModelRelatedInfo()
    {
        if (!string.IsNullOrEmpty(hdnModelID.Value.ToString()))
        {
            AssetModelBAL modelBAL = new AssetModelBAL();
            modelBAL.ModelID = Convert.ToInt32(hdnModelID.Value.ToString());
            DataSet ds = modelBAL.retrieve();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string maxPower = ds.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_MAX_POWER].ToString();
                string deratedPower = ds.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_SS_POWER].ToString();
                if (!string.IsNullOrEmpty(maxPower.Trim()))
                    txtMaxPower.Text = Math.Round(Convert.ToDouble(maxPower), 3).ToString();
                else
                    txtMaxPower.Text = "";

                if (!string.IsNullOrEmpty(deratedPower.Trim()))
                    txtDerated.Text = Math.Round(Convert.ToDouble(deratedPower), 3).ToString();
                else
                    txtDerated.Text = "";

                //Model type
                this.ModelType = ds.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_TYPE].ToString();
                this.ModelTypeID = Convert.ToInt32(ds.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_TYPE_ID].ToString());
                //Mount type
                this.MountType = ds.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_MOUNT_TYPE].ToString();
                //no of rus
                this.NoOfRUs = Convert.ToInt32(ds.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_UHEIGHT].ToString());
                //IsBlade
                this.IsBlade = Convert.ToBoolean(ds.Tables[0].Rows[0][DBFields.DBFIELD_IS_BLADE].ToString());

                if (this.IsBlade)
                {
                    if (!string.IsNullOrEmpty(hdnParentAssetID.Value.ToString()))
                    {
                        AssetBAL parentBAL = new AssetBAL();
                        parentBAL.AssetID = Convert.ToInt32(hdnParentAssetID.Value.ToString());
                        DataSet dsParent = parentBAL.Retrieve();
                        int parentmodelID = Convert.ToInt32(dsParent.Tables[0].Rows[0][DBFields.DBFIELD_MODELID].ToString());

                        AssetModelBAL parentModelBAL = new AssetModelBAL();
                        parentModelBAL.ModelID = parentmodelID;
                        DataSet dsParentModel = parentModelBAL.retrieve();
                        if (dsParentModel != null && dsParentModel.Tables.Count > 0 && dsParentModel.Tables[0].Rows.Count > 0)
                        {
                            this.EnclFrontRowCount = Convert.ToInt16(dsParentModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_ENCL_FRONT_ROW_COUNT].ToString());
                            this.EnclFrontColCount = Convert.ToInt16(dsParentModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_ENCL_FRONT_COL_COUNT].ToString());
                            this.EnclRearRowCount = Convert.ToInt16(dsParentModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_ENCL_REAR_ROW_COUNT].ToString());
                            this.EnclRearColCount = Convert.ToInt16(dsParentModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_ENCL_REAR_COL_COUNT].ToString());
                        }

                    }
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        this.BladeRowCount = Convert.ToInt16(ds.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_BLADE_ROW_COUNT].ToString());
                        this.BladeColCount = Convert.ToInt16(ds.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_BLADE_COL_COUNT].ToString());
                    }

                    LoadEnclPositions();
                }
                else
                {
                    LoadStartPositions(false, "", 0, "");
                    ddlOrientation.SelectedIndex = 0;
                    txtParentAsset.Text = "";
                    hdnParentAssetID.Value = "";
                    hdnParentAssetName.Value = "";
                }

            }
        }
    }

    private void LoadEnclPositions()
    {
        //if asset is blade and parent selected (in case of rack, parent selection is compulsory)
        if (!string.IsNullOrEmpty(hdnParentAssetID.Value.ToString().Trim()))
        {
            AssetBAL assetBAL = new AssetBAL();
            assetBAL.AssetID = Convert.ToInt32(hdnParentAssetID.Value.ToString());
            DataSet dsAsset = assetBAL.retrieveEnclPositions();

            //get Asset orientation
            AssetBAL bladeAsset = new AssetBAL();
            bladeAsset.AssetID = this.AssetID;
            DataSet dsBlade = bladeAsset.Retrieve();
            string bladeOrientation = string.Empty;

            if (dsBlade != null && dsBlade.Tables.Count > 0 && dsBlade.Tables[0].Rows.Count > 0)
            {
                bladeOrientation = dsBlade.Tables[0].Rows[0]["Orientation"].ToString();
            }

            if (dsAsset != null && dsAsset.Tables.Count > 0 && dsAsset.Tables[0].Rows.Count > 0)
            {
                switch (ddlOrientation.SelectedItem.Text)
                {
                    case "Front":
                        LoadStartPositions(true, dsAsset.Tables[0].Rows[0][DBFields.DBFIELD_FRONT_POSITIONS].ToString(), this.StartPosition, bladeOrientation);
                        break;
                    case "Rear":
                        LoadStartPositions(true, dsAsset.Tables[0].Rows[0][DBFields.DBFIELD_REAR_POSITIONS].ToString(), this.StartPosition, bladeOrientation);
                        break;
                    default:
                        LoadStartPositions(false, "", 0, "");
                        break;

                }
            }
        }
    }

    private void LoadStartPositions(bool ApplyFilter, string Positions, int StartPosition, string AssetOrientation)
    {
        ddlPosition.Items.Clear();
        ArrayList arrExistisng = new ArrayList();
        if (this.IsBlade)
        {
            for (int br = 0; br < this.BladeRowCount; br++)
            {
                for (int bc = 0; bc < this.BladeColCount; bc++)
                {
                    if (ddlOrientation.SelectedItem.Text == "Front")
                    {
                        if (ddlOrientation.SelectedItem.Text.ToLower().CompareTo(AssetOrientation.ToLower()) == 0)
                            arrExistisng.Add((this.StartPosition + (this.EnclFrontColCount * br) + bc).ToString());
                    }
                    else if (ddlOrientation.SelectedItem.Text == "Rear")
                    {
                        if (ddlOrientation.SelectedItem.Text.ToLower().CompareTo(AssetOrientation.ToLower()) == 0)
                            arrExistisng.Add((this.StartPosition + (this.EnclRearColCount * br) + bc).ToString());
                    }
                }
            }

        }
        else
        {
            for (int e = StartPosition; e < (StartPosition + this.NoOfRUs); e++)
            {
                if (ddlOrientation.SelectedItem.Text.ToLower().CompareTo(AssetOrientation.ToLower()) == 0)
                    arrExistisng.Add(e.ToString());
            }
        }

        if (ApplyFilter)
        {
            ddlPosition.Items.Add("-Select-");
            for (int i = 1; i <= Positions.Length; i++)
            {
                if (arrExistisng.Contains(i.ToString()))
                {
                    ddlPosition.Items.Add(i.ToString());
                }
                else if (Positions[i - 1] == 'P' || Positions[i - 1] == 'V')
                {
                    ddlPosition.Items.Add(i.ToString());
                }
            }
            if (this.StartPosition > 0 && ddlOrientation.SelectedItem.Text.ToLower().CompareTo(this.Orientation.ToLower()) == 0)
                CompareItem(ddlPosition, this.StartPosition.ToString());
        }
        else
        {
            ddlPosition.Items.Add("-Select-");
        }
    }

    private void populateOrientationList()
    {
        if (ddlOrientation.Items.Count > 0)
            ddlOrientation.Items.Clear();
        OrientationBAL objOrientation = new iAssetTrack.BAL.OrientationBAL();
        DataSet dsOrn = objOrientation.retrieve();
        DataTable dtOrn = dsOrn.Tables[0];

        DataRow dr = dtOrn.NewRow();
        dr[0] = 0;
        dr[1] = "-Select-";
        dtOrn.Rows.InsertAt(dr, 0);

        ddlOrientation.DataSource = dtOrn;
        ddlOrientation.DataValueField = dtOrn.Columns[0].ToString();
        ddlOrientation.DataTextField = dtOrn.Columns[1].ToString();
        ddlOrientation.DataBind();
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

    protected void ddlOrientation_SelectedIndexChanged(object sender, EventArgs e)
    {
        OrientationChanged();
    }

    private void OrientationChanged()
    {
        LocationBAL locBAL = new LocationBAL();
        if (!string.IsNullOrEmpty(hdnLocID.Value))
        {
            locBAL.LocationID = Convert.ToInt32(hdnLocID.Value.ToString());
            DataSet ds = locBAL.retrieveRackPositions();

            //get Asset orientation
            AssetBAL rackAsset = new AssetBAL();
            rackAsset.AssetID = this.AssetID;
            DataSet dsAsset = rackAsset.Retrieve();
            string assetOrientation = string.Empty;

            if (dsAsset != null && dsAsset.Tables.Count > 0 && dsAsset.Tables[0].Rows.Count > 0)
            {
                assetOrientation = dsAsset.Tables[0].Rows[0]["Orientation"].ToString();
            }

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                switch (ddlOrientation.SelectedItem.Text)
                {
                    case "Front":
                        LoadStartPositions(true, ds.Tables[0].Rows[0][DBFields.DBFIELD_FRONT_POSITIONS].ToString(), this.StartPosition, assetOrientation);
                        break;
                    case "Rear":
                        LoadStartPositions(true, ds.Tables[0].Rows[0][DBFields.DBFIELD_REAR_POSITIONS].ToString(), this.StartPosition, assetOrientation);
                        break;
                    default:
                        LoadStartPositions(false, "", 0, "");
                        break;

                }
            }
        }

        if (this.IsBlade && !string.IsNullOrEmpty(hdnParentAssetID.Value.ToString().Trim()))
        {
            LoadEnclPositions();
        }

        if (ddlPosition.Items.Count == 0)
            LoadStartPositions(false, "", 0, "");
    }

    protected void btnParentLoad_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(hdnModelID.Value.ToString()))
        {
            AssetModelBAL modelBAL = new AssetModelBAL();
            modelBAL.ModelID = Convert.ToInt32(hdnModelID.Value.ToString());
            DataSet ds = modelBAL.retrieve();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string maxPower = ds.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_MAX_POWER].ToString();
                string deratedPower = ds.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_SS_POWER].ToString();
                if (!string.IsNullOrEmpty(maxPower.Trim()))
                    txtMaxPower.Text = Math.Round(Convert.ToDouble(maxPower), 3).ToString();
                else
                    txtMaxPower.Text = "";

                if (!string.IsNullOrEmpty(deratedPower.Trim()))
                    txtDerated.Text = Math.Round(Convert.ToDouble(deratedPower), 3).ToString();
                else
                    txtDerated.Text = "";

                //Model type
                this.ModelType = ds.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_TYPE].ToString();
                this.ModelTypeID = Convert.ToInt32(ds.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_TYPE_ID].ToString());
                //Mount type
                this.MountType = ds.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_MOUNT_TYPE].ToString();
                //no of rus
                this.NoOfRUs = Convert.ToInt32(ds.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_UHEIGHT].ToString());
                //IsBlade
                this.IsBlade = Convert.ToBoolean(ds.Tables[0].Rows[0][DBFields.DBFIELD_IS_BLADE].ToString());

                if (this.IsBlade)
                {
                    if (!string.IsNullOrEmpty(hdnParentAssetID.Value.ToString()))
                    {
                        AssetBAL parentBAL = new AssetBAL();
                        parentBAL.AssetID = Convert.ToInt32(hdnParentAssetID.Value.ToString());
                        DataSet dsParent = parentBAL.Retrieve();
                        int parentmodelID = Convert.ToInt32(dsParent.Tables[0].Rows[0][DBFields.DBFIELD_MODELID].ToString());

                        AssetModelBAL parentModelBAL = new AssetModelBAL();
                        parentModelBAL.ModelID = parentmodelID;
                        DataSet dsParentModel = parentModelBAL.retrieve();
                        if (dsParentModel != null && dsParentModel.Tables.Count > 0 && dsParentModel.Tables[0].Rows.Count > 0)
                        {
                            this.EnclFrontRowCount = Convert.ToInt16(dsParentModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_ENCL_FRONT_ROW_COUNT].ToString());
                            this.EnclFrontColCount = Convert.ToInt16(dsParentModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_ENCL_FRONT_COL_COUNT].ToString());
                            this.EnclRearRowCount = Convert.ToInt16(dsParentModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_ENCL_REAR_ROW_COUNT].ToString());
                            this.EnclRearColCount = Convert.ToInt16(dsParentModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_ENCL_REAR_COL_COUNT].ToString());
                        }

                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            this.BladeRowCount = Convert.ToInt16(ds.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_BLADE_ROW_COUNT].ToString());
                            this.BladeColCount = Convert.ToInt16(ds.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_BLADE_COL_COUNT].ToString());
                        }

                    }
                    LoadEnclPositions();
                }
            }

            //Site
            if (!string.IsNullOrEmpty(hdnSiteID.Value.ToString().Trim()))
            {
                if (ddlSite.Items.Count > 1)
                {
                    string itemOrigin = hdnSiteID.Value.ToString();
                    string itemToCompare = string.Empty;
                    foreach (ListItem item in ddlSite.Items)
                    {
                        itemToCompare = item.Value.ToLower();
                        if (itemOrigin.ToLower().CompareTo(itemToCompare) == 0)
                        {
                            ddlSite.ClearSelection();
                            item.Selected = true;
                        }
                    }

                }
            }

            if (!string.IsNullOrEmpty(hdnLocID.Value))
            {
                LocationBAL locBAL = new LocationBAL();
                locBAL.LocationID = Convert.ToInt32(hdnLocID.Value.ToString());
                DataSet dsLoc = locBAL.retrieve();
                if (dsLoc != null && dsLoc.Tables.Count > 0 && dsLoc.Tables[0].Rows.Count > 0)
                {
                    this.LocationType = dsLoc.Tables[0].Rows[0][DBFields.DBFIELD_LOCATIONTYPE].ToString();
                }
            }
        }
    }

}
