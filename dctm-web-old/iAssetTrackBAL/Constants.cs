/*
File Name   :	constants.cs

Description :	constants

Date created:	27 March 2006

Modification History:
***********************
CR		Name			Date			Description
New		Murugan K	27/03/2006		File has been created.
*/
using System;
using System.Data;
using System.Configuration;
using System.Web;

namespace iAssetTrackBAL
{
    /// <summary>
    /// Summary description for Constants
    /// </summary>
    public class StoredProcedures
    {
        //RptTransaction List
        public const string SP_GETTRANSACTIONTYPE = "iAssetTrack_Sp_TransactionType";
        //RptTransactionHistory List
        public const string SP_GETTRANSACTIONSTATUS = "iAssetTrack_Sp_TransactionStatus";
        // tblExternalSysIntegrationLog
        public const string SP_GETTRANSTYPE = "iAssetTrack_Sp_TransType";
        public const string SP_GETSTATUS = "iAssetTrack_Sp_Status";

        // Configuration

        // (Redundant) public const string SP_GETCONFIGVALUE = "iAssetTrack_Sp_GetConfigValue";

        //Check Before Delete
        public const string SP_CHECKBEFOREDELETE = "iAssetTrack_Sp_CheckBeforeInactive_DoesExist";
        // (Redundant) public const string SP_Site_List_By_User = "iAssetTrack_Sp_Site_List_By_User";

        // (Redundant) public const string SP_COUNTRYLIST_LIST = "iAssetTrack_Sp_CountryList_List";



        ////Business Unit
        //public const string SP_BUSINESSUNIT_UPDATE = "SN_Dctrack.iAssetTrack_Sp_BusinessUnit_Update";
        //public const string SP_BUSINESSUNIT_DELETE = "SN_Dctrack.iAssetTrack_Sp_BusinessUnit_Delete";
        //public const string SP_BUSINESSUNIT_LIST = "SN_Dctrack.iAssetTrack_Sp_BusinessUnit_List";
        //public const string SP_BUSINESSUNIT_LISTBYBUNAME = "SN_Dctrack.iAssetTrack_Sp_BusinessUnit_ListByBUName";
        //public const string SP_BUSINESSUNIT_LISTBYUSERID = "SN_Dctrack.iAssetTrack_Sp_BusinessUnit_ListByUserID";
        //public const string SP_BUSINESSUNIT_DOESEXIST = "SN_Dctrack.iAssetTrack_Sp_BusinessUnit_DoesExist";

        //Business Unit
        public const string SP_BUSINESSUNIT_UPDATE = "iAssetTrack_Sp_BusinessUnit_Update";
        public const string SP_BUSINESSUNIT_DELETE = "iAssetTrack_Sp_BusinessUnit_Delete";
        public const string SP_BUSINESSUNIT_LIST = "iAssetTrack_Sp_BusinessUnit_List";
        public const string SP_BUSINESSUNIT_LISTBYBUNAME = "iAssetTrack_Sp_BusinessUnit_ListByBUName";
        public const string SP_BUSINESSUNIT_LISTBYUSERID = "iAssetTrack_Sp_BusinessUnit_ListByUserID";
        public const string SP_BUSINESSUNIT_DOESEXIST = "iAssetTrack_Sp_BusinessUnit_DoesExist";
        public const string SP_BUSINESSUNIT_LIST_BY_USERID = "iAssetTrack_Sp_BusinessUnit_List_By_UserID";
        public const string SP_TENANT_LIST_BY_USERID = "iAssetTrack_Sp_Tenant_List_By_UserID";

        //spc rEPORTS - kjb 08 Jan 2013
        public const string SP_REPORTS_LIST = "iAssetTrack_Sp_SPCReports_List";

        //Device Reg -- kjb on 07 may 2012
        public const string SP_MOBILEDEVICE_UPDATE = "iAssetTrack_Sp_MobileDevice_Update";
        public const string SP_MOBILEDEVICE_DELETE = "iAssetTrack_Sp_MobileDevice_Delete";
        public const string SP_MOBILEDEVICE_LIST = "iAssetTrack_Sp_MobileDevice_List";
        public const string SP_MOBILEDEVICE_DOESEXIST = "iAssetTrack_Sp_MobileDevice_DoesExist";


        //Manufacturer
        public const string SP_MANUFACTURER_UPDATE = "iAssetTrack_Sp_Manufacturer_Update";
        public const string SP_MANUFACTURER_DELETE = "iAssetTrack_Sp_Manufacturer_Delete";
        public const string SP_MANUFACTURER_LIST = "iAssetTrack_Sp_Manufacturer_List";
        public const string SP_MANUFACTURER_LIST_BY_MODEL_TYPE = "iAssetTrack_Sp_Manufacturer_List_By_ModelType";
        //public const string SP_MANUFACTURER_LISTBYUSERID = "iAssetTrack_Sp_Manufacturer_ListByUserID";
        public const string SP_MANUFACTURER_DOESEXIST = "iAssetTrack_Sp_Manufacturer_DoesExist";



        //SPCMaster
        public const string SP_SPCMASTER_UPDATE = "iAssetTrack_Sp_SPCMaster_Update";
        public const string SP_SPCMASTER_DELETE = "iAssetTrack_Sp_SPCMaster_Delete";
        public const string SP_SPCMASTER_LIST = "iAssetTrack_Sp_SPCMaster_List";
        public const string SP_SPCMASTER_DOESEXIST = "iAssetTrack_Sp_SPCMaster_DoesExist";

        //UOM
        public const string SP_UOM_LIST = "iAssetTrack_Sp_UOM_List";

        //Asset Model
        public const string SP_ASSETMODEL_UPDATE = "iAssetTrack_Sp_AssetModel_Update";
        public const string SP_ASSETMODEL_DELETE = "iAssetTrack_Sp_AssetModel_Delete";
        public const string SP_ASSETMODEL_LIST = "iAssetTrack_Sp_AssetModel_List";
        //public const string SP_ASSETMODEL_LISTBYUSERID = "iAssetTrack_Sp_AssetModel_ListByUserID";
        public const string SP_ASSETMODEL_DOESEXIST = "iAssetTrack_Sp_AssetModel_DoesExist";
        public const string SP_ASSETMODEL_LIST_BYMFGID = "iAssetTrack_Sp_AssetModel_List_byMfgID";
        public const string SP_ASSETMODEL_LIST_BY_ASSETGROUP = "iAssetTrack_Sp_AssetModel_List_byAssetGroup";
        public const string SP_ASSETMODEL_EXCLUDE_MODELS = "iAssetTrack_Sp_AssetModel_ExcludeModelsForSPC";
        public const string SP_ASSETMODEL_LIST_BYPARENTMODELID = "iAssetTrack_Sp_AssetModel_List_byParentModelID";
        public const string SP_ASSETMODEL_LIST_BYMODELID = "iAssetTrack_Sp_AssetModel_List_byModelID";
        public const string SP_ASSETMODEL_AUTO_MAP = "iAssetTrack_sp_AutoMap";
        public const string SP_ASSETMODEL_AUTO_MAP_BY_BUSINESSUNIT = "iAssetTrack_sp_AutoMap_ByBusinessUnit";
        public const string SP_ASSETMODEL_SPC_UNMAP = "iAssetTrack_sp_SPCUnmap";
        public const string SP_BLADE_COMPATABILITY_CHECK = "iAssetTrack_sp_Blade_Compatability_Check";
        public const string SP_ASSETMODEL_LIST_BYMFG_AND_TYPE = "iAssetTrack_Sp_AssetModel_List_byMfgandType";
        


        //// V000021-Added on 19-Oct-2012
        public const string SP_ASSETMODEL_LIST_BYBUID = "iAssetTrack_Sp_AssetModel_List_byBUID";
        public const string SP_EXCLUDED_MODELS_BYBUID = "iAssetTrack_Sp_ExcludedModels_byBUID"; // by kjb on 29 Nov 2012

        //InventoryUpdate
        public const string SP_INVENTORY_UPDATE_DATA = "iAssetTrack_Sp_InventoryUpdate_Data";
        public const string SP_INVENTORY_UPDATE = "iAssetTrack_Sp_InventoryUpdate";

        //Application
        public const string SP_APPLICATION_UPDATE = "iAssetTrack_Sp_Application_Update";
        public const string SP_APPLICATION_DELETE = "iAssetTrack_Sp_Application_Delete";
        public const string SP_APPLICATION_LIST = "iAssetTrack_Sp_Application_List";
        public const string SP_APPLICATION_STATUS_LIST = "iAssetTrack_Sp_Application_Status_List";
        public const string SP_APPLICATION_LIST_BY_PAGE = "iAssetTrack_Sp_Application_List_ByPage";
        public const string SP_APPLICATION_DOESEXIST = "iAssetTrack_Sp_Application_DoesExist";
        //-->v3.9
        public const string SP_APPLICATION_LIST_BY_CRITICALITY = "iAssetTrack_Sp_Application_List_by_AppCriticality";

        //Division
        public const string SP_DIVISION_UPDATE = "iAssetTrack_Sp_Division_Update";
        public const string SP_DIVISION_DELETE = "iAssetTrack_Sp_Division_Delete";
        public const string SP_DIVISION_LIST = "iAssetTrack_Sp_Division_List";
        public const string SP_DIVISIONBYBU_LIST = "iAssetTrack_Sp_DivisionByBusinessUnit_List";
        public const string SP_DIVISION_DOESEXIST = "iAssetTrack_Sp_Division_DoesExist";
        public const string SP_BUDIVASSIGNMENT_LIST = "iAssetTrack_Sp_BUDivAssignment_List";
        public const string SP_BUDIVASSIGNMENT_UPDATE = "iAssetTrack_Sp_BUDivAssignment_Update";
        public const string SP_AVAILDIVISION_LIST = "iAssetTrack_Sp_AvailDivisions_List";
        public const string SP_ASSIGNDIVISION_LIST = "iAssetTrack_Sp_AssignDivisions_List";

        //Application Type
        public const string SP_APPLTYPE_UPDATE = "iAssetTrack_Sp_ApplType_Update";
        public const string SP_APPLTYPE_DELETE = "iAssetTrack_Sp_ApplType_Delete";
        public const string SP_APPLTYPE_LIST = "iAssetTrack_Sp_ApplType_List";
        public const string SP_APPLTYPE_DOESEXIST = "iAssetTrack_Sp_ApplType_DoesExist";

        //Application Criticality
        public const string SP_APPL_CRITICALITY_UPDATE = "iAssetTrack_Sp_ApplCriticality_Update";
        public const string SP_APPL_CRITICALITY_DELETE = "iAssetTrack_Sp_ApplCriticality_Delete";
        public const string SP_APPL_CRITICALITY_LIST = "iAssetTrack_Sp_ApplCriticality_List";
        public const string SP_APPL_CRITICALITY_DOESEXIST = "iAssetTrack_Sp_ApplCriticality_DoesExist";

        //Application Map
        public const string SP_APPLMAP_UPDATE = "iAssetTrack_Sp_ApplicationMap_Update";
        public const string SP_APPLMAP_DELETE = "iAssetTrack_sp_ApplicationMap_Delete";
        public const string SP_APPLMAP_LIST = "iAssetTrack_Sp_ApplMap_List";
        //public const string SP_APPLMAP_LIST_BY_ASSETID = "iAssetTrack_Sp_ApplMapByAssetID_List";
        public const string SP_APPLMAP_LIST_BY_HOSTID = "iAssetTrack_Sp_ApplMapByHostID_List";

        //AssetHostAssignment
        public const string SP_AHA_UPDATE = "iAssetTrack_Sp_AHA_Update";
        public const string SP_AHA_DELETE = "iAssetTrack_sp_AHA_Delete";
        public const string SP_AHA_LIST = "iAssetTrack_Sp_AHA_List";
        public const string SP_AHA_LIST_BY_ASSET_HOST_IDS = "iAssetTrack_Sp_AHA_ByAssetandHost_List";

        //


        //Site
        public const string SP_SITE_UPDATE = "iAssetTrack_Sp_Site_Update";
        public const string SP_SITE_DELETE = "iAssetTrack_Sp_Site_Delete";
        public const string SP_SITE_LIST = "iAssetTrack_Sp_Site_List";
        public const string SP_SITE_ALL_LIST = "iAssetTrack_Sp_Site_All_List";
        public const string SP_SITEBYSITENAME_LIST = "iAssetTrack_Sp_SiteBySiteName_List";
        public const string SP_SITEBYBU_LIST = "iAssetTrack_Sp_SiteByBusinessUnit_List";
        #region v3.8
        public const string SP_SITE_LIST_BY_RESTRICTION = "iAssetTrack_Sp_Site_List_by_User_Restriction";
        #endregion
        public const string SP_SITE_BY_VERSION_LIST = "iAssetTrack_Sp_SiteByVersion_List";
        public const string SP_AVAILSITE_LIST = "iAssetTrack_Sp_AvailSites_List";
        public const string SP_ASSIGNSITE_LIST = "iAssetTrack_Sp_AssignSites_List";
        public const string SP_SITE_DOESEXIST = "iAssetTrack_Sp_Site_DoesExist";

        // Department
        public const string SP_DEPARTMENT_UPDATE = "iAssetTrack_Sp_Department_Update";
        public const string SP_DEPARTMENT_DELETE = "iAssetTrack_Sp_Department_Delete";
        public const string SP_DEPARTMENT_LIST = "iAssetTrack_Sp_Department_List";
        public const string SP_DEPARTMENTBYBU_LIST = "iAssetTrack_Sp_DepartmentByBusinessUnit_List";
        public const string SP_AVAILDEPARTMENT_LIST = "iAssetTrack_Sp_AvailDepartments_List";
        public const string SP_ASSIGNDEPARTMENT_LIST = "iAssetTrack_Sp_AssignDepartments_List";
        public const string SP_DEPARTMENT_DOESEXIST = "iAssetTrack_Sp_Department_DoesExist";

        //Location
        public const string SP_LOCATION_UPDATE = "iAssetTrack_Sp_Location_Update";
        public const string SP_LOCATION_DELETE = "iAssetTrack_Sp_Location_Delete";
        public const string SP_LOCATION_LIST = "iAssetTrack_Sp_Location_List";
        public const string SP_LOCATION_ALL_LIST = "iAssetTrack_Sp_Location_All_List";
        //public const string SP_AVAILLOCATION_LIST = "iAssetTrack_Sp_AvailLocations_List";
        //public const string SP_ASSIGNLOCATION_LIST = "iAssetTrack_Sp_AssignLocations_List";
        public const string SP_AVAILLOCATION_LIST = "iAssetTrack_Sp_AvailLocations_List_New";
        public const string SP_ASSIGNLOCATION_LIST = "iAssetTrack_Sp_AssignLocations_List_New";
        public const string SP_ASSIGNLOCATION_ROOMS_ONLY_LIST = "iAssetTrack_Sp_AssignLocationsRoomsOnly_List";
        public const string SP_LOCATION_DOESEXIST = "iAssetTrack_Sp_Location_DoesExist";
        public const string SP_LOCATIONBYSITE_LIST = "iAssetTrack_Sp_LocationBySite_List";
        public const string SP_LOCATIONBYLOCATION_LIST = "iAssetTrack_Sp_LocationByLocation_List";
        public const string SP_ASSET_COUNT_BY_ROOM = "iAssetTrack_Sp_Location_AssetCountByRoom";
        public const string SP_CHECK_SPECIALROOM = "iAssettrack_Sp_Check_SpecialRoom";
        public const string SP_LOCATION_LIST_WITH_UPOSITIONS = "iAssettrack_sp_UPositions_List";
        public const string SP_LOCATION_LIST_WITH_UPOSITIONS_UPDATE = "iAssettrack_sp_Uposition_Details_Update";
        public const string SP_GET_LOCATION_PATH = "iAssetTrack_sp_Report_Location_Details_By_Site_And_Location";
        public const string SP_GET_RACK_POSITIONS = "iAssettrack_sp_Rack_Positions_List";
        public const string SP_VALIDATE_RACK_POSITIONS = "iAssettrack_sp_Validate_Rack_Position";
        public const string SP_LOCATION_IS_PART_OF_DISPOSE_DECOM_ROOMS = "iAssettrack_sp_Location_part_of_Decom_Dispose_Room";
        public const string SP_LOCATION_IS_A_TENANT_LOCATION = "iAssetTrack_Sp_IsATenantLocation";
        public const string SP_LOCATION_HAS_CHILD_NODES = "iAssetTrack_Sp_Location_HasChildNodes";
        


