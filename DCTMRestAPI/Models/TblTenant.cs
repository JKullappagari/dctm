using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblTenant
    {
        public int TenantId { get; set; }
        public string TenantFullName { get; set; }
        public string TenantShortName { get; set; }
        public string TenantType { get; set; }
        public int TenantTypeSize { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public long LastUpdatedTime { get; set; }
        public int UserCount { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactEmail { get; set; }
    }
}
