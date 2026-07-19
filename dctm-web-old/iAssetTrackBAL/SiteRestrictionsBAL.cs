/*
File Name   :	SiteRestrictionsBAL.cs

Description :	Site Restrictions Business logic

Date created:	03 Nov 2013

Modification History:
***********************
CR		Name			Date			Description
New		Jagadeesh       03 Nov 2013     Created.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;


namespace iAssetTrack.BAL
{
    [DALCOperationSP(InsertSP = StoredProcedures.SP_SITE_RESTRICTIONS_UPDATE, DeleteSP = StoredProcedures.SP_SITE_RESTRICTIONS_DELETE)]
    public class SiteRestrictionsBAL
    {
        private bool blnRead;
        private bool blnWrite;
        private int intUserID;
        private int intSiteID;
        private string strSiteName;
        private int intCreatedBy;
        private int intLastModifiedBy;

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_READ_PERMISSION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public bool ReadPermission
        {
            get
            {
                return this.blnRead;
            }
            set
            {
                this.blnRead = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_WRITE_PERMISSION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public bool WritePermission
        {
            get
            {
                return this.blnWrite;
            }
            set
            {
                this.blnWrite = value;
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

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_USERID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
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

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_SITEID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
        public int SiteID
        {
            get
            {
                return this.intSiteID;
            }
            set
            {
                this.intSiteID = value;
            }
        }
        
        public string SiteName
        {
            get
            {
                return this.strSiteName;
            }
            set
            {
                this.strSiteName = value;
            }
        }
 
        public void Persist(DALCOperation operation)
        {
            DALCBase<SiteRestrictionsBAL> dalc = new DALCBase<SiteRestrictionsBAL>(this);
            dalc.SaveData(operation, 1);
        }

       /// <summary>
       /// Retrives restriction list by user id
       /// </summary>
       /// <returns>User data set</returns>
       /// <Version>v3.8</Version>
        public DataSet RetrieveByUserID()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_SITE_RESTRICTIONS_LIST_BY_USERID, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_USERID, DbType.Int32, UserID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
      
        
       
    }
}