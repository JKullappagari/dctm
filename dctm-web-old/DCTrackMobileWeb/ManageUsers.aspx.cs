/*
File Name:	Login.aspx.cs

Description:User master screen
 * 
Date created:	15 November 2006

Modification History:
***********************
CR		Name			            Date			Description
New		venkatesh		15/11/2006		File has been created.
*/


using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using iAssetTrack.BAL;
using iAssetTrack.DALC;
//using iDocTrack.Remoting.Shared;
using iAssetTrack.Security;
using iAssetTrackBAL;
using System.Text;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Text.RegularExpressions;
using Infragistics.Web.UI.GridControls;
using System.Collections.Generic;
using System.Net.Configuration;

enum UserType
{
    External = 0,
    Internal = 1
}

public partial class ASPX_ManageUsers : System.Web.UI.Page
{
    #region "Declarations"

    # region v3.8
    private static Dictionary<string, int> siteResData = new Dictionary<string, int>();
    #endregion

    private iAssetTrack.BAL.BusinessUnitBAL objBU;
    private iAssetTrack.BAL.ManageUsersBAL objUser;
    private iAssetTrack.BAL.UserBAL objUserHistroy;
    private iAssetTrack.BAL.CommonBAL objCommon;

    private const string TAB_KEY_HISTORY = "History";
    private const string VIEWSTATE_HISTORY = "History";
    private const string VIEWSTATE_USERID = "UserID";

    private iAssetTrack.BAL.SitesBAL objSite; //V3.8-Added on 16Oct2013-By Amar Vidya

    DataTable _dtRights;

    private bool IsHistoryTabInitialized
    {
        get { return (ViewState[VIEWSTATE_HISTORY] != null ? Convert.ToBoolean(ViewState[VIEWSTATE_HISTORY]) : false); }
        set { ViewState[VIEWSTATE_HISTORY] = value; }
    }

    private int UserID
    {
        get { return (ViewState[VIEWSTATE_USERID] != null ? Convert.ToInt32(ViewState[VIEWSTATE_USERID]) : -1); }
        set { ViewState[VIEWSTATE_USERID] = value; }
    }


    #endregion
    public string rdmPassword = string.Empty;

    protected void Page_Init(object sender, EventArgs e)
    {
        Session["PageHeader"] = "Manage Users";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //Security check
        _dtRights = (DataTable)(Session["Rights"]);

        bool blfoundPage = false;

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "ManageUsers.aspx";
            Response.Redirect("Login.aspx");
        }

        if (_dtRights.Select("Module = 'Manage Users' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }
        if (_dtRights.Select("Module = 'Manage Users' and Rights = '" + "Create" + "'").Length != 0)
        {
            this.ibCreateInternal.Enabled = true;
        }
        else
        {
            ibCreateInternal.Enabled = false;
        }
        if (_dtRights.Select("Module = 'Manage Users' and Rights = '" + "Modify" + "'").Length != 0)
        {
            this.ibCreateInternal.Enabled = true;
        }
        else
        {
            ibCreateInternal.Enabled = false;
        }

        this.btnAssignRightInt.Visible = false;
        //this.ibAssignRFID.Visible = false;
        //this.ibDeassignRFID.Visible = false;
        //this.ibLostRFID.Visible = false;
        this.ibResetPass.Enabled = false;
        this.ibResetPass.Visible = false;
        if (!IsPostBack)
        {
            # region v3.8
            //Add all Sites with ResType as zero
            // later any changes to the grid will be stored into the list
            // which will be used in initialize row to apply same(since
            // values will be restet to zero every page load).
            siteResData.Clear();
            objSite = new iAssetTrack.BAL.SitesBAL();
            DataTable dt = objSite.retrieve().Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                siteResData.Add(row["Site"].ToString(), 0);
            }

            #endregion
            this.PopulateBusinessUnit();
            ddlBusinessUnit.SelectedIndex = 1;
            ddlBusinessUnit.Enabled = false;
            FillDropDownLists();
            FillDropdownsByBU();
            if (Context.Items["@@UserID"] != null)
            {
                //hide password Rule
                lblPasswordRule.Visible = false;
                lblRule.Visible = false;
                this.UserID = Convert.ToInt32(Context.Items["@@UserID"]);
                lblUserID.Text = Context.Items["@@UserID"].ToString();
                this.PopulateUserDetails();
                this.ibResetPass.Enabled = true;
                this.ibResetPass.Visible = true;
                if (_dtRights.Select("Module = 'Manage Users' and Rights = '" + "Modify" + "'").Length != 0)
                {
                    this.ibCreateInternal.Enabled = true;
                }
                else
                {
                    ibCreateInternal.Enabled = false;
                }

                //Site restriction enabled
                SiteRestrictionsBAL objSRBAL = new SiteRestrictionsBAL();
                objSRBAL.UserID = this.UserID;
                DataSet dsSR = objSRBAL.RetrieveByUserID();
                DataTable dtGrid = dsSR.Tables[0];
                foreach (DataRow dr in dtGrid.Rows)
                {
                    siteResData[dr["Site"].ToString()] = int.Parse(dr["ResTypeID"].ToString());
                }
            }
            else
            {
                ClearInternal();
            }
        }
        else
        {
            if (this.UserID > 0)
            {
                //hide password Rule
                lblPasswordRule.Visible = false;
                lblRule.Visible = false;

                this.populateRFIDStatus();
                if (_dtRights.Select("Module = 'Manage Users' and Rights = '" + "Modify" + "'").Length != 0)
                {
                    this.ibCreateInternal.Enabled = true;
                    this.ibResetPass.Enabled = true;
                    this.ibResetPass.Visible = true;
                }
                else
                {
                    ibCreateInternal.Enabled = false;
                }
            }
        }
        //*V3.8-Added on 17Oct2013-By Amar Vidya
        if (chkEnforceSiteRestriction.Checked == true)
        {
            pnlEnforceSiteRestriction.Attributes.Add("style", " display:inline");
        }
        else
        {
            pnlEnforceSiteRestriction.Attributes.Add("style", " display:none");
        }

        //this is disabled as Site Restriction function is not implemented yet.
        populateGrid();

    }

    #region Lookups

    private void PopulateBusinessUnit()
    {
        objBU = new iAssetTrack.BAL.BusinessUnitBAL();

        DataSet dsBU = objBU.retrieve();
        DataTable dtBU = dsBU.Tables[0];
        if (dtBU.Rows.Count > 0)
        {
            cblBusinessUnit.DataTextField = dtBU.Columns[1].ColumnName;
            cblBusinessUnit.DataValueField = dtBU.Columns[0].ColumnName;
            cblBusinessUnit.DataSource = dtBU.DefaultView;
            cblBusinessUnit.DataBind();
            // added by kjb on 30 Apr 2012
            foreach (ListItem ctrl in cblBusinessUnit.Items)
            {
                ctrl.Selected = true;
            }
        }
        else
        {
            lblMessage.ForeColor = System.Drawing.Color.Red;
            objCommon = new CommonBAL();
            lblMessage.Text = objCommon.displayMessage(MessageCodes.MANAGE_USER_BU_SETUP);
        }
    }

    //private void FillUserHistory()
    //{

    //    //int intUserID = 0;
    //    //if (!string.IsNullOrEmpty(lblUserID.Text))
    //    ////if   (lblUserID.Text != null)
    //    //{
    //    //    intUserID = Convert.ToInt32(lblUserID.Text.ToString());

