using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Infragistics.Web.UI.NavigationControls;
using System.Configuration;
using Microsoft.Reporting.WebForms;
using iAssetTrack.BAL;
using System.Collections;
using iAssetTrackBAL;
using System.Web.Services;
using Infragistics.Web.UI.GridControls;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

public partial class ApplicationMap : System.Web.UI.Page
{
    #region "Declarations"
    DataTable _dtRights;
    protected int totalRecordCount = 0;
    private iAssetTrack.BAL.BusinessUnitBAL objBU;
    private iAssetTrack.BAL.SitesBAL objSite;
    private iAssetTrack.BAL.LocationBAL objLocation;
    private iAssetTrack.BAL.AssetCreationBAL objAsset;
    //private iAssetTrack.BAL.CommonBAL objCommon;
    //private iAssetTrack.BAL.LocationTypeBAL objLocationType;
    //DataTable _dtRights;
    private const string JAVA_SCRIPT = "javascript:";

    private const string PROP_CURRENTGRIDPAGENUMBER = "AssetSearchPageNum";
    //private const string PROP_ISSUEDTO = "IssuedTo";
    private const string PROP_ALLOWMULTISELECT = "MultiSelect";
    private const string PROP_ALLOWEXPORTTOEXCEL = "AllowExport";
    //private const string PROP_ALLOWUSERSELECTION = "AllowUserSelection";
    //private const string PROP_ASSOCIATEDASSETS = "AssociatedAssets";
    //private const string PROP_PARENTASSETS = "ParentAssets";
    private const string PROP_PAGECOUNT = "PageCount";
    private const string PROP_AUTODISPLAYFIRSTPAGE = "AutoDisplayFirstPage";
    //private const string PROP_PARENTASSETID = "ParentAssetID";
    private const string PROP_SELECTEDLIST = "SelectedList";

    //private const string PROP_ORIGSELECTEDLIST = "OriginalSelectedList";
    //private const string PROP_SEARCHBARSTATE = "SearchBarState";
    //added by kjb on 11 Aug 2011
    private const string PROP_APP_PAGENUMBER = "AppPageNum";
    private const string PROP_APP_PAGECOUNT = "AppPageCount";
    private const string PROP_LOCATIONIDS = "LocationIDs";
    private const string PROP_SELECTEDLIST_APP = "SelectedAppList";
    private const string GRID_COL_KEY_APPID = "ApplID";
    private const string PROP_ASSET_GRID_CHECKED_ROW_COUNT = "CheckedRowCount";
    //

    //private const string PROP_ALLOWBUSELECTION = "AllowBUSelection";


    //private const string JS_PARAM_ASSETID = "<AssetID>";
    //private const string JS_PARAM_REFNO = "<RefNumber>";
    //private const string JS_PARAM_ASSETNAME = "<AssetName>";
    //private const string JS_PARAM_PARENTID = "<ParentAssetID>";
    //private const string JS_PARAM_LEVEL = "<Level>";


    private const string GRID_COL_KEY_ASSETID = "AssetID";
    //private const string GRID_COL_KEY_REFNO = "RefNumber";
    //private const string GRID_COL_KEY_ASSETNAME = "AssetName";
    private const string GRID_COL_KEY_SELECT = "SelectCheckBox";
    private const string GRID_ROW_CHECKBOX = "ChkSelect";
    private const string GRID_ROW_CHECKBOX_APP = "ChkApp";
    //private const string GRID_COL_KEY_ISPARENT = "IsParent";
    //private const string GRID_COL_KEY_PARENTASSETID = "ParentAssetID";
    //private const string GRID_COL_KEY_ISPARENTICON = "IsParentIcon";
    //private const string GRID_COL_KEY_RFIDTAGID = "RFIDTagID";
    //private const string GRID_COL_KEY_RFIDTAGICON = "RFIDTagIcon";

    private const int GRID_BAND_LEVEL1 = 0;
    private const int GRID_BAND_LEVEL2 = 1;

    private const string ISPARENT_IMAGE = "infragistics/images/Outlook2003/folders.gif";
    private const string ISCHILD_IMAGE = "infragistics/images/Outlook2003/folder.gif";
    private const string RFID_IMAGE = "images/label_16x16.gif";

    private const string VIEWSTATE_PAGE_PFX = "PAGE_";
    private const string VIEWSTATE_PAGE_APP_PFX = "APP_PAGE_";
    //private const string VIEWSTATE_INIT_SELECTIONS = "INIT_SELECTIONS";

    private iAssetTrack_WebDataGrid_Paging_CustomerPagerControl pagerControl;
    //public delegate void SelectionDelegate(object sender, SelectionChangedEventArgs e);
    //public event SelectionDelegate OnSelection;
    private string tenantAssignedLocations = string.Empty;
    #endregion

    #region Web Control Properties




    public int CheckRowCount
    {
        get
        {
            return (ViewState[PROP_ASSET_GRID_CHECKED_ROW_COUNT] != null ? Convert.ToInt32(ViewState[PROP_ASSET_GRID_CHECKED_ROW_COUNT]) : 0);
        }
        set
        {
            ViewState[PROP_ASSET_GRID_CHECKED_ROW_COUNT] = value;
        }
    }

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
    public int PageNumApp
    {
        get
        {
            return (ViewState[PROP_APP_PAGENUMBER] != null ? Convert.ToInt32(ViewState[PROP_APP_PAGENUMBER]) : 1);
        }
        set
        {
            ViewState[PROP_APP_PAGENUMBER] = value;
            //if (value > this.PageCount) this.PageCount = value;
        }
    }
    private int PageCountApp
    {
        get { return (ViewState[PROP_APP_PAGECOUNT] != null ? Convert.ToInt32(ViewState[PROP_APP_PAGECOUNT]) : 0); }
        set { ViewState[PROP_APP_PAGECOUNT] = value; }

    }

