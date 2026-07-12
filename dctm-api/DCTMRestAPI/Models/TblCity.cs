using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblCity
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int CountryId { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
