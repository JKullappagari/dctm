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

public partial class iAssetTrackCorporate : System.Web.UI.Page
{
    #region "Declarations"
        private iAssetTrack.BAL.UserBAL objUser;
    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        string strUsername;
        string strRemoteServer = string.Empty;
        
        string strCorporateDomain = System.Configuration.ConfigurationManager.AppSettings["CorporateDomain"];
        if (Session["Username"] != null)
        {
            strUsername = Session["Username"].ToString();
            _LoadSessionUser(strUsername);
        }
        else
        {
            Response.Redirect("UserNotFound.aspx");
        }
    }
    /// <summary>
    /// Setting up session variables for userid, username and user type and transfer to main page upon successful login.
    /// </summary>
    /// <param name="strCurrentIdentity"></param>
    private void _LoadSessionUser(string strCurrentIdentity)
    {
        string userName = strCurrentIdentity;        
        if (userName != "")
        {
            string[] nameParts = userName.Split('\\');
            string domainName = nameParts[nameParts.GetLowerBound(0)];
            string samAcctName = nameParts[nameParts.GetUpperBound(0)];
            objUser = new UserBAL();
            DataSet dsUser;
            objUser.LoginName = samAcctName;
            dsUser = objUser.retrieveUserByUserName();
            DataTable dtUser = dsUser.Tables[0];
            if (dtUser.Rows.Count > 0)
            {
                Session["UserID"] = Convert.ToInt32(dsUser.Tables[0].Rows[0][0].ToString());
                Session["UserName"] = dsUser.Tables[0].Rows[0][1].ToString();
                Session["UserType"] = dsUser.Tables[0].Rows[0]["UserType"].ToString();
                this.AuditLogin();
                Response.Redirect("MainPage.aspx");
            }
            else
            {
                Response.Redirect("UserNotFound.aspx");
            }
        }
        else
        {
            Session["UserID"] = null;
            Session["UserName"] = null;
            Session["UserType"] = null;
        }
    }

    /// <summary>
    /// Audit login.
    /// </summary>
    private void AuditLogin()
    {
        AuditLoginLogoutBAL objAudit = new AuditLoginLogoutBAL();
        objAudit.UserID = Convert.ToInt32(Session["UserID"]);
        objAudit.AuditType = 0;
        //System.Net.IPHostEntry TmpEntry = System.Net.Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]);
        //objAudit.IP = TmpEntry.AddressList[0].ToString();
        objAudit.IP = Request.ServerVariables["REMOTE_ADDR"];
        objAudit.Persist(DALCOperation.Insert);
    }
}
