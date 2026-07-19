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
    [DALCOperationSP(InsertSP = StoredProcedures.SP_AUDITLOGINLOGOUT)]
    public class AuditLoginLogoutBAL
    {
        #region AuditLoginLogout
        //* BAL Name:  AuditLoginLogoutBAL
        //* Author :  Murugan K
        //* Date	  :  10:27 AM 10/12/2006
        //* Use : To insert and update audit table.
        //* Caller: Login/Logout.aspx
        #endregion

        #region Declarations

        private int intUserID;
        private int intAuditType;
        private string strIP;
        private string strSessionID;

        #endregion

        #region Properties

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_USERID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]        
        public int UserID
        {
            get { return this.intUserID; }
            set { this.intUserID = value; }
        }
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = "@pIntAuditType", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int AuditType
        {
            get { return this.intAuditType; }
            set { this.intAuditType = value; }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = "@pVarIPAddress", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string IP
        {
            get { return this.strIP; }
            set { this.strIP = value; }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = "@pVarSessionID", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string SessionID
        {
            get { return this.strSessionID; }
            set { this.strSessionID = value; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// To insert and update audit table.
        /// </summary>
        /// <param name="operation"></param>
        public void Persist(DALCOperation operation)
        {
            DALCBase<AuditLoginLogoutBAL> dalc = new DALCBase<AuditLoginLogoutBAL>(this);
            dalc.SaveData(operation, 1);
        }
        #endregion

    }
}
