using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblUserPassword
    {
        public int UserPasswordId { get; set; }
        public int UserId { get; set; }
        public string Password { get; set; }
        public bool? Status { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long LastUpdatedTime { get; set; }

    }
}
