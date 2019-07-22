using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblAssetTransactionLogTagsSeen
    {
        public long TransactionId { get; set; }
        public string TagId { get; set; }
        public int TagStatus { get; set; }
        public short? EntityType { get; set; }
        public int? EntityId { get; set; }
        public DateTime CreatedDate { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
