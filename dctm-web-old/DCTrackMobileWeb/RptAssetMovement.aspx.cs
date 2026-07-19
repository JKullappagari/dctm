using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Configuration;
using iAssetTrackBAL;
using System.Data;
using Infragistics.Web.UI.NavigationControls;
using iAssetTrack.BAL;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Infragistics.Web.UI.EditorControls;

public partial class RptAssetMovement : System.Web.UI.Page
{
    DataTable _dtRights;
    protected string srcSite;
    protected string dstSite;

    // J00007 by kjb on 25 Sep 2012
    # region new control declaration
    protected Label lblErrorMessage;
    protected DropDownList ddlSrcSite, ddlDstSite, ddlSrcParentLocation, ddlDstParentLocation,ddlSrcRootEntity,ddlDstRootEntity,ddlAssetGroup;
    protected TextBox txtSrcParentLocation,txtDstParentLocation;
    protected CheckBox chkShowInTransit;
    protected WebDatePicker WebDatePickerStartDate, WebDatePickerEndDate;
    # endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        // J00007 by kjb on 25 Sep 2012
        # region New controls initialization
        lblErrorMessage = (Label)this.wpSearchOptions.Groups[0].Items[0].FindControl("lblErrorMessage");
        ddlSrcSite = (DropDownList)this.wpSearchOptions.Groups[0].Items[0].FindControl("ddlSrcSite");
        ddlDstSite = (DropDownList)this.wpSearchOptions.Groups[0].Items[0].FindControl("ddlDstSite");
        ddlSrcParentLocation = (DropDownList)this.wpSearchOptions.Groups[0].Items[0].FindControl("ddlSrcParentLocation");
        ddlDstParentLocation = (DropDownList)this.wpSearchOptions.Groups[0].Items[0].FindControl("ddlDstParentLocation");
        ddlSrcRootEntity = (DropDownList)this.wpSearchOptions.Groups[0].Items[0].FindControl("ddlSrcRootEntity");
        ddlDstRootEntity = (DropDownList)this.wpSearchOptions.Groups[0].Items[0].FindControl("ddlDstRootEntity");
        ddlAssetGroup = (DropDownList)this.wpSearchOptions.Groups[0].Items[0].FindControl("ddlAssetGroup");
        txtSrcParentLocation = (TextBox)this.wpSearchOptions.Groups[0].Items[0].FindControl("txtSrcParentLocation");
        txtDstParentLocation = (TextBox)this.wpSearchOptions.Groups[0].Items[0].FindControl("txtDstParentLocation");
        chkShowInTransit = (CheckBox)this.wpSearchOptions.Groups[0].Items[0].FindControl("chkShowInTransit");

        WebDatePickerStartDate = (WebDatePicker)this.wpSearchOptions.Groups[0].Items[0].FindControl("WebDatePickerStartDate");
        WebDatePickerEndDate = (WebDatePicker)this.wpSearchOptions.Groups[0].Items[0].FindControl("WebDatePickerEndDate");
        CompareValidator cvEndDate2 = (CompareValidator)this.wpSearchOptions.Groups[0].Items[0].FindControl("cvEndDate2");
        # endregion
        cvEndDate2.ValueToCompare = DateTime.Now.Date.ToShortDateString();
        Session["PageHeader"] = "Asset Movement Report";
        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "RptAssetMovement.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        if (_dtRights.Select("Module = 'Asset Movement Report' and Rights = '" + "View" + "'").Length != 0)
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
            InitializeDropDownLists();

