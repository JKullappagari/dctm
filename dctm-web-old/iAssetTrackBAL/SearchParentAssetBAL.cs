using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
   public class SearchParentAssetBAL
    {
        private int intSiteID;
        private int intBusinessUnitID;
        private int intLocationID;
        private int intMfgID;
        private int intModelID;
        private string strRefNumber;


        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSET_SITEID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
       
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
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_LOCATIONID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
       
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
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MFGID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int MFGID
        {
            get
            {
                return this.intMfgID;
            }
            set
            {
                this.intMfgID = value;
            }
        }
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MODELID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int ModelID
        {
            get
            {
                return this.intModelID;
            }
            set
            {
                this.intModelID = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ASSETNUMBER, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string RefNumber
        {
            get
            {
                return this.strRefNumber;
            }
            set
            {
                this.strRefNumber = value;
            }
        }
        public DataSet ParentAssetSearch()
        {

            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_PARENTASSET_SEARCH, DALCResultType.DataSet);

            criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, BusinessUnitID);
            criteria.AddInParameter("@pIntPrimarySite", DbType.Int32, SiteID);
            criteria.AddInParameter(Parameters.PARAM_LOCATIONID, DbType.Int32, LocationID);
            criteria.AddInParameter(Parameters.PARAM_MFGID, DbType.Int32,MFGID);
            criteria.AddInParameter(Parameters.PARAM_MODELID, DbType.Int32,ModelID);
            criteria.AddInParameter(Parameters.PARAM_ASSETNUMBER, DbType.String, strRefNumber);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            return ds;
        }
    }
}
