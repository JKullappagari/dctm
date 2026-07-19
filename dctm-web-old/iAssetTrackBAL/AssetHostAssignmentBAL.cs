using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    [DALCOperationSP(InsertSP = StoredProcedures.SP_AHA_UPDATE, DeleteSP = StoredProcedures.SP_AHA_DELETE)]
    public class AssetHostAssignmentBAL
    {
        private Guid guidID;
        private int intAssetID;
        private int intHostID;
        private int intCreatedBy;
        private int intLastModifiedBy;
        private string strDelimiters;

        //[DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_APPLMAPID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public Guid ID
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

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_AHA_ASSET_ID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int AssetID
        {
            get
            {
                return this.intAssetID;
            }
            set
            {
                this.intAssetID = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_AHA_HOST_ID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int HostID
        {
            get
            {
                return this.intHostID;
            }
            set
            {
                this.intHostID = value;
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

        //[DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_APPLMAP_APPLIDS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        //[DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_APPLMAP_APPLIDS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        //public string ApplIDs
        //{
        //    get
        //    {
        //        return this.strApplIds;
        //    }
        //    set
        //    {
        //        this.strApplIds = value;
        //    }
        //}

        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            DALCBase<AssetHostAssignmentBAL> dalc = new DALCBase<AssetHostAssignmentBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_AHA_LIST, DALCResultType.DataSet);
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
        public DataSet retrieveByAssetandHostID(int AssetID,int HostID)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_AHA_LIST_BY_ASSET_HOST_IDS, DALCResultType.DataSet);
            //TODO: Check if the parameter PARAM_APPLMAPID is required.
            criteria.AddInParameter(Parameters.PARAM_AHA_ASSET_ID, DbType.Int32, AssetID);
            criteria.AddInParameter(Parameters.PARAM_AHA_HOST_ID, DbType.Int32, HostID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
    }
}
