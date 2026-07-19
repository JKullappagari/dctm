using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    //V00001
    [DALCOperationSP(InsertSP = StoredProcedures.SP_MANAGE_USERS,UpdateSP=StoredProcedures.SP_MANAGE_USERSRESETPASSWORD, DeleteSP = StoredProcedures.SP_USER_DELETE)]
    public class ManageUsersBAL
    {

        #region ManageUsers
        //* BAL Name:  ManageUsersBAL.cs
        //* Author :  Murugan K
        //* Date	  :  2:25 PM 9/15/2006
        //* Use : To insert and update user details.
        //* Caller: ManageUsers.aspx
        #endregion

        #region Declarations
            private string strLoginName;            
            private string strFirstName;
            private string strLastName;
            private string strEmail;

            //HP: Added UserGuid private variable.
            //private string strUserGuid;

            //private int intSubContractorID;
            private int intLastModifiedBy;
            private int intStatus;
            private int intIsDeleted;
            private string strPassword;
            private string strBusinessUnitIDs;
            private string strDelimiters;        
            private int intCreatedBy;            
            private int intUserID;

            private DateTime dtexpiryDate; //CR3001:Password Expiration, by kjb on 09 Jan 2012
            private int bitIsUserSelectionAllowed;

            //private string strBadge;
            //private DateTime DtRFIDAssignDate;


            //For search
            private string strSearch;


            //Default Values
        private int intBusinessUnitID;
        private int intSiteID;
        //private int intDepartmentID;
        private int intLocationID;

        //V3.8-Added on 17Oct2013-By Amar Vidya
        private int intSiteRestriction;
        private string strSiteIDs;
        private string strErrorMessage;
        //*
        #endregion

       

        #region Properties for Display Only

        //Added by Rajesh for Retrieval and Display Purpose
        private string strDisplayName;

        public string DisplayName
        {
            get { return strDisplayName; }
            set { strDisplayName = value; }
        }
        //public string DisplayName
        //{
        //    get
        //    {
        //        string strDisplayName;
        //        if (LastName == "" && FirstName == "")
        //        {
        //            strDisplayName = FirstName + (LastName == "" || FirstName == "" ? "" : ", ") + LastName;
        //        }
        //        else
        //        {
        //            strDisplayName = LoginName;
        //        }
        //        return strDisplayName;
        //    }
        //}

        #endregion Properties for Display Only

        #region Properties

        //CR3001:Password Expiration, by kjb on 09 Jan 2012 -- Begin
        [DALCOperationParams(DataType = DbType.DateTime, ParameterName = Parameters.PARAM_EXPIRY_DATE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public DateTime ExpiryDate
        {
            get { return this.dtexpiryDate; }
            set { this.dtexpiryDate = value; }
        }
        //CR3001:Password Expiration, by kjb on 09 Jan 2012 -- End
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_LOGINNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string LoginName
        {
            get  {  return this.strLoginName;  }
            set  {  this.strLoginName = value; }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_FIRSTNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string FirstName
        {
            get  { return this.strFirstName;   }
            set  { this.strFirstName = value;   }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_LASTNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string LastName
        {
            get  {  return this.strLastName;   }
            set  { this.strLastName = value;   }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_EMAIL, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string Email
        {
            get  { return this.strEmail;  }
            set  { this.strEmail = value; }
        }

        //HP: Added UserGuid DALC Parameter with set and get accessors.


        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_STATUS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_STATUS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]        
        public int Status
        {
            get { return this.intStatus; }
            set { this.intStatus = value; }
        }
       // V00001
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ISDELETED, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public int IsDeleted
        {
            get { return this.intIsDeleted; }
            set { this.intIsDeleted = value; }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_PASSWORD, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_PASSWORD, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string Password
        {
            get { return this.strPassword;  }
            set { this.strPassword = value; }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_BUSINESSUNITIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string BusinessUnitIDs
        {
            get {  return this.strBusinessUnitIDs;  }
            set {  this.strBusinessUnitIDs = value;  }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_DELIMITERS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string Delimiters
        {
            get { return this.strDelimiters; }
            set { this.strDelimiters = value; }
        }
             
           
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_CREATEDBY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_CREATEDBY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int CreatedBy
        {
            get { return this.intCreatedBy; }
            set { this.intCreatedBy = value; }
        }
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MODIFIEDBY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public int LastModifiedBy
        {
            get
            {
                return this.intLastModifiedBy;
            }
            set
            {
                this.intLastModifiedBy = value;
            }
        }



        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_USERID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_USERID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_USERID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public int UserID
        {
            get {  return this.intUserID;  }
            set {  this.intUserID = value; }
        }


        [DALCOperationParams(DataType = DbType.Byte, ParameterName = Parameters.PARAM_ISUSERSELECTIONALLOWED, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int IsUserSelectionAllowed
        {
            get { return bitIsUserSelectionAllowed; }
            set { bitIsUserSelectionAllowed = value; }
        }


        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_BUSINESSUNITID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_BUSINESSUNITID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int BusinessUnitID
        {
            get { return this.intBusinessUnitID; }
            set { this.intBusinessUnitID = value; }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_SITEID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_SITEID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int SiteID
        {
            get { return this.intSiteID; }
            set { this.intSiteID = value; }
        }

        //V3.8-Added on 17Oct2013-By Amar Vidya
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_SITERESTRICTION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_SITERESTRICTION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_SITERESTRICTION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public int SiteRestriction
        {
            get { return this.intSiteRestriction; }
            set { this.intSiteRestriction = value; }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_SITEIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string SiteIDs
        {
            get { return this.strSiteIDs; }
            set { this.strSiteIDs = value; }
        }
        //[DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_OPERATIONs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        //public string Operations 
        //{
        //    get { return this.strOperations; }
        //    set { this.strOperations  = value; }
        //}
        //*

        //[DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_DEPARTMENTID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        //[DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_DEPARTMENTID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        //public int DepartmentID
        //{
        //    get { return this.intDepartmentID; }
        //    set { this.intDepartmentID = value; }
        //}

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_LOCATIONID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_LOCATIONID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int LocationID
        {
            get { return this.intLocationID; }
            set { this.intLocationID = value; }
        }

        //[DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_BADGE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        //public string CurrentRFIDBadge
        //{
        //    get { return this.strBadge; }
        //    set { this.strBadge = value; }
        //}

        //[DALCOperationParams(DataType = DbType.DateTime, ParameterName = Parameters.PARAM_RFID_ASSIGN_DATE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        //public DateTime RFIDAssignDate
        //{
        //    get { return this.DtRFIDAssignDate; }
        //    set { this.DtRFIDAssignDate = value; }
        //}

        public string Search
        {
            get { return this.strSearch; }
            set { this.strSearch = value; }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_MESSAGECODE, ParameterSize = 2000, ParameterDirection = ParameterDirection.Output, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_MESSAGECODE, ParameterSize = 2000, ParameterDirection = ParameterDirection.Output, DALCOperation = DALCOperation.Update)]
        public string MessageCode { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// To insert and update user details.
        /// </summary>
        /// <param name="operation"></param>
        public void Persist(DALCOperation operation)
        {
            DALCBase<ManageUsersBAL> dalc = new DALCBase<ManageUsersBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// To check the user already exists.
        /// </summary>
        /// <params>UserID</params>
        /// <params>LoginName</params>
        /// <returns>int</returns>
        public int exists()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_USER_DOESEXIST, DALCResultType.Scalar);
            criteria.AddInParameter(Parameters.PARAM_USERID, DbType.Int32, UserID);
            criteria.AddInParameter(Parameters.PARAM_LOGINNAME, DbType.String, LoginName);         
            int count = (int)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            //return() - Return No. of Records
            return (count);
        }

 
        /// <summary>
        /// To retrieve users list
        /// </summary>
        /// <params>Search</params>
        /// <returns>DataSet</returns>
        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_MANAGE_USERS_LIST, DALCResultType.DataSet);
            criteria.AddInParameter("@pVarSearch", DbType.String, Search);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// To retrieve User Details by userid
        /// </summary>
        /// <params>UserID</params>
        /// <params></params>
        /// <returns>UserType</returns>
        public DataSet retrieveUserDetails()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_USERS_DETAILSBYUSERID, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_USERID,DbType.Int32,UserID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }



        public void retrieveUserObject()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_USERS_DETAILSBYUSERID, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_USERID, DbType.Int32, UserID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;

            if (ds.Tables[0] != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    DataRow dr = ds.Tables[0].Rows[0];

                    this.FirstName = dr[DBFields.DBFIELD_FIRSTNAME].ToString();
                    this.LastName = dr[DBFields.DBFIELD_LASTNAME].ToString();
                    this.LoginName = dr[DBFields.DBFIELD_LOGINNAME].ToString();
                    this.Email = dr[DBFields.DBFIELD_EMAIL].ToString();
                    this.Status = (Convert.ToBoolean(dr[DBFields.DBFIELD_USERSTATUS].ToString()) ? 1 : 0);
                    //this.IsActive = Convert.ToBoolean(dr[DBFields.DBFIELD_USERSTATUS].ToString());
                    this.IsUserSelectionAllowed = (Convert.ToBoolean(dr[DBFields.DBFIELD_ISUSERSELECTIONALLOWED].ToString()) ? 1: 0);
                    this.BusinessUnitID = Convert.ToInt32(dr[DBFields.DBFIELD_DEFAULTBU].ToString());
                    this.SiteID = Convert.ToInt32(dr[DBFields.DBFIELD_DEFAULTSITE].ToString());
                    this.LocationID = Convert.ToInt32(dr[DBFields.DBFIELD_DEFAULTLOC].ToString());
                    //this.DepartmentID = Convert.ToInt32(dr[DBFields.DBFIELD_DEPARTMENTID].ToString());
                    this.DisplayName = dr[DBFields.DBFIELD_DISPLAYNAME].ToString();
                }
            }

            if (ds.Tables[1] != null)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    this.BusinessUnitIDs = "";
                    foreach (DataRow dr1 in ds.Tables[1].Rows)
                    {
                        this.BusinessUnitIDs += dr1[DBFields.DBFIELD_BUSINESSUNITID].ToString() + ",";
                    }
                }
            }

        }

        public void UpdateIsFirstLogin(int UserID)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_UPDATE_IS_FIRST_LOGIN, DALCResultType.Scalar);

            criteria.AddInParameter(Parameters.PARAM_USERID, DbType.Int32, UserID);

            criteria.ExecuteCommand();

        }

        #endregion

    }
}
