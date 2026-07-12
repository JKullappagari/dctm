using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblSite
    {
        public TblSite()
        {
        }

        public int SiteId { get; set; }
        public string Site { get; set; }
        public string Description { get; set; }
        public bool? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public string Url { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public long LastUpdatedTime { get; set; }

    }
}
