/*
File Name:AssignRights.aspx.cs

Description:	Assgin rights to users

Date created:	17 October 2006

Modification History:
***********************
CR		Name			Date			Description
New		venkatesh 		17/10/2006		File has been created.
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
using iAssetTrackBAL;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

public partial class ASPX_AssignRights : System.Web.UI.Page
{
    #region "Declarations"
    private iAssetTrack.BAL.GroupBAL objGroup;
    private iAssetTrack.BAL.CommonBAL objCommon;
    private iAssetTrack.BAL.UserBAL objUser;
    private iAssetTrack.BAL.AssignRightsBAL objAssignRights;
    DataTable _dtRights;
    private string tenantAssignedGroups = string.Empty;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        Session["PageHeader"] = "Assign Groups";

        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "UserSearch.aspx";
            Response.Redirect("Login.aspx");
        }


        if (!IsPostBack)
        {
            this.PopulateGroup();
            this.PolpulateValues();
        }
    }

    #region Lookups
    /// <summary>
    /// Populate group drop down based on the user type.
    /// </summary>
    /// 


    private void PopulateGroup()
    {
        objGroup = new iAssetTrack.BAL.GroupBAL();
        objGroup.GroupID = 0;
        DataSet dsGroup = objGroup.retrieveGroupByUserType();
        DataTable dtGroup = dsGroup.Tables[0];

        //tenant code
        if (bool.Parse(Session["TenantUser"].ToString()))
        {
            UserBAL objUser = new UserBAL();
            objUser.UserID = Convert.ToInt32(Session["UserID"]);
            DataSet dsTenant = objUser.retrieveTenantDetails();
            if (dsTenant.Tables.Count > 0 && dsTenant.Tables[0].Rows.Count > 0)
            {
                tenantAssignedGroups = dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_ASSIGNEDGROUPS].ToString();
            }

            DataTable tempTable = dtGroup.Clone();
            foreach (DataRow dr in dtGroup.Rows)
            {
                tempTable.Rows.Add(dr.ItemArray);
            }
            DataRow[] filteredRows = tempTable.Select("GroupID IN (" + tenantAssignedGroups + ")");
            dtGroup.Rows.Clear();
            if (filteredRows != null && filteredRows.Length > 0)
            {
                foreach (DataRow dr in filteredRows)
                {
                    dtGroup.Rows.Add(dr.ItemArray);
                }

            }

        }
        else
        {
            //non tenant user - generally service provider admin.
            //SP admin can't see tenant groups.
            ArrayList alGroupIds = new ArrayList();
            TenantBAL objTenant = new TenantBAL();
            DataSet dsTGList = objTenant.retrieveGroupAssignmentList();
            if (dsTGList.Tables.Count > 0 && dsTGList.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsTGList.Tables[0].Rows)
                {
                    if (!alGroupIds.Contains(dr[DBFields.DBFIELD_GROUPID].ToString()))
                        alGroupIds.Add(dr[DBFields.DBFIELD_GROUPID].ToString());
                }

                DataTable tempTable = dtGroup.Clone();
                foreach (DataRow dr in dtGroup.Rows)
                {
                    tempTable.Rows.Add(dr.ItemArray);
                }
                DataRow[] filteredRows = tempTable.Select("GroupID NOT IN (" + string.Join(",",alGroupIds.ToArray()) + ")");
                dtGroup.Rows.Clear();
                if (filteredRows != null && filteredRows.Length > 0)
                {
                    foreach (DataRow dr in filteredRows)
                    {
                        dtGroup.Rows.Add(dr.ItemArray);
                    }

                }

            }
        }


        cblGroup.DataTextField = dtGroup.Columns[2].ColumnName;
        cblGroup.DataValueField = dtGroup.Columns[0].ColumnName;
        cblGroup.DataSource = dsGroup.Tables[0].DefaultView;
        cblGroup.DataBind();

    }

    #endregion

    /// <summary>
    /// Populate values for cost center and group.
    /// </summary>
    private void PolpulateValues()
    {
        if (Session["@@UserID"] != null)
        {
            this.txtLogin.Text = Session["@@LoginName"].ToString();
            this.txtFirstName.Text = Session["@@Name"].ToString();
        }
        objUser = new UserBAL();
        objUser.UserID = Convert.ToInt32(Session["@@UserID"].ToString());
        DataSet dsAssignGroup = objUser.retrieveAssignGroup();

        DataTable dtAssignGroup = dsAssignGroup.Tables[0];

        for (int i = 0; i < cblGroup.Items.Count; i++)
        {
            cblGroup.Items[i].Selected = false;
        }
        foreach (DataRow drAssignGroup in dtAssignGroup.Rows)
        {
            for (int i = 0; i < cblGroup.Items.Count; i++)
            {
                if (Convert.ToInt32(cblGroup.Items[i].Value) == (int)drAssignGroup["GroupID"])
                {
                    cblGroup.Items[i].Selected = true;
                }
            }
        }

    }


    /// <summary>
    /// assign rights
    /// 
    /// </summary>

    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        try
        {
            //tenant code
            if (bool.Parse(Session["TenantUser"].ToString()))
            {
                int noOfUsers = 0;
                int assignedUsers = 0;
                bool isAlreadyAssignedUser = false;

                UserBAL objUser = new UserBAL();
                objUser.UserID = Convert.ToInt32(Session["UserID"]);
                DataSet dsTenant = objUser.retrieveTenantDetails();
                if (dsTenant.Tables.Count > 0 && dsTenant.Tables[0].Rows.Count > 0)
                {
                    noOfUsers = string.IsNullOrEmpty(dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_USER_COUNT].ToString()) ? 0 : int.Parse(dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_USER_COUNT].ToString());


                    if (noOfUsers > 0)
                    {
                        TenantBAL objTenant = new TenantBAL();
                        objTenant.TenantId = Convert.ToInt32(dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_ID].ToString());
                        DataSet dsAssignedUsers = objTenant.retrieveAssignedUsers();
                        if (dsAssignedUsers.Tables.Count > 0 && dsAssignedUsers.Tables[0].Rows.Count > 0)
                        {
                            assignedUsers = dsAssignedUsers.Tables[0].Rows.Count;

                            foreach (DataRow dr in dsAssignedUsers.Tables[0].Rows)
                            {
                                if (Convert.ToInt32(Session["@@UserID"].ToString()) == Convert.ToInt32(dr[DBFields.DBFIELD_USERID].ToString()))
                                {
                                    isAlreadyAssignedUser = true;
                                    break;
                                }

                            }
                            DataRow[] rows = dsAssignedUsers.Tables[0].Select("UserId = " + Session["@@UserID"].ToString());

                            if (assignedUsers == noOfUsers && !isAlreadyAssignedUser)
                            {
                                lblMessage.Text = "Number of users allocated for this tenant is exceeded";
                                lblMessage.Visible = true;
                                return;
                            }
                            else if (assignedUsers > noOfUsers)
                            {
                                lblMessage.Text = "Number of users allocated for this tenant is exceeded";
                                lblMessage.Visible = true;
                                return;
                            }
                        }

                    }
                    else
                    {
                        lblMessage.Text = "No users allocated for this tenant";
                        lblMessage.Visible = true;
                        return;

                    }
                }

            }
            else
            {
                UserBAL objUser = new UserBAL();
                objUser.UserID = Convert.ToInt32(Session["@@UserID"]);
                DataSet dsTenant = objUser.retrieveTenantDetails();
                if (dsTenant.Tables.Count > 0 && dsTenant.Tables[0].Rows.Count > 0)
                {
                    // is a tenant user
                    lblMessage.Text = "System admin can't add Tenant user to a group";
                    lblMessage.Visible = true;
                    return;
                }
            }
           

            if (Session["@@UserID"] != null)
            {
                int iSelected = 0;


                for (int iCol = 0; iCol < cblGroup.Items.Count; iCol++)
                {
                    if (cblGroup.Items[iCol].Selected == true)
                        iSelected += 1;
                }
                if (iSelected == 0)
                {
                    objCommon = new CommonBAL();
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = objCommon.displayMessage(MessageCodes.ASSIGN_RIGHTS_MEMBER);

                    lblMessage.Visible = true;
                    return;
                }

                /*
                if (cblCostCenter.Visible == true)
                {
                    iSelected = 0;

                    for (int iCol = 0; iCol < cblCostCenter.Items.Count; iCol++)
                    {
                        if (cblCostCenter.Items[iCol].Selected == true)
                            iSelected += 1;
                    }
                    if (iSelected == 0)
                    {
                        objCommon = new CommonBAL();
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Text = objCommon.displayMessage(MessageCodes.ASSIGN_RIGHTS_COST_CENTER);

                        lblMessage.Visible = true;
                        return;
                    }
                }
                */

                objAssignRights = new AssignRightsBAL();

                objAssignRights.UserID = Convert.ToInt32(Session["@@UserID"].ToString());

                string GroupDelimiter = ";";

                string strSGrps = "";

                for (int iCol = 0; iCol < cblGroup.Items.Count; iCol++)
                {
                    if (cblGroup.Items[iCol].Selected == true)
                    {
                        strSGrps += cblGroup.Items[iCol].Value.ToString() + GroupDelimiter;
                    }
                }

                objAssignRights.GroupIDs = strSGrps.ToString();

                objAssignRights.Delimiters = ";";

                objAssignRights.CreatedBy = Convert.ToInt32(Session["UserID"]);
                objAssignRights.Persist(DALCOperation.Insert);
                //lblMessage.ForeColor = System.Drawing.Color.Black;
                //this.lblMessage.Text = "User rights has been assigned successfully";
                objCommon = new CommonBAL();
                lblMessage.Text = objCommon.displayMessage(MessageCodes.ASSIGN_RIGHTS);
                //Response.Write("<script language=javascript>alert('" + lblMessage.Text + "');</script>");
                //Response.Redirect("UserSearch.aspx");
                this.ShowMessage(lblMessage.Text, "nourl");
            }
            Session["@@UserID"] = null;
            //Session["@@LoginName"] = null;
            Session["@@Name"] = null;

            App_Code.CSqlSiteMapProvider.RefreshSiteMap();
            //HttpRuntime.Cache.Remove(App_Code.CSqlSiteMapProvider._cacheDependencyName);

        }
        catch (System.Data.SqlClient.SqlException sds)
        {
            lblMessage.Text = "SQL Exception";
            ExceptionPolicy.HandleException(sds, "errDCTrack");
        }
        catch (Exception ex)
        {
            ExceptionPolicy.HandleException(ex, "errDCTrack");
            throw ex;
        }
    }

    /// <summary>
    /// To register alert message for user actions.
    /// </summary>
    /// <param name="mess"></param>
    /// <param name="URL"></param>
    private void ShowMessage(string mess, string URL)
    {
        string strScript = "<script type=\"text/javascript\">validNavigation=true;alert(\"" + mess + "\");window.location=\"UserSearch.aspx\";</script>";
        if (!Page.ClientScript.IsStartupScriptRegistered("FORMMESSAGE"))
            Page.ClientScript.RegisterStartupScript(typeof(Page), "FORMMESSAGE", strScript);
    }

    /// <summary>
    /// close and redirect to usersearch.aspx
    /// </summary>

    /// 
    protected void ibCancel_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        Session["@@UserID"] = null;
        //Session["@@LoginName"] = null;
        Session["@@Name"] = null;
        Response.Redirect("UserSearch.aspx");
    }

}
