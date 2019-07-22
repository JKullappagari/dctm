using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblBusiteAssignment
    {
        public int BusiteAssignmentId { get; set; }
        public int BusinessUnitId { get; set; }
        public int SiteId { get; set; }
        public bool? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public long LastUpdatedTime { get; set; }

    }
}
