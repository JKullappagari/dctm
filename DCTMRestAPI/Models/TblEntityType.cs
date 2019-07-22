using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblEntityType
    {
        public TblEntityType()
        {
        }

        public short EntityTypeId { get; set; }
        public string EntityType { get; set; }
        public string Description { get; set; }
        public long LastUpdatedTime { get; set; }

    }
}
