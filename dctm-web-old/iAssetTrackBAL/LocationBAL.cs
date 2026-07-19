/*
File Name   :	LocationBAL.cs

Description :	Business Logic layer for Location

Date created:	27 March 2006

Modification History:
***********************
CR		Name			Date			Description
New		Venkatesan M	27/03/2006		File has been created.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;

namespace iAssetTrack.BAL
{
    [DALCOperationSP(InsertSP = StoredProcedures.SP_LOCATION_UPDATE, DeleteSP = StoredProcedures.SP_LOCATION_DELETE, UpdateSP = StoredProcedures.SP_LOCATION_UPDATE)]
    public class LocationBAL
    {
        private int intLocationID;
        private int intParentLocationID;
        private int intLocationTypeID;
        private string strLocation;
        private string strDescription;
        private int intStatus;
        private int intIsExitDoor;
        private int intIsCheckOutLocation;
        private string strIpAddress;
        private int intCreatedBy;
        private int intLastModifiedBy;
        private string strLocationIDs;
        private int intSiteID;
        # region v3.8 - Data members
        private string strTagID;
        private string strManufacturer;
        private string strModel;
        private string strFloor;
        private string strSerialNumber;
        private int fltHeight;
        private int intModelID;
        private string strExternalID;

        # endregion


        # region v3.8 - Properties
        //*V3.8-Added on 21Oct2013-By Amar Vidya 
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_TAGID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_TAGID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string TagID
        {
            get
            {
                return this.strTagID;
            }
            set
            {
                this.strTagID = value;
            }
        }//*

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_FLOOR, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_FLOOR, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string FloorNo
        {
            get
            {
                return this.strFloor;
            }
            set
            {
                this.strFloor = value;
            }
        }

        # endregion

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_LOCATION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_LOCATION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string Location
        {
            get
            {
                return this.strLocation;
            }
            set
            {
                this.strLocation = value;
            }
        }
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ISEXITDOOR, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ISEXITDOOR, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int IsExitDoor
        {
            get
            {
                return this.intIsExitDoor;
            }
            set
            {
                this.intIsExitDoor = value;
            }
        }



        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ISCHECKOUTLOCATION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_ISCHECKOUTLOCATION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int IsCheckOutLocation
        {
            get
            {
                return this.intIsCheckOutLocation;
            }
            set
            {
                this.intIsCheckOutLocation = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_IPADDRESS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_IPADDRESS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string IpAddress
        {
            get
            {
                return this.strIpAddress;
            }
            set
            {
                this.strIpAddress = value;
            }
        }


        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_PARENTLOCATIONID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_PARENTLOCATIONID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int ParentLocationID
        {
            get
            {
                return this.intParentLocationID;
            }
            set
            {
                this.intParentLocationID = value;
            }
        }
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_LOCATIONTYPEID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_LOCATIONTYPEID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int LocationTypeID
        {
            get
            {
                return this.intLocationTypeID;
            }
            set
            {
                this.intLocationTypeID = value;
            }
        }
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_DESCRIPTION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_DESCRIPTION, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
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


        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_LOCATIONIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public string LocationIDs
        {
            get
            {
                return this.strLocationIDs;
            }
            set
            {
                this.strLocationIDs = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_STATUS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_STATUS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
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

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_LOCATIONID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_LOCATIONID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Update)]
        public int LocationID
        {
            get
            {
                return this.intLocationID;
            }
            set
            {
                this.intLocationID = value;
            }
        }
        //[DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_SITEID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
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


        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_MFG, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_MFG, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string Manufacturer
        {
            get
            {
                return this.strManufacturer;
            }
            set
            {
                this.strManufacturer = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_Model, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_Model, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string Model
        {
            get
            {
                return this.strModel;
            }
            set
            {
                this.strModel = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_SERIAL_NUMBER, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_SERIAL_NUMBER, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string SerialNumber
        {
            get
            {
                return this.strSerialNumber;
            }
            set
            {
                this.strSerialNumber = value;
            }
        }
        
        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_HEIGHT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.Double, ParameterName = Parameters.PARAM_HEIGHT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public int Height
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

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MODELID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_MODELID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
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

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_LOCATION_EXTERNAL_ID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_LOCATION_EXTERNAL_ID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Update)]
        public string ExternalID
        {
            get
            {
                return this.strExternalID;
            }
            set
            {
                this.strExternalID = value;
            }
        }

        /// <summary>
        /// Insert / Update data
        /// </summary>
        /// <param name="operation">Operation mode</param>
        public void Persist(DALCOperation operation)
        {
            DALCBase<LocationBAL> dalc = new DALCBase<LocationBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrieve records
        /// </summary>
        /// <returns>Data Set</returns>
        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_LOCATION_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_LOCATIONID, DbType.Int32, LocationID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        public DataSet retrieveUPositions()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_LOCATION_LIST_WITH_UPOSITIONS, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_LOCATIONID, DbType.Int32, LocationID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }




        public DataSet retrieveAllLocations()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_LOCATION_ALL_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_LOCATIONID, DbType.Int32, LocationID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        public DataSet GetLocationBYSite()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_LOCATIONBYSITE_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_SITEID, DbType.Int32, SiteID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
        public DataSet GetLocationBYLocation(int intLocID)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_LOCATIONBYLOCATION_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_LOCATIONID, DbType.Int32, intLocID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }
        public bool hasChildren(int intLocID)
        {
            DataSet dsLocations = GetLocationBYLocation(intLocationID);
            if (dsLocations.Tables[0].Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// To check whether the Business Unit is already exists
        /// </summary>
        /// <returns>integer value</returns>
        public int exists()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_LOCATION_DOESEXIST, DALCResultType.Scalar);

            criteria.AddInParameter(Parameters.PARAM_LOCATIONID, DbType.Int32, LocationID);
            criteria.AddInParameter(Parameters.PARAM_LOCATION, DbType.String, Location);
            criteria.AddInParameter(Parameters.PARAM_PARENTLOCATIONID, DbType.Int32, ParentLocationID);

            //criteria.AddOutParameter("@count", DbType.Int32,0,5);

            int count = (int)criteria.ExecuteCommand();

            Dictionary<string, object> output = criteria.OutputParameters;
            //return() - Return No. of Records
            return (count);

        }

        public int GetAssetCountByRoom()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSET_COUNT_BY_ROOM, DALCResultType.Scalar);

            criteria.AddInParameter(Parameters.PARAM_LOCATIONID, DbType.Int32, LocationID);

            int count = (int)criteria.ExecuteCommand();

            Dictionary<string, object> output = criteria.OutputParameters;
            //return() - Return No. of Records
            return (count);
        }

        public void UpdateUpositionDetails(int AssetID,int LocationID, int StartPos,int OrientationID,float AssetWidth, float AssetDepth, int NoOfRUs, bool OnOff)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_LOCATION_LIST_WITH_UPOSITIONS_UPDATE, DALCResultType.Scalar);
            criteria.AddInParameter(Parameters.PARAM_ASSETID, DbType.Int32, AssetID);
            criteria.AddInParameter(Parameters.PARAM_LOCATIONID, DbType.Int32, LocationID);
            criteria.AddInParameter(Parameters.PARAM_ASSET_START_POS, DbType.Int32, StartPos);
            criteria.AddInParameter(Parameters.PARAM_ASSET_NO_OF_RUS, DbType.Int32, NoOfRUs);
            criteria.AddInParameter(Parameters.PARAM_ORIENTATION_ID, DbType.Int32, OrientationID);
            criteria.AddInParameter(Parameters.PARAM_ASSET_WIDTH, DbType.Double, AssetWidth);
            criteria.AddInParameter(Parameters.PARAM_ASSET_DEPTH, DbType.Double, AssetDepth);
            criteria.AddInParameter(Parameters.PARAM_BIT_VALUE, DbType.Boolean, OnOff);

            criteria.ExecuteCommand();

        }


        public DataSet retrieveLocationPath(int LoggedInUserId)
        {
            int buID, siteID;
            SiteLocationAssignmentBAL slBAL = new SiteLocationAssignmentBAL();
            DataSet dsSL = slBAL.retrieve();
            if (dsSL.Tables.Count > 0 && dsSL.Tables[0].Rows.Count > 0)
            {
                DataRow[] dr = dsSL.Tables[0].Select(" LocationID = " + LocationID.ToString());
                if (dr.Length > 0)
                {
                    buID = int.Parse(dr[0][DBFields.DBFIELD_BUSINESSUNITID].ToString());
                    siteID = int.Parse(dr[0][DBFields.DBFIELD_SITEID].ToString());
                }
                else
                {
                    buID = 0;
                    siteID = 0;
                }
            }
            else
            {
                buID = 0;
                siteID = 0;
            }
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_GET_LOCATION_PATH, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_LOCATIONID, DbType.Int32, LocationID);
            criteria.AddInParameter(Parameters.PARAM_SITEID, DbType.Int32, siteID);
            criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, buID);
            criteria.AddInParameter(Parameters.PARAM_LOGGEDIN_USER_ID, DbType.Int32, LoggedInUserId);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        public DataSet retrieveRackPositions()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_GET_RACK_POSITIONS, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_LOCATIONID, DbType.Int32, LocationID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        public bool ValidateRackPosition(int LocationID, int AssetID, int StartPosition, int NoOfRUs, int OrientationID, float AssetWidth,float AssetDepth)
        {
            int returnValue = 0;
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_VALIDATE_RACK_POSITIONS, DALCResultType.Scalar);

            criteria.AddInParameter(Parameters.PARAM_LOCATIONID, DbType.Int32, LocationID);
            criteria.AddInParameter(Parameters.PARAM_ASSETID, DbType.Int32, AssetID);
            criteria.AddInParameter(Parameters.PARAM_ASSET_START_POS, DbType.Int32, StartPosition);
            criteria.AddInParameter(Parameters.PARAM_ASSET_NO_OF_RUS, DbType.Int32, NoOfRUs);
            criteria.AddInParameter(Parameters.PARAM_ORIENTATION_ID, DbType.Int32, OrientationID);
            criteria.AddInParameter(Parameters.PARAM_ASSET_DEPTH, DbType.Int32, AssetDepth);
            criteria.AddInParameter(Parameters.PARAM_ASSET_WIDTH, DbType.Int32, AssetWidth);
            criteria.AddOutParameter(Parameters.PARAM_ASSET_RETURN_VALUE, DbType.Int32, returnValue,5);

            criteria.ExecuteCommand();

            Dictionary<string, object> output = criteria.OutputParameters;
            return Convert.ToInt32(output[Parameters.PARAM_ASSET_RETURN_VALUE].ToString()) == 1 ? true : false;
        }

        public bool IsPartOfDisposeDecomRooms(int LocationID)
        {
            bool returnValue = false;
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_LOCATION_IS_PART_OF_DISPOSE_DECOM_ROOMS, DALCResultType.Scalar);

            criteria.AddInParameter(Parameters.PARAM_LOCATIONID, DbType.Int32, LocationID);
            criteria.AddOutParameter(Parameters.PARAM_ASSET_RETURN_VALUE, DbType.Int32, returnValue, 5);

            criteria.ExecuteCommand();
            
            Dictionary<string, object> output = criteria.OutputParameters;
            return Convert.ToInt32(output[Parameters.PARAM_ASSET_RETURN_VALUE].ToString()) == 1 ? true : false;
        }

        public int IsATenantLocation(int LocationId,int UserId)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_LOCATION_IS_A_TENANT_LOCATION, DALCResultType.Scalar);

            criteria.AddInParameter(Parameters.PARAM_LOCATIONID, DbType.Int32, LocationId);
            criteria.AddInParameter(Parameters.PARAM_USERID, DbType.Int32, UserId);

            int count = (int)criteria.ExecuteCommand();

            Dictionary<string, object> output = criteria.OutputParameters;
            return (count);

        }

        public int HasChildNodes(int LocationId)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_LOCATION_HAS_CHILD_NODES, DALCResultType.Scalar);

            criteria.AddInParameter(Parameters.PARAM_LOCATIONID, DbType.Int32, LocationId);

            int count = (int)criteria.ExecuteCommand();

            Dictionary<string, object> output = criteria.OutputParameters;
            return (count);

        }

        

    }
}
