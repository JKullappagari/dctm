using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblAssetTransactionLog
    {
        public Guid Id { get; set; }
        public long TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public int TransactionType { get; set; }
        public int TransactionStatus { get; set; }
        public int? TransactionUser { get; set; }
        public string GantryLocation { get; set; }
        public int? AssetId { get; set; }
        public int? AssetGroupId { get; set; }
        public int? NoOfPages { get; set; }
        public string AssetName { get; set; }
        public string RefNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? BusinessUnitId { get; set; }
        public int? SiteId { get; set; }
        public string RfidcardNumber { get; set; }
        public bool? IsPermRestrict { get; set; }
        public bool? IsMustered { get; set; }
        public DateTime? BarredStartDate { get; set; }
        public DateTime? BarredEndDate { get; set; }
        public int? IssuedBy { get; set; }
        public DateTime? IssuedDate { get; set; }
        public int? IssuedTo { get; set; }
        public int? ReceivedBy { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public int? LocationId { get; set; }
        public int? CurrentOwnerId { get; set; }
        public DateTime? LastSeenLocationTime { get; set; }
        public int? LastMovedPurposeId { get; set; }
        public int? LastCheckOutDestinationId { get; set; }
        public int? DeviceId { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
