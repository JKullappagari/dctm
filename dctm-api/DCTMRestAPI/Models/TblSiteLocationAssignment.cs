using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblSiteLocationAssignment
    {
        public int SiteLocationAssignmentId { get; set; }
        public int? SiteId { get; set; }
        public int LocationId { get; set; }
        public int? BusinessUnitId { get; set; }
        public bool? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public long LastUpdatedTime { get; set; }

    }
}
