using System.ComponentModel.DataAnnotations;

namespace SwiftSkool.ViewModel
{
    public class AssignSubjectViewModel
    {
        [Display(Name = "Class Name")]
        [Required(ErrorMessage = "Class Name cannot be empty")]
        public string ClassName { get; set; }

        [Display(Name = "Subject Name")]
        [Required(ErrorMessage = "Subject Name cannot be empty")]
        public int[] SubjectId { get; set; }

        [Display(Name = "Term Name")]
        [Required(ErrorMessage = "Term Name cannot be empty")]
        public string[] TermName { get; set; }


    }
}