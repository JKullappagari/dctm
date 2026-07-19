using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    public class ConfigBAL
    {
        string m_ConfigKey;

        public string ConfigKey
        {
            get { return m_ConfigKey; }
            set { m_ConfigKey = value; }
        }
        string m_ConfigValue;

        public string ConfigValue
        {
            get { return m_ConfigValue; }
            set { m_ConfigValue = value; }
        }


        public void Retrieve()
        {
            //* Debasish (Redundant)
            //DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_GETCONFIGVALUE, DALCResultType.DataSet);
            //criteria.AddInParameter(Parameters.PARAM_CONFIGKEY, DbType.String, ConfigKey);
            //DataSet ds = (DataSet)criteria.ExecuteCommand();
            //Dictionary<string, object> output = criteria.OutputParameters;

            //this.ConfigValue = ds.Tables[0].Rows[0]["ConfigValue"].ToString();
             

            //return (ds);
            

        }

        public DataSet RetrieveAssetStatus()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSET_RETRIEVE_STATUS, DALCResultType.DataSet);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            return (ds);
        }

    }
}
