using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SwiftSkoolv1.Domain;
using SwiftSkoolv1.Domain.CBT;

namespace SwiftSkool.Models
{
    public class Subject : GeneralSchool
    {
        [Key]
        public int SubjectId { get; set; }

        [Display(Name = "Subject Code")]
        [Required(ErrorMessage = "Subject Code is required")]
        [Index]
        [MaxLength(20)]
        public string SubjectCode { get; set; }

        [Display(Name = "Subject Name")]
        [Required(ErrorMessage = "Subject Name is required")]
        [StringLength(35, ErrorMessage = "Subject Name is too long")]
        public string SubjectName { get; set; }

        //[DisplayName("Subject Unit")]
        //[Range(1, 2)]
        //public int SubjectUnit { get; set; }

        public virtual ContinuousAssessment ContinuousAssessment { get; set; }
        public ICollection<CaList> CaList { get; set; }
        public virtual ICollection<SubjectRegistration> SubjectRegistrations { get; set; }
        public virtual ICollection<AssignSubject> AssignSubjects { get; set; }
        public virtual ICollection<AssignSubjectTeacher> AssignSubjectTeachers { get; set; }
        public virtual ICollection<TimeTable> TimeTables { get; set; }
        public ICollection<ExamLog> ExamLogs { get; set; }
        public ICollection<ExamRule> ExamRules { get; set; }
        public ICollection<ExamSetting> ExamSettings { get; set; }
        public ICollection<QuestionAnswer> QuestionAnswers { get; set; }
        public ICollection<ResultDivision> ResultDivisions { get; set; }




    }
}