using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblRight
    {
        public TblRight()
        {
        }

        public int RightsId { get; set; }
        public string Rights { get; set; }
        public string Description { get; set; }
        public bool? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public long LastUpdatedTime { get; set; }

    }
}
