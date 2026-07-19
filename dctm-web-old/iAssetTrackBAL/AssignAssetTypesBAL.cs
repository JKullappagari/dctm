/*
File Name   :	

Description :	

Date created:	08 Nov 2012

Modification History:
***********************
CR		Name			Date			Description

*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    [DALCOperationSP(InsertSP = StoredProcedures.SP_ASSIGN_ASSETTYPES)]
    public class AssignAssetTypesBAL
    {

        #region Declarations
        //private string strCostCenterIDs;
        private string strDelimiters;
        private string strGroupIDs;
        private int intCreatedBy;
        private int _intBusinessUnitID;
        #endregion

        #region Properties

        //[DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_USERID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        //public int UserID
        //{
        //    get { return this.intUserID; }
        //    set { this.intUserID = value; }
        //}
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_BUSINESSUNITID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
       
        public int BusinessUnitID
        {
            get 
            {
                return this._intBusinessUnitID;
            }
            set
            {
                this._intBusinessUnitID = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_DELIMITERS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string Delimiters
        {
            get { return this.strDelimiters; }
            set { this.strDelimiters = value; }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_GROUPIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string GroupIDs
        {
            get { return this.strGroupIDs; }
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
            DALCBase<AssignAssetTypesBAL> dalc = new DALCBase<AssignAssetTypesBAL>(this);
            dalc.SaveData(operation, 1);
        }

        #endregion

    }
}
