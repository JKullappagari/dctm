using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblWorldRegion
    {
        public int WorldRegionId { get; set; }
        public string WorldRegion { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
