using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblEnclPositions
    {
        public int Id { get; set; }
        public int EnclId { get; set; }
        public string FrontPositions { get; set; }
        public string RearPositions { get; set; }
        public long LastUpdatedTime { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
    }
}
