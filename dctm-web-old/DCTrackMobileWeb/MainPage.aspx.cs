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
using System.Web.Services;
using iAssetTrack.DALC;
using iAssetTrack.BAL;

public partial class MainPage : System.Web.UI.Page
{

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        
    }
    // HP: Added Page_Load event to handle display of password expiry status and change password option.
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["LoggedInUser"] == null)
        {
            Response.Redirect("Login.aspx");
        }


        Session["PageHeader"] = "";
        //CR3001:Password Expiration, by kjb on 09 Jan 2012 -- Begin
        if (Session["ExpiryMsg"] != null)
        {
            if (!string.IsNullOrEmpty(Session["ExpiryMsg"].ToString()))
            {
                this.ShowMessage(Session["ExpiryMsg"].ToString(), "nourl");
                Session["ExpiryMsg"] = "";
            }

        }
        if (Session["ExpiredMsg"] != null)
        {
            if (!string.IsNullOrEmpty(Session["ExpiredMsg"].ToString()))
            {
                this.ShowMessage(Session["ExpiredMsg"].ToString(), "nourl");
                Session["ExpiredMsg"] = "";
            }

        }

        //Added on 16-Nov-2012-V000061

        //if    ((((string[])(Session["Groups"]))[0])=="")
        //{
        //    this.ShowMessage("No groups assigned", "nourl");
        //}

        //CR3001:Password Expiration, by kjb on 09 Jan 2012 -- End


        /*
        hypChangePassword.Visible = true;
        hypPasswordHelp.Visible = false;
        long daysLeft = -1;

        Session["PageHeader"] = "";
        if (Session["UserType"].Equals("E"))
        {
            if ((bool)Session["pwdExpired"])
                lblPasswordExpiry.Text = "Your password had expired. ";
            else
            {
                if (long.TryParse(Session["pwdExpires"].ToString(), out daysLeft))
                {
                    if (daysLeft > 0)
                        lblPasswordExpiry.Text = "Your password expires in " + daysLeft + " days. ";
                    else
                        return;
                }
            }
            AppendMessage();
        }
        */
    }

    /// <summary>
    /// To register alert message for user actions.
    /// </summary>
    /// <param name="mess"></param>
    /// <param name="URL"></param>
    private void ShowMessage(string mess, string URL)
    {
        string strScript = "<script type=\"text/javascript\">validNavigation = true;alert(\'" + mess + "\');</script>";
        if (!Page.ClientScript.IsStartupScriptRegistered("FORMMESSAGE"))
            Page.ClientScript.RegisterStartupScript(typeof(Page), "FORMMESSAGE", strScript);
    }


    //HP: Added to append Change Password option if user can change password.
    /// <summary>
    /// Appends Change Password option if user can change password.
    /// </summary>
    protected void AppendMessage()
    {
        if ((bool)Session["userCannotChangePwd"])
        {
            hypChangePassword.Visible = false;
            hypPasswordHelp.Visible = false;
            lblPasswordExpiry.Text += "Please contact the system administrator.";
        }
        else
        {
            hypChangePassword.Text = "Click here to change the password.";
            hypPasswordHelp.Text = "(?)";
        }
    }

    //HP: Added to display the Change Password dialog.
    protected void hypChangePassword_Init(object sender, EventArgs e)
    {
        hypChangePassword.Attributes.Add("onclick", "openWin('" + Session["LoginName"] + "', 'PasswordChange');");
    }

    //HP: Added to display the Password Hint dialog.
    protected void hypPasswordHelp_Init(object sender, EventArgs e)
    {
        hypPasswordHelp.Attributes.Add("onclick", "openWin('', 'PasswordHelp');");
    }

   
}
