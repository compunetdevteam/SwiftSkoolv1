using SwiftSkoolv1.Domain;
using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.WebUI.ViewModels
{
    public class GuardianViewModel
    {
        public int GuardianId { get; set; }

        [Display(Name = "Student Id")]
        [Required(ErrorMessage = "Your StudentId is required")]
        [StringLength(40, ErrorMessage = "Your StudentId name is too long")]
        public string StudentId { get; set; }

        public Salutation Salutation { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Your Parent/Guardian First Name is required")]
        [StringLength(50, ErrorMessage = "Your Parent/Guardian First name is too long")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        //[Required(ErrorMessage = "Your Next of kin's Middle name is required")]
        [StringLength(50, ErrorMessage = "Your Parent/Guardian Middle name is too long")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Your Parent/Guardian last name is required")]
        [StringLength(50, ErrorMessage = "Your Parent/Guardian last name is too long")]
        public string LastName { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Your Gender is required")]
        public PopUp.Gender Gender { get; set; }

        [Display(Name = "Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; }

        //[Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Address")]
        [DataType(DataType.MultilineText)]
        // [Required(ErrorMessage = "Your Next of kin's Address is required")]
        [StringLength(50, ErrorMessage = "Your Next of kin's Address name is too long")]
        public string Address { get; set; }


        [Display(Name = "Occupation")]
        //[Required(ErrorMessage = "Your Occupation is required")]
        public string Occupation { get; set; }

        [Display(Name = "Religion")]
        //[Required(ErrorMessage = "Your Religion is required")]
        public PopUp.Religion Religion { get; set; }

        [Display(Name = "Local Government Area")]
        public string LGAOforigin { get; set; }

        [Display(Name = "State Of Origin")]
        public PopUp.State StateOfOrigin { get; set; }

        [Display(Name = "Mother's Name")]
        public string MotherName { get; set; }

        [Display(Name = "Mother's Maiden Name")]
        public string MotherMaidenName { get; set; }

        [Display(Name = "Relationship With Student")]
        //[Required(ErrorMessage = "Your Relationship is required")]
        public PopUp.Relationship Relationship { get; set; }

        public string Username => FirstName + " " + LastName;

        public virtual Student Student { get; set; }
    }
}