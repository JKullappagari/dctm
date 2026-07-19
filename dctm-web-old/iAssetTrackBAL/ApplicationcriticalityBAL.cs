/*
File Name   :	ApplicationcriticalityBAL.cs

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
    [DALCOperationSP(InsertSP = StoredProcedures.SP_APPL_CRITICALITY_UPDATE, DeleteSP = StoredProcedures.SP_APPL_CRITICALITY_DELETE)]
    public class ApplicationCriticalityBAL
    {
        private int intApplCriticalityID;
        private string strApplCriticality;
        private string strDescription;
        private int intCreatedBy;
        private int intLastModifiedBy;
        private string strApplCriticalityIDs;
        private int intStatus;
        //v3.9
        private string strBackColorCode;
        private string strForeColorCode;


        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_APPL_CRITICALITY_ID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
        public int ApplCriticalityID
        {
            get
            {
                return this.intApplCriticalityID;
            }
            set
            {
                this.intApplCriticalityID = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_APPL_CRITICALITY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string ApplCriticality
        {
            get
            {
                return this.strApplCriticality;
            }
            set
            {
                this.strApplCriticality = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_APPL_CRITICALITY_DESCRIPTION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
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
         
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_APPL_CRITICALITY_IDS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public string ApplCriticalityIDs
        {
            get
            {
                return this.strApplCriticalityIDs;
            }
            set
            {
                this.strApplCriticalityIDs = value;
            }
        }

        //-->v3.9
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_APPL_CRITICALITY_BACK_COLOR_CODE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string BackColorCode
        {
            get
            {
                return this.strBackColorCode;
            }
            set
            {
                this.strBackColorCode = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_APPL_CRITICALITY_FORE_COLOR_CODE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string ForeColorCode
        {
            get
            {
                return this.strForeColorCode;
            }
            set
            {
                this.strForeColorCode = value;
            }
        }

        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            DALCBase<ApplicationCriticalityBAL> dalc = new DALCBase<ApplicationCriticalityBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_APPL_CRITICALITY_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_APPL_CRITICALITY_ID, DbType.Int32, ApplCriticalityID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// To check whether the Application Criticality already exists
        /// </summary>
        /// <returns>integer value</returns>
        public int exists()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_APPL_CRITICALITY_DOESEXIST, DALCResultType.Scalar);

            criteria.AddInParameter(Parameters.PARAM_APPL_CRITICALITY_ID, DbType.Int32, ApplCriticalityID);
            criteria.AddInParameter(Parameters.PARAM_APPL_CRITICALITY, DbType.String, ApplCriticality);

            //criteria.AddOutParameter("@count", DbType.Int32,0,5);

            int count = (int)criteria.ExecuteCommand();

            Dictionary<string, object> output = criteria.OutputParameters;
            //return() - Return No. of Records
            return (count);

        }
    }
}
