using System;
using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.Domain
{
    public class Grade : GeneralSchool
    {
        public int GradeId { get; set; }
        [StringLength(15)]
        public string GradeName { get; set; }
        public int MinimumValue { get; set; }
        public int MaximumValue { get; set; }
        //public int GradePoint { get; set; }
        [StringLength(25)]
        public string Remark { get; set; }
        // public string ClassName { get; set; }

    }

    public class PrincipalComment : GeneralSchool
    {
        public int Id { get; set; }

        [Display(Name = "Minimum Grade")]
        [Required(ErrorMessage = "Minimum Grade is required")]
        [Range(1, 100, ErrorMessage = "Value must be between 1.0 and 100.0")]
        public double MinimumGrade { get; set; }

        [Display(Name = "Maximum Grade")]
        [Required(ErrorMessage = "Maximum Grade is required")]
        [Range(1, 100, ErrorMessage = "Value must be between 1.0 and 100.0")]
        public double MaximumGrade { get; set; }

        [Display(Name = "Principal Remark")]
        [Required(ErrorMessage = "Principal Remark is required")]
        [StringLength(135)]
        public string Remark { get; set; }
        [StringLength(65)]
        public string ClassName { get; set; }

    }

    public class PrincipalCommentVm
    {
        public int Id { get; set; }

        [Display(Name = "Minimum Grade")]
        [Required(ErrorMessage = "Minimum Grade is required")]
        [Range(0, 100, ErrorMessage = "Value must be between 1.0 and 100.0")]
        public double MinimumGrade { get; set; }

        [Display(Name = "Maximum Grade")]
        [Required(ErrorMessage = "Maximum Grade is required")]
        [Range(0, 100, ErrorMessage = "Value must be between 1.0 and 100.0")]
        public double MaximumGrade { get; set; }

        [Display(Name = "Principal Remark")]
        [Required(ErrorMessage = "Principal Remark is required")]
        public string Remark { get; set; }

        public string[] ClassName { get; set; }

    }

    public class TeacherComment
    {
        public int TeacherCommentId { get; set; }

        [Display(Name = "Student Id")]
        [Required(ErrorMessage = "Student Id is required")]
        [StringLength(65)]
        public string StudentId { get; set; }


        [Display(Name = "Term Name")]
        [Required(ErrorMessage = "Term is required")]
        [StringLength(15)]
        public string TermName { get; set; }

        [Display(Name = "Session Name")]
        [Required(ErrorMessage = "Session is required")]
        public string SessionName { get; set; }


        [Display(Name = "Teacher's Remark")]
        [Required(ErrorMessage = "Teacher's remark is required")]
        [StringLength(135)]
        public string Remark { get; set; }

        public DateTime? Date { get; set; }

    }
}