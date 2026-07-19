using System.Collections.Generic;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    [DALCOperationSP()]
    public class OutputConnectorTypeBAL
    {
        private int _intOutputConnectorTypeID;
        private string _strOutputConnectorType;
        private int _intCreatedBy;
        private int _intLastModifiedBy;

        public string Orientation
        {
            get
            {
                return this._strOutputConnectorType;
            }
            set
            {
                this._strOutputConnectorType = value;
            }
        }

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

        public int LastModifiedBy
        {
            get
            {
                return this._intLastModifiedBy;
            }
            set
            {
                this._intLastModifiedBy = value;
            }
        }

        public int OutputConnectorTypeID
        {
            get
            {
                return this._intOutputConnectorTypeID;
            }
            set
            {
                this._intOutputConnectorTypeID = value;
            }
        }

        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            var dalc = new DALCBase<OutputConnectorTypeBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieve()
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_OUTPUT_CONNECTOR_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_OUTPUT_CONNECTOR_TYPE_ID, DbType.Int32, _intOutputConnectorTypeID);
            var ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
    }
}
