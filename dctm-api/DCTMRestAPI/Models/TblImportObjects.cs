using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblImportObjects
    {
        public TblImportObjects()
        {
        }

        public int ImportObjectId { get; set; }
        public string ImportObject { get; set; }
        public long LastUpdatedTime { get; set; }

    }
}
