using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblPurpose
    {
        public int PurposeId { get; set; }
        public string Purpose { get; set; }
        public string Description { get; set; }
        public bool? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
