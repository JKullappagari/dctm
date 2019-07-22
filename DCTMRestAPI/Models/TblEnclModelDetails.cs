using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblEnclModelDetails
    {
        public int Id { get; set; }
        public int? EnclModelId { get; set; }
        public byte? EnclFrontRowCount { get; set; }
        public byte? EnclFrontColumnCount { get; set; }
        public byte? EnclRearRowCount { get; set; }
        public byte? EnclRearColumnCount { get; set; }
        public byte? FrontChildCount { get; set; }
        public byte? RearChildCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