        // Location Type
        public const string SP_LOCATIONTYPE_UPDATE = "iAssetTrack_Sp_LocationType_Update";
        public const string SP_LOCATIONTYPE_DELETE = "iAssetTrack_Sp_LocationType_Delete";
        public const string SP_LOCATIONTYPE_LIST = "iAssetTrack_Sp_LocationType_List";
        public const string SP_AVAILLOCATIONTYPE_LIST = "iAssetTrack_Sp_AvailLocationTypes_List";
        public const string SP_ASSIGNLOCATIONTYPE_LIST = "iAssetTrack_Sp_AssignLocationTypes_List";
        public const string SP_LOCATIONTYPE_DOESEXIST = "iAssetTrack_Sp_LocationType_DoesExist";

        //Host

        public const string SP_HOST_LIST = "iAssetTrack_Sp_Host_List";
        public const string SP_HOST_DOESEXIST = "iAssetTrack_Sp_Host_DoesExist";
        public const string SP_HOST_UPDATE = "iAssetTrack_Sp_Host_Update";
        public const string SP_HOST_DELETE = "iAssetTrack_Sp_Host_Delete";
        public const string SP_HOST_LOCATION_BY_HOST = "iAssetTrack_Sp_Host_GetLocByHost";


        //*Added on 09-Jan-2013
        //Global Parameters
        public const string SP_GLPARAMS_LIST = "iAssetTrack_Sp_GL_List";
        public const string SP_GLPARAMS_DOESEXIST = "iAssetTrack_Sp_GLParams_DoesExist";
        public const string SP_GLPARAMS_UPDATE = "iAssetTrack_Sp_GLParams_Update";
        public const string SP_GLPARAMS_DELETE = "iAssetTrack_Sp_GLParams_Delete";
        public const string SP_UOMPerUOM = "iAssetTrack_Sp_UOMPerUOM_List";
        public const string SP_Measure = "iAssetTrack_Sp_Measure_List";

        //*
        //RackDetailsBAL
        //v3.9
        public const string SP_RACK_DETAILS_UPDATE = "iAssetTrack_Sp_RackDetails_Update";

        //AppetailsBAL
        //v3.9
        public const string SP_APP_DETAILS_UPDATE = "iAssetTrack_Sp_AppDetails_Update";


        //Asset Group
        public const string SP_ASSETGROUP_UPDATE = "iAssetTrack_Sp_AssetGroup_Update";
        public const string SP_ASSETGROUP_DELETE = "iAssetTrack_Sp_AssetGroup_Delete";
        public const string SP_ASSETGROUP_LIST = "iAssetTrack_Sp_AssetGroup_List";
        public const string SP_ASSETGROUP_ALL_LIST = "iAssetTrack_Sp_AssetGroup_All_List";
        //Added on 08-Nov-2012
        public const string SP_ASSIGNEDASSETGROUP_LIST = "iAssetTrack_Sp_AssignedAssetGroup_List";
        public const string SP_ASSIGN_ASSETTYPES = "iAssetTrack_Sp_AssignAssetTypes";

        //Tech. Category
        public const string SP_TECHCAT_UPDATE = "iAssetTrack_Sp_TechCat_Update";
        public const string SP_TECHCAT_DELETE = "iAssetTrack_Sp_TechCat_Delete";
        public const string SP_TECHCAT_LIST = "iAssetTrack_Sp_TechCat_List";
        public const string SP_TECHCAT_DOESEXIST = "iAssetTrack_Sp_TechCat_DoesExist";
        public const string SP_ASSETGROUP_DOESEXIST = "iAssetTrack_SP_ASSETGROUP_DOESEXIST";

        //TECH CAT VARIABLES -- KJB 0N 23 JAN 2013
        public const string SP_TECHCAT_VARIABLES_UPDATE = "iAssetTrack_sp_TechCatVariables_Update";

        // (Redundant) public const string SP_ASSETGROUP_LISTBYUSERID = "iAssetTrack_Sp_AssetGroup_ListByUserID";
        public const string Sp_TechCat_Name_byAssetID = "iAssetTrack_Sp_TechCat_Name_byAssetID";


        // Mustering Reasons
        public const string SP_MUSTERREASON_UPDATE = "iAssetTrack_Sp_MusterReason_Update";
        public const string SP_MUSTERREASON_DELETE = "iAssetTrack_Sp_MusterReason_Delete";
        public const string SP_MUSTERREASON_LIST = "iAssetTrack_Sp_MusterReason_List";
        public const string SP_MUSTERREASON_DOESEXIST = "iAssetTrack_Sp_MusterReason_DoesExist";

        // AuditCycle
        public const string SP_AUDITCYCLE_UPDATE = "iAssetTrack_Sp_AuditCycle_Update";
        public const string SP_AUDITCYCLE_DELETE = "iAssetTrack_Sp_AuditCycle_Delete";
        public const string SP_AUDITCYCLE_LIST = "iAssetTrack_Sp_AuditCycle_List";
        public const string SP_AUDITCYCLE_DOESEXIST = "iAssetTrack_Sp_AuditCycle_DoesExist";


        //BU Site Assignment
        public const string SP_REMOVESITESFROMBU = "iAssetTrack_Sp_RemoveSitesFromBU";
        public const string SP_ASSIGNSITETOBU = "iAssetTrack_Sp_AssignSiteToBU";

        public const string SP_GETASSIGNEDSITESTOBU = "iAssetTrack_Sp_GetAssignedSitesToBU";
        public const string SP_GETUNASSIGNEDSITESTOBU = "iAssetTrack_Sp_GetUnAssignedSitesToBU";


        public const string SP_BUSITEASSIGNMENT_UPDATE = "iAssetTrack_Sp_BUSiteAssignment_Update";
        public const string SP_BUSITEASSIGNMENT_LIST = "iAssetTrack_Sp_BUSiteAssignment_List";

        // BU Department Assignment

        public const string SP_REMOVEDEPARTMENTSFROMBU = "iAssetTrack_Sp_RemoveDepartmentsFromBU";
        public const string SP_ASSIGNDEPARTMENTTOBU = "iAssetTrack_Sp_AssignDepartmentToBU";

        public const string SP_GETASSIGNEDDEPARTMENTSTOBU = "iAssetTrack_Sp_GetAssignedDepartmentsToBU";
        public const string SP_GETUNASSIGNEDDEPARTMENTSTOBU = "iAssetTrack_Sp_GetUnAssignedDepartmentsToBU";


        public const string SP_BUDEPARTMENTASSIGNMENT_UPDATE = "iAssetTrack_Sp_BUDepartmentAssignment_Update";
        public const string SP_BUDEPARTMENTASSIGNMENT_LIST = "iAssetTrack_Sp_BUDepartmentAssignment_List";


        //public const string SP_SITELOCATIONASSIGNMENT_UPDATE = "iAssetTrack_Sp_SiteLocationAssignment_Update";
        public const string SP_SITELOCATIONASSIGNMENT_UPDATE = "iAssetTrack_Sp_SiteLocationAssignment_Update_New";
        public const string SP_SITELOCATIONASSIGNMENT_LIST = "iAssetTrack_Sp_SiteLocationAssignment_List";
        //public const string SP_SITELOCATIONASSIGNMENT_LIST_ALL = "iAssetTrack_Sp_SiteLocationAssignment_List_All";


        //Site Location Assignment
        public const string SP_REMOVELOCATIONSFROMSITE = "iAssetTrack_Sp_RemoveLocationFromSite";
        public const string SP_ASSIGNLOCATIONTOSITE = "iAssetTrack_Sp_AssignLocationToSite";
        public const string SP_GETASSIGNEDLOCATIONSTOSITE = "iAssetTrack_Sp_GetAssignedLocationsToSites";
        public const string SP_GETUNASSIGNEDLOCATIONSTOSITE = "iAssetTrack_Sp_GetUnAssignedLocationsToSite";


        //User Group
        public const string SP_GROUP_UPDATE = "iAssetTrack_Sp_Group_Update";
        public const string SP_GROUP_DELETE = "iAssetTrack_Sp_Group_Delete";
        public const string SP_GROUP_LIST = "iAssetTrack_Sp_Group_List";
        public const string SP_GROUP_LISTBYBU = "iAssetTrack_Sp_Group_ListByBU";
        public const string SP_GROUP_LISTBYUSERTYPE = "iAssetTrack_Sp_Group_ListByUserType";
        public const string SP_ASSIGNGROUP_LIST = "iAssetTrack_Sp_AssignGroup_List";
        // (Redundant) public const string SP_ASSIGNCOSTCENTER_LIST = "iAssetTrack_Sp_CostCenterList_ByUserID";
        public const string SP_GROUP_DOESEXIST = "iAssetTrack_Sp_Group_DoesExist";
        public const string SP_GROUP_USERLIST = "iAssetTrack_Sp_User_ListBy_Group";
        

        //Module
        // (Redundant) public const string SP_MODULE_UPDATE = "iAssetTrack_Sp_Module_Update";
        // (Redundant) public const string SP_MODULE_DELETE = "iAssetTrack_Sp_Module_Delete";
        // (Redundant) public const string SP_MODULE_LIST = "iAssetTrack_Sp_Module_List";
        // (Redundant) public const string SP_ASSIGNMODULE_LIST = "iAssetTrack_Sp_AssignModule_List";
        // (Redundant) public const string SP_MODULE_DOESEXIST = "iAssetTrack_Sp_Module_DoesExist";

        //Assign Rights
        public const string SP_USER_ASSIGN_RIGHTS = "iAssetTrack_Sp_User_AssignRights";

        //AccessRights
        // (Redundant)public const string SP_ACCESSRIGHTS_UPDATE = "iAssetTrack_Sp_Rights_Update";
        // (Redundant)public const string SP_ACCESSRIGHTS_DELETE = "iAssetTrack_Sp_Rights_Delete";
        public const string SP_ACCESSRIGHTS_LIST = "iAssetTrack_Sp_Rights_List";
        public const string SP_AVAILACCESSRIGHTS_LIST = "iAssetTrack_Sp_AvailAccessRights_List";
        public const string SP_ASSIGNACCESSRIGHTS_LIST = "iAssetTrack_Sp_AssignAccessRights_List";
        // (Redundant)public const string SP_ACCESSRIGHTS_DOESEXIST = "iAssetTrack_Sp_Rights_DoesExist";

        // Group RightsAssignment
        // (Redundant) public const string SP_GROUPACCESSRIGHTSASSIGNMENT_UPDATE = "iAssetTrack_Sp_GroupRightsAssignment_Update";
        // (Redundant) public const string SP_GROUPACCESSRIGHTSASSIGNMENT_LIST = "iAssetTrack_Sp_GroupAccessRightsAssignment_List";


        // Module Rights Assignment
        // (Redundant) public const string SP_MODULERIGHTSASSIGNMENT_UPDATE = "iAssetTrack_Sp_ModuleRightsAssignment_Update";
        // (Redundant)public const string SP_MODULERIGHTSASSIGNMENT_LIST = "iAssetTrack_Sp_ModuleRights_List";

        # region v3.8 SP
        //v3.8
        //SiteRestrictions -- 03 Nov 2013
        public const string SP_SITE_RESTRICTIONS_UPDATE = "iAssetTrack_Sp_SiteRestrictions_Update";
        public const string SP_SITE_RESTRICTIONS_DELETE = "iAssetTrack_Sp_SiteRestrictions_Delete";
        public const string SP_SITE_RESTRICTIONS_LIST_BY_USERID = "iAssetTrack_sp_SiteRestriction_List_byUserID";

        //City
        public const string SP_CITY_LIST_BY_COUNTRY = "iAssetTrack_Sp_City_List_By_Country";

        //Country
        public const string SP_COUNTRY_LIST_BY_REGION = "iAssetTrack_Sp_Country_List_By_Region";

        #endregion
        //User
        public const string SP_USER_UPDATE = "iAssetTrack_Sp_User_Update";
        public const string SP_MANAGE_USERS = "iAssetTrack_Sp_Manage_Users";
        public const string SP_MANAGE_USERSRESETPASSWORD = "iAssetTrack_Sp_User_ResetPassword";
        public const string SP_MANAGE_USERS_LIST = "iAssetTrack_Sp_ManageUser_List";
        public const string SP_USERS_DETAILSBYUSERID = "iAssetTrack_Sp_User_DetailsByUserID";
        public const string SP_USERS_IS_A_TENANT_USER = "iAssetTrack_Sp_IsATenantUser";
        


        //V00001
        public const string SP_USER_DELETE = "iAssetTrack_Sp_User_Delete";
        public const string SP_USER_LIST = "iAssetTrack_Sp_User_List";
        public const string SP_USER_LOGIN = "iAssetTrack_Sp_User_Login";
        public const string SP_USER_LOGINBYUSERNAME = "iAssetTrack_Sp_User_LoginByUserName";
        public const string SP_USER_LOGIN_ATTEMPT_UPDATE = "iAssetTrack_Sp_Manage_Users_LoginAttempt_Update";
        public const string SP_LOCK_USER = "iAssetTrack_Sp_Manage_Users_Lock_User";
        public const string SP_RESET_USER = "iAssetTrack_Sp_Manage_Users_Reset_User";
        public const string SP_USER_GROUPBYUSERNAME = "iAssetTrack_Sp_User_GroupByUserName";
        public const string SP_USERBU_DoesExist = "iAssetTrack_Sp_UserBU_DoesExist";

        // (Redundant) public const string SP_AVAILUSER_LIST = "iAssetTrack_Sp_AvailUser_List";
        // (Redundant)public const string SP_ASSIGNUSER_LIST = "iAssetTrack_Sp_AssignUser_List";
        public const string SP_USER_DOESEXIST = "iAssetTrack_Sp_User_DoesExist";
        public const string SP_USER_CHANGEPASSWORD = "iAssetTrack_Sp_User_ChangePassword";
        public const string SP_USER_LIST_BY_BUSINESSUNIT = "iAssetTrack_Sp_User_List_By_BusinessUnit";
        // (Redundant) public const string SP_RFIDCARDASSIGNMENTUSER_UPDATE = "iAssetTrack_Sp_RFIDCardAssignmentUser_Update";
        public const string SP_USER_HISTORY = "iAssetTrack_Sp_User_History";
        public const string SP_USER_SEARCH = "iAssetTrack_Sp_User_Search";
        public const string SP_UPDATE_IS_FIRST_LOGIN = "iAssetTrack_Sp_IsFirstLogin_Update";


