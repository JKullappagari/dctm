using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblApplicationMap
    {
        public Guid ApplMapId { get; set; }
        public int ApplId { get; set; }
        public Guid Id { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public long LastUpdatedTime { get; set; }

    }
}
