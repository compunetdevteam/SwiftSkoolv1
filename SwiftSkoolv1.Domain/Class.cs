using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SwiftSkoolv1.Domain.CBT;

namespace SwiftSkool.Models
{
    public class Class : GeneralSchool
    {
        public int ClassId { get; set; }

        [Display(Name = "Class")]
        [Required(ErrorMessage = "Class Name cannot be empty")]
        [StringLength(25)]
        public string SchoolName { get; set; }

        [Display(Name = "Class Level")]
        [Range(1, 6)]
        [Required(ErrorMessage = "Class Level cannot be empty")]
        public int ClassLevel { get; set; }

        [Display(Name = "Class Type")]
        [Required(ErrorMessage = "Class Type cannot be empty")]
        [StringLength(15)]
        public string ClassType { get; set; }

        public string ClassName
        {
            get
            {
                return $"{this.SchoolName.ToString()}{this.ClassLevel}";
            }
            set { }
        }
        public string FullClassName
        {
            get
            {
                return $"{this.ClassName.ToUpper()} {this.ClassType.ToUpper()}";
            }
            set { }
        }
        // public string ClassName => $"{this.SchoolName.ToString()}{this.ClassLevel}";
        //public string FullClassName => $"{this.ClassName} {this.ClassType.ToString()}";

        public virtual AssignedClass AssignClass { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<CaSetUp> SchoolCas { get; set; }
        public virtual ICollection<TimeTable> TimeTables { get; set; }
        public ICollection<ExamLog> ExamLogs { get; set; }
        public ICollection<ExamRule> ExamRules { get; set; }
        public ICollection<ExamSetting> ExamSettings { get; set; }
        public ICollection<QuestionAnswer> QuestionAnswers { get; set; }
        public ICollection<ResultDivision> ResultDivisions { get; set; }
        //public virtual ICollection<CaSetUp> AssignCas { get; set; }
    }
}