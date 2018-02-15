using SwiftSkoolv1.Domain;
using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.WebUI.ViewModels.JambExam
{
    public class JambQuestionAnswerVm
    {
        public int JambQuestionAnswerId { get; set; }

        [Display(Name = "Subject Name")]
        [Required(ErrorMessage = "Subject Name is required")]
        public int JambSubjectId { get; set; }

        public string ExamYear { get; set; }

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
        public string Answer { get; set; }

        [Display(Name = "Question Hint")]
        public string QuestionHint { get; set; }

        public QuestionType QuestionType { get; set; }
    }
}