    //    //}

    //    objUserHistroy = new iAssetTrack.BAL.UserBAL();
    //    //objUserHistroy.UserID = intUserID;
    //    objUserHistroy.UserID = this.UserID;
    //    DataSet dtUser = objUserHistroy.GetUserHistoryDetails(this.UserID);

    //    if (dtUser.Tables[0].Rows.Count > 0)
    //    {
    //        uwgUserHistory.DataSource = dtUser.Tables[0];
    //        uwgUserHistory.DataBind();
    //        uwgUserHistory.Columns[0].Hidden = true;
    //        //uwgUserHistory.Columns[0].Hidden = true;
    //        //uwgUserHistory.Columns[1].Width = Unit.Pixel(130);
    //        //uwgUserHistory.Columns[2].Width = Unit.Pixel(130);
    //        //uwgUserHistory.Columns[3].Width = Unit.Pixel(200);
    //        //uwgUserHistory.Columns[4].Width = Unit.Pixel(250);
    //        //uwgUserHistory.Columns[5].Width = Unit.Pixel(250);

    //    }

    //}

    private DataRow AddRow(DataRow dr)
    {
        dr[0] = "0";
        dr[1] = "--Select--";
        return dr;
    }

    #endregion

    /// <summary>
    /// populated user datails
    /// </summary>
    /// <author>venkatesh</author>
    /// 
    private void PopulateUserDetails()
    {
        objUser = new ManageUsersBAL();
        //objUser.UserID = Convert.ToInt32(Context.Items["@@UserID"].ToString());

        objUser.UserID = this.UserID;

        uwtManageUsers.SelectedIndex = 0;
        // Make the buttons visibile or invisible depending on User Rights

        //this.ibAssignRFID.Visible = false;
        //this.ibDeassignRFID.Visible = false;
        //this.ibLostRFID.Visible = false;


        DataSet dsUser = objUser.retrieveUserDetails();
        DataTable dtUser = dsUser.Tables[0];
        DataTable dtBU = dsUser.Tables[1];

        if (dtUser.Rows.Count > 0)
        {

            this.lblPassword.Visible = false;
            this.txtPassword.Enabled = false;
            this.txtPassword.Visible = false;
            this.rfvLoginPassword.Enabled = false;
            this.ibGeneratePass.Enabled = false;
            this.ibGeneratePass.Visible = false;
            this.rfvPassword.Visible = false;
            this.rfvPassword.Enabled = false;
            this.rfvLoginPassword.Visible = false;
            this.rfvLoginPassword.Enabled = false;


            this.txtLogin.Text = dtUser.Rows[0]["LoginName"].ToString();
            this.txtLogin.Enabled = false;
            this.txtFirstName.Text = dtUser.Rows[0]["FirstName"].ToString();
            this.txtLastName.Text = dtUser.Rows[0]["LastName"].ToString();
            this.txtEmail.Text = dtUser.Rows[0]["Email"].ToString();

            //this.cbIsAllowUserSelection.Checked = (dtUser.Rows[0]["IsUserSelectionAllowed"].ToString().Equals("True"));
            //V3.8-Added on 17Oct2013-By Amar Vidya
            this.chkEnforceSiteRestriction.Checked = (dtUser.Rows[0]["SiteRestriction"].ToString().Equals("True"));
            //*

            //this.hdnUserGuid.Value = dtUser.Rows[0]["UserGuid"].ToString();
            this.rdoActiveInt.Checked = false;
            this.rdoDisabledInt.Checked = false;
            if (dtUser.Rows[0]["Status"].ToString().Equals("True"))
                this.rdoActiveInt.Checked = true;
            else
                this.rdoDisabledInt.Checked = true;

            //if (dtUser.Rows[0]["CurrentRFIDBadge"] != DBNull.Value)
            //{
            //    this.txtRFIDTag.Text = dtUser.Rows[0]["CurrentRFIDBadge"].ToString();
            //    this.txtRFIDAssigned.Text = dtUser.Rows[0]["RFIDAssignDate"].ToString();
            //    this.txtRFIDAssigned.Visible = true;
            //}
            //else
            //{
            //    this.txtRFIDTag.Text = "Un-Assigned";
            //    this.txtRFIDAssigned.Visible = false;
            //}

            if (dtBU.Rows.Count > 0)
            {
                for (int i = 0; i < dtBU.Rows.Count; i++)
                {
                    for (int j = 0; j < cblBusinessUnit.Items.Count; j++)
                    {
                        if (cblBusinessUnit.Items[j].Value.Equals(dtBU.Rows[i][0].ToString()))
                            cblBusinessUnit.Items[j].Selected = true;
                    }
                }
            }
            //this.ibCreateInternal.ImageUrl = "images/Buttob/bluesave.gif"; -- 07-May


            if (dtUser.Rows[0]["DefaultBU"] != DBNull.Value)
            {
                ddlBusinessUnit.SelectedValue = dtUser.Rows[0]["DefaultBU"].ToString();
                FillDropdownsByBU();
            }

            if (dtUser.Rows[0]["DefaultSite"] != DBNull.Value)
            {
                ddlPrimarySite.SelectedValue = dtUser.Rows[0]["DefaultSite"].ToString();
                FillLocationsBySite();
            }

            if (dtUser.Rows[0]["DefaultLocation"] != DBNull.Value)
            {
                ddlLocation.SelectedValue = dtUser.Rows[0]["DefaultLocation"].ToString();
            }

            //if (dtUser.Rows[0]["DepartmentID"] != DBNull.Value)
            //{
            //    ddlDepartment.SelectedValue = dtUser.Rows[0]["DepartmentID"].ToString();
            //}

            //string strRFIDCardNo = Convert.IsDBNull(dtUser.Rows[0]["CurrentRFIDBadge"]) ? "" : dtUser.Rows[0]["CurrentRFIDBadge"].ToString();
            //if (strRFIDCardNo.Trim() != "")
            //{
            //if (_dtRights.Select("Module = 'Manage Users' and Rights = '" + "De-assign RFID" + "'").Length != 0)
            //{
            //    this.ibDeassignRFID.Visible = true;
            //}
            //else
            //{
            //    ibDeassignRFID.Visible = false;
            //}
            //if (_dtRights.Select("Module = 'Manage Users' and Rights = '" + "Lost RFID" + "'").Length != 0)
            //{
            //    this.ibLostRFID.Visible = true;
            //}
            //else
            //{
            //    ibLostRFID.Visible = false;
            //}

            //}
            //else // RFID Badge is null, only can Assign the card
            //{

            //    if (_dtRights.Select("Module = 'Manage Users' and Rights = '" + "Assign RFID" + "'").Length != 0)
            //    {
            //        this.ibAssignRFID.Visible = true;
            //    }
            //    else
            //    {
            //        ibAssignRFID.Visible = false;
            //    }

            //}


        }

    }


