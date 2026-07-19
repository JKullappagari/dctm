using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Infragistics.Web.UI.NavigationControls;
using iAssetTrack.BAL;
using iAssetTrackBAL;

public partial class TreeList : System.Web.UI.Page
{
    iAssetTrack.BAL.BusinessUnitBAL objBU;
    iAssetTrack.BAL.SitesBAL objSite;
    iAssetTrack.BAL.LocationBAL objLocation;
    public string header = string.Empty;
    public bool fromAssetSearch = false;
    private DataTable _dtRights;
    private string locType = string.Empty;
    private int disableNodeLevel = 0;
    private string tenantAssignedLocations = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "TreeList.aspx";
            Response.Redirect("Login.aspx");
        }

        if (bool.Parse(Session["TenantUser"].ToString()))
        {
            UserBAL objUser = new UserBAL();
            objUser.UserID = Convert.ToInt32(Session["UserID"]);
            DataSet dsTenant = objUser.retrieveTenantDetails();
            if(dsTenant.Tables.Count > 0 && dsTenant.Tables[0].Rows.Count > 0)
            {
                tenantAssignedLocations = dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_ASSIGNEDLOCATIONS].ToString();
            }
        }

            if (Request.QueryString.Count > 0)
        {
            string type = Request.QueryString["Type"].ToString();
            header = Request.QueryString["Header"].ToString();
            Session["PageHeader"] = header;

            disableNodeLevel = 0;
            if (!string.IsNullOrEmpty(Request.QueryString.Get("LocType")))
            {
                locType = Request.QueryString["LocType"].ToString();
                if (!string.IsNullOrEmpty(locType))
                {

                    if (locType == "Room")
                        disableNodeLevel = 1;
                    else if (locType == "Row")
                        disableNodeLevel = 2;
                    else if (locType == "Rack")
                    {
                        disableNodeLevel = 3;
                    }

                }
            }
            if (type == "Locations")
            {
                LoadLocations();
            }
            else if (type == "Models")
            {
                string mfgName = Request.QueryString["MfgName"].ToString();
                string mfgID = Request.QueryString["MfgID"].ToString();
                string bu = Request.QueryString["BU"].ToString();

                LoadModels(mfgName, mfgID, bu);
            }
            else if (type == "RptLocations")
            {
                string site = Request.QueryString["Site"].ToString();
                FillLocationsBySite(site, false);
            }
            else if (type == "CA")
            {
                string site = Request.QueryString["Site"].ToString();
                FillLocationsBySite(site, true);
            }
            else if (type == "SearchModels")
            {
                string mfgName = Request.QueryString["MfgName"].ToString();
                string mfgID = Request.QueryString["MfgID"].ToString();
                string bu = Request.QueryString["BU"].ToString();
                fromAssetSearch = true;

                LoadModels(mfgName, mfgID, bu);
            }
            else if (type == "RackModels")
            {
                string mfgName = Request.QueryString["MfgName"].ToString();
                string mfgID = Request.QueryString["MfgID"].ToString();
                string bu = Request.QueryString["BU"].ToString();

                LoadRackModels(mfgName, mfgID, bu);
            }
        }


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

        //WebDataTree TreeLocation = (WebDataTree)ddlLocationList.Items[0].FindControl("TreeLocation");
        TreeLocation.Nodes.Clear();
        TreeLocation.EnableExpandOnClick = false;
        // add BUs
        DataTreeNode clearNode = new DataTreeNode();
        clearNode.Text = "No Location";
        clearNode.Value = "0";
        clearNode.ToolTip = "No Location";
        TreeLocation.Nodes.Add(clearNode);
        //Commented on -05-Nov-2012//uncommented on 05Mar2013
        using (DataTableReader dtBURdr = dtBU.CreateDataReader())
        {
            while (dtBURdr.Read())
            {

                DataTreeNode buNode = new DataTreeNode();
                buNode.Text = dtBURdr.GetValue(1).ToString();
                buNode.Value = dtBURdr.GetValue(0).ToString();
                buNode.Expanded = true;
                buNode.Enabled = false;
                buNode.CssClass = ".TreeNodeDisabled";
                buNode.ToolTip = "Company";
                TreeLocation.Nodes.Add(buNode);
                //Added on 05-Nov-2012

                // DataTreeNode buNode = new DataTreeNode();
                // buNode.Text = Session["BU"].ToString();
                //buNode.Value = Session["BUID"].ToString();
                //buNode.Enabled = false;
                //buNode.Expanded = true;
                //buNode.CssClass = ".TreeNodeDisabled";
                //buNode.ToolTip = "Project";
                //TreeLocation.Nodes.Add(buNode);

                DataSet dsSiteByLocation = objSite.retrieveByBusinessUnitId(Convert.ToInt32(dtBURdr.GetValue(0)));
                //DataSet dsSiteByLocation = objSite.retrieveByBusinessUnitId(Convert.ToInt32(Session["BUID"]));//Commented on 05Mar2013
                DataTable dtSiteByLocation = dsSiteByLocation.Tables[0];
                using (DataTableReader dtsitebyBURdr = dtSiteByLocation.CreateDataReader())
                {
                    while (dtsitebyBURdr.Read())
                    {
                        DataTreeNode siteNode = new DataTreeNode();
                        siteNode.Text = dtsitebyBURdr.GetValue(1).ToString();
                        siteNode.Value = dtsitebyBURdr.GetValue(0).ToString();
                        //siteNode.CssClass = ".TreeNodeDisabled";
                        siteNode.Expanded = true;
                        siteNode.Enabled = false;
                        siteNode.ToolTip = "Site";
                        TreeLocation.Nodes.Add(siteNode);

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
                                        LocationNode.ToolTip = "Room";
                                        siteNode.Nodes.Add(LocationNode);
                                        if (!string.IsNullOrEmpty(Request.QueryString.Get("LocType")))
                                        {
                                            if (LocationNode.Level == disableNodeLevel) // indicates room
                                            {
                                                LocationNode.CssClass = ".TreeNodeDisabled";
                                                LocationNode.Enabled = false;
                                                LocationNode.Expanded = true;
                                            }
                                        }
                                        if (Int32.Parse(dtLocationbySiteRdr.GetValue(2).ToString()) > 0) // child count
                                            getChildLocations(LocationNode, Convert.ToInt32(dtLocationbySiteRdr.GetValue(0).ToString()));
                                    }
                                }
                                else
                                {
                                    DataTreeNode LocationNode = new DataTreeNode();
                                    LocationNode.Text = dtLocationbySiteRdr.GetValue(1).ToString();
                                    LocationNode.Value = dtLocationbySiteRdr.GetValue(0).ToString();
                                    LocationNode.ToolTip = "Room";
                                    siteNode.Nodes.Add(LocationNode);
                                    if (!string.IsNullOrEmpty(Request.QueryString.Get("LocType")))
                                    {
                                        if (LocationNode.Level == disableNodeLevel) // indicates room
                                        {
                                            LocationNode.CssClass = ".TreeNodeDisabled";
                                            LocationNode.Enabled = false;
                                            LocationNode.Expanded = true;
                                        }
                                    }
                                    if (Int32.Parse(dtLocationbySiteRdr.GetValue(2).ToString()) > 0) // child count
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
                        if (!string.IsNullOrEmpty(Request.QueryString.Get("LocType")))
                        {
                            if (subLocationNode.Level >= disableNodeLevel)
                            {
                                subLocationNode.CssClass = ".TreeNodeDisabled";
                                subLocationNode.Enabled = false;
                                subLocationNode.Expanded = true;
                            }
                        }
                        if (Int32.Parse(dtLocationbyLocationRdr.GetValue(5).ToString()) > 0) // child count
                            getChildLocations(subLocationNode, Convert.ToInt32(dtLocationbyLocationRdr.GetValue(0).ToString()));
                    }
                }
                else
                {
                    DataTreeNode subLocationNode = new DataTreeNode();
                    subLocationNode.Text = dtLocationbyLocationRdr.GetValue(1).ToString();
                    subLocationNode.Value = dtLocationbyLocationRdr.GetValue(0).ToString();
                    node.Nodes.Add(subLocationNode);
                    if (!string.IsNullOrEmpty(Request.QueryString.Get("LocType")))
                    {
                        if (subLocationNode.Level >= disableNodeLevel)
                        {
                            subLocationNode.CssClass = ".TreeNodeDisabled";
                            subLocationNode.Enabled = false;
                            subLocationNode.Expanded = true;
                        }
                    }
                    if (Int32.Parse(dtLocationbyLocationRdr.GetValue(5).ToString()) > 0) // child count
                        getChildLocations(subLocationNode, Convert.ToInt32(dtLocationbyLocationRdr.GetValue(0).ToString()));
                }
            }
        }
    }

    private void LoadModels(string MfgName, string MfgID, string BUID)
    {

        AssetModelBAL objParentAM = new iAssetTrack.BAL.AssetModelBAL();
        TreeLocation.Nodes.Clear();
        // add Manufacturers
        DataTreeNode clearNode = new DataTreeNode();
        if (fromAssetSearch)
        {
            clearNode.Text = "(All)";
            clearNode.Value = "0";
        }
        else
        {
            clearNode.Text = "No Model";
            clearNode.Value = "0";
        }
        TreeLocation.Nodes.Add(clearNode);
        DataTreeNode mfgNode = new DataTreeNode();
        mfgNode.Text = MfgName;
        mfgNode.Value = MfgID;
        mfgNode.Enabled = false;
        mfgNode.Expanded = true;
        mfgNode.CssClass = ".TreeNodeDisabled";
        TreeLocation.Nodes.Add(mfgNode);
        DataSet dsParentAM = objParentAM.retrieveByMfg(Int32.Parse(MfgID),
                                int.Parse(BUID));
        DataTable dtParentAM = dsParentAM.Tables[0];
        using (DataTableReader dtModelByMfgRdr = dtParentAM.CreateDataReader())
        {
            while (dtModelByMfgRdr.Read())
            {
                DataTreeNode ModelNode = new DataTreeNode();
                ModelNode.Text = dtModelByMfgRdr.GetValue(1).ToString();
                ModelNode.Value = dtModelByMfgRdr.GetValue(0).ToString();
                ModelNode.Key = dtModelByMfgRdr.GetValue(2).ToString();
                mfgNode.Nodes.Add(ModelNode);
                //if (Int32.Parse(dtModelByMfgRdr.GetValue(2).ToString()) > 0) // child count of this model
                //getChildModels(ModelNode, Convert.ToInt32(dtModelByMfgRdr.GetValue(0).ToString()));
            }
        }
    }

    private void LoadRackModels(string MfgName, string MfgID, string BUID)
    {

        AssetModelBAL objParentAM = new iAssetTrack.BAL.AssetModelBAL();
        TreeLocation.Nodes.Clear();
        // add Manufacturers
        DataTreeNode clearNode = new DataTreeNode();
        if (fromAssetSearch)
        {
            clearNode.Text = "(All)";
            clearNode.Value = "0";
        }
        else
        {
            clearNode.Text = "No Model";
            clearNode.Value = "0";
        }
        TreeLocation.Nodes.Add(clearNode);
        DataTreeNode mfgNode = new DataTreeNode();
        mfgNode.Text = MfgName;
        mfgNode.Value = MfgID;
        mfgNode.Enabled = false;
        mfgNode.Expanded = true;
        mfgNode.CssClass = ".TreeNodeDisabled";
        TreeLocation.Nodes.Add(mfgNode);

        int rackTypeId = GetRackTypeId();
        
        DataSet dsParentAM = objParentAM.retrieveByMfgAndType(Int32.Parse(MfgID), rackTypeId,int.Parse(BUID));
        DataTable dtParentAM = dsParentAM.Tables[0];
        using (DataTableReader dtModelByMfgRdr = dtParentAM.CreateDataReader())
        {
            while (dtModelByMfgRdr.Read())
            {
                DataTreeNode ModelNode = new DataTreeNode();
                ModelNode.Text = dtModelByMfgRdr.GetValue(1).ToString();
                ModelNode.Value = dtModelByMfgRdr.GetValue(0).ToString();
                ModelNode.Key = dtModelByMfgRdr.GetValue(2).ToString();
                mfgNode.Nodes.Add(ModelNode);
            }
        }
    }

    private static int GetRackTypeId()
    {
        int rackTypeId = 0;
        AssetGroupBAL objAssetGroup = new AssetGroupBAL();
        DataSet dsAssetGroup = objAssetGroup.retrieve();
        if (dsAssetGroup.Tables.Count > 0 && dsAssetGroup.Tables[0].Rows.Count > 0)
        {
            DataTable dtAssetGroup = dsAssetGroup.Tables[0];
            foreach (DataRow dr in dtAssetGroup.Rows)
            {
                if (dr[DBFields.DBFIELD_ASSETGROUP].ToString().ToLower().CompareTo("rack") == 0)
                {
                    rackTypeId = Convert.ToInt32(dr[DBFields.DBFIELD_ASSETGROUPID].ToString());
                    break;
                }
            }
        }
        return rackTypeId;
    }

    private void getChildModels(DataTreeNode node, int ModelID)
    {
        AssetModelBAL objChildAM = new AssetModelBAL();
        DataSet dsChildAM = objChildAM.retrieveByParentModelID(ModelID);
        DataTable dtChildAM = dsChildAM.Tables[0];
        if (dtChildAM.Rows.Count > 0)
        {
            using (DataTableReader dtModelByModelRdr = dtChildAM.CreateDataReader())
            {
                while (dtModelByModelRdr.Read())
                {
                    DataTreeNode ModelNode = new DataTreeNode();
                    ModelNode.Text = dtModelByModelRdr.GetValue(1).ToString();
                    ModelNode.Value = dtModelByModelRdr.GetValue(0).ToString();
                    ModelNode.Key = dtModelByModelRdr.GetValue(2).ToString();
                    //node.Nodes.Add(ModelNode);
                    //getChildModels(ModelNode, Convert.ToInt32(dtModelByModelRdr.GetValue(0).ToString()));
                }
            }
        }

    }

    private void FillLocationsBySite(string site, bool isCreateAsset)
    {
        //ddlLocation.Items.Clear();
        int intSiteID = 0;
        if (site != "")
            intSiteID = Convert.ToInt32(site);
        objLocation = new iAssetTrack.BAL.LocationBAL();
        objLocation.SiteID = intSiteID;
        DataSet ds = objLocation.GetLocationBYSite();


        //WebDataTree TreeLocation = (WebDataTree)ddlLocation.Items[0].FindControl("TreeLoc");
        TreeLocation.Nodes.Clear();
        // add BUs
        DataTreeNode clearNode = new DataTreeNode();
        clearNode.Text = "(All)";
        clearNode.Value = "0";
        if (isCreateAsset)
        {
            clearNode.CssClass = ".TreeNodeDisabled";
            clearNode.Enabled = false;
        }

        TreeLocation.Nodes.Add(clearNode);


        using (DataTableReader dtLocationbySiteRdr = ds.Tables[0].CreateDataReader())
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
                        if(!tenantAssignedLocations.Contains(dtLocationbySiteRdr.GetValue(0).ToString()))
                        {
                            //tenant assigned list not contains this location 
                            //node will be disabled and expanded
                            LocationNode.Expanded = true;
                            LocationNode.Enabled = false;

                        }
                        TreeLocation.Nodes.Add(LocationNode);
                        if (Int32.Parse(dtLocationbySiteRdr.GetValue(2).ToString()) > 0)
                            getChildLocations(LocationNode, Convert.ToInt32(dtLocationbySiteRdr.GetValue(0).ToString()));
                    }
                }
                else
                {

                    DataTreeNode LocationNode = new DataTreeNode();
                    LocationNode.Text = dtLocationbySiteRdr.GetValue(1).ToString();
                    LocationNode.Value = dtLocationbySiteRdr.GetValue(0).ToString();
                    TreeLocation.Nodes.Add(LocationNode);
                    if (Int32.Parse(dtLocationbySiteRdr.GetValue(2).ToString()) > 0)
                        getChildLocations(LocationNode, Convert.ToInt32(dtLocationbySiteRdr.GetValue(0).ToString()));
                }

            }
        }



    }


}
