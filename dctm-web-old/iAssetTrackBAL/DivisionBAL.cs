/*
File Name   :	AssetModelBAL.cs

Description :	Business Logic layer for Division setup

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

    [DALCOperationSP(InsertSP = StoredProcedures.SP_DIVISION_UPDATE, DeleteSP = StoredProcedures.SP_DIVISION_DELETE)]
    public class DivisionBAL
    {
        private int intDivisionlID;
        private int intBUID;
        private string strDivision;
        private string strDescription;
        private int intCreatedBy;
        private int intLastModifiedBy;
        private string strDivisionIDs;
        private int intStatus;



        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_DIVISIONID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
        public int DivisionID
        {
            get
            {
                return this.intDivisionlID;
            }
            set
            {
                this.intDivisionlID = value;
            }
        }

        //[DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_DIVISION_BUID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
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

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_DIVISIONNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string Division
        {
            get
            {
                return this.strDivision;
            }
            set
            {
                this.strDivision = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_DIVISIONDESCRIPTION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
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
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_DIVISIONIDS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public string DivisionIDs
        {
            get
            {
                return this.strDivisionIDs;
            }
            set
            {
                this.strDivisionIDs = value;
            }
        }

        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            DALCBase<DivisionBAL> dalc = new DALCBase<DivisionBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_DIVISION_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_DIVISIONID, DbType.Int32, DivisionID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// Retrieve records By BU
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieveByBusinessUnitId(int BusinessUnitID)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_DIVISIONBYBU_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_DIVISIONID, DbType.Int32, DivisionID);
            criteria.AddInParameter(Parameters.PARAM_DIVISION_BUID, DbType.Int32, BusinessUnitID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// To check whether the Division already exists
        /// </summary>
        /// <returns>integer value</returns>
        public int exists()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_DIVISION_DOESEXIST, DALCResultType.Scalar);

            criteria.AddInParameter(Parameters.PARAM_DIVISIONID, DbType.Int32, DivisionID);
            criteria.AddInParameter(Parameters.PARAM_DIVISIONNAME, DbType.String, Division);

            //criteria.AddOutParameter("@count", DbType.Int32,0,5);

            int count = (int)criteria.ExecuteCommand();

            Dictionary<string, object> output = criteria.OutputParameters;
            //return() - Return No. of Records
            return (count);

        }
    }
}
