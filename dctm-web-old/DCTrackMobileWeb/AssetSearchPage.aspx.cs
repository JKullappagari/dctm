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
using System.ComponentModel;
using System.Drawing.Design;
using iAssetTrack.DALC;
using System.Drawing;
using iAssetTrack.BAL;
using iAssetTrackBAL;
//using Infragistics.Web.UI;
//using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.NavigationControls;
using Infragistics.Web.UI.GridControls;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Infragistics.WebUI.WebDataInput;
using System.IO;
using System.Collections.Generic;

public partial class AssetSearchPage : System.Web.UI.Page
{

    #region "Declarations"
    private int intBusinessUnitID;
    private int intPrimarySite;
    private int intLocation;
    private int intAssetType;
    private int intOwnerID;
    private int intIssuedTo;
    private int intReceivedBy;
    private string strOwner;
    private int intModelID;
    private char chrRFIDStatus;
    private int intAssetStatus;
    private int intMfgID;
    AssociatedStatusValues intAssociatedStatus;

    protected string ddlPrimarySiteVal;
    protected string ddlMfgName = string.Empty;
    protected string ddlMfgID = string.Empty;
    protected string ddlBusinessUnitVal = string.Empty;
    private bool enableModifyAll;

    private bool btnSearchClicked;
    private CommonBAL objCommon;
    private iAssetTrack.BAL.AssetCreationBAL objAsset;
    private iAssetTrack.BAL.ConfigBAL objAssetStatus;

    private const string JAVA_SCRIPT = "javascript:";

    private const string PROP_CLIENTSIDEONSELECTION = "ClientSideOnSelection";
    private const string PROP_SEARCHPURPOSE = "SearchPurpose";
    private const string PROP_BUSINESSUNITID = "BusinessUnitID";
    private const string PROP_DEPARTMENTID = "DepartmentID";
    private const string PROP_SITEID = "SiteID";
    private const string PROP_ASSETNAME = "AssetName";
    private const string PROP_LOCATIONID = "LocationID";
    private const string PROP_USERID = "UserID";
    private const string PROP_USERNAME = "UserName";
    private const string PROP_RFIDCARDSTATUS = "RFIDCardStatus";
    private const string PROP_ASSETSTATUSID = "AssetStatusID";
    private const string PROP_CURRENTGRIDPAGENUMBER = "AssetSearchPageNum";
    private const string PROP_ISSUEDTO = "IssuedTo";
    private const string PROP_ALLOWMULTISELECT = "MultiSelect";
    private const string PROP_ALLOWEXPORTTOEXCEL = "AllowExport";
    private const string PROP_ALLOWUSERSELECTION = "AllowUserSelection";
    private const string PROP_ASSOCIATEDASSETS = "AssociatedAssets";
    private const string PROP_PARENTASSETS = "ParentAssets";
    private const string PROP_PAGECOUNT = "PageCount";
    private const string PROP_AUTODISPLAYFIRSTPAGE = "AutoDisplayFirstPage";
    private const string PROP_PARENTASSETID = "ParentAssetID";
    private const string PROP_ASSETID = "AssetID";

    private const string PROP_SELECTEDLIST = "SelectedList";
    private const string PROP_ORIGSELECTEDLIST = "OriginalSelectedList";
    private const string PROP_SEARCHBARSTATE = "SearchBarState";


    private const string PROP_ALLOWBUSELECTION = "AllowBUSelection";


    private const string JS_PARAM_ASSETID = "<AssetID>";
    private const string JS_PARAM_REFNO = "<RefNumber>";
    private const string JS_PARAM_ASSETNAME = "<AssetName>";
    private const string JS_PARAM_PARENTID = "<ParentAssetID>";
    // private const string JS_PARAM_ASSETID = "<AssetID>";
    private const string JS_PARAM_LEVEL = "<Level>";


    private const string GRID_COL_KEY_ASSETID = "AssetID";
    private const string GRID_COL_KEY_REFNO = "RefNumber";
    private const string GRID_COL_KEY_ASSETNAME = "AssetName";
    private const string GRID_COL_KEY_SELECT = "SelectCheckBox";
    private const string GRID_ROW_CHECKBOX = "ChkSelect";
    private const string GRID_COL_KEY_ISPARENT = "IsParent";
    private const string GRID_COL_KEY_PARENTASSETID = "ParentAssetID";
    private const string GRID_COL_KEY_ISPARENTICON = "IsParentIcon";
    private const string GRID_COL_KEY_RFIDTAGID = "RFIDTagID";
    private const string GRID_COL_KEY_RFIDTAGICON = "RFIDTagIcon";
    private const string GRID_COL_KEY_STARTPOS = "StartPos";
    private const string GRID_COL_KEY_NOOFRUS = "NoOfRUs";

    private const int GRID_BAND_LEVEL1 = 0;
    private const int GRID_BAND_LEVEL2 = 1;

    private const string ISPARENT_IMAGE = "infragistics/images/Outlook2003/folders.gif";
    private const string ISCHILD_IMAGE = "infragistics/images/Outlook2003/folder.gif";
    private const string RFID_IMAGE = "images/label_16x16.gif";

    private const string VIEWSTATE_PAGE_PFX = "PAGE_";
    //private const string VIEWSTATE_INIT_SELECTIONS = "INIT_SELECTIONS";
    public string strAssetId = "";
    #endregion

    #region Enumerations
    public enum AssetSearchPurposeValues
    {
        Combine,
        DepositToRegistry,
        IssueFromRegistry,
        ReceiptConfirmation,
        Transfer,
        Search,
        Delete
    };


    public enum AssociatedStatusValues
    {
        All = 0,
        ShowAssociatedOnly = -2,
        ShowNonAssociatedOnly = -1,
        ShowSubAssetsOnly = -3
    };


    public enum ParentStatusValues
    {
        All = -1,
        ShowParentsOnly = 1,
        ShowNonParentsOnly = 0,
    }


    public enum RFIDCardStatusValues
    {
        All,
        DeAssigned,
        Assigned,
        NotAssigned
    };


    public enum AssetStatusValues
    {
        All = 0,
        Active = 5,
        Barred = 2,
        Created = 1,
        Mustered = 4,
        Restricted = 3,
        Deleted = 6
    };


    public enum SearchBarStateValues
    {
        Expanded = 0,
        Collapsed = 1

    };
    #endregion Enumerations

    // J00007 by kjb on 25 Sep 2012
    # region new control declaration
    protected Label lblMsg;
    protected WebImageButton ibCreate;
    protected DropDownList ddlBusinessUnit, ddlPrimarySite, ddlSite, ddlMfg, ddlAssetType, ddlAssetStatus, ddlRFIDCardStatus;
    protected TextBox txtModel, txtOwner, txtParentLocation, txtRefNumber, txtRFIDTag, txtAssetName, txtHostName;
    # endregion

    #region Web Control SubControls


    [Browsable(true), Category("SearchOptionProperties"), Description("Enable or Disable Search Option Controls"), Bindable(true)]
    public bool AllowBusinessUnitSelection
    {
        get { return (bool)ViewState["AllowBusinessUnitSelection"]; }
        set { ViewState["AllowBusinessUnitSelection"] = value; }
    }

    public DropDownList BusinessUnitDropDown
    {
        get { return this.ddlBusinessUnit; }
    }

    public DropDownList SiteDropDown
    {
        get { return this.ddlPrimarySite; }
    }


    public DropDownList AssetTypeDropDown
    {
        get { return this.ddlAssetType; }
    }

    public DropDownList DocumentStatusDropDown
    {
        get { return this.ddlAssetStatus; }
    }

    public DropDownList RFIDCardStatusDropDown
    {
        get { return this.ddlRFIDCardStatus; }
    }

    #endregion

