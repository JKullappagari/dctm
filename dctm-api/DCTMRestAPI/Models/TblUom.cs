using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblUom
    {
        public int Id { get; set; }
        public string Uomfrom { get; set; }
        public string Uomto { get; set; }
        public double Factor { get; set; }
        public int? Category { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