    private void populateRFIDStatus()
    {
        objUser = new ManageUsersBAL();
        //objUser.UserID = Convert.ToInt32(Context.Items["@@UserID"].ToString());

        objUser.UserID = this.UserID;

        //uwtManageUsers.SelectedTab = 0;
        // Make the buttons visibile or invisible depending on User Rights

        //this.ibAssignRFID.Visible = false;
        //this.ibDeassignRFID.Visible = false;
        //this.ibLostRFID.Visible = false;


        DataSet dsUser = objUser.retrieveUserDetails();
        DataTable dtUser = dsUser.Tables[0];
        DataTable dtBU = dsUser.Tables[1];

        if (dtUser.Rows.Count > 0)
        {
            //if (dtUser.Rows[0]["CurrentRFIDBadge"] != DBNull.Value)
            //{
            //    this.txtRFIDTag.Text = dtUser.Rows[0]["CurrentRFIDBadge"].ToString();
            //    this.txtRFIDAssigned.Text = dtUser.Rows[0]["RFIDAssignDate"].ToString();
            //    this.txtRFIDAssigned.Visible = true;
            //}
            //else
            //{
            //    this.txtRFIDTag.Text = "Un-Assigned";
            //    this.txtRFIDAssigned.Visible = false;
            //}

            //string strRFIDCardNo = Convert.IsDBNull(dtUser.Rows[0]["CurrentRFIDBadge"]) ? "" : dtUser.Rows[0]["CurrentRFIDBadge"].ToString();
            //if (strRFIDCardNo.Trim() != "")
            //{
            //if (_dtRights.Select("Module = 'Manage Users' and Rights = '" + "De-assign RFID" + "'").Length != 0)
            //{
            //    this.ibDeassignRFID.Visible = true;
            //}
            //else
            //{
            //    ibDeassignRFID.Visible = false;
            //}
            //if (_dtRights.Select("Module = 'Manage Users' and Rights = '" + "Lost RFID" + "'").Length != 0)
            //{
            //    this.ibLostRFID.Visible = true;
            //}
            //else
            //{
            //    ibLostRFID.Visible = false;
            //}

            //}
            //else // RFID Badge is null, only can Assign the card
            //{

            //    //if (_dtRights.Select("Module = 'Manage Users' and Rights = '" + "Assign RFID" + "'").Length != 0)
            //    //{
            //    //    this.ibAssignRFID.Visible = true;
            //    //}
            //    //else
            //    //{
            //    //    ibAssignRFID.Visible = false;
            //    //}

            //}
        }
    }

    /// <summary>
    /// this code refers to Active Directory Searc and commented.
    /// </summary>
    /// <author>venkatesh</author>
    /// 
    protected void ibInternalSearch_Click(object sender, ImageClickEventArgs e)
    {


        //this.ibCreateInternal.Visible = true;
        //this.lblMessage.Text = string.Empty;

        //if (string.IsNullOrEmpty(txtLogin.Text.Trim()))
        //{
        //    lblMessage.ForeColor = System.Drawing.Color.Red;
        //    lblMessage.Visible = true;
        //    lblMessage.Text = objCommon.displayMessage(MessageCodes.LOGIN_NAME_REQUIRED);
        //    txtLogin.Text = string.Empty;
        //    return;
        //}

        //bool blCheck = this.checkUser(txtLogin.Text, UserType.Internal);
        //if (blCheck)
        //{
        //    if (this.checkUserLocalDb(this.txtLogin.Text.Trim()))
        //    {
        //        lblMessage.ForeColor = System.Drawing.Color.Red;
        //        objCommon = new CommonBAL();
        //        lblMessage.Text = objCommon.displayMessage(MessageCodes.MANAGE_USER_EXISTS);
        //        this.ibCreateInternal.Visible = false;
        //        return;
        //    }
        //}
        //else
        //{
        //    txtFirstName.Text = string.Empty;
        //    txtLastName.Text = string.Empty;
        //    txtEmail.Text = string.Empty;
        //    hdnUserGuid.Value = string.Empty;
        //    cbIsAllowUserSelection.Checked = false;
        //    this.ibCreateInternal.Visible = false;
        //    lblMessage.ForeColor = System.Drawing.Color.Red;
        //    objCommon = new CommonBAL();
        //    lblMessage.Text = objCommon.displayMessage(MessageCodes.MANAGE_USER_LDAP_DIR);
        //    return;
        //}
    }

    //HP: Added to check if user exists in Active Directory.
    /// <summary>
    /// Checks if user exists in Active Directory given by userName (sAMAccountName) and userType (Internal or
    /// External).
    /// </summary>
    /// <author>Sivashanmugam, Muniappan</author>
    /// <createdOn>07 Aug 2007</createdOn>
    private bool checkUser(string strUserName, UserType userType)
    {
        objCommon = new CommonBAL();
        bool retVal = false;

        try
        {
            MembershipUser user = null;
            if (userType == UserType.Internal)
            {
                user = Membership.Domains["InternalDomain"].GetUser(strUserName);
                String displayName = user.DisplayName;
                String givenName = user.GivenName;
                txtFirstName.Text = givenName;
                txtLastName.Text = displayName.Replace(givenName, String.Empty);
                txtEmail.Text = user.Email;
            }
            hdnUserGuid.Value = String.Empty;
            retVal = true;
        }
        catch (MembershipException b)
        {
            if (userType == UserType.Internal)
                lblMessage.Text = objCommon.displayMessage(b.Message);
            retVal = false;
        }
        catch (Exception x)
        {
            if (userType == UserType.Internal)
                lblMessage.Text = objCommon.displayMessage(x.Message);
            retVal = false;
        }
        return retVal;
    }
    /// <summary>
    /// sending E-mail-Default network credentials
    /// </summary>
    /// <author>Vidya</author>
    /// 
    private void SendMsgWithDefaultNetworkCredentials(bool IsReset)
    {
        try
        {
            MailMessage message = new MailMessage(); ;
            string URLRoot = Utility.GetUrlRootPath();
            if (IsReset)
            {
                message.From = new MailAddress(Session["EmailId"].ToString());
                message.To.Add(new MailAddress(this.txtEmail.Text.Trim()));
                message.Subject = "Your Password has been reset for " + ConfigurationManager.AppSettings["CompanyName"].ToString() + " account";
                message.Body = "User information is as follows:" + "<br></br>" + "<b>Login Name :</b>" + this.txtLogin.Text + "<br></br>" + "<b>NewPassword :</b>" + rdmPassword + "<br></br>" + "<b>Website :</b>" + URLRoot + "/Login.aspx" + "<br></br>" + "After login in, please change your password by selecting Change Password option ." + "<br></br>" + "<b><font color='DarkGray'>PLEASE DO NOT REPLY TO THIS MESSAGE, this is an automated mail to inform you that your password has been reset.</font></b>";
                lblMessage.Visible = true;
                lblMessage.Text = "User account reset is successful!";
            }
            else
            {
                message.From = new MailAddress(Session["EmailId"].ToString());
                message.To.Add(new MailAddress(this.txtEmail.Text.Trim()));
                message.Subject = "Your user account for " + ConfigurationManager.AppSettings["CompanyName"].ToString() + " application is created successfully";
                message.Body = "User information is as follows:" + "<br></br>" + "<b>Login Name :</b>" + this.txtLogin.Text + "<br></br>" + "<b>Password :</b>" + txtPassword.Text + "<br></br>" + "<b>Website :</b>" + URLRoot + "/Login.aspx" + "<br></br>" + "After login in, please change your password by selecting Change Password option ." + "<br></br>" + "<b><font color='DarkGray'>PLEASE DO NOT REPLY TO THIS MESSAGE, this is an automated mail to inform you that your account is created.</font></b>";

                lblMessage.Visible = true;
                lblMessage.Text = objCommon.displayMessage(MessageCodes.USER_CREATE_SUCCESS);
            }

            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SmtpServer"]);
            client.Send(message);
            client.Credentials = CredentialCache.DefaultNetworkCredentials;

        }
        catch (Exception ex)
        {
            ExceptionPolicy.HandleException(ex, "errDCTrack");
            if (IsReset)
            {

                this.ShowMessage(Microsoft.Security.Application.Encoder.JavaScriptEncode("New Password:" + rdmPassword +
                "\\r\\nPlease share these details with user"), "nourl");
                lblMessage.Text = "User account reset is successful!" + " (New Password:" + rdmPassword + ")";
                ShowMessage("New Password:" + rdmPassword + "\\r\\n" + "Please share this password with user", "nourl");

            }
            else
            {
                this.ShowMessage(Microsoft.Security.Application.Encoder.JavaScriptEncode("New Login Name:" + txtLogin.Text + "\\r\\n New Password:" + txtPassword.Text +
                  "\\r\\nPlease share these details with user"), "nourl");
                lblMessage.Text = Microsoft.Security.Application.Encoder.HtmlEncode(objCommon.displayMessage(MessageCodes.USER_CREATE_SUCCESS) + " (New UserID:" +
                   txtLogin.Text + ",Password:" + txtPassword.Text + ")");
            }
        }


    }


