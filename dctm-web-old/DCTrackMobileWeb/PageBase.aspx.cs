using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using iAssetTrack.BAL;
using iAssetTrack.DALC;
using System.Web.Security;


public partial class  PageBase : Page
{
    [WebMethod]
    public static void ForceUserLogOut()
    {
        AuditLoginLogoutBAL objAudit = new AuditLoginLogoutBAL();
        objAudit.UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);
        objAudit.AuditType = 1;
        objAudit.IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        objAudit.Persist(DALCOperation.Insert);

        System.Web.HttpContext.Current.Session.Abandon();
        FormsAuthentication.SignOut();
    }

}