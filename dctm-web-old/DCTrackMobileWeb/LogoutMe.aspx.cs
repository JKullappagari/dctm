/*
File Name:	Login.aspx

Description:	login page	

Date created:	27 March 2006

Modification History:
***********************
CR		Name			Date			Description
New		srinivas 		27/03/2006		File has been created.
*/



using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using iAssetTrack.BAL;
using iAssetTrack.DALC;
//using iDocTrack.Remoting.Shared;
public partial class LogoutMe : System.Web.UI.Page
{
    string mode = ConfigurationManager.AppSettings["Mode"];
    protected void Page_Load(object sender, EventArgs e)
    {
        this.AuditLogout();
        if (Convert.ToInt32(mode) != 3)
        {
                Session.Abandon();
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddYears(-30);
                //Response.Write("<script language='javascript'>var obj_window = window.open('close.htm', '_self', 'width=650,height=525,status=no,resizable=no,top='+((screen.height-575)/2)+',left='+((screen.width-650)/2)+',dependent=yes,alwaysRaised=yes');obj_window.opener = window;obj_window.focus();</script>");
                Response.Redirect("Login.aspx");
                Response.End();
        }
        if (Convert.ToInt32(mode) == 3)
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
            Response.Redirect("Login.aspx");
        }
    }
      /// <summary>
    /// Audit logout information
    /// </summary>
    /// 
    private void AuditLogout()
    {

        if (Convert.ToInt32(mode) != 3)
        {
            AuditLoginLogoutBAL objAudit = new AuditLoginLogoutBAL();
            objAudit.UserID = Convert.ToInt32(Session["UserID"]);
            objAudit.AuditType = 1;
            objAudit.SessionID = HttpContext.Current.Session.SessionID;
            //System.Net.IPHostEntry TmpEntry = System.Net.Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]);
            //objAudit.IP = TmpEntry.AddressList[0].ToString();
            objAudit.IP = Request.ServerVariables["REMOTE_ADDR"];
            objAudit.Persist(DALCOperation.Insert);
        }
    }


}
