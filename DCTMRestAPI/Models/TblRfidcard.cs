using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblRfidcard
    {
        public int AutoId { get; set; }
        public string RfidcardNumber { get; set; }
        public int? StatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public byte RfidcardType { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
