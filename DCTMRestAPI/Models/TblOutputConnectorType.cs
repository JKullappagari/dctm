using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblOutputConnectorType
    {
        public int OutputConnectorTypeId { get; set; }
        public string OutputConnectorType { get; set; }
        public long LastUpdatedTime { get; set; }
    }
}
