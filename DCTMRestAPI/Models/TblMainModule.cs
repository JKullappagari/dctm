using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblMainModule
    {
        public int MainModuleId { get; set; }
        public string MainModule { get; set; }
        public int SortOrder { get; set; }
        public bool Status { get; set; }
        public int ModuleType { get; set; }
        public string PageUrl { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
