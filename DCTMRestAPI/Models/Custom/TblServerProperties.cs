using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCTMRestAPI.Models.Custom
{
    public class TblServerProperties
    {
        public int Id { get; set; }
        public DateTime ServerDateTimeUtc { get; set; }
        public DateTime ServerDateTimeLocal { get; set; }
    }
}