        // Group Module Rights Assignment
        public const string SP_GROUPMODULERIGHTSASSIGNMENT_UPDATE = "iAssetTrack_Sp_GroupModuleRightsAssignment_Update";
        public const string SP_GROUPMODULERIGHTSASSIGNMENT_LIST = "iAssetTrack_Sp_GroupModuleRights_List";
        // (Redundant) public const string SP_GROUPACCESSRIGHTSASSIGNMENT_DELETE = "iAssetTrack_Sp_GroupAccessRightsAssignment_Delete";
        public const string SP_GROUPMODULERIGHTS_LIST = "iAssetTrack_Sp_RetrieveGroupModuleRight";

        //Role
        // (Redundant) public const string SP_ROLE_UPDATE = "iAssetTrack_Sp_Role_Update";
        // (Redundant) public const string SP_RIGHTS_LIST = "usp_SecRightList";
        // (Redundant) public const string SP_ROLE_DELETE = "iAssetTrack_Sp_Role_Delete";
        // (Redundant) public const string SP_ROLE_LIST = "usp_GetRoleByRoleID";
        // (Redundant) public const string SP_ROLE_DOESEXIST = "iAssetTrack_Sp_Role_DoesExist";

        //Audit Login
        public const string SP_AUDITLOGINLOGOUT = "iAssetTrack_Sp_AuditLoginLogout";

        // BU Site User Assignment
        // (Redundant)public const string SP_BUSITEUSERASSIGNMENT_UPDATE = "iAssetTrack_Sp_BUSiteUserAssignment_Update";
        // (Redundant) public const string SP_BUSITEUSERASSIGNMENT_LIST = "iAssetTrack_Sp_BUSiteUserAssignment_List";


        //RFID Card
        public const string SP_RFIDCARDASSIGNMENT_UPDATE = "iAssetTrack_Sp_RFIDCardAssignment_Update";
        // (Redundant) public const string SP_RFIDCARDASSIGNMENT_LIST = "iAssetTrack_Sp_RFIDCardAssignment_List";

        //Error
        // (Redundant)public const string SP_ERROR_UPDATE = "iAssetTrack_Sp_Error_Update";
        // (Redundant)public const string SP_ERROR_DISPLAY = "iAssetTrack_Sp_Error_Display";

        //Message
        // (Redundant) public const string SP_MESSAGE_UPDATE = "iAssetTrack_Sp_Message_Update";
        public const string SP_MESSAGE_DISPLAY = "iAssetTrack_Sp_Message_Display";

        //SPC
        public const string SPC_Details_BySiteID = "iAssetTrack_Sp_SPC_Details_BySiteID";
        public const string SPC_MFG_ByTechID = "iAssetTrack_Sp_SPC_MFG_ByTechID";
        public const string SPC_Model_ByMfgID = "iAssetTrack_Sp_SPC_Model_ByMfgID";

        // Asset
        //public const string SP_ASSET_UPDATE = "iAssetTrack_Sp_Asset_Update";//Commented on 06May2013
        public const string SP_ASSET_UPDATE = "iAssetTrack_Sp_Asset_UpdateNew";//Added on 06May2013
        public const string SP_ASSET_VIEWDETAILS_BYASSETID = "iAssetTrack_Sp_Asset_ViewDetails_ByAssetID";
        public const string SP_ASSET_VIEWDETAILSMODIFYALL_BYASSETID = "iAssetTrack_Sp_Asset_ViewDetailsModifyAll_ByAssetID";
        public const string SP_BLADE_ASSET_UPDATE = "iAssetTrack_Sp_Blade_Asset_Update";//21Oct2014

        public const string SP_ASSET_DETAILS_BYASSETID = "iAssetTrack_Sp_Asset_Details_ByAssetID";
        public const string SP_ASSET_SELECTED_DETAILS_BYASSETID = "iAssetTrack_Sp_Asset_Selected_Details_ByAssetID";
        public const string SP_ASSET_DETAILS_BYASSETNO = "iAssetTrack_Sp_Asset_Details_ByAssetNo";
        public const string SP_ASSET_DETAILS_BY_SERIALNO_ASSETTAG = "iAssetTrack_Sp_Asset_Details_BySerialNo_AssetTag";//20Oct2014
        public const string SP_ASSET_APP_MAP_EXISTS = "iAssetTrack_Sp_AssetAppMap_Exists";//06Nov2014
        public const string SP_ASSET_BARRING = "iAssetTrack_Sp_Asset_Barring";
        public const string SP_ASSET_UNBARRING = "iAssetTrack_Sp_Asset_UnBarring";
        public const string SP_ASSET_RESTRICTION = "iAssetTrack_Sp_Asset_Restriction";
        public const string SP_ASSET_WRITEOFF = "iAssetTrack_Sp_Asset_Writeoff_Update";
        public const string SP_ASSET_DERESTRICTION = "iAssetTrack_Sp_Asset_DeRestriction";
        public const string SP_ASSET_DEWRITEOFF = "iAssetTrack_Sp_Asset_DeWriteoff";
        public const string SP_ASSET_RESTRICTION_DETAILS = "iAssetTrack_Sp_Asset_Restrict_Details";
        public const string SP_ASSET_WRITEOFF_DETAILS = "iAssetTrack_Sp_Asset_WriteOff_Details";
        public const string SP_ASSET_MUSTER = "iAssetTrack_Sp_Asset_Muster";
        // (Redundant)public const string SP_ASSET_DELETE = "iAssetTrack_Sp_Asset_Delete";
        // (Redundant) public const string SP_ASSET_GENERATEEPC = "iAssetTrack_Sp_Asset_GenerateEPC";
        // (Redundant) public const string SP_ASSET_CHECKOUT = "iAssetTrack_Sp_Asset_CheckOut";
        // (Redundant) public const string SP_CHECKOUT_SESSION = "iAssetTrack_Sp_CheckOutSession_Create";
        public const string SP_ASSET_GETBYPAGE = "iAssetTrack_Sp_Asset_Search_BYPage";
        public const string SP_ASSET_GETBYPAGE_FOR_EXPORT = "iAssetTrack_Sp_Asset_Search_BYPage_ForExport";
        public const string SP_ASSET_BARRING_DETAILS = "iAssetTrack_Sp_Asset_Barring_Details";
        public const string SP_ASSET_WITH_APPLICATION_GETBYPAGE = "iAssetTrack_Sp_AssetsByLocations_withApplications_Search_BYPage";
        public const string SP_ASSET_CHECK_CHILD_ASSET_DECOM_STATUS = "iAssettrack_sp_Check_Child_Assets_Decomm_Status";
        public const string SP_ASSET_CHECK_CHILD_ASSET_WRITEOFF_STATUS = "iAssettrack_sp_Check_Child_Assets_WriteOff_Status";

        // (Redundant) public const string SP_ASSET_QUERY_BYDEPT = "iAssetTrack_Sp_AssetQuery_ByDept";
        public const string SP_ASSET_HISTORY = "iAssetTrack_Sp_Asset_History";
        public const string SP_ASSET_ALERT_DETAILS = "iAssetTrack_Sp_AlertView";
        public const string SP_ASSET_RETRIEVE_BYID = "iAssetTrack_Sp_Asset_DetailsByAssetId";
        public const string SP_ASSET_RETRIEVE_STATUS = "iAssetTrack_Sp_Asset_RetrieveStatus";
        public const string SP_ASSETLIST_GETBYLOCATIONID = "iAssetTrack_Sp_Asset_Details_ByLocationID";
        public const string SP_ENCL_POSITIONS_LIST = "iAssettrack_sp_Enclosure_Positions_List";
        public const string SP_BLADE_POSITIONS_UPDATE = "iAssetTrack_sp_Blade_Positions_Update";


        //SPC Model MAP
        public const string SP_SPCMODELMAP_GETDATA = "iAssetTrack_sp_SPCModelMap_GetData";
        public const string SP_SPCMODELMAP_GETDATA_BY_FILTER = "iAssetTrack_sp_SPCModelMap_GetData_By_Filter";
        public const string SP_SPCMODELMAP_DATA_FOR_CHARTS = "iAssetTrack_sp_SPCModelMap_Data_For_Charts";
        public const string SP_SPCMODELMAP_GETDATA_BY_MFGMODEL = "iAssetTrack_sp_SPCModelMap_GetData_By_Manufacturer_Model";


        // Purpose
        public const string SP_PURPOSE_UPDATE = "iAssetTrack_Sp_Purpose_Update";
        public const string SP_PURPOSE_DELETE = "iAssetTrack_Sp_Purpose_Delete";
        public const string SP_PURPOSE_LIST = "iAssetTrack_Sp_Purpose_List";
        public const string SP_PURPOSE_DOESEXIST = "iAssetTrack_Sp_Purpose_DoesExist";
        public const string SP_PURPOSEBYBU_LIST = "iAssetTrack_Sp_PurposeByBusinessUnit_List";

        // Customer---Added on 03-Sep-2012
        public const string SP_CUSTOMER_UPDATE = "iAssetTrack_Sp_Customer_Update";
        public const string SP_CUSTOMER_DELETE = "iAssetTrack_Sp_Customer_Delete";
        public const string SP_CUSTOMER_LIST = "iAssetTrack_Sp_Customer_List";
        public const string SP_CUSTOMER_DOESEXIST = "iAssetTrack_Sp_Customer_DoesExist";
        public const string SP_INDUSTRYTYPE_LIST = "iAssetTrack_Sp_IndustryType_List";
        public const string SP_COST_COUNTRY_LIST = "Sp_Cost_Country_List";
        public const string SP_COST_STATE_LIST_BY_COUNTRY = "Sp_Cost_State_List_ByCountry";
        public const string SP_COST_CITY_LIST_BY_COUNTRY = "Sp_Cost_City_List_ByCountry";
        public const string SP_CUSTOMER_SEARCH = "iAssetTrack_Sp_Customer_Search";
        public const string SP_CUSTOMERDETAILS_SEARCH = "iAssetTrack_Sp_CustomerDetails_Search";
        public const string SP_ADDRESSTYPE_LIST = "iAssetTrack_Sp_AddressType_List";

        //V00008 by Vidya on 9-Oct-2012
        public const string SP_ADDRESS_LIST = "iAssetTrack_Sp_Address_List";
        // public const string SP_COST_STATE_LIST = "Sp_Cost_State_List";

        ///Project
        ///
        public const string SP_PROJECT_UPDATE = "iAssetTrack_Sp_Project_Update";
        public const string SP_PROJECT_DELETE = "iAssetTrack_Sp_Project_Delete";
        public const string SP_PROJECT_LIST = "iAssetTrack_Sp_Project_List";
        public const string SP_PROJECT_DOESEXIST = "iAssetTrack_Sp_Project_DoesExist";
        public const string SP_PROJECTDETAILS_SEARCH = "iAssetTrack_Sp_ProjectDetails_Search";
        public const string SP_PROJECT_SEARCH = "iAssetTrack_Sp_Project_Search";
        public const string SP_PROJECT_LIST_BYCOUNTRYID = "iAssetTrack_Sp_Project_List_ByCountryID";

        //Added on 11-Dec-2012
        //SPC Version
        public const string SP_SPCVERSION_PARAMS = "iAssetTrack_Sp_SPCVersion_Params";
        public const string SP_SPCVERSION = "iAssetTrack_sp_Create_SPC_Header";
        public const string SP_SPCPROJECTDETAILS = "iAssetTrack_sp_SPC_ProjectDetails";
        public const string SP_SPCSitesByBU = "iAssetTrack_sp_SPC_SitesByBU";
        public const string SP_SPCHEADER_LIST = "iAssetTrack_sp_SPC_HeaderList";
        // kjb on 22 Jan 2013
        public const string SP_UPDATE_SPCVERSION = "iAssetTrack_sp_SPC_Header_Update";


        //by kjb on 14 Jan 2013
        public const string SP_VERSION_LIST_BY_BU = "iAssetTrack_sp_SPC_VersionList_By_BU";
        public const string SP_SPCHEADER_LIST_BY_HEADERID = "iAssetTrack_sp_SPC_HeaderList_By_HeaderID";
        public const string SP_TECHCAT_COMPLETE_LIST_BY_HEADERID = "iAssetTrack_sp_Tech_Main_Sub_Groups_List ";
        //by kjb on 17 Jan 2013
        public const string SP_YEAR_LIST_BY_HEADERID = "iAssetTrack_sp_GetYears_By_SPCHeader";

        //Added on 18-Dec-2012
        //SPC Version
        public const string SP_MODELS_BYHEADERID = "iAssetTrack_sp_SPC_ModelsByHeaderID";

        //Added on 19-Dec-2012
        //Custom Groups Default
        public const string SP_CUSTOMGROUPSDEFAULT = "iAssetTrack_sp_Create_Custom_Groups_Default";

        //SPC MainGroups
        public const string SP_CUSTOMGROUPSLIST = "iAssetTrack_sp_Custom_Groups_List";
        public const string SP_SPCMAINGROUPS_UPDATE = "iAssetTrack_Sp_SPCMainGroups_Update";

        // kjb on 16 Jan 2013
        public const string SP_SPC_MAIN_SUB_GROUPS_LIST = "iAssetTrack_Sp_SPCMainSubGroups_List";


        //ModelCountBAL 
        // kjb on 18 Jan 2013
        public const string SP_SPC_Model_LIST_FOR_FACILITY_DETAILS = "iAssetTrack_sp_Get_ModelData_For_FacilityDetails";
        public const string SP_MODELCOUNT_SPLIT_COMBINE = "iAssetTrack_sp_SPC_Split_Combine_Data";
        public const string SP_MODELCOUNT_UPDATE = "iAssetTrack_sp_SPC_ModelCount_Data_Update";
        public const string SP_MODELCOUNT_LIST_BY_SPC_HEADER = "iAssetTrack_sp_ModelCountList_By_SPC_Header";


        //SPC SubGroups
        public const string SP_CUSTOMSUBGROUPSLIST = "iAssetTrack_sp_Custom_SubGroups_List";
        public const string SP_SPCSUBGROUPS_UPDATE = "iAssetTrack_Sp_SPCSubGroups_Update";
        public const string SP_SPCSUBGROUPS_DELETE = "iAssetTrack_Sp_SPCSubGroups_Delete";
        public const string SP_CUSTOMSUBGROUPSBYSUBGROUPSLIST = "iAssetTrack_Sp_SubGroupsBySubGroups_List";

        //SPC SubGroupModelsMap
        public const string SP_SUBGROUPSMODELSMAP = "iAssetTrack_sp_SPCSubGroupsModelMap_Add";

        //*Added on 17Apr2013
        public const string SP_PARENTASSET_SEARCH = "iAssetTrack_Sp_ParentAsset_Search";

