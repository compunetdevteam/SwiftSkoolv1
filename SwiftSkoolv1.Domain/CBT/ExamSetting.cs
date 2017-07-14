using SwiftSkool.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.Domain.CBT
{

    public class ExamSetting : GeneralSchool
    {
        public int ExamSettingId { get; set; }

        [Display(Name = "Course Name")]
        [Required(ErrorMessage = "Course Name is required")]
        public int SubjectId { get; set; }

        [Display(Name = "Level Name")]
        [Required(ErrorMessage = "Level Name is required")]
        public int ClassId { get; set; }

        [Display(Name = "Semester")]
        [Required(ErrorMessage = "Semester is required")]
        public int TermId { get; set; }

        [Display(Name = "Session")]
        [Required(ErrorMessage = "Session is required")]
        public int SessionId { get; set; }

        [Display(Name = "Start Date")]
        [Required(ErrorMessage = "Start Date is required")]
        [DataType(DataType.Date)]
        public DateTime ExamDate { get; set; }

        public int ExamTypeId { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual Class Class { get; set; }
        public virtual Term Term { get; set; }
        public virtual Session Sessions { get; set; }
        public virtual ExamType ExamType { get; set; }
    }
}
