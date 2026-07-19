using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    [DALCOperationSP(InsertSP = StoredProcedures.SP_PURPOSE_UPDATE, DeleteSP = StoredProcedures.SP_PURPOSE_DELETE)]
    public class PurposesBAL
    {
        private int intPurposeID;
        private string strPurpose;
        private string strDescription;
        private int intStatus;
        private int intCreatedBy;
        private int intLastModifiedBy;
        private string strPurposeIDs;

         [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_PURPOSE , ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string Purpose
        {
            get
            {
                return this.strPurpose;
            }
            set
            {
                this.strPurpose  = value;
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
         [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_PURPOSEIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
         public string PurposeIDs
         {

             get
             {
                 return this.strPurposeIDs;
             }
             set
             {
                 this.strPurposeIDs = value;
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
         [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_PURPOSEID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
         public int PurposeID
         {
             get
             {
                 return this.intPurposeID;
             }
             set
             {
                 this.intPurposeID = value;
             }
         }
         public void Persist(DALCOperation operation)
         {
             DALCBase<PurposesBAL> dalc = new DALCBase<PurposesBAL>(this);
             dalc.SaveData(operation, 1);
         }
         public DataSet retrieve()
         {
             DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_PURPOSE_LIST, DALCResultType.DataSet);
             criteria.AddInParameter(Parameters.PARAM_PURPOSEID, DbType.Int32, PurposeID);
             DataSet ds = (DataSet)criteria.ExecuteCommand();
             Dictionary<string, object> output = criteria.OutputParameters;
             return (ds);
         }
         public DataSet retrieveByBusinessUnitId(int intBUId)
         {
             DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_PURPOSEBYBU_LIST, DALCResultType.DataSet);
             criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, intBUId);
             DataSet ds = (DataSet)criteria.ExecuteCommand();
             Dictionary<string, object> output = criteria.OutputParameters;
             return (ds);
         }


         public int exists()
         {
             DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_PURPOSE_DOESEXIST, DALCResultType.Scalar);

             criteria.AddInParameter(Parameters.PARAM_PURPOSEID, DbType.Int32, PurposeID);
             criteria.AddInParameter(Parameters.PARAM_PURPOSE, DbType.String, Purpose);

             //criteria.AddOutParameter("@count", DbType.Int32,0,5);

             int count = (int)criteria.ExecuteCommand();

             Dictionary<string, object> output = criteria.OutputParameters;
             //return() - Return No. of Records
             return (count);

         }
    }
}
