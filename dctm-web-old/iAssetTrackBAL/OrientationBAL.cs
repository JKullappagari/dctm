using System.Collections.Generic;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    [DALCOperationSP()]
    public class OrientationBAL
    {
        private int _intOrientationID;
        private string _strOrientationName;
        private int _intCreatedBy;
        private int _intLastModifiedBy;

        public string Orientation
        {
            get
            {
                return this._strOrientationName;
            }
            set
            {
                this._strOrientationName = value;
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

        public int OrientationID
        {
            get
            {
                return this._intOrientationID;
            }
            set
            {
                this._intOrientationID = value;
            }
        }

        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            var dalc = new DALCBase<OrientationBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieve()
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_ORIENTATION_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_ORIENTATION_ID, DbType.Int32, OrientationID);
            var ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
    }
}
