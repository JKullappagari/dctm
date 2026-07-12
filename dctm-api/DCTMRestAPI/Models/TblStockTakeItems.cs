using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblStockTakeItems
    {
        public Guid Id { get; set; }
        public long StockTakeSessionId { get; set; }
        public long AssetId { get; set; }
        public int StatusId { get; set; }
        public bool IsUpdated { get; set; }

        public int? StartPos { get; set; }

        public string Orientation { get; set; }

        public int? ParentAssetID { get; set; }

        public string ValidationMsg { get; set; }

        public bool? IsValidated { get; set; }

        public long LastUpdatedTime { get; set; }
    }
}
