/*
File Name:      iSRPSharedConstants.cs
Description:	Define constants for Membership, Membership Error, Password Policy and ACL Trustee.
Date created:	08 Aug 2007

Modification History:
*********************
CR		Name			            Date			Description
New		Sivashanmugam, Muniappan    08/08/2007		File has been created.
Changed	Sivashanmugam, Muniappan    03/10/2007		File has been modified to remove unused classes.
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace iAssetTrack.Security
{
    public static class MembershipAttributeConstants
    {
        public const string cn = "cn";
        public const string uid = "uid";
        public const string name = "name";
        public const string givenName = "givenName";
        public const string displayName = "displayName";
        public const string distinguishedName = "distinguishedName";
        public const string initials = "initials";
        public const string mail = "mail";
        public const string o = "o";
        public const string memberOf = "memberOf";
        public const string codePage = "codePage";
        public const string samAccountType = "samAccountType";
        public const string samAccountName = "samAccountName";
        public const string localeID = "localeID";
        public const string accountLocked = "accountLocked";
        public const string accountExpires = "accountExpires";
        public const string pwdLastSet = "pwdLastSet";
        public const string pwdNeverExpires = "pwdNeverExpires";
        public const string lastLogon = "lastLogon";
        public const string lastLogonTimestamp = "lastLogonTimestamp";
        public const string badPasswordTime = "badPasswordTime";
        public const string badPwdCount = "badPwdCount";
        public const string objectClass = "objectClass";
        public const string objectGuid = "objectGuid";
        public const string userAccountControl = "userAccountControl";
        public const string userAccountDisabled = "userAccountDisabled";
        public const string userAccountExpired = "userAccountExpired";
        public const string ntSecurityDescriptor = "ntSecurityDescriptor";
        public const string userPrincipalName = "userPrincipalName";
        public const string adsPath = "adsPath";
    }

    public static class MembershipErrorConstants
    {
        public const String ADERR_SERVER_CONFIGURATIONSETTINGS_NOT_FOUND = "ADERR_SERVER_CONFIGURATIONSETTINGS_NOT_FOUND";
        public const String ADERR_LOGON_FAILURE_UNKNOWN_USERNAME_OR_PWD = "ADERR_LOGON_FAILURE_UNKNOWN_USERNAME_OR_PWD";
        public const String ADERR_USER_ACCOUNT_LOCKED = "ADERR_USER_ACCOUNT_LOCKED";
        public const String ADERR_USER_ACCOUNT_DISABLED = "ADERR_USER_ACCOUNT_DISABLED";
        public const String ADERR_USER_ACCOUNT_EXPIRED = "ADERR_USER_ACCOUNT_EXPIRED";
        public const String ADERR_PWDCHANGE_FAILURE_UNKNOWN_USERNAME_OR_PWD = "ADERR_PWDCHANGE_FAILURE_UNKNOWN_USERNAME_OR_PWD";
        public const String ADERR_PWDCHANGE_FAILURE_INVALID_PWD = "ADERR_PWDCHANGE_FAILURE_INVALID_PWD";
        public const String ADERR_PWDCHANGE_FAILURE_CANNOT_CHANGE_PWD = "ADERR_PWDCHANGE_FAILURE_CANNOT_CHANGE_PWD";
        public const String ADERR_PWDCHANGE_FAILURE_USER_ACCOUNT_DISABLED = "ADERR_PWDCHANGE_FAILURE_USER_ACCOUNT_DISABLED";
        public const String ADERR_PWDCHANGE_FAILURE_PWD_DOES_NOT_MEET_REQ = "ADERR_PWDCHANGE_FAILURE_PWD_DOES_NOT_MEET_REQ";
        public const String ADERR_ADINSTANCE_INVALID_INSTANCE = "ADERR_ADINSTANCE_INVALID_INSTANCE";
        public const String ADERR_ADINSTANCE_UNRECOGNIZED_SCHEMA_CLASS = "ADERR_ADINSTANCE_UNRECOGNIZED_SCHEMA_CLASS";
        public const String ADERR_USER_UNKNOWN_USER = "ADERR_USER_UNKNOWN_USER";
        public const String ADERR_WINDOWSIMPERSONATIONCONTEXT_FAILED = "ADERR_WINDOWSIMPERSONATIONCONTEXT_FAILED";
    }
}