        //v454
        //Owner
        public const string SP_OWNER_UPDATE = "iAssetTrack_Sp_Owner_Update";
        public const string SP_OWNER_LIST = "iAssetTrack_Sp_Owner_List";
        public const string SP_OWNER_DOESEXIST = "iAssetTrack_Sp_Owner_DoesExist";
        public const string SP_OWNER_DELETE = "iAssetTrack_Sp_Owner_Delete";
        public const string SP_OWNER_LIST_BY_SEARCH = "iAssetTrack_Sp_Owner_List_By_Search";

        //AirFlowDirection
        public const string SP_AIRFLOWDIRECTION_LIST = "iAssetTrack_Sp_AirFlowDirection_List";

        //MountType
        public const string SP_MOUNTTYPE_LIST = "iAssetTrack_Sp_MountType_List";

        //Orientation
        public const string SP_ORIENTATION_LIST = "iAssetTrack_Sp_Orientation_List";

        //App Status
        public const string SP_AppStatus_LIST = "iAssetTrack_Sp_AppStatus_List";

        //InputConnectorType
        public const string SP_INPUT_CONNECTOR_LIST = "iAssetTrack_Sp_InputConnectorType_List";

        //OutputConnectorType
        public const string SP_OUTPUT_CONNECTOR_LIST = "iAssetTrack_Sp_OutputConnectorType_List";
        //Tenant
        public const string SP_TENANT_UPDATE = "iAssetTrack_Sp_Tenant_Update";
        public const string SP_TENANT_LIST = "iAssetTrack_Sp_Tenant_List";
        public const string SP_TENANT_DOESEXIST = "iAssetTrack_Sp_Tenant_DoesExist";
        public const string SP_TENANT_DELETE = "iAssetTrack_Sp_Tenant_Delete";
        public const string SP_TENANT_LOCATION_LIST = "iAssetTrack_Sp_Tenant_LocationList";
        public const string SP_TENANT_ASSIGNED_USERS = "iAssetTrack_Sp_Tenant_AssignedUsers";
        public const string SP_TENANT_GROUP_ASSIGNMENT_LIST = "iAssetTrack_Sp_Tenant_Group_Assignment_List";
        public const string SP_TENANT_LOCATION_ASSIGNMENT_LIST = "iAssetTrack_Sp_Tenant_Assigned_Location_List";
        public const string SP_TENANT_DIVISION_ASSIGNMENT_LIST = "iAssetTrack_Sp_Tenant_Division_Assignment_List";
        public const string SP_TENANT_OWNER_ASSIGNMENT_LIST = "iAssetTrack_Sp_Tenant_Owner_Assignment_List";
        public const string SP_TENANT_APPLICATION_ASSIGNMENT_LIST = "iAssetTrack_Sp_Tenant_Application_Assignment_List";
        public const string SP_TENANT_HOST_ASSIGNMENT_LIST = "iAssetTrack_Sp_Tenant_Host_Assignment_List";
        public const string SP_TENANT_ASSET_ASSIGNMENT_LIST = "iAssetTrack_Sp_Tenant_Asset_Assignment_List";
        

        //TenantAsset
        public const string SP_TENANT_ASSET_UPDATE = "iAssetTrack_Sp_Tenant_Asset_Update";
        public const string SP_TENANT_ASSET_LIST = "iAssetTrack_Sp_Tenant_Get_AssetList";

    };

    public class Parameters
    {
        //v3.8
        //SiteRestrictions
        public const string PARAM_READ_PERMISSION = "@pBitReadPermission";
        public const string PARAM_WRITE_PERMISSION = "@pBitWritePermission";

        //General

        public const string PARAM_EXCEPTION_TYPE = "@ExceptionType";

        //Audit Details
        public const string PARAM_RESULT = "@pIntResult";
        public const string PARAM_RETURNVALUE = "@pIntReturnValue";
        public const string PARAM_STATUS = "@pBitStatus";
        public const string PARAM_UPDATEDBY = "@pIntUpdatedBy";
        public const string PARAM_CREATEDBY = "@pIntCreatedBy";
        public const string PARAM_MODIFIEDBY = "@pIntLastModifiedBy";
        public const string PARAM_DELIMITERS = "@pVarDelimiter";
        public const string PARAM_ACTION = "@pVarAction";
        public const string PARAM_COLUMN = "@pVarColumnName";
        //public const string PARAM_VALUE = "@pIntColumnValue"; // commented by kjb on 02 Sep 2011
        // to make value from int ot string to accomadate HostID, which is GUID
        public const string PARAM_VALUE = "@pStrColumnValue";
        public const string PARAM_FOREMAN = "@pVarForeman";
        public const string PARAM_IS_GUID_FLAG = "@pBitIsGuid";

        //spc rEPORTS KJB - 10 JAN 2013
        public const string PARAM_SPC_REPORTS_ID = "@pIntID";

        //
        public const string PARAM_PAGE_SIZE = "@pIntPageSize";
        public const string PARAM_PAGE_INDEX = "@pIntPageIndex";

        //Business Unit
        public const string PARAM_BUSINESSUNIT = "@pVarBusinessUnit";
        public const string PARAM_DESCRIPTION = "@pVarDescription";
        public const string PARAM_COPREFIX = "@pVarCoPrefix";
        public const string PARAM_BUSINESSUNITID = "@pIntBusinessUnitID";
        public const string PARAM_BUSINESSUNITIDs = "@pVarBusinessUnitIDs";
        public const string PARAM_FROMIA = "@pBitFromIA";

        //Device Reg -- kjb on 07 may 2012
        public const string PARAM_DEVICEID = "@pVarDeviceID";
        public const string PARAM_DEVICENAME = "@pVarDeviceName";
        //public const string PARAM_SITEID = "@pIntSiteID";
        //public const string PARAM_SITE = "@pVarSite";
        public const string PARAM_ID = "@pIntID";
        public const string PARAM_IDs = "@pVarIDs";

        //Manufacturer
        public const string PARAM_MFGNAME = "@pVarMfgName";
        public const string PARAM_MFGDESCRIPTION = "@pVarDescription";
        public const string PARAM_MFGID = "@pIntMfgID";
        public const string PARAM_MFGIDs = "@pVarMfgIDs";
        public const string PARAM_MODEL_TYPE = "@pVarModelType";
        //AssetModel
        public const string PARAM_MODELNAME = "@pVarModelName";
        public const string PARAM_MODELDESCRIPTION = "@pVarDescription";
        public const string PARAM_MODELID = "@pIntModelID";
        public const string PARAM_IS_OVERWRITE = "@pBitOverwrite";
        public const string PARAM_MODELIDs = "@pVarModelIDs";
        public const string PARAM_MODELMFGID = "@pIntMfgID";
        public const string PARAM_PARENTMODELID = "@pIntParentModelID";
        public const string PARAM_SPCID = "@pIntSpcID";
        public const string PARAM_COMMENTS = "@pVarComment";
        public const string PARAM_AM_BUID = "@pIntBuID";
        public const string PARAM_IS_BLADE = "@pBitIsBlade";
        public const string PARAM_IS_ENCLOSURE = "@pBitIsEnclosure";

        public const string PARAM_MODEL_TYPE_ID = "@pIntModelTypeID";
        public const string PARAM_MODEL_WIDTH = "@pFltWidth";
        public const string PARAM_MODEL_DEPTH = "@pFltDepth";
        public const string PARAM_MODEL_HEIGHT = "@pFltHeight";
        public const string PARAM_MODEL_UHEIGHT = "@pIntUHeight";
        public const string PARAM_MODEL_WEIGHT = "@pFltWeight";
        public const string PARAM_MODEL_MAX_POWER = "@pFltMaxPower";
        public const string PARAM_MODEL_SS_POWER = "@pFltSSPower";
        public const string PARAM_MODEL_CONN_PDU_SIDE = "@pVarConnPDUSide";
        public const string PARAM_MODEL_CONN_DEV_SIDE = "@pVarConnDevSide";
        public const string PARAM_MODEL_TOTAL_PSU_COUNT = "@pIntTotalPSUCount";
        public const string PARAM_MODEL_REQ_PSU_COUNT = "@pIntReqPSUCount";
        public const string PARAM_MODEL_MOUNT_TYPE_ID = "@pIntMountTypeID";
        public const string PARAM_MODEL_AF_DIRECTION_ID = "@pIntAFDirectionID";
        public const string PARAM_MODEL_RINTERNAL_DEPTH = "@pFltRInternalDepth";
        public const string PARAM_MODEL_RINTERNAL_HEIGHT = "@pFltRInternalHeight";
        public const string PARAM_MODEL_ENCL_FRONT_ROW_COUNT = "@pIntEnclFrontRowCount";
        public const string PARAM_MODEL_ENCL_FRONT_COL_COUNT = "@pIntEnclFrontColCount";
        public const string PARAM_MODEL_ENCL_REAR_ROW_COUNT = "@pIntEnclRearRowCount";
        public const string PARAM_MODEL_ENCL_REAR_COL_COUNT = "@pIntEnclRearColCount";
        public const string PARAM_MODEL_BLADE_ROW_COUNT = "@pIntBladeRowCount";
        public const string PARAM_MODEL_BLADE_COL_COUNT = "@pIntBladeColCount";
        public const string PARAM_MODEL_RINTERNAL_WIDTH = "@pFltRInternalWidth";

        public const string PARAM_MODEL_ENCL_MODEL_ID = "@pIntEnclModelID";
        public const string PARAM_MODEL_BLADE_MODEL_ID = "@pIntBladeModelID";
        public const string PARAM_MODEL_RETURN_VAL = "@RETURNVAL";

        //InventoryUpdate
        public const string PARAM_LOC_LIST = "@LocList";
        //Application
        public const string PARAM_APPLICATIONID = "@pIntApplID";
        public const string PARAM_APPL_BUID = "@pIntBUID";
        public const string PARAM_APPLICATIONNAME = "@pVarApplName";
        public const string PARAM_APPLICATIONDESCRIPTION = "@pVarApplDesc";
        public const string PARAM_APPLICATIONTYPEID = "@pIntApplTypeID";
        public const string PARAM_APPLICATIONSTATUS_ID = "@pIntAppStatusID";
        public const string PARAM_APPLICATIONCRITICALITY = "@pIntApplCriticality";
        public const string PARAM_APPLICATIONMANAGEID = "@pIntApplManageID";
        public const string PARAM_APPLICATIONIDs = "@pVarApplIDs";

        //APP DETAILS BAL
        //V3.9
        public const string PARAM_APPLICATION_DIVISION = "@pVarAppDivision";


        //Division
        public const string PARAM_DIVISIONID = "@pIntDivisionID";
        public const string PARAM_DIVISION_BUID = "@pIntBUID";
        public const string PARAM_DIVISIONNAME = "@pVarDivision";
        public const string PARAM_DIVISIONDESCRIPTION = "@pVarDivisionDesc";
        public const string PARAM_DIVISIONIDS = "@pVarDivisionIDs";


        //Application Type
        public const string PARAM_APPLTYPEID = "@pIntApplTypeID";
        public const string PARAM_APPLTYPE = "@pVarApplType";
        public const string PARAM_APPLTYPEDESCRIPTION = "@pVarApplTypeDesc";
        public const string PARAM_APPLTYPEIDS = "@pVarApplTypeIDs";

        //Application Criticality
        public const string PARAM_APPL_CRITICALITY_ID = "@pIntApplCriticalityID";
        public const string PARAM_APPL_CRITICALITY = "@pVarApplCriticality";
        public const string PARAM_APPL_CRITICALITY_DESCRIPTION = "@pVarApplCriticalityDesc";
        public const string PARAM_APPL_CRITICALITY_IDS = "@pVarApplCriticalityIDs";
        //-->v3.9
        public const string PARAM_APPL_CRITICALITY_BACK_COLOR_CODE = "@pVarBackColorCode";
        public const string PARAM_APPL_CRITICALITY_FORE_COLOR_CODE = "@pVarForeColorCode";


        //Application Map
        public const string PARAM_APPLMAPID = "@pIntApplMapID";
        public const string PARAM_APPLMAPAPPLID = "@pIntApplID";
        //public const string PARAM_APPLMAPASSETID = "@pIntAssetID";
        //public const string PARAM_APPLMAP_HOSTID = "@pIntHostID";
        public const string PARAM_APPLMAP_ID = "@pGuidID";
        public const string PARAM_APPLMAP_APPLIDS = "@pVarApplIDs";

        //AssetHostAssignment
        public const string PARAM_AHA_ID = "@pGuidID";
        public const string PARAM_AHA_ASSET_ID = "@pIntAssetID";
        //public const string PARAM_APPLMAPASSETID = "@pIntAssetID";
        //public const string PARAM_APPLMAP_HOSTID = "@pIntHostID";
        public const string PARAM_AHA_HOST_ID = "@pIntHostID";
        //public const string PARAM_APPLMAP_APPLIDS = "@pVarApplIDs";

        //SPCMaster
        public const string PARAM_SPCMASTER_SPCID = "@pIntSpcID";
        public const string PARAM_MAKEMODEL = "@pVarMakeModel";
        public const string PARAM_SPCIDs = "@pVarSPCIDs";
        public const string PARAM_PRODNO = "@pVarProdNo";
        public const string PARAM_DEPTH_INCHES = "@pFltDepthIn";
        public const string PARAM_DEPTH_MM = "@pFltDepthMM";
        public const string PARAM_WIDTH_INCHES = "@pFltWidthIn";
        public const string PARAM_WIDTH_MM = "@pFltWidthMM";
        public const string PARAM_HEIGHT_INCHES = "@pFltHeightIn";
        public const string PARAM_HEIGHT_MM = "@pFltHeightMM";
        public const string PARAM_WEIGHT_LB = "@pFltWeightLB";
        public const string PARAM_WEIGHT_KG = "@pFltWeightKG";
        public const string PARAM_EMPTY2 = "@pVarEmpty2";
        public const string PARAM_SQ_FT = "@pFltSqFt";
        public const string PARAM_SQ_MT = "@pFltSqMt";
        public const string PARAM_STEADY_WATTS = "@pFltSteadyWatts";
        public const string PARAM_MAX_WATTS = "@pFltMaxWatts";
        public const string PARAM_NOTE1 = "@pVarNotes1";
        public const string PARAM_NOTES2 = "@pVarNotes2";
        public const string PARAM_ITEMTYPE = "@pVarItemType";
        public const string PARAM_PATH = "@pVarPath";
        public const string PARAM_REFID = "@pVarRefID";
        public const string PARAM_SOURCE_FILE = "@pVarSourceFile";
        public const string PARAM_DEVICECOST = "@pDeviceCost";


        //Configuration
        public const string PARAM_CONFIGKEY = "@pVarConfigKey";

        //Inventory Update
        public const string PARAM_INVUPDATE_ID = "@pVarID";
        public const string PARAM_INVUPDATE_ASSETIDS = "@pStrAssetIDs";
        public const string PARAM_INVUPDATE_LOCATIONID = "@pIntLocationID";

