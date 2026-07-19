/*
File Name   :	AssetModelBAL.cs

Description :	Business Logic layer for Application Type setup

Date created:	27 July 2011

Modification History:
***********************
CR		Name		    Date			Description
New		Nayana M    	23 June 2011	File has been created.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    [DALCOperationSP(InsertSP = StoredProcedures.SP_APPLTYPE_UPDATE, DeleteSP = StoredProcedures.SP_APPLTYPE_DELETE)]
    public class ApplicationTypeBAL
    {
        private int intApplTypeID;
        private string strApplType;
        private string strDescription;
        private int intCreatedBy;
        private int intLastModifiedBy;
        private string strApplTypeIDs;
        private int intStatus;

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_APPLTYPEID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
        public int ApplTypelID
        {
            get
            {
                return this.intApplTypeID;
            }
            set
            {
                this.intApplTypeID = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_APPLTYPE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string ApplType
        {
            get
            {
                return this.strApplType;
            }
            set
            {
                this.strApplType = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_APPLTYPEDESCRIPTION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
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
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_APPLTYPEIDS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public string ApplTypeIDs
        {
            get
            {
                return this.strApplTypeIDs;
            }
            set
            {
                this.strApplTypeIDs = value;
            }
        }

        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            DALCBase<ApplicationTypeBAL> dalc = new DALCBase<ApplicationTypeBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_APPLTYPE_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_APPLTYPEID, DbType.Int32, ApplTypelID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// To check whether the Application Type already exists
        /// </summary>
        /// <returns>integer value</returns>
        public int exists()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_APPLTYPE_DOESEXIST, DALCResultType.Scalar);

            criteria.AddInParameter(Parameters.PARAM_APPLTYPEID, DbType.Int32, ApplTypelID);
            criteria.AddInParameter(Parameters.PARAM_APPLTYPE, DbType.String, ApplType);

            //criteria.AddOutParameter("@count", DbType.Int32,0,5);

            int count = (int)criteria.ExecuteCommand();

            Dictionary<string, object> output = criteria.OutputParameters;
            //return() - Return No. of Records
            return (count);

        }
    }
}
