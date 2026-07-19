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

public partial class RptCapacityReport : System.Web.UI.Page
{
    DataTable _dtRights;
    protected string srcSite;
    protected string dstSite;

    // J00007 by kjb on 25 Sep 2012
    # region new control declaration
    protected Label lblErrorMessage;
    protected DropDownList ddlSite, ddlSrcParentLocation, ddlRootEntity;
    protected TextBox txtLocation;
    # endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        // J00007 by kjb on 25 Sep 2012
        # region New controls initialization
        lblErrorMessage = (Label)this.wpSearchOptions.Groups[0].Items[0].FindControl("lblErrorMessage");
        ddlSite = (DropDownList)this.wpSearchOptions.Groups[0].Items[0].FindControl("ddlSite");
        ddlSrcParentLocation = (DropDownList)this.wpSearchOptions.Groups[0].Items[0].FindControl("ddlSrcParentLocation");
        ddlRootEntity = (DropDownList)this.wpSearchOptions.Groups[0].Items[0].FindControl("ddlRootEntity");
        txtLocation = (TextBox)this.wpSearchOptions.Groups[0].Items[0].FindControl("txtLocation");
        # endregion

        Session["PageHeader"] = "Capacity Report";
        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "RptCapacityReport.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        if (_dtRights.Select("Module = 'Capacity Report' and Rights = '" + "View" + "'").Length != 0)
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
            LoadSrcLocationData();
            //--- end
        }
        // added by kjb on 15 May 2012
        if (hdnLocName1 != null)
            txtLocation.Text = hdnLocName1.Value;
        //----end

    }
    protected void btnShowReport_Click(object sender, EventArgs e)
    {

        ReportParameter[] parameters = new ReportParameter[5];

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

            //string srcLoc = string.Empty;
            string srcLoc = hdnLocationID1.Value;

            // if Location (All) selected than 0 will be passed to represent all locations
            if (string.IsNullOrEmpty(srcLoc))
                srcLoc = "0";
            parameters[0] = new ReportParameter("pIntBusinessUnitID", srcBU);
            parameters[1] = new ReportParameter("pIntSiteID", srcSite);
            parameters[2] = new ReportParameter("pIntLocationID", srcLoc);
            parameters[3] = new ReportParameter("ShowHPELogo", int.Parse(ConfigurationManager.AppSettings["Reports.ShowHPELogo"].ToString()) == 1 ? "true" : "false");
            parameters[4] = new ReportParameter("pIntLoggedInUserId", Session["UserID"].ToString());

            ReportViewer1.ShowParameterPrompts = false;

            string user = ConfigurationManager.AppSettings["ReportServerUser"];

            string pass = ConfigurationManager.AppSettings["ReportServerPass"];

            string domain = ConfigurationManager.AppSettings["ReportServerDomain"];

            ReportViewer1.ServerReport.ReportServerCredentials = new ReportCredentials(user, pass, domain);

            ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            ReportViewer1.ServerReport.ReportServerUrl = new System.Uri(ConfigurationManager.AppSettings["ReportServerUrl"]);
            ReportViewer1.ServerReport.ReportPath = ConfigurationManager.AppSettings["ReportServerFolder"] + "CapacityReport";
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


    private void FillDropdownsByBU(ref DropDownList ddlRE, string ddlType)
    {

        iAssetTrack.BAL.SitesBAL objSite = new iAssetTrack.BAL.SitesBAL();
        DataSet dsSite = objSite.retrieveByBusinessUnitId(Convert.ToInt32(ddlRE.SelectedValue));
        DataTable dtSite = dsSite.Tables[0];
        iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();
        if (ddlType == "Source")
        {
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
        LoadSrcLocationData();
    }

    private void LoadSrcLocationData()
    {
        FillDropdownsByBU(ref ddlRootEntity, "Source");
        txtLocation.Text = "(All)";
        hdnLocationID1.Value = "0";
        hdnLocName1.Value = "(All)";
    }

    protected void ddlSite_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtLocation.Text = "(All)";
        hdnLocationID1.Value = "0";
        hdnLocName1.Value = "(All)";
        hdnSrcSite.Value = ddlSite.SelectedItem.Value;
        lblErrorMessage.Visible = false;
        lblErrorMessage.Text = "";
        ReportViewer1.Visible = false;
    }

}
