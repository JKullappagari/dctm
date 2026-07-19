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
using iAssetTrack.Security;
using iAssetTrack.DALC;
using iAssetTrackBAL;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;


public partial class ASPX_Mobile_Login : System.Web.UI.Page
{
    #region "Declarations"
    private int maxLoginAttempts = Convert.ToInt32(ConfigurationManager.AppSettings["MaxNoOfLoginAttempts"]);
    private int noOfDays;
    private int pageWidth;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        pageWidth = Page.Request.Browser.ScreenPixelsWidth;
        this.txtUser.Focus();

        if (!IsPostBack)
        {
        }

    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        try
        {
            if (Request["Cmd"] != null)
            {
                string Cmd = Request["Cmd"];
                if (Cmd != null)
                {
                    if (Cmd.Equals("1") || Cmd.Equals("2"))
                        Response.Write("<script language='javascript'>window.open('close.htm', '_self');</script>");
                }
            }
        }
        catch(Exception ex)
        {
            ExceptionPolicy.HandleException(ex, "errDCTrack");
        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string mode = ConfigurationManager.AppSettings["Mode"];
        UserBAL objUser;
        CommonBAL objCommon;


        if (Convert.ToInt32(mode) != 3)
        {
            int noOfAttempts = 0;
            try
            {
                objUser = new UserBAL();
                DataSet dsUser;
                objUser.LoginName = txtUser.Text;
                objCommon = new CommonBAL();
                string hashPassword = objCommon.GetSHA256HashValue(txtPassword.Text);
                dsUser = objUser.retrieveUserByUserName();
                ManageUsersBAL objMUBAL = new ManageUsersBAL();
                objMUBAL.Search = txtUser.Text;
                DataSet ds = objMUBAL.retrieve();
                if (ds.Tables[0].Rows.Count > 0 && dsUser.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Status"].ToString().CompareTo("Active") == 0)
                    {
                        noOfAttempts = Convert.ToInt32(string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FailedLoginAttempts"].ToString()) ? "0" :
                                ds.Tables[0].Rows[0]["FailedLoginAttempts"].ToString());
                        if (noOfAttempts < (maxLoginAttempts - 1))
                        {
                            if (dsUser.Tables[0].Rows.Count > 0)
                            {
                                if (dsUser.Tables[0].Rows[0]["Password"].ToString().Equals(hashPassword))
                                {
                                    //update the failedLoginAttempts  count to zero by kjb on 06 Jan 2012
                                    UserLoginBAL ob = new UserLoginBAL();

                                    ob.LoginName = txtUser.Text;
                                    ob.NoOfAttempts = 0;
                                    ob.Persist(DALCOperation.Update);

                                    lblMsg.Text = "";
                                    //Rajesh - There seems to be some problem with the Session["UserID"]. After Login it is set to 3 somewhere.
                                    // Dunno where, so storing it in another Session Variable, and in the iAssetTrackMasterPage, Copying it from this LoggedInUser variable.

                                    Session["LoggedInUser"] = Convert.ToInt32(dsUser.Tables[0].Rows[0][0].ToString());
                                    Session["UserID"] = Convert.ToInt32(dsUser.Tables[0].Rows[0][0].ToString());
                                    Session["UserName"] = dsUser.Tables[0].Rows[0][1].ToString();
                                    Session["UserType"] = dsUser.Tables[0].Rows[0]["UserType"].ToString();
                                    Session["EmailId"] = dsUser.Tables[0].Rows[0]["Email"].ToString();
                                    //v3.8
                                    // tells whether user has site restrictions enabled
                                    Session["SiteRestrictEnabled"] = bool.Parse(dsUser.Tables[0].Rows[0]["SiteRestriction"].ToString());
                                    //**//
                                    FormsAuthentication.RedirectFromLoginPage(Session["UserId"].ToString(), false);
                                    //CR3001:Password Expiration, by kjb on 09 Jan 2012 -- Begin
                                    // if Password is already expired redirect to Change Password page 
                                    //and disable all the menu options
                                    string expiryDate = dsUser.Tables[0].Rows[0]["ExpiryDate"].ToString();
                                    if (!string.IsNullOrEmpty(expiryDate))
                                    {
                                        noOfDays = System.Data.Linq.SqlClient.SqlMethods.DateDiffDay(DateTime.Now,
                                            Convert.ToDateTime(expiryDate));
                                        int expirationWarningDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpirationWarningDays"].ToString());

                                        if (noOfDays <= 0)
                                        {
                                            //password expired section
                                            Session["ExpiredMsg"] = objCommon.displayMessage(MessageCodes.PASSWORD_EXPIRED);
                                            //App_Code.CSqlSiteMapProvider.PasswordExpired = "true";
                                            //App_Code.CSqlSiteMapProvider.RefreshSiteMap();
                                        }
                                        else
                                        {
                                            //App_Code.CSqlSiteMapProvider.PasswordExpired = "";
                                            //App_Code.CSqlSiteMapProvider.RefreshSiteMap();
                                        }

                                        if (noOfDays <= expirationWarningDays && noOfDays > 0)
                                        {
                                            Session["ExpiryMsg"] = objCommon.displayMessage(MessageCodes.PASSWORD_WILL_EXPIRE_1) + noOfDays.ToString() + " " + objCommon.displayMessage(MessageCodes.PASSWORD_WILL_EXPIRE_2);
                                        }
                                    }
                                    else
                                    {
                                        Session["ExpiryMsg"] = "";
                                        //App_Code.CSqlSiteMapProvider.PasswordExpired = "";
                                        //App_Code.CSqlSiteMapProvider.RefreshSiteMap();
                                    }

                                    //CR3001:Password Expiration, by kjb on 09 Jan 2012 -- End

                                    ///V000010
                                    ///Added on 21-Aug-2012-By Vidya
                                    this.AuditLogin();

                                }
                                else
                                {
                                    lblMsg.Text = objCommon.displayMessage(MessageCodes.AUTHENTICATION_FAILED);
                                    lblMsg.ForeColor = System.Drawing.Color.Red;
                                    UserLoginBAL ob = new UserLoginBAL();
                                    ob.LoginName = txtUser.Text;
                                    ob.NoOfAttempts = noOfAttempts + 1;
                                    ob.Persist(DALCOperation.Update);
                                }
                            }
                            else
                            {
                                lblMsg.Text = objCommon.displayMessage(MessageCodes.AUTHENTICATION_FAILED);
                                lblMsg.ForeColor = System.Drawing.Color.Red;
                                UserLoginBAL ob = new UserLoginBAL();
                                ob.LoginName = txtUser.Text;
                                ob.NoOfAttempts = noOfAttempts + 1;
                                ob.Persist(DALCOperation.Update);
                            }
                        }
                        else
                        {

                            UserLoginBAL ob = new UserLoginBAL();
                            ob.LoginName = txtUser.Text;
                            ob.Persist(DALCOperation.Delete);
                            lblMsg.Text = objCommon.displayMessage(MessageCodes.ACCOUNT_LOCKED);
                            lblMsg.ForeColor = System.Drawing.Color.Red;

                        }
                    }
                    else
                    {
                        lblMsg.Text = "User account is disabled,contact Administrator";
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                    }

                }
                else
                {
                    lblMsg.Text = objCommon.displayMessage(MessageCodes.AUTHENTICATION_FAILED);
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (SqlException ex)
            {
                ExceptionPolicy.HandleException(ex, "errDCTrack");
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Database connection error";
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "errDCTrack");
                lblMsg.ForeColor = System.Drawing.Color.Red;
                //lblMsg.Text = ex.Message;
                lblMsg.Text = "Login failed, check with Administrator";
            }
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Mobile/MobileLogin.aspx");
    }

    private void AuditLogin()
    {
        string mode = ConfigurationManager.AppSettings["Mode"];

        if (Convert.ToInt32(mode) != 3)
        {
            AuditLoginLogoutBAL objAudit = new AuditLoginLogoutBAL();
            objAudit.UserID = Convert.ToInt32(Session["UserID"]);
            objAudit.AuditType = 0;

            /// V000010

            ///Commented on 21-Aug-2012- By Vidya
            System.Net.IPHostEntry TmpEntry = System.Net.Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]);
            objAudit.IP = TmpEntry.AddressList[0].ToString();

            ///Added on 21-Aug-2012-By Vidya
            //objAudit.IP = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(2).ToString();

            objAudit.Persist(DALCOperation.Insert);
        }

    }

}

