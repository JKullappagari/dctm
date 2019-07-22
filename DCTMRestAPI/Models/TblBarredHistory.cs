using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblBarredHistory
    {
        public int BarredHistoryId { get; set; }
        public long StatusHistoryRefId { get; set; }
        public DateTime BarredPeriodStart { get; set; }
        public DateTime BarredPeriodEnd { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
