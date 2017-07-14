using System.ComponentModel.DataAnnotations;

namespace SwiftSkool.ViewModel.CBTE
{
    public class SelectSubjectViewModel
    {
        [Display(Name = "Subject Name")]
        [Required(ErrorMessage = "Subject Name is required")]
        public string SubjectName { get; set; }
    }
}