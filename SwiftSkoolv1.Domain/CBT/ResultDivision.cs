using SwiftSkool.Models;
using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.Domain.CBT
{
    public class ResultDivision : GeneralSchool
    {
        public int ResultDivisionId { get; set; }

        [Display(Name = "Subject Name")]
        [Required(ErrorMessage = "Subject Name is required")]
        public int SubjectId { get; set; }

        [Display(Name = "Class Name")]
        [Required(ErrorMessage = "Class Name is required")]
        public int ClassId { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual Class Class { get; set; }
    }
}
