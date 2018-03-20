using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.Domain
{
    public class School
    {
        [Key]
        public string SchoolId { get; set; }

        [StringLength(205)]
        public string Name { get; set; }

        [StringLength(105)]
        public string Alias { get; set; }

        [StringLength(135)]
        public string Motto { get; set; }

        [StringLength(135)]
        public string SchoolWebsite { get; set; }

        [StringLength(135)]
        public string Country { get; set; }

        public bool IsActive { get; set; }

        [StringLength(135)]
        public string Email { get; set; }

        [StringLength(25)]
        public string Color { get; set; }

        [StringLength(135)]
        public string OwernshipType { get; set; }

        public DateTime? DateOfEstablishment { get; set; }

        [StringLength(175)]
        public string Address { get; set; }

        [StringLength(75)]
        public string LocalGovtArea { get; set; }

        [StringLength(45)]
        public string State { get; set; }

        public DateTime? SubscriptionDate { get; set; }

        public DateTime SubscriptionEndDate
        {
            get
            {
                if (SubscriptionDate != null)
                {
                    var t = Convert.ToDateTime(SubscriptionDate);
                    return t.AddDays(60);
                }
                return DateTime.Now;
            }
            set { }
        }

        public bool IsTrialAccount { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }

        public byte[] Logo { get; set; }
        public byte[] SchoolBanner { get; set; }

        public virtual ICollection<RemitaFeeSetting> RemitaFeeSettings { get; set; }
        public virtual ICollection<FeeType> FeeTypes { get; set; }
    }
}