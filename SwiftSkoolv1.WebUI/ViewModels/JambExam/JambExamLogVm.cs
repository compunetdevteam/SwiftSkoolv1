namespace SwiftSkoolv1.WebUI.ViewModels.JambExam
{
    public class JambExamLogVm
    {
        public int ExamLogId { get; set; }
        public string StudentId { get; set; }
        public int JambSubjectId { get; set; }
        public double Score { get; set; }
        public bool ExamTaken { get; set; }
    }
}