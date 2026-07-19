using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

/// <summary>
/// Summary description for Reportcredentials
/// </summary>
public class ReportCredentials : Microsoft.Reporting.WebForms.IReportServerCredentials
{
    string _userName, _password, _domain;
    public ReportCredentials(string userName, string password, string domain)
    {
        _userName = userName;
        _password = password;
        _domain = domain;
    }
    public System.Security.Principal.WindowsIdentity ImpersonationUser
    {
        get
        {
            return null;
        }
    }

    public System.Net.ICredentials NetworkCredentials
    {
        get
        {
            string user = ConfigurationManager.AppSettings["ReportServerUser"];
            string pass = ConfigurationManager.AppSettings["ReportServerPass"];
            string domain = ConfigurationManager.AppSettings["ReportServerDomain"];
            return new System.Net.NetworkCredential(user, pass, domain);
        }
    }

    public bool GetFormsCredentials(out System.Net.Cookie authCoki, out string userName, out string password, out string authority)
    {
        userName = null;
        password = null;
        authority = null;
        //authCoki = new System.Net.Cookie(".ASPXAUTH", ".ASPXAUTH", "/", "Domain");
        authCoki = null;
        return false;
    }
}
