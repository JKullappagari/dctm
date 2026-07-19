/*
File Name   :	AssetModelBAL.cs

Description :	Business Logic layer for Application Map setup

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
    [DALCOperationSP(InsertSP = StoredProcedures.SP_APPLMAP_UPDATE, DeleteSP = StoredProcedures.SP_APPLMAP_DELETE)]
    public class ApplicationMapBAL
    {
        private int intApplMapID;
        private int intApplID;
        private string guidID;
        private int intCreatedBy;
        private int intLastModifiedBy;
        private string strDelimiters;
        private string strApplIds;

        //[DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_APPLMAPID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int ApplMapID
        {
            get
            {
                return this.intApplMapID;
            }
            set
            {
                this.intApplMapID = value;
            }
        }

        //[DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_APPLMAPAPPLID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public int ApplID
        {
            get
            {
                return this.intApplID;
            }
            set
            {
                this.intApplID = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_APPLMAP_ID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_APPLMAP_ID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public string ID
        {
            get
            {
                return this.guidID;
            }
            set
            {
                this.guidID = value;
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

        //[DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MODIFIEDBY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
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

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_APPLMAP_APPLIDS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_APPLMAP_APPLIDS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public string ApplIDs
        {
            get
            {
                return this.strApplIds;
            }
            set
            {
                this.strApplIds = value;
            }
        }

        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            DALCBase<ApplicationMapBAL> dalc = new DALCBase<ApplicationMapBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_APPLMAP_LIST, DALCResultType.DataSet);
            //TODO: Check if the parameter PARAM_APPLMAPID is required.
            //criteria.AddInParameter(Parameters.PARAM_APPLMAPID, DbType.Int32, ApplMapID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieveByAssetID(int ID)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_APPLMAP_LIST_BY_HOSTID, DALCResultType.DataSet);
            //TODO: Check if the parameter PARAM_APPLMAPID is required.
            criteria.AddInParameter(Parameters.PARAM_APPLMAP_ID, DbType.Int32, ID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
    }
}