    public string LocationIDs
    {
        get { return (ViewState[PROP_LOCATIONIDS] != null ? Convert.ToString(ViewState[PROP_LOCATIONIDS]) : ""); }
        set { ViewState[PROP_LOCATIONIDS] = value; }
    }

    //public int BusinessUnitID
    //{
    //    get
    //    {
    //        return (ViewState[PROP_BUSINESSUNITID] != null ? Convert.ToInt32(ViewState[PROP_BUSINESSUNITID]) : 0);
    //    }
    //    set { ViewState[PROP_BUSINESSUNITID] = value; }
    //}

    //public int SiteID
    //{
    //    get { return (ViewState[PROP_SITEID] != null ? Convert.ToInt32(ViewState[PROP_SITEID]) : 0); }
    //    set { ViewState[PROP_SITEID] = value; }
    //}



    //public int LocationID
    //{
    //    get { return (ViewState[PROP_LOCATIONID] != null ? Convert.ToInt32(ViewState[PROP_LOCATIONID]) : 0); }
    //    set { ViewState[PROP_LOCATIONID] = value; }
    //}


    ////Do not set this value in the html control. Always set it in code, since it references the hdnUserID control
    //public int UserID
    //{
    //    get { return (ViewState[PROP_USERID] != null ? Convert.ToInt32(ViewState[PROP_USERID]) : 0); }
    //    set { ViewState[PROP_USERID] = value; this.hdnUserID.Value = ViewState[PROP_USERID].ToString(); }
    //}


    //public string AssetName
    //{
    //    get { return (ViewState[PROP_ASSETNAME] != null ? Convert.ToString(ViewState[PROP_ASSETNAME]) : ""); }
    //    set { ViewState[PROP_ASSETNAME] = value; }
    //}


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



    //public bool AllowUserSelection
    //{
    //    get { return (ViewState[PROP_ALLOWUSERSELECTION] != null ? Convert.ToBoolean(ViewState[PROP_ALLOWUSERSELECTION]) : false); }
    //    set { ViewState[PROP_ALLOWUSERSELECTION] = value; }
    //}


    public bool AutoDisplayFirstPage
    {
        get { return (ViewState[PROP_AUTODISPLAYFIRSTPAGE] != null ? Convert.ToBoolean(ViewState[PROP_AUTODISPLAYFIRSTPAGE]) : true); }
        set { ViewState[PROP_AUTODISPLAYFIRSTPAGE] = value; }
    }

    //protected bool m_ClientSideEventEnabled = false;
    ////public string ClientSideOnSelection
    ////{
    ////    get { return (ViewState[PROP_CLIENTSIDEONSELECTION] != null ? Convert.ToString(ViewState[PROP_CLIENTSIDEONSELECTION]) : ""); }
    ////    set { ViewState[PROP_CLIENTSIDEONSELECTION] = value; m_ClientSideEventEnabled = true; }
    ////}

    //public SearchBarStateValues SearchBarState
    //{
    //    get { return (ViewState[PROP_SEARCHBARSTATE] != null ? (SearchBarStateValues)ViewState[PROP_SEARCHBARSTATE] : SearchBarStateValues.Expanded); }
    //    set { ViewState[PROP_SEARCHBARSTATE] = value; }
    //}


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

    // For App List -- added by kjb on 12th Aug 2011
    private string CurrentPageSelectionKeyApp
    {
        get { return VIEWSTATE_PAGE_APP_PFX + Convert.ToString(this.PageNumApp).Trim(); }
    }
    private string CurrentPageSelectionsApp
    {
        get { return (string)ViewState[CurrentPageSelectionKeyApp]; }
        set { ViewState[CurrentPageSelectionKeyApp] = value; }
    }
    private string[] SelectedListApp
    {
        get { return ((string[])ViewState[PROP_SELECTEDLIST_APP]); }
        set { ViewState[PROP_SELECTEDLIST_APP] = value; }
    }
    // --end

    //private string OriginalSelectedList
    //{
    //    get { return (ViewState[PROP_ORIGSELECTEDLIST] != null ? (string)ViewState[PROP_ORIGSELECTEDLIST] : ""); }
    //    set { ViewState[PROP_ORIGSELECTEDLIST] = value; }

    //}





    #endregion

