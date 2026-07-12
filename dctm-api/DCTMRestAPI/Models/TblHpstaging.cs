using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblHpstaging
    {
        public int AssetId { get; set; }
        public string AssetName { get; set; }
        public string Os { get; set; }
        public string RefNumber { get; set; }
        public string CurrentRfidcardNumber { get; set; }
        public int? ParentAssetId { get; set; }
        public string AssetGroup { get; set; }
        public string SiteName { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string Region { get; set; }
        public string ModelName { get; set; }
        public string MfgName { get; set; }
        public string Rack { get; set; }
        public string Row { get; set; }
        public string Room { get; set; }
        public string FloorNo { get; set; }
        public bool? IsParent { get; set; }
        public int? DestIdentity { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
