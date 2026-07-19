using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrackBAL
{
      [DALCOperationSP(InsertSP = StoredProcedures.SP_CUSTOMGROUPSDEFAULT)]
  public class CustomGroupsDefaultBAL
    {
        private int intCreatedBy;
        private int intSPHeaderID;
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_USERID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
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
          [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_SPCHEADERID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int SPCHEADERID
        {
            get
            {
                return this.intSPHeaderID;
            }
            set
            {
                this.intSPHeaderID = value;
            }
        }
          public void Persist(DALCOperation operation)
          {
              DALCBase<CustomGroupsDefaultBAL> dalc = new DALCBase<CustomGroupsDefaultBAL>(this);
              dalc.SaveData(operation, 1);
          }

    }
}