        //Location
        public const string PARAM_LOCATION = "@pVarLocation";
        public const string PARAM_LOCATIONID = "@pIntLocationID";
        public const string PARAM_PARENTLOCATIONID = "@pIntParentLocationID";
        public const string PARAM_IPADDRESS = "@pVarIpAddress";
        public const string PARAM_ISEXITDOOR = "@pIntIsExitDoor";
        public const string PARAM_ISCHECKOUTLOCATION = "@pIntIsCheckOutLocation";
        public const string PARAM_LOCATIONIDs = "@pVarLocationIDs";
        public const string PARAM_TAGID = "@pVarTagID";
        public const string PARAM_FLOOR = "@pVarFloorNo";//v3.8
        public const string PARAM_MFG = "@pVarMfg";
        public const string PARAM_Model = "@pVarModel";
        public const string PARAM_SERIAL_NUMBER = "@pVarSerialNumber";
        public const string PARAM_HEIGHT = "@pIntHeight";
        public const string PARAM_BIT_VALUE = "@pBitValue";
        public const string PARAM_LOCATION_EXTERNAL_ID = "@pVarExternalID";




        // ASSET Group
        public const string PARAM_ASSETGROUP = "@pVarAssetGroup";
        // (Redundant)public const string PARAM_ASSETFILTERVALUE = "@pVarAssetFilterValue";
        //(Redundant) public const string PARAM_ASSETTAGDATA = "@pVarAssetTagData";
        // (Redundant) public const string PARAM_ASSETPRINTERNAME = "@pVarAssetPrinterName";
        //(Redundant) public const string PARAM_TEMPLATENAME = "@pVarAssetTemplateName";


        public const string PARAM_ASSETGROUPID = "@pIntAssetGroupID";
        public const string PARAM_ASSETGROUPIDs = "@pVarAssetGroupIDs";

        //Host
        public const string PARAM_HOSTID = "@pGuidHostID";
        public const string PARAM_HOSTIDs = "@pVarHostIDs";
        public const string PARAM_HOSTNAME = "@pVarHost";

        //*Added on 10-Jan2013
        //GLParams
        public const string PARAM_SNo = "@pSno";
        public const string PARAM_SNos = "@pVarSNos";
        public const string PARAM_SPCVariable = "@pVarSPCVariable";
        public const string PARAM_SPCValue = "@pVarSPCValue";
        public const string PARAM_MeasureID = "@pIntMeasureID";
        public const string PARAM_UOMID = "@pIntUOMID";
        public const string PARAM_PerUOMID = "@pIntPerUOMID";
        public const string PARAM_COMMENT = "@pVarComment";
        //*

        // Tech Category
        public const string PARAM_TECHCAT = "@pVarTechCat";
        public const string PARAM_TECHDESCRIPTION = "@pVarDescription";
        // (Redundant)public const string PARAM_ASSETFILTERVALUE = "@pVarAssetFilterValue";
        //(Redundant) public const string PARAM_ASSETTAGDATA = "@pVarAssetTagData";
        // (Redundant) public const string PARAM_ASSETPRINTERNAME = "@pVarAssetPrinterName";
        //(Redundant) public const string PARAM_TEMPLATENAME = "@pVarAssetTemplateName";

        public const string PARAM_ASSETsID = "@pIntAssetID";
        public const string PARAM_TECHID = "@pIntTechID";
        public const string PARAM_TECHNAME = "@pVarTechName";

        public const string PARAM_TECHIDs = "@pVarTechIDs";
        public const string PARAM_Duration = "@pIntDuration";
        public const string PARAM_TECH_NAMES = "@pVarTechNames";
        public const string PARAM_ALL_SYSTEMS = "@pIntAllSystems";


        //*Added on 07-Jan2013
        public const string PARAM_RUPerCabLimit = "@pRealRUPerCabLimit";
        public const string PARAM_KWPerCabLimit = "@pRealKWPerCabLimit";
        public const string PARAM_AreaPerCabSQFT = "@pRealAreaPerCabSQFT";
        public const string PARAM_AreaPerCabSQMT = "@pRealAreaPerCabSQMT";
        public const string PARAM_RackGrossAreaPerCabSQFT = "@pRealRackGrossAreaPerCabSQFT";
        public const string PARAM_RackGrossAreaPerCabSQMT = "@pRealRackGrossAreaPerCabSQMT";
        public const string PARAM_StandGrossAreaPerCabSQFT = "@pRealStandGrossAreaPerCabSQFT";
        public const string PARAM_StandGrossAreaPerCabSQMT = "@pRealStandGrossAreaPerCabSQMT";
        public const string PARAM_PDF = "@pRealPDF";
        public const string PARAM_GrowthRate = "@pRealGrowthRate";
        //*

        //kjb on 23 Jan 2013
        public const string PARAM_TECHCAT_GROUP_ID = "@pIntTechGroupID";
        public const string PARAM_TECHCAT_SUB_GROUP_ID = "@pIntTechSubGroupID";
        public const string PARAM_LIMIT_PARAMETER = "@pVarLimitParameter";


        // AuditCycle
        public const string PARAM_LOCATIOONID = "@pIntLocationId";
        public const string PARAM_AUDITCYCLEID = "@pIntAuditCycleID";
        public const string PARAM_AUDITCYCLECOUNT = "@pIntCycleCount";
        public const string PARAM_STARTDATE = "@pdtStartDate";
        public const string PARAM_ENDDATE = "@pdtEndDate";
        public const string PARAM_AUDIT_STARTDATE = "@pStartDate";
        public const string PARAM_AUDIT_ENDDATE = "@pEndDate";
        public const string PARAM_AUDITCYCLEIDs = "@pIntAuditCycleIDs";

        // Muster Reason
        public const string PARAM_MUSTERREASON = "@pVarMusterReason";
        public const string PARAM_MUSTERREASONID = "@pIntMusterReasonID";
        public const string PARAM_MUSTERREASONIDs = "@pVarMusterReasonIDs";
        public const string PARAM_MUSTER_ACTION = "@pVarAction";

        //SPC MOEL MAP
        public const string PARAM_SPCMODELMAP_MODELID = "@pIntModelID";
        public const string PARAM_SPCMODELMAP_MFGID = "@pIntMfgID";
        public const string PARAM_SPCMODELMAP_SPCID = "@pIntSpcID";
        public const string PARAM_SPCMODELMAP_MODELNAME = "@pVarModelName";
        public const string PARAM_SPCMODELMAP_MFGNAME = "@pVarMfgName";
        public const string PARAM_SPCMODELMAP_MAKEMODEL = "@pVarMakeModel";
        public const string PARAM_SPCMODELMAP_MAXWATTS = "@pIntMaxWatts";
        public const string PARAM_SPCMODELMAP_STEADYWATTS = "@pIntSteadyWatts";
        public const string PARAM_SPCMODELMAP_FILTER = "@pIntFilter";
        public const string PARAM_SPCMODELMAP_BUID = "@pIntBuID";
        public const string PARAM_SPCMODELMAP_SiteID = "@pIntSiteID"; //Added on 23-Jan-2013
        public const string PARAM_SPCMODELMAP_TechID = "@pIntTechID"; //Added on 23-Jan-2013
        //public const string PARAM_SPCMODELMAP_MUSTERREASONIDs = "@pVarMusterReasonIDs";

        // ASSET
        public const string PARAM_ASSETID = "@pIntAssetID";
        public const string PARAM_ASSETIDAll = "@AssetID";
        public const string PARAM_ASSETNUMBER = "@pVarRefNumber";
        // (Redundant) public const string PARAM_ASSET_PAGECOUNT = "@pIntNoOfPages";
        public const string PARAM_ASSET_BUSINESSUNITID = "@pIntBusinessUnitID";
        public const string PARAM_ASSET_SITEID = "@pIntPrimarySiteID";
        public const string PARAM_ASSET_LOCATIONID = "@pIntDefaultLocationID";
        public const string PARAM_ASSET_NAME = "@pVarAssetName";
        public const string PARAM_ASSET_CREATEDDATE = "@pDtAssetCreatedDate";
        public const string PARAM_ASSET_CREATEDBY = "@pIntAssetCreatedBy";
        public const string PARAM_ASSET_LASTSEENLOCATIONID = "@pIntLastSeenLocationID";
        public const string PARAM_ASSET_CURRENTOWNERID = "@pIntCurrentOwnerID";
        public const string PARAM_ASSET_UPDATEDBY = "@pIntUpdatedBy";
        public const string PARAM_ASSET_AUTHORIZATION_LIST = "@pVarGroupMemberIDs";
        public const string PARAM_ASSET_ISSUED_BY = "@pIntIssuedBy";
        public const string PARAM_ASSET_ISSUED_TO = "@pIntIssuedTo";
        public const string PARAM_ASSET_RECEIVED_BY = "@pIntReceivedBy";
        public const string PARAM_SUBASSET_LIST = "@pVarSubAssetIDs";
        //public const string PARAM_ASSET_COPYNUMBER = "@pIntPrintCopyNumber"; -- commented by kjb 21 feb 2011
        public const string ASSET_ID = "@pAssetID";
        public const string CHECKOUT_BY = "@pCheckedOutBy";
        public const string CHECKOUT_SESSIONID = "@pCheckOutSessionId";
        public const string LOCATION_ID = "@pLocationId";
        public const string WORKFLOW_INSTANCE_ID = "@pWorkFlowInstanceId";
        public const string PARAM_ASSET_EXISTS = "@pIntIsAssetNoExists";
        // new fields added by kjb on 26th June 2011
        public const string PARAM_ASSET_OS = "@pVarOS";
        public const string PARAM_ASSET_CPU = "@pVarCPU";
        public const string PARAM_ASSET_CPU_COUNT = "@pIntCPUCount";
        public const string PARAM_ASSET_CPU_CORE = "@pVarCPUCore";
        public const string PARAM_ASSET_MODELID = "@pIntModelID";
        public const string PARAM_ASSET_TECHID = "@pIntTechID";
        public const string PARAM_ASSET_RACK_OR_STAND = "@pVarRackOrStand";

        // added by kjb on 03 Oct 2011 -- to include 2 new cols StartPos,NoOfRUs
        public const string PARAM_ASSET_START_POS = "@pIntStartPos";
        public const string PARAM_ASSET_NO_OF_RUS = "@pIntNoOfRUs";
        // created by kjb on 05 Oct 2011
        public const string PARAM_ASSET_IS_IMPORT = "@pBitIsImport";
        public const string PARAM_ASSET_SERIALNO_MODEL_CHECK = "@pBitSerialModelCheck";
        //*Added on 17Apr2013
        public const string PARAM_ASSET_ISPARENT = "@PIsParent";
        public const string PARAM_ASSET_PARENTASSETID = "@pIntParentAssetID";//*
        //*Added on 6May2013
        public const string PARAM_ASSET_TAG = "@PCurrentRFIDCardNumber";//*
        # region v3.8
        //*V3.8-Added on 14Oct2013-By Amar Vidya
        public const string PARAM_ASSET_ORIENTATION = "@POrientation";//*
        //v3.9
        public const string PARAM_APPLICATIONS = "@pVarApplications";//*
        //*V3.8-Added on 21Oct2013-By Amar Vidya
        public const string PARAM_ASSET_RACKTAG = "@PRackTag";//*
        //*V3.8-Added on 17Oct2013-By Amar Vidya
        // public const string PARAM_SITEIDs = "@pVarSiteIDs";
        public const string PARAM_OPERATIONs = "@pVarOperations";//*
        public const string PARAM_LOGGEDIN_USER_ID = "@pIntLoggedInUserID";

        public const string PARAM_ASSET_INTERNAL_ID = "@pVarInternalID";//*
        public const string PARAM_ASSET_EXTERNAL_ID = "@pVarExternalID";
        public const string PARAM_ASSET_DERATED_POWER = "@pFltDeratedPower";
        public const string PARAM_ASSET_DEPTH = "@pFltADepth";
        public const string PARAM_ASSET_WIDTH = "@pFltAWidth";
        public const string PARAM_ASSET_RETURN_VALUE = "@RETURN_VALUE";
        public const string PARAM_OPERATION_ID = "@pBitOperation";
        public const string PARAM_BLADE_MODEL_ID = "@pIntBladeModelID";
        public const string PARAM_ENCL_ID = "@pIntEnclID";


        //City
        public const string PARAM_CITY_ID = "@pIntCityID";//*
        public const string PARAM_CITY_NAME = "@pVarCityName";
        //COUNTRY
        public const string PARAM_COUNTRY_ID = "@pIntCountryID";
        public const string PARAM_COUNTRY_NAME = "@pVarCountry";//*
        public const string PARAM_REGION = "@pVarRegion";
        public const string PARAM_COUNTRY_CODE = "@pVarCountryCode";
        # endregion


        //SPC
        //RackDetailsBAL
        //v3.9
        public const string PARAM_ROOM = "@pVarRoom";
        public const string PARAM_ROW = "@pVarRow";
        public const string PARAM_RACK = "@pVarRack";
        public const string PARAM_RACK_EXTERNAL_ID = "@pVarExternalID";

        //Site
        public const string PARAM_SITE = "@pVarSite";
        public const string PARAM_SITEID = "@pIntSiteID";
        public const string PARAM_SITEIDs = "@pVarSiteIDs";

        //Department
        public const string PARAM_DEPARTMENT = "@pVarDepartment";
        public const string PARAM_DEPARTMENTID = "@pIntDepartmentID";
        public const string PARAM_DEPARTMENTIDs = "@pVarDepartmentIDs";

        //Location Type
        public const string PARAM_LOCATIONTYPE = "@pVarLocationType";
        public const string PARAM_LOCATIONTYPEID = "@pIntLocationTypeID";
        public const string PARAM_LOCATIONTYPEIDs = "@pVarLocationTypeIDs";
        public const string PARAM_LOCATIONTYPEACCESSID = "@pIntLocationTypeAccessID";
        public const string PARAM_ISSTORAGETYPE = "@pIntIsStorageType";
        public const string PARAM_ISRFIDLOCATION = "@pIntIsRfidLocation";


        //BU Site Assignment
        public const string PARAM_BUSITEID = "@pIntBUSiteID";
        public const string PARAM_SITEACCESSID = "@pIntSiteAccessID";

        //BU Division Assignment
        public const string PARAM_BUDIVID = "@pIntBUDivID";
        public const string PARAM_DIVACCESSID = "@pIntDivisionAccessID";


        // BU Department Assignment
        public const string PARAM_BUDEPARTMENTID = "@pIntBUDepartmentID";
        public const string PARAM_DEPARTMENTACCESSID = "@pIntDepartmentAccessID";


        //Site Location Assignment
        public const string PARAM_SITELOCATIONID = "@pIntSiteLocationID";

        //Group
        public const string PARAM_GROUP = "@pVarGroup";
        public const string PARAM_GROUPID = "@pIntGroupID";
        public const string PARAM_PARENT_GROUPID = "@pIntParentGroupID";
        public const string PARAM_GROUPIDs = "@pVarGroupIDs";

        //Module
        public const string PARAM_MODULE = "@pVarModule";
        public const string PARAM_MODULEID = "@pIntModuleID";
        public const string PARAM_MODULEIDs = "@pVarModuleIDs";


        //Access Rights
        public const string PARAM_ACCESSRIGHTS = "@pVarAccessRights";
        public const string PARAM_ACCESSRIGHTSID = "@pIntAccessRightsID";
        public const string PARAM_ACCESSRIGHTSIDs = "@pVarAccessRightsIDs";
        public const string PARAM_GROUPACCESSRIGHTSID = "@pIntGroupAccessRightID";
        public const string PARAM_MODULEACCESSRIGHTSID = "@pIntModuleAccessRightID";
        public const string PARAM_GROUPMODULEACCESSRIGHTSID = "@pIntGroupModuleRightID";


