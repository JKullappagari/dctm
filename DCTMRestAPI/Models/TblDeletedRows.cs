using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblDeletedRows
    {
        public Guid Id { get; set; }

        public string TableName { get; set; }

        public Guid PrimaryId { get; set; }

        public long? SecondaryId { get; set; }

        public DateTime? DeletedTime { get; set; }

        public int UpdatedUserId { get; set; }

        public long LastUpdatedTime { get; set; }
    }
}
