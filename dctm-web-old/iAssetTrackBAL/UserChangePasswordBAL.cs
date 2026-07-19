using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    [DALCOperationSP(UpdateSP = StoredProcedures.SP_USER_CHANGEPASSWORD)]
    public class UserChangePasswordBAL
    {

        int m_UserID;
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_USERID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; }
        }


        string m_OldPassword;
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_USER_OLDPASSWORD, ParameterSize=50, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string OldPassword
        {
            get { return m_OldPassword; }
            set { m_OldPassword = value; }
        }


        string m_NewPassword;
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_USER_NEWPASSWORD, ParameterSize=50, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string NewPassword
        {
            get { return m_NewPassword; }
            set { m_NewPassword = value; }
        }

        string m_ErrorType;
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_EXCEPTION_TYPE, ParameterSize=255, ParameterDirection = ParameterDirection.Output, DALCOperation = DALCOperation.Update)]
        public string ErrorType
        {
            get { return m_ErrorType; }
            set { m_ErrorType = value; }
        }

        //CR3001:Password Expiration, by kjb on 09 Jan 2012 -- Begin
        DateTime m_ExpiryDate;
        [DALCOperationParams(DataType = DbType.DateTime, ParameterName = Parameters.PARAM_EXPIRY_DATE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public DateTime ExpiryDate
        {
            get { return this.m_ExpiryDate; }
            set { this.m_ExpiryDate = value; }
        }
        //CR3001:Password Expiration, by kjb on 09 Jan 2012 -- End


        public void Persist(DALCOperation operation)
        {
            DALCBase<UserChangePasswordBAL> dalc = new DALCBase<UserChangePasswordBAL>(this);
            dalc.SaveData(operation, 1);
        }

    }
}
