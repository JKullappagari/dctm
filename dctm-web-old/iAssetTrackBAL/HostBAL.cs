/*
File Name   :	AssetTypeBAL.cs

Description :	Business Logic layer for Asset Type

Date created:	27 March 2006

Modification History:
***********************
CR		Name			 Date			Description
New		Jaagdeesh Baub K 27 Aug 2011		File has been created.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrackBAL
{
    [DALCOperationSP(InsertSP = StoredProcedures.SP_HOST_UPDATE, DeleteSP = StoredProcedures.SP_HOST_DELETE)]
    public class HostBAL
    {
        private string _strHostID;
        private string _strHostName;
        private string _strDescription;
        private int _intStatus;
        private int _intCreatedBy;
        private int _intLastModifiedBy;
        private string _strHostIDs;
       
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_HOSTNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string HostName
        {
            get
            {
                return this._strHostName;
            }
            set
            {
                this._strHostName = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_DESCRIPTION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string Description
        {
            get
            {
                return this._strDescription;
            }
            set
            {
                this._strDescription = value;
            }
        }


        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_HOSTIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public string HostIDs
        {
            get
            {
                return this._strHostIDs;
            }
            set
            {
                this._strHostIDs = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_STATUS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_STATUS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public int Status
        {
            get
            {
                return this._intStatus;
            }
            set
            {
                this._intStatus = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_CREATEDBY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int CreatedBy
        {
            get
            {
                return this._intCreatedBy;
            }
            set
            {
                this._intCreatedBy = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MODIFIEDBY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public int LastModifiedBy
        {
            get
            {
                return this._intLastModifiedBy;
            }
            set
            {
                this._intLastModifiedBy = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_HOSTID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
        public string HostID
        {
            get
            {
                return this._strHostID;
            }
            set
            {
                this._strHostID = value;
            }
        }

        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            var dalc = new DALCBase<HostBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieve()
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_HOST_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_HOSTID, DbType.String, HostID);
            var ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieveLocIDFromHostID(string HostName )
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_HOST_LOCATION_BY_HOST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_HOSTNAME, DbType.String, HostName);
            var ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// To check whether the Business Unit is already exists
        /// </summary>
        /// <returns>integer value</returns>
        public int exists()
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_HOST_DOESEXIST, DALCResultType.Scalar);
            
            criteria.AddInParameter(Parameters.PARAM_HOSTID, DbType.String, HostID);
            criteria.AddInParameter(Parameters.PARAM_HOSTNAME, DbType.String, HostName);
            
            //criteria.AddOutParameter("@count", DbType.Int32,0,5);

            var count = (int)criteria.ExecuteCommand();
            
            Dictionary<string , object> output = criteria.OutputParameters;
            //return() - Return No. of Records
            return (count); 

        }

    }
}
