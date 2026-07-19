/*
File Name   :	InterfaceErrorBAL.cs

Description :	Business Logic layer for JDE Error Module.

Date created:	03 April 2007

Modification History:
***********************
CR		Name			Date			Description
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using iAssetTrack.DALC;

namespace iAssetTrack.BAL
{

    
    
    public partial class ExternalSysIntegrationLogBAL 
    {

        private int _ID;

        private string _FileName;

        private int _NoOfRecords;

        private System.DateTime _DateOfUpdate;

        private string _TransactionType;

        private string _Status;

        private string _ExternalSystem;


        //public DataSet retrieveExternalSysIntegrationLogData()
        //{
        //    DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_GETEXTERNALSYSLOG, DALCResultType.DataSet);
                       
        //    return (DataSet)criteria.ExecuteCommand();
        //}

        
        public int ID
        {
            get
            {
                return this._ID;
            }
            set
            {
                if ((this._ID != value))
                {
                    
                    this._ID = value;
                   
                }
            }
        }

        public string FileName
        {
            get
            {
                return this._FileName;
            }
            set
            {
                if ((this._FileName != value))
                {
                    
                    this._FileName = value;
                   
                }
            }
        }

        public int NoOfRecords
        {
            get
            {
                return this._NoOfRecords;
            }
            set
            {
                if ((this._NoOfRecords != value))
                {
                    this._NoOfRecords = value;
                }
            }
        }

        public System.DateTime DateOfUpdate
        {
            get
            {
                return this._DateOfUpdate;
            }
            set
            {
                if ((this._DateOfUpdate != value))
                {
                    this._DateOfUpdate = value;
                }
            }
        }

        public string TransactionType
        {
            get
            {
                return this._TransactionType;
            }
            set
            {
                if ((this._TransactionType != value))
                {
                    this._TransactionType = value;
                }
            }
        }

        public string Status
        {
            get
            {
                return this._Status;
            }
            set
            {
                if ((this._Status != value))
                {
                    this._Status = value;
                }
            }
        }

        public string ExternalSystem
        {
            get
            {
                return this._ExternalSystem;
            }
            set
            {
                if ((this._ExternalSystem != value))
                {
                    this._ExternalSystem = value;
                }
            }
        }

       
    }


}
