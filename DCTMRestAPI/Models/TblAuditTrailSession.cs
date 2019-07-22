using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblAuditTrailSession
    {
        public long AuditTrailSessionId { get; set; }
        public int? UserId { get; set; }
        public DateTime? LogonDate { get; set; }
        public DateTime? LogoffDate { get; set; }
        public string Ipaddress { get; set; }
        public string SessionId { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