        //v454
        //Owner
        public const string PARAM_OWNERID = "@pIntOwnerID";
        public const string PARAM_OWNERIDS = "@pVarOwnerIDs";


        //USER
        public const string PARAM_USERTYPE = "@pBitUserType";
        public const string PARAM_USERID = "@pIntUserID";
        public const string PARAM_ISDELETED = "@pBitIsDeleted";
        public const string PARAM_PASSWORD = "@pVarPassword";
        public const string PARAM_LOGINNAME = "@pVarLoginName";
        public const string PARAM_FIRSTNAME = "@pVarFirstName";
        public const string PARAM_LASTNAME = "@pVarLastName";
        public const string PARAM_EMAIL = "@pVarEmail";
        public const string PARAM_USERIDs = "@pVarUserIDs";
        public const string PARAM_USERSORTBY = "@pUserSortBy";
        public const string PARAM_ISUSERSELECTIONALLOWED = "@pBitIsUserSelectionAllowed";
        public const string PARAM_USER_OLDPASSWORD = "@pVarOldPassword";
        public const string PARAM_USER_NEWPASSWORD = "@pVarNewPassword";
        public const string PARAM_BADGE = "@pVarRFIDBadge";
        public const string PARAM_RFID_ASSIGN_DATE = "@pDtRFIDAssignDate";
        public const string PARAM_RFID_USEREXISTS = "@pIntIsUserIDExists";
        public const string PARAM_LOGIN_ATTEMPTS = "@pIntLoginAttempts";
        public const string PARAM_EXPIRY_DATE = "@dtExpiryDate";//CR3001:Password Expiration, by kjb on 09 Jan 2012
        public const string PARAM_SITERESTRICTION = "@pSiteRestriction"; //V3.8-Added on 17Oct2013-By Amar Vidya
        public const string PARAM_PASSWORD_EXCEPTION_TYPE = "@pVarExceptionType";

        //HP: Added constant for sp parameter for UserGuid.
        // public const string PARAM_USERGUID = "@pVarUserGuid";



        //HP: Added constant for sp parameter for UserGuid.
        public const string PARAM_USERGUID = "@pVarUserGuid";

        //Role
        public const string PARAM_ROLE = "@pVarRole";
        public const string PARAM_ROLEID = "@pIntRoleID";
        public const string PARAM_ROLETYPE = "@pVarRoleType";
        public const string PARAM_ROLEIDs = "@pVarRoleIDs";


        //BU Site User  Assignment
        public const string PARAM_BUSITEUSERID = "@pIntBUSiteUserID";

        //Error Code

        public const string PARAM_ERRORCODE = "@pVarErrCode";


        // Message
        public const string PARAM_MESSAGECODE = "@pVarMessageCode";
        public const string PARAM_MESSAGEID = "@pIntMessageId";
        public const string PARAM_MESSAGE = "@pVarMessage";


        //Worker
        public const string PARAM_WORKERID = "@pIntWorkerID";
        public const string PARAM_BARREDREASON = "@pVarBarredReason";
        public const string PARAM_UNBARREDREASON = "@pVarUnBarredReason";
        public const string PARAM_BARREDFROMDATE = "@pDtBarredFromDate";
        public const string PARAM_BARREDTODATE = "@pDtBarredToDate";
        //public const string PARAM_REASON            = "@pVarReason";


        // RFID CARD Related

        public const string PARAM_RFIDCARDNUMBER = "@pVarRFIDCardNumber";
        public const string PARAM_REASON = "@pVarReason";



        // Transaction Types
        public const string PARAM_TRANSACTIONTYPE_CODE = "@pVarTranTypeCode";
        public const string PARAM_TRANSACTIONTYPE_ID = "@pIntTranTypeID";
        public const string PARAM_TRANSACTIONTYPE_DESC = "@pVarTranTypeDesc";

        // Interface

        public const string PARAM_INTERFACE_AUTOID = "@AutoID";
        public const string PARAM_INTERFACE_ERROR_FROM_DATE = "@FromDate";
        public const string PARAM_INTERFACE_ERROR_TO_DATE = "@ToDate";
        public const string PARAM_FAILURE_REASON = "@FailureReason";


        // Asset Group Label Template

        public const string PARAM_LABELTEMPLATE_VARIABLENAME = "@pVarVariableName";
        public const string PARAM_LABELTEMPLATE_SUBSTITUTIONVALUE = "@pVarSubstitutionValue";
        public const string PARAM_LABELTEMPLATE_ISCONSTANT = "@pBitIsConstant";

        //Purpose
        public const string PARAM_PURPOSE = "@pVarPurpose";
        public const string PARAM_PURPOSEID = "@pIntPurposeID";
        public const string PARAM_PURPOSEIDs = "@pVarPurposeIDs";

        //Customer---Added 03-Sep-2012
        public const string PARAM_NAME = "@pVarName";
        public const string PARAM_SHORTNAME = "@pVarShortName";

        public const string PARAM_PRIMARYADDRESS1 = "@pVarPrimaryAddress1";
        public const string PARAM_PRIMARYADDRESS2 = "@pVarPrimaryAddress2";
        public const string PARAM_PRIMARYADDRESS3 = "@pVarPrimaryAddress3";
        public const string PARAM_PRIMARYADDRESS4 = "@pVarPrimaryAddress4";
        public const string PARAM_PRIMARYPOSTALCODE = "@pVarPrimaryPostalCode";
        public const string PARAM_PRIMARYCONTACTPERSON = "@pVarPrimaryContactPerson";
        public const string PARAM_PRIMARYPHONENUMBER = "@pVarPrimaryPhoneNumber";
        public const string PARAM_PRIMARYMOBILENUMBER = "@pVarPrimaryMobileNumber";
        public const string PARAM_PRIMARYFAXNUMBER = "@pVarPrimaryFaxNumber";
        public const string PARAM_PRIMARYEMAIL = "@pVarPrimaryEmail";

        public const string PARAM_ALTERNATEADDRESS1 = "@pVarAlternateAddress1";
        public const string PARAM_ALTERNATEADDRESS2 = "@pVarAlternateAddress2";
        public const string PARAM_ALTERNATEADDRESS3 = "@pVarAlternateAddress3";
        public const string PARAM_ALTERNATEADDRESS4 = "@pVarAlternateAddress4";

        public const string PARAM_ALTERNATEPOSTALCODE = "@pVarAlternatePostalCode";
        public const string PARAM_ALTERNATECONTACTPERSON = "@pVarAlternateContactPerson";
        public const string PARAM_ALTERNATEPHONENUMBER = "@pVarAlternatePhoneNumber";
        public const string PARAM_ALTERNATEMOBILENUMBER = "@pVarAlternateMobileNumber";
        public const string PARAM_ALTERNATEFAXNUMBER = "@pVarAlternateFaxNumber";

        public const string PARAM_ALTERNATEEMAIL = "@pVarAlternateEmail";

        public const string PARAM_CITYID = "@pIntCityID";
        public const string PARAM_INDUSTRYID = "@pIntIndustryID";
        public const string PARAM_CUSTOMERID = "@pIntCustomerID";
        public const string PARAM_CUSTOMERIDs = "@pVarCustomerIDs";
        public const string PARAM_ADDRESSID = "@pIntAddressID";
        //Country
        public const string PARAM_COST_COUNTRY_ID = "@pIntCountryID";
        //State
        //public const string PARAM_COST_STATE_ID = "@pIntStateID";
        /// <summary>
        /// Project
        /// 
        public const string PARAM_PROJECTID = "@pIntProjectID";
        public const string PARAM_PROJECTNAME = "@pVarProjectName";
        public const string PARAM_PROJECTCODE = "@pVarProjectCode";
        public const string PARAM_PROJECTWBSCODE = "@pVarWBSCode";
        public const string PARAM_PROJECTDESCRIPTION = "@pVarDescription";
        public const string PARAM_PROJECTSTARTDATE = "@pdtStartDate";
        public const string PARAM_PROJECTENDDATE = "@pdtEndDate";
        public const string PARAM_PROJECTVALUE = "@pProjectValue";
        public const string PARAM_PROJECTSTARTYEAR = "@pStartYear";
        public const string PARAM_PROJECTSTARTMONTH = "@pStartMonth";
        public const string PARAM_PROJECTIONYEAR = "@pProjectionYears";

        ///SPCHEADER-- Added on 05-Dec-2012

        public const string PARAM_SPCDESCRIPTION = "@pVarDescription";
        // public const string PARAM_VERSIONNO = "";
        public const string PARAM_UOM = "@pVarUOM";

        //*Added on 04-Feb-2013
        public const string PARAM_PLUSSWING = "@pVarSwing";
        //*

        // public const string PARAM_UOM = "@pStrUOM";

        // Added on 11-Dec-2012
        public const string PARAM_COMBINESITES = "@pVarCombineSites";
        public const string PARAM_INDIVIDUALSITES = "@pVarIndividualSites";
        public const string PARAM_DELIMETER = "@pVarDelimeter";
        public const string PARAM_GROSSFACTORPERSTANDALONE = "@pRealGrossFactorPerStandalone";
        public const string PARAM_CABINETWEIGHT = "@pRealCabinetWeight";
        public const string PARAM_SWINGFACTOR = "@pRealSwingFactor ";
        public const string PARAM_FLOORSPACECOST = "@pRealFloorSpaceCost";
        public const string PARAM_POWERCOST = "@pRealPowerCost";
        public const string PARAM_BTUMULTIPLIER = "@pRealBTUMultiplier";
        public const string PARAM_KCALMULTIPLIER = "@pRealKCALMultiplier";
        public const string PARAM_PUE = "@pRealPUE";
        public const string PARAM_MABuffer = "@pIntMnABuffer";
        public const string PARAM_LIMITPARAM = "@pVarLimitParam";

        //ModelCount
        public const string PARAM_SPCHEADERID = "@pIntSPCHeaderID";
        public const string PARAM_SPCQUANTITY = "@pIntQuantity";
        public const string PARAM_SPC_OLD_QUANTITY = "@pRealOldQuantity";
        public const string PARAM_SPC_NEW_QUANTITY = "@pRealNewQuantity";
        public const string PARAM_MODEL_COUNT_ID = "@pIntModelCountID";
        public const string PARAM_MODEL_COUNT_IDS = "@pVarModelCountIDs";
        public const string PARAM_OPERATION_TYPE = "@pIntOperation";

        public const string PARAM_RU_PER_CAB_LIMIT = "@pRealRUPercabLimit";
        public const string PARAM_KW_PER_CAB_LIMIT = "@pRealKWPercabLimit";
        public const string PARAM_GROWTH_RATE = "@pRealGrowthRate";
        public const string PARAM_PER_CAB = "@pIntPerCab";

        // kjb on 18 Jan 2013
        public const string PARAM_TECH_MAIN_GROUP_ID = "@pIntTechMainGroupID";
        public const string PARAM_TECH_SUB_GROUP_ID = "@pIntTechSubGroupID";
        public const string PARAM_INDEX_NO = "@pIntIndexNo";


        /// </summary>

        //Added on 17-Dec-2012
        //SPCGroup
        public const string PARAM_SPCMainGroupID = "@PIntSPCMainGroupID";
        public const string PARAM_SPCSubGroupID = "@PIntSPCSubGroupID";
        public const string PARAM_SPCSubGroupModelID = "@PIntSPCSubGroupModelID";
        public const string PARAM_PARENTGROUPID = "@PIntSPCParentGroupID";
        public const string PARAM_SPCMAinGroupNameNew = "@PVarSPCMainGroupNameNew";
        public const string PARAM_SPCSubGroupName = "@PVarSPCSubGroupName";

        //AirFlowDirection
        public const string PARAM_AIRFLOWDIRECTION_ID = "@pIntAirFlowDirectionID";

        //MountType
        public const string PARAM_MOUNTTYPE_ID = "@pIntMountTypeID";

        //ORIENTATION
        public const string PARAM_ORIENTATION_ID = "@pIntOrientationID";

        //AppStatus
        public const string PARAM_APPSTATUS_ID = "@pIntAppStatusID";

        //ORIENTATION
        public const string PARAM_INPUT_CONNECTOR_TYPE_ID = "@pIntInputConnectorTypeID";

        //AppStatus
        public const string PARAM_OUTPUT_CONNECTOR_TYPE_ID = "@pIntOutputConnectorTypeID";

        //Tenant
        public const string PARAM_TENANTID = "@pIntTenantId";
        public const string PARAM_TENANTIDS = "@pVarTenantIds";
        public const string PARAM_TENANT_FULLNAME = "@pVarTenantFullName";
        public const string PARAM_TENANT_SHORTNAME = "@pVarTenantShortName";
        public const string PARAM_TENANT_TYPE = "@pVarTenantType";
        public const string PARAM_TENANT_TYPE_SIZE = "@pIntTenantTypeSize";
        public const string PARAM_TENANT_CONTACT_FNAME = "@pVarContactFName";
        public const string PARAM_TENANT_CONTACT_LNAME = "@pVarContactLName";
        public const string PARAM_TENANT_CONTACT_EMAIL = "@pVarContactEmail";
        public const string PARAM_TENANT_USER_COUNT = "@pIntUserCount";
        public const string PARAM_TENANT_ASSIGNED_LOCATIONS = "@pVarAssignedLocs";
        public const string PARAM_TENANT_ADMIN_PERMISSIONS = "@pVarAdminPermissions";
        public const string PARAM_TENANT_USER_PERMISSIONS = "@pVarUserPermissions";


        public class MetaData
        {
            // Objects (Business Objects)
            public const string PARAM_BOID = "@pIntObjectID";
            public const string PARAM_BONAME = "@pVarObjectName";
            public const string PARAM_BODESCRIPTION = "@pVarObjectDescription";
            public const string PARAM_BOIDs = "@pVarObjectIDs";



            // Tables 
            public const string PARAM_TABLEID = "@pVarTableID";
            public const string PARAM_TABLENAME = "@pVarTableName";
            public const string PARAM_TABLEDESCRIPTION = "@pVarTableDescription";
            public const string PARAM_TABLEIDs = "@pVarTableIDs";


            // Columns
            public const string PARAM_ATTRID = "@pVarAttrID";
            public const string PARAM_ATTRNAME = "@pVarAttrName";
            public const string PARAM_ATTRDESCRIPTION = "@pVarAttrDescription";
            public const string PARAM_ATTRIDs = "@pVarAttrIDs";


        }

    };

