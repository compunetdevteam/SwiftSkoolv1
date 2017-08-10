using SwiftSkoolv1.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.WebUI.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email or Phone Number")]
        //[EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class EditUserViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Salutation")]
        public Salutation Salutation { get; set; }

        [Display(Name = "Surname")]
        [Required(ErrorMessage = "Your Surname is required")]
        [StringLength(50, ErrorMessage = "Your Surname is too long")]
        public string Surname { get; set; }

        [Display(Name = "Other Names")]
        [Required(ErrorMessage = "Your Other Name is required")]
        [StringLength(50, ErrorMessage = "Your Other Name is too long")]
        public string OtherName { get; set; }

        [Display(Name = "Mobile Number")]
        [Required(ErrorMessage = "Enter the Correct Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Address")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Your Address is required")]
        [StringLength(50, ErrorMessage = "Your Address name is too long")]
        public string Address { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        public string Username
        {
            get
            {
                return string.Format("{0} {1}", this.Surname, this.OtherName);
            }
        }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Name { get; set; }

    }
    public class RegisterAdminVm
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Your First Name is required")]
        [StringLength(50, ErrorMessage = "Your First Name is too long")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Your Last Name is required")]
        [StringLength(50, ErrorMessage = "Your Last Name is too long")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }



        [Display(Name = "School Name")]
        public string SchoolId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Username
        {
            get
            {
                return string.Format("{0} {1}", this.LastName, this.FirstName);
            }
        }
    }

    public class RegisterViewModel
    {
        [Display(Name = "Student ID")]
        [Required(ErrorMessage = "Your Student ID Number is required")]
        [StringLength(10, ErrorMessage = "Your Student ID is too long")]
        public string StudentId { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Your First Name is required")]
        [StringLength(50, ErrorMessage = "Your First Name is too long")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Your Last Name is required")]
        [StringLength(50, ErrorMessage = "Your Last Name is too long")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Current Address")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Your Current Address is required")]
        [StringLength(150, ErrorMessage = "Your Current Address name is too long")]
        public string Address { get; set; }

        [Display(Name = "Current Class")]
        public string Class { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Username
        {
            get
            {
                return string.Format("{0} {1}", this.LastName, this.FirstName);
            }
        }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
