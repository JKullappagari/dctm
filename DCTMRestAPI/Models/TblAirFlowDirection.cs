using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblAirFlowDirection
    {
        public int Id { get; set; }
        public string AirFlowDirection { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
