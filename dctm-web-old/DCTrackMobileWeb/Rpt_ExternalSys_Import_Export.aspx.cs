using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using iAssetTrackBAL;
using Microsoft.Reporting.WebForms;
using Infragistics.Web.UI.EditorControls;


public partial class Rpt_ExternalSys_Import_Export : System.Web.UI.Page
{

    DataTable _dtRights;
    ReportParameter[] parm = new ReportParameter[3];

    //J00007 by kjb on 25 Sep 2012
    # region new control declaration
    protected DropDownList ddlTranType,ddlStatus;
    protected WebDatePicker WebDatePickerDate;
    # endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        //J00007 by kjb on 25 Sep 2012
        # region New controls initialization
        ddlTranType = (DropDownList)this.wpSearchOptions.Groups[0].Items[0].FindControl("ddlTranType");
        ddlStatus = (DropDownList)this.wpSearchOptions.Groups[0].Items[0].FindControl("ddlStatus");

        WebDatePickerDate = (WebDatePicker)this.wpSearchOptions.Groups[0].Items[0].FindControl("WebDatePickerDate");

        # endregion

        Session["PageHeader"] = "Import-Export Report";
        //this.WebDatePickerDate.MaxDate = DateTime.Now;
       
        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "Rpt_ExternalSys_Import_Export.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        if (_dtRights.Select("Module = 'Import-Export Report' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }

        //ReportViewer1.Visible = false;
        if (!Page.IsPostBack)
        {
            // report viewer settings

            // fill drop down lists
            FillDropDownLists(StoredProcedures.SP_GETTRANSTYPE, ddlTranType);
            FillDropDownLists(StoredProcedures.SP_GETSTATUS, ddlStatus);
            this.WebDatePickerDate.MaxValue = DateTime.Now;
            this.WebDatePickerDate.Value = DateTime.Today;
        }
       

    }


    private void FillDropDownLists(string strStoredProc, DropDownList ddl)
    {
        //ICommon svc = (ICommon)RemotingHelper.CreateProxy(typeof(ICommon));
        iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();
        DataTable dt = objCommon.FillDropDownListSingleParam(strStoredProc, "(All)");

        ddl.DataSource = dt;
        ddl.DataTextField = dt.Columns[0].ToString();
        ddl.DataBind();
    }


    protected void btnShow_Click(object sender, EventArgs e)
    {
        this.ReportViewer1.Visible = true;
        //param 1
        parm[0] = new ReportParameter("TranType", ddlTranType.SelectedItem.Text);

        //param 2
        parm[1] = new ReportParameter("Status", ddlStatus.SelectedItem.Text);


        // parm[0] = new ReportParameter("StartDate", this.WebDatePickerStartDate.Text.Trim());
        DateTime dtStartDate = Convert.ToDateTime(this.WebDatePickerDate.Text);
        string strStartDate = dtStartDate.ToString("yyyy") + "-" + dtStartDate.ToString("MM") + "-" + dtStartDate.ToString("dd");
        parm[2] = new Microsoft.Reporting.WebForms.ReportParameter("DateOfUpdate", strStartDate);//amr StartDAte

        this.ReportViewer1.Reset();

        this.ReportViewer1.AsyncRendering = false;
        this.ReportViewer1.ShowCredentialPrompts = false;
        this.ReportViewer1.ShowParameterPrompts = false;
        this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;

        //this.ReportViewer1.ServerReport.ReportServerCredentials = new ReportCredentials("Reportfolder Name", "Password of the folder", "");
        string user = ConfigurationManager.AppSettings["ReportServerUser"];
        string pass = ConfigurationManager.AppSettings["ReportServerPass"];
        string domain = ConfigurationManager.AppSettings["ReportServerDomain"];

        ReportViewer1.ServerReport.ReportServerCredentials = new ReportCredentials(user, pass, domain);

        this.ReportViewer1.ServerReport.ReportServerUrl = new System.Uri(ConfigurationManager.AppSettings["ReportServerUrl"]);
        this.ReportViewer1.ServerReport.ReportPath = ConfigurationManager.AppSettings["ReportServerFolder"] + "Import_Export_Report";
        this.ReportViewer1.ServerReport.SetParameters(parm);
        this.ReportViewer1.ServerReport.Refresh();
        //this.ReportViewer1.Visible = true;
        parm = null;
        //wpSearchOptions.Expanded = false; // J00007 by kjb on 2 Sep 2012
        wpSearchOptions.Groups[0].Expanded = false;

    }
    protected void ReportViewer1_Drillthrough(object sender, DrillthroughEventArgs e)
    {
        //isSubReport = true;
        this.ReportViewer1.ServerReport.ReportPath = e.ReportPath;
        System.Collections.Generic.List<ReportParameterInfo> param = e.Report.GetParameters().ToList();
        System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> svrParam = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();
        svrParam.Add(new Microsoft.Reporting.WebForms.ReportParameter(param[0].Name, param[0].Values[0]));
        this.ReportViewer1.ServerReport.SetParameters(svrParam);
        this.ReportViewer1.ServerReport.DisplayName = e.Report.DisplayName;

    }


}
