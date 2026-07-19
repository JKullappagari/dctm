using System.Collections.Generic;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    [DALCOperationSP()]
    public class AirFlowDirectionBAL
    {
        private int _intID;
        private string _strAirFlowDirection;
        private int _intCreatedBy;
        private int _intLastModifiedBy;
       
        public string AirFlowDirection
        {
            get
            {
                return this._strAirFlowDirection;
            }
            set
            {
                this._strAirFlowDirection = value;
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

        public int ID
        {
            get
            {
                return this._intID;
            }
            set
            {
                this._intID = value;
            }
        }

        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            var dalc = new DALCBase<AirFlowDirectionBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieve()
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_AIRFLOWDIRECTION_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_AIRFLOWDIRECTION_ID, DbType.Int32, ID);
            var ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
    }
}
