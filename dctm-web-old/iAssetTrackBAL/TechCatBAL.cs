/*
File Name   :	TechCatBAL.cs

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
    [DALCOperationSP(InsertSP = StoredProcedures.SP_TECHCAT_UPDATE, DeleteSP = StoredProcedures.SP_TECHCAT_DELETE)]
    public class TechCatBAL
    {
        private int _intTechCatID;
        private string _strTechName;
        private string _strDescription;
        //private string strFilterValue;
        //private string strTagData;
        //private string strPrinterName;
        //private string strTemplateName;
        private int _intStatus;
        private int _intCreatedBy;
        private int _intLastModifiedBy;
        private string _strTechIDs;
       
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_TECHCAT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string TechName
        {
            get
            {
                return this._strTechName;
            }
            set
            {
                this._strTechName = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_TECHDESCRIPTION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
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



        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_TECHIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public string TechIDs
        {
            get
            {
                return this._strTechIDs;
            }
            set
            {
                this._strTechIDs = value;
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

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSETGROUPID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
        public int TechID
        {
            get
            {
                return this._intTechCatID;
            }
            set
            {
                this._intTechCatID = value;
            }
        }

        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            var dalc = new DALCBase<TechCatBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieve()
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_TECHCAT_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_TECHID, DbType.Int32, TechID);
            var ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
        public DataSet retrieveByAssetID(int AssetID)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.Sp_TechCat_Name_byAssetID, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_ASSETsID, DbType.Int32, AssetID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
        public DataSet retrieveAllTechCategory()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_TECHCAT_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_TECHID, DbType.Int32, TechID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
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
            
            criteria.AddInParameter(Parameters.PARAM_TECHID, DbType.Int32, TechID);
            criteria.AddInParameter(Parameters.PARAM_TECHNAME, DbType.String, TechName);
            
            //criteria.AddOutParameter("@count", DbType.Int32,0,5);

            var count = (int)criteria.ExecuteCommand();
            
            Dictionary<string , object> output = criteria.OutputParameters;
            //return() - Return No. of Records
            return (count); 

        }

    }
}
