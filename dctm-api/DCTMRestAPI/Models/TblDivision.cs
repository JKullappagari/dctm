using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblDivision
    {
        public TblDivision()
        {
        }

        public int DivisionId { get; set; }
        public string Division { get; set; }
        public string DivisionDesc { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public long LastUpdatedTime { get; set; }

    }
}
