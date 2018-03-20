namespace SwiftSkoolv1.WebUI.ViewModels.JambExam
{
    public class JambDisplayQuestionVm
    {
        public string Question { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }

        public bool Check1 { get; set; }
        public bool Check2 { get; set; }
        public bool Check3 { get; set; }
        public bool Check4 { get; set; }
        public string SelectedAnswer { get; set; }
        public string FilledAnswer { get; set; }
        public int QuestionNo { get; set; }
        public int TotalQuestion { get; set; }
        public string ExamType { get; set; }
        public string ExamYear { get; set; }
        public string NextQuestion { get; set; }

        public bool IsFillInTheGag { get; set; }
        public bool IsMultiChoiceAnswer { get; set; }
        public int JambSubjectId { get; set; }
        public string StudentId { get; set; }
    }
}