using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DCTMRestAPI.Models
{
    public partial class TblCheckoutPurpose
    {
        [Required]
        public Guid Id { get; set; }
        public int CheckoutPurposeId { get; set; }
        public Guid CheckoutSessionId { get; set; }
        public int TotalCoutItems { get; set; }
        public int CinItems { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
