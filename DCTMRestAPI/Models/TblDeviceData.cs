using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblDeviceData
    {
        public int Sno { get; set; }
        public string DeviceId { get; set; }
        public DateTime FirstInstallDateTime { get; set; }
        public DateTime LatestInstallDateTime { get; set; }
        public int Count { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
