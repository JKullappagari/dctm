/*
File Name:	AssetCreationBAL.cs

Description:	Used to update validated data to the database.	

Date created:	27 Dec 2006

Modification History:
***********************
CR		Name			Date			Description
New		Atchuta 		27/12/2006		File has been created.
*/
using System;
using iAssetTrack.DALC;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    [DALCOperationSP(InsertSP = "", DeleteSP = StoredProcedures.SP_SITE_DELETE, UpdateSP = StoredProcedures.SP_ASSET_UPDATE)]
    public class AssetCreationBAL
    {
        //Property variables 
        // (Redundant) private byte bytIsPermRestrict;

        //Property methods



        //added by venu

        [DALCOperationParams(DataType = DbType.String, ParameterName = "@pVarRefNumber", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string RefNumber { get; set; }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = "@pIntAssetTypeID", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int AssetTypeID { get; set; }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = "@pIntParentAssetID", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int ParentAssetID { get; set; }

        // (Redundant) [DALCOperationParams(DataType = DbType.Int32, ParameterName = "@pIntNoOfPages", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        //public int NoOfPages { get; set; }

        [DALCOperationParams(DataType = DbType.DateTime, ParameterName = "@pDtExpiryDate", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public DateTime ExpiryDate { get; set; }

        [DALCOperationParams(DataType = DbType.String, ParameterName = "@pVarAssetName", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string AssetName { get; set; }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = "@pIntBusinessUnitID", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int BusinessUnitID { get; set; }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = "@pIntPrimarySiteID", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int PrimarySiteID { get; set; }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = "@pIntLocationID", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int LocationID { get; set; }

        // Start from here ..
        [DALCOperationParams(DataType = DbType.String, ParameterName = "@pDtAssetCreatedDate", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public DateTime AssetCreatedDate { get; set; }

        [DALCOperationParams(DataType = DbType.DateTime, ParameterName = "@pDtRFIDAssignDate", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public DateTime RFIDAssignedDate { get; set; }


        [DALCOperationParams(DataType = DbType.DateTime, ParameterName = "@pDtBarStartDate", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public DateTime BarStartDate { get; set; }

        [DALCOperationParams(DataType = DbType.DateTime, ParameterName = "@pDtBarEndDate", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public DateTime BarEndDate { get; set; }


        [DALCOperationParams(DataType = DbType.String, ParameterName = "@pDtIssuedDate", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public DateTime IssuedDate { get; set; }


        [DALCOperationParams(DataType = DbType.DateTime, ParameterName = "@pDtReceivedDate", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public DateTime ReceivedDate { get; set; }

        [DALCOperationParams(DataType = DbType.DateTime, ParameterName = "@pDtLastSeenLocationTime", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public DateTime LastSeenLocationTime { get; set; }

        //ends 

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSETID, ParameterDirection = ParameterDirection.Output, DALCOperation = DALCOperation.Insert), DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSETID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Update), DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSETID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public int AssetID { get; set; }

        [DALCOperationParams(DataType = DbType.String, ParameterName = "@pChrCurrentStatus", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public char CurrentStatus { get; set; }

        [DALCOperationParams(DataType = DbType.String, ParameterName = "@pVarCurrentRFIDCardNumber", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string CurrentRFIDCardNumber { get; set; }


        [DALCOperationParams(DataType = DbType.Int32, ParameterName = "@pIntCurrentOwnerID", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int CurrentOwnerID { get; set; }


        [DALCOperationParams(DataType = DbType.Int32, ParameterName = "@pIntAssetCreatedBy", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int AssetCreatedBy { get; set; }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = "@pIntIssuedBy", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int IssuedBy { get; set; }


        [DALCOperationParams(DataType = DbType.Int32, ParameterName = "@pIntIssuedTo", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int IssuedTo { get; set; }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = "@pIntReceivedBy", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int ReceivedBy { get; set; }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = "@pIntLastSeenLocationID", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int LastSeenLocationID { get; set; }

        [DALCOperationParams(DataType = DbType.Byte, ParameterName = "@pBytIsApproved", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public byte IsApproved { get; set; }

        [DALCOperationParams(DataType = DbType.Byte, ParameterName = "@pBytIsParent", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public byte IsParent { get; set; }


        [DALCOperationParams(DataType = DbType.String, ParameterName = "pVar@CurrentOwner", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string CurrentOwner { get; set; }

        [DALCOperationParams(DataType = DbType.String, ParameterName = "@pVarPrinterName", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string PrinterName { get; set; }

        [DALCOperationParams(DataType = DbType.Byte, ParameterName = "@pBytIsPermRestrict", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public byte IsPermRestrict
        {
            get { return IsPermRestrict; }
            set { IsPermRestrict = value; }
        }

        [DALCOperationParams(DataType = DbType.Byte, ParameterName = "@pBytIsMustered", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public byte IsMustered { get; set; }

        [DALCOperationParams(DataType = DbType.Byte, ParameterName = "@pBytIsWriteOff", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public byte IsWriteOff { get; set; }


        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_UPDATEDBY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update, ParameterSize = 4)]
        public int UpdatedBy { get; set; }


        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_RESULT, ParameterDirection = ParameterDirection.Output, DALCOperation = DALCOperation.Update)]
        public int Result { get; set; }

        /// <summary>
        /// Used to save data to the database.
        /// </summary>
        /// <author>Atchuta</author>
        /// <createdOn>27 Dec 2006</createdOn>
        public void Persist(DALCOperation operation)
        {
            if (operation == (DALCOperation)1 || operation == (DALCOperation)2 || operation == (DALCOperation)3)
            {
                var dalc = new DALCBase<AssetCreationBAL>(this);
                dalc.SaveData(operation, 1);
            }
            else
            {

                var criteria = new DALCCommandHelper("usp_Retrieve", DALCResultType.DataSet);
                criteria.AddInParameter("@column1", DbType.String, "123");
                criteria.AddOutParameter("@column", DbType.String, null, 20);
                var ds = (DataSet)criteria.ExecuteCommand();
                Dictionary<string, object> output = criteria.OutputParameters;
            }
        }





        /// <summary>
        /// Used to get site details from database.
        /// </summary>
        /// <author>Atchuta</author>
        /// <createdOn>27 Dec 2006</createdOn>
        public DataSet GetSiteList(DALCOperation operation)
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_SITE_LIST, DALCResultType.DataSet);
            var ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return ds;
        }

        /// <summary>
        /// Used to get worker details from database.
        /// </summary>
        /// <author>Atchuta</author>
        /// <createdOn>27 Dec 2006</createdOn>
        public DataTable GetAssetDetails()
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_ASSET_VIEWDETAILS_BYASSETID, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_ASSETID, DbType.Int32, AssetID);
            var ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return ds.Tables[0];
        }
       
        public DataSet GetAlertDetails(int intAssetID)
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_ASSET_ALERT_DETAILS, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_ASSETID, DbType.Int32, intAssetID);
            var ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return ds;
        }



        /// <summary>
        /// Used to get worker view details from database.
        /// </summary>
        /// <author>Atchuta</author>
        /// <createdOn>27 Dec 2006</createdOn>
        public DataTable GetAssetViewDetails()
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_ASSET_VIEWDETAILS_BYASSETID, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_ASSETID, DbType.Int32, AssetID);
            var ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return ds.Tables[0];
        }


        public DataSet GetAssetListByLocations(string LocationIDs, int pageindex, int pagesize)
        {
            DataSet ds;
            try
            {
                var criteria = new DALCCommandHelper(StoredProcedures.SP_ASSET_WITH_APPLICATION_GETBYPAGE, DALCResultType.DataSet);
                criteria.AddInParameter("@pVarLocationIDs", DbType.String, LocationIDs);
                criteria.AddInParameter("@PageNum", DbType.Int32, pageindex);
                criteria.AddInParameter("@PageSize", DbType.Byte, pagesize);

                ds = (DataSet)criteria.ExecuteCommand();
                //return ds;
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return ds;
        }


        /// <summary>
        /// Used to get Asset list details from database.
        /// </summary>
        /// <author>Atchuta</author>
        /// <createdOn>27 Dec 2006</createdOn>

        public DataTable GetAssetsList(int intBusinessUnitID, int intPrimarySite, int intAssetTypeID, int intLocationID, int intCurrentOwnerID, string strRefNumber, string strAssetName, char chrRFIDStatus, int intAssetStatus, string strOwner, int intUserID)
        {
            var criteria = new DALCCommandHelper("iAssetTrack_SP_ASSET_Search", DALCResultType.DataSet);
            criteria.AddInParameter("@pIntBusinessUnitID", DbType.Int32, intBusinessUnitID);
            criteria.AddInParameter("@pIntPrimarySite", DbType.Int32, intPrimarySite);
            criteria.AddInParameter("@pIntAssetTypeID", DbType.Int32, intAssetTypeID);
            criteria.AddInParameter("@pIntLocationID", DbType.Int32, intLocationID);
            criteria.AddInParameter("@pIntCurrentOwnerID", DbType.Int32, intCurrentOwnerID);
            criteria.AddInParameter("@pVarRefNumber", DbType.Int32, strRefNumber);
            criteria.AddInParameter("@pVarAssetName", DbType.Int32, strAssetName);
            criteria.AddInParameter("@pChrRFIDStatus", DbType.String, chrRFIDStatus);
            criteria.AddInParameter("@pIntAssetStatus", DbType.Int32, intAssetStatus);
            criteria.AddInParameter("@pVarOwner", DbType.String, strOwner);
            criteria.AddInParameter("@pIntUserID", DbType.Int32, intUserID);

            var ds = (DataSet)criteria.ExecuteCommand();
            return ds.Tables[0];
        }

        public DataSet GetAssetsList(int intBusinessUnitID, int intDepartmentID, int intAssetTypeID, int intLocationID, int intCurrentOwnerID, string strRefNumber, string strAssetName, string strHostName, char chrRFIDStatus, int intAssetStatus, string strOwner, int intUserIDint, int pageindex, int pagesize, int intIssuedTo,
            int intReceivedBy, int intAssociated, int intParent, int intParentAsset, string varRFIDTag, int intMfgID, int intModelID)
        {
            DataSet ds;
            try
            {
                var objAssetCreationBAL = new AssetCreationBAL();
                ds = objAssetCreationBAL.GetAssetsListByPage(intBusinessUnitID,
                                                            intDepartmentID,
                                                            intAssetTypeID,
                                                            intLocationID,
                                                            intCurrentOwnerID,
                                                            strRefNumber,
                                                            strAssetName,
                                                            strHostName,///Added on 8May2013
                                                            chrRFIDStatus,
                                                            intAssetStatus,
                                                            strOwner,
                                                            intUserIDint,
                                                            pageindex,
                                                            pagesize,
                                                            intIssuedTo,
                                                            intReceivedBy,
                                                            intAssociated,
                                                            intParent,
                                                            intParentAsset,
                                                            varRFIDTag,
                                                            intMfgID,
                                                            intModelID
                                                            );
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;

        }

       
        

        public DataSet GetAssetsList(int intBusinessUnitID, int intDepartmentID, int intAssetTypeID, int intLocationID, int intCurrentOwnerID, string strRefNumber, string strAssetName, char chrRFIDStatus, int intAssetStatus, string strOwner, int intUserIDint, int pageindex, int pagesize, int intIssuedTo, int intReceivedBy, int intAssociated, int intParent, int intParentAsset, string varRFIDTag)
        {
            DataSet ds;
            try
            {
                var objAssetCreationBAL = new AssetCreationBAL();
                ds = objAssetCreationBAL.GetAssetsListByPage(intBusinessUnitID,
                                                            intDepartmentID,
                                                            intAssetTypeID,
                                                            intLocationID,
                                                            intCurrentOwnerID,
                                                            strRefNumber,
                                                            strAssetName,
                                                            chrRFIDStatus,
                                                            intAssetStatus,
                                                            strOwner,
                                                            intUserIDint,
                                                            pageindex,
                                                            pagesize,
                                                            intIssuedTo,
                                                            intReceivedBy,
                                                            intAssociated,
                                                            intParent,
                                                            intParentAsset,
                                                            varRFIDTag);
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return ds;

        }


        /// <summary>
        /// Used to get worker list details from database.
        /// </summary>
        /// <author>Atchuta</author>
        /// <createdOn>27 Dec 2006</createdOn>
        public DataSet GetAssetsListByPage(int intBusinessUnitID, int intPrimarySite, int intAssetTypeID, int intLocationID, int intCurrentOwnerID, string strRefNumber, string strAssetName, char chrRFIDStatus, int intAssetStatus, string strOwner, int intUserID, int pageindex, int pagesize, int intIssuedTo, int intReceivedBy, int intAssociated, int intParent, int intParentAsset, string varRFIDTag)
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_ASSET_GETBYPAGE, DALCResultType.DataSet);
            criteria.AddInParameter("@pIntBusinessUnitID", DbType.Int32, intBusinessUnitID);
            criteria.AddInParameter("@pIntPrimarySite", DbType.Int32, intPrimarySite);
            criteria.AddInParameter("@pIntAssetGroupID", DbType.Int32, intAssetTypeID);
            criteria.AddInParameter("@pIntLocationID", DbType.Int32, intLocationID);
            criteria.AddInParameter("@pIntCurrentOwnerID", DbType.Int32, intCurrentOwnerID);
            criteria.AddInParameter("@pVarRefNumber", DbType.String, strRefNumber);
            criteria.AddInParameter("@pVarAssetName", DbType.String, strAssetName);
            criteria.AddInParameter("@pChrRFIDStatus", DbType.String, chrRFIDStatus);
            criteria.AddInParameter("@pIntAssetStatus", DbType.Int32, intAssetStatus);
            criteria.AddInParameter("@pVarOwner", DbType.String, strOwner);
            criteria.AddInParameter("@pIntUserID", DbType.Int32, intUserID);
            criteria.AddInParameter("@PageNum", DbType.Int32, pageindex);
            criteria.AddInParameter("@PageSize", DbType.Byte, pagesize);
            criteria.AddInParameter("@pIntIssuedTo", DbType.Int32, intIssuedTo);
            criteria.AddInParameter("@pIntReceivedBy", DbType.Int32, intReceivedBy);
            criteria.AddInParameter("@pIntAssociated", DbType.Int32, intAssociated);
            criteria.AddInParameter("@pIntParent", DbType.Int32, intParent);
            criteria.AddInParameter("@pIntParentAssetID", DbType.Int32, intParentAsset);
            criteria.AddInParameter("@pVarRFIDTag", DbType.String, varRFIDTag);
            //criteria.AddInParameter("@pIntPrintCopyNumber", DbType.Int32, intCopyNumber);

            var ds = (DataSet)criteria.ExecuteCommand();
            return ds;
        }


        public DataSet GetAssetsListByPage(int intBusinessUnitID, int intPrimarySite, int intAssetTypeID, int intLocationID,
            int intCurrentOwnerID, string strRefNumber, string strAssetName, string strHostName, char chrRFIDStatus, int intAssetStatus,
            string strOwner, int intUserID, int pageindex, int pagesize, int intIssuedTo, int intReceivedBy,
            int intAssociated, int intParent, int intParentAsset, string varRFIDTag, int intMfgID,
            int intModelID)
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_ASSET_GETBYPAGE, DALCResultType.DataSet);
            criteria.AddInParameter("@pIntBusinessUnitID", DbType.Int32, intBusinessUnitID);
            criteria.AddInParameter("@pIntPrimarySite", DbType.Int32, intPrimarySite);
            criteria.AddInParameter("@pIntAssetGroupID", DbType.Int32, intAssetTypeID);
            criteria.AddInParameter("@pIntLocationID", DbType.Int32, intLocationID);
            criteria.AddInParameter("@pIntCurrentOwnerID", DbType.Int32, intCurrentOwnerID);
            criteria.AddInParameter("@pVarRefNumber", DbType.String, strRefNumber);
            criteria.AddInParameter("@pVarAssetName", DbType.String, strAssetName);
           criteria.AddInParameter("@pVarHost", DbType.String, strHostName);///Added on 8May2013
            criteria.AddInParameter("@pChrRFIDStatus", DbType.String, chrRFIDStatus);
            criteria.AddInParameter("@pIntAssetStatus", DbType.Int32, intAssetStatus);
            criteria.AddInParameter("@pVarOwner", DbType.String, strOwner);
            criteria.AddInParameter("@pIntUserID", DbType.Int32, intUserID);
            criteria.AddInParameter("@PageNum", DbType.Int32, pageindex);
            criteria.AddInParameter("@PageSize", DbType.Byte, pagesize);
            criteria.AddInParameter("@pIntIssuedTo", DbType.Int32, intIssuedTo);
            criteria.AddInParameter("@pIntReceivedBy", DbType.Int32, intReceivedBy);
            criteria.AddInParameter("@pIntAssociated", DbType.Int32, intAssociated);
            criteria.AddInParameter("@pIntParent", DbType.Int32, intParent);
            criteria.AddInParameter("@pIntParentAssetID", DbType.Int32, intParentAsset);
            criteria.AddInParameter("@pVarRFIDTag", DbType.String, varRFIDTag);
            criteria.AddInParameter("@pIntMfgID", DbType.Int32, intMfgID);
            criteria.AddInParameter("@pIntModelID", DbType.Int32, intModelID);

            //criteria.AddInParameter("@pIntPrintCopyNumber", DbType.Int32, intCopyNumber);

            var ds = (DataSet)criteria.ExecuteCommand();
            return ds;
        }

        public DataSet GetAssetsListByPageForExport(int intBusinessUnitID, int intPrimarySite, int intAssetTypeID, int intLocationID,
            int intCurrentOwnerID, string strRefNumber, string strAssetName, string strHostName, char chrRFIDStatus, int intAssetStatus,
            string strOwner, int intUserID, int pageindex, int pagesize, int intIssuedTo, int intReceivedBy,
            int intAssociated, int intParent, int intParentAsset, string varRFIDTag, int intMfgID,
            int intModelID)
        {
            DataSet ds ;
            try
            {
                var criteria = new DALCCommandHelper(StoredProcedures.SP_ASSET_GETBYPAGE_FOR_EXPORT, DALCResultType.DataSet);
                criteria.AddInParameter("@pIntBusinessUnitID", DbType.Int32, intBusinessUnitID);
                criteria.AddInParameter("@pIntPrimarySite", DbType.Int32, intPrimarySite);
                criteria.AddInParameter("@pIntAssetGroupID", DbType.Int32, intAssetTypeID);
                criteria.AddInParameter("@pIntLocationID", DbType.Int32, intLocationID);
                criteria.AddInParameter("@pIntCurrentOwnerID", DbType.Int32, intCurrentOwnerID);
                criteria.AddInParameter("@pVarRefNumber", DbType.String, strRefNumber);
                criteria.AddInParameter("@pVarAssetName", DbType.String, strAssetName);
                criteria.AddInParameter("@pVarHost", DbType.String, strHostName);///Added on 8May2013
                criteria.AddInParameter("@pChrRFIDStatus", DbType.String, chrRFIDStatus);
                criteria.AddInParameter("@pIntAssetStatus", DbType.Int32, intAssetStatus);
                criteria.AddInParameter("@pVarOwner", DbType.String, strOwner);
                criteria.AddInParameter("@pIntUserID", DbType.Int32, intUserID);
                criteria.AddInParameter("@PageNum", DbType.Int32, pageindex);
                criteria.AddInParameter("@PageSize", DbType.Byte, pagesize);
                criteria.AddInParameter("@pIntIssuedTo", DbType.Int32, intIssuedTo);
                criteria.AddInParameter("@pIntReceivedBy", DbType.Int32, intReceivedBy);
                criteria.AddInParameter("@pIntAssociated", DbType.Int32, intAssociated);
                criteria.AddInParameter("@pIntParent", DbType.Int32, intParent);
                criteria.AddInParameter("@pIntParentAssetID", DbType.Int32, intParentAsset);
                criteria.AddInParameter("@pVarRFIDTag", DbType.String, varRFIDTag);
                criteria.AddInParameter("@pIntMfgID", DbType.Int32, intMfgID);
                criteria.AddInParameter("@pIntModelID", DbType.Int32, intModelID);

                //criteria.AddInParameter("@pIntPrintCopyNumber", DbType.Int32, intCopyNumber);

                ds = (DataSet)criteria.ExecuteCommand();
            }
            catch 
            {
                throw new Exception();
            }
            return ds;
        }


        # region v3.8
        public DataSet GetAssetsList(int intBusinessUnitID, int intDepartmentID, int intAssetTypeID, int intLocationID, int intCurrentOwnerID, string strRefNumber, string strAssetName, string strHostName, char chrRFIDStatus, int intAssetStatus, string strOwner, int intUserIDint, int pageindex, int pagesize, int intIssuedTo,
           int intReceivedBy, int intAssociated, int intParent, int intParentAsset, string varRFIDTag, int intMfgID, int intModelID, int SiteRestrictionEnabled, int LoggedInUserID)
        {
            DataSet ds;
            try
            {
                var objAssetCreationBAL = new AssetCreationBAL();
                ds = objAssetCreationBAL.GetAssetsListByPage(intBusinessUnitID,
                                                            intDepartmentID,
                                                            intAssetTypeID,
                                                            intLocationID,
                                                            intCurrentOwnerID,
                                                            strRefNumber,
                                                            strAssetName,
                                                            strHostName,///Added on 8May2013
                                                            chrRFIDStatus,
                                                            intAssetStatus,
                                                            strOwner,
                                                            intUserIDint,
                                                            pageindex,
                                                            pagesize,
                                                            intIssuedTo,
                                                            intReceivedBy,
                                                            intAssociated,
                                                            intParent,
                                                            intParentAsset,
                                                            varRFIDTag,
                                                            intMfgID,
                                                            intModelID,
                                                            SiteRestrictionEnabled,
                                                            LoggedInUserID
                                                            );
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;

        }

        public DataSet GetAssetsListByPage(int intBusinessUnitID, int intPrimarySite, int intAssetTypeID, int intLocationID,
           int intCurrentOwnerID, string strRefNumber, string strAssetName, string strHostName, char chrRFIDStatus, int intAssetStatus,
           string strOwner, int intUserID, int pageindex, int pagesize, int intIssuedTo, int intReceivedBy,
           int intAssociated, int intParent, int intParentAsset, string varRFIDTag, int intMfgID,
           int intModelID, int intSiteResEnabled, int intLoggedInUserID)
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_ASSET_GETBYPAGE, DALCResultType.DataSet);
            criteria.AddInParameter("@pIntBusinessUnitID", DbType.Int32, intBusinessUnitID);
            criteria.AddInParameter("@pIntPrimarySite", DbType.Int32, intPrimarySite);
            criteria.AddInParameter("@pIntAssetGroupID", DbType.Int32, intAssetTypeID);
            criteria.AddInParameter("@pIntLocationID", DbType.Int32, intLocationID);
            criteria.AddInParameter("@pIntCurrentOwnerID", DbType.Int32, intCurrentOwnerID);
            criteria.AddInParameter("@pVarRefNumber", DbType.String, strRefNumber);
            criteria.AddInParameter("@pVarAssetName", DbType.String, strAssetName);
            criteria.AddInParameter("@pVarHost", DbType.String, strHostName);///Added on 8May2013
            criteria.AddInParameter("@pChrRFIDStatus", DbType.String, chrRFIDStatus);
            criteria.AddInParameter("@pIntAssetStatus", DbType.Int32, intAssetStatus);
            criteria.AddInParameter("@pVarOwner", DbType.String, strOwner);
            criteria.AddInParameter("@pIntUserID", DbType.Int32, intUserID);
            criteria.AddInParameter("@PageNum", DbType.Int32, pageindex);
            criteria.AddInParameter("@PageSize", DbType.Byte, pagesize);
            criteria.AddInParameter("@pIntIssuedTo", DbType.Int32, intIssuedTo);
            criteria.AddInParameter("@pIntReceivedBy", DbType.Int32, intReceivedBy);
            criteria.AddInParameter("@pIntAssociated", DbType.Int32, intAssociated);
            criteria.AddInParameter("@pIntParent", DbType.Int32, intParent);
            criteria.AddInParameter("@pIntParentAssetID", DbType.Int32, intParentAsset);
            criteria.AddInParameter("@pVarRFIDTag", DbType.String, varRFIDTag);
            criteria.AddInParameter("@pIntMfgID", DbType.Int32, intMfgID);
            criteria.AddInParameter("@pIntModelID", DbType.Int32, intModelID);
            criteria.AddInParameter("@pBItSiteRestrictionEnabled", DbType.Int16, intSiteResEnabled);
            criteria.AddInParameter("@pIntLoggedInUserID", DbType.Int32, intLoggedInUserID);

            //criteria.AddInParameter("@pIntPrintCopyNumber", DbType.Int32, intCopyNumber);

            var ds = (DataSet)criteria.ExecuteCommand();
            return ds;
        }

        public DataSet GetAssetsListByPageForExport(int intBusinessUnitID, int intPrimarySite, int intAssetTypeID, int intLocationID,
            int intCurrentOwnerID, string strRefNumber, string strAssetName, string strHostName, char chrRFIDStatus, int intAssetStatus,
            string strOwner, int intUserID, int pageindex, int pagesize, int intIssuedTo, int intReceivedBy,
            int intAssociated, int intParent, int intParentAsset, string varRFIDTag, int intMfgID,
            int intModelID , int intSiteResEnabled, int intLoggedInUserID,int intShowInDrillDownOnly)
        {
            DataSet ds;
            try
            {
                var criteria = new DALCCommandHelper(StoredProcedures.SP_ASSET_GETBYPAGE_FOR_EXPORT, DALCResultType.DataSet);
                criteria.AddInParameter("@pIntBusinessUnitID", DbType.Int32, intBusinessUnitID);
                criteria.AddInParameter("@pIntPrimarySite", DbType.Int32, intPrimarySite);
                criteria.AddInParameter("@pIntAssetGroupID", DbType.Int32, intAssetTypeID);
                criteria.AddInParameter("@pIntLocationID", DbType.Int32, intLocationID);
                criteria.AddInParameter("@pIntCurrentOwnerID", DbType.Int32, intCurrentOwnerID);
                criteria.AddInParameter("@pVarRefNumber", DbType.String, strRefNumber);
                criteria.AddInParameter("@pVarAssetName", DbType.String, strAssetName);
                criteria.AddInParameter("@pVarHost", DbType.String, strHostName);///Added on 8May2013
                criteria.AddInParameter("@pChrRFIDStatus", DbType.String, chrRFIDStatus);
                criteria.AddInParameter("@pIntAssetStatus", DbType.Int32, intAssetStatus);
                criteria.AddInParameter("@pVarOwner", DbType.String, strOwner);
                criteria.AddInParameter("@pIntUserID", DbType.Int32, intUserID);
                criteria.AddInParameter("@PageNum", DbType.Int32, pageindex);
                criteria.AddInParameter("@PageSize", DbType.Byte, pagesize);
                criteria.AddInParameter("@pIntIssuedTo", DbType.Int32, intIssuedTo);
                criteria.AddInParameter("@pIntReceivedBy", DbType.Int32, intReceivedBy);
                criteria.AddInParameter("@pIntAssociated", DbType.Int32, intAssociated);
                criteria.AddInParameter("@pIntParent", DbType.Int32, intParent);
                criteria.AddInParameter("@pIntParentAssetID", DbType.Int32, intParentAsset);
                criteria.AddInParameter("@pVarRFIDTag", DbType.String, varRFIDTag);
                criteria.AddInParameter("@pIntMfgID", DbType.Int32, intMfgID);
                criteria.AddInParameter("@pIntModelID", DbType.Int32, intModelID);
                criteria.AddInParameter("@pBItSiteRestrictionEnabled", DbType.Int16, intSiteResEnabled);
                criteria.AddInParameter("@pIntLoggedInUserID", DbType.Int32, intLoggedInUserID);
                criteria.AddInParameter("@pBitShowInDrillDownOnly", DbType.Int32, intShowInDrillDownOnly);

                //criteria.AddInParameter("@pIntPrintCopyNumber", DbType.Int32, intCopyNumber);

                ds = (DataSet)criteria.ExecuteCommand();
            }
            catch
            {
                throw new Exception();
            }
            return ds;
        }

        #endregion

        public DataSet GetAssetHistoryDetails(int intAssetID)
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_ASSET_HISTORY, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_ASSETID, DbType.Int32, intAssetID);
            var ds = (DataSet)criteria.ExecuteCommand();
            return ds;
        }



    }
}
