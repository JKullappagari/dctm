using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblAssetDrwdata
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public int StatusId { get; set; }
        public int ReasonId { get; set; }
        public string Comments { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
