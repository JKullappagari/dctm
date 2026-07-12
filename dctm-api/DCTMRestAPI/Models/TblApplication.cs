using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblApplication
    {
        public TblApplication()
        {
        }

        public int ApplId { get; set; }
        public int Buid { get; set; }
        public string ApplName { get; set; }
        public string ApplDesc { get; set; }
        public int? ApplTypeId { get; set; }
        public int? ApplCriticalityId { get; set; }
        public int? ApplManageId { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public int? OwnerId { get; set; }
        public int AppStatusId { get; set; }
        public long LastUpdatedTime { get; set; }

    }
}
