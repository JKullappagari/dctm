/*
File Name   :	SiteLocationAssignmentBAL.cs

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
    [DALCOperationSP(InsertSP = StoredProcedures.SP_SITELOCATIONASSIGNMENT_UPDATE)]
    public class SiteLocationAssignmentBAL
    {
        private int intSiteLocationID;
        private int intBusinessUnitID;
        private int intSiteID;
        private int intStatus;
        private int intCreatedBy;
        private string strDelimiters;
        private int intLastModifiedBy;
        private string strLocationIDs;

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

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_LOCATIONIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string LocationIDs
        {
            get
            {
                return this.strLocationIDs;
            }
            set
            {
                this.strLocationIDs = value;
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

        public int SiteLocationID
        {
            get
            {
                return this.intSiteLocationID;
            }
            set
            {
                this.intSiteLocationID = value;
            }
        }

        public void Persist(DALCOperation operation)
        {
            DALCBase<SiteLocationAssignmentBAL> dalc = new DALCBase<SiteLocationAssignmentBAL>(this);
            dalc.SaveData(operation, 1);
        }

        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_SITELOCATIONASSIGNMENT_LIST, DALCResultType.DataSet);
            //criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, BusinessUnitID);
            //criteria.AddInParameter(Parameters.PARAM_SITEID, DbType.Int32, SiteID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        //public DataSet retrieveAll()
        //{
        //    DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_SITELOCATIONASSIGNMENT_LIST_ALL, DALCResultType.DataSet);
        //    DataSet ds = (DataSet)criteria.ExecuteCommand();
        //    Dictionary<string, object> output = criteria.OutputParameters;
        //    return (ds);
        //}


        public DataSet retrieveAvailLocations()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_AVAILLOCATION_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, BusinessUnitID);
            criteria.AddInParameter(Parameters.PARAM_SITEID, DbType.Int32, SiteID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        public bool CheckSpecialRoom(int LocationID)
        {
            bool returnValue = false;
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_CHECK_SPECIALROOM, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_LOCATIONID, DbType.Int32, LocationID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                if (int.Parse(dr[0].ToString()) > 0)
                {
                    returnValue = true;
                }
                else
                {
                    returnValue = false;
                }
            }
            else
            {
                returnValue = false;
            }

            return returnValue;

        }



        public DataSet retrieveAssignLocation()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSIGNLOCATION_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, BusinessUnitID);
            criteria.AddInParameter(Parameters.PARAM_SITEID, DbType.Int32, SiteID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// Retrives Room list assigned to a Site
        /// </summary>
        /// <returns></returns>
        public DataSet retrieveAssignLocationRoomsOnly()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSIGNLOCATION_ROOMS_ONLY_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, BusinessUnitID);
            criteria.AddInParameter(Parameters.PARAM_SITEID, DbType.Int32, SiteID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }


    }

}



