/*
File Name   :	DeviceBAL.cs

Description :	business access layer for DeviceReg

Date created:	07 May 2012

Modification History:
***********************
CR		Name			Date			Description
New		Jagadeesh	07 May 2012		File has been created.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    [DALCOperationSP(InsertSP = StoredProcedures.SP_MOBILEDEVICE_UPDATE, DeleteSP = StoredProcedures.SP_MOBILEDEVICE_DELETE)]
    public class DeviceBAL
    {
        private int intID;
        private string strDeviceID;
        private string strDeviceName;
        private int intSiteID;
        private int intStatus;
        private int intCreatedBy;
        private int intLastModifiedBy;
        private string strIDs;
       
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_DEVICEID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string DeviceID
        {
            get
            {
                return this.strDeviceID;
            }
            set
            {
                this.strDeviceID = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_DEVICENAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string DeviceName
        {
            get
            {
                return this.strDeviceName;
            }
            set
            {
                this.strDeviceName = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_SITEID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int SiteID
        {
            get
            {
                return this.intSiteID;
            }
            set
            {
                this.intSiteID = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_IDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public string IDs
        {
            get
            {
                return this.strIDs;
            }
            set
            {
                this.strIDs = value;
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

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
        public int ID
        {
            get
            {
                return this.intID;
            }
            set
            {
                this.intID = value;
            }
        }


        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            DALCBase<DeviceBAL> dalc = new DALCBase<DeviceBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_MOBILEDEVICE_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_ID, DbType.Int32, ID);
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
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_MOBILEDEVICE_DOESEXIST, DALCResultType.Scalar);
            
            criteria.AddInParameter(Parameters.PARAM_ID, DbType.Int32, ID);
            criteria.AddInParameter(Parameters.PARAM_DEVICEID, DbType.String, DeviceID);
            criteria.AddInParameter(Parameters.PARAM_DEVICENAME, DbType.String, DeviceName);//Added on 28-feb-2013
            
            //criteria.AddOutParameter("@count", DbType.Int32,0,5);

            int count = (int)criteria.ExecuteCommand();
            
            Dictionary<string , object> output = criteria.OutputParameters;
            //return() - Return No. of Records
            return (count); 

        }

    }
}
