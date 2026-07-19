using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for EnterpriseLoggingUtil
/// </summary>
public class EnterpriseLoggingUtil
{
    public EnterpriseLoggingUtil()
    {
        //
        // TODO: Add constructor logic here
        //
    }


    public class LogPriority
    {
        public const int LOG_PRIORITY_LOW = 3;
        public const int LOG_PRIORITY_MEDIUM = 2;
        public const int LOG_PRIORITY_HIGH = 1;
    }

    public class LogCategory
    {
        public const string LOG_CATEGORY_AUDIT = "Audit";
        public const string LOG_CATEGORY_EXCEPTION = "Exception";
        public const string LOG_CATEGORY_VIOLATION = "Violation";
    }

    public class LogExtendedProps
    {
        public const string LOG_PROP_TRANSACTIONSTATUSID = "Transaction Status";
        public const string LOG_PROP_LOGONUSERID = "Logon User";
        public const string LOG_PROP_DOCUMENTID = "Document ID";
        public const string LOG_PROP_DOCUMENTOWNERID = "Document Owner ID";
    }
}
