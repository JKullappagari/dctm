using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblGroupModuleRight
    {
        public int GroupModuleRightId { get; set; }
        public int GroupId { get; set; }
        public int RightModuleId { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public long LastUpdatedTime { get; set; }

    }
}
