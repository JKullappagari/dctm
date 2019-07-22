using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblAssetExcel
    {
        public int AssetID { get; set; }
        public string SerialNo { get; set; }

        public string AssetType  { get; set; }
        public string Manufacturer { get; set; }
        public string AssetModel { get; set; }
        public string Assetname { get; set; }
        public string Mounttype { get; set; }
        public int? StartRU { get; set; }
        public int? EndRU { get; set; }
        public string Orientation { get; set; }
        public string Site { get; set; }
        public string Room { get; set; }
        public string Row { get; set; }
        public string Rack { get; set; }
        public string RackTag { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string Tagno { get; set; }
        public string Hostname { get; set; }
        public DateTime CreatedDate { get; set; }

        public string Creator { get; set; }
        public string Custodian { get; set; }
        public string OS { get; set; }
        public string CPU { get; set; }
        public int? CPUCount { get; set; }
        public string CPUCore { get; set; }

        public int LocationID { get; set; }

        public int ParentAssetID { get; set; }

        public int ModelID { get; set; }
        public int MfgID { get; set; }

        public string AssetStatus { get; set; }
        //Application
        //ApplicationCriticality
        //ApplicationType
        //ApplicationDivision


    }
}
