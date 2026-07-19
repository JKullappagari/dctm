
/*
File Name   :	GlobalParametersBALL.cs

Description :	Business Logic layer for GlobalParameters

Date created:	09-Jan-2013

Modification History:
***********************
CR		Name			 Date			Description
New		   		        09-Jan-2013           File has been created.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrackBAL
{
    [DALCOperationSP(InsertSP = StoredProcedures.SP_GLPARAMS_UPDATE, DeleteSP = StoredProcedures.SP_GLPARAMS_DELETE)]
    public class GlobalParametersBAL
    {
        private string _strSNo;
        private string _strSPCVariable;
        private string _strSPCValue;
        private int _intMeasureID;
        private int _intUOMID;
        private int _intPerUOMID;
        private int _intStatus;
        private int _intCreatedBy;
        private int _intLastModifiedBy;
        private string _strSNos;
        private string _strComment;

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_SPCVariable, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string SPCVariable
        {
            get
            {
                return this._strSPCVariable;
            }
            set
            {
                this._strSPCVariable = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_SPCValue, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string SPCValue
        {
            get
            {
                return this._strSPCValue;
            }
            set
            {
                this._strSPCValue = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_COMMENTS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string Comment
        {
            get
            {
                return this._strComment;
            }
            set
            {
                this._strComment = value;
            }
        }


        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_SNos, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public string SNos
        {
            get
            {
                return this._strSNos;
            }
            set
            {
                this._strSNos = value;
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
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MeasureID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]

        public int MeasureID
        {
            get
            {
                return this._intMeasureID;
            }
            set
            {
                this._intMeasureID = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_UOMID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int UOMID
        {
            get
            {
                return this._intUOMID;
            }
            set
            {
                this._intUOMID = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_PerUOMID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int PerUOMID
        {
            get
            {
                return this._intPerUOMID;
            }
            set
            {
                this._intPerUOMID = value;
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

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_SNo, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
        public string SNo
        {
            get
            {
                return this._strSNo;
            }
            set
            {
                this._strSNo = value;
            }
        }

        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            var dalc = new DALCBase<GlobalParametersBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieve()
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_GLPARAMS_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_SNo, DbType.String, SNo);
            var ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
        public DataSet retrieveUOMPerUOM()
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_UOMPerUOM, DALCResultType.DataSet);
           // criteria.AddInParameter(Parameters.PARAM_SNo, DbType.String, SNo);
            var ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
        public DataSet retrieveMeasure()
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_Measure, DALCResultType.DataSet);
            // criteria.AddInParameter(Parameters.PARAM_SNo, DbType.String, SNo);
            var ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
      
        /// <summary>
        /// To check whether the GLParams is already exists
        /// </summary>
        /// <returns>integer value</returns>
        public int exists()
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_GLPARAMS_DOESEXIST, DALCResultType.Scalar);
            
            criteria.AddInParameter(Parameters.PARAM_SNo, DbType.String, SNo);
            criteria.AddInParameter(Parameters.PARAM_SPCVariable, DbType.String,SPCVariable);
          
            var count = (int)criteria.ExecuteCommand();
            
            Dictionary<string , object> output = criteria.OutputParameters;
         
            return (count); 

        }

    }
}

