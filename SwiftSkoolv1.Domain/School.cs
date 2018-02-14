using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.Domain
{
    public class School
    {
        [Key]
        public string SchoolId { get; set; }

        [StringLength(105)]
        public string Name { get; set; }

        [StringLength(105)]
        public string Alias { get; set; }

        [StringLength(35)]
        public string Motto { get; set; }

        [StringLength(35)]
        public string SchoolWebsite { get; set; }

        [StringLength(35)]
        public string Country { get; set; }


        public bool IsActive { get; set; }

        [StringLength(35)]
        public string Email { get; set; }
        [StringLength(15)]
        public string Color { get; set; }

        [StringLength(35)]
        public string OwernshipType { get; set; }

        public DateTime? DateOfEstablishment { get; set; }

        [StringLength(75)]
        public string Address { get; set; }
        [StringLength(25)]
        public string LocalGovtArea { get; set; }

        [StringLength(25)]
        public string State { get; set; }


        public byte[] Logo { get; set; }
        public byte[] SchoolBanner { get; set; }

        public virtual ICollection<RemitaFeeSetting> RemitaFeeSettings { get; set; }
        public virtual ICollection<FeeType> FeeTypes { get; set; }

    }


}