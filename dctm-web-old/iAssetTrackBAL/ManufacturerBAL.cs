/*
File Name   :	ManufacturerBAL.cs

Description :	Business Logic layer forManufacturer setup

Date created:	22 June 2011

Modification History:
***********************
CR		Name			    Date			Description
New		Jaagdeesh Babu K	22 June 2011	File has been created.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    [DALCOperationSP(InsertSP = StoredProcedures.SP_MANUFACTURER_UPDATE, DeleteSP = StoredProcedures.SP_MANUFACTURER_DELETE)]
    public class ManufacturerBAL
    {
        private int intMfgID;
        private string strMfgName;
        private string strDescription;
        private int intStatus;
        private int intCreatedBy;
        private int intLastModifiedBy;
        private string strMfgNameIDs;
       
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_MFGNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string MfgName
        {
            get
            {
                return this.strMfgName;
            }
            set
            {
                this.strMfgName = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_MFGDESCRIPTION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
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

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_MFGIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public string MFGIDs
        {
            get
            {
                return this.strMfgNameIDs;
            }
            set
            {
                this.strMfgNameIDs = value;
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

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MFGID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
        public int MFGID
        {
            get
            {
                return this.intMfgID;
            }
            set
            {
                this.intMfgID = value;
            }
        }

        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            DALCBase<ManufacturerBAL> dalc = new DALCBase<ManufacturerBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_MANUFACTURER_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_MFGID, DbType.Int32, MFGID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public DataSet retrieveUserList()
        //{
        //    DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_USER_LIST_BY_BUSINESSUNIT, DALCResultType.DataSet);
        //    criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, BusinessUnitID);
        //    DataSet ds = (DataSet)criteria.ExecuteCommand();
        //    Dictionary<string, object> output = criteria.OutputParameters;
        //    return (ds);
        //}



        /// <summary>
        /// To check whether the Business Unit is already exists
        /// </summary>
        /// <returns>integer value</returns>
        public int exists()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_MANUFACTURER_DOESEXIST, DALCResultType.Scalar);

            criteria.AddInParameter(Parameters.PARAM_MFGID, DbType.Int32, MFGID);
            criteria.AddInParameter(Parameters.PARAM_MFGNAME, DbType.String, MfgName);
            
            //criteria.AddOutParameter("@count", DbType.Int32,0,5);

            int count = (int)criteria.ExecuteCommand();
            
            Dictionary<string , object> output = criteria.OutputParameters;
            //return() - Return No. of Records
            return (count); 

        }

        /// <summary>
        /// Retrieve Manufacturers by Model Type
        /// </summary>
        /// <param name="ModelType"></param>
        /// <returns></returns>
        public DataSet retrieveByModelType(string ModelType)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_MANUFACTURER_LIST_BY_MODEL_TYPE, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_MFGID, DbType.Int32, MFGID);
            criteria.AddInParameter(Parameters.PARAM_MODEL_TYPE, DbType.String, ModelType);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

    }
}
