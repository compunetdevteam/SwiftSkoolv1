using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.WebUI.ViewModels.JambExam
{
    public class JambSelectSubjectVm
    {
        public int JambSubjectId { get; set; }
        [Required]
        public int ExamTime { get; set; }
        [Required]
        public int ExamYear { get; set; }
        [Required]
        public int TotalQuestion { get; set; }

    }
}