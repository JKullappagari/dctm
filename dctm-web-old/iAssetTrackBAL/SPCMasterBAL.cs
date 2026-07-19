/*
File Name   :	AssetModelBAL.cs

Description :	Business Logic layer for Asset Model setup

Date created:	20 july 2011

Modification History:
***********************
CR		Name			    Date			Description
New		Jaagdeesh Babu K	20 July 2011	File has been created.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    [DALCOperationSP(InsertSP = StoredProcedures.SP_SPCMASTER_UPDATE, DeleteSP = StoredProcedures.SP_SPCMASTER_DELETE)]
    public class SPCMasterBAL
    {
        private int spcID;
        private  string makemodel;
        private string strSPCIDs;
        private string prodNo;
        private Nullable<float> depth_in;
        private Nullable<float> depth_mm;
        private Nullable<float> width_in;
        private Nullable<float> width_mm;
        private Nullable<float> height_in;
        private Nullable<float> height_mm;
        private Nullable<float> weight_lb;
        private Nullable<float> weight_kg;
        private Nullable<float> sqFT;
        private Nullable<float> sqMT;
        private string empty2;
        private Nullable<float> steadyWatts;
        private Nullable<float> maxWatts;
        private string notes1;
        private string notes2;
        private string refID;
        private string sourceFile;
        private int intCreatedBy;
        private int intMfgID;
        private int intTechID;
        private string strRackOrStand;
        private double dbDeviceCost;

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_SPCMASTER_SPCID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int SPCID
        {
            get
            {
                return this.spcID;
            }
            set
            {
                this.spcID = value;
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
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MFGID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
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

       

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_MAKEMODEL, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string MakeModel
        {
            get
            {
                return this.makemodel;
            }
            set
            {
                this.makemodel = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ASSET_RACK_OR_STAND, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string RackOrStand
        {
            get
            {
                return this.strRackOrStand;
            }
            set
            {
                this.strRackOrStand = value;
            }
        }
        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_DEVICECOST, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public Double DeviceCost
        {
            get
            {
                return this.dbDeviceCost;
            }
            set
            {
                this.dbDeviceCost = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_SPCIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public string SPCIDs
        {
            get
            {
                return this.strSPCIDs;
            }
            set
            {
                this.strSPCIDs = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_PRODNO, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string ProdNo
        {
            get
            {
                return this.prodNo;
            }
            set
            {
                this.prodNo = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_DEPTH_INCHES, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public Nullable<float> DepthIN
        {
            get
            {
                if (this.depth_in.HasValue)
                    return this.depth_in;
                else
                    return 0; 
            }
            set
            {
                this.depth_in = value;

            }
        }

        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_DEPTH_MM, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public Nullable<float> DepthMM
        {
            get
            {
                if (this.depth_mm.HasValue)
                    return this.depth_mm;
                else
                    return 0;
            }
            set
            {
                this.depth_mm = value;

            }
        }

        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_WIDTH_INCHES, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public Nullable<float> WidthIN
        {
            get
            {
                if (this.width_in.HasValue)
                    return this.width_in;
                else
                    return 0;
            }
            set
            {
                this.width_in = value;

            }
        }


        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_WIDTH_MM, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public Nullable<float> WidthMM
        {
            get
            {
                if (this.width_mm.HasValue)
                    return this.width_mm;
                else
                    return 0;
            }
            set
            {
                this.width_mm = value;

            }
        }

        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_HEIGHT_INCHES, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public Nullable<float> HeightIN
        {
            get
            {
                if (this.height_in.HasValue)
                    return this.height_in;
                else
                    return 0;
            }
            set
            {
                this.height_in = value;

            }
        }


        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_HEIGHT_MM, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public Nullable<float> HeightMM
        {
            get
            {
                if (this.height_mm.HasValue)
                    return this.height_mm;
                else
                    return 0;
            }
            set
            {
                this.height_mm = value;

            }
        }

        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_WEIGHT_LB, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public Nullable<float> WeightLB
        {
            get
            {
                if (this.weight_lb.HasValue)
                    return this.weight_lb;
                else
                    return 0;
            }
            set
            {
                this.weight_lb = value;

            }
        }

        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_WEIGHT_KG, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public Nullable<float> WeightKG
        {
            get
            {
                if (this.weight_kg.HasValue)
                    return this.weight_kg;
                else
                    return 0;
            }
            set
            {
                this.weight_kg = value;

            }
        }

        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_SQ_FT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public Nullable<float> SqFT
        {
            get
            {
                if (this.sqFT.HasValue)
                    return this.sqFT;
                else
                    return 0;
            }
            set
            {
                this.sqFT = value;

            }
        }

        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_SQ_MT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public Nullable<float> SqMT
        {
            get
            {
                if (this.sqMT.HasValue)
                    return this.sqMT;
                else
                    return 0;
            }
            set
            {
                this.sqMT = value;

            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_EMPTY2, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string Empty2
        {
            get
            {
                return this.empty2;
            }
            set
            {
                this.empty2 = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_STEADY_WATTS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public Nullable<float> SteadyStateWatts
        {
            get
            {
                if (this.steadyWatts.HasValue)
                    return this.steadyWatts;
                else
                    return 0;
            }
            set
            {
                this.steadyWatts = value;

            }
        }

        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_MAX_WATTS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public Nullable<float> MaxWatts
        {
            get
            {
                if (this.maxWatts.HasValue)
                    return this.maxWatts;
                else
                    return 0;
            }
            set
            {
                this.maxWatts = value;

            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_NOTE1, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string Notes1
        {
            get
            {
                return this.notes1;
            }
            set
            {
                this.notes1 = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_NOTES2, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string Notes2
        {
            get
            {
                return this.notes2;
            }
            set
            {
                this.notes2 = value;
            }
        }
       
        //[DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_ITEMTYPE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        //public string ItemType
        //{
        //    get
        //    {
        //        return this.itemType;
        //    }
        //    set
        //    {
        //        this.itemType = value;
        //    }
        //}

        //[DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_PATH, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        //public string Path
        //{
        //    get
        //    {
        //        return this.path;
        //    }
        //    set
        //    {
        //        this.path = value;
        //    }
        //}
        
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_REFID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string RowRefID
        {
            get
            {
                return this.refID;
            }
            set
            {
                this.refID = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_SOURCE_FILE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string SourceFile
        {
            get
            {
                return this.sourceFile;
            }
            set
            {
                this.sourceFile = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_CREATEDBY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_CREATEDBY, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
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
        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            DALCBase<SPCMasterBAL> dalc = new DALCBase<SPCMasterBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_SPCMASTER_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_SPCMASTER_SPCID, DbType.Int32, SPCID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        
        /// <summary>
        /// To check whether the Business Unit is already exists
        /// </summary>
        /// <returns>integer value</returns>
        public int exists()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_SPCMASTER_DOESEXIST, DALCResultType.Scalar);

            criteria.AddInParameter(Parameters.PARAM_SPCMASTER_SPCID, DbType.Int32, SPCID);
            criteria.AddInParameter(Parameters.PARAM_MAKEMODEL, DbType.String, MakeModel);
            
            //criteria.AddOutParameter("@count", DbType.Int32,0,5);

            int count = (int)criteria.ExecuteCommand();
            
            Dictionary<string , object> output = criteria.OutputParameters;
            //return() - Return No. of Records
            return (count); 

        }

    }
}
