using System.Collections.Generic;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    [DALCOperationSP()]
    public class MountTypeBAL
    {
        private int _intMountTypeID;
        private string _strMountType;
        private int _intCreatedBy;
        private int _intLastModifiedBy;

        public string AirFlowDirection
        {
            get
            {
                return this._strMountType;
            }
            set
            {
                this._strMountType = value;
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

        public int MountTypeID
        {
            get
            {
                return this._intMountTypeID;
            }
            set
            {
                this._intMountTypeID = value;
            }
        }

        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            var dalc = new DALCBase<MountTypeBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieve()
        {
            var criteria = new DALCCommandHelper(StoredProcedures.SP_MOUNTTYPE_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_MOUNTTYPE_ID, DbType.Int32, MountTypeID);
            var ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
    }
}
