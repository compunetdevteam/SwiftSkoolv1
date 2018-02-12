using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftSkoolv1.Domain.CEPractice
{
    public class CEExamLog
    {
        public int CEExamLogId { get; set; }
        public string StudentId { get; set; }
        public int CESubjectId { get; set; }
        public double Score { get; set; }
        public double TotalScore { get; set; }
        public bool ExamTaken { get; set; }
        public virtual Student Student { get; set; }

        public virtual CESubject CESubject { get; set; }
    }
}
