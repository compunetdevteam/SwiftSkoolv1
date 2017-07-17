using SwiftSkool.Models;

namespace SwiftSkoolv1.Domain.CBT
{
    public class ExamLog : GeneralSchool
    {
        public int ExamLogId { get; set; }
        public string StudentId { get; set; }
        public int SubjectId { get; set; }
        public int ClassId { get; set; }
        public int TermId { get; set; }
        public int SessionId { get; set; }
        public int ExamTypeId { get; set; }
        public double Score { get; set; }
        public double TotalScore { get; set; }
        public bool ExamTaken { get; set; }
        public virtual Student Student { get; set; }

        public virtual Subject Subject { get; set; }

        public virtual Class Class { get; set; }
        public virtual ExamType ExamType { get; set; }
        public virtual Term Term { get; set; }
        public virtual Session Sessions { get; set; }

    }
}