    /// <summary>
    /// check user local database exist or not
    /// </summary>
    /// <author>venkatesh</author>
    /// 
    private bool checkUserLocalDb(string strLoginName)
    {
        objUser = new ManageUsersBAL();
        objUser.LoginName = strLoginName;
        objUser.UserID = objUser.exists();
        if (objUser.UserID != 0)
            return true;
        else
            return false;
    }
    /// <summary>
    /// validate businessunit
    /// </summary>
    /// <author>venkatesh</author>
    /// 
    private bool ValidateBusinessUnit()
    {

        bool returnVal = false;
        bool IsDefaultBUValid = false;
        for (int i = 0; i < cblBusinessUnit.Items.Count; i++)
        {
            //Check if the BU is selected from one of the BU list


            if (cblBusinessUnit.Items[i].Selected)
            {
                if (cblBusinessUnit.Items[i].Value.ToString() == ddlBusinessUnit.SelectedValue)
                {
                    IsDefaultBUValid = true;
                }
                returnVal = true;
                //break;
            }
        }

        if (!IsDefaultBUValid)
        {
            this.lblDefaultBUInvalid.Visible = true;
            return false;
        }
        else
        {
            this.lblDefaultBUInvalid.Visible = false;
        }

        return returnVal;
    }
    /// <summary>
    /// create internal user
    /// </summary>
    /// <author>venkatesh</author>
    /// 

    protected void ibCreateInternal_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {

        //for tenant
        if (bool.Parse(Session["TenantUser"].ToString()))
        {
            UserBAL objUser = new UserBAL();
            objUser.UserID = Convert.ToInt32(Session["UserID"]);
            DataSet dsTenant = objUser.retrieveTenantDetails();
            if (dsTenant.Tables.Count > 0 && dsTenant.Tables[0].Rows.Count > 0)
            {
                if (!txtLogin.Text.ToLower().StartsWith(dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_SHORT_NAME].ToString().ToLower() + "."))
                {
                    lblMessage.Text = "Login name must be prefixed with Tenant short name followed by dot(.)";
                    lblMessage.Visible = true;
                    return;

                }
            }

        }

        if (!this.ValidateBusinessUnit())
        {
            lblMessage.ForeColor = System.Drawing.Color.Red;
            objCommon = new CommonBAL();
            lblMessage.Text = objCommon.displayMessage(MessageCodes.MANAGE_USER_BU_REQ);
            lblMessage.Visible = true;
            return;
        }

