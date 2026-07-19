using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    [DALCOperationSP(InsertSP= StoredProcedures.SP_RESET_USER, UpdateSP = StoredProcedures.SP_USER_LOGIN_ATTEMPT_UPDATE,DeleteSP = StoredProcedures.SP_LOCK_USER)]
    public class UserLoginBAL
    {

        string m_LoginName;
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_LOGINNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_LOGINNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_LOGINNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string LoginName
        {
            get { return m_LoginName; }
            set { m_LoginName = value; }
        }

        int m_noOfAttempts;
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_LOGIN_ATTEMPTS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int NoOfAttempts
        {
            get { return m_noOfAttempts; }
            set { m_noOfAttempts = value; }
        }

        string m_Password;
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_PASSWORD, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string Password
        {
            get { return m_Password; }
            set { m_Password = value; }
        }

        //CR3001:Password Expiration, by kjb on 09 Jan 2012 -- Begin
        DateTime m_ExpiryDate;
        [DALCOperationParams(DataType = DbType.DateTime, ParameterName = Parameters.PARAM_EXPIRY_DATE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public DateTime ExpiryDate
        {
            get { return this.m_ExpiryDate; }
            set { this.m_ExpiryDate = value; }
        }
        //CR3001:Password Expiration, by kjb on 09 Jan 2012 -- End

        int m_CreatedBy;
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_CREATEDBY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int CreatedBy
        {
            get { return m_CreatedBy; }
            set { m_CreatedBy = value; }
        }

        string m_ExceptionType;
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_PASSWORD_EXCEPTION_TYPE, ParameterSize = 255, ParameterDirection = ParameterDirection.Output, DALCOperation = DALCOperation.Insert)]
        public string ExceptionType
        {
            get { return m_ExceptionType; }
            set { m_ExceptionType = value; }
        }



        public void Persist(DALCOperation operation)
        {
            DALCBase<UserLoginBAL> dalc = new DALCBase<UserLoginBAL>(this);
            dalc.SaveData(operation, 1);
        }

       

    }
}
