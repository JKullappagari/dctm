using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using iAssetTrack.DALC;
using iAssetTrackBAL;

// ReSharper disable CheckNamespace
namespace iAssetTrack.BAL
// ReSharper restore CheckNamespace
{
    [DALCOperationSP(UpdateSP = StoredProcedures.SP_ASSET_UPDATE)]
// ReSharper disable InconsistentNaming
    public class AssetBAL
// ReSharper restore InconsistentNaming
    {
// ReSharper disable InconsistentNaming
        int m_AssetID;
// ReSharper restore InconsistentNaming
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSETID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Update)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSETID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
// ReSharper disable InconsistentNaming
        public int AssetID
// ReSharper restore InconsistentNaming
        {
            get { return m_AssetID; }
            set { m_AssetID = value; }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ASSETNUMBER, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string RefNumber { get; set; }

        //[DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ASSET_COPYNUMBER, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        //public string CopyNumber { get; set; }


        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ASSET_NAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string AssetName { get; set; }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSETGROUPID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int AssetTypeId { get; set; }

// ReSharper restore InconsistentNaming

        ////[DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSET_PAGECOUNT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        //public int NoOfPages { get; set; }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSET_BUSINESSUNITID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int BusinessUnitID { get; set; }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSET_SITEID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int PrimarySiteID { get; set; }


        [DALCOperationParams(DataType = DbType.DateTime, ParameterName = Parameters.PARAM_ASSET_CREATEDDATE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public DateTime AssetCreatedDate { get; set; }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSET_CREATEDBY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int AssetCreatedBy { get; set; }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSET_LASTSEENLOCATIONID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int LastSeenLocationID { get; set; }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSET_CURRENTOWNERID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int CurrentOwnerID { get; set; }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSET_UPDATEDBY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update), DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSET_UPDATEDBY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public int UpdatedBy { get; set; }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_RESULT, ParameterDirection = ParameterDirection.Output, DALCOperation = DALCOperation.Update)]
        public int Result { get; set; }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_MESSAGECODE, ParameterSize = 50, ParameterDirection = ParameterDirection.Output, DALCOperation = DALCOperation.Update)]
        public string MessageCode { get; set; }


        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_REASON, ParameterSize = 1000, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public string Reason { get; set; }


        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ACTION, ParameterSize = 50, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public string Action { get; set; }


        [DALCOperationParams(DataType = DbType.Int32, ParameterName = "@pIntIsDocumentNoExists", ParameterDirection = ParameterDirection.Output, DALCOperation = DALCOperation.Delete)]
        public int DeletedId { get; set; }


        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSET_LOCATIONID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int DefaultLocationID { get; set; }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSET_MODELID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int ModelID { get; set; }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ASSET_OS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string OS { get; set; }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ASSET_CPU, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string CPU { get; set; }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSET_CPU_COUNT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int CPUCount { get; set; }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ASSET_CPU_CORE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string CPUCore { get; set; }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSET_TECHID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int TechID { get; set; }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ASSET_RACK_OR_STAND, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string RackOrStand { get; set; }

        //added by kjb on 03 Oct 2011
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSET_START_POS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int StartPos { get; set; }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSET_NO_OF_RUS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int NoOfRUs { get; set; }


        // created by kjb on 05 Oct 2011, flag to check whether insert is from Import
        [DALCOperationParams(DataType = DbType.Boolean, ParameterName = Parameters.PARAM_ASSET_IS_IMPORT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public bool IsImport { get; set; }
        
        //by kjb on 24 Feb 2013 to handle Serail no + Model no check
        [DALCOperationParams(DataType = DbType.Boolean, ParameterName = Parameters.PARAM_ASSET_SERIALNO_MODEL_CHECK, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public bool SerialNoModelCheck { get; set; }

        //*Added on 17April2013
        [DALCOperationParams(DataType = DbType.Boolean, ParameterName = Parameters.PARAM_ASSET_ISPARENT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public bool IsParent { get; set; }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSET_PARENTASSETID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int AssetParentID { get; set; }
        //*
        //*Added on 6May2013
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ASSET_TAG, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string AssetTAG { get; set; }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_HOSTNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string HostName { get; set; }//*

        //*V3.8-Added on 14-Oct-2013-By Amar Vidya
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ASSET_ORIENTATION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string Orientation { get; set; }//*
        
        //*V3.8-Added on 21Oct2013-By Amar Vidya 
        //[DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ASSET_ORIENTATION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        //public string RackTag { get; set; }
        //*
        //int _abc;
        //public int Abc
        //{
        //    get { return _abc; }
        //    set { _abc = value; }
        //}

        //v3.9
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_APPLICATIONS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string ApplicationsList { get; set; }


        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ASSET_INTERNAL_ID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string InternalID { get; set; }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ASSET_EXTERNAL_ID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string ExternalID { get; set; }

        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_ASSET_DERATED_POWER, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public float DeratedPower { get; set; }


        public void Persist(DALCOperation operation)
        {
            var dalc = new DALCBase<AssetBAL>(this);
            dalc.SaveData(operation, 1);
        }


        public DataSet Retrieve()
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_ASSET_RETRIEVE_BYID, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_ASSETID, DbType.Int32, m_AssetID);
            var ds = (DataSet)criteria.ExecuteCommand();
            return (ds);
        }

        //public DataSet GetAssetDetailsModifyAll()
        //{
        //    var criteria = new DALCCommandHelper(StoredProcedures.SP_ASSET_VIEWDETAILSMODIFYALL_BYASSETID, DALCResultType.DataSet);
        //    criteria.AddInParameter(Parameters.PARAM_ASSETIDAll, DbType.String, AssetID);
        //    var ds = (DataSet)criteria.ExecuteCommand();
        //    Dictionary<string, object> output = criteria.OutputParameters;
        //    return (ds);
        //}
        public DataSet GetAssetDetailsModifyAll(string ASSETIDAll)
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_ASSET_VIEWDETAILSMODIFYALL_BYASSETID, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_ASSETIDAll, DbType.String, ASSETIDAll);
            var ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
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

        public DataSet GetAssetByLocationId(int locationId)
        {
                var criteria = new DALCCommandHelper(StoredProcedures.SP_ASSETLIST_GETBYLOCATIONID, DALCResultType.DataSet);
                criteria.AddInParameter(Parameters.PARAM_LOCATIONID, DbType.Int32, locationId);
                var ds = (DataSet)criteria.ExecuteCommand();
                return (ds);
                
        }

        public DataRow GetAssetDetails(string strRefNo, int intAssetID)
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_ASSET_DETAILS_BYASSETNO, DALCResultType.DataSet);
            criteria.AddInParameter("@pVarRefNo", DbType.String, strRefNo);
            criteria.AddInParameter("@pIntAssetID", DbType.Int32, intAssetID);
            var ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0] : null;

        }

        /// <summary>
        /// Get Parent Assset Details by Serial No or Asset Tag
        /// </summary>
        /// <param name="strRefNo"></param>
        /// <param name="intAssetID"></param>
        /// <returns></returns>
        public DataRow GetParentAssetDetails(string strRefNo, string strAssetTag)
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_ASSET_DETAILS_BY_SERIALNO_ASSETTAG, DALCResultType.DataSet);
            criteria.AddInParameter("@pVarRefNo", DbType.String, !string.IsNullOrEmpty(strRefNo) ? strRefNo : null);
            criteria.AddInParameter("@PCurrentRFIDCardNumber", DbType.String, !string.IsNullOrEmpty(strAssetTag) ? strAssetTag : null);
            var ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0] : null;

        }

        // (Redundant) public DataSet RetrieveAssociatedAssets()
        //{
        //    DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSET_ASSOCIATION_LIST, DALCResultType.DataSet);
        //    criteria.AddInParameter(Parameters.PARAM_ASSETID, DbType.Int32, m_AssetID);
        //    DataSet ds = (DataSet)criteria.ExecuteCommand();
        //    return (ds);
        //}

        public bool AssetAppMapExists(string HostName, string Application, int AssetID)
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_ASSET_APP_MAP_EXISTS, DALCResultType.DataSet);
            criteria.AddInParameter("@pVarHostName", DbType.String, HostName);
            criteria.AddInParameter("@pIntAssetID", DbType.Int32, AssetID);
            criteria.AddInParameter("@pVarApplication", DbType.String, Application);
            var ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return ds.Tables[0].Rows.Count > 0 ? true : false;

        }

        public DataSet retrieveEnclPositions()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ENCL_POSITIONS_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_ASSETID, DbType.Int32, AssetID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        public void ReleaseBladePositions(int EnclID, int StartPos,int BladeModelID, int OrientationID)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_BLADE_POSITIONS_UPDATE, DALCResultType.Scalar);

            criteria.AddInParameter(Parameters.PARAM_ENCL_ID, DbType.Int32, EnclID);
            criteria.AddInParameter(Parameters.PARAM_ASSET_START_POS, DbType.Int32, StartPos);
            criteria.AddInParameter(Parameters.PARAM_BLADE_MODEL_ID, DbType.Int32, BladeModelID);
            criteria.AddInParameter(Parameters.PARAM_ORIENTATION_ID, DbType.Boolean, OrientationID);
            criteria.AddInParameter(Parameters.PARAM_OPERATION_ID, DbType.Boolean, 0);

            criteria.ExecuteCommand();

        }
        
    }
}
