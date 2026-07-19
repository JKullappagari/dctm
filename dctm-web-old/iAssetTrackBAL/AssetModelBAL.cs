/*
File Name   :	AssetModelBAL.cs

Description :	Business Logic layer for Asset Model setup

Date created:	23 June 2011

Modification History:
***********************
CR		Name			    Date			Description
New		Jaagdeesh Babu K	23 June 2011	File has been created.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    [DALCOperationSP(InsertSP = StoredProcedures.SP_ASSETMODEL_UPDATE, DeleteSP = StoredProcedures.SP_ASSETMODEL_DELETE)]
    public class AssetModelBAL
    {
        private int intMfgID;
        private int intModelID;
        private string strModelName;
        private string strDescription;
        private int intStatus;
        private int intCreatedBy;
        private int intLastModifiedBy;
        private string strModelIDs;
        private int intSpcID;
        private int intTechID;
        private string strComment;
        private int buid;
        private Boolean isOverwrite;
        private bool isBlade;
        private bool isEnclosure;
        
        private int intModelTypeID;
        private float fltWidth;
        private float fltDepth;
        private float fltHeight;
        private float fltWeight;
        private int intUHeight;
        private float fltMaxPower;
        private float fltSteadyStatePower;
        private string strConnPDUSide;
        private string strConnDevSide;
        private int intTotalPSUCount;
        private int intReqPSUCount;
        private int intMountTypeID;
        private int intAFDirectionID;
        private float fltRInternalDepth;
        private float fltRInternalHeight;
        private int intEnclFrontRowCount;
        private int intEnclFrontColCount;
        private int intEnclRearRowCount;
        private int intEnclRearColCount;
        private int intBladeRowCount;
        private int intBladeColCount;
        private float fltRInternalWidth;



        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_MODELNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string ModelName
        {
            get
            {
                return this.strModelName;
            }
            set
            {
                this.strModelName = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_MODELDESCRIPTION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string Description
        {
            get
            {
                return this.strDescription;
            }
            set
            {
                this.strDescription = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_MODELIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public string ModelIDs
        {
            get
            {
                return this.strModelIDs;
            }
            set
            {
                this.strModelIDs = value;
            }
        }
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_STATUS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_STATUS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public int Status
        {
            get
            {
                return this.intStatus;
            }
            set
            {
                this.intStatus = value;
            }
        }

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

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MODIFIEDBY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public int LastModifiedBy
        {
            get
            {
                return this.intLastModifiedBy;
            }
            set
            {
                this.intLastModifiedBy = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MODELID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
        public int ModelID
        {
            get
            {
                return this.intModelID;
            }
            set
            {
                this.intModelID = value;
            }
        }

        //[DALCOperationParams(DataType = DbType.Boolean, ParameterName = Parameters.PARAM_IS_OVERWRITE, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
        public Boolean IsOverwrite
        {
            get
            {
                return this.isOverwrite;
            }
            set
            {
                this.isOverwrite = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MODELMFGID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int MfgID
        {
            get
            {
                return this.intMfgID;
            }
            set
            {
                this.intMfgID = value;
            }
        }


        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_SPCID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int SPCID
        {
            get
            {
                return this.intSpcID;
            }
            set
            {
                this.intSpcID = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_TECHID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int TechID
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

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_COMMENT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string Comment
        {
            get
            {
                return this.strComment;
            }
            set
            {
                this.strComment = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_AM_BUID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int BUID
        {
            get
            {
                return this.buid;
            }
            set
            {
                this.buid = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Boolean, ParameterName = Parameters.PARAM_IS_BLADE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public bool IsBlade
        {
            get
            {
                return this.isBlade;
            }

            set
            {
                this.isBlade = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Boolean, ParameterName = Parameters.PARAM_IS_ENCLOSURE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public bool IsEnclosure
        {
            get
            {
                return this.isEnclosure;
            }

            set
            {
                this.isEnclosure = value;
            }
        }


        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MODEL_TYPE_ID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int ModelTypeID
        {
            get
            {
                return this.intModelTypeID;
            }
            set
            {
                this.intModelTypeID = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_MODEL_WIDTH, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public float Width
        {
            get
            {
                return this.fltWidth;
            }
            set
            {
                this.fltWidth = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_MODEL_DEPTH, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public float Depth
        {
            get
            {
                return this.fltDepth;
            }
            set
            {
                this.fltDepth = value;
            }
        }
        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_MODEL_HEIGHT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public float Height
        {
            get
            {
                return this.fltHeight;
            }
            set
            {
                this.fltHeight = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_MODEL_WEIGHT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public float Weight
        {
            get
            {
                return this.fltWeight;
            }
            set
            {
                this.fltWeight = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MODEL_UHEIGHT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int UHeight
        {
            get
            {
                return this.intUHeight;
            }
            set
            {
                this.intUHeight = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_MODEL_MAX_POWER, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public float MaxPower
        {
            get
            {
                return this.fltMaxPower;
            }
            set
            {
                this.fltMaxPower = value;
            }
        }
        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_MODEL_SS_POWER, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public float SteadyStatePower
        {
            get
            {
                return this.fltSteadyStatePower;
            }
            set
            {
                this.fltSteadyStatePower = value;
            }
        }
        
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_MODEL_CONN_PDU_SIDE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string ConnectorPDUSide
        {
            get
            {
                return this.strConnPDUSide;
            }
            set
            {
                this.strConnPDUSide = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_MODEL_CONN_DEV_SIDE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string ConnectorDevSide
        {
            get
            {
                return this.strConnDevSide;
            }
            set
            {
                this.strConnDevSide = value;
            }
        }
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MODEL_TOTAL_PSU_COUNT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int TotalPSUCount
        {
            get
            {
                return this.intTotalPSUCount;
            }
            set
            {
                this.intTotalPSUCount = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MODEL_REQ_PSU_COUNT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int RequiredPSUCount
        {
            get
            {
                return this.intReqPSUCount;
            }
            set
            {
                this.intReqPSUCount = value;
            }
        }
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MODEL_MOUNT_TYPE_ID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int MountTypeID
        {
            get
            {
                return this.intMountTypeID;
            }
            set
            {
                this.intMountTypeID = value;
            }
        }
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MODEL_AF_DIRECTION_ID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int AFDirectionID
        {
            get
            {
                return this.intAFDirectionID;
            }
            set
            {
                this.intAFDirectionID = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_MODEL_RINTERNAL_DEPTH, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public float RackInternalDepth
        {
            get
            {
                return this.fltRInternalDepth;
            }
            set
            {
                this.fltRInternalDepth = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_MODEL_RINTERNAL_HEIGHT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public float RackInternalHeight
        {
            get
            {
                return this.fltRInternalHeight;
            }
            set
            {
                this.fltRInternalHeight = value;
            }
        }
        
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MODEL_ENCL_FRONT_ROW_COUNT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int EnclFrontRowCount
        {
            get
            {
                return this.intEnclFrontRowCount;
            }
            set
            {
                this.intEnclFrontRowCount = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MODEL_ENCL_FRONT_COL_COUNT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int EnclFrontColCount
        {
            get
            {
                return this.intEnclFrontColCount;
            }
            set
            {
                this.intEnclFrontColCount = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MODEL_ENCL_REAR_ROW_COUNT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int EnclRearRowCount
        {
            get
            {
                return this.intEnclRearRowCount;
            }
            set
            {
                this.intEnclRearRowCount = value;
            }
        }
        
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MODEL_ENCL_REAR_COL_COUNT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int EnclRearColCount
        {
            get
            {
                return this.intEnclRearColCount;
            }
            set
            {
                this.intEnclRearColCount = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MODEL_BLADE_ROW_COUNT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int BladeRowCount
        {
            get
            {
                return this.intBladeRowCount;
            }
            set
            {
                this.intBladeRowCount = value;
            }
        }
        
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MODEL_BLADE_COL_COUNT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int BladeColCount
        {
            get
            {
                return this.intBladeColCount;
            }
            set
            {
                this.intBladeColCount = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_MODEL_RINTERNAL_WIDTH, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public float RackInternalWidth
        {
            get
            {
                return this.fltRInternalWidth;
            }
            set
            {
                this.fltRInternalWidth = value;
            }
        }

        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            DALCBase<AssetModelBAL> dalc = new DALCBase<AssetModelBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>

        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSETMODEL_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_MODELID, DbType.Int32, ModelID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }


        public DataSet retrieveExcludedModes()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_EXCLUDED_MODELS_BYBUID, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, BUID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        public DataSet retrieveByMfg(int MfgID, int BUID)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSETMODEL_LIST_BYMFGID, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_MFGID, DbType.Int32, MfgID);
            criteria.AddInParameter(Parameters.PARAM_AM_BUID, DbType.Int32, BUID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        public DataSet retrieveByAssetGroup(int AssetGroupID, int BUID)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSETMODEL_LIST_BY_ASSETGROUP, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_ASSETGROUPID, DbType.Int32, AssetGroupID);
            criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, BUID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// To exclude Asset models from SPC mapping
        /// </summary>
        public void ExcludeModelFromSPC(string ModelIDs, int BusinessUnitID, string Separator, int UserID)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSETMODEL_EXCLUDE_MODELS, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, BusinessUnitID);
            criteria.AddInParameter(Parameters.PARAM_MODELIDs, DbType.String, ModelIDs);
            criteria.AddInParameter(Parameters.PARAM_DELIMITERS, DbType.String, Separator);
            criteria.AddInParameter(Parameters.PARAM_USERID, DbType.Int32, UserID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();

        }


        public DataSet retrieveByParentModelID(int ModelID) //ParentModel
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSETMODEL_LIST_BYPARENTMODELID, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_MODELID, DbType.Int32, ModelID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        public DataSet retrieveByModelID(int ModelID)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSETMODEL_LIST_BYMODELID, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_MODELID, DbType.Int32, ModelID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public DataSet retrieveUserList()
        //{
        //    DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_USER_LIST_BY_BUSINESSUNIT, DALCResultType.DataSet);
        //    criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, BusinessUnitID);
        //    DataSet ds = (DataSet)criteria.ExecuteCommand();
        //    Dictionary<string, object> output = criteria.OutputParameters;
        //    return (ds);
        //}



        /// <summary>
        /// To check whether the Business Unit is already exists
        /// </summary>
        /// <returns>integer value</returns>
        public int exists()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSETMODEL_DOESEXIST, DALCResultType.Scalar);

            criteria.AddInParameter(Parameters.PARAM_MODELID, DbType.Int32, ModelID);
            criteria.AddInParameter(Parameters.PARAM_MODELNAME, DbType.String, ModelName);
            criteria.AddInParameter(Parameters.PARAM_MFGID, DbType.String, MfgID);
            criteria.AddInParameter(Parameters.PARAM_AM_BUID, DbType.String, BUID);

            //criteria.AddOutParameter("@count", DbType.Int32,0,5);

            int count = (int)criteria.ExecuteCommand();

            Dictionary<string, object> output = criteria.OutputParameters;
            //return() - Return No. of Records
            return (count);

        }

        public DataSet AutoMap(bool overWrite)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSETMODEL_AUTO_MAP, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_IS_OVERWRITE, DbType.Boolean, overWrite);
            criteria.AddInParameter(Parameters.PARAM_MODIFIEDBY, DbType.Boolean, CreatedBy);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);

        }

        public DataSet AutoMapByBusinessUnitID(bool overWrite)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSETMODEL_AUTO_MAP_BY_BUSINESSUNIT, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_IS_OVERWRITE, DbType.Boolean, overWrite);
            criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, BUID);
            criteria.AddInParameter(Parameters.PARAM_MODIFIEDBY, DbType.Int32, CreatedBy);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);

        }

        public DataSet Unmap()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSETMODEL_SPC_UNMAP, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_MODELIDs, DbType.String, ModelIDs);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        public bool IsCompatabile(int EnclModelID, int BladeModelID, int OrientationID)
        {
            int returnValue = 0;
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_BLADE_COMPATABILITY_CHECK, DALCResultType.Scalar);

            criteria.AddInParameter(Parameters.PARAM_MODEL_ENCL_MODEL_ID, DbType.Int32, EnclModelID);
            criteria.AddInParameter(Parameters.PARAM_MODEL_BLADE_MODEL_ID, DbType.Int32, BladeModelID);
            criteria.AddInParameter(Parameters.PARAM_ORIENTATION_ID, DbType.Int32, OrientationID);
            criteria.AddOutParameter(Parameters.PARAM_MODEL_RETURN_VAL, DbType.Int32, returnValue, 5);

            criteria.ExecuteCommand();

            Dictionary<string, object> output = criteria.OutputParameters;
            return Convert.ToInt32(output[Parameters.PARAM_MODEL_RETURN_VAL].ToString()) == 1 ? true : false;
        }

        public DataSet retrieveByMfgAndType(int MfgID,int AssetGroupId, int BUID)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSETMODEL_LIST_BYMFG_AND_TYPE, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_MFGID, DbType.Int32, MfgID);
            criteria.AddInParameter(Parameters.PARAM_ASSETGROUPID, DbType.Int32, AssetGroupId);
            criteria.AddInParameter(Parameters.PARAM_AM_BUID, DbType.Int32, BUID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        
    }
}
