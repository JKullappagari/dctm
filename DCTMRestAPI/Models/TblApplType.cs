using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblApplType
    {
        public int ApplTypeId { get; set; }
        public string ApplType { get; set; }
        public string ApplTypeDesc { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
