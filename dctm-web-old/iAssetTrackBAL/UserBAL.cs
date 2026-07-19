/*
File Name   :	UserBAL.cs

Description :	user Business logic

Date created:	27 March 2006

Modification History:
***********************
CR		Name			Date			Description
New		murugan	27/03/2006		File has been created.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;


namespace iAssetTrack.BAL
{
    [DALCOperationSP(InsertSP = StoredProcedures.SP_USER_UPDATE, DeleteSP = StoredProcedures.SP_USER_DELETE)]
    public class UserBAL
    {
        private string strLoginName;
        private string strPassword;
        private string strFirstName;
        private string strLastName;
        private string strName;
        private string strEmail;
        private int intDepartmentID;
        //private string strCostCenterIDs;
        private int intStatus;
        private int intCreatedBy;
        private int intLastModifiedBy;
        private string strGroupIDs;
        private string strUserIDs;
        private string strDelimiters;
        private int intUserID;
        private int intBUID;

        private string strDisplayName;

        public string DisplayName
        {
            get 
            {
                if (LastName == "" && FirstName == "")
                {
                    strDisplayName = FirstName + (LastName == "" || FirstName == "" ? "": ", ") + LastName;
                }
                else
                {
                    strDisplayName = LoginName;
                }
                return strDisplayName; 
            }
        }



        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_LOGINNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string LoginName
        {
            get
            {
                return this.strLoginName;
            }
            set
            {
                this.strLoginName = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_PASSWORD, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string Password
        {
            get
            {
                return this.strPassword;
            }
            set
            {
                this.strPassword = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_FIRSTNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string FirstName
        {
            get
            {
                return this.strFirstName;
            }
            set
            {
                this.strFirstName = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_LASTNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string LastName
        {
            get
            {
                return this.strLastName;
            }
            set
            {
                this.strLastName = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_EMAIL, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string Email
        {
            get
            {
                return this.strEmail;
            }
            set
            {
                this.strEmail = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_DEPARTMENTID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int DepartmentId
        {
            get
            {
                return this.intDepartmentID;
            }
            set
            {
                this.intDepartmentID = value;
            }
        }
 
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_DELIMITERS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string Delimiters
        {
            get
            {
                return this.strDelimiters;
            }
            set
            {
                this.strDelimiters = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_GROUPIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string GroupIDs
        {
            get
            {
                return this.strGroupIDs;
            }
            set
            {
                this.strGroupIDs = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_USERIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public string UserIDs
        {
            get
            {
                return this.strUserIDs;
            }
            set
            {
                this.strUserIDs = value;
            }
        }
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_STATUS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_STATUS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public int Status
        {
            get
            {
                return this.intStatus;
            }
            set
            {
                this.intStatus = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_CREATEDBY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int CreatedBy
        {
            get
            {
                return this.intCreatedBy;
            }
            set
            {
                this.intCreatedBy = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MODIFIEDBY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public int LastModifiedBy
        {
            get
            {
                return this.intLastModifiedBy;
            }
            set
            {
                this.intLastModifiedBy = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_USERID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
        public int UserID
        {
            get
            {
                return this.intUserID;
            }
            set
            {
                this.intUserID = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_BUSINESSUNIT, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
        public int BusinessUnitID
        {
            get
            {
                return this.intBUID;
            }
            set
            {
                this.intBUID = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = "@pVchName", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string Name
        {
            get
            {
                return this.strName;
            }
            set
            {
                this.strName = value;
            }
        }
 
        public void Persist(DALCOperation operation)
        {
            DALCBase<UserBAL> dalc = new DALCBase<UserBAL>(this);
            dalc.SaveData(operation, 1);
        }

             /// <summary>
        /// Retrive user details
        /// </summary>
        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_USER_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_USERID, DbType.Int32, UserID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
             /// <summary>
        /// Retrive hassign group
        /// </summary>
        public DataSet retrieveAssignGroup()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSIGNGROUP_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_USERID, DbType.Int32, UserID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
             /// <summary>
        /// Retrive assign cost center
        /// </summary>
        // (Redundant) public DataSet retrieveAssignCostCenter()
        //{
        //    DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSIGNCOSTCENTER_LIST, DALCResultType.DataSet);
        //    criteria.AddInParameter(Parameters.PARAM_USERID, DbType.Int32, UserID);
        //    criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, 0);
        //    DataSet ds = (DataSet)criteria.ExecuteCommand();
        //    Dictionary<string, object> output = criteria.OutputParameters;
        //    return (ds);
        //}

             /// <summary>
        /// Retrive user by password
        /// </summary>
        public DataSet  retrieveUserByPassword()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_USER_LOGIN, DALCResultType.DataSet );
            criteria.AddInParameter(Parameters.PARAM_LOGINNAME, DbType.String, LoginName);
            criteria.AddInParameter(Parameters.PARAM_PASSWORD, DbType.String, Password);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
             /// <summary>
        /// Retrive user by user name
        /// </summary>
        public DataSet retrieveUserByUserName()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_USER_LOGINBYUSERNAME, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_LOGINNAME, DbType.String, LoginName);            
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
        /// <summary>
        /// Retrive Group user by user name
        /// </summary>
        public DataSet retrieveGroupByUserName()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_USER_GROUPBYUSERNAME, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_LOGINNAME, DbType.String, LoginName);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
        /// <summary>
        /// Retrive Group user by user name
        /// </summary>
        public DataSet retrieveUserBU()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_USERBU_DoesExist, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_LOGINNAME, DbType.String, LoginName);
            criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.String, BusinessUnitID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

             /// <summary>
        /// ceeck user exist or not
        /// </summary>
        public int exists()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_USER_DOESEXIST, DALCResultType.Scalar);

            criteria.AddInParameter(Parameters.PARAM_USERID, DbType.Int32, UserID);
            criteria.AddInParameter(Parameters.PARAM_LOGINNAME, DbType.String, LoginName);

            //criteria.AddOutParameter("@count", DbType.Int32,0,5);

            int count = (int)criteria.ExecuteCommand();

            Dictionary<string, object> output = criteria.OutputParameters;
            //return() - Return No. of Records
            return (count);

        }

        

        public DataSet GetUserByBU()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_USER_LIST_BY_BUSINESSUNIT, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, BusinessUnitID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        public DataSet Search()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_USER_SEARCH, DALCResultType.DataSet);
            //change int16 to int32
            criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, BusinessUnitID);
            // (Redundant) criteria.AddInParameter(Parameters.PARAM_DEPARTMENTID, DbType.Int32, DepartmentId);
            criteria.AddInParameter(Parameters.PARAM_GROUPIDs, DbType.String, GroupIDs);
            criteria.AddInParameter("@pVchName", DbType.String, strName);
            criteria.AddInParameter(Parameters.PARAM_ASSETID, DbType.Int32, 0);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            return ds;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet retrieveBUListByUserId()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_BUSINESSUNIT_LIST_BY_USERID, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_USERID, DbType.Int32, UserID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }


        public DataSet SearchWithAssetAuthorization(int pIntAssetID)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_USER_SEARCH, DALCResultType.DataSet);
            //change int16 to int32
            criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, BusinessUnitID);
            // (Redundant) criteria.AddInParameter(Parameters.PARAM_DEPARTMENTID, DbType.Int32, DepartmentId);
            criteria.AddInParameter(Parameters.PARAM_GROUPIDs, DbType.String, GroupIDs);
            criteria.AddInParameter("@pVchName", DbType.String, strName);
            criteria.AddInParameter(Parameters.PARAM_ASSETID, DbType.Int32, pIntAssetID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            return ds;
        }

        public DataSet GetUserHistoryDetails(int intUserID)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_USER_HISTORY, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_USERID, DbType.Int32, UserID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            return ds;
        }


        public int IsATenantUser(int UserId)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_USERS_IS_A_TENANT_USER, DALCResultType.Scalar);

            criteria.AddInParameter(Parameters.PARAM_USERID, DbType.Int32, UserId);

            int count = (int)criteria.ExecuteCommand();

            Dictionary<string, object> output = criteria.OutputParameters;
            return (count);

        }

        public DataSet retrieveTenantDetails()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_TENANT_LIST_BY_USERID, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_USERID, DbType.Int32, UserID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
    }
}