using System.Collections.Generic;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    [DALCOperationSP()]
    public class InputConnectorTypeBAL
    {
        private int _intInputConnectorTypeID;
        private string _strInputConnectorType;
        private int _intCreatedBy;
        private int _intLastModifiedBy;

        public string InputConnectorType
        {
            get
            {
                return this._strInputConnectorType;
            }
            set
            {
                this._strInputConnectorType = value;
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

        public int InputConnectorTypeID
        {
            get
            {
                return this._intInputConnectorTypeID;
            }
            set
            {
                this._intInputConnectorTypeID = value;
            }
        }

        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            var dalc = new DALCBase<InputConnectorTypeBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieve()
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_INPUT_CONNECTOR_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_INPUT_CONNECTOR_TYPE_ID, DbType.Int32, InputConnectorTypeID);
            var ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
    }
}
