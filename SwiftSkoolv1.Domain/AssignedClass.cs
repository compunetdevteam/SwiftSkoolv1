using SwiftSkool.Models;
using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.Domain
{
    public class AssignedClass : GeneralSchool
    {
        public int AssignedClassId { get; set; }

        [Display(Name = "Student Number")]
        [Required(ErrorMessage = "Student Number cannot be empty")]
        [StringLength(25)]
        public string StudentId { get; set; }

        [Display(Name = "Class Name")]
        [Required(ErrorMessage = "Class Name cannot be empty")]
        [StringLength(15)]
        public string ClassName { get; set; }

        [Display(Name = "Term Name")]
        [Required(ErrorMessage = "Term Name cannot be empty")]
        [StringLength(15)]
        public string TermName { get; set; }

        [Display(Name = "Session Name")]
        [Required(ErrorMessage = "Session Name cannot be empty")]
        [StringLength(15)]
        public string SessionName { get; set; }
        [StringLength(35)]
        public string StudentName { get; set; }

        public virtual Student Student { get; set; }

        // public virtual ICollection<AssignSubject> AssignSubjects { get; set; }
    }
}