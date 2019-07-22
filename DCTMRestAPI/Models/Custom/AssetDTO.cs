using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DCTMRestAPI.Models.Custom
{
    public class AssetDTO
    {
        public int AssetId { get; set; }

        [StringLength(100, ErrorMessage = "Serial Number: maximum of 100 characters allowed.")]
        [RegularExpression(@"^[\w\-\.]+(\s{1}[\w\-\.]+)*\s{0,1}$", ErrorMessage = "Alphanumeric, hyphen, underscore, dot and single space only allowed for Serial Number")]
        public string RefNumber { get; set; }
        public int AssetGroupId { get; set; }
        public int ModelId { get; set; }
        public int? TechId { get; set; }
        public string RackorStand { get; set; }
        public DateTime? ExpiryDate { get; set; }

        [StringLength(100,ErrorMessage = "Assetname: Maximum of 100 characters allowed")]
        [RegularExpression(@"^[\w\-\.]+(\s{1}[\w\-\.]+)*\s{0,1}$",ErrorMessage = "Alphanumeric, hyphen, underscore, dot and single space only allowed for Asset Name")]
        public string AssetName { get; set; }
        public int BusinessUnitId { get; set; }
        public int PrimarySiteId { get; set; }
        public DateTime? AssetCreatedDate { get; set; }
        public int? AssetCreatedBy { get; set; }
        [StringLength(24,ErrorMessage = "Asset tag: Maximum of 24 characters allowed")]
        [RegularExpression(@"^[A-Za-z0-9]+$",ErrorMessage = "Enter Alphanumeric only for Barcode/EPC")]
        [DisplayName("Assettag")]
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

        [DisplayName("LocationID")]
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

        [DisplayName("Uposition")]
        public int? StartPos { get; set; }
        public int? NoOfRus { get; set; }
        public bool? IsWriteOff { get; set; }
        public DateTime? RfidupdatedDateTime { get; set; }

        [StringLength(50, ErrorMessage = "Orientation: Maximum of 50 characters allowed")]
        [RegularExpression(@"^[A-Za-z\-]+$", ErrorMessage = "Enter Alphanumeric only for Barcode/EPC")]
        public string Orientation { get; set; }
        public string InternalId { get; set; }
        public string ExternalId { get; set; }
        public int? WriteOffReasonId { get; set; }
        public bool IsChild { get; set; }
        public double DeratedPower { get; set; }
        public long LastUpdatedTime { get; set; }

        //public int Result { get; set; }
        //public string MessageCode { get; set; }

        //public string HostName { get; set; }


    }
}
