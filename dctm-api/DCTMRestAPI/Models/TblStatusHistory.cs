using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblStatusHistory
    {
        public Guid Id { get; set; }
        public long StatusHistoryId { get; set; }
        public short EntityTypeId { get; set; }
        public long EntityId { get; set; }
        public DateTime StatusDate { get; set; }
        public int StatusId { get; set; }
        public string RefId { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? DeviceId { get; set; }

        public int LocationID { get; set; }

        public long LastUpdatedTime { get; set; }

    }
}
