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
using iAssetTrack.BAL;

public partial class RptTransactionsList : System.Web.UI.Page
{
    #region "Declarations"
    private iAssetTrack.BAL.BusinessUnitBAL objBU;
    private iAssetTrack.BAL.SitesBAL objSite;
    private iAssetTrack.BAL.LocationBAL objLocation;
    string strSNo;
    DataTable _dtRights;
    private string tenantAssignedLocations = string.Empty;
    #endregion

    // J00007 by kjb on 26 Sep 2012
    # region new control declaration
    protected Label lblMessage;
    protected DropDownList ddlBusinessUnit, ddlPrimarySite, ddlSite, ddlMfg, ddlTransType;
    protected WebDataTree TreeLocation;
    protected TextBox txtSNo;
    protected WebDatePicker WebDatePickerStartDate, WebDatePickerEndDate;
    # endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        // J00007 by kjb on 26 Sep 2012
        # region New controls initialization
        lblMessage = (Label)this.wpSearchOptions.Groups[0].Items[0].FindControl("lblMessage");
        ddlBusinessUnit = (DropDownList)this.wpSearchOptions.Groups[0].Items[0].FindControl("ddlBusinessUnit");
        ddlPrimarySite = (DropDownList)this.wpSearchOptions.Groups[0].Items[0].FindControl("ddlPrimarySite");
        ddlMfg = (DropDownList)this.wpSearchOptions.Groups[0].Items[0].FindControl("ddlMfg");
        ddlTransType = (DropDownList)this.wpSearchOptions.Groups[0].Items[0].FindControl("ddlTransType");

        txtSNo = (TextBox)this.wpSearchOptions.Groups[0].Items[0].FindControl("txtSNo");

        TreeLocation = (WebDataTree)this.wpSearchOptions.Groups[0].Items[0].FindControl("TreeLocation");
        WebDatePickerStartDate = (WebDatePicker)this.wpSearchOptions.Groups[0].Items[0].FindControl("WebDatePickerStartDate");
        WebDatePickerEndDate = (WebDatePicker)this.wpSearchOptions.Groups[0].Items[0].FindControl("WebDatePickerEndDate");
        CompareValidator cvEndDate2 = (CompareValidator)this.wpSearchOptions.Groups[0].Items[0].FindControl("cvEndDate2");

        # endregion



        cvEndDate2.ValueToCompare = DateTime.Now.Date.ToShortDateString();
        Session["PageHeader"] = "Asset Transaction Report";
        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "RptTransactionsList.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        if (_dtRights.Select("Module = 'Asset Transaction Report' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
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
            FillDropDownLists(StoredProcedures.SP_GETTRANSACTIONTYPE, ddlTransType);
            LoadLocations();
        }
        lblMessage.Enabled = false;
        lblMessage.Text = string.Empty;

        // cvStartDate.ValueToCompare =

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

        foreach (DataTreeNode node in TreeLocation.CheckedNodes)
        {
            // level = 0 indicates BU, level = 1 indicates Site and level 2,3,4.... indicates locations
            // and sub locations
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

        if (string.IsNullOrEmpty(txtSNo.Text))
        {
            strSNo = "NULL";
        }
        else
        {
            strSNo = txtSNo.Text;
        }

        ReportParameter[] param = new ReportParameter[7];
        param[0] = new ReportParameter("StartDate", strStartDate);
        param[1] = new ReportParameter("EndDate", strEndDate);
        param[2] = new ReportParameter("SNo", strSNo);
        param[3] = new ReportParameter("LocList", locList.Trim(','));
        param[4] = new ReportParameter("TransType", ddlTransType.SelectedItem.Value);
        param[5] = new ReportParameter("ShowHPELogo", int.Parse(ConfigurationManager.AppSettings["Reports.ShowHPELogo"].ToString()) == 1 ? "true" : "false");
        param[6] = new ReportParameter("pIntLoggedInUserId", Session["UserID"].ToString());

        string user = ConfigurationManager.AppSettings["ReportServerUser"];
        string pass = ConfigurationManager.AppSettings["ReportServerPass"];
        string domain = ConfigurationManager.AppSettings["ReportServerDomain"];
        ReportViewer1.ServerReport.ReportServerCredentials = new ReportCredentials(user, pass, domain);


        ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
        ReportViewer1.ServerReport.ReportServerUrl = new System.Uri(ConfigurationManager.AppSettings["ReportServerUrl"]);
        ReportViewer1.ServerReport.ReportPath = ConfigurationManager.AppSettings["ReportServerFolder"] + "TransactionList";
        ReportViewer1.ServerReport.SetParameters(param);
        ReportViewer1.ServerReport.Refresh();
        ReportViewer1.Visible = true;
        param = null;

        this.wpSearchOptions.Groups[0].Expanded = false;
    }
}
