using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;


namespace iAssetTrack.BAL
{
    [DALCOperationSP(InsertSP = StoredProcedures.SP_TENANT_ASSET_UPDATE, UpdateSP = StoredProcedures.SP_TENANT_ASSET_UPDATE)]
    public class TenantAssetBAL
    {
        private int intTenantId;
        private int intAssetId;
        private int intCreatedBy;
        private int intLastModifiedBy;
        private int intStatus;
        private string strAssetIds;

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_TENANTID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
        public int TenantId
        {
            get
            {
                return this.intTenantId;
            }
            set
            {
                this.intTenantId = value;
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

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_TENANTIDS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public string AssetIds
        {
            get
            {
                return this.strAssetIds;
            }
            set
            {
                this.strAssetIds = value;
            }
        }



        public void Persist(DALCOperation operation)
        {
            DALCBase<TenantAssetBAL> dalc = new DALCBase<TenantAssetBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrive tenant details
        /// </summary>
        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_TENANT_ASSET_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_TENANTID, DbType.Int32, TenantId);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }


    }
}