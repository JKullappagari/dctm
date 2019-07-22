using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblUserBusinessUnit
    {
        public int UserBusinessUnitId { get; set; }
        public int BusinessUnitId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public long LastUpdatedTime { get; set; }

    }
}
