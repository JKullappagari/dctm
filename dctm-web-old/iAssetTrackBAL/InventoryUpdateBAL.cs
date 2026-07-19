/*
File Name   :	InventoryUpdateBAL.cs

Description :	Business Logic layer for InventoryUpdate

Date created:	28 May 2012

Modification History:
***********************
CR		Name			Date			Description
New		Jagadeesh	28 May 2012		File has been created.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    [DALCOperationSP(UpdateSP = StoredProcedures.SP_INVENTORY_UPDATE)]
    public class InventoryUpdateBAL
    {
        private string strID;
        private int intLocationID;
        private int intCreatedBy;
        private string strAssetIDs;

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_INVUPDATE_ID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string ID
        {
            get
            {
                return this.strID;
            }
            set
            {
                this.strID = value;
            }
        }
        

        //[DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_INVUPDATE_LOCATIONID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int LocationID
        {
            get
            {
                return this.intLocationID;
            }
            set
            {
                this.intLocationID = value;
            }
        }



       
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_INVUPDATE_ASSETIDS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string AssetIDs
        {
            get
            {
                return this.strAssetIDs;
            }
            set
            {
                this.strAssetIDs = value;
            }
        }

        
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

        //[DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MODIFIEDBY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        //public int LastModifiedBy
        //{
        //    get
        //    {
        //        return this.intLastModifiedBy;
        //    }
        //    set
        //    {
        //        this.intLastModifiedBy = value;
        //    }
        //}

        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            DALCBase<InventoryUpdateBAL> dalc = new DALCBase<InventoryUpdateBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieve(string LocationIDs, int pageindex, int pagesize)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_INVENTORY_UPDATE_DATA, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_LOC_LIST, DbType.String, LocationIDs);
            criteria.AddInParameter("@PageNum", DbType.Int32, pageindex);
            criteria.AddInParameter("@PageSize", DbType.Byte, pagesize);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        

        

    }
}
