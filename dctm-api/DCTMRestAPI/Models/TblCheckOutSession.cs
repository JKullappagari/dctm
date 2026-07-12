using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblCheckOutSession
    {
        public Guid Id { get; set; }
        public long CheckOutSessionId { get; set; }
        public string CheckOutWorkflowId { get; set; }
        public DateTime CheckOutDateTime { get; set; }
        public int CheckOutUserId { get; set; }
        public int CheckOutLocationId { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
