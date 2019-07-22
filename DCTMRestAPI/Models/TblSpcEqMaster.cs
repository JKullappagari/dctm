using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblSpcEqMaster
    {
        public long SpcId { get; set; }
        public string MakeModel { get; set; }
        public string ProductNumber { get; set; }
        public double? DepthInches { get; set; }
        public double? DepthMm { get; set; }
        public double? WidthInches { get; set; }
        public double? WidthMm { get; set; }
        public double? HeightInches { get; set; }
        public double? HeightMm { get; set; }
        public double? WeightLb { get; set; }
        public double? WeightKg { get; set; }
        public double? SqftStandalone { get; set; }
        public double? SqmetreStandalone { get; set; }
        public string Empty2 { get; set; }
        public double? SteadyStateWatts { get; set; }
        public double? MaxWatts { get; set; }
        public string Notes1 { get; set; }
        public string Notes2 { get; set; }
        public string ItemType { get; set; }
        public string Path { get; set; }
        public long? NewRowRefId { get; set; }
        public string SourceFile { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
