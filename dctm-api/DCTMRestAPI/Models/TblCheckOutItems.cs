using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblCheckOutItems
    {
        public long CheckOutSessionId { get; set; }
        public int CheckOutAssetId { get; set; }
        public bool? IsRfidCheckOut { get; set; }
        public int? PurposeId { get; set; }
        public int? DestinationLocationId { get; set; }
        public long LastUpdatedTime { get; set; }
        public Guid? Id { get; set; }
    }
}
