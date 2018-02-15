using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwiftSkoolv1.Domain.JambPractice
{
    public class JambExamRule
    {
        public int JambExamRuleId { get; set; }

        [ForeignKey("JambSubject")]
        [Display(Name = "Subject Name")]
        [Required(ErrorMessage = "Subject Name is required")]
        public int JambSubjectId { get; set; }

        public int? ResultDivision { get; set; }

        [Display(Name = "Score per Question")]
        [Required(ErrorMessage = "Score per Question is required")]
        public double ScorePerQuestion { get; set; }

        [Display(Name = "Total Question")]
        //[Range(1, 100)]
        //[Required(ErrorMessage = "Total Question is required")]
        public int TotalQuestion { get; set; }

        //[Display(Name = "Duration In Minute")]
        //[Required(ErrorMessage = "Maximum Exam Time is required")]
        public int? MaximumTime { get; set; }

        public virtual JambSubject JambSubject { get; set; }

    }
}
