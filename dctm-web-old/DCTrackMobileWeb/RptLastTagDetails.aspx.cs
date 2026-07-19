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
using iAssetTrack.BAL;

public partial class RptLastTagDetails : System.Web.UI.Page
{
    #region "Declarations"
    DataTable _dtRights;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        Session["PageHeader"] = "Last Printed Tag Details";
        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "RptLastTagDetails.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        if (_dtRights.Select("Module = 'Last Printed Tag Details' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }


        if (!Page.IsPostBack)
        {
            ReportParameter[] param = new ReportParameter[1];
            param[0] = new ReportParameter("ShowHPELogo", int.Parse(ConfigurationManager.AppSettings["Reports.ShowHPELogo"].ToString()) == 1 ? "true" : "false");


            string user = ConfigurationManager.AppSettings["ReportServerUser"];

            string pass = ConfigurationManager.AppSettings["ReportServerPass"];

            string domain = ConfigurationManager.AppSettings["ReportServerDomain"];

            ReportViewer1.ServerReport.ReportServerCredentials = new ReportCredentials(user, pass, domain);
            
            ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            ReportViewer1.ServerReport.ReportServerUrl = new System.Uri(ConfigurationManager.AppSettings["ReportServerUrl"]);
            ReportViewer1.ServerReport.ReportPath = ConfigurationManager.AppSettings["ReportServerFolder"] + "LastPrintTagDetails";
            ReportViewer1.ServerReport.SetParameters(param);
        }
    }

}
