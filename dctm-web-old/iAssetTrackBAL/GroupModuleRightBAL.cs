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
    [DALCOperationSP(InsertSP = StoredProcedures.SP_GROUPMODULERIGHTSASSIGNMENT_UPDATE)]
    public class GroupModuleRightBAL
    {

        #region GroupModuleRight
        //* BAL Name:  GroupModuleRightBAL
        //* Author :  Murugan K
        //* Date	  :  10:27 AM 10/12/2006
        //* Use : To insert,update and delete group module rights.
        //* Caller: GroupModuleRights.aspx
        #endregion

        #region Declarations
        private int intGroupModuleRightsID;
            private int intGroupID;
        // (Redundant) private int intModuleRightsID;
            private int intRightsID;
            private int intModuleID;
            private int intStatus;
            private string strDelimiters;
            private int intCreatedBy;
            private int intLastModifiedBy;
            private string strRightsIDs;
            private string strModuleIDs;
        #endregion

        #region Properties
            [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_GROUPID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
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

            [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MODULEID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
            public int ModuleID
            {
                get
                {
                    return this.intModuleID;
                }
                set
                {
                    this.intModuleID = value;
                }
            }

            public int RightsID
            {
                get
                {
                    return this.intRightsID;
                }
                set
                {
                    this.intRightsID = value;
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

            [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ACCESSRIGHTSIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
            public string RightsIDs
            {
                get
                {
                    return this.strRightsIDs;
                }
                set
                {
                    this.strRightsIDs = value;
                }
            }

            public string ModuleIDs
            {
                get
                {
                    return this.strModuleIDs;
                }
                set
                {
                    this.strModuleIDs = value;
                }
            }

            [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_STATUS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
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

          
            public int GroupModuleRightsID
            {
                get
                {
                    return this.intGroupModuleRightsID;
                }
                set
                {
                    this.intGroupModuleRightsID = value;
                }
            }
        #endregion

        #region Methods

        /// <summary>
        /// To insert,update and delete group module rights.
        /// </summary>
        /// <param name="operation"></param>
        public void Persist(DALCOperation operation)
        {
            DALCBase<GroupModuleRightBAL> dalc = new DALCBase<GroupModuleRightBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// To retrieve group module rights by group module rights id.
        /// </summary>
        /// <params>GroupModuleRightsID</params>
        /// <returns>DataSet</returns>
        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_GROUPMODULERIGHTSASSIGNMENT_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_GROUPMODULEACCESSRIGHTSID, DbType.Int32, GroupModuleRightsID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// To retrieve available module rights by module id.
        /// </summary>
        /// <params>ModuleID</params>
        /// <returns>DataSet</returns>
        public DataSet retrieveAvailModuleRights()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_AVAILACCESSRIGHTS_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_MODULEID, DbType.Int32, ModuleID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// To retrieve assigned module rights by module id.
        /// </summary>
        /// <params>ModuleID</params>
        /// <returns>DataSet</returns>
        public DataSet retrieveAssignModuleRights()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSIGNACCESSRIGHTS_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_MODULEID, DbType.Int32, ModuleID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        public DataSet retrieveRights()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ACCESSRIGHTS_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_ACCESSRIGHTSID, DbType.Int32, RightsID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        #endregion


    }

 }



