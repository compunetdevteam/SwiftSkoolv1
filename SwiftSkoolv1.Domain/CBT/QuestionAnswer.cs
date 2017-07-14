using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.Domain.CBT

{
    public class QuestionAnswer : GeneralSchool
    {
        public int QuestionAnswerId { get; set; }

        [Display(Name = "Course Name")]
        [Required(ErrorMessage = "Course Name is required")]
        public int SubjectId { get; set; }

        [Display(Name = "Level Name")]
        //[Required(ErrorMessage = "Level Name is required")]
        public int ClassId { get; set; }

        [Display(Name = "Exam Type")]
        public int ExamTypeId { get; set; }

        [Display(Name = "Question")]
        [Required(ErrorMessage = "Question is required")]
        [DataType(DataType.MultilineText)]
        public string Question { get; set; }

        [Display(Name = "Option 1")]
        //[Required(ErrorMessage = "Option 1 is required")]
        [DataType(DataType.MultilineText)]
        public string Option1 { get; set; }

        [Display(Name = "Option 2")]
        //[Required(ErrorMessage = "Option 2 is required")]
        [DataType(DataType.MultilineText)]
        public string Option2 { get; set; }

        [Display(Name = "Option 3")]
        //[Required(ErrorMessage = "Option 3 is required")]
        [DataType(DataType.MultilineText)]
        public string Option3 { get; set; }

        [Display(Name = "Option 4")]
        // [Required(ErrorMessage = "Option 4 is required")]
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

        public virtual Subject Subject { get; set; }
        public virtual Class Class { get; set; }
        public virtual ExamType ExamType { get; set; }
    }
}