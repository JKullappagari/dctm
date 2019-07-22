using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblAssetSearch
    {
        public string Site { get; set; }
        public string Location { get; set; }
        public string Custodian { get; set; }
        public string Assettype { get; set; }
        public string Tagno { get; set; }
        public string Manufacturer { get; set; }
        public string Assetmodel { get; set; }
        public string Hostname { get; set; }
        public string Serialno { get; set; }
        public string Assetname { get; set; }
    }
}
