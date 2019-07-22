using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblTenantApplicationAssignment
    {
        public int TenantApplicationId { get; set; }
        public int TenantId { get; set; }
        public int ApplicationId { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public long LastUpdatedTime { get; set; }

        public virtual TblApplication Application { get; set; }
    }
}
