/*
File Name   :	DepartmentsBAL.cs

Description :	Business Logic layer for Department setup

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
    [DALCOperationSP(InsertSP = StoredProcedures.SP_DEPARTMENT_UPDATE, DeleteSP = StoredProcedures.SP_DEPARTMENT_DELETE)]
    public class DepartmentsBAL
    {
        private int intDepartmentID;
        private string strDepartment;
        private string strDescription;
        private int intStatus;
        private int intCreatedBy;
        private int intLastModifiedBy;
        private string strDepartmentIDs;


        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_DEPARTMENT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string Department
        {
            get
            {
                return this.strDepartment;
            }
            set
            {
                this.strDepartment = value;
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

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_DEPARTMENTIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
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

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_DEPARTMENTID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
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

        public void Persist(DALCOperation operation)
        {
            DALCBase<DepartmentsBAL> dalc = new DALCBase<DepartmentsBAL>(this);
            dalc.SaveData(operation, 1);
        }

        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_DEPARTMENT_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_DEPARTMENTID, DbType.Int32, DepartmentID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
        public DataSet retrieveByBusinessUnitId(int intBUId)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_DEPARTMENTBYBU_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, intBUId);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        public int exists()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_DEPARTMENT_DOESEXIST, DALCResultType.Scalar);

            criteria.AddInParameter(Parameters.PARAM_DEPARTMENTID, DbType.Int32, DepartmentID);
            criteria.AddInParameter(Parameters.PARAM_DEPARTMENT, DbType.String, Department);

            //criteria.AddOutParameter("@count", DbType.Int32,0,5);

            int count = (int)criteria.ExecuteCommand();

            Dictionary<string, object> output = criteria.OutputParameters;
            //return() - Return No. of Records
            return (count);

        }

    }
}