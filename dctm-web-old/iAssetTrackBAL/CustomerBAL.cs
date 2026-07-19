using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    [DALCOperationSP(InsertSP = StoredProcedures.SP_CUSTOMER_UPDATE, DeleteSP = StoredProcedures.SP_CUSTOMER_DELETE)]
    public class CustomerBAL
    {
        private int intCustomerID;
        private string strName;
        private string strShortName;

        private string strPrimaryAddress1;
        private string strPrimaryAddress2;
        private string strPrimaryAddress3;
        private string strPrimaryAddress4;
        private string strPrimaryPostalCode;
        private string strPrimaryContactPerson;
        private string strPrimaryPhoneNumber;
        private string strPrimaryMobileNumber;
        private string strPrimaryFaxNumber;
        private string strPrimaryEmail;

        private string strAlternateAddress1;
        private string strAlternateAddress2;
        private string strAlternateAddress3;
        private string strAlternateAddress4;
        private string strAlternatePostalCode;
        private string strAlternateContactPerson;
        private string strAlternatePhoneNumber;
        private string strAlternateMobileNumber;
        private string strAlternateFaxNumber;
        private string strAlternateEmail;

        private int intStatus;
        private int intCreatedBy;
        private int intLastModifiedBy;
        private string strCustomerIDs;
        private int intCityID;
        private int intIndustryID;
        private int intAddressID;
        //private int stateID;
        //[DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_BUSINESSUNITID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        //public int BusinessUnitID
        //{
        //    get
        //    {
        //        return this.intBusinessUnitID;
        //    }
        //    set
        //    {
        //        this.intBusinessUnitID = value;
        //    }
        //}
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_CITYID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int CityID
        {
            get
            {
                return this.intCityID;
            }
            set
            {
                this.intCityID = value;
            }
        }
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_INDUSTRYID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int IndustryID
        {
            get
            {
                return this.intIndustryID;
            }
            set
            {
                this.intIndustryID = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_NAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string Name
        {
            get
            {
                return this.strName;
            }
            set
            {
                this.strName = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_SHORTNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string ShortName
        {
            get
            {
                return this.strShortName;
            }
            set
            {
                this.strShortName = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_PRIMARYADDRESS1, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string PrimaryAddress1
        {
            get
            {
                return this.strPrimaryAddress1;
            }
            set
            {
                this.strPrimaryAddress1 = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ALTERNATEADDRESS1, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string AlternateAddress1
        {
            get
            {
                return this.strAlternateAddress1;
            }
            set
            {
                this.strAlternateAddress1 = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_PRIMARYADDRESS2, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string PrimaryAddress2
        {
            get
            {
                return this.strPrimaryAddress2;
            }
            set
            {
                this.strPrimaryAddress2 = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ALTERNATEADDRESS2, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string AlternateAddress2
        {
            get
            {
                return this.strAlternateAddress2;
            }
            set
            {
                this.strAlternateAddress2 = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_PRIMARYADDRESS3, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string PrimaryAddress3
        {
            get
            {
                return this.strPrimaryAddress3;
            }
            set
            {
                this.strPrimaryAddress3 = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ALTERNATEADDRESS3, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string AlternateAddress3
        {
            get
            {
                return this.strAlternateAddress3;
            }
            set
            {
                this.strAlternateAddress3 = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_PRIMARYADDRESS4, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string PrimaryAddress4
        {
            get
            {
                return this.strPrimaryAddress4;
            }
            set
            {
                this.strPrimaryAddress4 = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ALTERNATEADDRESS4, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string AlternateAddress4
        {
            get
            {
                return this.strAlternateAddress4;
            }
            set
            {
                this.strAlternateAddress4 = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_PRIMARYPOSTALCODE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string PrimaryPostalCode
        {
            get
            {
                return this.strPrimaryPostalCode;
            }
            set
            {
                this.strPrimaryPostalCode = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ALTERNATEPOSTALCODE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string AlternatePostalCode
        {
            get
            {
                return this.strAlternatePostalCode;
            }
            set
            {
                this.strAlternatePostalCode = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_PRIMARYCONTACTPERSON, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string PrimaryContactPerson
        {
            get
            {
                return this.strPrimaryContactPerson;
            }
            set
            {
                this.strPrimaryContactPerson = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ALTERNATECONTACTPERSON, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string AlternateContactPerson
        {
            get
            {
                return this.strAlternateContactPerson;
            }
            set
            {
                this.strAlternateContactPerson = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_PRIMARYPHONENUMBER, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string PrimaryPhoneNumber
        {
            get
            {
                return this.strPrimaryPhoneNumber;
            }
            set
            {
                this.strPrimaryPhoneNumber = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ALTERNATEPHONENUMBER, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string AlternatePhoneNumber
        {
            get
            {
                return this.strAlternatePhoneNumber;
            }
            set
            {
                this.strAlternatePhoneNumber = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_PRIMARYMOBILENUMBER, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string PrimaryMobileNumber
        {
            get
            {
                return this.strPrimaryMobileNumber;
            }
            set
            {
                this.strPrimaryMobileNumber = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ALTERNATEMOBILENUMBER, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string AlternateMobileNumber
        {
            get
            {
                return this.strAlternateMobileNumber;
            }
            set
            {
                this.strAlternateMobileNumber = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_PRIMARYFAXNUMBER, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string PrimaryFaxNumber
        {
            get
            {
                return this.strPrimaryFaxNumber;
            }
            set
            {
                this.strPrimaryFaxNumber = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ALTERNATEFAXNUMBER, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string AlternateFaxNumber
        {
            get
            {
                return this.strAlternateFaxNumber;
            }
            set
            {
                this.strAlternateFaxNumber = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_PRIMARYEMAIL, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string PrimaryEmail
        {
            get
            {
                return this.strPrimaryEmail;
            }
            set
            {
                this.strPrimaryEmail = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ALTERNATEEMAIL, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string AlternateEmail
        {
            get
            {
                return this.strAlternateEmail;
            }
            set
            {
                this.strAlternateEmail = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_CUSTOMERIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public string CustomerIDs
        {

            get
            {
                return this.strCustomerIDs;
            }
            set
            {
                this.strCustomerIDs = value;
            }
        }
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_STATUS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        //[DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_STATUS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public int Status
        {
            get
            {
                return this.intStatus;
            }
            set
            {
                this.intStatus = value;
            }
        }
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_CREATEDBY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int CreatedBy
        {
            get
            {
                return this.intCreatedBy;
            }
            set
            {
                this.intCreatedBy = value;
            }
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
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_CUSTOMERID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
        public int CustomerID
        {
            get
            {
                return this.intCustomerID;
            }
            set
            {
                this.intCustomerID = value;
            }
        }
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ADDRESSID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int AddressID
        {
            get
            {
                return this.intAddressID;
            }
            set
            {
                this.intAddressID = value;
            }
        }
        //[DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_COST_COUNTRY_ID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        //public int CountryID
        //{
        //    get
        //    {
        //        return this.countryID;
        //    }
        //    set
        //    {
        //        this.countryID = value;
        //    }
        //}
     
        public void Persist(DALCOperation operation)
        {
            DALCBase<CustomerBAL> dalc = new DALCBase<CustomerBAL>(this);
            dalc.SaveData(operation, 1);
        }
        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_CUSTOMER_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_CUSTOMERID, DbType.Int32, CustomerID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
        //V00008 by Vidya on 9-Oct-2012
        public DataSet retrieveAddress()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ADDRESS_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_CUSTOMERID, DbType.Int32, CustomerID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
    
        public DataSet retrieveIndustryType()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_INDUSTRYTYPE_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_INDUSTRYID, DbType.Int32, IndustryID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
        public DataSet retrieveAddressType()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ADDRESSTYPE_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_ADDRESSID, DbType.Int32, AddressID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
        //public DataSet retrieveCountry()
        //{
        //    DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_COST_COUNTRY_LIST, DALCResultType.DataSet);
        //    criteria.AddInParameter(Parameters.PARAM_COST_COUNTRY_ID, DbType.Int32, CountryID);
        //    DataSet ds = (DataSet)criteria.ExecuteCommand();
        //    Dictionary<string, object> output = criteria.OutputParameters;
        //    return (ds);
        //}
       
        //public DataSet retrieveStateByCountyID()
        //{
        //    DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_COST_STATE_LIST_BY_COUNTRY, DALCResultType.DataSet);
        //    criteria.AddInParameter(Parameters.PARAM_COST_COUNTRY_ID, DbType.Int32, CountryID);
        //    DataSet ds = (DataSet)criteria.ExecuteCommand();
        //    Dictionary<string, object> output = criteria.OutputParameters;
        //    return (ds);
        //}
        //public DataSet retrieveCityByCountyID()
        //{
        //    DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_COST_CITY_LIST_BY_COUNTRY, DALCResultType.DataSet);
        //    criteria.AddInParameter(Parameters.PARAM_COST_COUNTRY_ID, DbType.Int32, CountryID);
        //    DataSet ds = (DataSet)criteria.ExecuteCommand();
        //    Dictionary<string, object> output = criteria.OutputParameters;
        //    return (ds);
        //}
        public DataSet SearchCustomer()
        {
          
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_CUSTOMER_SEARCH, DALCResultType.DataSet);
           
            criteria.AddInParameter("@pVchName", DbType.String, strName);
            criteria.AddInParameter(Parameters.PARAM_INDUSTRYID, DbType.Int32, IndustryID);
            criteria.AddInParameter(Parameters.PARAM_ADDRESSID, DbType.Int32, AddressID);

            DataSet ds = (DataSet)criteria.ExecuteCommand();
            return ds;
        }
        public DataSet CustomerDetailsSearch()
        {

            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_CUSTOMERDETAILS_SEARCH, DALCResultType.DataSet);

            criteria.AddInParameter("@pVchName", DbType.String, strName);
            criteria.AddInParameter("@pVchShortName", DbType.String, strShortName);
            criteria.AddInParameter(Parameters.PARAM_INDUSTRYID, DbType.Int32, IndustryID);
            criteria.AddInParameter(Parameters.PARAM_CITYID, DbType.Int32, CityID);
            criteria.AddInParameter(Parameters.PARAM_ADDRESSID, DbType.Int32, AddressID);

            DataSet ds = (DataSet)criteria.ExecuteCommand();
            return ds;
        }
        public int exists()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_CUSTOMER_DOESEXIST, DALCResultType.Scalar);

            criteria.AddInParameter(Parameters.PARAM_CUSTOMERID, DbType.Int32, CustomerID);
            criteria.AddInParameter(Parameters.PARAM_SHORTNAME, DbType.String, ShortName);
            //criteria.AddInParameter(Parameters.PARAM_NAME, DbType.String, Name);

            //criteria.AddOutParameter("@count", DbType.Int32,0,5);

            int count = (int)criteria.ExecuteCommand();

            Dictionary<string, object> output = criteria.OutputParameters;
            //return() - Return No. of Records
            return (count);

        }
    }
}
