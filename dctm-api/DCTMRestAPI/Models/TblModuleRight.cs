using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblModuleRight
    {
        public TblModuleRight()
        {
        }

        public int RightModuleId { get; set; }
        public int RightId { get; set; }
        public int ModuleId { get; set; }
        public string RightType { get; set; }
        public string TabDisplay { get; set; }
        public string KeyValue { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long LastUpdatedTime { get; set; }

        public TblModule Module { get; set; }
        public TblRight Right { get; set; }
    }
}
