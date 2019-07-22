using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblExternalSysInterfaceErrors
    {
        public long ErrorId { get; set; }
        public int ImportObjectId { get; set; }
        public string FileName { get; set; }
        public int RowNumber { get; set; }
        public string RowData { get; set; }
        public string FailureReason { get; set; }
        public DateTime ProcessedDate { get; set; }
        public long Id { get; set; }
        public long LastUpdatedTime { get; set; }

    }
}
