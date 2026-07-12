using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblAuditCycle
    {
        public long Id { get; set; }
        public int CycleCount { get; set; }
        public int LocationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
