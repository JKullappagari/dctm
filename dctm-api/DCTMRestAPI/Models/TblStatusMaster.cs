using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblStatusMaster
    {
        public TblStatusMaster()
        {
        }

        public int StatusId { get; set; }
        public short EntityTypeId { get; set; }
        public string Action { get; set; }
        public string Status { get; set; }
        public string RefIdremarks { get; set; }
        public string Description { get; set; }
        public bool? Selectable { get; set; }
        public bool? CascadeStatus { get; set; }
        public bool? CascadeHistory { get; set; }
        public string Sqlcondition { get; set; }
        public long LastUpdatedTime { get; set; }

    }
}
