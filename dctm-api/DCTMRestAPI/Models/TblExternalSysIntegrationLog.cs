using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblExternalSysIntegrationLog
    {
        public TblExternalSysIntegrationLog()
        {
        }

        public long Id { get; set; }
        public string FileName { get; set; }
        public int? NoOfRecords { get; set; }
        public int? SuccessRecords { get; set; }
        public int? FailureRecords { get; set; }
        public DateTime ProcessedDate { get; set; }
        public string TransactionType { get; set; }
        public string Status { get; set; }
        public string ExternalSystem { get; set; }
        public long LastUpdatedTime { get; set; }

    }
}