    public class DBFields
    {
        //Customer
        public const string DBFIELD_CUSTOMERID = "CustomerID";
        public const string DBFIELD_NAME = "Name";
        public const string DBFIELD_SHORTNAME = "ShortName";
        public const string DBFIELD_INDUSTRYTYPE = "IndustryType";
        public const string DBFIELD_INDUSTRYNAME = "IndustryName";
        public const string DBFIELD_CITY = "City";
        public const string DBFIELD_CITYNAME = "CityName";
        public const string DBFIELD_COUNTRY = "Country";
        public const string DBFIELD_STATE = "State";
        public const string DBFIELD_PRIMARYADDRESS1 = "PrimaryAddress1";
        public const string DBFIELD_PRIMARYADDRESS2 = "PrimaryAddress2";
        public const string DBFIELD_PRIMARYADDRESS3 = "PrimaryAddress3";
        public const string DBFIELD_PRIMARYADDRESS4 = "PrimaryAddress4";
        public const string DBFIELD_PRIMARYPOSTALCODE = "PrimaryPostalCode";
        public const string DBFIELD_PRIMARYCONTACTPERSON = "PrimaryContactPerson";
        public const string DBFIELD_PRIMARYPHONENUMBER = "PrimaryPhoneNumber";
        public const string DBFIELD_PRIMARYMOBILENUMBER = "PrimaryMobileNumber";
        public const string DBFIELD_PRIMARYFAXNUMBER = "PrimaryFaxNumber";
        public const string DBFIELD_PRIMARYEMAIL = "PrimaryEmail";

        public const string DBFIELD_ALTERNATEADDRESS1 = "AlternateAddress1";
        public const string DBFIELD_ALTERNATEADDRESS2 = "AlternateAddress2";
        public const string DBFIELD_ALTERNATEADDRESS3 = "AlternateAddress3";
        public const string DBFIELD_ALTERNATEADDRESS4 = "AlternateAddress4";
        public const string DBFIELD_ALTERNATEPOSTALCODE = "AlternatePostalCode";
        public const string DBFIELD_ALTERNATECONTACTPERSON = "AlternateContactPerson";
        public const string DBFIELD_ALTERNATEPHONENUMBER = "AlternatePhoneNumber";
        public const string DBFIELD_ALTERNATEMOBILENUMBER = "AlternateMobileNumber";
        public const string DBFIELD_ALTERNATEFAXNUMBER = "AlternateFaxNumber";
        public const string DBFIELD_ALTERNATEEMAIL = "AlternateEmail";

        public const string DBFIELD_ADDRESSTYPE = "AddressType";
        public const string DBFIELD_ADDRESSTYPEID = "AddressTypeID";

        //Project
        public const string DBFIELD_PROJECTNAME = "ProjectName";
        public const string DBFIELD_PROJECTCODE = "ProjectCode";
        public const string DBFIELD_PROJECTVALUE = "ProjectValue";
        public const string DBFIELD_PROJDESCRIPTION = "Description";
        public const string DBFIELD_WBSCODE = "WBSCode";
        public const string DBFIELD_CUSTOMERNAME = "Name";
        public const string DBFIELD_PROJSTARTDATE = "StartDate";
        public const string DBFIELD_PROJENDDATE = "EndDate";
        public const string DBFIELD_ASSETID = "AssetID";
        public const string DBFIELD_COUNTRYNAME = "CountryName";
        public const string DBFIELD_COUNTRYID = "CountryID";

        //Added on 04-Dec-2012
        public const string DBFIELD_STARTYEAR = "YearNo";
        public const string DBFIELD_STARTMONTH = "MonthName";
        public const string DBFIELD_STARTMONTHNO = "MonthNo";
        public const string DBFIELD_PROJECTIONYEARS = "SPCProjYears";
        public const string DBFIELD_PROJECTID = "ProjectID";

        //Technology category
        public const string DBFIELD_TECHID = "TechID";
        public const string DBFIELD_TECHNAME = "TechName";
        public const string DBFIELD_TECHID_STATUS = "Status";
        public const string DBFIELD_TECHID_DESCRIPTION = "Description";

        //*Added on 07-Jan2013
        public const string DBFIELD_RUPerCabLimit = "RUPerCabLimit";
        public const string DBFIELD_KWPerCabLimit = "KWPerCabLimit";
        public const string DBFIELD_AreaPerCabSQFT = "AreaPerCabSQFT";
        public const string DBFIELD_AreaPerCabSQMT = "AreaPerCabSQMT";
        public const string DBFIELD_RackGrossAreaPerCabSQFT = "RackGrossAreaPerCabSQFT";
        public const string DBFIELD_RackGrossAreaPerCabSQMT = "RackGrossAreaPerCabSQMT";
        public const string DBFIELD_StandGrossAreaPerCabSQFT = "StandGrossAreaPerCabSQFT";
        public const string DBFIELD_StandGrossAreaPerCabSQMT = "StandGrossAreaPerCabSQMT";
        public const string DBFIELD_PDF = "PDF";
        public const string DBFIELD_GrowthRate = "GrowthRate";
        //*

        //Audit Details
        public const string DBFIELD_RETURNVALUE = "ReturnValue";
        public const string DBFIELD_STATUS = "Status";
        public const string DBFIELD_CREATEDBY = "CreatedBy";
        public const string DBFIELD_CREATEDON = "CreatedDate";
        public const string DBFIELD_MODIFIEDBY = "LastModifiedBy";
        public const string DBFIELD_REMARKS = "WONumber";

        //Host
        public const string DBFIELD_HOST = "HostName";
        public const string DBFIELD_HOSTID = "HostID";

        //Added on 10-Jan-2013
        //Global Parameters
        public const string DBFIELD_SNo = "Sno";
        public const string DBFIELD_SPCVariable = "SPCVariable";
        public const string DBFIELD_SPCValue = "SPCValue";
        public const string DBFIELD_UOMID = "UOMID";
        public const string DBFIELD_PerUOMID = "PerUOMID";
        public const string DBFIELD_MeasureID = "MeasureID";
        public const string DBFIELD_Comment = "Comment";
        //*


        //Business Unit
        public const string DBFIELD_BUSINESSUNIT = "BusinessUnit";
        public const string DBFIELD_DESCRIPTION = "Description";
        public const string DBFIELD_BUSINESSUNITID = "BusinessUnitID";
        public const string DBFIELD_COPREFIX = "CoPrefix";

        //Audit Cycle
        public const string DBFIELD_AUDITCYCLE_ID = "ID";

        //Device Reg -- kjb 0n 07 May 2012
        public const string DBFIELD_DEVICEID = "DeviceID";
        public const string DBFIELD_DEVICENAME = "DeviceName";
        public const string DBFIELD_ID = "ID";
        //public const string DBFIELD_SITEID = "SiteID";

        //Asset Model
        public const string DBFIELD_MODELNAME = "ModelName";
        public const string DBFIELD_MODELDESCRIPTION = "Description";
        public const string DBFIELD_MODELID = "ModelID";
        public const string DBFIELD_AMMFGID = "MfgID";
        public const string DBFIELD_PARENTMODELID = "ParentModelID";
        public const string DBFIELD_IS_BLADE = "IsBlade";
        public const string DBFIELD_IS_ENCLOSURE = "IsEnclosure";
        public const string DBFIELD_ASSET_COUNT = "AssetCount";
        public const string DBFIELD_MODEL_TYPE_ID = "AssetTypeID";
        public const string DBFIELD_MODEL_WIDTH = "Width_mm";
        public const string DBFIELD_MODEL_DEPTH = "Depth_mm";
        public const string DBFIELD_MODEL_HEIGHT = "Height_mm";
        public const string DBFIELD_MODEL_UHEIGHT = "UHeight";
        public const string DBFIELD_MODEL_WEIGHT = "Weight_kg";
        public const string DBFIELD_MODEL_MAX_POWER = "MaxPower_Watts";
        public const string DBFIELD_MODEL_SS_POWER = "SteadyState_Watts";
        public const string DBFIELD_MODEL_TOTAL_PSU_COUNT = "TotalPSUCount";
        public const string DBFIELD_MODEL_REQ_PSU_COUNT = "RequiredPSUCount";
        public const string DBFIELD_MODEL_CONN_PDU_SIDE = "ConnectorType_PDUSide";
        public const string DBFIELD_MODEL_CONN_DEV_SIDE = "ConnectorType_DeviceSide";
        public const string DBFIELD_MODEL_MOUNT_TYPE_ID = "MountTypeID";
        public const string DBFIELD_MODEL_AF_DIRECTION_ID = "AirFlowDirectionID";
        public const string DBFIELD_MODEL_RACK_INTERNAL_DEPTH = "InternalDepth_Rack";
        public const string DBFIELD_MODEL_RACK_INTERNAL_HEIGHT = "InternalHeight_Rack";
        public const string DBFIELD_MODEL_RACK_INTERNAL_WIDTH = "InternalWidth_Rack";
        public const string DBFIELD_MODEL_MOUNT_TYPE = "MountType";
        public const string DBFIELD_MODEL_ENCL_FRONT_ROW_COUNT = "EnclFrontRowCount";
        public const string DBFIELD_MODEL_ENCL_FRONT_COL_COUNT = "EnclFrontColumnCount";
        public const string DBFIELD_MODEL_ENCL_REAR_ROW_COUNT = "EnclRearRowCount";
        public const string DBFIELD_MODEL_ENCL_REAR_COL_COUNT = "EnclRearColumnCount";
        public const string DBFIELD_MODEL_BLADE_ROW_COUNT = "BladeRowCount";
        public const string DBFIELD_MODEL_BLADE_COL_COUNT = "BladeColumnCount";
        public const string DBFIELD_MODEL_TYPE = "ModelType";


        //SPC Model
        public const string DBFIELD_MAKEMODEL = "MakeModel";
        public const string DBFIELD_SPCID = "SPCID";
        public const string DBFIELD_PRODNO = "ProductNumber";
        public const string DBFIELD_DEPTHIN = "Depth_Inches";
        public const string DBFIELD_DEPTHMM = "Depth_MM";
        public const string DBFIELD_WIDTHIN = "Width_Inches";
        public const string DBFIELD_WIDTHMM = "Width_MM";
        public const string DBFIELD_HEIGHTIN = "Height_Inches";
        public const string DBFIELD_HEIGHTMM = "Height_MM";
        public const string DBFIELD_WEIGHTLB = "Weight_LB";
        public const string DBFIELD_WEIGHTKG = "Weight_KG";
        public const string DBFIELD_SQFT = "SQFT_Standalone";
        public const string DBFIELD_SQMT = "SQMetre_Standalone";
        public const string DBFIELD_STEADYSTATEWATTS = "SteadyStateWatts";
        public const string DBFIELD_MAXWATTS = "MaxWatts";
        public const string DBFIELD_EMPTY2 = "Empty2";
        public const string DBFIELD_NOTES1 = "Notes_1";
        public const string DBFIELD_NOTES2 = "Notes_2";
        public const string DBFIELD_ITEMTYPE = "ItemType";
        public const string DBFIELD_PATH = "Path";
        public const string DBFIELD_ROWREFID = "NewRowRefID";
        public const string DBFIELD_SOURCEFILE = "SourceFile";
        public const string DBFIELD_DEVICECOST = "DeviceCost";
        public const string DBFIELD_RACKSTAND = "RackOrStand";

        //Apllication
        public const string DBFIELD_APPLID = "ApplID";
        public const string DBFIELD_APPLNAME = "ApplName";
        public const string DBFIELD_APPLDESC = "ApplDesc";
        public const string DBFIELD_APPLTYPEID = "ApplTypeID";
        public const string DBFIELD_APPLSTATUS = "ApplStatus";
        public const string DBFIELD_APPLCRITICALITYID = "ApplCriticalityID";
        public const string DBFIELD_APPLBUID = "BUID";
        public const string DBFIELD_APPLDIVISIONID = "ApplDivisionID";
        public const string DBFIELD_APPLMANAGEID = "ApplManageID";
        public const string DBFIELD_APPLSTATUS_ID = "AppStatusID";
        public const string DBFIELD_APP_STATUS = "AppStatus";

        //Application Type
        public const string DBFIELD_APPLTYPE = "ApplType";
        public const string DBFIELD_APPLTYPEDESC = "ApplTypeDesc";

        //Application Criticality
        public const string DBFIELD_APPLCRITICALITY = "ApplCriticality";
        public const string DBFIELD_APPLCRITICALITYDESC = "ApplCriticalityDesc";
        //-->v3.9
        public const string DBFIELD_APPLCRITICALITy_BACK_COLOR = "BackColorCode";

        //Division
        public const string DBFIELD_DIVISIONID = "DivisionID"; //Added on 19Apr2013
        public const string DBFIELD_DIVISION = "Division";
        public const string DBFIELD_DIVISIONDESC = "DivisionDesc";

        //Manufacturer
        public const string DBFIELD_MFGNAME = "MfgName";
        public const string DBFIELD_MFGDESCRIPTION = "Description";
        public const string DBFIELD_MFGID = "MfgID";

        //Site
        public const string DBFIELD_SITE = "Site";
        public const string DBFIELD_SITEID = "SiteID";
        //v3.8
        public const string DBFIELD_COUNTRY_ID = "CountryID";
        public const string DBFIELD_CITY_ID = "CityID";
        public const string DBFIELD_REGION = "Region";
        public const string DBFIELD_COUNTRY_NAME = "Country";
        public const string DBFIELD_CITY_NAME = "City";
        //--//

        //Audit Cycle
        public const string DBFIELD_STARTDATE = "StartDate";
        public const string DBFIELD_ENDDATE = "EndDate";

        public const string DBFIELD_BU = "BusinessUnitID";
        public const string DBFIELD_Sites = "Site";
        public const string DBFIELD_SitesID = "SiteID";
        public const string DBFIELD_Locatioon = "Room";
        public const string DBFIELD_LocatioonID = "LocationID";

        // Department

        public const string DBFIELD_DEPARTMENTID = "DepartmentID";

        //Location
        public const string DBFIELD_LOCATION = "Location";
        public const string DBFIELD_LOCATION_DESC = "Description";
        public const string DBFIELD_LOCATIONID = "LocationID";
        public const string DBFIELD_IPADDRESS = "IpAddress";
        public const string DBFIELD_ISEXITDOOR = "IsExitDoor";
        public const string DBFIELD_ISCHECKOUTLOCATION = "IsCheckOutLocation";
        public const string DBFIELD_PARENTLOCATIONID = "ParentLocationID";
        public const string DBFIELD_PARENTLOCATION = "ParentLocation";
        public const string DBFIELD_LOCATION_SERIAL_NO = "SerialNumber";
        public const string DBFIELD_LOCATION_WIDTH = "Width";
        public const string DBFIELD_LOCATION_DEPTH = "Depth";
        public const string DBFIELD_LOCATION_HEIGHT = "Height";
        public const string DBFIELD_LOCATION_MODEL_ID = "ModelID";
        public const string DBFIELD_LOCATION_TAG_ID = "TagID";
        public const string DBFIELD_FLOOR_NO = "FloorNo";
        public const string DBFIELD_UPOSITIONS = "UPositions";
        public const string DBFIELD_FRONT_POSITIONS = "FrontPositions";
        public const string DBFIELD_REAR_POSITIONS = "RearPositions";

        //Location Type
        public const string DBFIELD_LOCATIONTYPE = "LocationType";
        public const string DBFIELD_LOCATIONTYPEID = "LocationTypeID";
        public const string DBFIELD_RFID_LOC = "IsRFIDLocation";
        public const string DBFIELD_STORAGE_LOC = "IsStorageType";


        //ASSET Group

        public const string DBFIELD_ASSETGROUP = "AssetGroup";
        // (Redundant) public const string DBFIELD_ASSETFILTERVALUE = "FilterValue";
        // (Redundant)public const string DBFIELD_ASSETTAGDATA = "TagData";
        // (Redundant) public const string DBFIELD_ASSETPRINTERNAME = "PrinterName";
        // (Redundant) public const string DBFIELD_ASSETTEMPLATENAME = "TemplateName";
        public const string DBFIELD_ASSETGROUPID = "AssetGroupID";

