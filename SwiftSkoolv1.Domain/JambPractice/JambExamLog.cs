namespace SwiftSkoolv1.Domain.JambPractice
{
    public class JambExamLog
    {
        public int JambExamLogId { get; set; }
        public string StudentId { get; set; }
        public int JambSubjectId { get; set; }
        public double Score { get; set; }
        public double TotalScore { get; set; }
        public bool ExamTaken { get; set; }
        public virtual Student Student { get; set; }

        public virtual JambSubject JambSubject { get; set; }

    }
}
