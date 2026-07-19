/*
File Name:	AssetBarringBAL.cs

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

[DALCOperationSP(InsertSP = "", DeleteSP = "", UpdateSP = StoredProcedures.SP_ASSET_BARRING)]
   public class AssetBarringBAL
    {
      //Property variables        

    //Property methods
    [DALCOperationParams(DataType = DbType.Int32, ParameterName =Parameters.PARAM_ASSETID, ParameterDirection = ParameterDirection.Output, DALCOperation = DALCOperation.Insert), DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSETID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update), DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSETID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
    public int AssetID { get; set; }

    [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_BARREDREASON, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
    public string BarredReason { get; set; }

    [DALCOperationParams(DataType = DbType.DateTime, ParameterName = Parameters.PARAM_BARREDFROMDATE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
    public DateTime BarredFromDate { get; set; }

    [DALCOperationParams(DataType = DbType.DateTime, ParameterName = Parameters.PARAM_BARREDTODATE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
    public DateTime BarredToDate { get; set; }

    [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_UPDATEDBY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update, ParameterSize = 4)]
    public int UpdatedBy { get; set; }

    /// <summary>
      /// Used to save data to the database.
      /// </summary>
      /// <author>Atchuta</author>
      /// <createdOn>27 Dec 2006</createdOn>
      public void Persist(DALCOperation operation)
      {
            if (operation == (DALCOperation)1 || operation == (DALCOperation)2 || operation == (DALCOperation)3)
            {
                var dalc = new DALCBase<AssetBarringBAL>(this);
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
       /// Used to get worker details from database.
       /// </summary>
       /// <author>Atchuta</author>
       /// <createdOn>27 Dec 2006</createdOn>
       public DataRow FillAssetDetails()
       {
           var criteria = new DALCCommandHelper(StoredProcedures.SP_ASSET_SELECTED_DETAILS_BYASSETID, DALCResultType.DataSet);
           criteria.AddInParameter(Parameters.PARAM_ASSETID, DbType.Int32, AssetID);
           var ds = (DataSet)criteria.ExecuteCommand();
           Dictionary<string, object> output = criteria.OutputParameters;
           return ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0] : null;
       }

    }
