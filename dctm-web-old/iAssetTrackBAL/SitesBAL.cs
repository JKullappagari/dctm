/*
File Name   :	SitesBAL.cs

Description :	Business Logic layer for Site setup

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
    [DALCOperationSP(InsertSP = StoredProcedures.SP_SITE_UPDATE, DeleteSP = StoredProcedures.SP_SITE_DELETE,UpdateSP = StoredProcedures.SP_SITE_UPDATE)]
    public class SitesBAL
    {
        private int intSiteID;
        private string strSite;
        private string strDescription;
        private int intStatus;
        private int intCreatedBy;
        private int intLastModifiedBy;
        private string strSiteIDs;
        # region v3.8 - data memebers
        private int intCountryID;
        private int intCityID;
        # endregion


        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_SITE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_SITE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string Site
        {
            get
            {
                return this.strSite;
            }
            set
            {
                this.strSite = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_DESCRIPTION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_DESCRIPTION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
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

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_SITEIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public string SiteIDs
        {
            get
            {
                return this.strSiteIDs;
            }
            set
            {
                this.strSiteIDs = value;
            }
        }
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_STATUS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_STATUS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
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
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_CREATEDBY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
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

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_SITEID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_SITEID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Update)]
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

        # region v3.8 - properties
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_COUNTRY_ID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int CountryID
        {
            get
            {
                return this.intCountryID;
            }
            set
            {
                this.intCountryID = value;
            }
        }


        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_CITY_ID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int CityID
        {
            get
            {
                return this.intCityID;
            }
            set
            {
                this.intCityID = value;
            }
        }

        # endregion

        public void Persist(DALCOperation operation)
        {
            DALCBase<SitesBAL> dalc = new DALCBase<SitesBAL>(this);
            dalc.SaveData(operation, 1);
        }

        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_SITE_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_SITEID, DbType.Int32, SiteID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        # region v3.8 - Methods
        /// <summary>
        /// RETRIEVE site list by user restriction
        /// </summary>
        /// <param name="BusinessUnitID"></param>
        /// <param name="LoggedInUserID"></param>
        /// <returns></returns>
        /// <Version>v3.8</Version>
        public DataSet retrieveRestrictedSiteList(int BusinessUnitID,int LoggedInUserID)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_SITE_LIST_BY_RESTRICTION, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, BusinessUnitID);
            criteria.AddInParameter(Parameters.PARAM_LOGGEDIN_USER_ID, DbType.Int32, LoggedInUserID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        # endregion

        public DataSet retrieveAllSites()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_SITE_ALL_LIST, DALCResultType.DataSet);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        public DataSet retrieveBySiteName()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_SITEBYSITENAME_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_SITE, DbType.String, Site);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        public DataSet retrieveByBusinessUnitId(int intBUId)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_SITEBYBU_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, intBUId);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        public DataSet retrieveByVersionNo(int SPCHeaderID)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_SITE_BY_VERSION_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_SPCHEADERID, DbType.Int32, SPCHeaderID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        public int exists()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_SITE_DOESEXIST, DALCResultType.Scalar);

            criteria.AddInParameter(Parameters.PARAM_SITEID, DbType.Int32, SiteID);
            criteria.AddInParameter(Parameters.PARAM_SITE, DbType.String, Site);

            //criteria.AddOutParameter("@count", DbType.Int32,0,5);

            int count = (int)criteria.ExecuteCommand();

            Dictionary<string, object> output = criteria.OutputParameters;
            //return() - Return No. of Records
            return (count);

        }

    }
}