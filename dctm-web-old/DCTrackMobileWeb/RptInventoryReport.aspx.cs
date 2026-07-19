using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Infragistics.Web.UI.NavigationControls;
using System.Configuration;
using iAssetTrackBAL;
using Microsoft.Reporting.WebForms;
using Infragistics.Web.UI.EditorControls;
public partial class RptInventoryReport : System.Web.UI.Page
{
    #region "Declarations"
    private iAssetTrack.BAL.BusinessUnitBAL objBU;
    private iAssetTrack.BAL.SitesBAL objSite;
    private iAssetTrack.BAL.LocationBAL objLocation;
    DataTable _dtRights;
    bool showHistoricaldata = false;
    #endregion

    // J00007 by kjb on 25 Sep 2012
    # region new control declaration
    protected Label lblMessage;
    protected WebDataTree TreeLocation;
    protected WebDatePicker WebDatePickerStartDate, WebDatePickerEndDate;
    protected DropDownList ddlBusinessUnit, ddlPrimarySite, ddlSite, ddlMfg, ddlAssetType;
    protected RadioButton rbHistory;
    # endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        // J00007 by kjb on 26 Sep 2012
        # region New controls initialization
        lblMessage = (Label)this.wpSearchOptions.Groups[0].Items[0].FindControl("lblMessage");
        ddlBusinessUnit = (DropDownList)this.wpSearchOptions.Groups[0].Items[0].FindControl("ddlBusinessUnit");
        ddlPrimarySite = (DropDownList)this.wpSearchOptions.Groups[0].Items[0].FindControl("ddlPrimarySite");
        ddlMfg = (DropDownList)this.wpSearchOptions.Groups[0].Items[0].FindControl("ddlMfg");
        ddlAssetType = (DropDownList)this.wpSearchOptions.Groups[0].Items[0].FindControl("ddlAssetType");

        TreeLocation = (WebDataTree)this.wpSearchOptions.Groups[0].Items[0].FindControl("TreeLocation");
        rbHistory = (RadioButton)this.wpSearchOptions.Groups[0].Items[0].FindControl("rbHistory");

        WebDatePickerStartDate = (WebDatePicker)this.wpSearchOptions.Groups[0].Items[0].FindControl("WebDatePickerStartDate");
        WebDatePickerEndDate = (WebDatePicker)this.wpSearchOptions.Groups[0].Items[0].FindControl("WebDatePickerEndDate");
        CompareValidator cvEndDate2 = (CompareValidator)this.wpSearchOptions.Groups[0].Items[0].FindControl("cvEndDate2");
        # endregion
        
        cvEndDate2.ValueToCompare = DateTime.Now.Date.ToShortDateString();
        Session["PageHeader"] = "Audit Report";
        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "RptInventoryReport.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        if (_dtRights.Select("Module = 'Audit Report' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }
        if (!IsPostBack)
        {
            //FillDropDownLists(StoredProcedures.SP_GETTRANSACTIONTYPE, ddlTransType);
            LoadLocations();
            //this.WebDatePickerStartDate.MaxValue = DateTime.Now;
            //this.WebDatePickerStartDate.Value = DateTime.Today;
        }
        lblMessage.Enabled = false;
        lblMessage.Text = string.Empty;

        //cvStartDate.ValueToCompare =

        //DateTime.Now.ToShortDateString();

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
                if (int.Parse(dtLocationbyLocationRdr.GetValue(5).ToString()) > 0) //Child location count
                    getChildLocations(subLocationNode, Convert.ToInt32(dtLocationbyLocationRdr.GetValue(0).ToString()));
            }
        }
    }
    private void FillDropDownLists(string strStoredProc, DropDownList ddl)
    {

        iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();
        DataTable dt = objCommon.FillDropDownLst(strStoredProc);

        ddl.DataSource = dt;
        ddl.DataTextField = dt.Columns[1].ToString();
        ddl.DataValueField = dt.Columns[0].ToString();
        ddl.DataBind();
    }


    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        string locList = string.Empty;
        // ReportViewer1.ServerReport.Refresh();

        foreach (DataTreeNode node in TreeLocation.CheckedNodes)
        {
            // level = 0 indicates BU, level = 1 indicates Site and level 2,3,4.... indicates locations
            // and sub locations
            //if (node.Level == 1)
            //{

            //}
            if (node.Level >= 2)
            {
                locList = locList + node.Value + ",";
            }


        }
        if (string.IsNullOrEmpty(locList))
        {
            locList = "NULL";
        }

        DateTime dtStartDate = Convert.ToDateTime(this.WebDatePickerStartDate.Text);
        string strStartDate = dtStartDate.ToString("yyyy") + "-" + dtStartDate.ToString("MM") + "-" + dtStartDate.ToString("dd");

        DateTime dtEndDate = Convert.ToDateTime(this.WebDatePickerEndDate.Text);
        string strEndDate = dtEndDate.ToString("yyyy") + "-" + dtEndDate.ToString("MM") + "-" + dtEndDate.ToString("dd");

        //Historical/latest data
        if (rbHistory.Checked)
        {
            showHistoricaldata = true;
        }
        else
        {
            showHistoricaldata = false;
        }

        //if (string.IsNullOrEmpty(txtSNo.Text))
        //{
        //    strSNo = "NULL";
        //}
        //else
        //{
        //    strSNo = txtSNo.Text;
        //}

        //if (!string.IsNullOrEmpty(locList))
        //{
        ReportParameter[] param = new ReportParameter[6];
        param[0] = new ReportParameter("StartDate", strStartDate);
        param[1] = new ReportParameter("EndDate", strEndDate);
        param[2] = new ReportParameter("LocList", locList.Trim(','));
        param[3] = new ReportParameter("HistoricalData", Convert.ToInt32(showHistoricaldata).ToString());
        param[4] = new ReportParameter("ShowHPELogo", int.Parse(ConfigurationManager.AppSettings["Reports.ShowHPELogo"].ToString()) == 1 ? "true" : "false");
        param[5] = new ReportParameter("pIntLoggedInUserId", Session["UserID"].ToString());

        string user = ConfigurationManager.AppSettings["ReportServerUser"];

        string pass = ConfigurationManager.AppSettings["ReportServerPass"];

        string domain = ConfigurationManager.AppSettings["ReportServerDomain"];

        ReportViewer1.ServerReport.ReportServerCredentials = new ReportCredentials(user, pass, domain);


        ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
        ReportViewer1.ServerReport.ReportServerUrl = new System.Uri(ConfigurationManager.AppSettings["ReportServerUrl"]);
        ReportViewer1.ServerReport.ReportPath = ConfigurationManager.AppSettings["ReportServerFolder"] + "InventoryReport";
        ReportViewer1.ServerReport.SetParameters(param);
        ReportViewer1.ServerReport.Refresh();
        ReportViewer1.Visible = true;
        param = null;

        //wpSearchOptions.Expanded = false;//J00007 by kjb on 25 Sep 2012
        wpSearchOptions.Groups[0].Expanded = false;
        //}
        //else
        //{
        //    ReportViewer1.Visible = false;
        //    lblMessage.Text = "Select atleast one location";
        //    lblMessage.Enabled = true;
        //}


    }
}
