/*
File Name   :	BUSiteAssignmentBAL.cs

Description :	Business Logic layer for Business unit and site assignment setup

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
    [DALCOperationSP(InsertSP = StoredProcedures.SP_BUSITEASSIGNMENT_UPDATE)]
    public class BUSiteAssignmentBAL
    {
            private int intBUSiteID;
            private int intBusinessUnitID;
            private int intSiteAccessID;
            private int intSiteID;
            private int intStatus;
            private int intCreatedBy;
            private string strDelimiters;
            private int intLastModifiedBy;
            private string strSiteIDs;

            [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_BUSINESSUNITID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
            public int BusinessUnitID
            {
                get
                {
                    return this.intBusinessUnitID;
                }
                set
                {
                    this.intBusinessUnitID = value;
                }
            }
            [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_SITEACCESSID, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
            public int SiteAccessID
            {
                get
                {
                    return this.intSiteAccessID; 
                }
                set
                {
                    this.intSiteAccessID = value;
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
            [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_DELIMITERS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
            public string Delimiters
            {
                get
                {
                    return this.strDelimiters;
                }
                set
                {
                    this.strDelimiters = value;
                }
            }

            [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_SITEIDs, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
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
            [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_STATUS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
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

            public int BUSiteID
            {
                get
                {
                    return this.intBUSiteID;
                }
                set
                {
                    this.intBUSiteID = value;
                }
            }

            public void Persist(DALCOperation operation)
            {
                DALCBase<BUSiteAssignmentBAL> dalc = new DALCBase<BUSiteAssignmentBAL>(this);
                dalc.SaveData(operation, 1);
            }

            public DataSet retrieve()
            {
                DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_BUSITEASSIGNMENT_LIST, DALCResultType.DataSet);
                criteria.AddInParameter(Parameters.PARAM_BUSITEID, DbType.Int32, BUSiteID);
                DataSet ds = (DataSet)criteria.ExecuteCommand();
                Dictionary<string, object> output = criteria.OutputParameters;
                return (ds);
            }


            public DataSet retrieveAvailSites()
            {
                DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_AVAILSITE_LIST, DALCResultType.DataSet);
                criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, BusinessUnitID);
                criteria.AddInParameter(Parameters.PARAM_SITEACCESSID, DbType.Int32, SiteAccessID);
                DataSet ds = (DataSet)criteria.ExecuteCommand();
                Dictionary<string, object> output = criteria.OutputParameters;
                return (ds);
            }


            public DataSet retrieveAssignSite()
            {
                DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_ASSIGNSITE_LIST, DALCResultType.DataSet);
                criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, BusinessUnitID);
                criteria.AddInParameter(Parameters.PARAM_SITEACCESSID, DbType.Int32, SiteAccessID);
                DataSet ds = (DataSet)criteria.ExecuteCommand();
                Dictionary<string, object> output = criteria.OutputParameters;
                return (ds);
            }


        }

    }



