using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.WebUI.ViewModels
{
    public class ResultViewModel
    {
        public int ResultId { get; set; }
        //[Display(Name = "Student ID")]
        //[Required(ErrorMessage = "Your Student ID Number is required")]
        //[StringLength(25, ErrorMessage = "Your Student ID is too long")]
        public string StudentId { get; set; }

        [Display(Name = "Term")]
        [Required(ErrorMessage = "Term is required")]
        public string TermName { get; set; }

        [Display(Name = "Session")]
        [Required(ErrorMessage = "Session is required")]
        public string SessionName { get; set; }

        [Display(Name = "Subject")]
        [Required(ErrorMessage = "Subject is required")]
        public int SubjectCode { get; set; }

        [Display(Name = "Class Name")]
        [Required(ErrorMessage = "Class Name is required")]
        public string ClassName { get; set; }
    }
}