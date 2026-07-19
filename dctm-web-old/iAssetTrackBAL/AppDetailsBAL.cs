using System.Collections.Generic;
using System.Data;
using iAssetTrack.DALC;

namespace iAssetTrackBAL
{
    [DALCOperationSP(UpdateSP = StoredProcedures.SP_APP_DETAILS_UPDATE)]
    public class AppDetailsBAL
    {

        private string _strApplication;
        private string _strAppCriticality;
        private string _strAppType;
        private string _strAppDivision;
        private string _strVendor;
        private int _intCreatedBy;
        private int _intBusinessUnitID;

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_APPLICATIONNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string Application
        {
            get
            {
                return this._strApplication;
            }
            set
            {
                this._strApplication = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_APPL_CRITICALITY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string AppCriticality
        {
            get
            {
                return this._strAppCriticality;
            }
            set
            {
                this._strAppCriticality = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_APPLTYPE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string AppType
        {
            get
            {
                return this._strAppType;
            }
            set
            {
                this._strAppType = value;
            }
        }
       

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_APPLICATION_DIVISION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string AppDivision
        {
            get
            {
                return this._strAppDivision;
            }
            set
            {
                this._strAppDivision = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_MFGNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string Vendor
        {
            get
            {
                return this._strVendor;
            }
            set
            {
                this._strVendor = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_CREATEDBY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int CreatedBy
        {
            get
            {
                return this._intCreatedBy;
            }
            set
            {
                this._intCreatedBy = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_BUSINESSUNITID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
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



        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            var dalc = new DALCBase<AppDetailsBAL>(this);
            dalc.SaveData(operation, 1);
        }

    }
}
