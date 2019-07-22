using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblAppStatus
    {
        public int AppStatusId { get; set; }
        public string AppStatus { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
