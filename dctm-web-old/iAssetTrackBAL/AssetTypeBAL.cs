/*
File Name   :	AssetTypeBAL.cs

Description :	Business Logic layer for Asset Type

Date created:	27 March 2006

Modification History:
***********************
CR		Name			Date			Description
New		Venkatesan M	27/03/2006		File has been created.
*/

using System.Collections.Generic;
using System.Data;
using iAssetTrack.DALC;

namespace iAssetTrackBAL
{
    [DALCOperationSP(InsertSP = StoredProcedures.SP_ASSETGROUP_UPDATE, DeleteSP = StoredProcedures.SP_ASSETGROUP_DELETE, UpdateSP = StoredProcedures.SP_ASSETGROUP_UPDATE)]
    public class AssetGroupBAL
    {
        private int _intAssetTypeID;
        private string _strAssetType;
        private string _strDescription;
        //private string strFilterValue;
        //private string strTagData;
        //private string strPrinterName;
        //private string strTemplateName;
        private int _intStatus;
        private int _intCreatedBy;
        private int _intLastModifiedBy;
        private string _strAssetTypeIDs;
       
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ASSETGROUP, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ASSETGROUP, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string AssetType
        {
            get
            {
                return this._strAssetType;
            }
            set
            {
                this._strAssetType = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_DESCRIPTION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_DESCRIPTION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
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

        // (Redundant)[DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ASSETFILTERVALUE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        //public string FilterValue
        //{
        //    get
        //    {
        //        return this.strFilterValue;
        //    }
        //    set
        //    {
        //        this.strFilterValue = value;
        //    }
        //}

        // (Redundant) [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ASSETTAGDATA, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        //public string TagData
        //{
        //    get
        //    {
        //        return this.strTagData;
        //    }
        //    set
        //    {
        //        this.strTagData = value;
        //    }
        //}

        // (Redundant)[DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ASSETPRINTERNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        //public string PrinterName
        //{
        //    get
        //    {
        //        return this.strPrinterName;
        //    }
        //    set
        //    {
        //        this.strPrinterName = value;
        //    }
        //}

        // (Redundant)[DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_TEMPLATENAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        //public string TemplateName
        //{
        //    get
        //    {
        //        return this.strTemplateName;
        //    }
        //    set
        //    {
        //        this.strTemplateName = value;
        //    }
        //}



        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ASSETGROUPIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public string AssetTypeIDs
        {
            get
            {
                return this._strAssetTypeIDs;
            }
            set
            {
                this._strAssetTypeIDs = value;
            }
        }
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_STATUS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_STATUS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
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
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_CREATEDBY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
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

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSETGROUPID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSETGROUPID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Update)]
        public int AssetTypeID
        {
            get
            {
                return this._intAssetTypeID;
            }
            set
            {
                this._intAssetTypeID = value;
            }
        }

        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            var dalc = new DALCBase<AssetGroupBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieve()
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_ASSETGROUP_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_ASSETGROUPID, DbType.Int32, AssetTypeID);
            var ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        public DataSet retrieveAllAssetGroup()
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_ASSETGROUP_ALL_LIST, DALCResultType.DataSet);
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
            var criteria = new DALCCommandHelper(StoredProcedures.SP_ASSETGROUP_DOESEXIST, DALCResultType.Scalar);
            
            criteria.AddInParameter(Parameters.PARAM_ASSETGROUPID, DbType.Int32, AssetTypeID);
            criteria.AddInParameter(Parameters.PARAM_ASSETGROUP, DbType.String, AssetType);
            
            //criteria.AddOutParameter("@count", DbType.Int32,0,5);

            var count = (int)criteria.ExecuteCommand();
            
            Dictionary<string , object> output = criteria.OutputParameters;
            //return() - Return No. of Records
            return (count); 

        }

    }
}
