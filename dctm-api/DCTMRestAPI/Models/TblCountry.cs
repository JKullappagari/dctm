using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblCountry
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public int? WorldRegionId { get; set; }
        public string Region { get; set; }
        public int? Uomid { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
