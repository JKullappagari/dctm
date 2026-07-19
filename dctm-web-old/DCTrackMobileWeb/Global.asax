<%@ Import Namespace="System.Runtime.Remoting" %>
<%@ Application Language="C#" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="System.Configuration" %>
<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        RemotingConfiguration.Configure(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, false);
        SiteMap.SiteMapResolve += new SiteMapResolveEventHandler(AppendQueryString);

        InitializeSessionCount();
        App_Code.LicenseData data = App_Code.DecryptLicenses.GetTenantData();
        if(data != null)
        {
            Application["TenantCount"] = data.colSettings.nooftenants;
            Application["CompanyName"] = data.colSettings.companyname;
            DateTime dt = DateTime.ParseExact(data.colSettings.licensevalidthru, "ddMMyyyy", CultureInfo.InvariantCulture);
            Application["ValidThru"] = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            Application["LicenseMode"] = data.colSettings.licensemode;

        }

        //OpenPrinterConnections();

    }

    void Application_End(object sender, EventArgs e)
    {
        //ClosePrinterConnections();
    }

    void Application_Error(object sender, EventArgs e)
    {
        try
        {
            if (Server.GetLastError() != null)
            {
                if (Server.GetLastError().InnerException == null)
                {
                    Application["ErrorClass"] = Server.GetLastError();
                }
                else
                {
                    Application["ErrorClass"] = Server.GetLastError().InnerException;
                }
                Server.ClearError();
                if ((this.Context.Handler as Page).MasterPageFile.ToString().ToLower().Contains("popup"))
                {
                    Server.Transfer("~/GeneralErrorPopUp.aspx");
                }
                else
                {
                    Server.Transfer("~/GeneralError.aspx");
                }
            }
            //Response.Redirect("../GeneralError.aspx");
            //ClosePrinterConnections();
        }
        catch (Exception ex)
        {
            Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicy.HandleException(ex, "errDCTrack");
        }
    }

    void Session_Start(object sender, EventArgs e)
    {
        //make sure Asp.net_SessionID is secure.
        if (Request.IsSecureConnection)
        {
            Response.Cookies["ASP.NET_SessionId"].Secure = true;
        }

        // Redirect mobile users to the mobile site
        HttpRequest httpRequest = HttpContext.Current.Request;
        if (httpRequest.Browser.IsMobileDevice)
        {
            string path = httpRequest.Url.PathAndQuery;
            bool isOnMobilePage = path.StartsWith("/Mobile/",
                                                  StringComparison.OrdinalIgnoreCase);
            if (!isOnMobilePage)
            {
                string redirectTo = "~/Mobile/MobileLogin.aspx";
                HttpContext.Current.Response.Redirect(redirectTo);
            }
            Session["TimoutInMin"] = System.Configuration.ConfigurationManager.AppSettings["TimoutInMin"].ToString();
            Session.Timeout = Convert.ToInt32(Session["TimoutInMin"].ToString()) + 3;

        }
        else
        {
            IncrementSessionsCount();
            //OpenPrinterConnections();
            // Code that runs when a new session is started 
            LoadUser();
            Session["TimoutInMin"] = System.Configuration.ConfigurationManager.AppSettings["TimoutInMin"].ToString();
            Session.Timeout = Convert.ToInt32(Session["TimoutInMin"].ToString()) + 3;
        }
    }






    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

        DecrementSessionsCount();
        if (Session != null)
        {
            //if (!String.IsNullOrEmpty(Session["WorkflowInstanceId"].ToString()))
            //{
            //    //WorkflowUtil.CheckAndDeleteWorkflow(Session["WorkflowInstanceId"].ToString());
            //}
        }

    }


    void Session_Error(object sender, EventArgs e)
    {
        DecrementSessionsCount();
        if (Session != null)
        {
            if (!String.IsNullOrEmpty(Session["WorkflowInstanceId"].ToString()))
            {
                //WorkflowUtil.CheckAndDeleteWorkflow(Session["WorkflowInstanceId"].ToString());
            }
        }
        RedirectOnError();
        int sessCount = GetSessionCount();
        //if (sessCount <= 0) ClosePrinterConnections();
    }

    protected void Application_BeginRequest(object sender, EventArgs e)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
        Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;

        System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";

        ////X-Frame-Options:deny
        //HttpContext.Current.Response.AddHeader("x-frame-options", "DENY");
        ////X-Content-Type_Options:nosniff
        //HttpContext.Current.Response.AddHeader("X-Content-Type-Options", "nosniff");

        //Check Session cloning
        //Check If it is a new session or not , if not then do the further checks
        if (Request.Cookies["ASP.NET_SessionId"] != null && Request.Cookies["ASP.NET_SessionId"].Value != null &&
            Request.Cookies["ASP.NET_SessionId"].Value.Length > 24)
        {
            string newSessionID = Request.Cookies["ASP.NET_SessionID"].Value;
            //Check the valid length of your Generated Session ID
            if (newSessionID.Length <= 24)
            {
                //Log the attack details here
                throw new HttpException("Invalid Request");
            }

            //Genrate Hash key for this User,Browser and machine and match with the Entered NewSessionID
            if (newSessionID.Length > 24 && GenerateHashKey() != newSessionID.Substring(24))
            {
                //Log the attack details here
                throw new HttpException("Invalid Request");
            }

            //Use the default one so application will work as usual//ASP.NET_SessionId
            Request.Cookies["ASP.NET_SessionId"].Value = Request.Cookies["ASP.NET_SessionId"].Value.Substring(0, 24);
        }
    }

    protected void Application_AcquireRequestState(object sender, EventArgs e)
    {

    }


    protected void Application_EndRequest(object sender, EventArgs e)
    {

        //Pass the custom Session ID to the browser.
        if (Response.Cookies["ASP.NET_SessionId"] != null && Request.Cookies["ASP.NET_SessionId"].Value != null &&
             Request.Cookies["ASP.NET_SessionId"].Value.Length > 0)
        {
            Response.Cookies["ASP.NET_SessionId"].Value = Request.Cookies["ASP.NET_SessionId"].Value.Substring(0, 24) +GenerateHashKey();
        }

    }

    private void CheckSiteMapCache()
    {
        App_Code.CSqlSiteMapProvider siteMapProvider = (App_Code.CSqlSiteMapProvider)SiteMap.Provider;

        //siteMapProvider.CheckSiteMapCache();



    }

    private void LoadUser()
    {

        Session["@@AuthType"] = HttpContext.Current.User.Identity.AuthenticationType.ToString();
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            string userName = HttpContext.Current.User.Identity.Name;
            if (userName != null)
            {
                string[] nameParts = userName.Split('\\');
                string domainName = nameParts[nameParts.GetLowerBound(0)];
                string samAcctName = nameParts[nameParts.GetUpperBound(0)];
                Session["UserName"] = samAcctName;
            }
        }
        else
            Session["UserName"] = null;
    }


    SiteMapNode AppendQueryString(object o, SiteMapResolveEventArgs e)
    {
        if (SiteMap.CurrentNode != null)
        {
            SiteMapNode temp;
            temp = SiteMap.CurrentNode.Clone(true);
            Uri u = new Uri(e.Context.Request.Url.ToString());
            temp.Url += u.Query;
            if (temp.ParentNode != null)
            {
                temp.ParentNode.Url += u.Query;
            }
            return temp;
        }
        else
        {
            //String url = e.Context.Request.Url.ToString();
            Uri u = new Uri(e.Context.Request.Url.ToString());
            return e.Provider.FindSiteMapNode(e.Context);
        }
    }



    private void RedirectOnError()
    {

        if (Server.GetLastError().InnerException != null)
        {
            Application["ErrorClass"] = Server.GetLastError();
        }
        else
        {
            Application["ErrorClass"] = Server.GetLastError().InnerException;
        }
        Server.ClearError();
        //Response.Redirect("../GeneralError.aspx");

        string URLRoot = Utility.GetUrlRootPath(); //System.Configuration.ConfigurationManager.AppSettings["URLPathRoot"].ToString();
        if ((this.Context.Handler as Page).MasterPageFile.ToString().ToLower().Contains("popup"))
        {
            Server.Transfer(URLRoot + "/GeneralErrorPopUp.aspx");
        }
        else
        {
            Server.Transfer(URLRoot + "/GeneralError.aspx");
        }

    }


    private void InitializeSessionCount()
    {
        Application["SessionCount"] = 0;
    }

    private int GetSessionCount()
    {
        return Convert.ToInt32(Application["SessionCount"]);
    }

    private void IncrementSessionsCount()
    {
        int sessionCount = Convert.ToInt32(Application["SessionCount"]);
        sessionCount++;
        Application["SessionCount"] = sessionCount;
    }


    private void DecrementSessionsCount()
    {
        int sessionCount = Convert.ToInt32(Application["SessionCount"]);
        sessionCount--;
        Application["SessionCount"] = sessionCount;
    }

    private string GenerateHashKey()
    {
        StringBuilder myStr = new StringBuilder();
        myStr.Append(Request.Browser.Browser);
        myStr.Append(Request.Browser.Platform);
        myStr.Append(Request.Browser.MajorVersion);
        myStr.Append(Request.Browser.MinorVersion);
        iAssetTrack.BAL.CommonBAL comBAL = new iAssetTrack.BAL.CommonBAL();
        return comBAL.GetSHA256HashValue(myStr.ToString());
    }


</script>
