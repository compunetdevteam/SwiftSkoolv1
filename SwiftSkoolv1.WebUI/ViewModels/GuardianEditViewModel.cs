using SwiftSkool.Models;
using System.ComponentModel.DataAnnotations;

namespace SwiftSkool.ViewModel
{
    public class GuardianEditViewModel
    {
        public string GuardianId { get; set; }

        [Display(Name = "Student's Name")]
        [Required(ErrorMessage = "Student's Name is required")]
       // public string StudentId { get; set; }

        public PopUp.Salutation Salutation { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Your Next of kin's First Name is required")]
        [StringLength(40, ErrorMessage = "Your Next of kin's First name is too long")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [Required(ErrorMessage = "Your Next of kin's Middle name is required")]
        [StringLength(40, ErrorMessage = "Your Next of kin's Middle name is too long")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Your Next of kin's last name is required")]
        [StringLength(40, ErrorMessage = "Your Next of kin's last name is too long")]
        public string LastName { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Your Gender is required")]
        public PopUp.Gender Gender { get; set; }

        [Display(Name = "Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Address")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Your Next of kin's Address is required")]
        [StringLength(50, ErrorMessage = "Your Next of kin's Address name is too long")]
        public string Address { get; set; }


        [Display(Name = "Occupation")]
        [Required(ErrorMessage = "Your Occupation is required")]
        public string Occupation { get; set; }

        [Display(Name = "Relationship")]
        [Required(ErrorMessage = "Your Next of kin's Relationship is required")]
        public PopUp.Relationship Relationship { get; set; }

        public string Username => FirstName + " " + MiddleName + " " + LastName;
    }
}