    [WebMethod]
    public static string[] getHostNames(string prefixText, int count)
    {
        string[] arrHost;
        ArrayList arrList = new ArrayList();

        HostBAL objHost = new HostBAL();
        DataSet dsHost = objHost.retrieve();
        foreach (DataRow dr in dsHost.Tables[0].Rows)
        {
            if (dr[1].ToString().ToLower().StartsWith(prefixText.ToLower()))
                arrList.Add(dr[1].ToString());
        }
        arrHost = (string[])arrList.ToArray(typeof(string));
        //int index = 0;
        //foreach (string str in arrList)
        //{
        //    arrHost[index] = str;
        //}
        //arrHost = (string[])arrList.ToArray();
        //arrHost = (string[])(from crHost in dsHost.Tables[0].AsEnumerable()
        //            where crHost["HostName"].ToString().StartsWith(prefixText)
        //            select crHost["HostName"]).ToArray();
        return arrHost;

    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        uwgApps.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(uwgApps_ItemCommand);
        pagerControl = uwgApps.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);
    }
    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.uwgApps.Behaviors.Paging.PageIndex = e.PageNumber;
        FillApps_Details();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageHeader"] = "Application Map";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];


        _dtRights = (DataTable)(Session["Rights"]);

        bool blfoundPage = false;

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "ApplicationMap.aspx";
            Response.Redirect("Login.aspx");
        }

        if (_dtRights.Select("Module = 'Application Map' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }

        if (_dtRights.Select("Module = 'Application Map' and Rights = '" + "Map" + "'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }

        if (_dtRights.Select("Module = 'Application Map' and Rights = '" + "Unmap" + "'").Length != 0)
        {
            ibDelete.Enabled = true;
        }
        else
        {
            ibDelete.Enabled = false;
        }

        if (bool.Parse(Session["TenantUser"].ToString()))
        {
            UserBAL objUser = new UserBAL();
            objUser.UserID = Convert.ToInt32(Session["UserID"]);
            DataSet dsTenant = objUser.retrieveTenantDetails();
            if (dsTenant.Tables.Count > 0 && dsTenant.Tables[0].Rows.Count > 0)
            {
                tenantAssignedLocations = dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_ASSIGNEDLOCATIONS].ToString();
            }
        }

        if (!IsPostBack)
        {
            LoadLocations();

            // allow multi selection in Asset List
            this.AllowMultiSelect = true;
            uwgAsset.Visible = false;
            // Initial load, hide paging buttons of Asset list.
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

            ibCreate.Visible = false;
            ibDelete.Visible = false;
            ibReset.Visible = false;
        }

        FillApps_Details();
        //FillAsset_Details();
    }

    private void LoadLocations()
    {

        objBU = new iAssetTrack.BAL.BusinessUnitBAL();
        DataSet dsBU = objBU.retrieve();
        DataTable dtBU = dsBU.Tables[0];
        objSite = new iAssetTrack.BAL.SitesBAL();
        //DataSet dsSite = objSite.retrieve();
        //DataTable dtSite = dsSite.Tables[0];
        objLocation = new iAssetTrack.BAL.LocationBAL();
        //DataSet dsLocation = objLocation.retrieve();
        //DataTable dtLocation = dsLocation.Tables[0];

        WebDataTree loctree = (WebDataTree)this.TreeLocation;
        loctree.Nodes.Clear();
        // add BUs
        DataTreeNode clearNode = new DataTreeNode();
        using (DataTableReader dtBURdr = dtBU.CreateDataReader())
        {
            while (dtBURdr.Read())
            {

                DataTreeNode buNode = new DataTreeNode();
                buNode.Text = dtBURdr.GetValue(1).ToString();
                buNode.Value = dtBURdr.GetValue(0).ToString();
                //buNode.Enabled = false;
                //buNode.CssClass = ".TreeNodeDisabled";
                loctree.Nodes.Add(buNode);
                DataSet dsSiteByLocation = objSite.retrieveByBusinessUnitId(Convert.ToInt32(dtBURdr.GetValue(0)));
                DataTable dtSiteByLocation = dsSiteByLocation.Tables[0];
                using (DataTableReader dtsitebyBURdr = dtSiteByLocation.CreateDataReader())
                {
                    while (dtsitebyBURdr.Read())
                    {
                        DataTreeNode siteNode = new DataTreeNode();
                        siteNode.Text = dtsitebyBURdr.GetValue(1).ToString();
                        siteNode.Value = dtsitebyBURdr.GetValue(0).ToString();
                        //siteNode.Enabled = false;
                        //siteNode.CssClass = ".TreeNodeDisabled";
                        buNode.Nodes.Add(siteNode);

                        objLocation.SiteID = Convert.ToInt32(dtsitebyBURdr.GetValue(0));
                        DataSet dsLocationbySite = objLocation.GetLocationBYSite();
                        DataTable dtLocationbySite = dsLocationbySite.Tables[0];

                        using (DataTableReader dtLocationbySiteRdr = dtLocationbySite.CreateDataReader())
                        {
                            while (dtLocationbySiteRdr.Read())
                            {
                                if (bool.Parse(Session["TenantUser"].ToString()))
                                {
                                    if (objLocation.IsATenantLocation(int.Parse(dtLocationbySiteRdr.GetValue(0).ToString()), Convert.ToInt32(Session["UserID"])) == 1)
                                    {
                                        DataTreeNode LocationNode = new DataTreeNode();
                                        LocationNode.Text = dtLocationbySiteRdr.GetValue(1).ToString();
                                        LocationNode.Value = dtLocationbySiteRdr.GetValue(0).ToString();
                                        if (!tenantAssignedLocations.Contains(dtLocationbySiteRdr.GetValue(0).ToString()))
                                        {
                                            //tenant assigned list not contains this location 
                                            //node will be disabled and expanded
                                            LocationNode.Expanded = true;
                                            LocationNode.Enabled = false;

                                        }
                                        siteNode.Nodes.Add(LocationNode);
                                        if (int.Parse(dtLocationbySiteRdr.GetValue(2).ToString()) > 0) //Child location count
                                            getChildLocations(LocationNode, Convert.ToInt32(dtLocationbySiteRdr.GetValue(0).ToString()));
                                    }
                                }
                                else
                                {
                                    DataTreeNode LocationNode = new DataTreeNode();
                                    LocationNode.Text = dtLocationbySiteRdr.GetValue(1).ToString();
                                    LocationNode.Value = dtLocationbySiteRdr.GetValue(0).ToString();
                                    siteNode.Nodes.Add(LocationNode);
                                    if (int.Parse(dtLocationbySiteRdr.GetValue(2).ToString()) > 0) //Child location count
                                        getChildLocations(LocationNode, Convert.ToInt32(dtLocationbySiteRdr.GetValue(0).ToString()));
                                }

                            }
                        }
                    }
                }
            }

        }




    }

    private void getChildLocations(DataTreeNode node, int LocationID)
    {
        DataSet dsLocationbyLocation = objLocation.GetLocationBYLocation(LocationID);
        DataTable dtLocationbyLocation = dsLocationbyLocation.Tables[0];

        using (DataTableReader dtLocationbyLocationRdr = dtLocationbyLocation.CreateDataReader())
        {
            while (dtLocationbyLocationRdr.Read())
            {
                if (bool.Parse(Session["TenantUser"].ToString()))
                {
                    if (objLocation.IsATenantLocation(int.Parse(dtLocationbyLocationRdr.GetValue(0).ToString()), Convert.ToInt32(Session["UserID"])) == 1)
                    {
                        DataTreeNode subLocationNode = new DataTreeNode();
                        subLocationNode.Text = dtLocationbyLocationRdr.GetValue(1).ToString();
                        subLocationNode.Value = dtLocationbyLocationRdr.GetValue(0).ToString();
                        if (!tenantAssignedLocations.Contains(dtLocationbyLocationRdr.GetValue(0).ToString()))
                        {
                            //tenant assigned list not contains this location 
                            //node will be disabled and expanded
                            subLocationNode.Expanded = true;
                            subLocationNode.CssClass = ".TreeNodeDisabled";
                            subLocationNode.Enabled = false;

                        }
                        node.Nodes.Add(subLocationNode);
                        if (int.Parse(dtLocationbyLocationRdr.GetValue(5).ToString()) > 0) //Child location count
                            getChildLocations(subLocationNode, Convert.ToInt32(dtLocationbyLocationRdr.GetValue(0).ToString()));
                    }

                }
                else
                {
                    DataTreeNode subLocationNode = new DataTreeNode();
                    subLocationNode.Text = dtLocationbyLocationRdr.GetValue(1).ToString();
                    subLocationNode.Value = dtLocationbyLocationRdr.GetValue(0).ToString();
                    node.Nodes.Add(subLocationNode);
                    if (int.Parse(dtLocationbyLocationRdr.GetValue(5).ToString()) > 0) //Child location count
                        getChildLocations(subLocationNode, Convert.ToInt32(dtLocationbyLocationRdr.GetValue(0).ToString()));
                }
            }
        }
    }

    //private void LoadLocations()
    //{

    //    objBU = new iAssetTrack.BAL.BusinessUnitBAL();
    //    DataSet dsBU = objBU.retrieve();
    //    DataTable dtBU = dsBU.Tables[0];
    //    objSite = new iAssetTrack.BAL.SitesBAL();
    //    //DataSet dsSite = objSite.retrieve();
    //    //DataTable dtSite = dsSite.Tables[0];
    //    objLocation = new iAssetTrack.BAL.LocationBAL();
    //    //DataSet dsLocation = objLocation.retrieve();
    //    //DataTable dtLocation = dsLocation.Tables[0];

    //    WebDataTree loctree = (WebDataTree)this.TreeLocation;
    //    loctree.Nodes.Clear();
    //    // add BUs
    //    DataTreeNode clearNode = new DataTreeNode();
    //    using (DataTableReader dtBURdr = dtBU.CreateDataReader())
    //    {
    //        while (dtBURdr.Read())
    //        {

    //            DataTreeNode buNode = new DataTreeNode();
    //            buNode.Text = dtBURdr.GetValue(1).ToString();
    //            buNode.Value = dtBURdr.GetValue(0).ToString();
    //            buNode.Enabled = false;
    //            buNode.Expanded = true;
    //            buNode.CssClass = ".TreeNodeDisabled";
    //            loctree.Nodes.Add(buNode);
    //            DataSet dsSiteByLocation = objSite.retrieveByBusinessUnitId(Convert.ToInt32(dtBURdr.GetValue(0)));
    //            DataTable dtSiteByLocation = dsSiteByLocation.Tables[0];
    //            using (DataTableReader dtsitebyBURdr = dtSiteByLocation.CreateDataReader())
    //            {
    //                while (dtsitebyBURdr.Read())
    //                {
    //                    DataTreeNode siteNode = new DataTreeNode();
    //                    siteNode.Text = dtsitebyBURdr.GetValue(1).ToString();
    //                    siteNode.Value = dtsitebyBURdr.GetValue(0).ToString();
    //                    siteNode.Enabled = false;
    //                    siteNode.Expanded = true;
    //                    siteNode.CssClass = ".TreeNodeDisabled";
    //                    buNode.Nodes.Add(siteNode);

    //                    objLocation.SiteID = Convert.ToInt32(dtsitebyBURdr.GetValue(0));
    //                    DataSet dsLocationbySite = objLocation.GetLocationBYSite();
    //                    DataTable dtLocationbySite = dsLocationbySite.Tables[0];

    //                    using (DataTableReader dtLocationbySiteRdr = dtLocationbySite.CreateDataReader())
    //                    {
    //                        while (dtLocationbySiteRdr.Read())
    //                        {
    //                            DataTreeNode LocationNode = new DataTreeNode();
    //                            LocationNode.Text = dtLocationbySiteRdr.GetValue(1).ToString();
    //                            LocationNode.Value = dtLocationbySiteRdr.GetValue(0).ToString();
    //                            siteNode.Nodes.Add(LocationNode);
    //                            if (int.Parse(dtLocationbySiteRdr.GetValue(2).ToString()) > 0) //Child location count
    //                                getChildLocations(LocationNode, Convert.ToInt32(dtLocationbySiteRdr.GetValue(0).ToString()));

    //                        }
    //                    }
    //                }
    //            }
    //        }

    //    }




    //}

    //private void getChildLocations(DataTreeNode node, int LocationID)
    //{
    //    DataSet dsLocationbyLocation = objLocation.GetLocationBYLocation(LocationID);
    //    DataTable dtLocationbyLocation = dsLocationbyLocation.Tables[0];

    //    using (DataTableReader dtLocationbyLocationRdr = dtLocationbyLocation.CreateDataReader())
    //    {
    //        while (dtLocationbyLocationRdr.Read())
    //        {
    //            DataTreeNode subLocationNode = new DataTreeNode();
    //            subLocationNode.Text = dtLocationbyLocationRdr.GetValue(1).ToString();
    //            subLocationNode.Value = dtLocationbyLocationRdr.GetValue(0).ToString();
    //            node.Nodes.Add(subLocationNode);
    //            if (int.Parse(dtLocationbyLocationRdr.GetValue(5).ToString()) > 0) //Child location count
    //                getChildLocations(subLocationNode, Convert.ToInt32(dtLocationbyLocationRdr.GetValue(0).ToString()));
    //        }
    //    }
    //}


    protected void btnGetAssets_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        LocationIDs = string.Empty;
        if (!string.IsNullOrEmpty(txtHost.Text.Trim()))
        {
            HostBAL obHost = new HostBAL();
            DataSet dsLocByHost = obHost.retrieveLocIDFromHostID(txtHost.Text.Trim());
            foreach (DataRow dr in dsLocByHost.Tables[0].Rows)
            {
                LocationIDs = LocationIDs + dr[0].ToString() + ",";
            }
        }

        foreach (DataTreeNode node in TreeLocation.CheckedNodes)
        {
            if (node.Level >= 2)
            {
                LocationIDs = LocationIDs + node.Value + ",";
            }
        }
        this.PageNum = 1;
        PageNum = this.PageNum;
        FillAsset_Details();
    }

    protected void ibDelete_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        //int _AssetID;
        string _AssignmentID;
        // check whether any row selected in Asset List
        // if so than only do the delete map for that Asset only.
        //this.CurrentPageSelections = hdnPageSelections.Value;
        //if (!string.IsNullOrEmpty(CurrentPageSelections))
        //{
        //    _AssetID = int.Parse(CurrentPageSelections);

        //}
        try
        {
            lblMsg.Text = "";
            lblMsg.Visible = false;
            if (!string.IsNullOrEmpty(hdnSelectedAssetID.Value))
            {
                //_AssetID = int.Parse(hdnSelectedAssetID.Value.ToString());
                _AssignmentID = hdnSelectedAssetID.Value.ToString();
                ApplicationMapBAL objMap = new ApplicationMapBAL();
                objMap.ID = _AssignmentID.ToString(); //represents AssignmentID
                if (hdnSelectedChildIDs != null && !string.IsNullOrEmpty(hdnSelectedChildIDs.Value.Trim(',')))
                {
                    objMap.ApplIDs = hdnSelectedChildIDs.Value.Trim(',');
                }
                else
                {
                    objMap.ApplIDs = "";
                }
                objMap.Persist(iAssetTrack.DALC.DALCOperation.Delete);

                hdnSelectedChildIDs.Value = "";
                hdnSelectedAssetID.Value = "";


                this.CurrentPageSelections = hdnPageSelections.Value;

                this.CurrentPageSelectionsApp = hdnPageSelectionsApp.Value;

                FillAsset_Details();
                lblMsg.Text = "Un-Mapped successfully";
                lblMsg.Visible = true;

            }
            else
            {
                FillAsset_Details();
                lblMsg.Text = "Select only one Asset for un-map";
                lblMsg.Visible = true;
            }

        }
        catch (Exception ex)
        {
            FillAsset_Details();
            ExceptionPolicy.HandleException(ex, "errDCTrack");
            lblMsg.Text = "Un-Map Operation Failed, Check error log for more information";
            lblMsg.Visible = true;
        }

    }



    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        try
        {
            lblMsg.Text = "";
            lblMsg.Visible = false;
            //Traverse thru Asset list and Application list 
            //and select all checked ones to od othe Asset-Application map
            this.CurrentPageSelections = hdnPageSelections.Value.Trim(',');
            string[] _AssetList = this.CurrentPageSelections.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            this.CurrentPageSelectionsApp = hdnPageSelectionsApp.Value.Trim(',');
            string[] _AppList = this.CurrentPageSelectionsApp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //if Asset(s) and App(s) selected for mapping
            if (_AssetList != null && _AssetList.Length > 0 && _AppList != null && _AppList.Length > 0)
            {
                foreach (string AssetID in _AssetList)
                {
                    AssetHostAssignmentBAL obAHA = new AssetHostAssignmentBAL();
                    ApplicationMapBAL objMap = new ApplicationMapBAL();
                    //int _assetID = int.Parse(AssetID.Split('#')[0]);
                    //int _hostID = int.Parse(AssetID.Split('#')[1]);
                    //objMap.ID = obAHA.retrieveByAssetandHostID(_assetID, _hostID).Tables[0].Rows[0][0].ToString();
                    objMap.ID = AssetID; // AssigmentID
                    objMap.ApplIDs = string.Join("|", _AppList);
                    objMap.Delimiters = "|";
                    objMap.CreatedBy = Convert.ToInt32(Session["UserID"]);

                    objMap.Persist(iAssetTrack.DALC.DALCOperation.Insert);
                }

                hdnPageSelections.Value = "";
                hdnPageSelectionsApp.Value = "";
                this.CurrentPageSelections = "";
                this.CurrentPageSelectionsApp = "";

                FillAsset_Details();
                FillApps_Details();
                lblMsg.Text = "Mapped successfully";
                lblMsg.Visible = true;
                txtHost.Text = "";
                txtHost.Focus();
            }
            else
            {
                FillAsset_Details();
                FillApps_Details();
                lblMsg.Text = "Select Asset(s) and Application(s) for map";
                lblMsg.Visible = true;
            }


        }
        catch (Exception ex)
        {
            FillAsset_Details();
            FillApps_Details();
            ExceptionPolicy.HandleException(ex, "errDCTrack");
            lblMsg.Text = "Map Operation failed, check error log for more information";
            lblMsg.Visible = true;
        }
    }


    protected void ibReset_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        lblMsg.Text = "";
        lblMsg.Visible = false;

        hdnPageSelections.Value = string.Empty;
        hdnPageSelectionsApp.Value = string.Empty;
        hdnSelectedAssetID.Value = string.Empty;
        hdnSelectedChildIDs.Value = string.Empty;
        hdnSeletedRowID.Value = string.Empty;

        FillApps_Details();
        FillAsset_Details();
    }

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
        FillAsset_Details();
        if (this.AllowMultiSelect)
        {
            if (this.CurrentPageSelections == null && this.SelectedList != null) this.CurrentPageSelections = String.Join(",", this.SelectedList);
            hdnPageSelections.Value = "";
        }
    }

    private void FillApps_Details()
    {
        //int iPageIndex = (this.PageNumApp <= 0 ? 1 : this.PageNumApp);
        //int pagesize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());

        ApplicationBAL objApp = new ApplicationBAL();
        //DataSet dsApp = objApp.retrieveByPage(iPageIndex, pagesize);
        DataSet dsApp = objApp.retrieve();
        totalRecordCount = dsApp.Tables[0].Rows.Count;
        uwgApps.DataSource = dsApp.Tables[0];
        uwgApps.DataBind();

        if (uwgApps.Rows.Count == 0)
        {
            uwgApps.DataSource = dsApp.Tables[0];
            uwgApps.DataBind();

            //Session["Document"] = null;
            //ibExport.Visible = this.AllowExportToExcel;
            lblMsg.Visible = true;
            lblMsg.Text = "No Application(s) Records Found.";
            uwgApps.Visible = false;

        }
        else
        {
            lblMsg.Visible = false;
            uwgApps.Visible = true;
            uwgApps.ClearDataSource();
            uwgApps.DataSource = dsApp.Tables[0];
            uwgApps.DataBind();

        }

        this.uwgApps.Focus();

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

    private void FillAsset_Details()
    {
        int iPageIndex = (this.PageNum <= 0 ? 1 : this.PageNum);
        int pagesize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());
        if (!string.IsNullOrEmpty(LocationIDs))
        {
            objAsset = new iAssetTrack.BAL.AssetCreationBAL();
            DataSet dtAsset = objAsset.GetAssetListByLocations
                                            (LocationIDs.Trim(','),
                                                iPageIndex,
                                                pagesize
                                                );


            if (dtAsset.Tables[0].Rows.Count > 0)
            {
                // TODO:
                uwgAsset.GridView.ClearDataSource();
                ibExport.Visible = this.AllowExportToExcel;
                lblMsg.Visible = false;
                lblAssetList.Visible = true;
                uwgAsset.Visible = true;


                dtAsset.Tables[0].TableName = "ParentTable";
                dtAsset.Tables[1].TableName = "ChildTable";

                if (dtAsset.Tables[1].Rows.Count > 0)
                {
                    dtAsset.Relations.Add("Applications", dtAsset.Tables[0].Columns["ID"], dtAsset.Tables[1].Columns["ID"]);
                }

                this.uwgAsset.DataSource = dtAsset;
                this.uwgAsset.DataBind();

                //if (dtAsset.Tables[1].Rows.Count > 0)
                //{
                //    dtAsset.Relations.Add("Applications", dtAsset.Tables[0].Columns["ID"], dtAsset.Tables[1].Columns["ID"]);
                //}
                //uwgAsset.DataSource = dtAsset.Tables[0];
                //uwgAsset.DataBind();
                # region commented code
                //if (this.AllowMultiSelect)
                //{
                //    if (uwgAsset.Columns.Count > 0)
                //    {
                //        uwgAsset.Columns[0].AllowUpdate = Infragistics.WebUI.UltraWebGrid.AllowUpdate.Yes;
                //        uwgAsset.Columns[0].Width = Unit.Pixel(20);

                //        uwgAsset.Columns[0].Hidden = false;
                //    }

                //}
                //else
                //{
                //    if (uwgAsset.Columns.Count > 0)
                //    {
                //        uwgAsset.Columns[0].Hidden = true;
                //    }
                //}
                # endregion

                if (dtAsset.Tables[2].Rows.Count > 0)
                {
                    Navigation(Convert.ToInt32(dtAsset.Tables[2].Rows[0][0].ToString()));
                }

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

                ibCreate.Visible = true;
                ibReset.Visible = true;
                ibDelete.Visible = true;


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

                //Session["Document"] = null;
                ibExport.Visible = this.AllowExportToExcel;
                lblMsg.Visible = true;
                lblMsg.Text = "No Records Found. Please expand your search criteria.";
                uwgAsset.Visible = false;

                FirstPage.Visible = false;
                LastPage.Visible = false;
                NextPage.Visible = false;
                btnPreviousPage.Visible = false;

                SepLbl.Visible = false;
                GoToPageImb.Visible = false;
                GoToPageTxt.Visible = false;
                CurrentPage.Visible = false;
                TotalPages.Visible = false;

                //control buttons
                ibCreate.Visible = false;
                ibReset.Visible = false;
                ibDelete.Visible = false;
            }
            this.uwgAsset.Focus();
            //uwgAsset.Behaviors.ColumnFixing.FixedColumns.Add("RefNumber");
            //uwgAsset.Behaviors.ColumnFixing.FixedColumns.Add("MfgName");
            //uwgAsset.Behaviors.ColumnFixing.FixedColumns.Add("ModelName");
        }
        else
        {
            ibExport.Visible = this.AllowExportToExcel;
            uwgAsset.Visible = false;

            FirstPage.Visible = false;
            LastPage.Visible = false;
            NextPage.Visible = false;
            btnPreviousPage.Visible = false;

            SepLbl.Visible = false;
            GoToPageImb.Visible = false;
            GoToPageTxt.Visible = false;
            CurrentPage.Visible = false;
            TotalPages.Visible = false;

            lblAssetList.Visible = false;
            //control buttons
            ibCreate.Visible = false;
            ibReset.Visible = false;
            ibDelete.Visible = false;
            lblMsg.Visible = true;
            lblMsg.Text = "Please select at least one location to see Asset/Host details.";
        }
    }


    protected void ibExport_Click(object sender, ImageClickEventArgs e)
    {
        //Do Nothing for now
    }

    protected void ibExportToExcel_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {

        //Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        //this.eExporter.DataExportMode = DataExportMode.AllDataInDataSource;
        //Infragistics.Documents.Excel.Workbook wBook = new Infragistics.Documents.Excel.Workbook(excelFormat);
        //this.eExporter.Export(this.uwgAsset, wBook);
    }


    //protected void uwgAsset_PageIndexChanged(object sender, Infragistics.WebUI.UltraWebGrid.PageEventArgs e)
    //{
    //    this.CurrentPageSelections = hdnPageSelections.Value;
    //    this.PageNum = e.NewPageIndex;
    //    FillAsset_Details();
    //    this.hdnPageSelections.Value = this.CurrentPageSelections;
    //    //this.Fill_AssetDetails(e.NewPageIndex);
    //}


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
            catch
            {
                ShowMessage("Enter valid page count");
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


    private void ShowMessage(string mess)
    {
        string strScript = "<script type=\"text/javascript\">validNavigation = true;alert(\"" + mess + "\");</script>";
        if (!Page.ClientScript.IsStartupScriptRegistered("FORMMESSAGE"))
            Page.ClientScript.RegisterStartupScript(typeof(Page), "FORMMESSAGE", strScript);
    }



    protected void uwgAsset_InitializeRow(object sender, Infragistics.Web.UI.GridControls.RowEventArgs e)
    {
        ContainerGridRecord gridRecord = (ContainerGridRecord)e.Row;
        var owner = gridRecord.Owner;
       

        if (owner.ControlMain.Band != null && owner.ControlMain.Band.Key == "Level 0")
        {
            this.CheckRowCount++;

            //parent
            //check/uncheck based on the valu in the hidden field value
            if (!string.IsNullOrEmpty(hdnPageSelections.Value.Trim(',')))
            {
                foreach (string id in hdnPageSelections.Value.Trim(',').Split(','))
                {
                    if (e.Row.Items.FindItemByKey("ID").Value.ToString().CompareTo(id) == 0)
                    {
                        CheckBox theCB = (CheckBox)e.Row.Items.FindItemByKey(GRID_COL_KEY_SELECT).FindControl("ChkSelect");
                        if (theCB != null)
                            theCB.Checked = true;
                    }
                }

            }
            else
            {
                if (e.Row.Items.FindItemByKey("SelectCheckBox") != null)
                {
                    CheckBox theCB = (CheckBox)e.Row.Items.FindItemByKey("SelectCheckBox").FindControl("ChkSelect");
                    if (theCB != null)
                    {
                        theCB.Checked = false;
                    }

                    CheckBox chkBox = (CheckBox)uwgAsset.Columns[0].Header.FindControl("chkAll");

                    if (chkBox != null)
                    {
                        chkBox.Checked = false;
                    }
                }
            }

            if (uwgAsset.Rows.Count == this.CheckRowCount)
            {
                int checkedrowcount = 0;
                foreach (GridRecord rec in uwgAsset.Rows)
                {
                    CheckBox theCB = (CheckBox)rec.Items.FindItemByKey(GRID_COL_KEY_SELECT).FindControl("ChkSelect");
                    if (theCB != null)
                    {
                        if (theCB.Checked)
                            checkedrowcount++;
                    }

                }

                HtmlInputCheckBox chkBox = (HtmlInputCheckBox)uwgAsset.GridView.Columns[0].Header.FindControl("chkAll");
                if (chkBox != null)
                {
                    if (uwgAsset.Rows.Count == checkedrowcount)
                        chkBox.Checked = true;
                    else
                        chkBox.Checked = false;
                }
                else
                {
                    chkBox.Checked = false;
                }

                this.CheckRowCount = 0;
            }

        }
        else
        {
            if (hdnSelectedChildIDs != null && !string.IsNullOrEmpty(hdnSelectedChildIDs.Value.ToString().Trim(',')))
            {


                foreach (string id in hdnSelectedChildIDs.Value.Trim(',').Split(','))
                {
                    if (e.Row.Items.FindItemByKey("ApplID").Value.ToString().CompareTo(id) == 0)
                    {
                        CheckBox theCB = (CheckBox)e.Row.Items.FindItemByKey("SelectChildCheckBox").FindControl("ChkChild");
                        if (theCB != null)
                            theCB.Checked = true;
                    }
                }


            }
            else
            {
                CheckBox theCB = (CheckBox)e.Row.Items.FindItemByKey("SelectChildCheckBox").FindControl("ChkChild");
                if (theCB != null)
                    theCB.Checked = false;
            }

            if (e.Row.Items.FindItemByKey(GRID_COL_KEY_SELECT) != null)
            {
                CheckBox theCB = (CheckBox)e.Row.Items.FindItemByKey(GRID_COL_KEY_SELECT).FindControl("ChkSelect");
                if (theCB != null)
                    theCB.Checked = false;

                HtmlInputCheckBox chkBox = (HtmlInputCheckBox)uwgAsset.GridView.Columns[0].Header.FindControl("chkAll");
                if (chkBox != null)
                    chkBox.Checked = false;
            }
        }

       


    }
    protected void uwgAsset_RowIslandDataBinding(object sender, Infragistics.Web.UI.GridControls.RowIslandEventArgs e)
    {

    }
    protected void uwgApps_InitializeRow(object sender, Infragistics.Web.UI.GridControls.RowEventArgs e)
    {
        if (hdnPageSelectionsApp != null && !string.IsNullOrEmpty(hdnPageSelectionsApp.Value.ToString().Trim()))
        {
            foreach (string id in hdnPageSelectionsApp.Value.Trim(',').Split(','))
            {
                if (e.Row.Items.FindItemByKey("ApplID").Value.ToString().CompareTo(id) == 0)
                {
                    CheckBox theCB = (CheckBox)e.Row.Items.FindItemByKey("SelectCheckBox").FindControl("ChkApp");
                    if (theCB != null)
                        theCB.Checked = true;
                }
            }

            if (uwgApps.Rows.Count == hdnPageSelectionsApp.Value.Trim(',').Split(',').Count())
            {
                CheckBox chkBox = (CheckBox)uwgApps.Columns[0].Header.FindControl("chkAllApp");
                if (chkBox != null)
                    chkBox.Checked = true;
            }
            else
            {
                CheckBox chkBox = (CheckBox)uwgApps.Columns[0].Header.FindControl("chkAllApp");
                if (chkBox != null)
                    chkBox.Checked = false;
            }
        }
        else
        {
            if (e.Row.Items.FindItemByKey("SelectCheckBox") != null)
            {
                CheckBox theCB = (CheckBox)e.Row.Items.FindItemByKey("SelectCheckBox").FindControl("ChkApp");
                if (theCB != null)
                {
                    theCB.Checked = false;
                }

                CheckBox chkBox = (CheckBox)uwgApps.Columns[0].Header.FindControl("chkAllApp");

                if (chkBox != null)
                {
                    chkBox.Checked = false;
                }
            }
        }
    }
    protected void uwgAsset_DataBound(object sender, EventArgs e)
    {

    }
    protected void uwgApps_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControl.SetupPageList(this.uwgApps.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(uwgApps.Behaviors.Paging.PageIndex);
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
            pagerControl.SetupPageList(this.uwgApps.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(uwgApps.Behaviors.Paging.PageIndex);
        }
    }
    protected void uwgApps_DataFiltered(object sender, FilteredEventArgs e)
    {
        // Cast the WebDataGrid DataSource to a DataTable  
        DataTable dt = this.uwgApps.DataSource as DataTable;

        // Disable WebDataGrid Paging
        uwgApps.Behaviors.Paging.Enabled = false;

        hdnFilterCount.Value = uwgApps.Rows.Count.ToString();


        // Enable WebDataGrid Paging
        uwgApps.Behaviors.Paging.Enabled = true;
    }
    protected void uwgApps_ItemCommand(object sender, HandleCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            pagerControl.SetCurrentPageNumber(uwgApps.Behaviors.Paging.PageIndex);
        }
    }
}
