using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblMobileDevice
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public int SiteId { get; set; }
        public bool Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LocationId { get; set; }
        public string ShortCode { get; set; }
        public long LastUpdatedTime { get; set; }

    }
}
