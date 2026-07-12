using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblMessage
    {
        public int MessageCodeId { get; set; }
        public string MessageCode { get; set; }
        public string Message { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