        // Muster Reason

        public const string DBFIELD_MUSTERREASON = "MusterReason";
        public const string DBFIELD_MUSTERREASONID = "MusterReasonID";


        //BU Site Assignment
        public const string DBFIELD_BUSITEID = "BUSiteAssignmentID";

        //Site Location Assignment
        public const string DBFIELD_SITELOCATIONID = "SiteLocationAssignmentID";


        //Department
        public const string DBFIELD_DEPARTMENT = "Department";


        //Group
        public const string DBFIELD_GROUPID = "GroupID";
        public const string DBFIELD_GROUP = "Group";
        public const string DBFIELD_PARENT_GROUP_ID = "ParentGroupID";
        public const string DBFIELD_PARENT_GROUP = "ParentGroup";

        //Module
        public const string DBFIELD_MODULEID = "ModuleID";
        public const string DBFIELD_MODULE = "Module";


        //Access Rights
        public const string DBFIELD_ACCESSRIGHTSID = "RightsID";
        public const string DBFIELD_ACCESSRIGHTS = "Rights";

        //User
        public const string DBFIELD_USERID = "UserID";
        public const string DBFIELD_LOGINNAME = "LoginName";
        public const string DBFIELD_FIRSTNAME = "FirstName";
        public const string DBFIELD_LASTNAME = "LastName";
        public const string DBFIELD_DISPLAYNAME = "DisplayName";
        public const string DBFIELD_EMAIL = "Email";
        public const string DBFIELD_USERTYPE = "UserType";
        public const string DBFIELD_USERSTATUS = "Status";
        public const string DBFIELD_ISUSERSELECTIONALLOWED = "IsUserSelectionAllowed";
        public const string DBFIELD_DEFAULTLOC = "DefaultLocation";
        public const string DBFIELD_DEFAULTSITE = "DefaultSite";
        public const string DBFIELD_DEFAULTBU = "DefaultBU";
        public const string DBFIELD_RFIDBADGE = "CurrentRFIDBadge";
        public const string DBFIELD_RFIDASSIGNDATE = "RFIDAssignDate";



        //BU Site User Assignment
        public const string DBFIELD_BUSITEUSERID = "BUSiteUserAssignmentID";




        //RFID Card Assignment
        public const string DBFIELD_WORKERRFIDCARDASSIGNMENTID = "WorkerRFIDCardAssignmentID";
        public const string DBFIELD_RFIDCARDSERIALNUMBER = "RFIDCardSerialNumber";
        public const string DBFIELD_ASSIGNEDDATE = "AssignedDate";
        public const string DBFIELD_ASSIGNEDBY = "AssignedBy";
        public const string DBFIELD_DEASSIGNEDDATE = "DeAssignedDate";
        public const string DBFIELD_DEASSIGNEDBY = "DeAssignedBy";
        public const string DBFIELD_DEASSIGNEDREASON = "DeAssignedReason";
        public const string DBFIELD_CARDISSUEDATE = "CardIssueDate";
        public const string DBFIELD_LOSTRFIDCARDFLAG = "DeAssignedReason";
        public const string DBFIELD_RFIDSTATUS = "CardIssueDate";


        //Asset Group Label Template
        public const string DBFIELD_LABELTEMPLATE_VARIABLENAME = "VariableName";
        public const string DBFIELD_LABELTEMPLATE_SUBSTITUTIONVALUE = "SubstitutionValue";
        public const string DBFIELD_LABELTEMPLATE_ISCONSTANT = "IsConstantValue";

        // Purpose

        public const string DBFIELD_PURPOSEID = "PurposeID";

        //Purpose
        public const string DBFIELD_PURPOSE = "Purpose";

        //SPC Version
        //Added on 12-Dec-2012
        public const string DBFIELD_GROSSINGFACTOR = "ProjectName";
        public const string DBFIELD_SWINGFACTOR = "ProjectName";
        public const string DBFIELD_CABINETWEIGHT = "ProjectName";
        public const string DBFIELD_FLOORSPACE = "ProjectName";
        public const string DBFIELD_POWERCOST = "ProjectName";
        public const string DBFIELD_PUE = "ProjectName";
        public const string DBFIELD_BTUMULTIPLIER = "ProjectName";
        public const string DBFIELD_KCALMULTIPLIER = "ProjectName";
        public const string DBFIELD_MABUFFER = "ProjectName";
        public const string DBFIELD_VERSIONNO = "VersionNo";


        //v454
        //Owner
        public const string DBFIELD_OWNER_FNAME = "OwnerFirstName";
        public const string DBFIELD_OWNER_LNAME = "OwnerLastName";
        public const string DBFIELD_OWNER_EMAIL = "Email";
        public const string DBFIELD_OWNER_ID = "OwnerID";

        //Orientation
        public const string DBFIELD_ORIENTATION_ID = "OrientationID";
        public const string DBFIELD_ORIENTATION_NAME = "OrientationName";
        //Tenant
        public const string DBFIELD_TENANT_FULL_NAME = "TenantFullName";
        public const string DBFIELD_TENANT_SHORT_NAME = "TenantShortName";
        public const string DBFIELD_TENANT_TYPE = "TenantType";
        public const string DBFIELD_TENANT_TYPE_SIZE = "TenantTypeSize";
        public const string DBFIELD_TENANT_CONTANCT_FNAME = "ContactFirstName";
        public const string DBFIELD_TENANT_CONTANCT_LNAME = "ContactLastName";
        public const string DBFIELD_TENANT_CONTANCT_EMAIL = "ContactEmail";
        public const string DBFIELD_TENANT_USER_COUNT = "UserCount";
        public const string DBFIELD_TENANT_ID = "TenantId";
        public const string DBFIELD_TENANT_ASSIGNEDLOCATIONS = "TenantAssignedLocations";
        public const string DBFIELD_TENANT_ASSIGNEDGROUPS = "TenantAssignedGroups";
        public const string DBFIELD_APPLICATION_ID = "ApplicationId";
    };

    public class MessageCodes
    {

        //public const string USR_ERR_INSERT = "USR0001";
        //public const string USR_ERR_UPDATE = "USR0002";
        //public const string USR_ERR_DELETE = "USR0003";
        //public const string USR_ERR_EXISTS = "USR0004";
        //public const string USR_ERR_LIST = "USR0005";
        //public const string USR_ERR_JOBCARD = "USR0006";
        //public const string USR_ERR_ASSIGN = "USR0007";

        // Login 
        public const string PASSWORD_EXPIRED = "PASSWORD_EXPIRED";
        public const string PASSWORD_WILL_EXPIRE_1 = "PASSWORD_WILL_EXPIRE_1";
        public const string PASSWORD_WILL_EXPIRE_2 = "PASSWORD_WILL_EXPIRE_2";
        public const string AUTHENTICATION_FAILED = "AUTHENTICATION_FAILED";
        public const string ACCOUNT_LOCKED = "ACCOUNT_LOCKED";
        public const string FIRST_LOGIN = "FIRST_LOGIN";

        //Change Password
        public const string PASSWORD_CHANGE_SUCCESSFUL = "PASSWORD_CHANGE_SUCCESSFUL";

        //group
        public const string GRP_TYPE = "GRP_TYPE";

        //MANAGE USER
        public const string MANAGE_USER_BU_SETUP = "MANAGE_USER_BU_SETUP";
        public const string MANAGE_USER_EXISTS = "MANAGE_USER_EXISTS";
        public const string MANAGE_USER_LDAP_DIR = "MANAGE_USER_LDAP_DIR";
        public const string MANAGE_USER_BU_REQ = "MANAGE_USER_BU_REQ";
        public const string MANAGE_USER_EXISTS_LDAP_DIR = "MANAGE_USER_EXISTS_LDAP_DIR";

        public const string LOGIN_NAME_REQUIRED = "LOGIN_NAME_REQUIRED";
        public const string USER_UPDATE_SUCCESS = "USER_UPDATE_SUCCESS";
        public const string USER_CREATE_SUCCESS = "USER_CREATE_SUCCESS";

        //User Level

        public const string GEN_S_INSERTED = "GEN_S_INSERTED";
        public const string GEN_S_UPDATED = "GEN_S_UPDATED";
        public const string GEN_S_DELETED = "GEN_S_DELETED";
        public const string GEN_I_EXISTS = "GEN_I_EXISTS";
        public const string GEN_I_LIST = "GEN_I_LIST";
        public const string GEN_S_ASSIGNED = "GEN_S_ASSIGNED";
        public const string GEN_E_SYSTEM = "GEN_E_SYSTEM";
        public const string GEN_I_REMOVEASSIGNMENT = "GEN_I_REMOVEASSIGNMENT";
        public const string GEN_I_REMOVECHILD = "GEN_I_REMOVECHILD";
        public const string USER_CHANGE_PWD = "USER_CHANGE_PWD";
        public const string RESET_PWD = "RESET_PWD";

        //ASSIGN RIGHTS
        public const string ASSIGN_RIGHTS_MEMBER = "ASSIGN_RIGHTS_MEMBER";
        public const string ASSIGN_RIGHTS_COST_CENTER = "ASSIGN_RIGHTS_COST_CENTER";
        public const string ASSIGN_RIGHTS = "ASSIGN_RIGHTS";

        //* Added on 15-01-2013
        //Customize Group Map
        public const string SPCMAP_CustomizeGroupModels = "MAP_CUSTOMIZEGROUP_MODELS";

        //SEARCH
        public const string SEARCH_COMMON = "SEARCH_COMMON";

        //SPC Model
        public const string SPCMAP_JS_DELETE = "SPCMAP_JS_DELETE";

        //Host
        public const string HOST_JS_DELETE = "HOST_JS_DELETE";

        //*Added on 10-Jan-2013
        //Global Params
        public const string GlobalParams_JS_DELETE = "GlobalParams_JS_DELETE";
        //*

        //Business Unit
        public const string BU_JS_DELETE = "BU_JS_DELETE";  //for Business Unit - before Delete confirmation

        //DeviceReg.aspx
        public const string DEV_JS_DELETE = "DEV_JS_DELETE";  //for DeviceReg - before Delete confirmation

        //Audit Cycle
        public const string AC_JS_DELETE = "AC_JS_DELETE";  //for Audit Cycle - before Delete confirmation

        ////SPC Model
        //public const string SPCModel_JS_DELETE = "SPCModel_JS_DELETE";  //for SPC Model - before Delete confirmation

        //Manufacturer 
        public const string MFG_JS_DELETE = "MFG_JS_DELETE";  //for Manufacturer  - before Delete confirmation
        // added by kjb on 21st JUne 2011

        //Group
        public const string GROUP_JS_DELETE = "GROUP_JS_DELETE";


        //Asset Model 
        public const string ASSETMODEL_JS_DELETE = "ASSETMODEL_JS_DELETE";  //for aSSET mODEL  - before Delete confirmation
        // added by kjb on 23 JUne 2011

        //SPC Model
        public const string SPCMODEL_JS_DELETE = "SPCMODEL_JS_DELETE";

        //Create Application
        public const string APPL_JS_DELETE = "APPL_JS_DELETE"; //for Create Application  - before Delete confirmation



        // Technology Category
        public const string TC_JS_DELETE = "TC_JS_DELETE";  //for TechnologyCategory - before Delete confirmation
        //Site
        public const string SITE_JS_DELETE = "SITE_JS_DELETE";  //for Site - before Delete confirmation
        // Department
        public const string DEPARTMENT_JS_DELETE = "DEPARTMENT_JS_DELETE";  //for Department - before Delete confirmation

        //Location
        public const string LC_JS_DELETE = "LC_JS_DELETE";  //for Location - before Delete confirmation

        //Location Type
        public const string LCTYPE_JS_DELETE = "LCTYPE_JS_DELETE";  //for Location Type - before Delete confirmation


        // ASSET Group

        public const string ASSETGROUP_JS_DELETE = "ASSETGROUP_JS_DELETE";  //for Location - before Delete confirmation

        // Division

        public const string Division_JS_DELETE = "Division_JS_DELETE";

        // ApplicationType

        public const string APPTYPE_JS_DELETE = "APPTYPE_JS_DELETE";

        // ApplicationCriticality

        public const string APPCRITI_JS_DELETE = "APPCRITI_JS_DELETE";


        // Mustering Reason
        public const string DREASON_JS_DELETE = "DREASON_JS_DELETE";  //for Location - before Delete confirmation

        // Mustering Reason
        public const string MUSTERREASON_JS_DELETE = "MUSTERREASON_JS_DELETE";  //for Location - before Delete confirmation

        //Department
        public const string DP_JS_DELETE = "DP_JS_DELETE";  //for Department - before Delete confirmation

        //v454
        //Owner 
        public const string OWNER_JS_DELETE = "OWNER_JS_DELETE"; //for Owner  - before Delete confirmation

        public const string ASSET_E_INVALIDASSET = "ASSET_E_INVALIDASSET";

        //ASSET Search
        public const string ASSETSEARCH_W_BU_NOTSET = "ASSETSEARCH_W_BU_NOTSET";
        public const string ASSETSEARCH_W_DEPT_NOTSET = "ASSETSEARCH_W_DEPT_NOTSET";
        public const string ASSETSEARCH_W_LOC_NOTSET = "ASSETSEARCH_W_DEPT_NOTSET";
        public const string ASSETSEARCH_E_USERNOTSET = "ASSETSEARCH_E_USERNOTSET";


        //User
        public const string USER_IS_INACTIVE = "USER_E_ACCOUNT_INACTIVE";

        // Purpose
        public const string PURPOSE_JS_DELETE = "PURPOSE_JS_DELETE";  //for Purpose - before Delete confirmation

        //Purpose
        public const string PS_JS_DELETE = "PS_JS_DELETE";  //for Purpose - before Delete confirmation

        //public const string ASSET_E_INVALIDASSET = "ASSET_E_INVALIDASSET";

        //Site-Location Assignment
        public const string SPECIAL_ROOM_DEASSIGN_ERROR = "SPECIAL_ROOM_DEASSIGN_ERROR";

        //Tenant
        public const string TENANT_JS_DELETE = "TENANT_JS_DELETE"; //for Tenant  - before Delete confirmation

    };

    public class TransactionTypes
    {
        public const string EVENT_ASSET_TRANSFER = "ASSET_TRANSFER";
        public const string EVENT_ASSET_CREATE = "ASSET_CREATE";
        public const string EVENT_ASSET_UPDATE = "ASSET_UPDATE";
        public const string EVENT_ASSET_BAR = "ASSET_BAR";
        public const string EVENT_ASSET_RECEIVE = "ASSET_RECEIVE";
        public const string EVENT_ASSET_ISSUE = "ASSET_ISSUE";
        public const string EVENT_ASSET_RETURN = "ASSET_RETURN";
        public const string EVENT_ASSET_TRANSFER_CANCEL = "ASSET_TRANSFER_CANCEL";
    }

    public class DBStaticValues
    {
        public const int DOC_SEARCH_ONLY_IF_NULL = -1;
        public const int DOC_SEARCH_ONLY_IF_NOT_NULL = -2;
        public const int DOC_SEARCH_ANY_VALUE = 0;
    }
}