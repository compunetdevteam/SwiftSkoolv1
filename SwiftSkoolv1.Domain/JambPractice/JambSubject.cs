using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwiftSkoolv1.Domain.JambPractice
{
    public class JambSubject
    {
        public int JambSubjectId { get; set; }

        [Display(Name = "Subject Code")]
        [Required(ErrorMessage = "Subject Code is required")]
        [Index]
        [MaxLength(20)]
        public string SubjectCode { get; set; }

        [Display(Name = "Subject Name")]
        [Required(ErrorMessage = "Subject Name is required")]
        [StringLength(35, ErrorMessage = "Subject Name is too long")]
        public string SubjectName { get; set; }
        public ICollection<JambExamLog> JambExamLogs { get; set; }
        public ICollection<JambExamRule> JambExamRules { get; set; }
        public ICollection<JambQuestionAnswer> JambQuestionAnswers { get; set; }
        public ICollection<JambStudentQuestion> JambStudentQuestions { get; set; }

    }
}
