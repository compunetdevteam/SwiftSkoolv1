using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Web;

namespace SwiftSkoolv1.WebUI.ViewModels
{
    public class StaffViewModel
    {

        [Display(Name = "Salutation")]
        public PopUp.Salutation Salutation { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Your First Name is required")]
        [StringLength(50, ErrorMessage = "Your First Name is too long")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        //[Required(ErrorMessage = "Your Middle Name is required")]
        [StringLength(50, ErrorMessage = "Your Middle Name is too long")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Your Last Name is required")]
        [StringLength(50, ErrorMessage = "Your LastName is too long")]
        public string LastName { get; set; }

        [Display(Name = "Mobile Number")]
        //[Required(ErrorMessage = "Enter the Correct Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }


        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Gender")]
        public PopUp.Gender Gender { get; set; }

        [Display(Name = "Address")]
        [DataType(DataType.MultilineText)]
        // [Required(ErrorMessage = "Your Address is required")]
        [StringLength(50, ErrorMessage = "Your Address name is too long")]
        public string Address { get; set; }

        public PopUp.State StateOfOrigin { get; set; }

        [Display(Name = "Designation")]
        // [Required(ErrorMessage = "Designation is required")]
        public string Designation { get; set; }

        [Display(Name = "Date of Birth")]
        // [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Marital Status")]
        // [Required(ErrorMessage = "Marital Status is required")]
        public PopUp.Maritalstatus MaritalStatus { get; set; }

        [Display(Name = "Highest Qualification")]
        //[Required(ErrorMessage = "Highest Qualification  is required")]
        public PopUp.Qualifications Qualifications { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Role")]
       // [Required(ErrorMessage = "User Role is required")]
        public string Name { get; set; }


        public byte[] StaffPassport { get; set; }

        public string Username => $"{this.FirstName}_{this.LastName}";

        [Display(Name = "Upload A Passport/Picture")]
        [SwiftSkool.ViewModel.ValidateFile(ErrorMessage = "Please select a PNG/JPEG image smaller than 1MB")]
        [NotMapped]
        public HttpPostedFileBase File
        {
            get
            {
                return null;
            }

            set
            {
                try
                {
                    MemoryStream target = new MemoryStream();

                    if (value.InputStream == null)
                        return;

                    value.InputStream.CopyTo(target);
                    StaffPassport = target.ToArray();
                }
                catch (Exception)
                {

                    //logger.Error(ex.Message);
                    //logger.Error(ex.StackTrace);
                }
            }
        }
    }

    public class StaffVM
    {
        public Student Student { get; set; }
        public List<Student> StudentsInMyClass { get; set; }
        public string ClassName { get; set; }

        public int MaleStudent { get; set; }
        public int FemaleStudent { get; set; }
        public int TotalStudentInClass { get; set; }
    }
}