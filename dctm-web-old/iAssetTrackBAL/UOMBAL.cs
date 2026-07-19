/*
File Name   :	AssetModelBAL.cs

Description :	Business Logic layer for UOM

Date created:	21 july 2011

Modification History:
***********************
CR		Name			Date			Description
New		Nayana M    	21 July 2011	File has been created.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    public class UOMBAL
    {
        private int uomID;
        private string uomFrom;
        private string uomTo;
        private float factor;
        private int category;

        public int UOMID
        {
            get
            {
                return this.uomID;
            }
            set
            {
                this.uomID = value;
            }
        }

        public string UOMFrom
        {
            get
            {
                return this.uomFrom;
            }
            set
            {
                this.uomFrom = value;
            }
        }

        public string UOMTo
        {
            get
            {
                return this.uomTo;
            }
            set
            {
                this.uomTo = value;
            }
        }

        public float UOMFactor
        {
            get
            {
                return this.factor;
            }
            set
            {
                this.factor = value;
            }
        }

        public int Category
        {
            get
            {
                return category;
            }
            set
            {
                this.category = value;
            }
        }

        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            DALCBase<UOMBAL> dalc = new DALCBase<UOMBAL>(this);
            //Need to add below commented line of code if any operations are performed.
            //dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_UOM_LIST, DALCResultType.DataSet);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// To check whether the Business Unit is already exists - Not needed
        /// </summary>
        /// <returns>integer value</returns>
        //public int exists()
        //{
        //    DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_SPCMASTER_DOESEXIST, DALCResultType.Scalar);

        //    criteria.AddInParameter(Parameters.PARAM_MODELID, DbType.Int32, SPCID);

        //    //criteria.AddOutParameter("@count", DbType.Int32,0,5);

        //    int count = (int)criteria.ExecuteCommand();

        //    Dictionary<string, object> output = criteria.OutputParameters;
        //    //return() - Return No. of Records
        //    return (count);
        //}
    }
}
