/*
File Name:	Login.aspx.cs
Description: Login page for external Subcontractor users.
Date created:	15 Nov 2006

Modification History:
***********************
CR		Name			            Date			Description
New		venkatesh		            15/11/2006		File has been created.
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
//using iDocTrack.Remoting.Shared;
using iAssetTrack.Security;
using iAssetTrack.DALC;
using iAssetTrackBAL;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Net.NetworkInformation;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging;

public partial class ASPX_Login : System.Web.UI.Page
{
    #region "Declarations"

    //private int MAXPWDAGE_DEFAULT = 0;
    private int maxLoginAttempts = Convert.ToInt32(ConfigurationManager.AppSettings["MaxNoOfLoginAttempts"]);
    private int noOfDays;
    #endregion

    #region Page Event Methods

    protected void Page_Load(object sender, EventArgs e)
    {
        this.txtUser.Focus();
        if (!IsPostBack)
        {
        }
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        string Cmd = Request["Cmd"];
        if (Cmd != null)
        {
            if (Cmd.Equals("1") || Cmd.Equals("2"))
                Response.Write("<script language='javascript'>window.open('close.htm', '_self');</script>");
        }
    }

    #endregion

    /// <summary>
    /// Login button event handler. This authenticates against Active Directory given by Membership.Domain.
    /// </summary>
    /// <author>Sivashanmugam, Muniappan</author>
    /// <createdOn>03 Oct 2007</createdOn>
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
                                    if (objUser.IsATenantUser(Convert.ToInt32(dsUser.Tables[0].Rows[0][0].ToString())) == 1)
                                    {
                                        Session["TenantUser"] = "true";
                                    }
                                    else
                                    {
                                        Session["TenantUser"] = "false";
                                    }
                                    
                                    //v3.8
                                    // tells whether user has site restrictions enabled
                                    Session["SiteRestrictEnabled"] = bool.Parse(dsUser.Tables[0].Rows[0]["SiteRestriction"].ToString());
                                    //**//
                                    FormsAuthentication.RedirectFromLoginPage(Session["UserId"].ToString(), false);
                                    LoadGroups();
                                    //CR3001:Password Expiration, by kjb on 09 Jan 2012 -- Begin
                                    // if Password is already expired redirect to Change Password page 
                                    //and disable all the menu options
                                    string expiryDate = dsUser.Tables[0].Rows[0]["ExpiryDate"].ToString();
                                    if (!string.IsNullOrEmpty(expiryDate))
                                    {
                                        //admin and Import User won't go thru this routine
                                        bool isFirstLogin = Convert.ToBoolean(dsUser.Tables[0].Rows[0]["IsFirstLogin"].ToString()) == true ? true : false;

                                        noOfDays = System.Data.Linq.SqlClient.SqlMethods.DateDiffDay(DateTime.Now,
                                            Convert.ToDateTime(expiryDate));
                                        int expirationWarningDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpirationWarningDays"].ToString());

                                        //check whether user accessing the site for the first time
                                        // if so, force user to change password


                                        if (noOfDays <= 0)
                                        {
                                            //password expired section
                                            Session["ExpiredMsg"] = objCommon.displayMessage(MessageCodes.PASSWORD_EXPIRED);
                                            App_Code.CSqlSiteMapProvider.PasswordExpired = "true";
                                            App_Code.CSqlSiteMapProvider.RefreshSiteMap();
                                        }
                                        else if (isFirstLogin)
                                        {
                                            Session["ExpiredMsg"] = objCommon.displayMessage(MessageCodes.FIRST_LOGIN);
                                            App_Code.CSqlSiteMapProvider.PasswordExpired = "true";
                                            App_Code.CSqlSiteMapProvider.RefreshSiteMap();
                                        }
                                        else
                                        {
                                            App_Code.CSqlSiteMapProvider.PasswordExpired = "";
                                            App_Code.CSqlSiteMapProvider.RefreshSiteMap();
                                        }

                                        if (noOfDays <= expirationWarningDays && noOfDays > 0)
                                        {
                                            Session["ExpiryMsg"] = objCommon.displayMessage(MessageCodes.PASSWORD_WILL_EXPIRE_1) + noOfDays.ToString() + " " + objCommon.displayMessage(MessageCodes.PASSWORD_WILL_EXPIRE_2);
                                        }
                                    }
                                    else
                                    {
                                        Session["ExpiryMsg"] = "";
                                        App_Code.CSqlSiteMapProvider.PasswordExpired = "";
                                        App_Code.CSqlSiteMapProvider.RefreshSiteMap();
                                    }

                                    //CR3001:Password Expiration, by kjb on 09 Jan 2012 -- End

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
        Response.Redirect("Login.aspx");
    }

    private void AuditLogin()
    {


        string mode = ConfigurationManager.AppSettings["Mode"];

        if (Convert.ToInt32(mode) != 3)
        {
            AuditLoginLogoutBAL objAudit = new AuditLoginLogoutBAL();
            objAudit.UserID = Convert.ToInt32(Session["UserID"]);
            objAudit.AuditType = 0;
            objAudit.SessionID = HttpContext.Current.Session.SessionID;
            IPAddress[] addressList;

            try
            {
                //Get Client IP 
                objAudit.IP = GetUser_IP();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "errDCTrack");
                objAudit.IP = "";
            }

            objAudit.Persist(DALCOperation.Insert);

            // adds user id - session id  into a dictionary stored into a Application[Sessionss] table 
            // if same user id already exists than  it will be removed and userid with new session id will be added 
            // at the same time db will be updated with log out time for this userid -  old session ID record only if log out time is null
            Dictionary<string, string> dic = null;
            try
            {
                if (Application["Sessions"] == null)
                {
                    dic = new Dictionary<string, string>();
                    Application.Add("Sessions", dic);
                    ((Dictionary<string, string>)Application["Sessions"]).Add(txtUser.Text.Trim().ToLower(), HttpContext.Current.Session.SessionID);

                }
                else
                {
                    dic = ((Dictionary<string, string>)Application["Sessions"]);

                    if (dic.ContainsKey(txtUser.Text.Trim().ToLower()))
                    {

                        string oldSessionID = ((Dictionary<string, string>)Application["Sessions"])[txtUser.Text.Trim().ToLower()];
                        //logoff old Session
                        objAudit = new AuditLoginLogoutBAL();
                        objAudit.UserID = Convert.ToInt32(Session["UserID"]);
                        objAudit.AuditType = 1;
                        objAudit.SessionID = oldSessionID;
                        objAudit.Persist(DALCOperation.Insert);
                        // remove old record from Application[Sessions]
                        ((Dictionary<string, string>)Application["Sessions"]).Remove(txtUser.Text.Trim().ToLower());
                        // add new record with new session id
                        ((Dictionary<string, string>)Application["Sessions"]).Add(txtUser.Text.Trim().ToLower(), HttpContext.Current.Session.SessionID);
                    }
                    else
                    {
                        ((Dictionary<string, string>)Application["Sessions"]).Add(txtUser.Text.Trim().ToLower(), HttpContext.Current.Session.SessionID);
                    }
                }

            }
            catch
            {

            }

        }

    }

    protected string GetUser_IP()
    {
        string strIP = String.Empty;
        HttpRequest httpReq = HttpContext.Current.Request;

        //ArrayList ips = new ArrayList();
        //var address = NetworkInterface
        //    .GetAllNetworkInterfaces()
        //    .Where(i => i.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || i.NetworkInterfaceType == NetworkInterfaceType.Ethernet) 
        //    .Where (i => !i.Description.ToLower().Contains("virtual") && !i.Description.ToLower().Contains("tap") && !i.Description.ToLower().Contains("bluetooth"))
        //    .SelectMany(i => i.GetIPProperties().UnicastAddresses)
        //    .Where(a => a.Address.AddressFamily == AddressFamily.InterNetwork)
        //    .Select(a => a.Address.ToString())
        //    .ToList();

        //if (address != null && address.Count > 0)
        //    strIP = address[0].ToString();
        //else
        //    strIP = "";

        if (string.IsNullOrEmpty(strIP))
        {
            //test for non-standard proxy server designations of client's IP
            if (httpReq.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                strIP = httpReq.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (Request.ServerVariables["REMOTE_ADDR"] != null && System.Net.Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]).AddressList != null &&
                System.Net.Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]).AddressList.Length > 0)
            {
                IPAddress[] addressList;
                addressList = Array.FindAll(Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]).AddressList, a => a.AddressFamily == AddressFamily.InterNetwork);
                if (addressList != null && addressList.Length > 0)
                {
                    strIP = addressList[addressList.Length - 1].ToString();
                }
            }
            //test for host address reported by the server
            else if((httpReq.UserHostAddress.Length != 0) && ((httpReq.UserHostAddress != "::1") || (httpReq.UserHostAddress != "localhost")))
            {
                strIP = httpReq.UserHostAddress;
            }
        }

        return strIP;
    }

    private void LoadRights()
    {
        string mode = ConfigurationManager.AppSettings["Mode"];

        if (Convert.ToInt32(mode) != 3)
        {
            SessionManagerBAL objSession = new SessionManagerBAL((int)Session["UserID"]);
            Session["Rights"] = (DataTable)objSession.Rights;
        }
        # region commented code
        //else
        //{
        //    ICommon svc = (ICommon)RemotingHelper.CreateProxy(typeof(ICommon));
        //    Session["Rights"] = svc.LoadRights((int)Session["UserID"]);
        //}
        # endregion

    }

    private void LoadGroups()
    {
        iAssetTrack.BAL.UserBAL userBAL = new iAssetTrack.BAL.UserBAL();
        int iUserId = Int32.Parse(Session["UserId"].ToString());
        userBAL.UserID = iUserId;

        DataSet ds = userBAL.retrieveAssignGroup();

        String sGroups = "";

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            sGroups += ds.Tables[0].Rows[i]["Group"] + ",";
        }

        Session["Groups"] = (String[])sGroups.Split(',');

    }


    //public string encrypt(string encryptString)
    //{
    //    string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ@#$%";
    //    byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
    //    using (Aes encryptor = Aes.Create())
    //    {
    //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {  
    //            0x64,0x63,0x74,0x72,0x61,0x63,0x6b,0x6d,0x6f,0x62,0x69,0x6c,0x65, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76  
    //        });
    //        encryptor.Key = pdb.GetBytes(32);
    //        encryptor.IV = pdb.GetBytes(16);
    //        using (MemoryStream ms = new MemoryStream())
    //        {
    //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
    //            {
    //                cs.Write(clearBytes, 0, clearBytes.Length);
    //                cs.Close();
    //            }
    //            encryptString = Convert.ToBase64String(ms.ToArray());
    //        }
    //    }
    //    return encryptString;
    //}

    //public string Decrypt(string cipherText)
    //{
    //    string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ@#$%";
    //    cipherText = cipherText.Replace(" ", "+");
    //    byte[] cipherBytes = Convert.FromBase64String(cipherText);
    //    using (Aes encryptor = Aes.Create())
    //    {
    //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {  
    //            0x64,0x63,0x74,0x72,0x61,0x63,0x6b,0x6d,0x6f,0x62,0x69,0x6c,0x65, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76  
    //        });
    //        encryptor.Key = pdb.GetBytes(32);
    //        encryptor.IV = pdb.GetBytes(16);
    //        using (MemoryStream ms = new MemoryStream())
    //        {
    //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
    //            {
    //                cs.Write(cipherBytes, 0, cipherBytes.Length);
    //                cs.Close();
    //            }
    //            cipherText = Encoding.Unicode.GetString(ms.ToArray());
    //        }
    //    }
    //    return cipherText;
    //}  

}
