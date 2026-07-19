
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Script.Services;
using System.Globalization;
using System.Text;
using iAssetTrack.BAL;
using Infragistics.UltraGauge.Resources;
using System.Web.Services;
using iAssetTrack.DALC;
using System.Collections.Generic;
//using iDocTrack.Remoting.Shared;

public partial class iAssetTrackMasterPage : System.Web.UI.MasterPage
{

    #region Page Load Events
    /// <summary>
    /// Setting application mode and loading user rights.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {

        Dictionary<string, string> dic = null;
        try
        {
            if (Application["Sessions"] != null)
            {
                dic = ((Dictionary<string, string>)Application["Sessions"]);

                if (dic.ContainsKey(Session["UserName"].ToString().ToLower()))
                {
                    if (dic[Session["UserName"].ToString().ToLower()].CompareTo(HttpContext.Current.Session.SessionID.ToLower()) != 0)
                    {
                        //If Session id stored in Application[Sessions] for this user is different than this session id is belonged to earlier login 
                        // so page will be redirected to login page.
                        Response.Redirect("Login.aspx");
                    }

                }
                else
                {
                    //If User Name is not in Application[Sessions] than redirect to login.apsx
                    Response.Redirect("Login.aspx");
                }
            }

        }
        catch
        {

        }

        Session["UserID"] = Session["LoggedInUser"];
        //if ((Session["PageHeader"] != null) || (Session["PageHeader"].ToString() != "Home"))
        if (Session["PageHeader"] != null)
        {
            lblPageName.Text = Session["PageHeader"].ToString();
        }

        //this.ClickSite.Attributes.Add("onclick", "OpenSite()");
        //RegisterJScript("~/scripts/ajax.js");
        if (!Page.IsPostBack)
        {
            string mode = ConfigurationManager.AppSettings["Mode"];
            if ((Convert.ToInt32(mode) == 1) || (Convert.ToInt32(mode) == 2))
            {
                if (Convert.ToInt32(mode) == 1)
                {
                    //lblSite.Visible=true;
                    //ddlSite.Visible=true;
                    //  linlCentral.Visible=false;
                    // (Redundant) FillSiteList();
                    //ClickSite.Visible=true;

                }
                else
                {
                    //lblSite.Visible=false;
                    //ddlSite.Visible=false;
                    //ClickSite.Visible=false;
                    //   linlCentral.Visible=true;
                    //string path=ConfigurationManager.AppSettings["CentralWebAppRootPathUrl"];
                    //    hdnCentral.Value=path;


                }

            }
            else
            {
                //lblSite.Visible=false;
                //ddlSite.Visible=false;
                //   linlCentral.Visible=false;
                //ClickSite.Visible=false;
            }
        }

        if (Session["UserID"] != null)  //Session["UserID"] != null ||
        {
            this.LoadRights();
            //SessionManagerBAL objSession = new SessionManagerBAL((int)Session["UserID"]);
            //Session["Rights"] = (DataTable)objSession.Rights;  
            //this.lblUser.Text = "Welcome  " + Session["UserID"].ToString() ;
            //Session["PageUser"] = "Welcome  " + Session["UserName"].ToString();
            Session["PageUser"] = Session["UserName"].ToString();
            //commented by kjb on 19th July 2011
            //SegmentedDigitalGauge gauge = (SegmentedDigitalGauge)UltraGauge1.Gauges[0];
            //gauge.Text = DateTime.Now.Hour.ToString().PadLeft(2, '0') + ":" + DateTime.Now.Minute.ToString().PadLeft(2, '0') + ":" + DateTime.Now.Second.ToString().PadLeft(2, '0');
        }
        else
        {
            try
            {
                string[] objurl = Request.ServerVariables["URL"].Split('/');
                //Response.Write(objurl[3].ToString());
                Session["RedirectUrl"] = objurl[3].ToString();
                Response.Redirect("Login.aspx");
            }
            catch { Response.Redirect("Login.aspx"); }
        }
        if (Session["PageHeader"] != null)
        {
        }
        if (Session["PageUser"] != null)
        {
            lblUser.Text = Session["PageUser"].ToString();
        }

