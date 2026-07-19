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
using iAssetTrack.DALC;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI.DataSourceControls;

public partial class InventoryUpdate : System.Web.UI.Page
{
    #region "Declarations"
    private int isUpdated = 0;
    private iAssetTrack.BAL.BusinessUnitBAL objBU;
    private iAssetTrack.BAL.SitesBAL objSite;
    private iAssetTrack.BAL.LocationBAL objLocation;
    //private iAssetTrack.BAL.CommonBAL objCommon;
    //private iAssetTrack.BAL.LocationTypeBAL objLocationType;
    DataTable _dtRights;
    Hashtable hUpdateDate = new Hashtable();
    private const string JAVA_SCRIPT = "javascript:";

    private const string PROP_CURRENTGRIDPAGENUMBER = "AssetSearchPageNum";
    //private const string PROP_ISSUEDTO = "IssuedTo";
    private const string PROP_ALLOWMULTISELECT = "MultiSelect";
    private const string PROP_ALLOWEXPORTTOEXCEL = "AllowExport";
    private const string PROP_PAGECOUNT = "PageCount";
    private const string PROP_AUTODISPLAYFIRSTPAGE = "AutoDisplayFirstPage";
    //private const string PROP_PARENTASSETID = "ParentAssetID";
    private const string PROP_SELECTEDLIST = "SelectedList";

    //private const string PROP_ORIGSELECTEDLIST = "OriginalSelectedList";
    //private const string PROP_SEARCHBARSTATE = "SearchBarState";
    //added by kjb on 11 Aug 2011
    private const string PROP_APP_PAGENUMBER = "AppPageNo";

    private const string PROP_APP_PAGECOUNT = "AppPageCount";
    private const string PROP_LOCATIONIDS = "LocationIDs";
    private const string PROP_SELECTEDLIST_APP = "SelectedAppList";
    private const string GRID_COL_KEY_APPID = "ApplID";
    //

    //private const string PROP_ALLOWBUSELECTION = "AllowBUSelection";


    //private const string JS_PARAM_ASSETID = "<AssetID>";
    //private const string JS_PARAM_REFNO = "<RefNumber>";
    //private const string JS_PARAM_ASSETNAME = "<AssetName>";
    //private const string JS_PARAM_PARENTID = "<ParentAssetID>";
    //private const string JS_PARAM_LEVEL = "<Level>";


    private const string GRID_COL_KEY_ID = "ID";
    //private const string GRID_COL_KEY_REFNO = "RefNumber";
    //private const string GRID_COL_KEY_ASSETNAME = "AssetName";
    private const string GRID_COL_KEY_SELECT = "SelectCheckBox";
    private const string GRID_COL_KEY_SELECT_CHILD = "SelectChildCheckBox";
    private const string GRID_ROW_CHECKBOX = "ChkSelect";
    private const string GRID_ROW_CHECKBOX_CHILD = "ChkChild";
    private const string GRID_ROW_CHECKBOX_APP = "ChkApp";
    private const string GRID_COL_KEY_STTYPE = "STTYPE";

    private const int GRID_BAND_LEVEL1 = 0;
    private const int GRID_BAND_LEVEL2 = 1;

    private const string ISPARENT_IMAGE = "infragistics/images/Outlook2003/folders.gif";
    private const string ISCHILD_IMAGE = "infragistics/images/Outlook2003/folder.gif";
    private const string RFID_IMAGE = "images/label_16x16.gif";

    private const string VIEWSTATE_PAGE_PFX = "PAGE_";
    private const string VIEWSTATE_PAGE_APP_PFX = "APP_PAGE_";

    private iAssetTrack_WebDataGrid_Paging_CustomerPagerControl pagerControl; // added by kjb on 28 Feb 2013
    private DataSet dsInvData = new DataSet();
    private string tenantAssignedLocations = string.Empty;
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

