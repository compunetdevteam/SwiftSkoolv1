using SwiftSkool.Models;
using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.Domain
{
    public class SubjectRegistration : GeneralSchool
    {
        public int Id { get; set; }

        [Display(Name = "Student Number")]
        [Required(ErrorMessage = "Student Number cannot be empty")]
        [StringLength(25)]
        public string StudentId { get; set; }

        [Display(Name = "Student Name")]
        [Required(ErrorMessage = "Student Name cannot be empty")]
        [StringLength(65)]
        public string StudentName { get; set; }

        //[Display(Name = "Class Name")]
        //[Required(ErrorMessage = "Class Name cannot be empty")]
        //[StringLength(15)]
        //public string ClassName { get; set; }

        //[Display(Name = "Term Name")]
        //[Required(ErrorMessage = "Term Name cannot be empty")]
        //[StringLength(15)]
        //public string TermName { get; set; }

        //[Display(Name = "Session Name")]
        //[Required(ErrorMessage = "Session Name cannot be empty")]
        //public string SessionName { get; set; }

        [Display(Name = "Subject Name")]
        [Required(ErrorMessage = "Subject Name cannot be empty")]
        public int SubjectId { get; set; }

        public virtual Student Student { get; set; }

        public virtual Subject Subject { get; set; }
    }

    public class SubjectRegistrationVm
    {
        [Display(Name = "Student Number")]
        [Required(ErrorMessage = "Student Number cannot be empty")]
        public string StudentId { get; set; }

        //[Display(Name = "Class Name")]
        //[Required(ErrorMessage = "Class Name cannot be empty")]
        //public string ClassName { get; set; }

        //[Display(Name = "Term Name")]
        //[Required(ErrorMessage = "Term Name cannot be empty")]
        //public string TermName { get; set; }

        //[Display(Name = "Session Name")]
        //[Required(ErrorMessage = "Session Name cannot be empty")]
        //public string SessionName { get; set; }

        [Display(Name = "Subject Name")]
        [Required(ErrorMessage = "Subject Name cannot be empty")]
        public int[] SubjectId { get; set; }

    }
}