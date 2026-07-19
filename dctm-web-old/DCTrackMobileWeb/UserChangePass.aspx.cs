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
using Microsoft.Security.Application;
using iAssetTrack.BAL;
using iAssetTrack.DALC;
using iAssetTrackBAL;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

public partial class UserChangePass : System.Web.UI.Page
{
    DataTable _dtRights;

    protected void Page_Load(object sender, EventArgs e)
    {

        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "UserChangePass.aspx";
            Response.Redirect("Login.aspx");
        }

        //*V3.8-Commented on 14Oct2013-By Amar Vidya-change password link in masterpage
        //bool blfoundPage = false;

        //if (_dtRights.Select("Module = 'Change Password' and Rights = '" + "View" + "'").Length != 0)
        //{
        //    blfoundPage = true;
        //}

        //if (blfoundPage == false)
        //{
        //    Response.Redirect("AccessDeniedPage.aspx");
        //    return;
        //}

        //Session["PageHeader"] = SiteMap.CurrentNode.Description;//*
        Session["PageHeader"] = "Change Password";//V3.8-Added on 14Oct2013-By Amar Vidya
    }
    
    
    protected void ibConfirm_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        UserChangePasswordBAL chPassBAL = new UserChangePasswordBAL();
        CommonBAL objCommon = new CommonBAL();
        try
        {
            string strOldPass = objCommon.GetSHA256HashValue(this.txtCurPassword.Text);
            string strNewPass = objCommon.GetSHA256HashValue(this.txtNewPassword.Text);

            chPassBAL.UserID = Convert.ToInt32(Session["UserID"].ToString());
            chPassBAL.OldPassword = strOldPass;
            chPassBAL.NewPassword = strNewPass;
            //CR3001:Password Expiration, by kjb on 09 Jan 2012 -- Begin
            double expirationDays = Convert.ToDouble(ConfigurationManager.AppSettings["PwdExpiryDays"].ToString());
            chPassBAL.ExpiryDate = DateTime.Now.AddDays(expirationDays);
            //CR3001:Password Expiration, by kjb on 09 Jan 2012 -- End
            chPassBAL.Persist(DALCOperation.Update);
            if (chPassBAL.ErrorType.Trim() != "")
            {
                lblMessage.Text = objCommon.displayMessage(chPassBAL.ErrorType);
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Visible = true;
                ibConfirm.Enabled = true;
                Reset();
            }
            else
            {
                objCommon = new CommonBAL();
                lblMessage.Text = objCommon.displayMessage(MessageCodes.PASSWORD_CHANGE_SUCCESSFUL);
                lblMessage.Visible = true;
                lblMessage.ForeColor = System.Drawing.Color.Navy;
                ibConfirm.Enabled = false;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("validNavigation = true;alert(\'");
                sb.Append(lblMessage.Text);
                sb.Append("\');");
                sb.Append("document.location = \'logoutme.aspx\';");

                var script = HttpUtility.JavaScriptStringEncode(sb.ToString(), false);
                ClientScript.RegisterStartupScript(this.GetType(), "logoutme",HttpUtility.HtmlDecode(script).Replace("\\u0027","'"),true);

            }
        }
        catch (Exception ex)
        {
            //lblMessage.Text = ex.Message;
            //lblMessage.ForeColor = System.Drawing.Color.Red;
            //lblMessage.Visible = true;
            //ibConfirm.Enabled = true;
            ExceptionPolicy.HandleException(ex, "errDCTrack");
            throw ex;
        }
        
    }


    private void Reset()
    {
        txtCurPassword.Text = "";
        txtNewPassword.Text = "";
        txtReEnterPassword.Text = "";
    }


    protected void ibReset_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        Reset();
        lblMessage.Text = "";
        lblMessage.Visible = false;
    }
}
