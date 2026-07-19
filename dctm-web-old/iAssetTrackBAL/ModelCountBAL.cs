using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrackBAL
{
    // INSERTSP -- handles both split and combine operations,based on operation type. 0 = split,1= combine
    [DALCOperationSP(InsertSP = StoredProcedures.SP_MODELCOUNT_SPLIT_COMBINE,UpdateSP=StoredProcedures.SP_MODELCOUNT_UPDATE)]
  public class ModelCountBAL
    {
        #region Declarations
        private int intSPCID; 
        private int intSPCQuantity;
        private double dblSPCOldQuantity;
        private double dblSPCNewQuantity;
        private int intSPHeaderID;
        private int intSiteID;
           private string strSiteIDs;
        private int intProjectID;
        private string strUOM;
        private string strLimitParam;
        private string strModelCountIDs;
        private int intTechID;
        private int intModelCountID;
        private int intIndexNo;
        private int intCreatedBy;
        private int intOperartionType;
        private double dblRUPerCabLimit;
        private double dblKWPerCabLimit;
        private double dblPdf;
        private double dblGrowthRate;
        private int intPerCab;
        private int intTechSubGroupID;
        #endregion
        #region Properties


        

            [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_TECH_SUB_GROUP_ID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int TechSubGroupID
        {
            get
            {
                return this.intTechSubGroupID;
            }
            set
            {
                this.intTechSubGroupID = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_SPCID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_SPCID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int SPCID
        {
            get
            {
                return this.intSPCID;
            }
            set
            {
                this.intSPCID = value;
            }
        }

        public int SPCQuantity
        {
            get
            {
                return this.intSPCQuantity;
            }
            set
            {
                this.intSPCQuantity = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_SPC_OLD_QUANTITY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public double SPCOldQuantity
        {
            get
            {
                return this.dblSPCOldQuantity;
            }
            set
            {
                this.dblSPCOldQuantity = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_SPC_NEW_QUANTITY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public double SPCNewQuantity
        {
            get
            {
                return this.dblSPCNewQuantity;
            }
            set
            {
                this.dblSPCNewQuantity = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_SPCHEADERID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
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
        
        public int SiteID
        {
            get
            {
                return this.intSiteID;
            }
            set
            {
                this.intSiteID = value;
            }
        }

         [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_INDIVIDUALSITES, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_INDIVIDUALSITES, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string SiteIDs
        {
            get
            {
                return this.strSiteIDs;
            }
            set
            {
                this.strSiteIDs = value;
            }
        }

        public int ProjectID
        {
            get
            {
                return this.intProjectID;
            }
            set
            {
                this.intProjectID = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_UOM, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_UOM, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string UOM
        {
            get
            {
                return this.strUOM;
            }
            set
            {
                this.strUOM = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_LIMITPARAM, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string LimitParam
        {
            get
            {
                return this.strLimitParam;
            }
            set
            {
                this.strLimitParam = value;
            }
        }


        public int TECHID
        {
            get
            {
                return this.intIndexNo;
            }
            set
            {
                this.intIndexNo = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_INDEX_NO, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_INDEX_NO, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int IndexNo
        {
            get
            {
                return this.intTechID;
            }
            set
            {
                this.intTechID = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MODEL_COUNT_ID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MODEL_COUNT_ID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int ModelCountID
        {
            get
            {
                return this.intModelCountID;
            }
            set
            {
                this.intModelCountID = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_MODEL_COUNT_IDS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string ModelCountIDs
        {
            get
            {
                return this.strModelCountIDs;
            }
            set
            {
                this.strModelCountIDs = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_OPERATION_TYPE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int OperationType
        {
            get
            {
                return this.intOperartionType;
            }
            set
            {
                this.intOperartionType = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_CREATEDBY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
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

        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_RU_PER_CAB_LIMIT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public double RUPerCabLimit
        {
            get
            {
                return this.dblRUPerCabLimit;
            }
            set
            {
                this.dblRUPerCabLimit = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_KW_PER_CAB_LIMIT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public double KWPerCabLimit
        {
            get
            {
                return this.dblKWPerCabLimit;
            }
            set
            {
                this.dblKWPerCabLimit = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_PDF, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public double PDF
        {
            get
            {
                return this.dblPdf;
            }
            set
            {
                this.dblPdf = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_GROWTH_RATE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public double GrowthRate
        {
            get
            {
                return this.dblGrowthRate;
            }
            set
            {
                this.dblGrowthRate = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_PER_CAB, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int PerCab
        {
            get
            {
                return this.intPerCab;
            }
            set
            {
                this.intPerCab = value;
            }
        }

        #endregion

        # region Methods

        public void Persist(DALCOperation operation)
        {
            DALCBase<ModelCountBAL> dalc = new DALCBase<ModelCountBAL>(this);
            dalc.SaveData(operation, 1);
        }

        public DataSet retrieveModelData(int TechMainGroupID,int TechSubGroupID,int IndexNo,string UOM,int SPCHeaderID,string SiteIDs)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_SPC_Model_LIST_FOR_FACILITY_DETAILS, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_SPCHEADERID, DbType.Int32, SPCHeaderID);
            criteria.AddInParameter(Parameters.PARAM_TECH_MAIN_GROUP_ID, DbType.Int32, TechMainGroupID);
            criteria.AddInParameter(Parameters.PARAM_TECH_SUB_GROUP_ID, DbType.Int32, TechSubGroupID);
            criteria.AddInParameter(Parameters.PARAM_INDEX_NO, DbType.Int32, IndexNo);
            criteria.AddInParameter(Parameters.PARAM_UOM, DbType.String, UOM);
            criteria.AddInParameter(Parameters.PARAM_SITEIDs, DbType.String, SiteIDs);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        public DataSet retrieveModelCountListBySPCHeader(int SPCHeaderID)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_MODELCOUNT_LIST_BY_SPC_HEADER, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_SPCHEADERID, DbType.Int32, SPCHeaderID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        //

        # endregion
    }
}
