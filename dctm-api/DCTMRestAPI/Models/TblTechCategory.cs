using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblTechCategory
    {
        public int TechId { get; set; }
        public string TechName { get; set; }
        public string Description { get; set; }
        public bool? Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
