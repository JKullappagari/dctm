/*
File Name   :	AuditCycle.cs

Description :	Business Logic layer for AuditCycle

Date created:	

*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    [DALCOperationSP(InsertSP = StoredProcedures.SP_AUDITCYCLE_UPDATE, DeleteSP = StoredProcedures.SP_AUDITCYCLE_DELETE)]
    public class AuditCycleBAL
    {
        private int intAuditCycleID;
        //private string strMusterReason;
        //private string strDescription;
        private int intLocationID;
        private DateTime dtStartDate;
        private DateTime dtEndDate;
        private int intCreatedBy;
        private string strAuditCycleIDs;
       
        //[DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_MUSTERREASON, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        //public string MusterReason
        //{
        //    get
        //    {
        //        return this.strMusterReason;
        //    }
        //    set
        //    {
        //        this.strMusterReason = value;
        //    }
        //}
        //[DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_DESCRIPTION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        //public string Description
        //{
        //    get
        //    {
        //        return this.strDescription;
        //    }
        //    set
        //    {
        //        this.strDescription = value;
        //    }
        //}

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_AUDITCYCLEIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public string AuditCycleIDs
        {
            get
            {
                return this.strAuditCycleIDs;
            }
            set
            {
                this.strAuditCycleIDs = value;
            }
        }
        //[DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_AUDITCYCLECOUNT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        ////[DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_STATUS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        //public int AuditCycleCount
        //{
        //    get
        //    {
        //        return this.intAuditCycleCount;
        //    }
        //    set
        //    {
        //        this.intAuditCycleCount = value;
        //    }
        //}
        [DALCOperationParams(DataType = DbType.DateTime, ParameterName = Parameters.PARAM_AUDIT_STARTDATE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.DateTime, ParameterName = Parameters.PARAM_AUDIT_STARTDATE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public DateTime StartDate
        {
            get
            {
                return this.dtStartDate;
            }
            set
            {
                this.dtStartDate = value;
            }
        }
        [DALCOperationParams(DataType = DbType.DateTime, ParameterName = Parameters.PARAM_AUDIT_ENDDATE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.DateTime, ParameterName = Parameters.PARAM_AUDIT_ENDDATE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public DateTime EndDate
        {
            get
            {
                return this.dtEndDate;
            }
            set
            {
                this.dtEndDate = value;
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
        //public int LastModifiedBy
        //{
        //    get
        //    {
        //        return this.intLastModifiedBy;
        //    }
        //    set
        //    {
        //        this.intLastModifiedBy = value;
        //    }
        //}

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_AUDITCYCLEID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
        public int ID
        {
            get
            {
                return this.intAuditCycleID;
            }
            set
            {
                this.intAuditCycleID = value;
            }
        }
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_LOCATIOONID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int LocationID
        {
            get
            {
                return this.intLocationID;
            }
            set
            {
                this.intLocationID = value;
            }
        }

        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            DALCBase<AuditCycleBAL> dalc = new DALCBase<AuditCycleBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_AUDITCYCLE_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_AUDITCYCLEID, DbType.Int32, ID);
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
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_AUDITCYCLE_DOESEXIST, DALCResultType.Scalar);

            criteria.AddInParameter(Parameters.PARAM_AUDITCYCLEID, DbType.Int32, ID);
            criteria.AddInParameter(Parameters.PARAM_LOCATIOONID, DbType.Int32, LocationID);
            criteria.AddInParameter(Parameters.PARAM_AUDIT_STARTDATE, DbType.DateTime, StartDate);
            criteria.AddInParameter(Parameters.PARAM_AUDIT_ENDDATE, DbType.DateTime, EndDate);
          
            //criteria.AddOutParameter("@count", DbType.Int32,0,5);

            int count = (int)criteria.ExecuteCommand();
            
            Dictionary<string , object> output = criteria.OutputParameters;
            //return() - Return No. of Records
            return (count); 

        }

    }
}
