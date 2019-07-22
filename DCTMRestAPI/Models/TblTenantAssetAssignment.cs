using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblTenantAssetAssignment
    {
        public int TenantAssetId { get; set; }
        public int TenantId { get; set; }
        public int AssetId { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public long LastUpdatedTime { get; set; }

        public virtual TblAsset Asset { get; set; }
    }
}
