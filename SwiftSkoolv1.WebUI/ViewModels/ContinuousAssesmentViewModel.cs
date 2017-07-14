using System.ComponentModel.DataAnnotations;

namespace SwiftSkool.ViewModel
{
    public class ContinuousAssesmentViewModel
    {
        public int ContinuousAssessmentId { get; set; }

        [Display(Name = "Student ID")]
        [Required(ErrorMessage = "Your Student ID Number is required")]
        [StringLength(15, ErrorMessage = "Your Student ID is too long")]
        public string StudentId { get; set; }

        [Display(Name = "Term")]
        [Required(ErrorMessage = "Term is required")]
        public string TermName { get; set; }

        [Display(Name = "Session")]
        [Required(ErrorMessage = "Session is required")]
        public string SessionName { get; set; }

        [Display(Name = "Subject")]
        [Required(ErrorMessage = "Subject is required")]
        public int SubjectId { get; set; }

        public string SubjectCategory { get; set; }

        [Display(Name = "Class Name")]
        [Required(ErrorMessage = "Class Name is required")]
        public string ClassName { get; set; }


        [Display(Name = "Score for First Test")]
        //[Required(ErrorMessage = "First Test is required")]
        [Range(0, 10, ErrorMessage = "Enter number between 0 to 10")]
        public double FirstTest { get; set; }

        [Display(Name = "Score for Second Test")]
        //[Required(ErrorMessage = "Second Test is required")]
        [Range(0, 10, ErrorMessage = "Enter number between 0 to 10")]
        public double SecondTest { get; set; }

        [Display(Name = "Score Third Test")]
        //[Required(ErrorMessage = "Third Test score is required")]
        [Range(0, 10, ErrorMessage = "Enter number between 0 to 10")]
        public double ThirdTest { get; set; }


        [Display(Name = "Exam Score")]
        [Range(0, 70, ErrorMessage = "Enter number between 0 to 70")]
        //[Required(ErrorMessage = "Exam Score is required")]
        public double ExamScore { get; set; }

        [Display(Name = "Staff Name")]
        [Required(ErrorMessage = "Staff name is required")]
        public string StaffName { get; set; }
    }

    public class ContinuousAssesmentVm
    {
        public string SubjectName { get; set; }
        public int SubjectPosition { get; set; }
        public double SubjectLowest { get; set; }
        public double SubjectHighest { get; set; }
        public double ClassAverage { get; set; }
        public double FirstCa { get; set; }
        public double SecondCa { get; set; }
        public double ThirdCa { get; set; }
        public double ForthCa { get; set; }
        public double FifthCa { get; set; }
        public double SixthCa { get; set; }
        public double SeventhCa { get; set; }
        public double EightCa { get; set; }
        public double NinthtCa { get; set; }
        public double ExamCa { get; set; }
        public double Total { get; set; }

        public string Grading { get; set; }

        public string Remark { get; set; }

        public string StaffName { get; set; }
    }

    public class AggregateList
    {
        public string StudentId { get; set; }
        public double Score { get; set; }
    }
}