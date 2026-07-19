using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
   // [DALCOperationSP(InsertSP = StoredProcedures.SP_CUSTOMER_UPDATE, DeleteSP = StoredProcedures.SP_CUSTOMER_DELETE)]
    public class SearchProjectBAL
    {
        private int intIndustryID;
        private string strShortName;
        private string strProjectCode;
        private DateTime dtStartDate;
        private DateTime dtEndDate;
        private int intCustomerID;
        private string strProjectName;
        private int intCountryID;
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_INDUSTRYID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int IndustryID
        {
            get
            {
                return this.intIndustryID;
            }
            set
            {
                this.intIndustryID = value;
            }
        }
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_COST_COUNTRY_ID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
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
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_SHORTNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string ShortName
        {
            get
            {
                return this.strShortName;
            }
            set
            {
                this.strShortName = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_PROJECTCODE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string ProjectCode
        {
            get
            {
                return this.strProjectCode;
            }
            set
            {
                this.strProjectCode = value;
            }
        }
        [DALCOperationParams(DataType = DbType.DateTime, ParameterName = Parameters.PARAM_STARTDATE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
       
        public DateTime StartDate
        {
            get
            {
                return this.dtStartDate;
            }
            set
            {
                this.dtStartDate = value;
            }
        }
        [DALCOperationParams(DataType = DbType.DateTime, ParameterName = Parameters.PARAM_ENDDATE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        
        public DateTime EndDate
        {
            get
            {
                return this.dtEndDate;
            }
            set
            {
                this.dtEndDate = value;
            }
        }
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_CUSTOMERID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int CustomerID
        {
            get
            {
                return this.intCustomerID;
            }
            set
            {
                this.intCustomerID = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_PROJECTNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string ProjectName
        {
            get
            {
                return this.strProjectName;
            }
            set
            {
                this.strProjectName = value;
            }
        }
        public DataSet ProjectDetailsSearch()
        {

            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_PROJECTDETAILS_SEARCH, DALCResultType.DataSet);

            criteria.AddInParameter(Parameters.PARAM_PROJECTCODE, DbType.String, ProjectCode);
            criteria.AddInParameter("@pVchShortName", DbType.String, strShortName);
            criteria.AddInParameter(Parameters.PARAM_INDUSTRYID, DbType.Int32, IndustryID);
            criteria.AddInParameter("@pIntCountryID", DbType.Int32, CountryID);
            //criteria.AddInParameter("@pdtStartDate", DbType.DateTime, StartDate);
            //criteria.AddInParameter("@pdtEndDate", DbType.DateTime, EndDate);
            //criteria.AddInParameter(Parameters.PARAM_CITYID, DbType.Int32, CityID);

            DataSet ds = (DataSet)criteria.ExecuteCommand();
            return ds;
        }
        public DataSet SearchProject()
        {

            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_PROJECT_SEARCH, DALCResultType.DataSet);

            criteria.AddInParameter(Parameters.PARAM_PROJECTNAME, DbType.String, strProjectName);
            criteria.AddInParameter(Parameters.PARAM_INDUSTRYID, DbType.Int32, IndustryID);
            criteria.AddInParameter(Parameters.PARAM_CUSTOMERID, DbType.Int32, CustomerID);

            DataSet ds = (DataSet)criteria.ExecuteCommand();
            return ds;
        }

    }
}
