/*
File Name   :	AssetModelBAL.cs

Description :	Business Logic layer for Application setup

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
    [DALCOperationSP(InsertSP = StoredProcedures.SP_APPLICATION_UPDATE, DeleteSP = StoredProcedures.SP_APPLICATION_DELETE)]
    public class ApplicationBAL
    {
        private int intApplID;
        private int intBUID;
        private string strApplName;
        private string strDescription;
        private int intApplTypeID;
        private int intAppStatusID;
        private int intApplCriticality;
        private int intApplManageID;
        private int intStatus;
        private int intCreatedBy;
        private int intLastModifiedBy;
        private string strApplIDs;
        private int intBusinessUnitId;
        private int intOwnerID;

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_APPLICATIONID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int ApplID
        {
            get
            {
                return this.intApplID;
            }
            set
            {
                this.intApplID = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_APPL_BUID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int BUID
        {
            get
            {
                return this.intBUID;
            }
            set
            {
                this.intBUID = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_APPLICATIONNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string ApplName
        {
            get
            {
                return this.strApplName;
            }
            set
            {
                this.strApplName = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_APPLICATIONDESCRIPTION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
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

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_APPLICATIONTYPEID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int ApplTypeID
        {
            get
            {
                return this.intApplTypeID;
            }
            set
            {
                this.intApplTypeID = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_APPLICATIONSTATUS_ID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int AppStatusID
        {
            get
            {
                return this.intAppStatusID;
            }
            set
            {
                this.intAppStatusID = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_APPLICATIONCRITICALITY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int ApplCriticality
        {
            get
            {
                return this.intApplCriticality;
            }
            set
            {
                this.intApplCriticality = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_OWNERID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int OwnerID
        {
            get
            {
                return this.intOwnerID;
            }
            set
            {
                this.intOwnerID = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_APPLICATIONMANAGEID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int ApplManageID
        {
            get
            {
                return this.intApplManageID;
            }
            set
            {
                this.intApplManageID = value;
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

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_APPLICATIONIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public string ApplIDs
        {
            get
            {
                return this.strApplIDs;
            }
            set
            {
                this.strApplIDs = value;
            }
        }

        public int BusinessUnitID
        {
            get
            {
                return this.intBusinessUnitId;
            }
            set
            {
                this.intBusinessUnitId = value;
            }
        }

        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            DALCBase<ApplicationBAL> dalc = new DALCBase<ApplicationBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_APPLICATION_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_APPLICATIONID, DbType.Int32, ApplID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        //v3.6
        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieveByPage(int PageIndex,int PageSize)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_APPLICATION_LIST_BY_PAGE, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_APPLICATIONID, DbType.Int32, ApplID);
            criteria.AddInParameter(Parameters.PARAM_PAGE_INDEX, DbType.Int32, PageIndex);
            criteria.AddInParameter(Parameters.PARAM_PAGE_SIZE, DbType.Int32, PageSize);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// To check whether the Application is already exists
        /// </summary>
        /// <returns>integer value</returns>
        public int exists()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_APPLICATION_DOESEXIST, DALCResultType.Scalar);

            criteria.AddInParameter(Parameters.PARAM_APPLICATIONID, DbType.Int32, ApplID);
            criteria.AddInParameter(Parameters.PARAM_APPLICATIONNAME, DbType.String, ApplName);
            criteria.AddInParameter(Parameters.PARAM_APPLICATIONTYPEID, DbType.String, ApplTypeID);
            criteria.AddInParameter(Parameters.PARAM_OWNERID, DbType.String, OwnerID);
            criteria.AddInParameter(Parameters.PARAM_APPLICATIONSTATUS_ID, DbType.Int32, AppStatusID);

            //criteria.AddOutParameter("@count", DbType.Int32,0,5);

            int count = (int)criteria.ExecuteCommand();

            Dictionary<string, object> output = criteria.OutputParameters;
            //return() - Return No. of Records
            return (count);

        }

        //-->v3.9
        /// <summary>
        /// Retrieve Applications by App Criticality
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieveByAppCriticality(int AppCriticalityID)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_APPLICATION_LIST_BY_CRITICALITY, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_APPL_CRITICALITY_ID, DbType.Int32, AppCriticalityID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }


    }
}