        if (this.txtLogin.Enabled == true)
        {
            this.ibCreateInternal.Visible = true;
            this.lblMessage.Text = string.Empty;
            //bool blCheck = this.checkUser(txtLogin.Text, UserType.Internal);
            //if (blCheck)
            //{
            if (this.checkUserLocalDb(this.txtLogin.Text.Trim()))
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                objCommon = new CommonBAL();
                lblMessage.Text = objCommon.displayMessage(MessageCodes.MANAGE_USER_EXISTS);
                this.ibCreateInternal.Visible = true;
                lblMessage.Visible = true;
                return;

            }
            //                }
            else
            {
                //txtFirstName.Text = string.Empty;
                //txtLastName.Text = string.Empty;
                //txtEmail.Text = string.Empty;
                //hdnUserGuid.Value = string.Empty;
                //lblMessage.ForeColor = System.Drawing.Color.Red;
                objCommon = new CommonBAL();
                //lblMessage.Text = objCommon.displayMessage(MessageCodes.MANAGE_USER_LDAP_DIR);
                this.SaveInternalUser();

            }
        }
        else
        {
            objCommon = new CommonBAL();
            this.SaveInternalUser();
        }
    }

    /// <summary>
    /// save internal user
    /// </summary>
    /// <author>venkatesh</author>
    /// 
    private void SaveInternalUser()
    {

        //Check if the BU is selected from one of the BU list
        objUser = new ManageUsersBAL();
        objUser.LoginName = this.txtLogin.Text.Trim();
        objUser.FirstName = this.txtFirstName.Text.Trim();
        objUser.LastName = this.txtLastName.Text.Trim();
        objUser.Email = this.txtEmail.Text.Trim();
        //CR3001:Password Expiration, by kjb on 09 Jan 2012 -- Begin
        double expirationDays = Convert.ToDouble(ConfigurationManager.AppSettings["PwdExpiryDays"].ToString());
        objUser.ExpiryDate = DateTime.Now.AddDays(expirationDays);
        //CR3001:Password Expiration, by kjb on 09 Jan 2012 -- End

        objUser.Password = objCommon.GetSHA256HashValue(this.txtPassword.Text.Trim());
        objUser.IsUserSelectionAllowed = (this.cbIsAllowUserSelection.Checked ? 1 : 0);

        if (rdoActiveInt.Checked == true)
            objUser.Status = 1;
        string strBusinessUnitIDs = string.Empty;
        string GroupDelimiter = ";";

        for (int iCol = 0; iCol < cblBusinessUnit.Items.Count; iCol++)
        {
            if (cblBusinessUnit.Items[iCol].Selected == true)
            {

                strBusinessUnitIDs += cblBusinessUnit.Items[iCol].Value.ToString() + GroupDelimiter;
            }
        }

        objUser.BusinessUnitIDs = strBusinessUnitIDs;
        objUser.Delimiters = ";";
        objUser.CreatedBy = Convert.ToInt32(Session["UserID"].ToString());


        objUser.BusinessUnitID = Convert.ToInt32(ddlBusinessUnit.SelectedValue);
        objUser.SiteID = Convert.ToInt32(ddlPrimarySite.SelectedValue);
        //objUser.DepartmentID = Convert.ToInt32(ddlDepartment.SelectedValue);
        objUser.LocationID = Convert.ToInt32(ddlLocation.SelectedValue);

        //if (!string.IsNullOrEmpty(lblUserID.Text))
        //objUser.UserID = Convert.ToInt32(lblUserID.Text);
        //else
        //    objUser.UserID = -1;

        objUser.UserID = this.UserID;
        //V3.8-Added on 17Oct2013-By Amar Vidya
        objUser.SiteRestriction = chkEnforceSiteRestriction.Checked ? 1 : 0;

        //string test = EnforceSiteRestrictionRights();
        //string Sites = SiteIDs();
        //string Operations = Operation();
        //objUser.SiteIDs = SiteIDs();
        // objUser.Operations = Operation();
        objUser.SiteIDs = EnforceSiteRestrictionRights();

        // pnlEnforceSiteRestriction.Visible = true;
        //*

        objUser.Persist(DALCOperation.Insert);
        //if (!string.IsNullOrEmpty(lblUserID.Text))

        # region v3.8
        //v3.8
        //if the logged in user and modified user are same 
        // than site restriction flag needs to be updated.
        if (this.UserID == int.Parse(Session["UserID"].ToString()))
        {
            Session["SiteRestrictEnabled"] = chkEnforceSiteRestriction.Checked;
        }
        #endregion

        if (this.UserID > 0)
        {
            objCommon = new CommonBAL();
            //lblMessage.Visible = true;
            //lblMessage.Text = objCommon.displayMessage(MessageCodes.USER_UPDATE_SUCCESS);
            Session["@@UserID"] = null;
            Session["@@Name"] = null;
            this.ShowMessage("'" + objCommon.displayMessage(MessageCodes.USER_UPDATE_SUCCESS) + "'", "UserSearch.aspx");
        }
        else
        {
            if (!string.IsNullOrEmpty(objUser.MessageCode))
            {
                //user creation failed
                lblMessage.Text = objUser.MessageCode;
                lblMessage.Visible = true;
                return;
            }
            else
            {
                objCommon = new CommonBAL();

                if (Convert.ToInt32(ConfigurationManager.AppSettings["SendEmail"].ToString()) == 0)//No mail
                {
                    this.ShowMessage(Microsoft.Security.Application.Encoder.JavaScriptEncode("New Login Name:" + txtLogin.Text + "\\r\\n New Password:" + txtPassword.Text +
                    "\\r\\nPlease share these details with user"), "nourl");
                    lblMessage.Visible = true;
                    lblMessage.Text = Microsoft.Security.Application.Encoder.HtmlEncode(objCommon.displayMessage(MessageCodes.USER_CREATE_SUCCESS) + " (New UserID:" +
                        txtLogin.Text + ",Password:" + txtPassword.Text + ")");
                }
                else if (Convert.ToInt32(ConfigurationManager.AppSettings["SendEmail"].ToString()) == 1)//DefaultNetworkCredentials
                {
                    SendMsgWithDefaultNetworkCredentials(false);
                }
                else if (Convert.ToInt32(ConfigurationManager.AppSettings["SendEmail"].ToString()) == 2)//With User Credentials
                {
                    SendMsgWithUserCredentials(false);
                }
            }
        }

        this.ClearInternal();

    }
    /// <summary>
    /// clear internal data
    /// </summary>
    /// <author>venkatesh</author>
    /// 
    private void ClearInternal()
    {
        lblUserID.Text = string.Empty;
        lblUserType.Text = string.Empty;
        txtLogin.Enabled = true;
        this.txtLogin.Text = string.Empty;
        this.txtPassword.Text = string.Empty;
        this.txtFirstName.Text = string.Empty;
        this.txtLastName.Text = string.Empty;
        this.txtEmail.Text = string.Empty;
        this.hdnUserGuid.Value = string.Empty;
        this.cbIsAllowUserSelection.Checked = false;
        for (int iCol = 0; iCol < cblBusinessUnit.Items.Count; iCol++)
        {
            this.cblBusinessUnit.Items[iCol].Selected = true;
        }
        this.rdoActiveInt.Checked = true;
        this.rdoDisabledInt.Checked = false;

        txtPassword.Text = ""; // added by kjb on 09 Jan 2012
        txtPassword.Attributes["value"] = "";
        this.ddlBusinessUnit.SelectedIndex = 1;
        this.ddlPrimarySite.SelectedIndex = 0;
        this.ddlLocation.SelectedIndex = 0;

        //*V3.8-Added on 17Oct2013-By Amar Vidya
        chkEnforceSiteRestriction.Checked = false;
        pnlEnforceSiteRestriction.Attributes.Add("style", " display:none");
        DropDownList ddlSiteOperation;

        foreach (GridRecord rec in grdSiteRestriction.Rows)
        {
            ddlSiteOperation = (DropDownList)(rec.Items[2].FindControl("ddlSiteOperation"));
            ddlSiteOperation.SelectedIndex = 0;
        }//*

    }

    protected void ibCancelInternal_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        ClosePage();
    }

    private void ClosePage()
    {
        if (this.UserID > 0)
        {
            // Added by Debasish
            Session["@@UserID"] = null;
            Session["@@Name"] = null;
            Response.Redirect("UserSearch.aspx");
        }
        else
        {
            Response.Redirect("MainPage.aspx");
        }
    }


    /// <summary>
    /// Generates random password and displays it in txtPassword field
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author>kjb</author>
    protected void ibGeneratePass_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        string strPass = string.Empty;
        strPass = GenerateRandomPassword();

        txtPassword.Attributes["value"] = strPass;
        //string strScript = "<script type=\"text/javascript\"> function updatePassword() { var txtPass = document.getElementById('<%=txtPassword.ClientID%>'); " +
        //    " txtPass.value = '" + strPass + "'; } </script>";
        //if (!Page.ClientScript.IsStartupScriptRegistered("FORMMESSAGE"))
        //    Page.ClientScript.RegisterStartupScript(typeof(Page), "FORMMESSAGE", strScript);
        if (lblMessage.Visible)
            lblMessage.Visible = false;
        //*V3.8-Added on 17Oct2013-By Amar Vidya
        if (chkEnforceSiteRestriction.Checked == true)
        {
            pnlEnforceSiteRestriction.Attributes.Add("style", " display:inline");
        }
        else
        {
            pnlEnforceSiteRestriction.Attributes.Add("style", " display:none");
        }//*
    }




    /// <summary>
    /// Unlock user and resets the password(this is used by administrator's only)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author>kjb</author>
    protected void ibResetPass_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        try
        {
            //this will unlock The user 
            // and also resets the password
            rdmPassword = GenerateRandomPassword();
            UserLoginBAL objULBAL = new UserLoginBAL();
            objULBAL.LoginName = txtLogin.Text;
            objCommon = new CommonBAL();
            objULBAL.Password = objCommon.GetSHA256HashValue(rdmPassword);
            //CR3001:Password Expiration, by kjb on 09 Jan 2012 -- Begin
            double expirationDays = Convert.ToDouble(ConfigurationManager.AppSettings["PwdExpiryDays"].ToString());
            objULBAL.ExpiryDate = DateTime.Now.AddDays(expirationDays);
            objULBAL.CreatedBy = Convert.ToInt32(Session["UserID"]);
            objULBAL.Persist(DALCOperation.Insert);
            if (!string.IsNullOrEmpty(objULBAL.ExceptionType.ToString()))
            {
                //If exception type has value (it will be password already used) 
                //than call reset mathod again to try again.
                //Generally password generated using random method shouldn't create a duplicate password.
                ibResetPass_Click(sender, e);
                    //lblMessage.Visible = true;
                    //objCommon = new CommonBAL();
                    //lblMessage.Text = objCommon.displayMessage(objULBAL.ExceptionType);
            }
            else
            {

                //if user (whose password is reset) is currently logged in that his/her session will be removed forcing user
                // to login again.
                Dictionary<string, string> dic = null;
                try
                {
                    if (Application["Sessions"] != null)
                    {
                        dic = ((Dictionary<string, string>)Application["Sessions"]);

                        if (dic.ContainsKey(txtLogin.Text.Trim().ToLower()))
                        {

                            // remove old record from Application[Sessions]
                            ((Dictionary<string, string>)Application["Sessions"]).Remove(txtLogin.Text.Trim().ToLower());
                        }
                    }

                }
                catch
                {

                }

                if (Convert.ToInt32(ConfigurationManager.AppSettings["SendEmail"].ToString()) == 0)//No mail
                {
                    lblMessage.Text = "User account reset is successful!" + " (New Password:" + rdmPassword + ")";
                    ShowMessage("New Password:" + rdmPassword + "\\r\\n" + "Please share this password with user", "nourl");

                }
                else if (Convert.ToInt32(ConfigurationManager.AppSettings["SendEmail"].ToString()) == 1)//DefaultNetworkCredentials
                {
                    SendMsgWithDefaultNetworkCredentials(true);
                }
                else if (Convert.ToInt32(ConfigurationManager.AppSettings["SendEmail"].ToString()) == 2)//With User Credentials
                {
                    SendMsgWithUserCredentials(true);
                }
            }
        }
        catch (Exception ex)
        {
            //lblMessage.Text = "Reset Failed";
            //lblMessage.Visible = true;
            //lblMessage.ForeColor = Color.Red;
            ExceptionPolicy.HandleException(ex, "errDCTrack");
            throw ex;
        }
    }


    private void SendMsgWithUserCredentials(bool IsReset)
    {

        try
        {
            MailMessage message = new MailMessage();
            string URLRoot = Utility.GetUrlRootPath();
            if (IsReset)
            {
                //message.From = new MailAddress(Session["EmailId"].ToString());
                message.To.Add(new MailAddress(this.txtEmail.Text.Trim()));
                message.Subject = "Your Password has been reset for " + ConfigurationManager.AppSettings["CompanyName"].ToString() + " account";
                //message.Body = "User information is as follows:" + "<br></br>" + "<b>User Id :</b>" + this.txtLogin.Text + "<br></br>" + "<b>NewPassword :</b>" + rdmPassword + "<br></br>" + "Please change your password when you login next time." + "<br></br>" + "<b><font color='DarkGray'>PLEASE DO NOT REPLY TO THIS MESSAGE, this is an automated mail to inform you that your password has been reset.</font></b>";
                //message.Body = "User information is as follows:" + "<br></br>" + "<b>Login Name :</b>" + this.txtLogin.Text + "<br></br>" + "<b>NewPassword :</b>" + rdmPassword + "<br></br>" + "<b>Website :</b>" + URLRoot + "/Login.aspx" + "<br></br>" + "Please change your password when you login next time." + "<br></br>" + "<b><font color='DarkGray'>PLEASE DO NOT REPLY TO THIS MESSAGE, this is an automated mail to inform you that your password has been reset.</font></b>";
                //message.Body = "User information is as follows:" + "<br></br>" + "<b>Login Name :</b>" + this.txtLogin.Text + "<br></br>" + "<b>NewPassword :</b>" + rdmPassword + "<br></br>" + "<b>Website :</b>" + URLRoot + "/Login.aspx" + "<br></br>" + "After login in, please change your password by selecting User Administration->Change Password.." + "<br></br>" + "<b><font color='DarkGray'>PLEASE DO NOT REPLY TO THIS MESSAGE, this is an automated mail to inform you that your password has been reset.</font></b>";//V3.8-Commented on 14Oct2013-By Amar vidya
                message.Body = "User information is as follows:" + "<br></br>" + "<b>Login Name :</b>" + this.txtLogin.Text + "<br></br>" + "<b>NewPassword :</b>" + rdmPassword + "<br></br>" + "<b>Website :</b>" + URLRoot + "/Login.aspx" + "<br></br>" + "After login in, please change your password by selecting 'change password' hyperlink on top right" + "<br></br>" + "<b><font color='DarkGray'>PLEASE DO NOT REPLY TO THIS MESSAGE, this is an automated mail to inform you that your password has been reset.</font></b>";//V3.8-Modified on 14Oct2013-By Amar vidya
                lblMessage.Visible = true;
                lblMessage.Text = "User account reset is successful!";
            }
            else
            {
                //message.From = new MailAddress(Session["EmailId"].ToString());
                message.To.Add(new MailAddress(this.txtEmail.Text.Trim()));
                message.Subject = "Your user account for " + ConfigurationManager.AppSettings["CompanyName"].ToString() + " application is created successfully";
                //message.Body = "User information is as follows:" + "<br></br>" + "<b>User Id :</b>" + this.txtLogin.Text + "<br></br>" + "<b>Password :</b>" + txtPassword.Text + "<br></br>" + "Please change your password when you login next time." + "<br></br>" + "<b><font color='DarkGray'>PLEASE DO NOT REPLY TO THIS MESSAGE, this is an automated mail to inform you that your account is created.</font></b>";
                //message.Body = "User information is as follows:" + "<br></br>" + "<b>Login Name :</b>" + this.txtLogin.Text + "<br></br>" + "<b>Password :</b>" + txtPassword.Text + "<br></br>" + "<b>Website :</b>" + URLRoot + "/Login.aspx" + "<br></br>" + "Please change your password when you login next time." + "<br></br>" + "<b><font color='DarkGray'>PLEASE DO NOT REPLY TO THIS MESSAGE, this is an automated mail to inform you that your account is created.</font></b>";
                //message.Body = "User information is as follows:" + "<br></br>" + "<b>Login Name :</b>" + this.txtLogin.Text + "<br></br>" + "<b>Password :</b>" + txtPassword.Text + "<br></br>" + "<b>Website :</b>" + URLRoot + "/Login.aspx" + "<br></br>" + "After login in, please change your password by selecting User Administration->Change Password." + "<br></br>" + "<b><font color='DarkGray'>PLEASE DO NOT REPLY TO THIS MESSAGE, this is an automated mail to inform you that your account is created.</font></b>";//V3.8-Commented on 14Oct2013-By Amar vidya
                message.Body = "User information is as follows:" + "<br></br>" + "<b>Login Name :</b>" + this.txtLogin.Text + "<br></br>" + "<b>Password :</b>" + txtPassword.Text + "<br></br>" + "<b>Website :</b>" + URLRoot + "/Login.aspx" + "<br></br>" + "After login in, please change your password by selecting 'change password' hyperlink on top right" + "<br></br>" + "<b><font color='DarkGray'>PLEASE DO NOT REPLY TO THIS MESSAGE, this is an automated mail to inform you that your account is created.</font></b>";//V3.8-Modified on 14Oct2013-By Amar vidya
                lblMessage.Visible = true;
                lblMessage.Text = objCommon.displayMessage(MessageCodes.USER_CREATE_SUCCESS);
            }


            message.IsBodyHtml = true;
            var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            string username = smtpSection.Network.UserName;
            string password = smtpSection.Network.Password;
            string host = smtpSection.Network.Host;


            // Instantiate a new instance of SmtpClient
            SmtpClient client = new SmtpClient();
            client.Host = host;
            client.Credentials = new NetworkCredential(username, password);
            client.Send(message);
        }
        catch (Exception ex)
        {
            ExceptionPolicy.HandleException(ex, "errDCTrack");
            if (IsReset)
            {
                this.ShowMessage(Microsoft.Security.Application.Encoder.JavaScriptEncode("New Password:" + rdmPassword +
                "\\r\\nPlease share these details with user"), "nourl");
                lblMessage.Visible = true;
                lblMessage.Text = "User account reset is successful!" + " (New Password:" + rdmPassword + ")";
                ShowMessage("New Password:" + rdmPassword + "\\r\\n" + "Please share this password with user", "nourl");
            }
            else
            {
                this.ShowMessage(Microsoft.Security.Application.Encoder.JavaScriptEncode("New Login Name:" + txtLogin.Text + "\\r\\n New Password:" + txtPassword.Text +
                  "\\r\\nPlease share these details with user"), "nourl");
                lblMessage.Visible = true;
                lblMessage.Text = Microsoft.Security.Application.Encoder.HtmlEncode(objCommon.displayMessage(MessageCodes.USER_CREATE_SUCCESS) + " (New UserID:" +
                   txtLogin.Text + ",Password:" + txtPassword.Text + ")");
            }
        }
    }

    private string GenerateRandomPassword()
    {
        string strLetters = "QAabcLdUKeHfJgTPXhiFjDOklNmnBoIpGqYVrsCtSuMZvwWxyEzR";
        string strNumerics = "0123456789";
        StringBuilder sb = new StringBuilder();
        Random r = new Random();
        int te = 0;
        for (int i = 1; i <= 9; i++) // 9 chars length
        {
            te = r.Next(52);
            sb.Append(strLetters.Substring(te, 1));
        }
        //numeric - 1 
        r = new Random();
        te = 0;
        for (int i = 1; i <= 1; i++) // 1
        {
            te = r.Next(10);
            sb.Append(strNumerics.Substring(te, 1));
        }
        //check password
        string pass = sb.ToString();
        string pattern = "(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,10})$";
        Regex regxVal = new Regex(pattern);
        Match validate = regxVal.Match(pass);
        if (!validate.Success)
        {
            GenerateRandomPassword();
        }

        return sb.ToString();
    }

    /// <summary>
    /// To register alert message for user actions.
    /// </summary>
    /// <param name="mess"></param>
    /// <param name="URL"></param>
    private void ShowMessage(string mess, string URL)
    {
        string strScript = "<script type=\"text/javascript\">validNavigation = true;alert(" + mess + ")</script>";
        if (!string.IsNullOrEmpty(URL) && URL.ToLower().CompareTo("nourl") != 0)
            strScript = strScript + "<script type=\"text/javascript\">window.location.replace('" + URL + "');</script>";

        if (!Page.ClientScript.IsStartupScriptRegistered("FORMMESSAGE"))
            Page.ClientScript.RegisterStartupScript(typeof(Page), "FORMMESSAGE", strScript.Replace("\\x3a", ":").Replace("\\x5cr", "\\r").Replace("\\x5cn", "\\n"));
    }

    /// <summary>
    /// cancel redirect to UserSearch page
    /// </summary>
    /// <author>venkatesh</author>
    /// 
    protected void ibCancelExternal_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        Response.Redirect("UserSearch.aspx");
    }


    /// <summary>
    /// cancel redirect to UserSearch page
    /// </summary>
    /// <author>venkatesh</author>
    /// 
    protected void btnAssignRightInt_Click(object sender, EventArgs e)
    {
        Response.Redirect("AssignRights.aspx");
    }

    //protected void uwtManageUsers_TabClick(object sender, Infragistics.WebUI.UltraWebTab.WebTabEvent e)
    //{
    //    switch (e.Tab.Key)
    //    {
    //        case TAB_KEY_HISTORY:

    //            //if (!this.IsHistoryTabInitialized)
    //            //{
    //            //    FillUserHistory();
    //            //    this.IsHistoryTabInitialized = true;
    //            //}

    //            break;

    //    }
    //}


    private void FillDropDownLists()
    {

        //FillDropDownList(StoredProcedures.SP_SITEBYBU_LIST, ref ddlPrimarySite, 0);

        //int intUserID = 0;
        //if (Session["UserID"] != null)
        //    intUserID = Convert.ToInt32(Session["UserID"].ToString());

        //FillDropDownListBU(StoredProcedures.SP_BUSINESSUNIT_LISTBYUSERID, ref ddlBusinessUnit, intUserID);

        BusinessUnitBAL objBU = new iAssetTrack.BAL.BusinessUnitBAL();
        DataSet dsBU = objBU.retrieve();
        DataTable dtBU = dsBU.Tables[0];

        DataRow dr = dtBU.NewRow();
        dr[0] = 0;
        dr[1] = "-Select-";
        dtBU.Rows.InsertAt(dr, 0);

        ddlBusinessUnit.DataSource = dtBU;
        ddlBusinessUnit.DataValueField = dtBU.Columns[0].ToString();
        ddlBusinessUnit.DataTextField = dtBU.Columns[1].ToString();
        ddlBusinessUnit.DataBind();
    }


    protected void ddlBusinessUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDropdownsByBU();
    }

    protected void ddlPrimarySite_SelectedIndexChanged(object sender, EventArgs e)
    {

        FillLocationsBySite();
        //*V3.8-Added on 17Oct2013-By Amar Vidya
        if (chkEnforceSiteRestriction.Checked == true)
        {
            pnlEnforceSiteRestriction.Attributes.Add("style", " display:inline");
            // to enable read/write permission for the site selected in the default site drop down
            DropDownList ddlSiteOperation;
            if (grdSiteRestriction.Rows.Count > 0)
            {
                foreach (GridRecord rec in grdSiteRestriction.Rows)
                {
                    if (rec.Items[1].Text.CompareTo(ddlPrimarySite.SelectedItem.Text) == 0)
                    {
                        ddlSiteOperation = (DropDownList)(rec.Items[2].FindControl("ddlSiteOperation"));
                        ddlSiteOperation.SelectedIndex = 2; // 2 means Read/Write
                        break;
                    }
                }
            }
        }
        else
        {
            pnlEnforceSiteRestriction.Attributes.Add("style", " display:none");
        }//*
    }

    private void FillDropDownListBU(string strStoredProc, ref DropDownList ddl, int id)
    {

        //CommonBAL objCommonBAL = new CommonBAL();
        //ICommon svcBU = (ICommon)RemotingHelper.CreateProxy(typeof(ICommon));
        iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();

        DataTable dt = objCommon.FillDropDownListBU(strStoredProc, "-Select-", id);
        ddl.DataSource = dt;
        ddl.DataValueField = dt.Columns[0].ToString();
        ddl.DataTextField = dt.Columns[1].ToString();
        ddl.DataBind();
        ddlLocation.Items.Clear();

    }

    private void FillDropDownList(string strStoredProc, ref DropDownList ddl)
    {
        //ICommon svc = (ICommon)RemotingHelper.CreateProxy(typeof(ICommon));
        iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();
        DataTable dt = objCommon.FillDropDownList(strStoredProc, "-Select-");
        ddl.DataSource = dt;
        ddl.DataValueField = dt.Columns[0].ToString();
        ddl.DataTextField = dt.Columns[1].ToString();
        ddl.DataBind();
    }


    private void FillDropDownList(string strStoredProc, ref DropDownList ddl, int id)
    {
        //ICommon svc = (ICommon)RemotingHelper.CreateProxy(typeof(ICommon));
        iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();
        DataTable dt = objCommon.FillDropDownList(strStoredProc, "-Select-", id);
        ddl.DataSource = dt;
        ddl.DataValueField = dt.Columns[0].ToString();
        ddl.DataTextField = dt.Columns[1].ToString();
        ddl.DataBind();
    }


    private void FillDropdownsByBU()
    {
        int intBusinessUnitID = 0;
        if (ddlBusinessUnit.SelectedValue != "")
            intBusinessUnitID = Convert.ToInt32(ddlBusinessUnit.SelectedValue);
        FillDropDownList(StoredProcedures.SP_SITEBYBU_LIST, ref ddlPrimarySite, intBusinessUnitID);
        FillDepartmentByBU();
        //hdnUserID.Value = "";
        //hdnUserName.Value = "";
        //txtUser.Text = "";

    }

    private void FillLocationsBySite()
    {
        ddlLocation.Items.Clear();
        int intSiteID = 0;
        if (ddlPrimarySite.SelectedValue != "")
            intSiteID = Convert.ToInt32(ddlPrimarySite.SelectedValue);
        iAssetTrack.BAL.LocationBAL objLocation = new iAssetTrack.BAL.LocationBAL();
        objLocation.SiteID = intSiteID;
        DataSet ds = objLocation.GetLocationBYSite();

        ListItem itm = new ListItem("-Select-", "0");
        ddlLocation.Items.Add(itm);

        ddlLocation.DataSource = ds;
        DataTable dt = ds.Tables[0];
        ddlLocation.DataValueField = dt.Columns["LocationID"].ToString();
        ddlLocation.DataTextField = dt.Columns["Location"].ToString();
        ddlLocation.DataBind();

    }


    private void FillDepartmentByBU()
    {
        //ddlDepartment.Items.Clear();
        int intBusinessUnitID = 0;
        if (ddlBusinessUnit.SelectedValue != "")
            intBusinessUnitID = Convert.ToInt32(ddlBusinessUnit.SelectedValue);
    }

    private string EnforceSiteRestrictionRights()
    {
        string MainString = string.Empty;
        DropDownList ddlSiteOperation;
        //ddlSiteOperation = (DropDownList)(grdSiteRestriction.Items[2].FindControl("ddlSiteOperation"));

        foreach (GridRecord rec in grdSiteRestriction.Rows)
        {
            ddlSiteOperation = (DropDownList)(rec.Items[2].FindControl("ddlSiteOperation"));
            string Row;
            //string Row = rec.Items[0].Text + "#" + ddlSiteOperation.SelectedItem.Value;
            if (ddlSiteOperation.SelectedItem.Value == "0") //No Access
            {

                Row = rec.Items[0].Text + ",0,0";
            }
            else if (ddlSiteOperation.SelectedItem.Value == "1")//Read
            {
                Row = rec.Items[0].Text + ",1,0";
            }
            else// read and write
            {
                Row = rec.Items[0].Text + ",1,1";
            }


            MainString = MainString + Row + "#";
        }
        return MainString.Trim('#');
        //-- end
    }

    private string SiteIDs()
    {
        string SiteIDs = string.Empty;
        //DropDownList ddlSiteOperation;       

        foreach (GridRecord rec in grdSiteRestriction.Rows)
        {
            //ddlSiteOperation = (DropDownList)(rec.Items[2].FindControl("ddlSiteOperation"));
            string Row = rec.Items[0].Text;


            SiteIDs = SiteIDs + Row + ",";
        }
        return SiteIDs.Trim(',');
        //-- end
    }

    private string Operation()
    {
        string Operation = string.Empty;
        DropDownList ddlSiteOperation;

        foreach (GridRecord rec in grdSiteRestriction.Rows)
        {
            ddlSiteOperation = (DropDownList)(rec.Items[2].FindControl("ddlSiteOperation"));
            string Row = ddlSiteOperation.SelectedItem.Value;


            Operation = Operation + Row + ",";
        }
        return Operation.Trim(',');
        //-- end
    }

    protected void grdSiteRestriction_InitializeRow(object sender, RowEventArgs e)
    {
        DropDownList ddlSiteOperation = (DropDownList)(e.Row.Items[2].FindControl("ddlSiteOperation"));
        ddlSiteOperation.SelectedIndex = siteResData[e.Row.Items[1].Text];
    }



    /// <summary>
    /// Populates Site Restriction grid
    /// </summary>
    private void populateGrid()
    {
        if (this.UserID > 0) // when UserID exists -- means user update
        {

            UserBAL objUserBAL = new UserBAL();
            objUserBAL.UserID = this.UserID;
            DataSet dsUser = objUserBAL.retrieve();
            if (bool.Parse(dsUser.Tables[0].Rows[0]["SiteRestriction"].ToString()) == true)
            {
                //Site restriction enabled
                SiteRestrictionsBAL objSRBAL = new SiteRestrictionsBAL();
                objSRBAL.UserID = this.UserID;
                DataSet dsSR = objSRBAL.RetrieveByUserID();
                DataTable dtGrid = dsSR.Tables[0];
                grdSiteRestriction.DataSource = dtGrid;
                grdSiteRestriction.DataBind();
            }
            else
            {
                //// this is to check whether Site is already selected for first timers only.
                //// if sellected than Site must be set to read/Write in the grid.
                //if (ddlPrimarySite.SelectedIndex > 0)
                //{
                //    siteResData[ddlPrimarySite.SelectedItem.Text] = 2;
                //}
                //objSite = new iAssetTrack.BAL.SitesBAL();
                //DataTable dtGrid = objSite.retrieve().Tables[0];
                //dtGrid.Columns.Add(new DataColumn("ResTypeID", typeof(int)));
                //for (int i = 0; i < dtGrid.Rows.Count; i++)
                //{
                //    dtGrid.Rows[i]["ResTypeID"] = "0";
                //}

                //grdSiteRestriction.DataSource = dtGrid;
                //grdSiteRestriction.DataBind();
            }
        }
        else
        {
            //// this is to check whether Site is already selected for first timers only.
            //// if sellected than Site must be set to read/Write in the grid.
            //if (ddlPrimarySite.SelectedIndex > 0)
            //{
            //    siteResData[ddlPrimarySite.SelectedItem.Text] = 2;
            //}

            //objSite = new iAssetTrack.BAL.SitesBAL();
            //DataTable dtGrid = objSite.retrieve().Tables[0];
            //dtGrid.Columns.Add(new DataColumn("ResTypeID", typeof(int)));
            //for (int i = 0; i < dtGrid.Rows.Count; i++)
            //{
            //    dtGrid.Rows[i]["ResTypeID"] = "0";
            //}

            //grdSiteRestriction.DataSource = dtGrid;
            //grdSiteRestriction.DataBind();
        }
    }
    protected void chkEnforceSiteRestriction_OnCheckedChanged(Object sender, EventArgs args)
    {
        populateGrid();
    }


}
