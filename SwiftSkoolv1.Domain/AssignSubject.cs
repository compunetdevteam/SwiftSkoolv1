using System.ComponentModel.DataAnnotations;

namespace SwiftSkool.Models
{
    public class AssignSubject : GeneralSchool
    {
        public int AssignSubjectId { get; set; }

        [Display(Name = "Class Name")]
        [Required(ErrorMessage = "Class Name cannot be empty")]
        [StringLength(25)]
        public string ClassName { get; set; }

        [Display(Name = "Subject Name")]
        [Required(ErrorMessage = "Subject Name cannot be empty")]
        public int SubjectId { get; set; }

        [Display(Name = "Term Name")]
        [Required(ErrorMessage = "Term Name cannot be empty")]
        [StringLength(15)]
        public string TermName { get; set; }

        public virtual Class Class { get; set; }

        public virtual Subject Subject { get; set; }

    }
}