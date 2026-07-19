/*
File Name   :	GantryOfflineAttendanceBAL.cs

Description :	GantryOfflineAttendance BAL

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
    [DALCOperationSP(InsertSP = StoredProcedures.SP_GROUP_UPDATE, DeleteSP = StoredProcedures.SP_GROUP_DELETE)]
    public class GroupBAL
    {
        #region Group
        //* BAL Name:  GroupBAL
        //* Author :  Murugan K
        //* Date	  :  10:27 AM 10/12/2006
        //* Use : To insert,update and delete groups.
        //* Caller: Group.aspx
        #endregion

        #region Declarations
        private int intGroupID;
        private string strGroup;
        private string strDescription;
        private int intStatus;
        private int intBusinessUnitID;
        //private int intIsInternal;
        //private int intIsExternal;
        private int intCreatedBy;
        private int intLastModifiedBy;
        private string strGroupIDs;
        #endregion

        #region Properties
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_GROUP, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string Group
        {
            get
            {
                return this.strGroup;
            }
            set
            {
                this.strGroup = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_DESCRIPTION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string Description
        {
            get
            {
                return this.strDescription;
            }
            set
            {
                this.strDescription = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_GROUPIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
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

        //[DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_SITEID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        //public int SiteID
        //{
        //    get { return intSiteID; }
        //    set { intSiteID = value; }
        //}

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

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_GROUPID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
        public int GroupID
        {
            get
            {
                return this.intGroupID;
            }
            set
            {
                this.intGroupID = value;
            }
        }
        

        public int BusinessUnitID
        {
            get { return intBusinessUnitID; }
            set { intBusinessUnitID = value; }
        }

           #endregion

        #region Methods
        /// <summary>
        /// To insert,update and delete groups.
        /// </summary>
        /// <param name="operation"></param>
        public void Persist(DALCOperation operation)
        {
            DALCBase<GroupBAL> dalc = new DALCBase<GroupBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// To retrieve group list by group id.
        /// </summary>
        /// <params>GroupID</params>
        /// <returns>DataSet</returns>
        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_GROUP_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_GROUPID, DbType.Int32, GroupID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }


        /// <summary>
        /// To retrieve group list by department/site id.
        /// </summary>
        /// <params>GroupID</params>
        /// <returns>DataSet</returns>
        public DataSet retrieveUserByBUID()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_GROUP_LISTBYBU, DALCResultType.DataSet);
            //criteria.AddInParameter(Parameters.PARAM_SITEID, DbType.Int32, SiteID);
            criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, BusinessUnitID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// To retrieve group list by group id and user type
        /// </summary>
        /// <params>GroupID</params>
        /// <params>Status</params>
        /// <returns>DataSet</returns>
        public DataSet retrieveGroupByUserType()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_GROUP_LISTBYUSERTYPE, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_GROUPID, DbType.Int32, GroupID);
            criteria.AddInParameter(Parameters.PARAM_USERTYPE, DbType.Int32, Status);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        public DataSet retrieveGroupByUser(int UserID)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_GROUP_LISTBYUSERTYPE, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_GROUPID, DbType.Int32, GroupID);
            criteria.AddInParameter(Parameters.PARAM_USERID, DbType.Int32, UserID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// To retrieve user list by group id
        /// </summary>
        /// <param name="pUserListSortBy">The Sort Order of the User List. Applicable Values are LoginName, LastName & FirstName</param>
        /// <returns>DataSet</returns>
        public DataSet retrieveUsers(String pUserListSortBy)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_GROUP_USERLIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_GROUPID, DbType.Int32, GroupID);
            criteria.AddInParameter(Parameters.PARAM_USERSORTBY, DbType.String, pUserListSortBy);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// To check the group already exists using group and group id.
        /// </summary>
        /// <params>GroupID</params>
        /// <params>Group</params>
        /// <returns>int</returns>
        public int exists()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_GROUP_DOESEXIST, DALCResultType.Scalar);

            criteria.AddInParameter(Parameters.PARAM_GROUPID, DbType.Int32, GroupID);
            criteria.AddInParameter(Parameters.PARAM_GROUP, DbType.String, Group);

            //criteria.AddOutParameter("@count", DbType.Int32,0,5);

            int count = (int)criteria.ExecuteCommand();

            Dictionary<string, object> output = criteria.OutputParameters;
            //return() - Return No. of Records
            return (count);

        }

             

        #endregion

    }
}