            // added by kjb on 15 May 2012
            // to show bu/company by default.
            if (ddlSrcRootEntity.Items.Count > 1)
                ddlSrcRootEntity.SelectedIndex = 1;
            ddlSrcRootEntity.Enabled = false;
            if (ddlDstRootEntity.Items.Count > 1)
                ddlDstRootEntity.SelectedIndex = 1;
            ddlDstRootEntity.Enabled = false;
            LoadSrcLocationData();
            LoadDstLocationData();
            //--- end
            //((((((((((((((((((((((((((((((
            //ddlSrcLoc.SelectedItemIndex = 0;
            //ddlDstLoc.SelectedItemIndex = 0;
            //))))))))))))))))))))))))))))))))
        }
        // added by kjb on 15 May 2012
        if (hdnLocName1 != null)
            txtSrcParentLocation.Text = hdnLocName1.Value;
        if (hdnLocName2 != null)
            txtDstParentLocation.Text = hdnLocName2.Value;
        //----end

    }
    protected void btnShowReport_Click(object sender, EventArgs e)
    {

        ReportParameter[] parameters = new ReportParameter[12];

        //parameters
        try
        {
            lblErrorMessage.Visible = false;
            lblErrorMessage.Enabled = false;
            lblErrorMessage.Text = string.Empty;

            DateTime dtStartDate = Convert.ToDateTime(this.WebDatePickerStartDate.Text);
            string strStartDate = dtStartDate.ToString("yyyy") + "-" + dtStartDate.ToString("MM") + "-" + dtStartDate.ToString("dd");

            DateTime dtEndDate = Convert.ToDateTime(this.WebDatePickerEndDate.Text);
            string strEndDate = dtEndDate.ToString("yyyy") + "-" + dtEndDate.ToString("MM") + "-" + dtEndDate.ToString("dd");

            string srcBU = this.ddlSrcRootEntity.SelectedValue;
            string srcSite = string.Empty;
            if (int.Parse(this.ddlSrcSite.SelectedValue) == 0)
            {
                srcSite = GetValueList(ddlSrcSite);
            }
            else
            {
                srcSite = this.ddlSrcSite.SelectedValue;
            }

            //string srcLoc = string.Empty;
            string srcLoc = hdnLocationID1.Value;
            //))))))))))))))))))))))))))))))))))))))))))))

            //if (ddlSrcLoc.CurrentValue != null)
            //{
            //    //if (ddlSrcLoc.SelectedItemIndex > 0 || Utility.ISNumeric(ddlSrcLoc.CurrentValue))
            //    //    srcLoc = Utility.ISNumeric(ddlSrcLoc.CurrentValue) ? ddlSrcLoc.CurrentValue : ddlSrcLoc.SelectedValue;
            //    if (ddlSrcLoc.SelectedItemIndex > 0)
            //        srcLoc = ddlSrcLoc.SelectedItem.Value;
            //}

            //)))))))))))))))))))))))))))))))))))))))))))



            //if (int.Parse(this.ddlSrcLoc.SelectedValue) == 0)
            //{
            //    srcLoc = GetLocValueList(ddlSrcLoc, true);
            //}
            //else
            //{
            //    srcLoc = GetLocValueList(ddlSrcLoc, false);
            //}

            string dstBU = this.ddlDstRootEntity.SelectedValue;
            string dstSite = string.Empty;

            if (int.Parse(this.ddlDstSite.SelectedValue) == 0)
            {
                dstSite = GetValueList(ddlDstSite);
            }
            else
            {
                dstSite = this.ddlDstSite.SelectedValue;
            }

            //string dstLoc = string.Empty;
            string dstLoc = hdnLocationID2.Value;

            //((((((((((((((((((((((((((((((((((((((((((((
            //if (ddlDstLoc.CurrentValue != null)
            //{
            //    //if (ddlDstLoc.SelectedItemIndex > 0 || Utility.ISNumeric(ddlDstLoc.CurrentValue))
            //    //    dstLoc = Utility.ISNumeric(ddlDstLoc.CurrentValue) ? ddlDstLoc.CurrentValue : ddlDstLoc.SelectedValue;
            //    if (ddlDstLoc.SelectedItemIndex > 0)
            //        dstLoc = ddlDstLoc.SelectedItem.Value;
            //}
            //)))))))))))))))))))))))))))))))))))))))))))))


            //if (int.Parse(this.ddlDstLoc.SelectedValue) == 0)
            //{
            //    dstLoc = GetLocValueList(ddlDstLoc, true);
            //}
            //else
            //{
            //    dstLoc = GetLocValueList(ddlDstLoc, false);
            //}
            string assetType = string.Empty;
            if (int.Parse(this.ddlAssetGroup.SelectedValue) == 0)
            {
                assetType = "0";
            }
            else
            {
                assetType = this.ddlAssetGroup.SelectedValue;
            }
            bool transitOnly = this.chkShowInTransit.Checked;
            // if Location (All) selected than 0 will be passed to represent all locations
            if (string.IsNullOrEmpty(srcLoc))
                srcLoc = "0";
            if (string.IsNullOrEmpty(dstLoc))
                dstLoc = "0";


            parameters[0] = new ReportParameter("StartDate", strStartDate);
            parameters[1] = new ReportParameter("EndDate", strEndDate);
            parameters[2] = new ReportParameter("srcBU", srcBU);
            parameters[3] = new ReportParameter("srcSite", srcSite);
            parameters[4] = new ReportParameter("srcLoc", srcLoc);
            parameters[5] = new ReportParameter("dstBU", dstBU);
            parameters[6] = new ReportParameter("dstSite", dstSite);
            parameters[7] = new ReportParameter("dstLoc", dstLoc);
            parameters[8] = new ReportParameter("AssetType", assetType);
            parameters[9] = new ReportParameter("TransitOnly", transitOnly.ToString());
            parameters[10] = new ReportParameter("ShowHPELogo", int.Parse(ConfigurationManager.AppSettings["Reports.ShowHPELogo"].ToString()) == 1 ? "true":"false");
            parameters[11] = new ReportParameter("pIntLoggedInUserId", Session["UserID"].ToString());

            ReportViewer1.ShowParameterPrompts = false;
            //ReportViewer1.ServerReport.ReportServerCredentials = new ReportCredentials("Reportfolder Name", "Password of the folder", "");

            string user = ConfigurationManager.AppSettings["ReportServerUser"];

            string pass = ConfigurationManager.AppSettings["ReportServerPass"];

            string domain = ConfigurationManager.AppSettings["ReportServerDomain"];

            ReportViewer1.ServerReport.ReportServerCredentials = new ReportCredentials(user, pass, domain);

            ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            ReportViewer1.ServerReport.ReportServerUrl = new System.Uri(ConfigurationManager.AppSettings["ReportServerUrl"]);
            ReportViewer1.ServerReport.ReportPath = ConfigurationManager.AppSettings["ReportServerFolder"] + "AssetMovement";
            ReportViewer1.ServerReport.SetParameters(parameters);
            ReportViewer1.ServerReport.Refresh();
            ReportViewer1.Visible = true;
            //wpSearchOptions.Expanded = false;// J00007 by kjb on 25 Sep 2012
            wpSearchOptions.Groups[0].Expanded = false;

        }
        catch (Exception ex)
        {
            ExceptionPolicy.HandleException(ex, "errDCTrack");
            lblErrorMessage.Visible = true;
            lblErrorMessage.Enabled = true;
            lblErrorMessage.Text = "Error in processing report parameters";
        }
        finally
        {
            if (parameters != null)
                parameters = null;
        }



    }

    private string GetValueList(DropDownList ddl)
    {
        string val = string.Empty;
        foreach (ListItem selVal in ddl.Items)
        {
            if (int.Parse(selVal.Value) != 0)
            {
                val = val + selVal.Value + ",";
            }
        }

        return val.Trim(',');
    }

    //private string GetLocValueList(Infragistics.Web.UI.ListControls.WebDropDown ddl, bool isAll)
    //{
    //    string locList = string.Empty;
    //    WebDataTree lt = (WebDataTree)ddl.Items[0].FindControl("TreeLoc");
    //    if (isAll)
    //    {
    //        foreach (DataTreeNode node in lt.Nodes)
    //        {
    //            //if (node.Level >= 2)
    //            //{
    //            locList = locList + node.Value + ",";
    //            //}
    //        }
    //    }
    //    else
    //    {
    //        foreach (DataTreeNode node in lt.SelectedNodes)
    //        {
    //            //if (node.Level >= 2)
    //            //{
    //            locList = locList + node.Value + ",";
    //            //}
    //        }
    //    }

    //    return locList;
    //}

    private void FillDropdownsByBU(ref DropDownList ddlRE, string ddlType)
    {

        iAssetTrack.BAL.SitesBAL objSite = new iAssetTrack.BAL.SitesBAL();
        DataSet dsSite = objSite.retrieveByBusinessUnitId(Convert.ToInt32(ddlRE.SelectedValue));
        DataTable dtSite = dsSite.Tables[0];
        iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();
        if (ddlType == "Source")
        {
            this.ddlSrcSite.Items.Clear();
            //this.ddlSrcLoc.Items.Clear();
            DataRow dr = dtSite.NewRow();
            dr[0] = "0";
            dr[1] = "(All)";
            dtSite.Rows.InsertAt(dr, 0);

            ddlSrcSite.DataTextField = dtSite.Columns[1].ColumnName;
            ddlSrcSite.DataValueField = dtSite.Columns[0].ColumnName;
            ddlSrcSite.DataSource = dtSite;
            ddlSrcSite.DataBind();

        }
        else
        {
            this.ddlDstSite.Items.Clear();
            //this.ddlDstLoc.Items.Clear();
            //if (dtSite.Rows.Count > 0)
            //    objCommon.setDataSource(this.ddlDstSite, dtSite, "(All)");
            DataRow dr = dtSite.NewRow();
            dr[0] = "0";
            dr[1] = "(All)";
            dtSite.Rows.InsertAt(dr, 0);

            ddlDstSite.DataTextField = dtSite.Columns[1].ColumnName;
            ddlDstSite.DataValueField = dtSite.Columns[0].ColumnName;
            ddlDstSite.DataSource = dtSite;
            ddlDstSite.DataBind();

            //FillLocationsBySite("Dest");
        }



    }

    //private void FillLocationsBySite()
    //{
    //    ddlLocation.Items.Clear();
    //    int intSiteID = 0;
    //    if (ddlPrimarySite.SelectedValue != "")
    //        intSiteID = Convert.ToInt32(ddlPrimarySite.SelectedValue);
    //    objLocation = new iAssetTrack.BAL.LocationBAL();
    //    objLocation.SiteID = intSiteID;
    //    DataSet ds = objLocation.GetLocationBYSite();

    //    ListItem itm = new ListItem("(All)", "0");
    //    ddlLocation.Items.Add(itm);

    //    ddlLocation.DataSource = ds;
    //    DataTable dt = ds.Tables[0];
    //    ddlLocation.DataValueField = dt.Columns["LocationID"].ToString();
    //    ddlLocation.DataTextField = dt.Columns["Location"].ToString();
    //    ddlLocation.DataBind();


    //}
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

    private void FillDropDownListBU(string strStoredProc, ref DropDownList ddl, int id)
    {

        iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();

        DataTable dt = objCommon.FillDropDownListBU(strStoredProc, "-- Select --", id);
        ddl.DataSource = dt;
        ddl.DataValueField = dt.Columns[0].ToString();
        ddl.DataTextField = dt.Columns[1].ToString();
        ddl.DataBind();

    }


    private void InitializeDropDownLists()
    {
        FillDropDownList(StoredProcedures.SP_ASSETGROUP_LIST, ref ddlAssetGroup);
        if (ddlAssetGroup.Items.Count > 0)
            ddlAssetGroup.SelectedIndex = 0;
        int intUserID = 0;
        if (Session["UserID"] != null)
            intUserID = Convert.ToInt32(Session["UserID"].ToString());

        FillDropDownListBU(StoredProcedures.SP_BUSINESSUNIT_LISTBYUSERID, ref ddlSrcRootEntity, intUserID);
        FillDropDownListBU(StoredProcedures.SP_BUSINESSUNIT_LISTBYUSERID, ref ddlDstRootEntity, intUserID);

        //if (this.ddlSrcRootEntity.Items.Count > 1)
        //{
        //    this.ddlSrcRootEntity.SelectedIndex = 1;
        //}

        //if (this.ddlDstRootEntity.Items.Count > 1)
        //{
        //    this.ddlDstRootEntity.SelectedIndex = 1;
        //}

        //FillDropdownsByBU(ref ddlSrcRootEntity, "Source");
        //FillDropdownsByBU(ref ddlDstRootEntity, "Dest");

        //if (this.ddlSrcSite.Items.Count > 1)
        //{
        //    this.ddlSrcSite.SelectedIndex = 1;
        //}

        //if (this.ddlDstSite.Items.Count > 1)
        //{
        //    this.ddlDstSite.SelectedIndex = 1;
        //}

        //FillLocationsBySite(ref ddlSrcSite, "Source");
        //FillLocationsBySite(ref ddlDstSite, "Dest");

    }


    //((((((((((((((((((((((((((((((((((((((((((((((((((((((((
    //private void FillLocationsBySite(string ddlType)
    //{
    //    int intSiteID = 0;
    //    objLocation = new iAssetTrack.BAL.LocationBAL();

    //    if (ddlType == "Source")
    //    {
    //        if (ddlSrcSite.SelectedIndex > 0)
    //            intSiteID = Convert.ToInt32(ddlSrcSite.SelectedValue);
    //        objLocation.SiteID = intSiteID;
    //        DataSet ds = objLocation.GetLocationBYSite();


    //       // WebDataTree lt = (WebDataTree)ddlSrcLoc.Items[0].FindControl("TreeSrcLoc");
    //        lt.Nodes.Clear();
    //        // add BUs
    //        DataTreeNode clearNode = new DataTreeNode();
    //        clearNode.Text = "(All)";
    //        clearNode.Value = "0";
    //        lt.Nodes.Add(clearNode);


    //        using (DataTableReader dtLocationbySiteRdr = ds.Tables[0].CreateDataReader())
    //        {
    //            while (dtLocationbySiteRdr.Read())
    //            {
    //                DataTreeNode LocationNode = new DataTreeNode();
    //                LocationNode.Text = dtLocationbySiteRdr.GetValue(1).ToString();
    //                LocationNode.Value = dtLocationbySiteRdr.GetValue(0).ToString();
    //                lt.Nodes.Add(LocationNode);
    //                if (Int32.Parse(dtLocationbySiteRdr.GetValue(2).ToString()) > 0)
    //                    getChildLocations(LocationNode, Convert.ToInt32(dtLocationbySiteRdr.GetValue(0).ToString()));

    //            }
    //        }

    //    }
    //    else
    //    {
    //        if (ddlDstSite.SelectedIndex > 0)
    //            intSiteID = Convert.ToInt32(ddlDstSite.SelectedValue);
    //        objLocation.SiteID = intSiteID;
    //        DataSet ds = objLocation.GetLocationBYSite();


    //        WebDataTree lt = (WebDataTree)ddlDstLoc.Items[0].FindControl("TreeDstLoc");
    //        lt.Nodes.Clear();
    //        // add BUs
    //        DataTreeNode clearNode = new DataTreeNode();
    //        clearNode.Text = "(All)";
    //        clearNode.Value = "0";
    //        lt.Nodes.Add(clearNode);


    //        using (DataTableReader dtLocationbySiteRdr = ds.Tables[0].CreateDataReader())
    //        {
    //            while (dtLocationbySiteRdr.Read())
    //            {
    //                DataTreeNode LocationNode = new DataTreeNode();
    //                LocationNode.Text = dtLocationbySiteRdr.GetValue(1).ToString();
    //                LocationNode.Value = dtLocationbySiteRdr.GetValue(0).ToString();
    //                lt.Nodes.Add(LocationNode);
    //                if (Int32.Parse(dtLocationbySiteRdr.GetValue(2).ToString()) > 0)
    //                    getChildLocations(LocationNode, Convert.ToInt32(dtLocationbySiteRdr.GetValue(0).ToString()));

    //            }
    //        }

    //    }
    //}

    //))))))))))))))))))))))))))))))))))))))))))))))))))))


    //private void getChildLocations(DataTreeNode node, int LocationID)
    //{
    //    DataSet dsLocationbyLocation = objLocation.GetLocationBYLocation(LocationID);
    //    DataTable dtLocationbyLocation = dsLocationbyLocation.Tables[0];

    //    if (dtLocationbyLocation.Rows.Count > 0)
    //    {
    //        using (DataTableReader dtLocationbyLocationRdr = dtLocationbyLocation.CreateDataReader())
    //        {
    //            while (dtLocationbyLocationRdr.Read())
    //            {
    //                DataTreeNode subLocationNode = new DataTreeNode();
    //                subLocationNode.Text = dtLocationbyLocationRdr.GetValue(1).ToString();
    //                subLocationNode.Value = dtLocationbyLocationRdr.GetValue(0).ToString();
    //                node.Nodes.Add(subLocationNode);
    //                if (Int32.Parse(dtLocationbyLocationRdr.GetValue(5).ToString()) > 0) // sixth column which will return child loc count
    //                    getChildLocations(subLocationNode, Convert.ToInt32(dtLocationbyLocationRdr.GetValue(0).ToString()));
    //            }
    //        }
    //    }
    //}



    protected void ddlSrcRootEntity_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSrcLocationData();
    }

    private void LoadSrcLocationData()
    {
        FillDropdownsByBU(ref ddlSrcRootEntity, "Source");
        txtSrcParentLocation.Text = "(All)";
        hdnLocationID1.Value = "0";
        hdnLocName1.Value = "(All)";
    }

    protected void ddlSrcSite_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtSrcParentLocation.Text = "(All)";
        hdnLocationID1.Value = "0";
        hdnLocName1.Value = "(All)";
        hdnSrcSite.Value = ddlSrcSite.SelectedItem.Value;
    }
    protected void ddlDstRootEntity_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDstLocationData();
    }

    private void LoadDstLocationData()
    {
        FillDropdownsByBU(ref ddlDstRootEntity, "Desti");
        txtDstParentLocation.Text = "(All)";
        hdnLocationID2.Value = "0";
        hdnLocName2.Value = "(All)";

    }


    protected void ddlDstSite_SelectedIndexChanged(object sender, EventArgs e)
    {
        //txtSrcParentLocation.Text = hdnLocName1.Value;
        //  FillLocationsBySite("Dest");
        txtDstParentLocation.Text = "(All)";
        hdnLocationID2.Value = "0";
        hdnLocName2.Value = "(All)";
        hdnDstSite.Value = ddlDstSite.SelectedItem.Value;

    }





    //(((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((
    //protected void ddlSrcLoc_ValueChanged(object sender, Infragistics.Web.UI.ListControls.DropDownValueChangedEventArgs e)
    //{
    //    int locationID = 0;
    //    LocationBAL objLoc = new LocationBAL();
    //    if (ddlSrcLoc.SelectedItemIndex > 0 || Utility.ISNumeric(ddlSrcLoc.CurrentValue))
    //        locationID = Convert.ToInt32(Utility.ISNumeric(ddlSrcLoc.CurrentValue) ? ddlSrcLoc.CurrentValue : ddlSrcLoc.SelectedValue);
    //    objLoc.LocationID = locationID;
    //    DataSet ds = objLoc.retrieve();
    //    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
    //    {
    //        DataRow dr = ds.Tables[0].Rows[0];

    //        Infragistics.Web.UI.ListControls.DropDownItem locationItem = new Infragistics.Web.UI.ListControls.DropDownItem();
    //        locationItem.Text = dr[DBFields.DBFIELD_LOCATION].ToString();
    //        locationItem.Value = dr[DBFields.DBFIELD_LOCATIONID].ToString();
    //        ddlSrcLoc.Items.Insert(1, locationItem);
    //        ddlSrcLoc.SelectedItemIndex = 1;

    //    }

    //}
    //protected void ddlDstLoc_ValueChanged(object sender, Infragistics.Web.UI.ListControls.DropDownValueChangedEventArgs e)
    //{
    //    int locationID = 0;

    //    LocationBAL objLoc = new LocationBAL();
    //    if (ddlDstLoc.SelectedItemIndex > 0 || Utility.ISNumeric(ddlDstLoc.CurrentValue))
    //        locationID = Convert.ToInt32(Utility.ISNumeric(ddlDstLoc.CurrentValue) ? ddlDstLoc.CurrentValue : ddlDstLoc.SelectedValue);

    //    objLoc.LocationID = locationID;
    //    DataSet ds = objLoc.retrieve();
    //    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
    //    {
    //        DataRow dr = ds.Tables[0].Rows[0];

    //        Infragistics.Web.UI.ListControls.DropDownItem locationItem = new Infragistics.Web.UI.ListControls.DropDownItem();
    //        locationItem.Text = dr[DBFields.DBFIELD_LOCATION].ToString();
    //        locationItem.Value = dr[DBFields.DBFIELD_LOCATIONID].ToString();
    //        ddlDstLoc.Items.Insert(1, locationItem);
    //        ddlDstLoc.SelectedItemIndex = 1;

    //    }

    //}
    //))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))



    //private DataTreeNode getNode(DataTreeNode dt,string id)
    //{
    //    DataTreeNode dtNode=null;
    //    foreach (DataTreeNode node in dt.Nodes)
    //    {
    //        if (node.HasChildren)
    //        {
    //            dtNode = 
    //        }
    //    }
    //    return dtNode;
    //}

    protected void ddlAssetGroup_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    //protected void ddlSrcLoc_SelectionChanged(object sender, Infragistics.Web.UI.ListControls.DropDownSelectionChangedEventArgs e)
    //{

    //}
}
