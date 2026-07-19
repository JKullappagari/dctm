/*
File Name   :	LocationTypeBAL.cs

Description :	Business Logic layer for LocationType setup

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
    [DALCOperationSP(InsertSP = StoredProcedures.SP_LOCATIONTYPE_UPDATE, DeleteSP = StoredProcedures.SP_LOCATIONTYPE_DELETE)]
    public class LocationTypeBAL
    {
        private int intLocationTypeID;
        private string strLocationType;
        private string strDescription;
        private int intIsStorageType;
        private int intIsRfidLocation;
        private int intStatus;
        private int intCreatedBy;
        private int intLastModifiedBy;
        private string strLocationTypeIDs;


        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_LOCATIONTYPE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string LocationType
        {
            get
            {
                return this.strLocationType;
            }
            set
            {
                this.strLocationType = value;
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
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ISSTORAGETYPE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int IsStorageType
        {
            get
            {
                return this.intIsStorageType;
            }
            set
            {
                this.intIsStorageType = value;
            }
        }
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ISRFIDLOCATION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int IsRfidLocation
        {
            get
            {
                return this.intIsRfidLocation;
            }
            set
            {
                this.intIsRfidLocation = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_LOCATIONTYPEIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public string LocationTypeIDs
        {
            get
            {
                return this.strLocationTypeIDs;
            }
            set
            {
                this.strLocationTypeIDs = value;
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

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_LOCATIONTYPEID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
        public int LocationTypeID
        {
            get
            {
                return this.intLocationTypeID;
            }
            set
            {
                this.intLocationTypeID = value;
            }
        }

        public void Persist(DALCOperation operation)
        {
            DALCBase<LocationTypeBAL> dalc = new DALCBase<LocationTypeBAL>(this);
            dalc.SaveData(operation, 1);
        }

        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_LOCATIONTYPE_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_LOCATIONTYPEID, DbType.Int32, LocationTypeID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }


        public int exists()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_LOCATIONTYPE_DOESEXIST, DALCResultType.Scalar);

            criteria.AddInParameter(Parameters.PARAM_LOCATIONTYPEID, DbType.Int32, LocationTypeID);
            criteria.AddInParameter(Parameters.PARAM_LOCATIONTYPE, DbType.String, LocationType);

            //criteria.AddOutParameter("@count", DbType.Int32,0,5);

            int count = (int)criteria.ExecuteCommand();

            Dictionary<string, object> output = criteria.OutputParameters;
            //return() - Return No. of Records
            return (count);

        }

    }
}