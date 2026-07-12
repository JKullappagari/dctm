using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblTransactionTypes
    {
        public int TransTypeId { get; set; }
        public string TransTypeCode { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public string RefIdremarks { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
