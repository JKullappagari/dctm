/*
File Name   :	CountryBAL.cs

Description :	Country BAL

Date created:	06 NOV 2013

Modification History:
***********************
CR		Name			Date			Description
New		Jagadeesh       06 Nov 2013     Country BAL
*/


using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;


namespace iAssetTrack.BAL
{
    //[DALCOperationSP(InsertSP = , DeleteSP = )]
    public class CountryBAL
    {

        #region Declarations
        private int intCountryID;
        private string strCountryName;
        private string strRegion;
        #endregion

        #region Properties
        public string CountryName
        {
            get
            {
                return this.strCountryName;
            }
            set
            {
                this.strCountryName = value;
            }
        }

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


        public string Region
        {
            get { return strRegion; }
            set { strRegion = value; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// To insert,update and delete groups.
        /// </summary>
        /// <param name="operation"></param>
        public void Persist(DALCOperation operation)
        {
            DALCBase<CountryBAL> dalc = new DALCBase<CountryBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieves Country list based on Country
        /// </summary>
        /// <returns></returns>
        public DataSet RetrieveByRegion()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_COUNTRY_LIST_BY_REGION, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_REGION, DbType.String , Region);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        #endregion

    }
}