/*
File Name   :	MusterReasonBAL.cs

Description :	Business Logic layer for Mustering reasons

Date created:	27 March 2006

Modification History:
***********************
CR		Name			Date			Description
New		Venkatesan M	27/03/2006		File has been created.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    [DALCOperationSP(InsertSP = StoredProcedures.SP_MUSTERREASON_UPDATE, DeleteSP = StoredProcedures.SP_MUSTERREASON_DELETE)]
    public class MusterReasonBAL
    {
        private int intMusterReasonID;
        private string strMusterReason;
        private string strDescription;
        private int intStatus;
        private int intCreatedBy;
        private int intLastModifiedBy;
        private string strMusterReasonIDs;
       
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_MUSTERREASON, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string MusterReason
        {
            get
            {
                return this.strMusterReason;
            }
            set
            {
                this.strMusterReason = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_DESCRIPTION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
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

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_MUSTERREASONIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public string MusterReasonIDs
        {
            get
            {
                return this.strMusterReasonIDs;
            }
            set
            {
                this.strMusterReasonIDs = value;
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

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MUSTERREASONID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
        public int MusterReasonID
        {
            get
            {
                return this.intMusterReasonID;
            }
            set
            {
                this.intMusterReasonID = value;
            }
        }

        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            DALCBase<MusterReasonBAL> dalc = new DALCBase<MusterReasonBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_MUSTERREASON_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_MUSTERREASONID, DbType.Int32, MusterReasonID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// To check whether the Muster Reason exists
        /// </summary>
        /// <returns>integer value</returns>
        public int exists()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_MUSTERREASON_DOESEXIST, DALCResultType.Scalar);
            
            criteria.AddInParameter(Parameters.PARAM_MUSTERREASONID, DbType.Int32, MusterReasonID);
            criteria.AddInParameter(Parameters.PARAM_MUSTERREASON, DbType.String, MusterReason);
            
            //criteria.AddOutParameter("@count", DbType.Int32,0,5);

            int count = (int)criteria.ExecuteCommand();
            
            Dictionary<string , object> output = criteria.OutputParameters;
            //return() - Return No. of Records
            return (count); 

        }

    }
}
