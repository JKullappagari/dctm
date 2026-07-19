using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    [DALCOperationSP(InsertSP = StoredProcedures.SP_CUSTOMER_UPDATE, DeleteSP = StoredProcedures.SP_CUSTOMER_DELETE)]
    public class CountryStateCityBAL
    {
             
        private int countryID;
        //private int stateID;
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_COST_COUNTRY_ID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int CountryID
        {
            get
            {
                return this.countryID;
            }
            set
            {
                this.countryID = value;
            }
        }


        public DataSet retrieveCountry()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_COST_COUNTRY_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_COST_COUNTRY_ID, DbType.Int32, CountryID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        public DataSet retrieveStateByCountyID()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_COST_STATE_LIST_BY_COUNTRY, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_COST_COUNTRY_ID, DbType.Int32, CountryID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
        public DataSet retrieveCityByCountyID()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_COST_CITY_LIST_BY_COUNTRY, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_COST_COUNTRY_ID, DbType.Int32, CountryID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
       
    }
}
