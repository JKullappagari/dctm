/*
File Name   :	BUDivAssignmentBAL.cs

Description :	Business Logic layer for BU and Division Assignment setup

Date created:	03 Aug 2011

Modification History:
***********************
CR		Name		    Date			Description
New		Nayana M    	03 Aug 2011 	File has been created.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    [DALCOperationSP(InsertSP = StoredProcedures.SP_BUDIVASSIGNMENT_UPDATE)]
    public class BUDivAssignmentBAL
    {
        private int intBUDivID;
        private int intBusinessUnitID;
        private int intDivisionID;
        private int intDivAccessID;
        private int intStatus;
        private int intCreatedBy;
        private string strDelimiters;
        private int intLastModifiedBy;
        private string strDivisionIDs;

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
        //[DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_DIVISIONID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int DivisionID
        {
            get
            {
                return this.intDivisionID;
            }
            set
            {
                this.intDivisionID = value;
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

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_DIVISIONIDS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
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
        public int BUDivID
        {
            get
            {
                return this.intBUDivID;
            }
            set
            {
                this.intBUDivID = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_DIVACCESSID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int DivAccessID
        {
            get
            {
                return this.intDivAccessID;
            }
            set
            {
                this.intDivAccessID = value;
            }
        }

        public void Persist(DALCOperation operation)
        {
            DALCBase<BUDivAssignmentBAL> dalc = new DALCBase<BUDivAssignmentBAL>(this);
            dalc.SaveData(operation, 1);
        }

        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_BUDIVASSIGNMENT_LIST, DALCResultType.DataSet);
            // TODO: Check if the below parameter PARAM_BUDIVID is used
            criteria.AddInParameter(Parameters.PARAM_BUDIVID, DbType.Int32, BUDivID);
            //criteria.AddInParameter(Parameters.PARAM_DIVISIONID, DbType.Int32, DivisionID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        public DataSet retrieveAvailDivisions()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_AVAILDIVISION_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, BusinessUnitID);
            // TODO: Check if the below parameter PARAM_DIVACCESSID is used
            criteria.AddInParameter(Parameters.PARAM_DIVACCESSID, DbType.Int32, DivAccessID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }


        public DataSet retrieveAssignDivision()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSIGNDIVISION_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, BusinessUnitID);
            criteria.AddInParameter(Parameters.PARAM_DIVACCESSID, DbType.Int32, DivAccessID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
    }
}
