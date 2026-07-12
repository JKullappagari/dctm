using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DCTMRestAPI.Models
{
    public partial class TblLocation
    {
        public TblLocation()
        {
        }

        public int LocationId { get; set; }

        [StringLength(50, ErrorMessage = "Location: maximum of 50 characters allowed.")]
        [RegularExpression(@"^[\w\-\.]+(\s{1}[\w\-\.]+)*\s{0,1}$", ErrorMessage = "Location: alphanumeric, hyphen,underscore,dot and single space only allowed.")]
        public string Location { get; set; }
        public string Description { get; set; }
        public bool? Status { get; set; }
        public bool IsExitDoor { get; set; }
        public int? LocationTypeId { get; set; }
        public string FloorNo { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public string IpAddress { get; set; }
        public int? ParentLocationId { get; set; }
        public bool IsCheckOutLocation { get; set; }

        [StringLength(24, ErrorMessage = "Rack tag: maximum of 50 characters allowed.")]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "Rack tag: enter Alphanumeric only.")]
        public string TagId { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string ExternalId { get; set; }
        public bool IsSpecialRoom { get; set; }

        [StringLength(50, ErrorMessage = "Serial Number: maximum of 50 characters allowed.")]
        [RegularExpression(@"^[\w0-9\-\.]+([\w0-9\-\.]+)*$", ErrorMessage = "Serial Number: alphanumeric, hyphen, underscore and dot only allowed.")]
        public string SerialNumber { get; set; }
        public int? ModelId { get; set; }
        public int Uheight { get; set; }
        public long LastUpdatedTime { get; set; }

    }
}
