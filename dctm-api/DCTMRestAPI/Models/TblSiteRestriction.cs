using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblSiteRestriction
    {
        public int SiteRestrictionsId { get; set; }
        public int? SiteId { get; set; }
        public int? UserId { get; set; }
        public bool? Read { get; set; }
        public bool? Write { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
