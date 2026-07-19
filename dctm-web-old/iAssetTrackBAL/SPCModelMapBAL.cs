using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iAssetTrack.DALC;
using iAssetTrackBAL;
using System.Data;


namespace iAssetTrack.BAL
{
    [DALCOperationSP(InsertSP = "", DeleteSP = "", UpdateSP = "")]
    public class SPCModelMapBAL
    {

       
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_SPCMODELMAP_MODELID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int ModelID { get; set; }

       
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_SPCMODELMAP_MFGID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int MfgID { get; set; }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_SPCMODELMAP_SPCID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int SpcID { get; set; }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_SPCMODELMAP_MODELNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string ModelName { get; set; }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_SPCMODELMAP_MFGNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string MfgName { get; set; }


        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_SPCMODELMAP_MAKEMODEL, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public DateTime MakeModel { get; set; }

        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_SPCMODELMAP_MAXWATTS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public double MaxWatts { get; set; }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_SPCMODELMAP_STEADYWATTS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int SteadyStateWatts { get; set; }


        public DataSet Retrieve()
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_SPCMODELMAP_GETDATA, DALCResultType.DataSet);
            var ds = (DataSet)criteria.ExecuteCommand();
            return (ds);
        }

        public DataSet RetrieveByFilter(int filter,int businessUnitID)
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_SPCMODELMAP_GETDATA_BY_FILTER, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_SPCMODELMAP_FILTER, DbType.Int32, filter);
            criteria.AddInParameter(Parameters.PARAM_SPCMODELMAP_BUID, DbType.Int32, businessUnitID);
            var ds = (DataSet)criteria.ExecuteCommand();
            return (ds);
        }

        /// <summary>
        /// retrives data for charts
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="businessUnitID"></param>
        /// <returns></returns>
        /// by kjb on 29 Nov 2012
        public DataSet RetrieveDataForCharts(int BusinessUnitID,int SiteID, int TechID)
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_SPCMODELMAP_DATA_FOR_CHARTS, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_SPCMODELMAP_BUID, DbType.Int32, BusinessUnitID);
            criteria.AddInParameter(Parameters.PARAM_SPCMODELMAP_SiteID, DbType.Int32, SiteID);
            criteria.AddInParameter(Parameters.PARAM_SPCMODELMAP_TechID, DbType.Int32, TechID);

            var ds = (DataSet)criteria.ExecuteCommand();
            return (ds);
        }


        public DataSet RetrieveByAssetModel(string MfgModel) // consists Modelname + mfg name
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_SPCMODELMAP_GETDATA_BY_MFGMODEL, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_SPCMODELMAP_MODELNAME, DbType.String, MfgModel);
            var ds = (DataSet)criteria.ExecuteCommand();
            return (ds);
        }
        //public DataSet PieChart(int businessUnitID)
        //{
        //    DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SPC_ModelMap_Chart, DALCResultType.DataSet);
        //    criteria.AddInParameter(Parameters.PARAM_SPCMODELMAP_BUID, DbType.Int32, businessUnitID);
        //    DataSet ds = (DataSet)criteria.ExecuteCommand();
        //    Dictionary<string, object> output = criteria.OutputParameters;
        //    return (ds);
        //}
        //public DataSet Progressbar(int businessUnitID)
        //{
        //    DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SPC_ModelMap_Progressbar, DALCResultType.DataSet);
        //    criteria.AddInParameter(Parameters.PARAM_SPCMODELMAP_BUID, DbType.Int32, businessUnitID);
        //    DataSet ds = (DataSet)criteria.ExecuteCommand();
        //    Dictionary<string, object> output = criteria.OutputParameters;
        //    return (ds);
        //}

        // (Redundant) public DataSet RetrieveDistibutionList()
        //{
        //    DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSET_DISTRIBUTION_LIST, DALCResultType.DataSet);
        //    DataSet ds = (DataSet)criteria.ExecuteCommand();
        //    return (ds);
        //}


        //(Redundant)public DataSet RetrieveAddresseeList()
        //{
        //    DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSET_ADDRESSEE_LIST, DALCResultType.DataSet);
        //    criteria.AddInParameter(Parameters.PARAM_ASSETID, DbType.Int32, m_AssetID);
        //    DataSet ds = (DataSet)criteria.ExecuteCommand();
        //    return (ds);
        //}

        //public DataSet GetAssetByLocationId(int locationId)
        //{
        //    var criteria = new DALCCommandHelper(StoredProcedures.SP_ASSETLIST_GETBYLOCATIONID, DALCResultType.DataSet);
        //    criteria.AddInParameter(Parameters.PARAM_LOCATIONID, DbType.Int32, locationId);
        //    var ds = (DataSet)criteria.ExecuteCommand();
        //    return (ds);

        //}
    }
}
