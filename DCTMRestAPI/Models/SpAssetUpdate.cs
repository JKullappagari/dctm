using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DCTMRestAPI.Models
{
    public partial class SpAssetUpdate
    {
        public int pIntResult { get; set; }
        public string pVarMessageCode { get; set; }

        public int pIntAssetID { get; set; }
    }
}