        System.Web.HttpContext.Current.Session.Timeout = Convert.ToInt32(System.Web.HttpContext.Current.Session["TimoutInMin"].ToString()) + 1;
        this.KeepAliveNew3();
    }
    #endregion

    /// <summary>
    /// Load rights based on application mode and user.
    /// </summary>
    private void LoadRights()
    {
        string mode = ConfigurationManager.AppSettings["Mode"];

        if (Convert.ToInt32(mode) != 3)
        {
            SessionManagerBAL objSession = new SessionManagerBAL((int)Session["UserID"]);
            Session["Rights"] = (DataTable)objSession.Rights;
        }
        //else
        //{
        //    ICommon svc = (ICommon)RemotingHelper.CreateProxy(typeof(ICommon));
        //    Session["Rights"] = svc.LoadRights((int)Session["UserID"]);

        //}
    }

    /// <summary>
    /// Register external Jscript file (.js)
    /// </summary>
    /// <param name="pathToJScriptFile">path and file name of JScript file, eg. scripts/ajax.js</param>
    public void RegisterJScript(string pathToJScriptFile)
    {
        string key = "script::" + pathToJScriptFile.ToLower();
        if (!Page.ClientScript.IsClientScriptBlockRegistered(key))
        {
            string script = string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", pathToJScriptFile);
            this.Page.ClientScript.RegisterClientScriptBlock(typeof(string), key, script);
        }

    }

    /// <summary>
    /// Appending javascript for session timeout alert.
    /// </summary>
    private void KeepAlive()
    {

        int int_millisecondstimeout = Convert.ToInt32(Session["TimoutInMin"].ToString()) * 60000;
        Session["TargetPage"] = Request.ServerVariables["URL"];

        System.Text.StringBuilder str_script = new StringBuilder();
        str_script.Append("<script type='text/javascript'>");
        //Number of Reconnects
        str_script.Append("var count=0;");
        str_script.Append("var handle=null;");
        //Maximum reconnects setting
        str_script.Append("var max = 5;");
        str_script.Append("function Reconnect(){");
        //str_script.Append("alert('calling reconnect...');");
        str_script.Append("count++;");
        str_script.Append("if (count < max) ");
        str_script.Append("{");

        //str_script.Append("window.status = 'Link to Server Refreshed ' + count.toString()+' time(s)' ;");

        //str_script.Append("var img = new Image(1,1);");
        str_script.Append("var winUrl = 'Reconnect.aspx';");
        str_script.Append("var winSettings = 'center:yes;location:no;resizable:no;dialogWidth:350px;dialogHeight:125px;scroll:no;status:no;help:no';");
        str_script.Append("var args = window.showModalDialog(winUrl, args, winSettings);");
        str_script.Append("if (args != null) ");
        str_script.Append("{");
        // enable only when the current page requires reload.
        //str_script.Append("window.location.reload();");
        // reset the new time
        //str_script.Append("window.alert(args[0].toString());");
        str_script.Append("window.clearInterval(handle);");
        str_script.Append("handle=window.setInterval('Reconnect()', args[0] );");
        //str_script.Append("window.setInterval('Reconnect()',  args[0]);");

        str_script.Append(" }");
        str_script.Append("else ");
        str_script.Append("{");

        //string sUrl = Utility.GetUrlRootPath() + "Logoutme.aspx";
        str_script.Append("window.clearInterval(handle);");
        string sUrl = "Logoutme.aspx";
        str_script.Append("window.location.href='" + sUrl + "';");
        str_script.Append(" }");
        str_script.Append(" }");
        str_script.Append(" }");
        str_script.Append("handle=window.setInterval('Reconnect()', " + int_millisecondstimeout.ToString() + " );");
        //str_script.Append("window.setInterval('Reconnect()', " + int_millisecondstimeout.ToString() + " );");

        //Set to length required
        str_script.Append("</script>");
        Page.ClientScript.RegisterStartupScript(typeof(Page), "reconnect", str_script.ToString());

    }

    protected void btnSTReload_Click(object sender, EventArgs e)
    {
        System.Web.HttpContext.Current.Session.Timeout = Convert.ToInt32(System.Web.HttpContext.Current.Session["TimoutInMin"].ToString()) + 1;
    }

    protected void btnForceLogout_Click(object sender, EventArgs e)
    {
        string mode = ConfigurationManager.AppSettings["Mode"];
        this.AuditLogout();
        if (Convert.ToInt32(mode) != 3)
        {
            Session.Abandon();
            Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddYears(-30);
        }
        if (Convert.ToInt32(mode) == 3)
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }

    }

    /// <summary>
    /// Audit logout information
    /// </summary>
    /// 
    private void AuditLogout()
    {
        string mode = ConfigurationManager.AppSettings["Mode"];
        if (Convert.ToInt32(mode) != 3)
        {
            AuditLoginLogoutBAL objAudit = new AuditLoginLogoutBAL();
            objAudit.UserID = Convert.ToInt32(Session["UserID"]);
            objAudit.AuditType = 1;
            objAudit.IP = Request.ServerVariables["REMOTE_ADDR"];
            objAudit.Persist(DALCOperation.Insert);
        }
    }

    private void KeepAliveNew3()
    {

        int int_millisecondstimeout = Convert.ToInt32(Session["TimoutInMin"].ToString()) * 60000;
        Session["TargetPage"] = Request.ServerVariables["URL"];
        string sUrl = "Logoutme.aspx";

        System.Text.StringBuilder str_script = new StringBuilder();
        str_script.Append("function updateSessionTime()");
        str_script.Append("{");
        str_script.Append("document.getElementById('" + btnSTReload.ClientID.ToString() + "').click();");

        str_script.Append("}");

        str_script.Append("var nCount = 0;");
        str_script.Append("var interval = null;");
        str_script.Append("var count=0;");
        str_script.Append("var handle=null;");

        str_script.Append("var max = 5;"); ;
        str_script.Append(" var args = new Array('" + int_millisecondstimeout.ToString() + "');");
        str_script.Append("function windowRefresh() {");
        str_script.Append("self.focus();");
        str_script.Append("nCount--;");
        str_script.Append("  if (nCount <= 0) {");
        str_script.Append("window.clearInterval(interval);");
        str_script.Append(" nCount = 0;");
        str_script.Append(" args = null;");
        str_script.Append("validNavigation = true;");
        str_script.Append(" $('#tDialog').dialog('close');");
        str_script.Append("window.location.href='" + sUrl + "';");
        str_script.Append(" }");
        str_script.Append("  else {");
        str_script.Append("  $('#tDialog span').text(nCount.toString());");
        str_script.Append("}");
        str_script.Append("  }");
        //Maximum reconnects setting
        str_script.Append(" $('#tDialog').dialog({");
        str_script.Append("modal: true,");
        str_script.Append("autoOpen: false,");
        str_script.Append("buttons : {");
        str_script.Append(" 'Yes' : function() {");
        str_script.Append("args =  new Array('" + int_millisecondstimeout.ToString() + "');");
        str_script.Append("validNavigation = true;");
        str_script.Append(" $(this).dialog('close');");
        str_script.Append(" updateSessionTime();");
        str_script.Append(" },");
        str_script.Append("'No' : function() {");
        str_script.Append("args = null;");
        str_script.Append("validNavigation = true;");
        str_script.Append("$(this).dialog('close');");
        str_script.Append(" }},");
        str_script.Append("open: function () {");
        str_script.Append("nCount = 60;");
        str_script.Append("interval = window.setInterval('windowRefresh()', 1000);");
        str_script.Append("  },");
        str_script.Append(" close: function (e) {");
        //str_script.Append("$(this).empty();");
        //str_script.Append(" $(this).dialog('destroy');");
        str_script.Append("nCount = 0;");
        str_script.Append(Environment.NewLine);
        str_script.Append("if (args != null) ");
        str_script.Append("{");
        str_script.Append("window.clearInterval(handle);");
        str_script.Append("handle=window.setInterval('Reconnect()', args[0] );");
        str_script.Append(" }");
        str_script.Append("else ");
        str_script.Append("{");
        str_script.Append("window.clearInterval(handle);");

        str_script.Append("window.location.href='" + sUrl + "';");
        str_script.Append(" }");
        str_script.Append("},");
        str_script.Append(" height: 175,");
        str_script.Append(" width: 450,");
        str_script.Append(" resizable: false,");
        str_script.Append("title: 'Session Timeout',");
        str_script.Append("disableAnimation: !!window.MSInputMethodContext && !!document.documentMode");
        str_script.Append(" });");
        str_script.Append("function Reconnect(){");
        str_script.Append("count++;");
        str_script.Append("if (count < max) ");
        str_script.Append("{");

        str_script.Append("$('#tDialog').dialog('open');");
        str_script.Append(" }");
        str_script.Append(" }");
        str_script.Append("handle=window.setInterval('Reconnect()', " + int_millisecondstimeout.ToString() + " );");

        Page.ClientScript.RegisterStartupScript(typeof(string), "reconnect", str_script.ToString(), true);
    }




    /// <summary>
    /// Site click transferring to central.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ClickSite_Click(object sender, ImageClickEventArgs e)
    {
    }

    protected void UltraGauge1_AsyncRefresh(object sender, Infragistics.WebUI.UltraWebGauge.RefreshEventArgs e)
    {
        //commented by kjb on 19th July 2011
        //SegmentedDigitalGauge gauge = (SegmentedDigitalGauge)UltraGauge1.Gauges[0];

        //gauge.Text = DateTime.Now.Hour.ToString().PadLeft(2, '0') + ":" + DateTime.Now.Minute.ToString().PadLeft(2, '0') + ":" + DateTime.Now.Second.ToString().PadLeft(2, '0');
        //int val = Convert.ToInt32(gauge.Text);
        //val++;
        //gauge.Text = val.ToString();

    }
}

