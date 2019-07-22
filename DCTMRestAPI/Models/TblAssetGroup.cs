using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblAssetGroup
    {
        public int AssetGroupId { get; set; }
        public string AssetGroup { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
