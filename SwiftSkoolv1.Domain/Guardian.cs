using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.Domain
{
    public class Guardian : GeneralSchool
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GuardianId { get; set; }

        [StringLength(45)]
        public string Salutation { get; set; }

        [StringLength(85)]
        public string FirstName { get; set; }

        [StringLength(85)]
        public string MiddleName { get; set; }

        [StringLength(85)]
        public string LastName { get; set; }

        [StringLength(20)]
        public string Gender { get; set; }

        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [StringLength(85)]
        public string Email { get; set; }

        [StringLength(105)]
        public string Address { get; set; }

        [StringLength(95)]
        public string Occupation { get; set; }

        [StringLength(55)]
        public string Relationship { get; set; }

        [StringLength(55)]
        public string Religion { get; set; }

        [StringLength(55)]
        public string LGAOforigin { get; set; }

        [StringLength(55)]
        public string StateOfOrigin { get; set; }

        [StringLength(85)]
        public string MotherName { get; set; }

        [StringLength(85)]
        public string MotherMaidenName { get; set; }

        public string UserName
        {
            get { return $"{LastName} {FirstName}"; }
            set { }
        }

        public string FullName
        {
            get { return $"{LastName} {FirstName} {MiddleName}"; }
            set { }
        }

        public string StudentId { get; set; }
        public virtual Student Student { get; set; }


    }
}