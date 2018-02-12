using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwiftSkoolv1.Domain.CEPractice
{
    public class CESubject
    {
        public int CESubjectId { get; set; }
        [Display(Name = "Subject Code")]
        [Required(ErrorMessage = "Subject Code is required")]
        [Index]
        [MaxLength(20)]
        public string SubjectCode { get; set; }

        [Display(Name = "Subject Name")]
        [Required(ErrorMessage = "Subject Name is required")]
        [StringLength(35, ErrorMessage = "Subject Name is too long")]
        public string SubjectName { get; set; }
        public ICollection<CEExamLog> CEExamLogs { get; set; }
        public ICollection<CEExamRule> CEExamRules { get; set; }
        public ICollection<CEQuestionAnswer> CEQuestionAnswers { get; set; }
        public ICollection<CEStudentQuestion> CEStudentQuestions { get; set; }
    }
}
