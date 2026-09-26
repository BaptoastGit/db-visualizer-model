using System;
using System.Collections.Generic;
using System.Text;

namespace Model.BO.Data.Model
{
    public partial class User
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Role { get; set; }
        public string? Status { get; set; }
        public DateTime? LastLoginOn { get; set; }
        public int FailedLoginAttempts { get; set; }
        public bool IsTwoFactorEnabled { get; set; }
        public DateTime CreatedOn { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsPhoneConfirmed { get; set; }

        public string? Locale { get; set; }
        public string? TimeZone { get; set; }

        public string? LastLoginIp { get; set; }
        public DateTime? LastPasswordChangedOn { get; set; }
        public bool RequiresPasswordChange { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }

        public DateTime? GdprConsentOn { get; set; }
        public bool MarketingOptIn { get; set; }

        public string? InternalNotes { get; set; }
        public string? CreatedBy { get; set; }
    }
}
