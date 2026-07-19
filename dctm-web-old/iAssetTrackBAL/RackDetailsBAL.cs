
using System.Collections.Generic;
using System.Data;
using iAssetTrack.DALC;

namespace iAssetTrackBAL
{
    [DALCOperationSP(UpdateSP = StoredProcedures.SP_RACK_DETAILS_UPDATE)]
    public class RackDetailsBAL
    {

        private string _strSite;
        private string _strRoom;
        private string _strRow;
        private string _strRack;
        private string _strManufacturer;
        private string _strModel;
        private string _strExternalID;
        private int _intCreatedBy;

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_SITE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string Site
        {
            get
            {
                return this._strSite;
            }
            set
            {
                this._strSite = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ROOM, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string Room
        {
            get
            {
                return this._strRoom;
            }
            set
            {
                this._strRoom = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ROW, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string Row
        {
            get
            {
                return this._strRow;
            }
            set
            {
                this._strRow = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_RACK, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string RackID
        {
            get
            {
                return this._strRack;
            }
            set
            {
                this._strRack = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_MFGNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string Manufacturer
        {
            get
            {
                return this._strManufacturer;
            }
            set
            {
                this._strManufacturer = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_MODELNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string Model
        {
            get
            {
                return this._strModel;
            }
            set
            {
                this._strModel = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_RACK_EXTERNAL_ID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string ExternalID
        {
            get
            {
                return this._strExternalID;
            }
            set
            {
                this._strExternalID = value;
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


        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            var dalc = new DALCBase<RackDetailsBAL>(this);
            dalc.SaveData(operation, 1);
        }

    }
}
