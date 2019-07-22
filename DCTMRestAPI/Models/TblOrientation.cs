using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblOrientation
    {
        public int OrientationId { get; set; }
        public string OrientationName { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
