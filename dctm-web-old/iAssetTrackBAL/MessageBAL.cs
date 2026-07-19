using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    public class MessageBAL
    {
        #region Message
        //* BAL Name:  MessageBAL
        //* Author :  Murugan K
        //* Date	  :  10:27 AM 10/12/2006
        //* Use : To retrive message.
        //* Caller: 
        #endregion

        #region Delcarations
        private int intMessageId;
        private string strMessageCode;
        private string strMessage;
        #endregion

        #region Properties
        [DALCOperationParams(DataType = DbType.String, ParameterName = "@pVarCode", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string MessageCode
        {
            get { return this.strMessageCode;  }
            set { this.strMessageCode = value; }
        }


        [DALCOperationParams(DataType = DbType.String, ParameterName = "@pVarMessage", ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string Message
        {
            get { return this.strMessage; }
            set { this.strMessage = value;}
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = "@pIntMessageId", ParameterDirection = ParameterDirection.Output, DALCOperation = DALCOperation.Update)]
        public int MessageId
        {
            get { return this.intMessageId;  }
            set { this.intMessageId = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Insert update message to database.
        /// </summary>
        /// <param name="operation"></param>
        public void Persist(DALCOperation operation)
        {
            DALCBase<MessageBAL> dalc = new DALCBase<MessageBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieve message from database
        /// </summary>
        /// <params>MessageCode</params>
        /// <returns>string</returns>
        public String retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_MESSAGE_DISPLAY, DALCResultType.Scalar);
            criteria.AddInParameter(Parameters.PARAM_MESSAGECODE, DbType.String, MessageCode);
            String strCode = (String)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (strCode);
        }
        #endregion
    }

    }

