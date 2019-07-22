using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblOwnerDivisionAssignment
    {
        public int OwnerDivId { get; set; }
        public int OwnerId { get; set; }
        public int DivisionId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
