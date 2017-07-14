using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwiftSkool.Models
{
    public class ContinuousAssessment : GeneralSchool
    {
        private readonly GradeRemark _myGradeRemark = new GradeRemark();
        public int ContinuousAssessmentId { get; set; }

        [Display(Name = "Student ID")]
        [Required(ErrorMessage = "Your Student ID Number is required")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(25)]
        [Index]
        public string StudentId { get; set; }

        [Display(Name = "Term")]
        [Required(ErrorMessage = "Term is required")]
        [StringLength(15)]
        public string TermName { get; set; }

        [Display(Name = "Session")]
        [Required(ErrorMessage = "Session is required")]
        [StringLength(15)]
        public string SessionName { get; set; }

        [Display(Name = "Subject")]
        [Required(ErrorMessage = "Subject is required")]
        public int SubjectId { get; set; }

        [Display(Name = "Class Name")]
        [Required(ErrorMessage = "Class Name is required")]
        [StringLength(15)]
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
        [Required(ErrorMessage = "Exam Score is required")]
        public double ExamScore { get; set; }

        [Display(Name = "Staff Name")]
        [Required(ErrorMessage = "Staff name is required")]
        [StringLength(35)]
        public string StaffName { get; set; }

        public virtual List<Subject> Subjects { get; set; }
        public virtual Result Result { get; set; }
        public virtual List<Session> Sessions { get; set; }

        public double Total
        {
            get
            {
                double sum = FirstTest + SecondTest + ThirdTest + ExamScore;
                return sum;
            }
            private set { }
        }

        public string Grading
        {

            get
            {
                return _myGradeRemark.Grading(Total, ClassName, SchoolId);
            }
            private set { }

        }

        public string Remark
        {
            #region Checking grade

            get
            {
                return _myGradeRemark.Remark(Total, ClassName, SchoolId);
            }
            private set { }

            #endregion
        }
        public virtual Subject Subject { get; set; }
        //public int GradePoint
        //{
        //    get
        //    {
        //        return _myGradeRemark.GradingPoint(Total, ClassName);
        //    }
        //    private set { }
        //}

        //public int QualityPoint
        //{
        //    get
        //    {
        //        return 2 * GradePoint;
        //    }
        //    private set { }
        //}
    }
}