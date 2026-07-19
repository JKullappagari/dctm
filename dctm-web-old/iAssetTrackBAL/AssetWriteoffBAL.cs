/*
File Name:	AssetWriteoffBAL.cs

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

[DALCOperationSP(InsertSP = "", DeleteSP = "", UpdateSP = StoredProcedures.SP_ASSET_WRITEOFF)]
   public class AssetWriteoffBAL
    {

      //Property variables 
      private int intAssetID;
      private string strRestrictionReason;
      private int intUpdatedBy;
      private int intReasonID;


      //Property methods
      [DALCOperationParams(DataType = DbType.Int32, ParameterName =Parameters.PARAM_ASSETID, ParameterDirection = ParameterDirection.Output, DALCOperation = DALCOperation.Insert)]
      [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSETID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
      [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ASSETID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
      public int AssetID
      {
          get
          { return this.intAssetID; }
          set
          { this.intAssetID = value;}
      }
       [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_REASON, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
       public string RestrictionReason
      {
          get { return strRestrictionReason; }
          set { strRestrictionReason = value; }
      }

       [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MUSTERREASONID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
       public int ReasonID
       {
           get { return intReasonID; }
           set { intReasonID = value; }
       }

      [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_UPDATEDBY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update, ParameterSize = 4)]
      public int UpdatedBy
      {
          get
          { return this.intUpdatedBy; }
          set
          { this.intUpdatedBy = value;}
      }

      /// <summary>
      /// Used to save data to the database.
      /// </summary>
      /// <author>Atchuta</author>
      /// <createdOn>27 Dec 2006</createdOn>
      public void Persist(DALCOperation operation)
      {
            if (operation == (DALCOperation)1 || operation == (DALCOperation)2 || operation == (DALCOperation)3)
            {
                DALCBase<AssetWriteoffBAL> dalc = new DALCBase<AssetWriteoffBAL>(this);
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

       /// <summary>
       /// Used to get worker details from database.
       /// </summary>
       /// <author>Atchuta</author>
       /// <createdOn>27 Dec 2006</createdOn>
       public DataRow FillAssetDetails()
       {
           DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSET_DETAILS_BYASSETID, DALCResultType.DataSet);
           criteria.AddInParameter(Parameters.PARAM_ASSETID, DbType.Int32, intAssetID);
           DataSet ds = (DataSet)criteria.ExecuteCommand();
           Dictionary<string, object> output = criteria.OutputParameters;
           if (ds.Tables[0].Rows.Count > 0)
               return ds.Tables[0].Rows[0];
           else
               return null;
       }

       public bool CheckChildAssetsWriteOffStatus(int AssetID)
       {
           DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSET_CHECK_CHILD_ASSET_WRITEOFF_STATUS, DALCResultType.Scalar);

           criteria.AddInParameter(Parameters.PARAM_ASSETID, DbType.Int32, AssetID);

           int flag = (int)criteria.ExecuteCommand();

           if (flag == 0)
           {
               return false;
           }
           else
           {
               return true;
           }
       }

    }
