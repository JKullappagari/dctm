using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblCheckDelete
    {
        public int ColumnTableId { get; set; }
        public string ColumnName { get; set; }
        public string ColumnNameAlias { get; set; }
        public string TableName { get; set; }
        public bool? TableStatusCheck { get; set; }
        public bool Flag { get; set; }
        public bool Status { get; set; }
        public int? SortOrder { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
