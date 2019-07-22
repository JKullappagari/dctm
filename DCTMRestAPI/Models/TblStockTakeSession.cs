using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblStockTakeSession
    {
        public Guid Id { get; set; }
        public long StockTakeSessionId { get; set; }
        public int StockTakeLocationId { get; set; }
        public int StockTakeUserId { get; set; }
        public string WorkflowInstanceId { get; set; }
        public DateTime StockTakeDateTime { get; set; }
        public int? DeviceId { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
