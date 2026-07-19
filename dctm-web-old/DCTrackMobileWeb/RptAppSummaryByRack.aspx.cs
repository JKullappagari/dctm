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

public partial class RptAppSummaryByRack : System.Web.UI.Page
{
    DataTable _dtRights;
    protected string srcSite;
    protected string dstSite;

    // J00007 by kjb on 25 Sep 2012
    # region new control declaration
    protected Label lblErrorMessage;
    protected DropDownList ddlSite, ddlApplication, ddlRootEntity, ddlAppCriticality;
    protected TextBox txtLocation;
    # endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        // J00007 by kjb on 25 Sep 2012
        # region New controls initialization
        lblErrorMessage = (Label)this.wpSearchOptions.Groups[0].Items[0].FindControl("lblErrorMessage");
        ddlSite = (DropDownList)this.wpSearchOptions.Groups[0].Items[0].FindControl("ddlSite");
        ddlAppCriticality = (DropDownList)this.wpSearchOptions.Groups[0].Items[0].FindControl("ddlAppCriticality");
        ddlApplication = (DropDownList)this.wpSearchOptions.Groups[0].Items[0].FindControl("ddlApplication");
        ddlRootEntity = (DropDownList)this.wpSearchOptions.Groups[0].Items[0].FindControl("ddlRootEntity");
        txtLocation = (TextBox)this.wpSearchOptions.Groups[0].Items[0].FindControl("txtLocation");
        # endregion

        Session["PageHeader"] = "App Summary - Rack";
        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "RptAppSummaryByRack.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        if (_dtRights.Select("Module = 'App Summary - Rack' and Rights = '" + "View" + "'").Length != 0)
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
            if (ddlRootEntity.Items.Count > 1)
                ddlRootEntity.SelectedIndex = 1;
            ddlRootEntity.Enabled = false;
            FillDropdownsByBU(ref ddlRootEntity);
            //--- end
            //-->v3.9
            //get app criticality data
            PopulateAppCriticality();
            PopulateApplications();
        }
       

    }

    private void PopulateAppCriticality()
    {
        ApplicationCriticalityBAL objAC = new ApplicationCriticalityBAL();
        DataTable dtAC = objAC.retrieve().Tables[0];
        this.ddlAppCriticality.Items.Clear();
        //this.ddlSrcLoc.Items.Clear();
        DataRow dr = dtAC.NewRow();
        dr[0] = "0";
        dr[1] = "(All)";
        dtAC.Rows.InsertAt(dr, 0);

        ddlAppCriticality.DataTextField = dtAC.Columns[1].ColumnName;
        ddlAppCriticality.DataValueField = dtAC.Columns[0].ColumnName;
        ddlAppCriticality.DataSource = dtAC ;
        ddlAppCriticality.DataBind();
    }
    protected void btnShowReport_Click(object sender, EventArgs e)
    {

        ReportParameter[] parameters = new ReportParameter[6];

        //parameters
        try
        {
            lblErrorMessage.Visible = false;
            lblErrorMessage.Enabled = false;
            lblErrorMessage.Text = string.Empty;

            string srcBU = this.ddlRootEntity.SelectedValue;
            string srcSite = string.Empty;
            if (int.Parse(this.ddlSite.SelectedValue) == 0)
            {
                srcSite = "0";
            }
            else
            {
                srcSite = this.ddlSite.SelectedValue;
            }

            parameters[0] = new ReportParameter("BusinessUnitID", srcBU);
            parameters[1] = new ReportParameter("SiteID", srcSite);
            parameters[2] = new ReportParameter("AppCriticalityID",ddlAppCriticality.SelectedItem.Value.ToString());
            parameters[3] = new ReportParameter("AppID", ddlApplication.SelectedItem.Value.ToString());
            parameters[4] = new ReportParameter("ShowHPELogo", int.Parse(ConfigurationManager.AppSettings["Reports.ShowHPELogo"].ToString()) == 1 ? "true" : "false");
            parameters[5] = new ReportParameter("pIntLoggedInUserId", Session["UserID"].ToString());

            ReportViewer1.ShowParameterPrompts = false;

            string user = ConfigurationManager.AppSettings["ReportServerUser"];

            string pass = ConfigurationManager.AppSettings["ReportServerPass"];

            string domain = ConfigurationManager.AppSettings["ReportServerDomain"];

            ReportViewer1.ServerReport.ReportServerCredentials = new ReportCredentials(user, pass, domain);

            ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            ReportViewer1.ServerReport.ReportServerUrl = new System.Uri(ConfigurationManager.AppSettings["ReportServerUrl"]);
            ReportViewer1.ServerReport.ReportPath = ConfigurationManager.AppSettings["ReportServerFolder"] + "AppSummary";
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


    private void FillDropdownsByBU(ref DropDownList ddlRE)
    {

        iAssetTrack.BAL.SitesBAL objSite = new iAssetTrack.BAL.SitesBAL();
        DataSet dsSite = objSite.retrieveByBusinessUnitId(Convert.ToInt32(ddlRE.SelectedValue));
        DataTable dtSite = dsSite.Tables[0];
        iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();

        this.ddlSite.Items.Clear();
        //this.ddlSrcLoc.Items.Clear();
        DataRow dr = dtSite.NewRow();
        dr[0] = "0";
        dr[1] = "(All)";
        dtSite.Rows.InsertAt(dr, 0);

        ddlSite.DataTextField = dtSite.Columns[1].ColumnName;
        ddlSite.DataValueField = dtSite.Columns[0].ColumnName;
        ddlSite.DataSource = dtSite;
        ddlSite.DataBind();

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
        //FillDropDownList(StoredProcedures.SP_ASSETGROUP_LIST, ref ddlAssetGroup);
        //if (ddlAssetGroup.Items.Count > 0)
        //    ddlAssetGroup.SelectedIndex = 0;
        int intUserID = 0;
        if (Session["UserID"] != null)
            intUserID = Convert.ToInt32(Session["UserID"].ToString());

        FillDropDownListBU(StoredProcedures.SP_BUSINESSUNIT_LISTBYUSERID, ref ddlRootEntity, intUserID);



    }

    protected void ddlRootEntity_SelectedIndexChanged(object sender, EventArgs e)
    {
    }


    protected void ddlSite_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlAppCriticality_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateApplications();
    }

    private void PopulateApplications()
    {
        ApplicationBAL objApp = new ApplicationBAL();
        DataTable dtApp = objApp.retrieveByAppCriticality(int.Parse(ddlAppCriticality.SelectedItem.Value.ToString())).Tables[0];
        this.ddlApplication.Items.Clear();
        //this.ddlSrcLoc.Items.Clear();
        DataRow dr = dtApp.NewRow();
        dr[0] = "0";
        dr[2] = "(All)";
        dtApp.Rows.InsertAt(dr, 0);

        ddlApplication.DataTextField = dtApp.Columns[2].ColumnName;
        ddlApplication.DataValueField = dtApp.Columns[0].ColumnName;
        ddlApplication.DataSource = dtApp;
        ddlApplication.DataBind();
    }



}
