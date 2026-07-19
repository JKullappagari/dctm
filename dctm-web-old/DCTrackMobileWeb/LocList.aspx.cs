using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Infragistics.Web.UI.NavigationControls;
using iAssetTrack.BAL;


public partial class LocList : System.Web.UI.Page
{
    iAssetTrack.BAL.BusinessUnitBAL objBU;
    iAssetTrack.BAL.SitesBAL objSite;
    iAssetTrack.BAL.LocationBAL objLocation;
    public string header = string.Empty;
    public bool fromAssetSearch = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString.Count > 0)
        {
            string type = Request.QueryString["Type"].ToString();
            header = Request.QueryString["Header"].ToString();
            Session["PageHeader"] = header;

            if (type == "Locations")
            {
                LoadLocations();
            }
            else if (type == "Models")
            {
                string mfgName = Request.QueryString["MfgName"].ToString();
                string mfgID = Request.QueryString["MfgID"].ToString();
                string bu = Request.QueryString["BU"].ToString();

                LoadModels(mfgName, mfgID,bu);
            }
            else if (type == "RptLocations")
            {
                string site = Request.QueryString["Site"].ToString();
                FillLocationsBySite(site);
            }
            else if (type == "SearchModels")
            {
                string mfgName = Request.QueryString["MfgName"].ToString();
                string mfgID = Request.QueryString["MfgID"].ToString();
                string bu = Request.QueryString["BU"].ToString();
                fromAssetSearch = true;

                LoadModels(mfgName, mfgID, bu);
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
        // add BUs
        DataTreeNode clearNode = new DataTreeNode();
        clearNode.Text = "No Location";
        clearNode.Value = "0";
        TreeLocation.Nodes.Add(clearNode);
        using (DataTableReader dtBURdr = dtBU.CreateDataReader())
        {
            while (dtBURdr.Read())
            {

                DataTreeNode buNode = new DataTreeNode();
                buNode.Text = dtBURdr.GetValue(1).ToString();
                buNode.Value = dtBURdr.GetValue(0).ToString();
                buNode.Enabled = false;
                buNode.CssClass = ".TreeNodeDisabled";
                TreeLocation.Nodes.Add(buNode);
                DataSet dsSiteByLocation = objSite.retrieveByBusinessUnitId(Convert.ToInt32(dtBURdr.GetValue(0)));
                DataTable dtSiteByLocation = dsSiteByLocation.Tables[0];
                using (DataTableReader dtsitebyBURdr = dtSiteByLocation.CreateDataReader())
                {
                    while (dtsitebyBURdr.Read())
                    {
                        DataTreeNode siteNode = new DataTreeNode();
                        siteNode.Text = dtsitebyBURdr.GetValue(1).ToString();
                        siteNode.Value = dtsitebyBURdr.GetValue(0).ToString();
                        siteNode.Enabled = false;
                        siteNode.CssClass = ".TreeNodeDisabled";
                        buNode.Nodes.Add(siteNode);

                        objLocation.SiteID = Convert.ToInt32(dtsitebyBURdr.GetValue(0));
                        DataSet dsLocationbySite = objLocation.GetLocationBYSite();
                        DataTable dtLocationbySite = dsLocationbySite.Tables[0];

                        using (DataTableReader dtLocationbySiteRdr = dtLocationbySite.CreateDataReader())
                        {
                            while (dtLocationbySiteRdr.Read())
                            {
                                DataTreeNode LocationNode = new DataTreeNode();
                                LocationNode.Text = dtLocationbySiteRdr.GetValue(1).ToString();
                                LocationNode.Value = dtLocationbySiteRdr.GetValue(0).ToString();
                                siteNode.Nodes.Add(LocationNode);
                                if (Int32.Parse(dtLocationbySiteRdr.GetValue(2).ToString()) > 0) // child count
                                    getChildLocations(LocationNode, Convert.ToInt32(dtLocationbySiteRdr.GetValue(0).ToString()));

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
                DataTreeNode subLocationNode = new DataTreeNode();
                subLocationNode.Text = dtLocationbyLocationRdr.GetValue(1).ToString();
                subLocationNode.Value = dtLocationbyLocationRdr.GetValue(0).ToString();
                node.Nodes.Add(subLocationNode);
                if (Int32.Parse(dtLocationbyLocationRdr.GetValue(5).ToString()) > 0) // child count
                    getChildLocations(subLocationNode, Convert.ToInt32(dtLocationbyLocationRdr.GetValue(0).ToString()));
            }
        }
    }

    private void LoadModels(string MfgName,string MfgID,string BUID)
    {

        AssetModelBAL objParentAM = new iAssetTrack.BAL.AssetModelBAL();

        //ManufacturerBAL objManf = new iAssetTrack.BAL.ManufacturerBAL();
        //DataSet dsManf = objManf.retrieve();
        //DataTable dtManf = dsManf.Tables[0];

        //WebDataTree TreeLocation = (WebDataTree)ddlModelList.Items[0].FindControl("TreeLocation");
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
        //using (DataTableReader dtMFGRdr = dtManf.CreateDataReader())
        //{
        //    while (dtMFGRdr.Read())
        //    {

        DataTreeNode mfgNode = new DataTreeNode();
        mfgNode.Text = MfgName;
        mfgNode.Value = MfgID;
        mfgNode.Enabled = false;
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
                //ModelNode.Enabled = false;
                //ModelNode.CssClass = ".TreeNodeDisabled";
                mfgNode.Nodes.Add(ModelNode);
                if (Int32.Parse(dtModelByMfgRdr.GetValue(2).ToString()) > 0) // child count of this model
                    getChildModels(ModelNode, Convert.ToInt32(dtModelByMfgRdr.GetValue(0).ToString()));

            }
        }
        //    }

        //}




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
                    //ModelNode.Enabled = false;
                    //ModelNode.CssClass = ".TreeNodeDisabled";
                    node.Nodes.Add(ModelNode);
                    getChildModels(ModelNode, Convert.ToInt32(dtModelByModelRdr.GetValue(0).ToString()));
                }
            }
        }

    }

    private void FillLocationsBySite(string site)
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
        //DataTreeNode clearNode = new DataTreeNode();
        //clearNode.Text = "(All)";
        //clearNode.Value = "0";
        //TreeLocation.Nodes.Add(clearNode);


        using (DataTableReader dtLocationbySiteRdr = ds.Tables[0].CreateDataReader())
        {
            while (dtLocationbySiteRdr.Read())
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
