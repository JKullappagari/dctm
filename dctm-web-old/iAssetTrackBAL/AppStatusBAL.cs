using System.Collections.Generic;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    [DALCOperationSP()]
    public class AppStatusBAL
    {
        private int _intAppStatusID;
        private string _strAppStatusName;
        private int _intCreatedBy;
        private int _intLastModifiedBy;

        public string AppStatus
        {
            get
            {
                return this._strAppStatusName;
            }
            set
            {
                this._strAppStatusName = value;
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

        public int AppStatusID
        {
            get
            {
                return this._intAppStatusID;
            }
            set
            {
                this._intAppStatusID = value;
            }
        }

        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            var dalc = new DALCBase<AppStatusBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieve()
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_AppStatus_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_APPSTATUS_ID, DbType.Int32, AppStatusID);
            var ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
    }
}