    #region Web Control Properties
    public int PageNum
    {
        get
        {
            return (ViewState[PROP_CURRENTGRIDPAGENUMBER] != null ? Convert.ToInt32(ViewState[PROP_CURRENTGRIDPAGENUMBER]) : 1);
        }
        set
        {
            ViewState[PROP_CURRENTGRIDPAGENUMBER] = value;
            //if (value > this.PageCount) this.PageCount = value;
        }
    }
    private int PageCount
    {
        get { return (ViewState[PROP_PAGECOUNT] != null ? Convert.ToInt32(ViewState[PROP_PAGECOUNT]) : 0); }
        set { ViewState[PROP_PAGECOUNT] = value; }

    }
    public AssetSearchPurposeValues SearchPurpose
    {
        get
        {
            return (ViewState[PROP_SEARCHPURPOSE] != null ? (AssetSearchPurposeValues)ViewState[PROP_SEARCHPURPOSE] : AssetSearchPurposeValues.Search);
        }
        set { ViewState[PROP_SEARCHPURPOSE] = value; }
    }
    public AssociatedStatusValues AssociatedStatus
    {
        get
        {
            return (ViewState[PROP_ASSOCIATEDASSETS] != null ? (AssociatedStatusValues)ViewState[PROP_ASSOCIATEDASSETS] : AssociatedStatusValues.All);
        }
        set { ViewState[PROP_ASSOCIATEDASSETS] = value; }
    }
    public ParentStatusValues ParentStatus
    {
        get
        {
            return (ViewState[PROP_PARENTASSETS] != null ? (ParentStatusValues)ViewState[PROP_PARENTASSETS] : ParentStatusValues.All);
        }
        set { ViewState[PROP_PARENTASSETS] = value; }
    }
    //Do not set this value in the html control. Always set it in code, since it references the cbShowSubAssetsOnly control
    public int ParentAssetID
    {
        get { return (ViewState[PROP_PARENTASSETID] != null ? Convert.ToInt32(ViewState[PROP_PARENTASSETID]) : 0); }
        set
        {
            ViewState[PROP_PARENTASSETID] = value;
            //if (value > 0) this.cbShowSubAssetsOnly.Visible = true;
        }
    }
    public int AssetID
    {
        get { return (ViewState[PROP_ASSETID] != null ? Convert.ToInt32(ViewState[PROP_ASSETID]) : 0); }
        set
        {
            ViewState[PROP_ASSETID] = value;
            //if (value > 0) this.cbShowSubAssetsOnly.Visible = true;
        }
    }
    public int BusinessUnitID
    {
        get
        {
            return (ViewState[PROP_BUSINESSUNITID] != null ? Convert.ToInt32(ViewState[PROP_BUSINESSUNITID]) : 0);
        }
        set { ViewState[PROP_BUSINESSUNITID] = value; }
    }
    public int DepartmentID
    {
        get
        {
            return (ViewState[PROP_DEPARTMENTID] != null ? Convert.ToInt32(ViewState[PROP_DEPARTMENTID]) : 0);
        }
        set { ViewState[PROP_DEPARTMENTID] = value; }
    }
    public int SiteID
    {
        get { return (ViewState[PROP_SITEID] != null ? Convert.ToInt32(ViewState[PROP_SITEID]) : 0); }
        set { ViewState[PROP_SITEID] = value; }
    }
    public int LocationID
    {
        get { return (ViewState[PROP_LOCATIONID] != null ? Convert.ToInt32(ViewState[PROP_LOCATIONID]) : 0); }
        set { ViewState[PROP_LOCATIONID] = value; }
    }
    public int StatusID
    {
        get { return (ViewState[PROP_ASSETSTATUSID] != null ? Convert.ToInt32(ViewState[PROP_ASSETSTATUSID]) : 0); }
        set { ViewState[PROP_ASSETSTATUSID] = value; }

    }
    //Do not set this value in the html control. Always set it in code, since it references the hdnOwnerID control
    public int UserID
    {
        get { return (ViewState[PROP_USERID] != null ? Convert.ToInt32(ViewState[PROP_USERID]) : 0); }
        set { ViewState[PROP_USERID] = value; this.hdnOwnerID.Value = ViewState[PROP_USERID].ToString(); }
    }
    public string UserName
    {
        get { return (ViewState[PROP_USERNAME] != null ? Convert.ToString(ViewState[PROP_USERNAME]) : ""); }
        set { ViewState[PROP_USERNAME] = value; this.hdnOwnerName.Value = ViewState[PROP_USERNAME].ToString(); this.txtOwner.Text = ViewState[PROP_USERNAME].ToString(); }
    }
    public RFIDCardStatusValues RFIDCardStatus
    {
        get { return (ViewState[PROP_RFIDCARDSTATUS] != null ? (RFIDCardStatusValues)ViewState[PROP_RFIDCARDSTATUS] : RFIDCardStatusValues.All); }
        set { ViewState[PROP_RFIDCARDSTATUS] = value; }
    }
    public String RFIDStatusString
    {
        get
        {
            String retVal = "";
            switch (this.RFIDCardStatus)
            {
                case RFIDCardStatusValues.All: retVal = ""; break;
                case RFIDCardStatusValues.Assigned: retVal = "A"; break;
                case RFIDCardStatusValues.DeAssigned: retVal = "D"; break;
                case RFIDCardStatusValues.NotAssigned: retVal = "N"; break;
            }
            return retVal;
        }
    }
    public AssetStatusValues AssetStatus
    {
        get { return (ViewState[PROP_ASSETSTATUSID] != null ? (AssetStatusValues)ViewState[PROP_ASSETSTATUSID] : AssetStatusValues.All); }
        set { ViewState[PROP_ASSETSTATUSID] = value; }
    }
    public string AssetName
    {
        get { return (ViewState[PROP_ASSETNAME] != null ? Convert.ToString(ViewState[PROP_ASSETNAME]) : ""); }
        set { ViewState[PROP_ASSETNAME] = value; }
    }
    public bool AllowMultiSelect
    {
        get { return (ViewState[PROP_ALLOWMULTISELECT] != null ? Convert.ToBoolean(ViewState[PROP_ALLOWMULTISELECT]) : false); }
        set { ViewState[PROP_ALLOWMULTISELECT] = value; }
    }
    public bool AllowExportToExcel
    {
        get { return (ViewState[PROP_ALLOWEXPORTTOEXCEL] != null ? Convert.ToBoolean(ViewState[PROP_ALLOWEXPORTTOEXCEL]) : false); }
        set { ViewState[PROP_ALLOWEXPORTTOEXCEL] = value; }
    }
    public bool AllowUserSelection
    {
        get { return (ViewState[PROP_ALLOWUSERSELECTION] != null ? Convert.ToBoolean(ViewState[PROP_ALLOWUSERSELECTION]) : false); }
        set { ViewState[PROP_ALLOWUSERSELECTION] = value; }
    }
    public bool AutoDisplayFirstPage
    {
        get { return (ViewState[PROP_AUTODISPLAYFIRSTPAGE] != null ? Convert.ToBoolean(ViewState[PROP_AUTODISPLAYFIRSTPAGE]) : true); }
        set { ViewState[PROP_AUTODISPLAYFIRSTPAGE] = value; }
    }
    protected bool m_ClientSideEventEnabled = false;
    public string ClientSideOnSelection
    {
        get { return (ViewState[PROP_CLIENTSIDEONSELECTION] != null ? Convert.ToString(ViewState[PROP_CLIENTSIDEONSELECTION]) : ""); }
        set { ViewState[PROP_CLIENTSIDEONSELECTION] = value; m_ClientSideEventEnabled = true; }
    }
    public SearchBarStateValues SearchBarState
    {
        get { return (ViewState[PROP_SEARCHBARSTATE] != null ? (SearchBarStateValues)ViewState[PROP_SEARCHBARSTATE] : SearchBarStateValues.Expanded); }
        set { ViewState[PROP_SEARCHBARSTATE] = value; }
    }
    //The Current Page Selection is stored in the ViewState. One ViewState for each Page.
    //The following properties are used to store & retrieve the CurrentPageSelections
    private string CurrentPageSelectionKey
    {
        get { return VIEWSTATE_PAGE_PFX + Convert.ToString(this.PageNum).Trim(); }
    }
    private string CurrentPageSelections
    {
        get { return (string)ViewState[CurrentPageSelectionKey]; }
        set { ViewState[CurrentPageSelectionKey] = value; }
    }
    private string[] SelectedList
    {
        get { return ((string[])ViewState[PROP_SELECTEDLIST]); }
        set { ViewState[PROP_SELECTEDLIST] = value; }
    }
    private string OriginalSelectedList
    {
        get { return (ViewState[PROP_ORIGSELECTEDLIST] != null ? (string)ViewState[PROP_ORIGSELECTEDLIST] : ""); }
        set { ViewState[PROP_ORIGSELECTEDLIST] = value; }

    }
    #endregion

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        uwgAssetSearch.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(uwgAssetSearch_ItemCommand);
    }

    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {

        this.uwgAssetSearch.Behaviors.Paging.PageIndex = e.PageNumber;
        FillAsset_Details();
        FillAssetStatus();
        this.InitializeSearchCriteria();

    }


    protected void Page_Load(object sender, EventArgs e)
    {
        // J00007 by kjb on 25 Sep 2012
        # region New controls initialization
        lblMsg = (Label)this.webAssetSearch.Groups[0].Items[0].FindControl("lblMsg");
        ibCreate = (WebImageButton)this.webAssetSearch.Groups[0].Items[0].FindControl("ibCreate");
        ddlBusinessUnit = (DropDownList)this.webAssetSearch.Groups[0].Items[0].FindControl("ddlBusinessUnit");
        ddlPrimarySite = (DropDownList)this.webAssetSearch.Groups[0].Items[0].FindControl("ddlPrimarySite");
        ddlMfg = (DropDownList)this.webAssetSearch.Groups[0].Items[0].FindControl("ddlMfg");
        ddlAssetType = (DropDownList)this.webAssetSearch.Groups[0].Items[0].FindControl("ddlAssetType");
        ddlAssetStatus = (DropDownList)this.webAssetSearch.Groups[0].Items[0].FindControl("ddlAssetStatus");
        ddlRFIDCardStatus = (DropDownList)this.webAssetSearch.Groups[0].Items[0].FindControl("ddlRFIDCardStatus");

        txtRefNumber = (TextBox)this.webAssetSearch.Groups[0].Items[0].FindControl("txtRefNumber");
        txtOwner = (TextBox)this.webAssetSearch.Groups[0].Items[0].FindControl("txtOwner");
        txtRFIDTag = (TextBox)this.webAssetSearch.Groups[0].Items[0].FindControl("txtRFIDTag");

        txtModel = (TextBox)this.webAssetSearch.Groups[0].Items[0].FindControl("txtModel");
        txtParentLocation = (TextBox)this.webAssetSearch.Groups[0].Items[0].FindControl("txtParentLocation");
        txtAssetName = (TextBox)this.webAssetSearch.Groups[0].Items[0].FindControl("txtAssetName");
        txtHostName = (TextBox)this.webAssetSearch.Groups[0].Items[0].FindControl("txtHostName");///Added on 08 May 2013

        # endregion

        Session["PageHeader"] = "Asset Search";
        # region  20120427046
        //FirstPage.Visible = false;
        //LastPage.Visible = false;
        //NextPage.Visible = false;
        //btnPreviousPage.Visible = false;

        //SepLbl.Visible = false;
        //GoToPageImb.Visible = false;
        //GoToPageTxt.Visible = false;
        //CurrentPage.Visible = false;
        //TotalPages.Visible = false;

        // commented by kjb -- 20120427046

        //FirstPage.Visible = true;
        //LastPage.Visible = true;
        //NextPage.Visible = true;
        //btnPreviousPage.Visible = true;

        //SepLbl.Visible = true;
        //GoToPageImb.Visible = true;
        //GoToPageTxt.Visible = true;
        //CurrentPage.Visible = true;
        //TotalPages.Visible = true;

        //
        # endregion

        DataTable _dtRights;
        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "AssetSearchPage.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        if (_dtRights.Select("Module = 'Search Asset' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }

        if (_dtRights.Select("Module = 'Search Asset' and Rights = '" + "Modify" + "'").Length == 0)
        {
            enableModifyAll = false;
            btnModifyAll.Visible = false;
        }
        else
        {
            enableModifyAll = true;
            btnModifyAll.Visible = false;//true
        }

        ibExport.Enabled = false;
        ibExport.Visible = false;
        chkShowChild.Enabled = false;
        chkShowChild.Visible = false;

        if (!string.IsNullOrEmpty(hdnLocName.Value))
            txtParentLocation.Text = hdnLocName.Value;
        else
            txtParentLocation.Text = "";

        if (!string.IsNullOrEmpty(hdnModelName.Value))
            txtModel.Text = hdnModelName.Value;
        else
            txtModel.Text = "";

        if (!string.IsNullOrEmpty(hdnOwnerName.Value))
            txtOwner.Text = hdnOwnerName.Value;
        else
            txtOwner.Text = "";


        if (!IsPostBack)
        {
            Infragistics.Web.UI.AjaxIndicator ai = Infragistics.Web.UI.Framework.AppSettings.SharedAjaxIndicator;
            ai.Location = Infragistics.Web.UI.RelativeLocation.BottomCenter;
            ai.BlockArea = Infragistics.Web.UI.AjaxIndicatorBlockArea.Control;
            FillAssetStatus();
            // ddlAssetStatus_populate(); Commented Debasish on 22-May
            //this.imgParent.ImageUrl = ISPARENT_IMAGE;
            //this.imgChild.ImageUrl = ISCHILD_IMAGE;

            //hdnOwnerName.Value = this.UserName;
            //hdnOwnerID.Value = Convert.ToString(this.UserID);
            //txtOwner.Text = this.UserName;
            btnSearchClicked = true; // added by kjb on 19 Mar 2013
            this.InitializeSearchCriteria();
            this.txtRefNumber.Focus();
        }

        //if rows exists in  grid than only show  the modify all button and excel icon
        if (uwgAssetSearch.Visible == true && uwgAssetSearch.Rows.Count > 0)
        {
            if (enableModifyAll)
                btnModifyAll.Visible = false; // KJB - 19July2017
            ibExport.Visible = true;
            chkShowChild.Enabled = true;
            chkShowChild.Visible = true;
        }
        else
        {
            btnModifyAll.Visible = false;
            ibExport.Visible = false;
            chkShowChild.Enabled = false;
            chkShowChild.Visible = false;
        }

        if (ddlBusinessUnit != null && ddlBusinessUnit.Items.Count > 0)
            ddlBusinessUnitVal = ddlBusinessUnit.SelectedItem.Value;
        if (ddlMfg != null && ddlMfg.Items.Count > 0)
        {
            ddlMfgName = ddlMfg.SelectedItem.Text;
            ddlMfgID = ddlMfg.SelectedItem.Value;
        }
        if (ddlPrimarySite != null && ddlPrimarySite.Items.Count > 0)
        {
            ddlPrimarySiteVal = ddlPrimarySite.SelectedItem.Value;
        }

    }

    protected void Data_Filtered(object sender, FilteredEventArgs e)
    {
        //FillAsset_Details();
        // Cast the WebDataGrid DataSource to a DataTable  
        //DataTable dt = this.uwgAssetSearch.DataSource as DataTable;

    }
    private void InitializeDropDownLists()
    {
        ddlBusinessUnit.Enabled = false;
        FillDropDownLists();
        if (this.BusinessUnitID <= 0)
            ddlBusinessUnit.SelectedValue = "0";
        else
        {
            if (ddlBusinessUnit.Items.FindByValue(Convert.ToString(this.BusinessUnitID)) != null)
            {
                ddlBusinessUnit.SelectedValue = Convert.ToString(this.BusinessUnitID);
            }
        }
        if (ddlBusinessUnit.Items.Count > 1)
            ddlBusinessUnit.SelectedIndex = 1;
        FillDropdownsByBU();

        if (this.SiteID <= 0)
            ddlPrimarySite.SelectedValue = "0";
        else
        {
            if (ddlPrimarySite.Items.FindByValue(Convert.ToString(this.SiteID)) != null)
            {
                ddlPrimarySite.SelectedValue = Convert.ToString(this.SiteID);
            }
        }

        if (this.LocationID <= 0)
        {
            //ddlLocation.SelectedValue = "0";
            txtParentLocation.Text = "(All)";
            hdnLocationID.Value = "0";
            hdnLocName.Value = "(All)";
        }
        else
        {
            LocationBAL objLoc = new LocationBAL();
            objLoc.LocationID = this.LocationID;
            DataSet ds = objLoc.retrieve();
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                //Infragistics.Web.UI.ListControls.DropDownItem locationItem = new Infragistics.Web.UI.ListControls.DropDownItem();
                //locationItem.Text = dr[DBFields.DBFIELD_LOCATION].ToString();
                //locationItem.Value = dr[DBFields.DBFIELD_LOCATIONID].ToString();
                //ddlLocation.Items.Insert(1, locationItem);
                //ddlLocation.SelectedItemIndex = 1;
                txtParentLocation.Text = dr[DBFields.DBFIELD_LOCATION].ToString();
                hdnLocationID.Value = dr[DBFields.DBFIELD_LOCATIONID].ToString();
            }

        }



        // Asset Status need to come from the DB. Debasish to change.

        FillAssetStatus();
        ddlAssetStatus.SelectedValue = Convert.ToString((int)this.StatusID);

        ddlRFIDCardStatus.SelectedIndex = 0; // by kjb on 11 June 2012
        //ddlRFIDCardStatus.SelectedValue = this.RFIDStatusString;

        //this.cbShowSubAssetsOnly.Checked = ((this.AssociatedStatus == AssociatedStatusValues.ShowAssociatedOnly) ||
        // (this.AssociatedStatus == AssociatedStatusValues.ShowSubAssetsOnly));

    }


    private void InitializeSearchCriteria()
    {

        if (this.txtRefNumber.Enabled) { this.txtRefNumber.Text = ""; }
        if (this.txtAssetName.Enabled) { this.txtAssetName.Text = ""; }

        ddlAssetType.SelectedValue = "0";

        switch (this.SearchPurpose)
        {

            case AssetSearchPurposeValues.DepositToRegistry:
                ddlRFIDCardStatus.Enabled = true;
                ddlAssetStatus.Enabled = true;

                ddlBusinessUnit.Enabled = true;

                ddlPrimarySite.Enabled = true;
                // (Redundant) ddlDepartment.Enabled = true;

                //ddlLocation.Enabled = true;
                //txtParentLocation.Enabled = true;

                break;
            case AssetSearchPurposeValues.IssueFromRegistry:
                ddlRFIDCardStatus.Enabled = false;

                ddlAssetStatus.Enabled = true;

                ddlBusinessUnit.Enabled = true;

                ddlPrimarySite.Enabled = true;
                // (Redundant) ddlDepartment.Enabled = true;

                //ddlLocation.Enabled = true;
                //txtParentLocation.Enabled = true;

                break;

            case AssetSearchPurposeValues.ReceiptConfirmation:
                ddlRFIDCardStatus.Enabled = false;
                ddlAssetStatus.Enabled = true;

                if (this.UserID <= 0) lblMsg.Text = objCommon.displayMessage(MessageCodes.ASSETSEARCH_E_USERNOTSET);

                break;
            case AssetSearchPurposeValues.Transfer:
                ddlRFIDCardStatus.Enabled = false;
                ddlAssetStatus.Enabled = true;

                if (this.UserID <= 0) lblMsg.Text = objCommon.displayMessage(MessageCodes.ASSETSEARCH_E_USERNOTSET);

                break;
            case AssetSearchPurposeValues.Search:
                ddlRFIDCardStatus.Enabled = true;
                ddlBusinessUnit.Enabled = true;

                ddlPrimarySite.Enabled = true;
                // (Redundant) ddlDepartment.Enabled = true;

                //ddlLocation.Enabled = true;
                //txtParentLocation.Enabled = true;

                //cbShowSubAssetsOnly.Visible = true;


                break;
            case AssetSearchPurposeValues.Combine:
                ddlRFIDCardStatus.Enabled = true;

                ddlAssetStatus.Enabled = false;
                ddlBusinessUnit.Enabled = true;

                ddlPrimarySite.Enabled = true;
                // (Redundant) ddlDepartment.Enabled = true;

                //ddlLocation.Enabled = true;
                //txtParentLocation.Enabled = true;

                break;

            case AssetSearchPurposeValues.Delete:
                ddlRFIDCardStatus.Enabled = false;
                ddlAssetStatus.Enabled = false;

                break;
        }

        InitializeDropDownLists();

        PageNum = this.PageNum;

        //if (this.AutoDisplayFirstPage) 
        FillAsset_Details();
    }


    private void EnableRights()
    {

    }

    private void Navigation(int totalRecords)
    {
        int pagesize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());
        Double totalPages = Math.Ceiling(((double)totalRecords / pagesize));

        if ((totalRecords == 1) || (totalPages == 0))
        {
            totalPages = 1;
        }

        //if (pagesize > totalRecords)
        //{
        //    pagesize = (int)totalPages;
        //}

        GoToPageTxt.Text = PageNum.ToString();
        CurrentPage.Text = PageNum.ToString();
        TotalPages.Text = totalPages.ToString();
        this.PageCount = Convert.ToInt32(totalPages.ToString());
    }


    private void FillDropDownLists()
    {
        FillDropDownList(StoredProcedures.SP_ASSETGROUP_LIST, ref ddlAssetType);
        FillDropDownList(StoredProcedures.SP_SITEBYBU_LIST, ref ddlPrimarySite, 0);
        // Debasish : Redundant FillDropDownList(StoredProcedures.SP_DEPARTMENTBYBU_LIST, ref ddlDepartment, 0);

        int intUserID = 0;
        if (Session["UserID"] != null)
            intUserID = Convert.ToInt32(Session["UserID"].ToString());

        FillDropDownListBU(StoredProcedures.SP_BUSINESSUNIT_LISTBYUSERID, ref ddlBusinessUnit, intUserID);

        //Mfg
        PopulateMfgList();
        if (ddlMfg.SelectedIndex == 0)
        {
            hdnModelID.Value = "0";
            hdnModelName.Value = "(All)";
            txtModel.Text = "(All)";
        }
    }

    private void PopulateMfgList()
    {
        ManufacturerBAL objMfg = new iAssetTrack.BAL.ManufacturerBAL();

        ddlMfg.Items.Clear();
        DataSet dsMfg = objMfg.retrieve();
        DataTable dtMfg = dsMfg.Tables[0];

        DataRow dr = dtMfg.NewRow();
        dr[0] = 0;
        dr[1] = "(All)";
        dtMfg.Rows.InsertAt(dr, 0);

        ddlMfg.DataSource = dtMfg;
        ddlMfg.DataValueField = dtMfg.Columns[0].ToString();
        ddlMfg.DataTextField = dtMfg.Columns[1].ToString();
        ddlMfg.DataBind();
    }

    private void FillDropDownListBU(string strStoredProc, ref DropDownList ddl, int id)
    {

        //CommonBAL objCommonBAL = new CommonBAL();
        //ICommon svcBU = (ICommon)RemotingHelper.CreateProxy(typeof(ICommon));
        iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();

        DataTable dt = objCommon.FillDropDownListBU(strStoredProc, "-- Select --", id);
        ddl.DataSource = dt;
        ddl.DataValueField = dt.Columns[0].ToString();
        ddl.DataTextField = dt.Columns[1].ToString();
        ddl.DataBind();

    }

    private void FillDropDownList(string strStoredProc, ref DropDownList ddl)
    {
        //ICommon svc = (ICommon)RemotingHelper.CreateProxy(typeof(ICommon));
        iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();
        DataTable dt = objCommon.FillDropDownList(strStoredProc, "(All)");
        ddl.DataSource = dt;
        ddl.DataValueField = dt.Columns[0].ToString();
        ddl.DataTextField = dt.Columns[1].ToString();
        ddl.DataBind();
    }

    private void FillAsset_Details()
    {

        //this.wpSearchOptions.Expanded = false;

        int iPageIndex = (this.PageNum <= 0 ? 1 : this.PageNum);
        int pagesize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());


        if (btnSearchClicked)
        {
            btnSearchClicked = false;
            if (ddlBusinessUnit.SelectedValue != "") intBusinessUnitID = Convert.ToInt32(ddlBusinessUnit.SelectedValue);
            if (ddlPrimarySite.SelectedValue != "") intPrimarySite = Convert.ToInt32(ddlPrimarySite.SelectedValue);

            //added by kjb on 18th Aug 2011

            if (!string.IsNullOrEmpty(txtParentLocation.Text.Trim()))
            {
                if (!txtParentLocation.Text.Equals("(All)"))
                {
                    if (!string.IsNullOrEmpty(hdnLocationID.Value))
                    {
                        intLocation = int.Parse(hdnLocationID.Value.ToString());
                        this.LocationID = int.Parse(hdnLocationID.Value.ToString());
                        LocationBAL objLoc = new LocationBAL();
                        objLoc.LocationID = this.LocationID;
                        DataSet ds = objLoc.retrieve();
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr = ds.Tables[0].Rows[0];

                            //Infragistics.Web.UI.ListControls.DropDownItem locationItem = new Infragistics.Web.UI.ListControls.DropDownItem();
                            //locationItem.Text = dr[DBFields.DBFIELD_LOCATION].ToString();
                            //locationItem.Value = dr[DBFields.DBFIELD_LOCATIONID].ToString();
                            //ddlLocation.Items.Insert(1, locationItem);
                            //ddlLocation.SelectedItemIndex = 1;
                            txtParentLocation.Text = dr[DBFields.DBFIELD_LOCATION].ToString();
                            hdnLocationID.Value = dr[DBFields.DBFIELD_LOCATIONID].ToString();
                        }
                    }
                }
                else
                {
                    intLocation = 0;
                }
            }

            if (ddlAssetType.SelectedValue != "") intAssetType = Convert.ToInt32(ddlAssetType.SelectedValue);


            // added by kjb on 27th June 2011
            intMfgID = Int32.Parse(ddlMfg.SelectedItem.Value);


            if (!string.IsNullOrEmpty(hdnModelID.Value))
                intModelID = Convert.ToInt32(hdnModelID.Value);

            intAssociatedStatus = this.AssociatedStatus;


            switch (this.SearchPurpose)
            {

                case AssetSearchPurposeValues.Search:

                    intIssuedTo = DBStaticValues.DOC_SEARCH_ANY_VALUE;

                    if (this.hdnOwnerID.Value.Trim() != "0" && this.hdnOwnerID.Value.Trim() != "" && txtOwner.Text.Trim() != "")
                        intOwnerID = Convert.ToInt32(this.hdnOwnerID.Value);
                    else
                        intOwnerID = DBStaticValues.DOC_SEARCH_ANY_VALUE;

                    break;
                case AssetSearchPurposeValues.Combine:

                    if (this.hdnOwnerID.Value.Trim() != "0" && this.hdnOwnerID.Value.Trim() != "" && txtOwner.Text.Trim() != "")
                    {
                        intIssuedTo = Convert.ToInt32(this.hdnOwnerID.Value);
                        intOwnerID = Convert.ToInt32(this.hdnOwnerID.Value);
                    }
                    else
                    {
                        intIssuedTo = DBStaticValues.DOC_SEARCH_ONLY_IF_NULL;
                        intOwnerID = DBStaticValues.DOC_SEARCH_ANY_VALUE;
                    }

                    break;

                case AssetSearchPurposeValues.DepositToRegistry:

                    intReceivedBy = DBStaticValues.DOC_SEARCH_ONLY_IF_NOT_NULL;
                    intIssuedTo = DBStaticValues.DOC_SEARCH_ANY_VALUE;

                    if (this.hdnOwnerID.Value.Trim() != "0" && this.hdnOwnerID.Value.Trim() != "" && txtOwner.Text.Trim() != "")
                        intOwnerID = Convert.ToInt32(this.hdnOwnerID.Value);
                    else
                        intOwnerID = DBStaticValues.DOC_SEARCH_ANY_VALUE;

                    break;

                case AssetSearchPurposeValues.Transfer:

                    intIssuedTo = DBStaticValues.DOC_SEARCH_ANY_VALUE;

                    if (this.hdnOwnerID.Value.Trim() != "0" && this.hdnOwnerID.Value.Trim() != "" && txtOwner.Text.Trim() != "")
                        intOwnerID = Convert.ToInt32(this.hdnOwnerID.Value);
                    else
                        intOwnerID = DBStaticValues.DOC_SEARCH_ANY_VALUE;

                    break;
                case AssetSearchPurposeValues.IssueFromRegistry:

                    intReceivedBy = DBStaticValues.DOC_SEARCH_ANY_VALUE;
                    intIssuedTo = DBStaticValues.DOC_SEARCH_ANY_VALUE;
                    intOwnerID = DBStaticValues.DOC_SEARCH_ANY_VALUE;

                    break;

                case AssetSearchPurposeValues.ReceiptConfirmation:

                    if (this.hdnOwnerID.Value.Trim() != "0" && this.hdnOwnerID.Value.Trim() != "" && txtOwner.Text.Trim() != "")
                        intIssuedTo = Convert.ToInt32(this.hdnOwnerID.Value);
                    else
                        intIssuedTo = DBStaticValues.DOC_SEARCH_ANY_VALUE;


                    intReceivedBy = DBStaticValues.DOC_SEARCH_ONLY_IF_NULL;
                    intOwnerID = DBStaticValues.DOC_SEARCH_ANY_VALUE;

                    break;

                case AssetSearchPurposeValues.Delete:

                    if (this.hdnOwnerID.Value.Trim() != "0" && this.hdnOwnerID.Value.Trim() != "" && txtOwner.Text.Trim() != "")
                        intOwnerID = Convert.ToInt32(this.hdnOwnerID.Value);
                    else
                        intOwnerID = DBStaticValues.DOC_SEARCH_ANY_VALUE;

                    break;
            }

            chrRFIDStatus = Convert.ToChar(ddlRFIDCardStatus.SelectedValue);
            intAssetStatus = Convert.ToInt32(ddlAssetStatus.SelectedValue);

            //add values to session
            Session["Search-LocationID"] = intLocation;
            Session["Search-AssetStatus"] = intAssetStatus;
            Session["Search-RFIDStatus"] = chrRFIDStatus;
            Session["Search-OwnerID"] = intOwnerID;
            Session["Search-ReceivedBy"] = intReceivedBy;
            Session["Search-IssuedTo"] = intIssuedTo;
            Session["Search-AssociatedStatus"] = intAssociatedStatus;
            Session["Search-ModelID"] = intModelID;
            Session["Search-MfgID"] = intMfgID;
            Session["Search-AssetType"] = intAssetType;
            Session["Search-PrimarySite"] = intPrimarySite;
            Session["Search-BusinessUnitID"] = intBusinessUnitID;

        }
        else
        {
            intLocation = int.Parse(Session["Search-LocationID"].ToString());
            intAssetStatus = int.Parse(Session["Search-AssetStatus"].ToString());
            chrRFIDStatus = (char)Session["Search-RFIDStatus"];
            intOwnerID = int.Parse(Session["Search-OwnerID"].ToString());
            intReceivedBy = int.Parse(Session["Search-ReceivedBy"].ToString());
            intIssuedTo = int.Parse(Session["Search-IssuedTo"].ToString());
            intAssociatedStatus = (AssociatedStatusValues)Session["Search-AssociatedStatus"];
            intModelID = int.Parse(Session["Search-ModelID"].ToString());
            intMfgID = int.Parse(Session["Search-MfgID"].ToString());
            intAssetType = int.Parse(Session["Search-AssetType"].ToString());
            intPrimarySite = int.Parse(Session["Search-PrimarySite"].ToString());
            intBusinessUnitID = int.Parse(Session["Search-BusinessUnitID"].ToString());
        }

        #region v3.8
        int loggedInUserID = int.Parse(Session["UserID"].ToString());
        int siteResEnabled = bool.Parse(Session["SiteRestrictEnabled"].ToString()) == true ? 1 : 0;

        #endregion

        objAsset = new iAssetTrack.BAL.AssetCreationBAL();


        DataSet dtAsset = objAsset.GetAssetsList
                                        (intBusinessUnitID,
                                            intPrimarySite,
                                            intAssetType,
                                            intLocation,
                                            intOwnerID,
                                            txtRefNumber.Text,
                                            txtAssetName.Text,
                                            txtHostName.Text,///Added on 8May2013
                                            chrRFIDStatus,
                                            intAssetStatus,
                                            strOwner,
                                            intOwnerID,
                                            iPageIndex,
                                            pagesize,
                                            intIssuedTo,
                                            intReceivedBy,
                                            (int)intAssociatedStatus,
                                            (int)this.ParentStatus,
                                            (int)this.ParentAssetID,
                                            txtRFIDTag.Text,
                                            intMfgID,
                                            intModelID,
                                            siteResEnabled,
                                            loggedInUserID
                                            );

        //this  will execute when logged in user is a tenant user
        //if (bool.Parse(Session["TenantUser"].ToString()))
        //{
        //    UserBAL objUser = new UserBAL();
        //    objUser.UserID = Convert.ToInt32(Session["UserID"]);
        //    DataSet dsTenants = objUser.retrieveTenantDetails();
        //    if (dsTenants.Tables.Count > 0)
        //    {
        //        int tenantId = Convert.ToInt32(dsTenants.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_ID].ToString());
        //        //get asset list based on tenant id
        //        TenantAssetBAL objTenantAsset = new TenantAssetBAL();
        //        objTenantAsset.TenantId = tenantId;
        //        DataSet dsTenantAssets = objTenantAsset.retrieve();
        //        if (dsTenantAssets.Tables.Count > 0 && dsTenantAssets.Tables[0].Rows.Count > 0)
        //        {
        //            var arlAssetIds = (from a in dsTenantAssets.Tables[0].AsEnumerable()
        //                               select a.Field<int>("AssetId"));
        //            if (arlAssetIds != null)
        //            {
        //                ArrayList arlIds = new ArrayList();
        //                foreach (int val in arlAssetIds)
        //                {
        //                    arlIds.Add(val);
        //                }

        //                // apply tenant location list fileter on asset list from db.
        //                DataRow[] table1Rows = dtAsset.Tables[0].Select("AssetID IN ( " + string.Join(",", arlIds.ToArray()) + ")");
        //                DataRow[] table2Rows = dtAsset.Tables[1].Select("ParentAssetID IN ( " + string.Join(",", arlIds.ToArray()) + ")");

        //                if (table1Rows != null && table1Rows.Length > 0)
        //                {
        //                    dtAsset.Tables[0].Rows.Clear();
        //                    foreach (DataRow dr in table1Rows)
        //                    {
        //                        dtAsset.Tables[0].Rows.Add(dr.ItemArray);
        //                    }
        //                }
        //                //sub table
        //                if (table2Rows != null && table2Rows.Length > 0)
        //                {
        //                    dtAsset.Tables[1].Rows.Clear();
        //                    foreach (DataRow dr in table2Rows)
        //                    {
        //                        dtAsset.Tables[1].Rows.Add(dr.ItemArray);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}



        if (dtAsset.Tables[0].Rows.Count > 0)
        {
            // TODO:
            //ibExport.Visible = this.AllowExportToExcel;
            lblMsg.Visible = false;
            uwgAssetSearch.Visible = true;
            // enable ModifyAll button -- kjb 
            if (dtAsset.Tables[0].Rows.Count > 1)
            {
                if (enableModifyAll)
                {
                    btnModifyAll.Enabled = false;//true
                    btnModifyAll.Visible = false;
                }
            }
            else
            {
                btnModifyAll.Enabled = false;
                btnModifyAll.Visible = false;
            }
            //Session["Asset"] = dtAsset.Tables[0];
            dtAsset.Tables[0].TableName = "ParentTable";
            dtAsset.Tables[1].TableName = "ChildTable";
            if (dtAsset.Tables[1].Rows.Count > 0)
            {
                dtAsset.Relations.Add("Sub Assets", dtAsset.Tables[0].Columns["AssetID"], dtAsset.Tables[1].Columns["ParentAssetID"]);

            }

            ArrayList myArrayList = new ArrayList();
            for (int i = 0; i <= dtAsset.Tables[0].Rows.Count - 1; i++)
            {
                myArrayList.Add(dtAsset.Tables[0].Rows[i]["AssetID"].ToString());
                strAssetId = strAssetId + dtAsset.Tables[0].Rows[i]["AssetID"].ToString() + ",";
            }
            hdnAssetCount.Value = myArrayList.Count.ToString();
            hdnAssetId.Value = strAssetId.Substring(0, strAssetId.Length - 1);
            uwgAssetSearch.GridView.ClearDataSource();
            uwgAssetSearch.DataSource = dtAsset;
            //uwgAssetSearch.Bands.Clear();
            uwgAssetSearch.DataBind();


            if (this.AllowMultiSelect)
            {
                if (uwgAssetSearch.Columns.Count > 0)
                {
                    //uwgAssetSearch.Columns[0].AllowUpdate = Infragistics.WebUI.UltraWebGrid.AllowUpdate.Yes;
                    uwgAssetSearch.Columns[0].Width = Unit.Pixel(20);

                    uwgAssetSearch.Columns[0].Hidden = false;
                }

            }
            else
            {
                if (uwgAssetSearch.Columns.Count > 0)
                {
                    uwgAssetSearch.Columns[0].Hidden = true;
                }
            }


            EnableRights();

            ///--------------------------this is for performance issue code-------------------------------

            if (dtAsset.Tables[2].Rows.Count > 0)
            {
                Navigation(Convert.ToInt32(dtAsset.Tables[2].Rows[0][0].ToString()));

            }

            ibExport.Enabled = true;
            ibExport.Visible = true;
            chkShowChild.Enabled = true;
            chkShowChild.Visible = true;


            FirstPage.Visible = true;
            LastPage.Visible = true;
            NextPage.Visible = true;
            btnPreviousPage.Visible = true;

            SepLbl.Visible = true;
            GoToPageImb.Visible = true;
            GoToPageTxt.Visible = true;
            CurrentPage.Visible = true;
            TotalPages.Visible = true;


            FirstPage.Enabled = true;
            LastPage.Enabled = true;
            NextPage.Enabled = true;
            btnPreviousPage.Enabled = true;

            if (Convert.ToInt32(TotalPages.Text) == 1)
            {
                FirstPage.Enabled = false;
                LastPage.Enabled = false;
                NextPage.Enabled = false;
                btnPreviousPage.Enabled = false;


                FirstPage.Visible = false;
                LastPage.Visible = false;
                NextPage.Visible = false;
                btnPreviousPage.Visible = false;

                SepLbl.Visible = false;
                GoToPageImb.Visible = false;
                GoToPageTxt.Visible = false;
                CurrentPage.Visible = false;
                TotalPages.Visible = false;
                //ibExport.Enabled = false; 
                //ibExport.Visible = false;
            }
            else
            {
                if (Convert.ToInt32(CurrentPage.Text) == 1)
                {
                    FirstPage.Enabled = false;
                    btnPreviousPage.Enabled = false;
                }
                if (Convert.ToInt32(CurrentPage.Text) == Convert.ToInt32(TotalPages.Text))
                {



                    LastPage.Enabled = false;
                    NextPage.Enabled = false;
                }
            }
            ///
            this.uwgAssetSearch.Focus();
            //uwgAssetSearch.Behaviors.ColumnFixing.FixedColumns.Add("BusinessUnit");
            //uwgAssetSearch.Behaviors.ColumnFixing.FixedColumns.Add("AssetGroup");
            //uwgAssetSearch.Behaviors.ColumnFixing.FixedColumns.Add("RefNumber");
            //uwgAssetSearch.Behaviors.ColumnFixing.FixedColumns.Add("MfgName");
            //uwgAssetSearch.Behaviors.ColumnFixing.FixedColumns.Add("ModelName");
            //uwgAssetSearch.Behaviors.ColumnFixing.FixedColumns.Add("RFID Card No");

        }
        else
        {

            Session["Asset"] = null;
            //ibExport.Visible = this.AllowExportToExcel;
            //ibExport.Visible = false;
            lblMsg.Visible = true;
            lblMsg.Text = "No Records Found. Please expand your search criteria.";
            uwgAssetSearch.Visible = false;
            btnModifyAll.Enabled = false;
            btnModifyAll.Visible = false;

            ibExport.Enabled = false;
            ibExport.Visible = false;
            chkShowChild.Enabled = false;
            chkShowChild.Visible = false;

            // commented by kjb -- 20120427046  -- un commented by kjb on 06 Mar 2013

            FirstPage.Visible = false;
            LastPage.Visible = false;
            NextPage.Visible = false;
            btnPreviousPage.Visible = false;

            SepLbl.Visible = false;
            GoToPageImb.Visible = false;
            GoToPageTxt.Visible = false;
            CurrentPage.Visible = false;
            TotalPages.Visible = false;
            ibExport.Enabled = false;
            ibExport.Visible = false;
            chkShowChild.Enabled = false;
            chkShowChild.Visible = false;
            //

            this.uwgAssetSearch.Focus();

            //12July2012-V00004

            //uwgAssetSearch.Behaviors.ColumnFixing.FixedColumns.Add("BusinessUnit");
            //uwgAssetSearch.Behaviors.ColumnFixing.FixedColumns.Add("AssetGroup");
            //uwgAssetSearch.Behaviors.ColumnFixing.FixedColumns.Add("RefNumber");
            //uwgAssetSearch.Behaviors.ColumnFixing.FixedColumns.Add("MfgName");
            //uwgAssetSearch.Behaviors.ColumnFixing.FixedColumns.Add("ModelName");
            //uwgAssetSearch.Behaviors.ColumnFixing.FixedColumns.Add("RFID Card No");

        }



    }

    private void FillAssetDetailsForExport()
    {


        int iPageIndex = (this.PageNum <= 0 ? 1 : this.PageNum);
        int pagesize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());
        int intBusinessUnitID = 0;
        int intPrimarySite = 0;
        // (Redundant) int intDepartment = 0;
        int intLocation = 0;
        int intAssetType = 0;
        if (ddlBusinessUnit.SelectedValue != "") intBusinessUnitID = Convert.ToInt32(ddlBusinessUnit.SelectedValue);
        if (ddlPrimarySite.SelectedValue != "") intPrimarySite = Convert.ToInt32(ddlPrimarySite.SelectedValue);
        ////added by kjb on 18th Aug 2011
        if (!string.IsNullOrEmpty(txtParentLocation.Text.Trim()))
        {
            if (!txtParentLocation.Text.Equals("(All)"))
            {
                if (!string.IsNullOrEmpty(hdnLocationID.Value))
                {
                    intLocation = int.Parse(hdnLocationID.Value.ToString());
                    this.LocationID = int.Parse(hdnLocationID.Value.ToString());
                    LocationBAL objLoc = new LocationBAL();
                    objLoc.LocationID = this.LocationID;
                    DataSet ds = objLoc.retrieve();
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];

                        //Infragistics.Web.UI.ListControls.DropDownItem locationItem = new Infragistics.Web.UI.ListControls.DropDownItem();
                        //locationItem.Text = dr[DBFields.DBFIELD_LOCATION].ToString();
                        //locationItem.Value = dr[DBFields.DBFIELD_LOCATIONID].ToString();
                        //ddlLocation.Items.Insert(1, locationItem);
                        //ddlLocation.SelectedItemIndex = 1;
                        txtParentLocation.Text = dr[DBFields.DBFIELD_LOCATION].ToString();
                        hdnLocationID.Value = dr[DBFields.DBFIELD_LOCATIONID].ToString();
                    }
                }
            }
            else
            {
                intLocation = 0;
            }
        }
        //if (ddlLocation.SelectedItemIndex > 0)
        //    intLocation = Convert.ToInt32(ddlLocation.SelectedItem.Value);


        if (ddlAssetType.SelectedValue != "") intAssetType = Convert.ToInt32(ddlAssetType.SelectedValue);
        // added by kjb on 27th June 2011
        int intMfgID = Int32.Parse(ddlMfg.SelectedItem.Value);
        int intModelID = 0;

        //if (ddlModelList.SelectedItemIndex > 0)
        //    intModelID = Convert.ToInt32(ddlModelList.SelectedItem.Value);

        if (!string.IsNullOrEmpty(hdnModelID.Value))
            intModelID = Convert.ToInt32(hdnModelID.Value);


        AssociatedStatusValues intAssociatedStatus = this.AssociatedStatus;


        switch (this.SearchPurpose)
        {

            case AssetSearchPurposeValues.Search:

                intIssuedTo = DBStaticValues.DOC_SEARCH_ANY_VALUE;

                if (this.hdnOwnerID.Value.Trim() != "0" && this.hdnOwnerID.Value.Trim() != "" && txtOwner.Text.Trim() != "")
                    intOwnerID = Convert.ToInt32(this.hdnOwnerID.Value);
                else
                    intOwnerID = DBStaticValues.DOC_SEARCH_ANY_VALUE;

                break;
            case AssetSearchPurposeValues.Combine:

                if (this.hdnOwnerID.Value.Trim() != "0" && this.hdnOwnerID.Value.Trim() != "" && txtOwner.Text.Trim() != "")
                {
                    intIssuedTo = Convert.ToInt32(this.hdnOwnerID.Value);
                    intOwnerID = Convert.ToInt32(this.hdnOwnerID.Value);
                }
                else
                {
                    intIssuedTo = DBStaticValues.DOC_SEARCH_ONLY_IF_NULL;
                    intOwnerID = DBStaticValues.DOC_SEARCH_ANY_VALUE;
                }

                break;

            case AssetSearchPurposeValues.DepositToRegistry:

                intReceivedBy = DBStaticValues.DOC_SEARCH_ONLY_IF_NOT_NULL;
                intIssuedTo = DBStaticValues.DOC_SEARCH_ANY_VALUE;

                if (this.hdnOwnerID.Value.Trim() != "0" && this.hdnOwnerID.Value.Trim() != "" && txtOwner.Text.Trim() != "")
                    intOwnerID = Convert.ToInt32(this.hdnOwnerID.Value);
                else
                    intOwnerID = DBStaticValues.DOC_SEARCH_ANY_VALUE;

                break;

            case AssetSearchPurposeValues.Transfer:

                intIssuedTo = DBStaticValues.DOC_SEARCH_ANY_VALUE;

                if (this.hdnOwnerID.Value.Trim() != "0" && this.hdnOwnerID.Value.Trim() != "" && txtOwner.Text.Trim() != "")
                    intOwnerID = Convert.ToInt32(this.hdnOwnerID.Value);
                else
                    intOwnerID = DBStaticValues.DOC_SEARCH_ANY_VALUE;

                break;
            case AssetSearchPurposeValues.IssueFromRegistry:

                intReceivedBy = DBStaticValues.DOC_SEARCH_ANY_VALUE;
                intIssuedTo = DBStaticValues.DOC_SEARCH_ANY_VALUE;
                intOwnerID = DBStaticValues.DOC_SEARCH_ANY_VALUE;

                break;

            case AssetSearchPurposeValues.ReceiptConfirmation:

                if (this.hdnOwnerID.Value.Trim() != "0" && this.hdnOwnerID.Value.Trim() != "" && txtOwner.Text.Trim() != "")
                    intIssuedTo = Convert.ToInt32(this.hdnOwnerID.Value);
                else
                    intIssuedTo = DBStaticValues.DOC_SEARCH_ANY_VALUE;


                intReceivedBy = DBStaticValues.DOC_SEARCH_ONLY_IF_NULL;
                intOwnerID = DBStaticValues.DOC_SEARCH_ANY_VALUE;

                break;

            case AssetSearchPurposeValues.Delete:

                if (this.hdnOwnerID.Value.Trim() != "0" && this.hdnOwnerID.Value.Trim() != "" && txtOwner.Text.Trim() != "")
                    intOwnerID = Convert.ToInt32(this.hdnOwnerID.Value);
                else
                    intOwnerID = DBStaticValues.DOC_SEARCH_ANY_VALUE;

                break;

        }




        char chrRFIDStatus = Convert.ToChar(ddlRFIDCardStatus.SelectedValue);
        int intAssetStatus = Convert.ToInt32(ddlAssetStatus.SelectedValue);

        #region v3.8
        int loggedInUserID = int.Parse(Session["UserID"].ToString());
        int siteResEnabled = bool.Parse(Session["SiteRestrictEnabled"].ToString()) == true ? 1 : 0;

        #endregion

        objAsset = new iAssetTrack.BAL.AssetCreationBAL();


        DataSet dtAsset = objAsset.GetAssetsListByPageForExport
                                        (intBusinessUnitID,
                                            intPrimarySite,
                                            intAssetType,
                                            intLocation,
                                            intOwnerID,
                                            txtRefNumber.Text,
                                            txtAssetName.Text,
                                               txtHostName.Text,///Added on 8May2013
                                            chrRFIDStatus,
                                            intAssetStatus,
                                            strOwner,
                                            intOwnerID,
                                            iPageIndex,
                                            pagesize,
                                            intIssuedTo,
                                            intReceivedBy,
                                            (int)intAssociatedStatus,
                                            (int)this.ParentStatus,
                                            (int)this.ParentAssetID,
                                            txtRFIDTag.Text,
                                            intMfgID,
                                            intModelID,
                                            siteResEnabled,
                                            loggedInUserID,
                                            (chkShowChild.Checked == true ? 1 : 0)
                                            );


        if (dtAsset.Tables[0].Rows.Count > 0)
        {
            // TODO:
            ibExport.Visible = this.AllowExportToExcel;
            chkShowChild.Enabled = this.AllowExportToExcel;
            chkShowChild.Visible = this.AllowExportToExcel;
            lblMsg.Visible = false;
            uwgAssetSearch.Visible = true;
            //Session["Asset"] = dtAsset.Tables[0];
            ////*Added on 18Apr2013
            dtAsset.Tables[0].TableName = "ParentTable";
            dtAsset.Tables[1].TableName = "ChildTable";
            if (dtAsset.Tables[1].Rows.Count > 0)
            {
                dtAsset.Relations.Add("Sub Assets", dtAsset.Tables[0].Columns["AssetID"], dtAsset.Tables[1].Columns["ParentAssetID"]);

            }
            uwgAssetSearch.DataSource = dtAsset;

            //Commented on 18Apr2013--Removed binding from grid 
            //uwgAssetSearch.DataSource = dtAsset.Tables[0];
            //uwgAssetSearch.Bands.Clear();
            uwgAssetSearch.DataBind();


            // Disable paging if there aren't enough rows for more than one page
            ////////if (dtWorker.Rows.Count <= Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString()))
            ////////    this.uwgWorkerSearch.DisplayLayout.Pager.AllowPaging = false;

            if (this.AllowMultiSelect)
            {
                if (uwgAssetSearch.Columns.Count > 0)
                {
                    //uwgAssetSearch.Columns[0].AllowUpdate = Infragistics.WebUI.UltraWebGrid.AllowUpdate.Yes;
                    uwgAssetSearch.Columns[0].Width = Unit.Pixel(20);

                    uwgAssetSearch.Columns[0].Hidden = false;
                }

            }
            else
            {
                if (uwgAssetSearch.Columns.Count > 0)
                {
                    uwgAssetSearch.Columns[0].Hidden = true;
                }
            }


            EnableRights();

            ///--------------------------this is for performance issue code-------------------------------


            ibExport.Enabled = this.AllowExportToExcel;
            ibExport.Visible = this.AllowExportToExcel;
            chkShowChild.Enabled = this.AllowExportToExcel;
            chkShowChild.Visible = this.AllowExportToExcel;

            // commented by kjb -- 20120427046

            FirstPage.Visible = true;
            LastPage.Visible = true;
            NextPage.Visible = true;
            btnPreviousPage.Visible = true;

            SepLbl.Visible = true;
            GoToPageImb.Visible = true;
            GoToPageTxt.Visible = true;
            CurrentPage.Visible = true;
            TotalPages.Visible = true;


            FirstPage.Enabled = true;
            LastPage.Enabled = true;
            NextPage.Enabled = true;
            btnPreviousPage.Enabled = true;

            if (Convert.ToInt32(TotalPages.Text) == 1)
            {
                FirstPage.Enabled = false;
                LastPage.Enabled = false;
                NextPage.Enabled = false;
                btnPreviousPage.Enabled = false;


                FirstPage.Visible = false;
                LastPage.Visible = false;
                NextPage.Visible = false;
                btnPreviousPage.Visible = false;

                SepLbl.Visible = false;
                GoToPageImb.Visible = false;
                GoToPageTxt.Visible = false;
                CurrentPage.Visible = false;
                TotalPages.Visible = false;
                ibExport.Enabled = false; ;
                ibExport.Visible = false;
                chkShowChild.Enabled = false;
                chkShowChild.Visible = false;
            }
            else
            {
                if (Convert.ToInt32(CurrentPage.Text) == 1)
                {
                    FirstPage.Enabled = false;
                    btnPreviousPage.Enabled = false;
                }
                if (Convert.ToInt32(CurrentPage.Text) == Convert.ToInt32(TotalPages.Text))
                {



                    LastPage.Enabled = false;
                    NextPage.Enabled = false;
                }
            }



        }
        else
        {

            Session["Asset"] = null;
            ibExport.Visible = this.AllowExportToExcel;
            chkShowChild.Visible = this.AllowExportToExcel;
            chkShowChild.Enabled = this.AllowExportToExcel;

            lblMsg.Visible = true;
            lblMsg.Text = "No Records Found. Please expand your search criteria.";
            uwgAssetSearch.Visible = false;

            //commented by kjb -- 20120427046 -- un commented by kjb on 06 Mar 2013

            FirstPage.Visible = false;
            LastPage.Visible = false;
            NextPage.Visible = false;
            btnPreviousPage.Visible = false;

            SepLbl.Visible = false;
            GoToPageImb.Visible = false;
            GoToPageTxt.Visible = false;
            CurrentPage.Visible = false;
            TotalPages.Visible = false;


            ibExport.Enabled = false;
            ibExport.Visible = false;
            chkShowChild.Visible = false;
            chkShowChild.Enabled = false;

        }

        this.uwgAssetSearch.Focus();

        //ExportToExcel(dtAsset);//Added on 18Apr2013  -Exports to excel
    }

    private void FillDropdownsByBU()
    {
        int intBusinessUnitID = 0;
        if (ddlBusinessUnit.SelectedValue != "")
            intBusinessUnitID = Convert.ToInt32(ddlBusinessUnit.SelectedValue);
        #region v3.8

        //FillDropDownList(StoredProcedures.SP_SITEBYBU_LIST, ref ddlPrimarySite, intBusinessUnitID);
        if (bool.Parse(Session["SiteRestrictEnabled"].ToString()) == false)
        {
            FillDropDownList(StoredProcedures.SP_SITEBYBU_LIST, ref ddlPrimarySite, intBusinessUnitID);
        }
        else
        {
            SitesBAL objSitesBAL = new SitesBAL();
            int loggedInUserID = int.Parse(Session["UserID"].ToString());
            DataSet dsSites = objSitesBAL.retrieveRestrictedSiteList(intBusinessUnitID, loggedInUserID);
            DataTable dt = dsSites.Tables[0];

            DataRow dr = dt.NewRow();
            dr[0] = 0;
            dr[1] = "(All)";
            dr[2] = "(All)";

            dt.Rows.InsertAt(dr, 0);

            ddlPrimarySite.DataSource = dt;
            ddlPrimarySite.DataValueField = dt.Columns[0].ToString();
            ddlPrimarySite.DataTextField = dt.Columns[1].ToString();
            ddlPrimarySite.DataBind();
        }

        #endregion
        this.SiteID = 0;
        this.LocationID = 0;

    }


    private void FillDropDownList(string strStoredProc, ref DropDownList ddl, int id)
    {
        //ICommon svc = (ICommon)RemotingHelper.CreateProxy(typeof(ICommon));
        iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();
        DataTable dt = objCommon.FillDropDownList(strStoredProc, "(All)", id);
        ddl.DataSource = dt;
        ddl.DataValueField = dt.Columns[0].ToString();
        ddl.DataTextField = dt.Columns[1].ToString();
        ddl.DataBind();
    }

    private void FillAssetStatus()
    {
        ddlAssetStatus.Items.Clear();
        objAssetStatus = new iAssetTrack.BAL.ConfigBAL();
        DataSet ds = objAssetStatus.RetrieveAssetStatus();
        //ListItem itm = new ListItem("(All)", "0");
        //ddlAssetStatus.Items.Add(itm);
        ddlAssetStatus.DataSource = ds;
        DataTable dt = ds.Tables[0];
        DataRow dr = dt.NewRow();
        dr[0] = 0;
        dr[1] = "(All)";
        dt.Rows.InsertAt(dr, 0);
        ddlAssetStatus.DataValueField = dt.Columns["AssetStatusID"].ToString();
        ddlAssetStatus.DataTextField = dt.Columns["Status"].ToString();
        ddlAssetStatus.DataBind();

    }


    private void ShowMessage(string mess)
    {
        string strScript = "<script type=\"text/javascript\">validNavigation = true;alert(\"" + mess + "\");</script>";
        if (!Page.ClientScript.IsStartupScriptRegistered("FORMMESSAGE"))
            Page.ClientScript.RegisterStartupScript(typeof(Page), "FORMMESSAGE", strScript);
    }




    #region Page Events

    protected void wibSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        // commented by kjb -- 20120427046

        SepLbl.Visible = true;
        GoToPageImb.Visible = true;
        GoToPageTxt.Visible = true;
        CurrentPage.Visible = true;
        TotalPages.Visible = true;

        //

        //Store the selections
        //if (this.AllowMultiSelect)
        //{
        //    //Clear all the selections since the pagination has changed
        //    this.ReSetSelections();
        //    //this.CurrentPageSelections = hdnPageSelections.Value;
        //}

        if (this.hdnOwnerID.Value == "")
        {
            this.UserID = 0;
        }
        else
        {
            this.UserID = Convert.ToInt32(this.hdnOwnerID.Value);
        }
        this.UserName = this.hdnOwnerName.Value;
        txtOwner.Text = this.hdnOwnerName.Value;
        btnSearchClicked = true;
        PageNum = 1;
        FillAsset_Details();
    }


    protected void wibReset_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.txtRFIDTag.Text = "";
        this.InitializeSearchCriteria();
        this.hdnOwnerID.Value = "0";
        this.hdnOwnerName.Value = "";
        this.txtOwner.Text = "";
        lblMsg.Visible = false;
        lblMsg.Text = "";
    }

    #region commnted by kjb -- 20120427046 --- un commented by kjb on 06 Mar 2013

    protected void NavigationLink_Click(Object sender, CommandEventArgs e)
    {
        if (this.AllowMultiSelect) this.CurrentPageSelections = hdnPageSelections.Value;
        switch (e.CommandName)
        {
            case "First":
                this.PageNum = 1;
                break;
            case "Last":
                this.PageNum = Convert.ToInt16(TotalPages.Text);
                break;
            case "Next":
                this.PageNum = Convert.ToInt16(CurrentPage.Text) + 1;
                break;
            case "Prev":
                this.PageNum = Convert.ToInt16(CurrentPage.Text) - 1;
                break;
        }

        if (this.AllowMultiSelect)
        {
            if (this.CurrentPageSelections == null && this.SelectedList != null) this.CurrentPageSelections = String.Join(",", this.SelectedList);
            hdnPageSelections.Value = "";
        }
        FillAsset_Details();
        // BindGridView();
    }


    protected void GoToPageImb_Click(object sender, ImageClickEventArgs e)
    {


        if (GoToPageTxt.Text != "")
        {
            try
            {
                int pagenumber = Convert.ToInt32(GoToPageTxt.Text);
                if (pagenumber <= 0)
                {
                    ShowMessage("Enter valid page count");
                    return;

                }

            }
            catch (Exception ex)
            {
                ShowMessage("Enter valid page count");
                ExceptionPolicy.HandleException(ex, "errDCTrack");
                return;
            }

            int maxPage = Convert.ToInt32(TotalPages.Text);
            int goToPage = Convert.ToInt32(GoToPageTxt.Text);

            if (goToPage <= maxPage)
            {
                PageNum = goToPage;
                FillAsset_Details();
                // BindGridView();
            }
            else
            {

                //need to show message
                ShowMessage("Invalid page count");

            }
        }
        else
        {
            ShowMessage("Invalid page count");
        }


    }

    #endregion


    protected void ibExport_Click(object sender, ImageClickEventArgs e)
    {
        Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        this.eExporter.DataExportMode = Infragistics.Web.UI.GridControls.DataExportMode.AllDataInDataSource;
        Infragistics.Documents.Excel.Workbook wBook = new Infragistics.Documents.Excel.Workbook(excelFormat);

        FillAssetDetailsForExport();
        //FillAsset_Details();
        //this.eExporter.Export(this.uwgAssetSearch, wBook);// commented by kjb on 18 Apr 2013 
        if (this.eExporter.DataExportMode == DataExportMode.AllDataInDataSource)
        {
            this.uwgAssetSearch.InitialDataBindDepth = -1;
            this.uwgAssetSearch.DataBind();
        }
        bool singleGridPerSheet = false;
        this.eExporter.DataExportMode = DataExportMode.AllDataInDataSource;
        this.eExporter.WorkbookFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;

        this.eExporter.Export(singleGridPerSheet, this.uwgAssetSearch);
        FillAsset_Details();
    }

    protected void ddlBusinessUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBusinessUnit != null && ddlBusinessUnit.Items.Count > 0)
            ddlBusinessUnitVal = ddlBusinessUnit.SelectedItem.Value;

        FillDropdownsByBU();
        // mfg
        ddlMfg.SelectedIndex = 0;
    }

    protected void ddlPrimarySite_SelectedIndexChanged(object sender, EventArgs e)
    {
        //FillLocationsBySite();
        //ddlLocation.SelectedItemIndex = 0;
        //ddlLocation.CurrentValue = ddlLocation.Items[0].Text;
        //ddlLocation.SelectedValue = ddlLocation.Items[0].Value;

        if (ddlPrimarySite != null && ddlPrimarySite.Items.Count > 0)
        {
            ddlPrimarySiteVal = ddlPrimarySite.SelectedItem.Value;
        }
        txtParentLocation.Text = "(All)";
        hdnLocationID.Value = "0";
        hdnLocName.Value = "(All)";
        //Commented on 03-Mar-2013
        // ibExport.Visible = true;

        //V000013
        //Added on 24-08-2012
        // ibExport.Enabled = true;
    }

    private void ddlAssetStatus_populate()
    {
        ddlAssetStatus.Items.Clear();
        foreach (string item in Enum.GetNames(typeof(AssetStatusValues)))
        {

            int value = (int)Enum.Parse(typeof(AssetStatusValues), item);
            ListItem listItem = new ListItem(item, value.ToString());
            ddlAssetStatus.Items.Add(listItem);
        }

    }

    protected void uwgAssetSearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //grdBU.Pag = e.NewPageIndex;
        this.FillAsset_Details();
    }

    protected void uwgAssetSearch_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e)
    {
        //if (e.CommandName == "Page")
        //{
        //    switch (e.CommandArgument.ToString())
        //    {
        //        case "First":
        //            if (this.uwgAssetSearch.Behaviors.Paging.PageIndex > 0)
        //            {
        //                FillAsset_Details();
        //                pagerControl.SetupPageList(this.uwgAssetSearch.Behaviors.Paging.PageCount);
        //                pagerControl.SetCurrentPageNumber(1);
        //            }
        //            break;
        //        case "Prev":
        //            if (this.uwgAssetSearch.Behaviors.Paging.PageIndex > 0)
        //            {
        //                FillAsset_Details();
        //                pagerControl.SetupPageList(this.uwgAssetSearch.Behaviors.Paging.PageCount);
        //                pagerControl.SetCurrentPageNumber(this.uwgAssetSearch.Behaviors.Paging.PageIndex - 1);
        //            }
        //            break;
        //        case "Next":
        //            if (this.uwgAssetSearch.Behaviors.Paging.PageIndex + 1 < this.uwgAssetSearch.Behaviors.Paging.PageCount)
        //            {
        //                FillAsset_Details();
        //                pagerControl.SetupPageList(this.uwgAssetSearch.Behaviors.Paging.PageCount);
        //                pagerControl.SetCurrentPageNumber(this.uwgAssetSearch.Behaviors.Paging.PageIndex + 1);
        //            }
        //            break;
        //        case "Last":
        //            if (this.uwgAssetSearch.Behaviors.Paging.PageIndex + 1 < this.uwgAssetSearch.Behaviors.Paging.PageCount)
        //            {
        //                FillAsset_Details();
        //                pagerControl.SetupPageList(this.uwgAssetSearch.Behaviors.Paging.PageCount);
        //                pagerControl.SetCurrentPageNumber(this.uwgAssetSearch.Behaviors.Paging.PageCount);
        //            }
        //            break;
        //    }
        //}

    }
    protected void uwgAssetSearch_InitializeRow(object sender, Infragistics.Web.UI.GridControls.RowEventArgs e)
    {
        //uwgAssetSearch.DisplayLayout.Bands[0].FilterOptions.AllowRowFiltering = Infragistics.WebUI.UltraWebGrid.RowFiltering.OnClient;
        ////uwgAssetSearch.DisplayLayout.Bands[0].FilterOptions.FilterUIType = Infragistics.WebUI.UltraWebGrid.FilterUIType.FilterRow;
        try
        {
            #region commented code
            //if (m_ClientSideEventEnabled)
            //{

            //    string strID = e.Row.Cells.FromKey(GRID_COL_KEY_ASSETID).Text.Replace("'", "~~~");

            //    string strJSFunc = strClientSideOnSelection;
            //    strJSFunc = strJSFunc.Replace(JS_PARAM_ASSETID, strID);
            //    strJSFunc = strJSFunc.Replace(JS_PARAM_REFNO, e.Row.Cells.FromKey(GRID_COL_KEY_REFNO).Text);
            //    strJSFunc = strJSFunc.Replace(JS_PARAM_ASSETNAME, e.Row.Cells.FromKey(GRID_COL_KEY_ASSETNAME).Text);
            //    strJSFunc = strJSFunc.Replace(JS_PARAM_PARENTID, (e.Row.Cells.FromKey(GRID_COL_KEY_PARENTASSETID).Value == null ? "0" : e.Row.Cells.FromKey(GRID_COL_KEY_PARENTASSETID).Text));
            //    strJSFunc = strJSFunc.Replace(JS_PARAM_LEVEL, Convert.ToString(e.Row.Band.Index));

            //    //e.Row.Cells.FromKey(GRID_COL_KEY_ASSETNAME).Column.Type = ColumnType.HyperLink;
            //    //e.Row.Cells.FromKey(GRID_COL_KEY_ASSETNAME).TargetURL = strJSFunc;

            //    e.Row.Cells.FromKey(GRID_COL_KEY_REFNO).Text = "<div><a href=\"" + strJSFunc + "\" CssClass=\"lnk\">" + e.Row.Cells.FromKey(GRID_COL_KEY_REFNO).Text + "</a></div>";
            //}
            //else
            //{
            //    //e.Row.Cells.FromKey(GRID_COL_KEY_ASSETNAME).TargetURL = "lbTitle_Click()";
            //    //e.Row.Cells.FromKey(GRID_COL_KEY_ASSETNAME).Text = "<div><asp:LinkButton ID=\"lbName\" runat=\"server\" CssClass=\"lnk\" OnClick=\"lbTitle_Click\"><%# Container.Value %></asp:LinkButton></div>";
            //    //e.Row.Cells.FromKey(GRID_COL_KEY_ASSETNAME).Text = "<div><a href='' OnClick='lbTitle_Click' runat=server CssClass=lnk>" + e.Row.Cells.FromKey(GRID_COL_KEY_ASSETNAME).Text + "</a><div>";
            //}


            //if (e.Row.Band.Index == GRID_BAND_LEVEL1) // Allow Selection only at the first band level
            //{
            //    if (this.AllowMultiSelect)
            //    {

            //        string strID = e.Row.Cells.FromKey(GRID_COL_KEY_ASSETID).Text.Replace("'", "~~~");

            //        TemplatedColumn tempCol = (TemplatedColumn)e.Row.Cells.FromKey(GRID_COL_KEY_SELECT).Column;
            //        CellItem cellItem = (CellItem)tempCol.CellItems[e.Row.Index];
            //        CheckBox cbSelect = (CheckBox)cellItem.FindControl(GRID_ROW_CHECKBOX);

            //        if (this.CurrentPageSelections != null)
            //        {

            //            String csvPageSelections = this.CurrentPageSelections;

            //            String[] arrPageSelections = csvPageSelections.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            //            foreach (String s in arrPageSelections)
            //            {
            //                if (s.Equals(strID))
            //                {
            //                    hdnPageSelections.Value += s + ",";
            //                    cbSelect.Checked = true;
            //                    break;
            //                }
            //            }

            //            // Remove from the Selections Passed so that the list does not contain duplicates
            //            if (this.SelectedList != null)
            //            {
            //                String csvInitSelections = "";
            //                foreach (String s1 in this.SelectedList)
            //                {
            //                    if (!s1.Equals(strID)) csvInitSelections += s1 + ",";
            //                }
            //                this.SelectedList = csvInitSelections.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //            }
            //        }

            //    }



            //bool bIsParent = Convert.ToBoolean(e.Row.Cells.FromKey(GRID_COL_KEY_ISPARENT).Value);

            //if (bIsParent)
            //{

            //    e.Row.Cells.FromKey(GRID_COL_KEY_ISPARENTICON).Style.CustomRules = "Background-repeat:no-repeat";
            //    e.Row.Cells.FromKey(GRID_COL_KEY_ISPARENTICON).Style.BackgroundImage = ISPARENT_IMAGE;

            //}
            //else if (e.Row.Cells.FromKey(GRID_COL_KEY_PARENTASSETID).Value != null)
            //{

            //    e.Row.Cells.FromKey(GRID_COL_KEY_ISPARENTICON).Style.CustomRules = "Background-repeat:no-repeat";
            //    e.Row.Cells.FromKey(GRID_COL_KEY_ISPARENTICON).Style.BackgroundImage = ISCHILD_IMAGE;

            //}

            //}
            #endregion

            if (e.Row.Items[18].Column.Key == GRID_COL_KEY_RFIDTAGID && !string.IsNullOrEmpty(e.Row.Items[18].Text))
            {
                //e.Row.Items[17]. Style.CustomRules = "Background-repeat:no-repeat";
                //e.Row.Items[17].Style.BackgroundImage = RFID_IMAGE;
                //e.Row.Items[17].Title = e.Row.Items[18].Text;
                e.Row.Items[17].Text = "<img src='" + RFID_IMAGE + "' title='" + e.Row.Items[18].Text + "' >";
            }
            //Start Position
            if (e.Row.Items[2].Column.Key == GRID_COL_KEY_STARTPOS)
            {
                if (!string.IsNullOrEmpty(e.Row.Items[2].Text.Trim()))
                {
                    if (e.Row.Items[2].Text.CompareTo("0") == 0)
                        e.Row.Items[2].Text = "";
                }
                else
                {
                    e.Row.Items[2].Text = "";
                }
            }
            //No Of RUs
            if (e.Row.Items[3].Column.Key == GRID_COL_KEY_NOOFRUS)
            {
                if (!string.IsNullOrEmpty(e.Row.Items[3].Text.Trim()))
                {
                    if (e.Row.Items[3].Text.CompareTo("0") == 0)
                        e.Row.Items[3].Text = "";
                }
                else
                {
                    e.Row.Items[3].Text = "";
                }
            }

        }
        catch { }

    }

    //protected void ibAssignRFID_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    //{
    //    // FillWorkerDetails(0);
    //    PageNum = 1;
    //    FillAsset_Details();

    //}

    #endregion Page Events

    protected void eExporter_CellExported(object sender, Infragistics.Web.UI.GridControls.ExcelCellExportedEventArgs e)
    {
        int iRdex = e.CurrentRowIndex;
        int iCdex = e.CurrentColumnIndex;
        e.Worksheet.Columns[17].Width = 1;
        e.Worksheet.Columns[3].Width = 25 * 256;//Serial No
        e.Worksheet.Columns[4].Width = 25 * 256; //manufacturer

        if (e.CurrentOutlineLevel == 0)
        {
            if (iRdex != 0)
            {

                if (iCdex == 3 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null) //Serial No
                {
                    string str = e.Worksheet.Rows[iRdex].Cells[iCdex].Value.ToString();
                    char[] sep = { '<', '>' };
                    Array a = str.Split(sep);
                    if (a.Length > 1)
                    {
                        e.Worksheet.Rows[iRdex].Cells[iCdex].Value = a.GetValue(2).ToString().Trim();
                        //e.Worksheet.Columns[1].Width = 3000;
                        //e.Worksheet.Columns[3].Width = a.GetValue(4).ToString().Length;
                        //e.CurrentWorksheet.Columns[3].Hidden = true;
                        //e.Worksheet.Columns[3].Width = a.GetValue(2).ToString().Length;
                        //e.CurrentWorksheet.Columns["RefNo"].Hidden = false;
                        //e.CurrentWorksheet.Columns[3].Hidden = false;

                    }
                }
                //18- RFID Tag Icon column
                //if (iCdex == 18 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
                //{
                //    string str = e.Worksheet.Rows[iRdex].Cells[iCdex].Value.ToString();
                //    char[] sep = { '<', '>' };
                //    Array a = str.Split(sep);
                //    if (a.Length > 2)
                //        e.Worksheet.Rows[iRdex].Cells[iCdex].Value = a.GetValue(4);
                //}

            }
        }
        else
        {
            if (!e.IsHeaderCell && !e.IsFooterCell && !e.IsSummaryCell)
            {

                if (iCdex == 4 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null) //Serial No for Child List
                {
                    string str = e.Worksheet.Rows[iRdex].Cells[iCdex].Value.ToString();
                    char[] sep = { '<', '>' };
                    Array a = str.Split(sep);
                    if (a.Length > 1)
                    {
                        e.Worksheet.Rows[iRdex].Cells[iCdex].Value = a.GetValue(2).ToString().Trim();
                    }
                }
            }
        }

    }


    protected void ddlMfg_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMfg != null && ddlMfg.Items.Count > 0)
        {
            ddlMfgName = ddlMfg.SelectedItem.Text;
            ddlMfgID = ddlMfg.SelectedItem.Value;
        }

        txtModel.Text = "(All)";
        hdnModelID.Value = "0";
        hdnModelName.Value = "(All)";
        //V000013
        //Added on 24-08-2012
        //ibExport.Visible = true;
        //ibExport.Enabled = true;
        //above line commented by kjb on 28 Feb 2013
    }

    protected void uwgAssetSearch_DataBound(object sender, EventArgs e)
    {
        //if (!this.IsPostBack)
        //{
        //    pagerControl.SetupPageList(this.uwgAssetSearch.Behaviors.Paging.PageCount);
        //    pagerControl.SetCurrentPageNumber(this.uwgAssetSearch.Behaviors.Paging.PageIndex);
        //}

        //Control postbackControlInstance = null;
        //string postbackControlName = this.Request.Params.Get("__EVENTTARGET");
        //if (postbackControlName != null && postbackControlName != string.Empty)
        //    postbackControlInstance = this.FindControl(postbackControlName);
        //if (postbackControlInstance != null && postbackControlInstance.ID == "PagerPageList")
        //{
        //    //do nothing
        //    //pagerControl.SetupPageList(this.grdSPCModel.Behaviors.Paging.PageCount);
        //}
        //else
        //{
        //    pagerControl.SetupPageList(this.uwgAssetSearch.Behaviors.Paging.PageCount);
        //    pagerControl.SetCurrentPageNumber(this.uwgAssetSearch.Behaviors.Paging.PageIndex);

        //}
    }

    protected void btnModifyAll_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        if (hdnAssetId != null)
        {
            Session["AssetIDs"] = hdnAssetId.Value;
            ibExport.Visible = true;
            ibExport.Enabled = true;
            chkShowChild.Visible = true;
            chkShowChild.Enabled = true;
            Response.Redirect("ModifyAssetAll.aspx");
        }

    }
    public void ExportToExcel(DataSet ds)
    {
        Infragistics.Documents.Excel.Workbook workbook = new Infragistics.Documents.Excel.Workbook();

        Infragistics.Documents.Excel.Worksheet worksheet = workbook.Worksheets.Add("Sheet1");
        foreach (DataTable table in ds.Tables)
        {

            // Infragistics.Documents.Excel.Worksheet worksheet = new Infragistics.Documents.Excel.Worksheet(1);


            for (int columnIndex = 0; columnIndex < table.Columns.Count; columnIndex++)
            {
                worksheet.Rows[0].Cells[columnIndex].Value = table.Columns[columnIndex].ColumnName;
                // worksheet.Rows[0].CellFormat.FormatString = "@";
            }


            int rowIndex = 1;

            foreach (DataRow dataRow in table.Rows)
            {
                Infragistics.Documents.Excel.WorksheetRow row = worksheet.Rows[rowIndex++];

                for (int columnIndex = 0; columnIndex < dataRow.ItemArray.Length; columnIndex++)
                {
                    row.Cells[columnIndex].Value = dataRow.ItemArray[columnIndex];
                    // row.Cells[columnIndex].CellFormat.FormatString = "@";

                }
            }



        }

        string filename = "Exceldata.xls";
        workbook.Save(Server.MapPath("AssetData/Data.xls"));
        string Path = Server.MapPath("AssetData/Data.xls");

        FileInfo OutFile = new FileInfo(Path);
        Response.Clear();

        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "inline; filename=\"" + filename + "\"");

        Response.TransmitFile(Path, 0, OutFile.Length);
        Response.Flush();
        Response.End();



    }

}


public class SelectionChangedEventArgs : EventArgs
{
    public SelectionChangedEventArgs() : base() { }
    public SelectionChangedEventArgs(int pDocId, string pDocNum, string pTitle) : base() { AssetID = pDocId; RefNumber = pDocNum; Title = pTitle; }
    public int AssetID;
    public string RefNumber;
    public string Title;
}

