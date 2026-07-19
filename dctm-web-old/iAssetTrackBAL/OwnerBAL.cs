using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;


namespace iAssetTrack.BAL
{
    [DALCOperationSP(InsertSP = StoredProcedures.SP_OWNER_UPDATE, UpdateSP = StoredProcedures.SP_OWNER_UPDATE, DeleteSP = StoredProcedures.SP_OWNER_DELETE)]
    public class OwnerBAL
    {
        private int intOwnerID;
        private string strFirstName;
        private string strLastName;
        private string strEmail;
        private int intDivisionID;
        private int intCreatedBy;
        private int intLastModifiedBy;
        private int intStatus;
        private string strOwnerIDs;

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_FIRSTNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string FirstName
        {
            get
            {
                return this.strFirstName;
            }
            set
            {
                this.strFirstName = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_OWNERID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
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

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_LASTNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string LastName
        {
            get
            {
                return this.strLastName;
            }
            set
            {
                this.strLastName = value;
            }
        }


        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_EMAIL, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string Email
        {
            get
            {
                return this.strEmail;
            }
            set
            {
                this.strEmail = value;
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

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_DIVISIONID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
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

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_OWNERIDS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public string OwnerIDs
        {
            get
            {
                return this.strOwnerIDs;
            }
            set
            {
                this.strOwnerIDs = value;
            }
        }

        public void Persist(DALCOperation operation)
        {
            DALCBase<OwnerBAL> dalc = new DALCBase<OwnerBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrive user details
        /// </summary>
        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_OWNER_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_OWNERID, DbType.Int32, OwnerID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// Retrive Owner data by other fields like Division, First Name and Last Name
        /// </summary>
        public DataSet retrieveByOtherFields()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_OWNER_LIST_BY_SEARCH, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_OWNERID, DbType.Int32, OwnerID);
            criteria.AddInParameter(Parameters.PARAM_DIVISIONID, DbType.Int32, DivisionID);
            criteria.AddInParameter(Parameters.PARAM_FIRSTNAME, DbType.String, FirstName);
            criteria.AddInParameter(Parameters.PARAM_LASTNAME, DbType.String, LastName);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// ceeck user exist or not
        /// </summary>
        public int exists()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_OWNER_DOESEXIST, DALCResultType.Scalar);

            criteria.AddInParameter(Parameters.PARAM_OWNERID, DbType.Int32, OwnerID);
            criteria.AddInParameter(Parameters.PARAM_FIRSTNAME, DbType.String, FirstName);
            criteria.AddInParameter(Parameters.PARAM_LASTNAME, DbType.String, LastName);
            criteria.AddInParameter(Parameters.PARAM_EMAIL, DbType.String, Email);

            int count = (int)criteria.ExecuteCommand();

            Dictionary<string, object> output = criteria.OutputParameters;
            return (count);

        }

    }
}