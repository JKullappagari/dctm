using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;
//using Microsoft.Practices.EnterpriseLibrary.Data;
//using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using iAssetTrack.DALC;
using iAssetTrack.BAL;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    [Serializable]
    public class SessionManagerBAL : MarshalByRefObject
    {
        #region Group

        private int _userId;
        private string _userName = string.Empty;
        private string _fullName = string.Empty;
        //private DataTable _dtRoles;
        private DataTable _dtRights;

        ////Rajesh
        //private DataTable _dtUserPageControlRights;
        //public DataTable UserPageControlRights { get { return _dtUserPageControlRights; } }
        ////Rajesh

        public int UserId { get { return _userId; } }
        public  string UserName { get { return _userName; } }
        public  string FullName { get { return _fullName; } }
        public DataTable Rights { get { return _dtRights; } }
        



        /// <summary>
        /// Initialize Session Manager Object
        /// </summary>
        public SessionManagerBAL()
        {
            
        }

        /// <summary>
        /// Initialize Session Manager Object with role
        /// </summary>
        /// <param name="Role"></param>
        public SessionManagerBAL(int UserId)
        {
            _userId = UserId;
            this.getSecurityDetails();
        }

        /// <summary>
        /// Get Security Details and assign DataRights table.
        /// </summary>
        private void getSecurityDetails()
        {
            DataSet dsUser = GetGroupAndRight();
            _dtRights = dsUser.Tables[0];
        }

        /// <summary>
        /// To check the user rights.
        /// </summary>
        /// <param name="right"></param>
        /// <returns>true/false</returns>
        public  bool HasRight(string right)
        {
            string filter = "scrty_right_t = '" + right.Replace("'", "''") + "'";
            DataRow[] drRights = _dtRights.Select(filter);
            if (drRights.Length > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Get Group and Rights.
        /// </summary>
        /// <param>Group Name</param>
        /// <returns>DataSet</returns>
        private DataSet GetGroupAndRight()
        {
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.

            try
            {
                DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_GROUPMODULERIGHTS_LIST, DALCResultType.DataSet);
                criteria.AddInParameter(Parameters.PARAM_USERID, DbType.Int32, _userId);
                DataSet ds = (DataSet)criteria.ExecuteCommand();
                //Dictionary<string, object> output = criteria.OutputParameters;
                return (ds);

//                return DatabaseFactory.CreateDatabase().ExecuteDataSet(StoredProcedures.SP_GROUPMODULERIGHTS_LIST,_userId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        //#region User

        //private void GetUserPageControlRights()
        //{
        //    try
        //    {
        //        DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_USER_PAGECONTROLRIGHTS, DALCResultType.DataSet);
        //        criteria.AddInParameter(Parameters.PARAM_USERID, DbType.Int32, _userId);
        //        DataSet ds = (DataSet)criteria.ExecuteCommand();
        //        _dtUserPageControlRights = ds.Tables[0];
                
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }            
        //}

        //#endregion
    }
}
