using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using iAssetTrack.DALC;
using iAssetTrackBAL;


namespace iAssetTrack.BAL
{
    [DALCOperationSP(InsertSP = StoredProcedures.SP_TENANT_UPDATE, UpdateSP = StoredProcedures.SP_TENANT_UPDATE, DeleteSP = StoredProcedures.SP_TENANT_DELETE)]
    public class TenantBAL
    {
        private int intTenantId;
        private string strTenantFullName;
        private string strTenantShortName;
        private string strTenantType;
        private int intTenantTypeSize;
        private int intUserCount;
        private string strContactFName;
        private string strContactLName;
        private string strContactEmail;
        private string strAssignedLocations;
        private int intCreatedBy;
        private int intLastModifiedBy;
        private int intStatus;
        private string strTenantIds;
        private string strAdminPermissions;
        private string strUserPermissions;

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_TENANT_FULLNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string FullName
        {
            get
            {
                return this.strTenantFullName;
            }
            set
            {
                this.strTenantFullName = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_TENANT_SHORTNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string ShortName
        {
            get
            {
                return this.strTenantShortName;
            }
            set
            {
                this.strTenantShortName = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_TENANT_TYPE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string TenantType
        {
            get
            {
                return this.strTenantType;
            }
            set
            {
                this.strTenantType = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_TENANT_TYPE_SIZE, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int TenantTypeSize
        {
            get
            {
                return this.intTenantTypeSize;
            }
            set
            {
                this.intTenantTypeSize = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_TENANTID, ParameterDirection = ParameterDirection.InputOutput, DALCOperation = DALCOperation.Insert)]
        public int TenantId
        {
            get
            {
                return this.intTenantId;
            }
            set
            {
                this.intTenantId = value;
            }
        }

        [DALCOperationParams(DataType = DbType.Int32, ParameterName = Parameters.PARAM_TENANT_USER_COUNT, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public int TenantUserCount
        {
            get
            {
                return this.intUserCount;
            }
            set
            {
                this.intUserCount = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_TENANT_CONTACT_FNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string ContactFirstName
        {
            get
            {
                return this.strContactFName;
            }
            set
            {
                this.strContactFName = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_TENANT_CONTACT_LNAME, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string ContactLastName
        {
            get
            {
                return this.strContactLName;
            }
            set
            {
                this.strContactLName = value;
            }
        }


        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_TENANT_CONTACT_EMAIL, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string ContactEmail
        {
            get
            {
                return this.strContactEmail;
            }
            set
            {
                this.strContactEmail = value;
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

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_TENANTIDS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Delete)]
        public string TenantIds
        {
            get
            {
                return this.strTenantIds;
            }
            set
            {
                this.strTenantIds = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_TENANT_ASSIGNED_LOCATIONS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string TenantAssignedLocations
        {
            get
            {
                return this.strAssignedLocations;
            }
            set
            {
                this.strAssignedLocations = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_TENANT_ADMIN_PERMISSIONS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string AdminPermissions
        {
            get
            {
                return this.strAdminPermissions;
            }
            set
            {
                this.strAdminPermissions = value;
            }
        }

        [DALCOperationParams(DataType = DbType.String, ParameterName = Parameters.PARAM_TENANT_USER_PERMISSIONS, ParameterDirection = ParameterDirection.Input, DALCOperation = DALCOperation.Insert)]
        public string UserPermissions
        {
            get
            {
                return this.strUserPermissions;
            }
            set
            {
                this.strUserPermissions = value;
            }
        }


        public void Persist(DALCOperation operation)
        {
            DALCBase<TenantBAL> dalc = new DALCBase<TenantBAL>(this);
            dalc.SaveData(operation, 1);
        }

        /// <summary>
        /// Retrive tenant details
        /// </summary>
        public DataSet retrieve()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_TENANT_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_TENANTID, DbType.Int32, TenantId);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }


        /// <summary>
        /// Retrive Location list by Tenant type/Location Type
        /// </summary>
        public DataSet retrieveByTenantType(string LocationType)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_TENANT_LOCATION_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_TENANT_TYPE, DbType.String, LocationType);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }



        /// <summary>
        /// ceeck user exist or not
        /// </summary>
        public int exists()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_TENANT_DOESEXIST, DALCResultType.Scalar);

            criteria.AddInParameter(Parameters.PARAM_TENANTID, DbType.Int32, TenantId);
            criteria.AddInParameter(Parameters.PARAM_TENANT_FULLNAME, DbType.String, FullName);
            criteria.AddInParameter(Parameters.PARAM_TENANT_SHORTNAME, DbType.String, ShortName);

            int count = (int)criteria.ExecuteCommand();

            Dictionary<string, object> output = criteria.OutputParameters;
            return (count);

        }

        /// <summary>
        /// RETRIVE ASSIGNED USERS LIST FOR A TENANT
        /// </summary>
        /// <param name="LocationType"></param>
        /// <returns></returns>
        public DataSet retrieveAssignedUsers()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_TENANT_ASSIGNED_USERS, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_TENANTID, DbType.Int32, TenantId);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// RETRIVE ASSIGNED Groups for tenants
        /// </summary>
        /// <param name="LocationType"></param>
        /// <returns></returns>
        public DataSet retrieveGroupAssignmentList()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_TENANT_GROUP_ASSIGNMENT_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_TENANTID, DbType.Int32, TenantId);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// Retrives all locations assigned to a tenant. this list includes all levels of locations.
        /// </summary>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        public DataSet retrieveLocationAssignmentList(int LocationId)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_TENANT_LOCATION_ASSIGNMENT_LIST, DALCResultType.DataSet);
            if (LocationId != 0)
                criteria.AddInParameter(Parameters.PARAM_LOCATIONID, DbType.Int32, LocationId);
            criteria.AddInParameter(Parameters.PARAM_TENANTID, DbType.Int32, TenantId);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// to retrieve Tenant - Division assignment list
        /// </summary>
        /// <returns></returns>
        public DataSet retrieveDivisionAssignmentList()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_TENANT_DIVISION_ASSIGNMENT_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_TENANTID, DbType.Int32, TenantId);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }


        /// <summary>
        /// to retrieve Tenant - Owner assignment list
        /// </summary>
        /// <returns></returns>
        public DataSet retrieveOwnerAssignmentList()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_TENANT_OWNER_ASSIGNMENT_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_TENANTID, DbType.Int32, TenantId);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }


        /// <summary>
        /// to retrieve Tenant - Application ASSIGNMENT LIST
        /// </summary>
        /// <returns></returns>
        public DataSet retrieveApplicationAssignmentList()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_TENANT_APPLICATION_ASSIGNMENT_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_TENANTID, DbType.Int32, TenantId);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// to retrieve tenant - host assignment list
        /// </summary>
        /// <returns></returns>
        public DataSet retrieveHostAssignmentList()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_TENANT_HOST_ASSIGNMENT_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_TENANTID, DbType.Int32, TenantId);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// to retrieve Tenant - Asset assignment list
        /// </summary>
        /// <returns></returns>
        public DataSet retrieveAssetAssignmentList()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_TENANT_ASSET_ASSIGNMENT_LIST, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_TENANTID, DbType.Int32, TenantId);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        
    }
}