    #endregion

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        uwgAsset.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(uwgAsset_ItemCommand);
        pagerControl = uwgAsset.GridView.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);

    }

    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.uwgAsset.GridView.Behaviors.Paging.PageIndex = e.PageNumber;
        FillAsset_Details();
    }


    protected void uwgAsset_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControl.SetupPageList(this.uwgAsset.GridView.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(uwgAsset.GridView.Behaviors.Paging.PageIndex);
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
            pagerControl.SetupPageList(this.uwgAsset.GridView.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(uwgAsset.GridView.Behaviors.Paging.PageIndex);
        }

    }

    protected void uwgAsset_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {

            switch (e.CommandArgument.ToString())
            {
                case "First":

                    if (this.uwgAsset.GridView.Behaviors.Paging.PageIndex > 0)
                    {
                        FillAsset_Details();
                        pagerControl.SetupPageList(this.uwgAsset.GridView.Behaviors.Paging.PageCount);
                        pagerControl.SetCurrentPageNumber(1);

                    }
                    break;
                case "Prev":
                    if (this.uwgAsset.GridView.Behaviors.Paging.PageIndex > 0)
                    {
                        FillAsset_Details();
                        pagerControl.SetupPageList(this.uwgAsset.GridView.Behaviors.Paging.PageCount);
                        pagerControl.SetCurrentPageNumber(this.uwgAsset.GridView.Behaviors.Paging.PageIndex - 1);

                    }
                    break;
                case "Next":
                    if (this.uwgAsset.GridView.Behaviors.Paging.PageIndex + 1 < this.uwgAsset.GridView.Behaviors.Paging.PageCount)
                    {
                        FillAsset_Details();
                        pagerControl.SetupPageList(this.uwgAsset.GridView.Behaviors.Paging.PageCount);
                        pagerControl.SetCurrentPageNumber(this.uwgAsset.GridView.Behaviors.Paging.PageIndex + 1);

                    }
                    break;
                case "Last":
                    if (this.uwgAsset.GridView.Behaviors.Paging.PageIndex + 1 < this.uwgAsset.GridView.Behaviors.Paging.PageCount)
                    {
                        FillAsset_Details();
                        pagerControl.SetupPageList(this.uwgAsset.GridView.Behaviors.Paging.PageCount);
                        pagerControl.SetCurrentPageNumber(this.uwgAsset.GridView.Behaviors.Paging.PageIndex + 1);

                    }
                    break;
            }
        }

    }


    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageHeader"] = "Inventory Update";

        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "InventoryUpdate.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        if (_dtRights.Select("Module = 'Inventory Update' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }

        if (_dtRights.Select("Module = 'Inventory Update' and Rights = '" + "Inventory Update" + "'").Length != 0)
        {
            ibUpdate.Enabled = true;
        }
        else
        {
            ibUpdate.Enabled = false;
        }
        this.uwgAsset.GridView.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());

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
            lblMsg.Visible = false;
            LoadLocations();

            // allow multi selection in Asset List
            this.AllowMultiSelect = true;

            // Initial load, hide paging buttons of Asset list.
            # region commented page controls
            //FirstPage.Enabled = false;
            //LastPage.Enabled = false;
            //NextPage.Enabled = false;
            //PreviousPage.Enabled = false;


            //FirstPage.Visible = false;
            //LastPage.Visible = false;
            //NextPage.Visible = false;
            //PreviousPage.Visible = false;

            //SepLbl.Visible = false;
            //GoToPageImb.Visible = false;
            //GoToPageTxt.Visible = false;
            //CurrentPage.Visible = false;
            //TotalPages.Visible = false;

            # endregion

            ibUpdate.Visible = false;
            ibReset.Visible = false;
        }
        //else
        //{
        //    if (TreeLocation.CheckedNodes.Count > 0)
        //    {
        FillAsset_Details();
        //    }
        //}


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
    //            //buNode.Enabled = false;
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
    //                    //siteNode.Enabled = false;
    //                    //siteNode.CssClass = ".TreeNodeDisabled";
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
        lblMsg.Text = "";
        LocationIDs = string.Empty;
        foreach (DataTreeNode node in TreeLocation.CheckedNodes)
        {
            if (node.Level >= 2)
            {
                LocationIDs = LocationIDs + node.Value + ",";
            }
        }
        PageNum = this.PageNum;
        FillAsset_Details();
    }

    protected void ibDelete_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        # region commented code
        ////int _AssetID;
        //string _AssignmentID;
        //// check whether any row selected in Asset List
        //// if so than only do the delete map for that Asset only.
        ////this.CurrentPageSelections = hdnIDs.Value;
        ////if (!string.IsNullOrEmpty(CurrentPageSelections))
        ////{
        ////    _AssetID = int.Parse(CurrentPageSelections);

        ////}
        //try
        //{
        //    lblMsg.Text = "";
        //    lblMsg.Visible = false;
        //    if (!string.IsNullOrEmpty(hdnSelectedAssetID.Value) && hdnSelectedChildIDs != null)
        //    {
        //        //_AssetID = int.Parse(hdnSelectedAssetID.Value.ToString());
        //        _AssignmentID = hdnSelectedAssetID.Value.ToString();
        //        ApplicationMapBAL objMap = new ApplicationMapBAL();
        //        objMap.ID = _AssignmentID.ToString(); //represents AssignmentID
        //        objMap.ApplIDs = hdnSelectedChildIDs.Value.Trim(',');

        //        objMap.Persist(iAssetTrack.DALC.DALCOperation.Delete);

        //        hdnSelectedChildIDs.Value = "";
        //        hdnSelectedAssetID.Value = "";


        //        this.CurrentPageSelections = hdnIDs.Value;

        //        this.CurrentPageSelectionsApp = hdnPageSelectionsApp.Value;

        //        FillAsset_Details();
        //        lblMsg.Text = "Un-Mapped successfully";
        //        lblMsg.Visible = true;

        //    }
        //    else
        //    {
        //        FillAsset_Details();
        //        lblMsg.Text = "Select only one Asset for un-map";
        //        lblMsg.Visible = true;
        //    }

        //}
        //catch (Exception ex)
        //{
        //    FillAsset_Details();
        //    lblMsg.Text = ex.Message;
        //    lblMsg.Visible = true;
        //}
        # endregion
    }


    protected void ibShowData_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        lblMsg.Text = "";
        lblMsg.Visible = false;
        LocationIDs = string.Empty;

        foreach (DataTreeNode node in TreeLocation.CheckedNodes)
        {
            if (node.Level >= 2)
            {
                LocationIDs = LocationIDs + node.Value + ",";
            }
        }
        LocationIDs = LocationIDs.Trim(',');
        PageNum = this.PageNum;
        FillAsset_Details();
    }

    protected void ibUpdate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        try
        {
            lblMsg.Text = "";
            lblMsg.Visible = false;


            hUpdateDate.Clear();
            //for (int i = 0; i < uwgAsset.Rows.Count; i++)
            //{
            //    if (uwgAsset.GridView.Rows[i].RowIslands.Count > 0)
            //    {
            //        ContainerGrid childGrid = uwgAsset.GridView.Rows[i].RowIslands[0];//only one band
            //        foreach (ContainerGridRecord childRec in childGrid.Rows)
            //        {
            //            GridRecordItem recItem = (GridRecordItem)childRec.Items[0];

            //            if (recItem.FindControl("ChkSelectChild") != null)
            //            {
            //                CheckBox chkBox = (CheckBox)recItem.FindControl("ChkSelectChild");
            //                if (chkBox.Checked)
            //                {
            //                    string id = childRec.Items[9].Value.ToString();
            //                    string assetID = childRec.Items[10].Value.ToString();
            //                    hUpdateDate.Add(id, assetID);
            //                }
            //            }
            //        }
            //    }
            //}

            //invetory update 
            if (hdnAssetIDs != null && !string.IsNullOrEmpty(hdnAssetIDs.Value.ToString()))
            {
                //use this if java script functuions working...
                string keyVals = hdnAssetIDs.Value.ToString().Trim(',');
                foreach (string keyVal in keyVals.Split(','))
                {
                    string id = keyVal.Split('#')[0];
                    string assetId = keyVal.Split('#')[1];
                    if (!hUpdateDate.ContainsKey(id))
                    {
                        hUpdateDate.Add(id, assetId);
                    }
                    else
                    {
                        string assetIds = hUpdateDate[id].ToString() + "," + assetId;
                        hUpdateDate[id] = assetIds;
                    }
                }
                int errCount = 0;
                foreach (DictionaryEntry keyVal in hUpdateDate)
                {
                    try
                    {
                        InventoryUpdateBAL obIU = new InventoryUpdateBAL();
                        obIU.ID = keyVal.Key.ToString();
                        obIU.AssetIDs = keyVal.Value.ToString();
                        obIU.CreatedBy = Convert.ToInt32(Session["UserID"]);
                        obIU.Persist(DALCOperation.Update);
                    }
                    catch (Exception ex)
                    {
                        errCount++;
                        ExceptionPolicy.HandleException(ex, "errDCTrack");
                    }
                }
                if (errCount == 0)
                {
                    lblMsg.Text = "Inventory data updated successfully!";
                    lblMsg.Visible = true;
                    hdnAssetIDs.Value = "";
                    hdnIDs.Value = "";
                    isUpdated = 1;
                }
                else if (hUpdateDate.Count == errCount)
                {
                    lblMsg.Text = "Inventory data update failed!";
                    lblMsg.Visible = true;
                }
                else
                {
                    lblMsg.Text = "Inventory data updated with errors!";
                    lblMsg.Visible = true;
                    isUpdated = 2;
                }
            }
            else
            {
                lblMsg.Text = " Select atleast one asset to update!";
                lblMsg.Visible = true;
            }
            FillAsset_Details();
        }
        catch (Exception ex)
        {
            FillAsset_Details();
            ExceptionPolicy.HandleException(ex, "errDCTrack");
            lblMsg.Text = "Update Failed";
            lblMsg.Visible = true;
        }
    }


    protected void ibReset_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        lblMsg.Text = "";
        lblMsg.Visible = false;
        hdnAssetIDs.Value = "";
        hdnIDs.Value = "";
        FillAsset_Details();
    }



    private void FillAsset_Details()
    {
        int iPageIndex = (this.PageNum <= 0 ? 1 : this.PageNum);
        int pageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());
        if (!string.IsNullOrEmpty(LocationIDs))
        {
            InventoryUpdateBAL obIU = new InventoryUpdateBAL();
            dsInvData = obIU.retrieve(LocationIDs, iPageIndex, pageSize);


            if (dsInvData.Tables[0].Rows.Count > 0)
            {
                uwgAsset.GridView.ClearDataSource();
                ibExport.Visible = this.AllowExportToExcel;
                //lblMsg.Visible = false;
                this.uwgAsset.Visible = true;

                dsInvData.Tables[0].TableName = "ParentTable";
                dsInvData.Tables[1].TableName = "ChildTable";
                if (dsInvData.Tables[1].Rows.Count > 0)
                {
                    dsInvData.Relations.Add("Assets", dsInvData.Tables[0].Columns["ID"], dsInvData.Tables[1].Columns["ID"]);
                }

                this.uwgAsset.DataSource = dsInvData;
                this.uwgAsset.DataBind();


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

                //if (dsInvData.Tables[2].Rows.Count > 0)
                //{
                //    Navigation(Convert.ToInt32(dsInvData.Tables[2].Rows[0][0].ToString()));
                //}

                # region commented page controls
                //FirstPage.Visible = true;
                //LastPage.Visible = true;
                //NextPage.Visible = true;
                //PreviousPage.Visible = true;

                //SepLbl.Visible = true;
                //GoToPageImb.Visible = true;
                //GoToPageTxt.Visible = true;
                //CurrentPage.Visible = true;
                //TotalPages.Visible = true;


                //FirstPage.Enabled = true;
                //LastPage.Enabled = true;
                //NextPage.Enabled = true;
                //PreviousPage.Enabled = true;
                # endregion

                ibUpdate.Visible = true;
                ibReset.Visible = true;

                # region commented page controls
                //if (Convert.ToInt32(TotalPages.Text) == 1)
                //{
                //    FirstPage.Enabled = false;
                //    LastPage.Enabled = false;
                //    NextPage.Enabled = false;
                //    PreviousPage.Enabled = false;


                //    FirstPage.Visible = false;
                //    LastPage.Visible = false;
                //    NextPage.Visible = false;
                //    PreviousPage.Visible = false;

                //    SepLbl.Visible = false;
                //    GoToPageImb.Visible = false;
                //    GoToPageTxt.Visible = false;
                //    CurrentPage.Visible = false;
                //    TotalPages.Visible = false;
                //}
                //else
                //{
                //    if (Convert.ToInt32(CurrentPage.Text) == 1)
                //    {
                //        FirstPage.Enabled = false;
                //        PreviousPage.Enabled = false;
                //    }
                //    if (Convert.ToInt32(CurrentPage.Text) == Convert.ToInt32(TotalPages.Text))
                //    {
                //        LastPage.Enabled = false;
                //        NextPage.Enabled = false;
                //    }
                //}
                # endregion
            }
            else
            {

                //Session["Document"] = null;
                ibExport.Visible = this.AllowExportToExcel;

                lblMsg.Visible = true;
                // to handle the scenario, where
                // asset(s) updated and no more records to show in the grid.
                if (isUpdated == 1)
                {
                    lblMsg.Text = "Updated successfully. No Records Found. Please expand your search criteria.";
                }
                else if (isUpdated == 2)
                {
                    lblMsg.Text = "Updated with errors. No Records Found. Please expand your search criteria.";
                }
                else
                {
                    lblMsg.Text = "No Records Found. Please expand your search criteria.";
                }
                uwgAsset.Visible = false;

                # region commented page controls
                //FirstPage.Visible = false;
                //LastPage.Visible = false;
                //NextPage.Visible = false;
                //PreviousPage.Visible = false;

                //SepLbl.Visible = false;
                //GoToPageImb.Visible = false;
                //GoToPageTxt.Visible = false;
                //CurrentPage.Visible = false;
                //TotalPages.Visible = false;
                # endregion
                //control buttons
                ibUpdate.Visible = false;
                ibReset.Visible = false;
            }
            this.uwgAsset.Focus();
        }
        else
        {
            //control buttons
            this.uwgAsset.Visible = false;
            ibUpdate.Visible = false;
            ibReset.Visible = false;
            lblMsg.Visible = true;
            lblMsg.Text = "Please select at least one location.";
        }
    }


    protected void ibExport_Click(object sender, ImageClickEventArgs e)
    {
        //Do Nothing for now
    }

    private void ShowMessage(string mess)
    {
        string strScript = "<script type=\"text/javascript\">validNavigation = true;alert(\"" + mess + "\");</script>";
        if (!Page.ClientScript.IsStartupScriptRegistered("FORMMESSAGE"))
            Page.ClientScript.RegisterStartupScript(typeof(Page), "FORMMESSAGE", strScript);
    }

    protected void ibExportToExcel_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {

        //Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        //this.eExporter.DataExportMode = DataExportMode.AllDataInDataSource;
        //Infragistics.Documents.Excel.Workbook wBook = new Infragistics.Documents.Excel.Workbook(excelFormat);
        //this.eExporter.Export(this.uwgAsset, wBook);
    }

    protected void uwgAsset_InitializeRow(object sender, Infragistics.Web.UI.GridControls.RowEventArgs e)
    {
        ContainerGridRecord gridRecord = (ContainerGridRecord)e.Row;
        var owner = gridRecord.Owner;

        if (owner.ControlMain.Band != null && owner.ControlMain.Band.Key == "Level 0")
        {
            //parent
            //check/uncheck based on the valu in the hidden field value
            if (!string.IsNullOrEmpty(hdnIDs.Value))
            {
                foreach (string id in hdnIDs.Value.Trim(',').Split(','))
                {
                    if (e.Row.Items.FindItemByKey("ID").Value.ToString().CompareTo(id) == 0)
                    {
                        CheckBox theCB = (CheckBox)e.Row.Items.FindItemByKey(GRID_COL_KEY_SELECT).FindControl("ChkSelect");
                        theCB.Checked = true;
                    }
                }

            }
            if (uwgAsset.GridView.Rows.Count == hdnIDs.Value.Trim(',').Split(',').Length)
            {
                CheckBox chkBox = (CheckBox)uwgAsset.GridView.Columns[0].Header.FindControl("chkAll");
                chkBox.Checked = true;
            }
            else
            {
                CheckBox chkBox = (CheckBox)uwgAsset.GridView.Columns[0].Header.FindControl("chkAll");
                chkBox.Checked = false;
            }
        }
        else
        {
            // enable/ disable checkbox based on IsValidated column value
            CheckBox childCB = (CheckBox)e.Row.Items.FindItemByKey(GRID_COL_KEY_SELECT_CHILD).FindControl("ChkSelectChild");
            object colValue = e.Row.Items.FindItemByKey("IsValidated").Value;
            if (colValue == null)
                childCB.Enabled = true;
            else
                childCB.Enabled = (bool)colValue;

            // check/uncheck based on the valu in the hidden field value
            if (!string.IsNullOrEmpty(hdnAssetIDs.Value))
            {
                foreach (string keyVal in hdnAssetIDs.Value.Trim(',').Split(','))
                {
                    string id = keyVal.Split('#')[0];
                    string assetId = keyVal.Split('#')[1];

                    if (e.Row.Items.FindItemByKey("ID").Value.ToString().CompareTo(id) == 0 &&
                        e.Row.Items.FindItemByKey("AssetID").Value.ToString().CompareTo(assetId) == 0)
                    {
                        CheckBox theCB = (CheckBox)e.Row.Items.FindItemByKey(GRID_COL_KEY_SELECT_CHILD).FindControl("ChkSelectChild");
                        theCB.Checked = true;

                        ContainerGridRecord row = (ContainerGridRecord)e.Row;

                        // Get the row's owner row collection
                        ContainerGridRecordCollection rowCollection = row.Owner;

                        // Get the parent row by first accessing the owner Container Grid
                        ContainerGridRecord parentRow = rowCollection.ControlMain.ParentRow;

                        parentRow.ExpandChildren();
                    }
                }

            }
            //child
            if (e.Row.Items[1].Value.ToString() == "Scanned")
            {
                e.Row.CssClass = "Scanned";
            }

            if (e.Row.Items[1].Value.ToString() == "Missing")
            {
                e.Row.CssClass = "Missing";

                //CheckBox chkBox = (CheckBox)(e.Row.Items[0].FindControl("ChkSelectChild"));
                //chkBox.Visible = false;
            }

            if (e.Row.Items[1].Value.ToString() == "Misplaced")
            {
                e.Row.CssClass = "Misplaced";
            }
            //e.Row.Items[8].CssClass = "disabled";
            //e.Row.Items[9].CssClass = "disabled";
            //if (e.Row.Items[9].Column.Header.Text == "ID")
            //    e.Row.Items[9].Column.Hidden = true;
            //if (e.Row.Items[10].Column.Header.Text == "AssetID")
            //    e.Row.Items[10].Column.Hidden = true;
        }



        # region code for RowIsland_Populating functionality

        //ContainerGridRecord gridRecord = (ContainerGridRecord)e.Row;

        //var owner = gridRecord.Owner;

        //if (owner.ControlMain.Band != null && owner.ControlMain.Band.Key == "Level 0")
        //{
        //    gridRecord.IsEmptyParent = true;
        //}
        //else
        //{
        //    gridRecord.IsEmptyParent = false;
        //}

        # endregion

    }

    //if(e.Row.Owner == null)
    //{
    //    if (this.AllowMultiSelect)
    //    {

    //        // check/uncheck based on the valu in the hidden field value
    //        //if (!string.IsNullOrEmpty(hdnIDs.Value))
    //        //{
    //        //    foreach (string id in hdnIDs.Value.Trim(',').Split(','))
    //        //    {
    //        //        if (e.Row.Cells.FromKey("ID").Value.ToString().CompareTo(id) == 0)
    //        //        {
    //        //            TemplatedColumn tempChildCol = (TemplatedColumn)e.Row.Cells.FromKey(GRID_COL_KEY_SELECT).Column;
    //        //            CellItem cellItem = (CellItem)tempChildCol.CellItems[tempChildCol.CellItems.Count - 1];
    //        //            CheckBox cbSelect = (CheckBox)cellItem.FindControl(GRID_ROW_CHECKBOX);
    //        //            cbSelect.Checked = true;
    //        //        }
    //        //    }

    //        //}

    //    }
    //}

    //else
    //{
    //    if (this.AllowMultiSelect)
    //    {
    //        // check/uncheck based on the valu in the hidden field value
    //        //if (!string.IsNullOrEmpty(hdnAssetIDs.Value))
    //        //{
    //        //    foreach (string keyVal in hdnAssetIDs.Value.Trim(',').Split(','))
    //        //    {
    //        //        string id = keyVal.Split('#')[0];
    //        //        string assetId = keyVal.Split('#')[1];

    //        //        if (e.Row.Cells.FromKey("ID").Value.ToString().CompareTo(id) == 0 &&
    //        //            e.Row.Cells.FromKey("AssetID").Value.ToString().CompareTo(assetId) == 0)
    //        //        {
    //        //            TemplatedColumn tempChildCol = (TemplatedColumn)e.Row.Cells.FromKey(GRID_COL_KEY_SELECT_CHILD).Column;
    //        //            CellItem cellItem = (CellItem)tempChildCol.CellItems[tempChildCol.CellItems.Count - 1];
    //        //            CheckBox cbSelect = (CheckBox)cellItem.FindControl(GRID_ROW_CHECKBOX_CHILD);
    //        //            cbSelect.Checked = true;
    //        //        }
    //        //    }

    //        //}

    //        //if (e.Row.Items[3].Value.ToString() == "Scanned")
    //        //{
    //        //    foreach (GridRecordItem item in e.Row.Items)
    //        //    {
    //        //        item.CssClass = "Scanned";
    //        //    }
    //        //}

    //        //if (e.Row.Items[3].Value.ToString() == "Missing")
    //        //{
    //        //    foreach (GridRecordItem item in e.Row.Items)
    //        //    {
    //        //        item.CssClass = "Missing";
    //        //    }

    //        //    CheckBox chkBox = (CheckBox)(e.Row.Items[0].FindControl("SelectChildCheckBox"));
    //        //    chkBox.Visible = false;
    //        //}

    //        //if (e.Row.Items[3].Value.ToString() == "Misplaced")
    //        //{
    //        //    foreach (GridRecordItem item in e.Row.Items)
    //        //    {
    //        //        item.CssClass = "Misplaced";
    //        //    }
    //        //}

    //    }
    //}

    # region commented code
    //protected void NavigationLink_Click(Object sender, CommandEventArgs e)
    //{
    //    if (this.AllowMultiSelect) this.CurrentPageSelections = hdnIDs.Value;
    //    switch (e.CommandName)
    //    {
    //        case "First":
    //            this.PageNum = 1;
    //            break;
    //        case "Last":
    //            this.PageNum = Convert.ToInt16(TotalPages.Text);
    //            break;
    //        case "Next":
    //            this.PageNum = Convert.ToInt16(CurrentPage.Text) + 1;
    //            break;
    //        case "Prev":
    //            this.PageNum = Convert.ToInt16(CurrentPage.Text) - 1;
    //            break;
    //    }
    //    FillAsset_Details();
    //    if (this.AllowMultiSelect)
    //    {
    //        if (this.CurrentPageSelections == null && this.SelectedList != null) this.CurrentPageSelections = String.Join(",", this.SelectedList);
    //        hdnIDs.Value = "";
    //    }
    //}

    //protected void uwgApps_InitializeLayout(object sender, LayoutEventArgs e)
    //{
    //    //e.Layout.UseFixedHeaders = true;
    //    //e.Layout.Bands[0].Columns[1].Header.Fixed = true;
    //    //e.Layout.Bands[0].Columns[2].Header.Fixed = true;
    //    //e.Layout.Bands[0].Columns[3].Header.Fixed = true;
    //    e.Layout.StationaryMargins = StationaryMargins.Header;

    //}

    //protected void uwgApps_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    //{
    //    //try
    //    //{
    //    //    if (m_ClientSideEventEnabled)
    //    //    {

    //    //        string strID = e.Row.Cells.FromKey(GRID_COL_KEY_ID).Text.Replace("'", "~~~");

    //    //        string strJSFunc = strClientSideOnSelection;
    //    //        strJSFunc = strJSFunc.Replace(JS_PARAM_ASSETID, strID);
    //    //        strJSFunc = strJSFunc.Replace(JS_PARAM_REFNO, e.Row.Cells.FromKey(GRID_COL_KEY_REFNO).Text);
    //    //        strJSFunc = strJSFunc.Replace(JS_PARAM_ASSETNAME, e.Row.Cells.FromKey(GRID_COL_KEY_ASSETNAME).Text);
    //    //        strJSFunc = strJSFunc.Replace(JS_PARAM_PARENTID, (e.Row.Cells.FromKey(GRID_COL_KEY_PARENTASSETID).Value == null ? "0" : e.Row.Cells.FromKey(GRID_COL_KEY_PARENTASSETID).Text));
    //    //        strJSFunc = strJSFunc.Replace(JS_PARAM_LEVEL, Convert.ToString(e.Row.Band.Index));

    //    //        //e.Row.Cells.FromKey(GRID_COL_KEY_ASSETNAME).Column.Type = ColumnType.HyperLink;
    //    //        //e.Row.Cells.FromKey(GRID_COL_KEY_ASSETNAME).TargetURL = strJSFunc;

    //    //        e.Row.Cells.FromKey(GRID_COL_KEY_REFNO).Text = "<div><a href=\"" + strJSFunc + "\" CssClass=\"lnk\">" + e.Row.Cells.FromKey(GRID_COL_KEY_REFNO).Text + "</a></div>";
    //    //    }
    //    //    else
    //    //    {
    //    //        //e.Row.Cells.FromKey(GRID_COL_KEY_ASSETNAME).TargetURL = "lbTitle_Click()";
    //    //        //e.Row.Cells.FromKey(GRID_COL_KEY_ASSETNAME).Text = "<div><asp:LinkButton ID=\"lbName\" runat=\"server\" CssClass=\"lnk\" OnClick=\"lbTitle_Click\"><%# Container.Value %></asp:LinkButton></div>";
    //    //        //e.Row.Cells.FromKey(GRID_COL_KEY_ASSETNAME).Text = "<div><a href='' OnClick='lbTitle_Click' runat=server CssClass=lnk>" + e.Row.Cells.FromKey(GRID_COL_KEY_ASSETNAME).Text + "</a><div>";
    //    //    }


    //    if (e.Row.Band.Index == GRID_BAND_LEVEL1) // Allow Selection only at the first band level
    //    {
    //        if (this.AllowMultiSelect)
    //        {

    //            string strID = e.Row.Cells.FromKey(GRID_COL_KEY_APPID).Text.Replace("'", "~~~");

    //            TemplatedColumn tempCol = (TemplatedColumn)e.Row.Cells.FromKey(GRID_COL_KEY_SELECT).Column;
    //            CellItem cellItem = (CellItem)tempCol.CellItems[e.Row.Index];
    //            CheckBox cbSelect = (CheckBox)cellItem.FindControl(GRID_ROW_CHECKBOX_APP);

    //            if (this.CurrentPageSelectionsApp != null)
    //            {

    //                String csvPageSelections = this.CurrentPageSelectionsApp;

    //                String[] arrPageSelections = csvPageSelections.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

    //                foreach (String s in arrPageSelections)
    //                {
    //                    if (s.Equals(strID))
    //                    {
    //                        string[] list = hdnPageSelectionsApp.Value.Split(',');
    //                        if (!list.Contains(s))
    //                        {
    //                            hdnPageSelectionsApp.Value += ",";
    //                            hdnPageSelectionsApp.Value += s;
    //                        }
    //                        cbSelect.Checked = true;
    //                        break;
    //                    }
    //                }

    //                // Remove from the Selections Passed so that the list does not contain duplicates
    //                if (this.SelectedListApp != null)
    //                {
    //                    String csvInitSelections = "";
    //                    foreach (String s1 in this.SelectedListApp)
    //                    {
    //                        if (!s1.Equals(strID)) csvInitSelections += s1 + ",";
    //                    }
    //                    this.SelectedListApp = csvInitSelections.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
    //                }
    //            }

    //        }
    //    }



    //    //        bool bIsParent = Convert.ToBoolean(e.Row.Cells.FromKey(GRID_COL_KEY_ISPARENT).Value);

    //    //        if (bIsParent)
    //    //        {

    //    //            e.Row.Cells.FromKey(GRID_COL_KEY_ISPARENTICON).Style.CustomRules = "Background-repeat:no-repeat";
    //    //            e.Row.Cells.FromKey(GRID_COL_KEY_ISPARENTICON).Style.BackgroundImage = ISPARENT_IMAGE;

    //    //        }
    //    //        else if (e.Row.Cells.FromKey(GRID_COL_KEY_PARENTASSETID).Value != null)
    //    //        {

    //    //            e.Row.Cells.FromKey(GRID_COL_KEY_ISPARENTICON).Style.CustomRules = "Background-repeat:no-repeat";
    //    //            e.Row.Cells.FromKey(GRID_COL_KEY_ISPARENTICON).Style.BackgroundImage = ISCHILD_IMAGE;

    //    //        }

    //    //        if (e.Row.Cells.FromKey(GRID_COL_KEY_RFIDTAGID).Value != null)
    //    //        {
    //    //            e.Row.Cells.FromKey(GRID_COL_KEY_RFIDTAGICON).Style.CustomRules = "Background-repeat:no-repeat";
    //    //            e.Row.Cells.FromKey(GRID_COL_KEY_RFIDTAGICON).Style.BackgroundImage = RFID_IMAGE;
    //    //            e.Row.Cells.FromKey(GRID_COL_KEY_RFIDTAGICON).Title = e.Row.Cells.FromKey(GRID_COL_KEY_RFIDTAGID).Text;
    //    //        }

    //    //    }
    //    //}
    //    //catch { }

    //}



    //protected void uwgAsset_InitializeLayout(object sender, LayoutEventArgs e)
    //{
    //    //e.Layout.UseFixedHeaders = true;
    //    //e.Layout.Bands[0].Columns[1].Header.Fixed = true;
    //    //e.Layout.Bands[0].Columns[2].Header.Fixed = true;
    //    //e.Layout.Bands[0].Columns[3].Header.Fixed = true;
    //    e.Layout.StationaryMargins = StationaryMargins.Header;

    //}

    //protected void uwgAsset_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    //{
    //    # region commented code
    //    //try
    //    //{
    //    //    if (m_ClientSideEventEnabled)
    //    //    {

    //    //        string strID = e.Row.Cells.FromKey(GRID_COL_KEY_ID).Text.Replace("'", "~~~");

    //    //        string strJSFunc = strClientSideOnSelection;
    //    //        strJSFunc = strJSFunc.Replace(JS_PARAM_ASSETID, strID);
    //    //        strJSFunc = strJSFunc.Replace(JS_PARAM_REFNO, e.Row.Cells.FromKey(GRID_COL_KEY_REFNO).Text);
    //    //        strJSFunc = strJSFunc.Replace(JS_PARAM_ASSETNAME, e.Row.Cells.FromKey(GRID_COL_KEY_ASSETNAME).Text);
    //    //        strJSFunc = strJSFunc.Replace(JS_PARAM_PARENTID, (e.Row.Cells.FromKey(GRID_COL_KEY_PARENTASSETID).Value == null ? "0" : e.Row.Cells.FromKey(GRID_COL_KEY_PARENTASSETID).Text));
    //    //        strJSFunc = strJSFunc.Replace(JS_PARAM_LEVEL, Convert.ToString(e.Row.Band.Index));

    //    //        //e.Row.Cells.FromKey(GRID_COL_KEY_ASSETNAME).Column.Type = ColumnType.HyperLink;
    //    //        //e.Row.Cells.FromKey(GRID_COL_KEY_ASSETNAME).TargetURL = strJSFunc;

    //    //        e.Row.Cells.FromKey(GRID_COL_KEY_REFNO).Text = "<div><a href=\"" + strJSFunc + "\" CssClass=\"lnk\">" + e.Row.Cells.FromKey(GRID_COL_KEY_REFNO).Text + "</a></div>";
    //    //    }
    //    //    else
    //    //    {
    //    //        //e.Row.Cells.FromKey(GRID_COL_KEY_ASSETNAME).TargetURL = "lbTitle_Click()";
    //    //        //e.Row.Cells.FromKey(GRID_COL_KEY_ASSETNAME).Text = "<div><asp:LinkButton ID=\"lbName\" runat=\"server\" CssClass=\"lnk\" OnClick=\"lbTitle_Click\"><%# Container.Value %></asp:LinkButton></div>";
    //    //        //e.Row.Cells.FromKey(GRID_COL_KEY_ASSETNAME).Text = "<div><a href='' OnClick='lbTitle_Click' runat=server CssClass=lnk>" + e.Row.Cells.FromKey(GRID_COL_KEY_ASSETNAME).Text + "</a><div>";
    //    //    }
    //    # endregion

    //    if (e.Row.Band.Index == GRID_BAND_LEVEL1) // Allow Selection only at the first band level
    //    {
    //        if (this.AllowMultiSelect)
    //        {

    //            // check/uncheck based on the valu in the hidden field value
    //            if (!string.IsNullOrEmpty(hdnIDs.Value))
    //            {
    //                foreach (string id in hdnIDs.Value.Trim(',').Split(','))
    //                {
    //                    if (e.Row.Cells.FromKey("ID").Value.ToString().CompareTo(id) == 0)
    //                    {
    //                        TemplatedColumn tempChildCol = (TemplatedColumn)e.Row.Cells.FromKey(GRID_COL_KEY_SELECT).Column;
    //                        CellItem cellItem = (CellItem)tempChildCol.CellItems[tempChildCol.CellItems.Count - 1];
    //                        CheckBox cbSelect = (CheckBox)cellItem.FindControl(GRID_ROW_CHECKBOX);
    //                        cbSelect.Checked = true;
    //                    }
    //                }

    //            }

    //            //string strID = e.Row.Cells.FromKey(GRID_COL_KEY_ID).Text.Replace("'", "~~~");

    //            //TemplatedColumn tempCol = (TemplatedColumn)e.Row.Cells.FromKey(GRID_COL_KEY_SELECT).Column;
    //            //CellItem cellItem = (CellItem)tempCol.CellItems[e.Row.Index];
    //            //CheckBox cbSelect = (CheckBox)cellItem.FindControl(GRID_ROW_CHECKBOX);

    //            //if (this.CurrentPageSelections != null)
    //            //{

    //            //    String csvPageSelections = this.CurrentPageSelections;

    //            //    String[] arrPageSelections = csvPageSelections.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

    //            //    foreach (String s in arrPageSelections)
    //            //    {
    //            //        if (s.Equals(strID))
    //            //        {
    //            //            if (!hdnIDs.Value.Split(',').Contains(s))
    //            //            {
    //            //                hdnIDs.Value += ",";
    //            //                hdnIDs.Value += s;
    //            //            }
    //            //            cbSelect.Checked = true;
    //            //            break;
    //            //        }
    //            //    }

    //            //    // Remove from the Selections Passed so that the list does not contain duplicates
    //            //    if (this.SelectedList != null)
    //            //    {
    //            //        String csvInitSelections = "";
    //            //        foreach (String s1 in this.SelectedList)
    //            //        {
    //            //            if (!s1.Equals(strID)) csvInitSelections += s1 + ",";
    //            //        }
    //            //        this.SelectedList = csvInitSelections.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
    //            //    }
    //            //}
    //            // update check boxes of AssetIds using hdnAssetIDs

    //            //if (hdnSelectedAssetID.Value != null)
    //            //{
    //            //    if (hdnSelectedAssetID.Value.Equals(strID))
    //            //    {
    //            //        e.Row.Selected = true;
    //            //    }
    //            //}

    //        }
    //    }

    //    if (e.Row.Band.Index == GRID_BAND_LEVEL2)
    //    {
    //        if (this.AllowMultiSelect)
    //        {
    //            // check/uncheck based on the valu in the hidden field value
    //            if (!string.IsNullOrEmpty(hdnAssetIDs.Value))
    //            {
    //                foreach (string keyVal in hdnAssetIDs.Value.Trim(',').Split(','))
    //                {
    //                    string id = keyVal.Split('#')[0];
    //                    string assetId = keyVal.Split('#')[1];

    //                    if (e.Row.Cells.FromKey("ID").Value.ToString().CompareTo(id) == 0 &&
    //                        e.Row.Cells.FromKey("AssetID").Value.ToString().CompareTo(assetId) == 0)
    //                    {
    //                        TemplatedColumn tempChildCol = (TemplatedColumn)e.Row.Cells.FromKey(GRID_COL_KEY_SELECT_CHILD).Column;
    //                        CellItem cellItem = (CellItem)tempChildCol.CellItems[tempChildCol.CellItems.Count - 1];
    //                        CheckBox cbSelect = (CheckBox)cellItem.FindControl(GRID_ROW_CHECKBOX_CHILD);
    //                        cbSelect.Checked = true;
    //                    }
    //                }

    //            }

    //            if (e.Row.Cells.FromKey(GRID_COL_KEY_STTYPE).Value.ToString() == "Scanned")
    //            {
    //                e.Row.Style.BackColor = System.Drawing.Color.Green;
    //                e.Row.Style.ForeColor = System.Drawing.Color.White;
    //            }
    //            else if (e.Row.Cells.FromKey(GRID_COL_KEY_STTYPE).Value.ToString() == "Missing")
    //            {
    //                //TemplatedColumn tempChildCol = (TemplatedColumn)e.Row.Cells.FromKey(GRID_COL_KEY_SELECT_CHILD).Column;
    //                //CellItem cellItem = (CellItem)tempChildCol.CellItems[tempChildCol.CellItems.Count - 1];
    //                //CheckBox cbSelect = (CheckBox)cellItem.FindControl(GRID_ROW_CHECKBOX_CHILD);
    //                //cbSelect.Visible = false;

    //                e.Row.Style.BackColor = System.Drawing.Color.Orange;
    //                e.Row.Style.ForeColor = System.Drawing.Color.Black;


    //            }
    //            else if (e.Row.Cells.FromKey(GRID_COL_KEY_STTYPE).Value.ToString() == "Misplaced")
    //            {
    //                e.Row.Style.BackColor = System.Drawing.Color.Yellow;
    //                e.Row.Style.ForeColor = System.Drawing.Color.Black;
    //            }
    //        }
    //    }



    //}

    //private void Navigation(int totalRecords)
    //{
    //    int pagesize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());
    //    Double totalPages = Math.Ceiling(((double)totalRecords / pagesize));

    //    if ((totalRecords == 1) || (totalPages == 0))
    //    {
    //        totalPages = 1;
    //    }

    //    if (pagesize > totalRecords)
    //    {
    //        pagesize = (int)totalPages;
    //    }

    //    GoToPageTxt.Text = PageNum.ToString();
    //    CurrentPage.Text = PageNum.ToString();
    //    TotalPages.Text = totalPages.ToString();
    //    this.PageCount = Convert.ToInt32(totalPages.ToString());
    //}

    //protected void uwgAsset_PageIndexChanged(object sender, Infragistics.WebUI.UltraWebGrid.PageEventArgs e)
    //{
    //    this.CurrentPageSelections = hdnIDs.Value;
    //    this.PageNum = e.NewPageIndex;
    //    FillAsset_Details();
    //    this.hdnIDs.Value = this.CurrentPageSelections;
    //    //this.Fill_AssetDetails(e.NewPageIndex);
    //}


    //protected void GoToPageImb_Click(object sender, ImageClickEventArgs e)
    //{


    //    if (GoToPageTxt.Text != "")
    //    {
    //        try
    //        {
    //            int pagenumber = Convert.ToInt32(GoToPageTxt.Text);
    //            if (pagenumber <= 0)
    //            {
    //                ShowMessage("Enter valid page count");
    //                return;

    //            }

    //        }
    //        catch (Exception ex)
    //        {
    //            ShowMessage("Enter valid page count");
    //            return;
    //        }

    //        int maxPage = Convert.ToInt32(TotalPages.Text);
    //        int goToPage = Convert.ToInt32(GoToPageTxt.Text);

    //        if (goToPage <= maxPage)
    //        {
    //            PageNum = goToPage;
    //            FillAsset_Details();
    //            // BindGridView();
    //        }
    //        else
    //        {

    //            //need to show message
    //            ShowMessage("Invalid page count");

    //        }
    //    }
    //    else
    //    {
    //        ShowMessage("Invalid page count");
    //    }


    //}




    //public void headerCheckBox_CheckedChanged(object sender, EventArgs e)
    //{
    //    TemplatedColumn column = (TemplatedColumn)uwgAsset.Columns[0];
    //    CheckBox headerCheckBox = (CheckBox)sender;
    //    if (headerCheckBox.Checked == true)
    //    {
    //        foreach (CellItem item in column.CellItems)
    //        {
    //            CheckBox cellCheckBox = item.FindControl("CheckBoxCell") as CheckBox;
    //            cellCheckBox.Checked = true;
    //        }
    //    }
    //    else
    //    {
    //        foreach (CellItem item in column.CellItems)
    //        {
    //            CheckBox cellCheckBox = item.FindControl("CheckBoxCell") as CheckBox;
    //            cellCheckBox.Checked = false;
    //        }
    //    }
    //}

    # endregion


    protected void uwgAsset_RowIslandsPopulating(object sender, ContainerRowCancelEventArgs e)
    {
        //e.Cancel = true;

        //System.Data.DataView dv = dsInvData.Tables[1].DefaultView;
        //dv.RowFilter = "ID ='" + e.Row.DataKey[0].ToString() + "'"; // DataKey is from parent row
        //ContainerGrid child = new ContainerGrid();
        //child.AutoGenerateColumns = false;
        //child.InitializeRow += new InitializeRowEventHandler(child_InitializeRow);

        //if (e.Row.RowIslands.Count == 0)
        //{

        //    if (child.Columns.Count == 0)
        //    {
        //        TemplateDataField tChkBox = new TemplateDataField();
        //        tChkBox.ItemTemplate = new CustomItemTemplate();
        //        tChkBox.Key = "SelectChildCheckBox";
        //        tChkBox.Width = Unit.Pixel(30);
        //        tChkBox.Header.Text = "";
        //        child.Columns.Insert(0, tChkBox);

        //        //UnboundCheckBoxField cbCol = new UnboundCheckBoxField();
        //        //cbCol.Key = "ChkSelectChild";
        //        //cbCol.Width = Unit.Pixel(30);
        //        //cbCol.Header.Text = "";
        //        //child.Columns.Insert(0, cbCol);

        //        BoundDataField field1 = new BoundDataField();
        //        field1.DataFieldName = "STType";
        //        field1.Key = "STType";
        //        field1.Width = Unit.Pixel(120);
        //        field1.Header.Text = "Type";
        //        child.Columns.Add(field1);

        //        BoundDataField field2 = new BoundDataField();
        //        field2.DataFieldName = "Site";
        //        field2.Key = "Site";
        //        field2.Width = Unit.Pixel(120);
        //        field2.Header.Text = "Site";
        //        child.Columns.Add(field2);

        //        BoundDataField field3 = new BoundDataField();
        //        field3.DataFieldName = "Location";
        //        field3.Key = "Location";
        //        field3.Width = Unit.Pixel(120);
        //        field3.Header.Text = "Location";
        //        child.Columns.Add(field3);

        //        BoundDataField field4 = new BoundDataField();
        //        field4.DataFieldName = "RefNumber";
        //        field4.Key = "RefNumber";
        //        field4.Width = Unit.Pixel(120);
        //        field4.Header.Text = "SerialNo #";
        //        child.Columns.Add(field4);

        //        BoundDataField field5 = new BoundDataField();
        //        field5.DataFieldName = "AssetGroup";
        //        field5.Key = "AssetGroup";
        //        field5.Width = Unit.Pixel(120);
        //        field5.Header.Text = "AssetType";
        //        child.Columns.Add(field5);

        //        BoundDataField field6 = new BoundDataField();
        //        field6.DataFieldName = "MfgName";
        //        field6.Key = "MfgName";
        //        field6.Width = Unit.Pixel(120);
        //        field6.Header.Text = "Manufacturer";
        //        child.Columns.Add(field6);

        //        BoundDataField field7 = new BoundDataField();
        //        field7.DataFieldName = "ModelName";
        //        field7.Key = "ModelName";
        //        field7.Width = Unit.Pixel(120);
        //        field7.Header.Text = "Model";
        //        child.Columns.Add(field7);

        //        BoundDataField field9 = new BoundDataField();
        //        field9.DataFieldName = "Custodian";
        //        field9.Key = "Custodian";
        //        field9.Width = Unit.Pixel(120);
        //        field9.Header.Text = "Custodian";
        //        child.Columns.Add(field9);

        //        BoundDataField field10 = new BoundDataField();
        //        field10.DataFieldName = "ID";
        //        field10.Key = "ID";
        //        field10.Width = Unit.Pixel(0);
        //        field10.Header.Text = "";
        //        field10.Hidden = true;
        //        child.Columns.Add(field10);

        //        BoundDataField field11 = new BoundDataField();
        //        field11.DataFieldName = "AssetID";
        //        field11.Key = "AssetID";
        //        field11.Width = Unit.Pixel(0);
        //        field11.Header.Text = "";
        //        field11.Hidden = true;
        //        child.Columns.Add(field11);


        //    }
        //    e.Row.RowIslands.Add(child);
        //    child.DataSource = dv;
        //    child.DataBind();


        //}

    }

    void child_InitializeRow(object sender, RowEventArgs e)
    {
        if (e.Row.Items[1].Value.ToString() == "Scanned")
        {
            e.Row.CssClass = "Scanned";
        }

        if (e.Row.Items[1].Value.ToString() == "Missing")
        {
            e.Row.CssClass = "Missing";

            //CheckBox chkBox = (CheckBox)(e.Row.Items[0].FindControl("ChkSelectChild"));
            //chkBox.Visible = false;
        }

        if (e.Row.Items[1].Value.ToString() == "Misplaced")
        {
            e.Row.CssClass = "Misplaced";
        }


    }
    protected void uwgAsset_PreRender(object sender, EventArgs e)
    {

    }
    protected void uwgAsset_InitializeBand(object sender, BandEventArgs e)
    {

    }

    //protected void LoadData(string LocationIDs)
    //{
    //    //Creating SqlDataSource for Parent & Child Tables.
    //    SqlDataSource sdsParent = new SqlDataSource();
    //    sdsParent.ConnectionString = ConfigurationManager.ConnectionStrings["iDocTrack"].ConnectionString;
    //    sdsParent.SelectCommand = "exec iAssetTrack_sp_Inventoryupdate_data_1  '" + LocationIDs + "',1,1";

    //    SqlDataSource sdsChild = new SqlDataSource();
    //    sdsChild.ConnectionString = ConfigurationManager.ConnectionStrings["iDocTrack"].ConnectionString;
    //    sdsChild.SelectCommand = "exec iAssetTrack_sp_Inventoryupdate_data_2  '" + LocationIDs + "',1,1";

    //    //Views created for each datasource
    //    Infragistics.Web.UI.DataSourceControls.DataView dvParent = new Infragistics.Web.UI.DataSourceControls.DataView();
    //    dvParent.ID = "PARENT";
    //    dvParent.DataSource = sdsParent;

    //    Infragistics.Web.UI.DataSourceControls.DataView dvChild = new Infragistics.Web.UI.DataSourceControls.DataView();
    //    dvChild.ID = "CHILD";
    //    dvChild.DataSource = sdsChild;

    //    WebHierarchicalDataSource hdsMain = new WebHierarchicalDataSource();

    //    //Adding views to WebHierarchicalDataSource
    //    hdsMain.DataViews.Add(dvParent);
    //    hdsMain.DataViews.Add(dvChild);

    //    //Defining relation.

    //    Infragistics.Web.UI.DataSourceControls.DataRelation drMain = new Infragistics.Web.UI.DataSourceControls.DataRelation();
    //    drMain.ParentDataViewID = "PARENT";
    //    drMain.ChildDataViewID = "CHILD";
    //    drMain.ParentColumns = new string[] { "ID" };
    //    drMain.ChildColumns = new string[] { "ID" };
    //    hdsMain.DataRelations.Add(drMain);

    //    uwgAsset.DataKeyFields = "ID";
    //    uwgAsset.DataSource = hdsMain;
    //}

    //protected void Page_PreRender(object sender, EventArgs e)
    //{
    //    if (TreeLocation.CheckedNodes.Count > 0)
    //    {
    //        FillAsset_Details();
    //    }
    //}
    protected void uwgAsset_RowIslandDataBound(object sender, RowIslandEventArgs e)
    {
        //for (int x = 0; x < e.RowIsland.Columns.Count; ++x)
        //{
        //    if (e.RowIsland.Columns[x].Header.Text == "ID")
        //        e.RowIsland.Columns[x].Hidden = true;

        //    if (e.RowIsland.Columns[x].Header.Text == "AssetID")
        //        e.RowIsland.Columns[x].Hidden = true;
        //}


    }
    //protected void uwgAsset_RowCollapsed(object sender, ContainerRowEventArgs e)
    //{
    //    CheckBox chkBox = (CheckBox)e.Row.Items[0].FindControl("ChkSelect");
    //    chkBox.Enabled = false;
    //}
    //protected void uwgAsset_RowExpanded(object sender, ContainerRowEventArgs e)
    //{
    //    CheckBox chkBox = (CheckBox)e.Row.Items[0].FindControl("ChkSelect");
    //    chkBox.Enabled = true;
    //}
    protected void uwgAsset_RowIslandDataBinding(object sender, RowIslandEventArgs e)
    {
        // if (e.RowIsland.DataMember == "ChildTable"
        //&& e.RowIsland.ParentRow == this.uwgAsset.GridView.Rows[0])
        // {
        //     TemplateDataField field2 = new TemplateDataField();
        //     field2.Key = "SelectChildCheckBox";
        //     field2.Header.Text = "";
        //     this.uwgAsset.Bands[0].Columns.Insert(0, field2);

        //     TemplateDataField templateColumn2 = (TemplateDataField)this.uwgAsset.Bands[0].Columns["SelectChildCheckBox"];
        //     templateColumn2.ItemTemplate = new CustomItemTemplate();

        // }

    }
    protected void uwgAsset_DataFiltered(object sender, FilteredEventArgs e)
    {
        // Cast the WebDataGrid DataSource to a DataTable  
        DataTable dt = this.uwgAsset.DataSource as DataTable;

    }
}
class CustomItemTemplate : ITemplate
{
    #region ITemplate Members

    public void InstantiateIn(Control container)
    {

        CheckBox chkChild = new CheckBox();
        chkChild.ID = "ChkSelectChild";
        //chkChild.Attributes.Add("onclick", "javascript:checkChild();");
        //Label lblValue = new Label();
        //container.Controls.Add(lblValue);
        container.Controls.Add(chkChild);
    }

    #endregion
}

class CustomItemTemplateChecked : ITemplate
{
    #region ITemplate Members

    public void InstantiateIn(Control container)
    {

        CheckBox chkChild = new CheckBox();
        chkChild.ID = "ChkSelectChild";
        chkChild.Attributes.Add("onclick", "javascript:checkChild();");
        chkChild.Checked = true;
        container.Controls.Add(chkChild);
    }

    #endregion
}