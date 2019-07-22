using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblApplCriticality
    {
        public int ApplCriticalityId { get; set; }
        public string ApplCriticality { get; set; }
        public string ApplCriticalityDesc { get; set; }
        public bool Status { get; set; }
        public string BackColorCode { get; set; }
        public string ForeColorCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
