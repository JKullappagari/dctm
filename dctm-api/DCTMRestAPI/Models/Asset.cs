using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class Asset
    {
        public int AssetId { get; set; }
        public string RefNumber { get; set; }
        public int AssetGroupId { get; set; }
        public int ModelId { get; set; }
        public int? TechId { get; set; }
        public string RackorStand { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string AssetName { get; set; }
        public int BusinessUnitId { get; set; }
        public int PrimarySiteId { get; set; }
        public DateTime? AssetCreatedDate { get; set; }
        public int? AssetCreatedBy { get; set; }
        public string CurrentRfidcardNumber { get; set; }
        public DateTime? RfidassignDate { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsPermRestrict { get; set; }
        public bool? IsMustered { get; set; }
        public DateTime? BarredStartDate { get; set; }
        public DateTime? BarredEndDate { get; set; }
        public int? IssuedBy { get; set; }
        public DateTime? IssuedDate { get; set; }
        public int? IssuedTo { get; set; }
        public int? ReceivedBy { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public int CurrentStatusId { get; set; }
        public int LastSeenLocationId { get; set; }
        public DateTime LastSeenLocationTime { get; set; }
        public int CurrentOwnerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public int? LastValidationResult { get; set; }
        public DateTime? LastGantryUpdatedTime { get; set; }
        public int? ParentAssetId { get; set; }
        public bool IsParent { get; set; }
        public int? MusterReasonId { get; set; }
        public string CurrentOwnerRfidbadge { get; set; }
        public int? DefaultLocationId { get; set; }
        public string Os { get; set; }
        public string Cpu { get; set; }
        public int? Cpucount { get; set; }
        public string Cpucore { get; set; }
        public int? StartPos { get; set; }
        public int? NoOfRus { get; set; }
        public bool? IsWriteOff { get; set; }
        public DateTime? RfidupdatedDateTime { get; set; }
        public string Orientation { get; set; }
        public string InternalId { get; set; }
        public string ExternalId { get; set; }
        public int? WriteOffReasonId { get; set; }
        public bool IsChild { get; set; }
        public double DeratedPower { get; set; }
    }
}
