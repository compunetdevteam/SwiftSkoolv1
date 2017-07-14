using SwiftSkool.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.Domain.CBT
{
    public class ExamType : GeneralSchool
    {
        public int ExamTypeId { get; set; }

        [Display(Name = "Exam Name")]
        public string ExamName { get; set; }

        public virtual ICollection<ExamSetting> ExamSettings { get; set; }
        public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; }
        public virtual ICollection<ExamLog> ExamLogs { get; set; }
    }
}
