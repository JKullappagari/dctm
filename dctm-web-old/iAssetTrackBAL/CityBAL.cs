/*
File Name   :	CityBAL.cs

Description :	City BAL

Date created:	06 NOV 2013

Modification History:
***********************
CR		Name			Date			Description
New		Jagadeesh       06 Nov 2013     City BAL
*/


using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;


namespace iAssetTrack.BAL
{
    public class CityBAL
    {

        #region Declarations
        private int intCItyID;
        private string strCityName;
        private int intCountryID;
        #endregion

        #region Properties
        public string CityName
        {
            get
            {
                return this.strCityName;
            }
            set
            {
                this.strCityName = value;
            }
        }
       
        public int CityID
        {
            get
            {
                return this.intCItyID;
            }
            set
            {
                this.intCItyID = value;
            }
        }
        

        public int CountryID
        {
            get { return intCountryID; }
            set { intCountryID = value; }
        }

           #endregion

        #region Methods
        /// <summary>
        /// To insert,update and delete groups.
        /// </summary>
        /// <param name="operation"></param>
        public void Persist(DALCOperation operation)
        {
            DALCBase<CityBAL> dalc = new DALCBase<CityBAL>(this);
            dalc.SaveData(operation, 1);
        }

       /// <summary>
       /// Retrieves City list based on Country
       /// </summary>
       /// <returns></returns>
        public DataSet RetrieveByCountryID()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_CITY_LIST_BY_COUNTRY, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_COUNTRY_ID, DbType.Int32, CountryID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

           #endregion

    }
}