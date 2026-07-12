using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblExternalSync
    {
        public int ExternalSyncId { get; set; }
        public string ExternalSystemName { get; set; }
        public DateTime LastSyncDateTime { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
