using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.Domain.JambPractice
{
    public class JambQuestionAnswer
    {
        public int JambQuestionAnswerId { get; set; }

        [Display(Name = "Subject Name")]
        [Required(ErrorMessage = "Subject Name is required")]
        public int JambSubjectId { get; set; }

        public string ExamType { get; set; }

        public string ExamYear { get; set; }

        [Display(Name = "Question")]
        [Required(ErrorMessage = "Question is required")]
        [DataType(DataType.MultilineText)]
        public string Question { get; set; }

        [Display(Name = "Option 1")]
        [DataType(DataType.MultilineText)]
        public string Option1 { get; set; }

        [Display(Name = "Option 2")]
        [DataType(DataType.MultilineText)]
        public string Option2 { get; set; }

        [Display(Name = "Option 3")]
        [DataType(DataType.MultilineText)]
        public string Option3 { get; set; }

        [Display(Name = "Option 4")]
        [DataType(DataType.MultilineText)]
        public string Option4 { get; set; }


        [Display(Name = "Answer")]
        [Required(ErrorMessage = "Answer is required")]
        [DataType(DataType.MultilineText)]
        public string Answer { get; set; }

        [Display(Name = "Question Hint")]
        public string QuestionHint { get; set; }

        public QuestionType QuestionType { get; set; }

        [Display(Name = "Fill in the Gap")]
        public bool IsFillInTheGag
        {
            get
            {
                if (QuestionType.Equals(QuestionType.BlankChoice))
                {
                    return true;
                }
                return false;
            }
            private set { }
        }

        [Display(Name = "Multi-choice Answer")]
        public bool IsMultiChoiceAnswer
        {
            get
            {
                if (QuestionType.Equals(QuestionType.MultiChoice))
                {
                    return true;
                }
                return false;
            }
            private set { }
        }
        public bool IsSingleChoiceAnswer
        {
            get
            {
                if (QuestionType.Equals(QuestionType.SingleChoice))
                {
                    return true;
                }
                return false;
            }
            private set { }
        }

        public virtual JambSubject JambSubject { get; set; }
    }
}
