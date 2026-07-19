/*
File Name   :	BusinessUnitBAL.cs

Description :	Business Logic layer for Business unit setup

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
    [DALCOperationSP(InsertSP = StoredProcedures.SP_BUSINESSUNIT_UPDATE, DeleteSP = StoredProcedures.SP_BUSINESSUNIT_DELETE)]
    public class BusinessUnitBAL
    {
        private int intBusinessUnitID;
        private string strBusinessUnit;
        private string strDescription;
        private string strCoPrefix;
        private int intStatus;
        private int intCreatedBy;
        private int intLastModifiedBy;
        private string strBusinessUnitIDs;
        private Nullable<bool> isIA;
       
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_BUSINESSUNIT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string BusinessUnit
        {
            get
            {
                return this.strBusinessUnit;
            }
            set
            {
                this.strBusinessUnit = value;
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

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_COPREFIX, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string CoPrefix
        {
            get
            {
                return this.strCoPrefix;
            }
            set
            {
                this.strCoPrefix = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_BUSINESSUNITIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public string BusinessUnitIDs
        {
            get
            {
                return this.strBusinessUnitIDs;
            }
            set
            {
                this.strBusinessUnitIDs = value;
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

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_BUSINESSUNITID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
        public int BusinessUnitID
        {
            get
            {
                return this.intBusinessUnitID;
            }
            set
            {
                this.intBusinessUnitID = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Boolean, ParameterName = Parameters.PARAM_FROMIA, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public Nullable<bool> FromIA
        {
            get
            {
                return this.isIA;
            }
            set
            {
                if (isIA.HasValue)
                {
                    isIA = false;
                }
                else
                {
                    isIA = value;
                }
            }
        }


        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            DALCBase<BusinessUnitBAL> dalc = new DALCBase<BusinessUnitBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_BUSINESSUNIT_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, BusinessUnitID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// Retrieve records by BusinessUnit Name
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieveByBusinessUnitName()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_BUSINESSUNIT_LISTBYBUNAME, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_BUSINESSUNIT, DbType.String, BusinessUnit);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet retrieveUserList()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_USER_LIST_BY_BUSINESSUNIT, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, BusinessUnitID);
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
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_BUSINESSUNIT_DOESEXIST, DALCResultType.Scalar);
            
            criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, BusinessUnitID);
            criteria.AddInParameter(Parameters.PARAM_BUSINESSUNIT, DbType.String, BusinessUnit);
            
            //criteria.AddOutParameter("@count", DbType.Int32,0,5);

            int count = (int)criteria.ExecuteCommand();
            
            Dictionary<string , object> output = criteria.OutputParameters;
            //return() - Return No. of Records
            return (count); 

        }

    }
}
