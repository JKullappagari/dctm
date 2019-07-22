using System;
using System.Collections.Generic;

namespace DCTMRestAPI.Models
{
    public partial class TblUser
    {
        public TblUser()
        {
        }

        public int UserId { get; set; }
        public string LoginName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? DefaultBu { get; set; }
        public int? DefaultSite { get; set; }
        public int? DefaultLocation { get; set; }
        public bool IsUserSelectionAllowed { get; set; }
        public bool? UserType { get; set; }
        public bool? Status { get; set; }
        public int? FailedLoginAttempts { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public string DisplayName { get; set; }
        public string CurrentRfidbadge { get; set; }
        public DateTime? RfidassignDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool? SiteRestriction { get; set; }
        public bool IsFirstLogin { get; set; }
        public long LastUpdatedTime { get; set; }

    }
}
