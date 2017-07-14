using SwiftSkool.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwiftSkoolv1.Domain.CBT
{
    public class ExamRule : GeneralSchool
    {
        public int ExamRuleId { get; set; }

        [ForeignKey("Subject")]
        [Display(Name = "Subject Name")]
        [Required(ErrorMessage = "Subject Name is required")]
        public int SubjectId { get; set; }

        [Display(Name = "Class Name")]
        [Required(ErrorMessage = "Class Name is required")]
        public int ClassId { get; set; }

        public int ResultDivision { get; set; }

        [Display(Name = "Score per Question")]
        [Required(ErrorMessage = "Score per Question is required")]
        public double ScorePerQuestion { get; set; }

        [Display(Name = "Total Question")]
        [Range(1, 100)]
        [Required(ErrorMessage = "Total Question is required")]
        public int TotalQuestion { get; set; }

        [Display(Name = "Duration In Minute")]
        [Required(ErrorMessage = "Maximum Exam Time is required")]
        public int MaximumTime { get; set; }

        public virtual Subject Subject { get; set; }

        public virtual Class Class { get; set; }
    }


}