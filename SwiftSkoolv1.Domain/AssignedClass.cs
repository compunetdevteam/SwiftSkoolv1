using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.Domain
{
    public class AssignedClass : GeneralSchool
    {
        public int AssignedClassId { get; set; }

        [Display(Name = "Student Number")]
        [Required(ErrorMessage = "Student Number cannot be empty")]
        public string StudentId { get; set; }

        [Display(Name = "Class Name")]
        [Required(ErrorMessage = "Class Name cannot be empty")]
        public string ClassName { get; set; }

        [Display(Name = "Term Name")]
        [Required(ErrorMessage = "Term Name cannot be empty")]
        public string TermName { get; set; }

        [Display(Name = "Session Name")]
        [Required(ErrorMessage = "Session Name cannot be empty")]
        public string SessionName { get; set; }
        public string StudentName { get; set; }

        public virtual Student Student { get; set; }

        // public virtual ICollection<AssignSubject> AssignSubjects { get; set; }
    }
}