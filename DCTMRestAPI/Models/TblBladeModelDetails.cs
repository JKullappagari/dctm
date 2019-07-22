using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblBladeModelDetails
    {
        public int Id { get; set; }
        public int? BladeModelId { get; set; }
        public byte? BladeRowCount { get; set; }
        public byte? BladeColumnCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
