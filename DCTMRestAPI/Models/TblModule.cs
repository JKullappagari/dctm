using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblModule
    {
        public TblModule()
        {
        }

        public int ModuleId { get; set; }
        public string Module { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }
        public bool Status { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? MainModuleId { get; set; }
        public string PageUrl { get; set; }
        public long LastUpdatedTime { get; set; }

    }
}
