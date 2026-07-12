using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblAssetModel
    {
        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public int MfgId { get; set; }
        public long? Spcid { get; set; }
        public int? TechId { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public int? UpdateMethod { get; set; }
        public string Comment { get; set; }
        public int BusinessUnitId { get; set; }
        public bool IsBlade { get; set; }
        public bool IsEnclosure { get; set; }
        public double HeightMm { get; set; }
        public double DepthMm { get; set; }
        public double WidthMm { get; set; }
        public double WeightKg { get; set; }
        public byte Uheight { get; set; }
        public double MaxPowerWatts { get; set; }
        public double SteadyStateWatts { get; set; }
        public int AssetTypeId { get; set; }
        public byte TotalPsucount { get; set; }
        public byte RequiredPsucount { get; set; }
        public string ConnectorTypePduside { get; set; }
        public string ConnectorTypeDeviceSide { get; set; }
        public int? MountTypeId { get; set; }
        public int? AirFlowDirectionId { get; set; }
        public double InternalHeightRack { get; set; }
        public double InternalDepthRack { get; set; }
        public double InternalWidthRack { get; set; }
        public long LastUpdatedTime { get; set; }

    }
}
