/*
File Name   :	ActiveDirectoryBAL.cs

Description :	Business Logic layer for Active Directory

Date created:	27 March 2006

Modification History:
***********************
CR		Name			Date			Description
New		Murugan K	27/03/2006		File has been created.
*/


using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    [DALCOperationSP(InsertSP = StoredProcedures.SP_USER_ASSIGN_RIGHTS)]
    public class AssignRightsBAL
    {

       #region AssignRights
        //* BAL Name:  AssignRightsBAL
        //* Author :  Murugan K
        //* Date	  :  10:27 AM 10/12/2006
        //* Use : To assign and remove user rights. 
        //* Caller: AssignRights.aspx
        #endregion

       #region Declarations  
            private int intUserID;
            //private string strCostCenterIDs;
            private string strDelimiters;
            private string strGroupIDs;                      
            private int intCreatedBy;          
        #endregion

       #region Properties 

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_USERID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int UserID
        {
            get { return this.intUserID; }
            set { this.intUserID = value; }
        }
        //[DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_COSTCENTERIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        //public string CostCenterIDs
        //{
        //get { return this.strCostCenterIDs; }
        //set { this.strCostCenterIDs = value; }
        //}
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_DELIMITERS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string Delimiters
        {
        get { return this.strDelimiters; }
        set { this.strDelimiters = value; }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_GROUPIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string GroupIDs
        {
        get { return this.strGroupIDs;  }
        set { this.strGroupIDs = value; }
        }
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_CREATEDBY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int CreatedBy
        {
        get { return this.intCreatedBy; }
        set { this.intCreatedBy = value; }
        }
       #endregion

       #region Methods
        /// <summary>
        /// To assign and remove user rights. 
        /// </summary>
        /// <param name="operation"></param>
        public void Persist(DALCOperation operation)
        {
            DALCBase<AssignRightsBAL> dalc = new DALCBase<AssignRightsBAL>(this);
            dalc.SaveData(operation, 1);
        }

        #endregion

    }
}
