/*
File Name:	RFIDCardAssignmentBAL.cs

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

    [DALCOperationSP(InsertSP = "", DeleteSP = "", UpdateSP = StoredProcedures.SP_RFIDCARDASSIGNMENT_UPDATE)]
    public class RFIDCardAssignmentBAL
    {
        //Property variables 
        private int intAssetID;
        private string strRefNumber;
        private string strRFIDCardNumber;
        private string strReason;
        private string strAction;
        private int intUpdatedBy;
        private int intIsAssetExists;
        //private int intAutoCheckOutFlag;
        //private string dtRFIDExpiryDate;

        //Property methods
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSETID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int AssetID
        {
            get
            { return this.intAssetID; }
            set
            { this.intAssetID = value; }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ASSETNUMBER, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string RefNumber
        {
            get { return strRefNumber; }
            set { strRefNumber = value; }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_RFIDCARDNUMBER, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string RFIDCardNumber
        {
            get { return strRFIDCardNumber; }
            set { strRFIDCardNumber = value; }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_REASON, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string Reason
        {
            get { return strReason; }
            set { strReason = value; }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ACTION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string Action
        {
            get { return strAction; }
            set { strAction = value; }
        }
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_UPDATEDBY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update, ParameterSize = 4)]
        public int UpdatedBy
        {
            get
            { return this.intUpdatedBy; }
            set
            { this.intUpdatedBy = value; }
        }
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSET_EXISTS, ParameterDirection = ParameterDirection.Output, DALCOperation = DALCOperation.Update, ParameterSize = 4)]
        public int IsAssetExists
        {
            get
            { return this.intIsAssetExists; }
            set
            { this.intIsAssetExists = value; }
        }

        //[DALCOperationParams(DataType = DbType.Int32, ParameterName = "@pIntAutoCheckOutFlag", ParameterDirection = ParameterDirection.Output, DALCOperation = DALCOperation.Update, ParameterSize = 4)]
        //public int AutoCheckOutFlag
        //{
        //    get
        //    { return this.intAutoCheckOutFlag; }
        //    set
        //    { this.intAutoCheckOutFlag = value; }
        //}


        //----added by venu-------------------------------------------
        // [DALCOperationParams(DataType = DbType.String, ParameterName = "@pRFIDExpiryDate",ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        //public string RFIDExpiryDate
        //{
        //    get
        //    { return this.dtRFIDExpiryDate; }
        //    set
        //    { this.dtRFIDExpiryDate = value; }
        //}

        //------------------------------------------------------------------------

        /// <summary>
        /// Used to save data to the database.
        /// </summary>
        /// <author>Atchuta</author>
        /// <createdOn>27 Dec 2006</createdOn>
        public void Persist(DALCOperation operation)
        {
            if (operation == (DALCOperation)1 || operation == (DALCOperation)2 || operation == (DALCOperation)3)
            {
                DALCBase<RFIDCardAssignmentBAL> dalc = new DALCBase<RFIDCardAssignmentBAL>(this);
                dalc.SaveData(operation, 1);
            }
            else
            {

                DALCCommandHelper criteria = new DALCCommandHelper("usp_Retrieve", DALCResultType.DataSet);
                criteria.AddInParameter("@column1", DbType.String, "123");
                criteria.AddOutParameter("@column", DbType.String, null, 20);
                DataSet ds = (DataSet)criteria.ExecuteCommand();
                Dictionary<string, object> output = criteria.OutputParameters;
            }
        }

        //public DataSet GetSiteList(DALCOperation operation)
        //{
        //    DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_SITE_LIST, DALCResultType.DataSet);
        //    DataSet ds = (DataSet)criteria.ExecuteCommand();
        //    Dictionary<string, object> output = criteria.OutputParameters;
        //    return ds;
        //}

        /// <summary>
        /// Used to get worker details from database.
        /// </summary>
        /// <author>Atchuta</author>
        /// <createdOn>27 Dec 2006</createdOn>
        public DataRow FillWorkerDetails()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSET_DETAILS_BYASSETID, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_ASSETID, DbType.Int32, intAssetID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;

            return ds.Tables[0].Rows[0];

        }

        // (Redundant) public DataTable GenerateEPCForAsset()
        //{
        //    try
        //    {
        //        DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSET_GENERATEEPC, DALCResultType.DataSet);
        //        criteria.AddInParameter(Parameters.PARAM_ASSETID, DbType.Int32, intAssetID);
        //        //criteria.AddOutParameter("@pIntIsAssetExists", DbType.Int32, intIsAssetExists, 4);

        //        DataSet ds = (DataSet)criteria.ExecuteCommand();
        //        Dictionary<string, object> output = criteria.OutputParameters;
        //        return ds.Tables[0];
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        // (Redundant) public DataTable GenerateEPCForAsset1()
        //{
        //    DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSET_GENERATEEPC, DALCResultType.DataSet);
        //    criteria.AddInParameter(Parameters.PARAM_ASSETID, DbType.Int32, intAssetID);
        //    criteria.AddOutParameter("@pIntIsAssetExists", DbType.Int32, intIsAssetExists, 20);

        //    DataSet ds = (DataSet)criteria.ExecuteCommand();
        //    Dictionary<string, object> output = criteria.OutputParameters;
        //    return ds.Tables[0];

        //}

        /// <summary>
        /// Used to get RFID details from database.
        /// </summary>
        /// <author>Atchuta</author>
        /// <createdOn>27 Dec 2006</createdOn
        // (Redundant) public DataTable RFIDCardAssignmentList(string strAssetIDs)
        //{
        //    try
        //    {
        //        DALCCommandHelper criteria = new

        //        DALCCommandHelper(StoredProcedures.SP_RFIDCARDASSIGNMENT_LIST, DALCResultType.DataSet);
        //        criteria.AddInParameter("@pVarAssetIDS", DbType.String, strAssetIDs);
        //        DataSet ds = (DataSet)criteria.ExecuteCommand();
        //        Dictionary<string, object> output = criteria.OutputParameters;
        //        return ds.Tables[0];
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
    }
}
