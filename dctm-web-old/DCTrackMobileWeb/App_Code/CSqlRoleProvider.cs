using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


namespace App_Code
{
    /// <summary>
    /// Summary description for CSqlRoleProvider
    /// </summary>
    public class CSqlRoleProvider : RoleProvider
    {
        public CSqlRoleProvider()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string ApplicationName
        {
            get
            {
                return "DCTrackMobileWeb";
                //throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                //throw new Exception("The method or operation is not implemented.");
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string[] GetAllRoles()
        {

            iAssetTrack.BAL.GroupBAL groupBal = new iAssetTrack.BAL.GroupBAL();
            DataSet ds = groupBal.retrieve();

            String allGroups = "";

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    allGroups += ds.Tables[0].Rows[i]["Group"].ToString() + ",";
                }
            }

            return allGroups.Split(',');
            //throw new Exception("The method or operation is not implemented.");
        }

        public override string[] GetRolesForUser(string username)
        {

            iAssetTrack.BAL.UserBAL userBal = new iAssetTrack.BAL.UserBAL();
            userBal.UserID = Int32.Parse(HttpContext.Current.Session["UserId"].ToString());

            DataSet ds = userBal.retrieveAssignGroup();
            String userGroups = "";

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    userGroups += ds.Tables[0].Rows[i]["Group"].ToString() + ",";
                }
            }

            return userGroups.Split(',');
            //throw new Exception("The method or operation is not implemented.");
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool IsUserInRole(string username, string roleName)
        {

            iAssetTrack.BAL.UserBAL userBal = new iAssetTrack.BAL.UserBAL();


            userBal.UserID = Int32.Parse(HttpContext.Current.Session["UserId"].ToString());
            DataSet ds = userBal.retrieveAssignGroup();
            bool isUserInGroup = false;

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["Group"].ToString().Equals(roleName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        isUserInGroup = true;
                        break;
                    }
                }
            }

            return isUserInGroup;

            //throw new Exception("The method or operation is not implemented.");
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool RoleExists(string roleName)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
