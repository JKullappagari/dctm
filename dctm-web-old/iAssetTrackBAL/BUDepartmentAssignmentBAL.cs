/*
File Name   :	BUDepartmentAssignmentBAL.cs

Description :	Business Logic layer for Business unit and site assignment setup

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
    [DALCOperationSP(InsertSP = StoredProcedures.SP_BUDEPARTMENTASSIGNMENT_UPDATE)]
    public class BUDepartmentAssignmentBAL
    {
        private int intBUDepartmentID;
        private int intBusinessUnitID;
        private int intDepartmentAccessID;
        private int intDepartmentID;
        private int intStatus;
        private int intCreatedBy;
        private string strDelimiters;
        private int intLastModifiedBy;
        private string strDepartmentIDs;

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_BUSINESSUNITID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
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
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_DEPARTMENTACCESSID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int DepartmentAccessID
        {
            get
            {
                return this.intDepartmentAccessID;
            }
            set
            {
                this.intDepartmentAccessID = value;
            }
        }
        public int DepartmentID
        {
            get
            {
                return this.intDepartmentID;
            }
            set
            {
                this.intDepartmentID = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_DELIMITERS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string Delimiters
        {
            get
            {
                return this.strDelimiters;
            }
            set
            {
                this.strDelimiters = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_DEPARTMENTIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string DepartmentIDs
        {
            get
            {
                return this.strDepartmentIDs;
            }
            set
            {
                this.strDepartmentIDs = value;
            }
        }
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_STATUS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
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

        public int BUDepartmentID
        {
            get
            {
                return this.intBUDepartmentID;
            }
            set
            {
                this.intBUDepartmentID = value;
            }
        }

        public void Persist(DALCOperation operation)
        {
            DALCBase<BUDepartmentAssignmentBAL> dalc = new DALCBase<BUDepartmentAssignmentBAL>(this);
            dalc.SaveData(operation, 1);
        }

        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_BUDEPARTMENTASSIGNMENT_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_BUDEPARTMENTID, DbType.Int32, BUDepartmentID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }


        public DataSet retrieveAvailDepartments()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_AVAILDEPARTMENT_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, BusinessUnitID);
            criteria.AddInParameter(Parameters.PARAM_DEPARTMENTACCESSID, DbType.Int32, DepartmentAccessID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }


        public DataSet retrieveAssignDepartment()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSIGNDEPARTMENT_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, BusinessUnitID);
            criteria.AddInParameter(Parameters.PARAM_DEPARTMENTACCESSID, DbType.Int32, DepartmentAccessID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }


    }

}



