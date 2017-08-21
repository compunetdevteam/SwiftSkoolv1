using SwiftSkoolv1.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Web;

namespace SwiftSkoolv1.WebUI.ViewModels
{
    public class StudentViewModel
    {

        [Display(Name = "Student ID")]
        [Required(ErrorMessage = "Your Student ID Number is required")]
        [StringLength(25, ErrorMessage = "Your Student ID is too long")]
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


        [Display(Name = "Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Your Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Place of Birth")]
        public string PlaceOfBirth { get; set; }

        [Display(Name = "State of Origin")]
        public State StateOfOrigin { get; set; }

        public Gender Gender { get; set; }

        [Display(Name = "Religion")]
        public Religion Religion { get; set; }

        [Display(Name = "Tribe")]
        public string Tribe { get; set; }

        [Display(Name = "Admission Date")]
        [Required(ErrorMessage = "Your Admission Date is required")]
        [DataType(DataType.Date)]
        public DateTime AdmissionDate { get; set; }

        public string UserName
        {
            get { return $"{LastName} {FirstName}"; }
            set { }
        }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public byte[] StudentPassport { get; set; }

        [Display(Name = "Upload A Passport/Picture")]
        [ValidateFile(ErrorMessage = "Please select a PNG/JPEG image smaller than 1MB")]
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
                    StudentPassport = target.ToArray();
                }
                catch (Exception)
                {
                    //logger.Error(ex.Message);
                    //logger.Error(ex.StackTrace);
                }
            }
        }
    }

    public class StudentClientViewModel
    {

        public StudentClientViewModel()
        {

        }

        public StudentClientViewModel(Student student)
        {
            Map(student);
        }


        private void Map(Student student)
        {
            if (student != null)
            {
                StudentId = student.StudentId;
                FirstName = student.FirstName;
                MiddleName = student.MiddleName;
                LastName = student.LastName;
                PhoneNumber = student.PhoneNumber;
                DateOfBirth = student.DateOfBirth;
                StateOfOrigin = student.StateOfOrigin;
                Gender = student.Gender;
                Religion = student.Religion;
                AdmissionDate = student.AdmissionDate;
                UserName = student.UserName;
            }
            throw new ArgumentNullException("You must pass a valid student for mapping to be completed!");
        }

        [Display(Name = "Student ID")]
        [Required(ErrorMessage = "Your Student ID Number is required")]
        [StringLength(25, ErrorMessage = "Your Student ID is too long")]
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


        [Display(Name = "Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Your Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "State of Origin")]
        public string StateOfOrigin { get; set; }

        public string Gender { get; set; }

        [Display(Name = "Religion")]
        public string Religion { get; set; }


        [Display(Name = "Admission Date")]
        [Required(ErrorMessage = "Your Admission Date is required")]
        [DataType(DataType.Date)]
        public DateTime AdmissionDate { get; set; }

        public string UserName
        {
            get { return $"{LastName} {FirstName}"; }
            set { }
        }
    }

    public class StudentEditViewModel
    {

        [Display(Name = "Student ID")]
        [Required(ErrorMessage = "Your Student ID Number is required")]
        [StringLength(25, ErrorMessage = "Your Student ID is too long")]
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


        [Display(Name = "Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        //[Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Your Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Place of Birth")]
        public string PlaceOfBirth { get; set; }

        [Display(Name = "State of Origin")]
        public State StateOfOrigin { get; set; }

        public Gender Gender { get; set; }

        [Display(Name = "Religion")]
        public Religion Religion { get; set; }

        [Display(Name = "Tribe")]
        public string Tribe { get; set; }

        [Display(Name = "Admission Date")]
        [Required(ErrorMessage = "Your Admission Date is required")]
        [DataType(DataType.Date)]
        public DateTime AdmissionDate { get; set; }

        public string UserName
        {
            get { return $"{LastName} {FirstName}"; }
            set { }
        }



        public byte[] StudentPassport { get; set; }

        [Display(Name = "Upload A Passport/Picture")]
        [ValidateFile(ErrorMessage = "Please select a PNG/JPEG image smaller than 1MB")]
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
                    var target = new MemoryStream();

                    if (value.InputStream == null)
                        return;

                    value.InputStream.CopyTo(target);
                    StudentPassport = target.ToArray();
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            }
        }
    }



    public class AssignClassToStudentVm
    {
        public string ClassName { get; set; }
        public string[] StudentId { get; set; }
    }

    public class AggregateList
    {
        public string StudentId { get; set; }
        public double Score { get; set; }
    }
}