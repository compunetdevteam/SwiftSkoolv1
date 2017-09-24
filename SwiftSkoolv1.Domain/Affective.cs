using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.Domain
{
    public class Affective : GeneralSchool
    {
        [Key]
        public int AffectiveId { get; set; }
        public string StudentId { get; set; }
        public string TermName { get; set; }
        public string SessionName { get; set; }
        public string ClassName { get; set; }
        public string Honesty { get; set; }
        public string SelfConfidence { get; set; }
        public string Sociability { get; set; }
        public string Punctuality { get; set; }
        public string Neatness { get; set; }
        public string Initiative { get; set; }
        public string Organization { get; set; }
        public string AttendanceInClass { get; set; }

        public string HonestyAndReliability { get; set; }
        //public virtual ICollection<R>
    }

    public class Psychomotor : GeneralSchool
    {

        public int Id { get; set; }
        public string StudentId { get; set; }
        public string TermName { get; set; }
        public string SessionName { get; set; }
        public string ClassName { get; set; }
        public string Sports { get; set; }
        public string ExtraCurricularActivity { get; set; }
        public string Assignment { get; set; }
        public string HelpingOthers { get; set; }
        public string ManualDuty { get; set; }
        public string LevelOfCommitment { get; set; }
    }

    public class BehaviorSkillCategory : GeneralSchool
    {
        public int BehaviorSkillCategoryId { get; set; }
        //[Index(IsUnique = true)]
        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<BehaviouralSkill> BehaviouralSkill { get; set; }
    }

    public class BehaviouralSkill : GeneralSchool
    {
        public int BehaviouralSkillId { get; set; }

        [StringLength(35)]
        public string SkillName { get; set; }
        public string BehaviorSkillCategoryId { get; set; }
        public BehaviorSkillCategory BehaviorSkillCategories { get; set; }
        public ICollection<AssignBehavior> AssignBehaviors { get; set; }

    }

    public class AssignBehavior : GeneralSchool
    {
        public int AssignBehaviorId { get; set; }
        public string BehaviouralSkillId { get; set; }
        [StringLength(15)]
        public string SkillScore { get; set; }
        [StringLength(50)]
        public string TeacherComment { get; set; }
        [StringLength(25)]
        public string StudentId { get; set; }
        [StringLength(15)]
        public string TermName { get; set; }
        [StringLength(15)]
        public string SessionName { get; set; }
        public int SchoolOpened { get; set; }
        public int NoOfAbsence { get; set; }
        public DateTime Date { get; set; }
        public string BehaviorCategory { get; set; }


        public BehaviouralSkill BehaviouralSkill { get; set; }
    }

    public class AssignBehaviorVm
    {
        public int AssignBehaviorId { get; set; }

        [Display(Name = "Student Name")]
        [Required(ErrorMessage = "Student Name is required")]
        public string StudentId { get; set; }

        [Display(Name = "Term Name")]
        [Required(ErrorMessage = "Term  name is required")]
        public string TermName { get; set; }

        [Display(Name = "Session Name")]
        [Required(ErrorMessage = "Session Name is required")]
        public string SessionName { get; set; }

        [Display(Name = "No of Absence")]
        [Required(ErrorMessage = "No of Absence is required")]
        public int NoOfAbsence { get; set; }


        [Display(Name = "Behavioral Skills")]
        [Required(ErrorMessage = "Behavioral Skills is required")]
        public string[] BehaviouralSkillId { get; set; }

        [Display(Name = "Behavioral Score")]
        [Required(ErrorMessage = "Behavioral Score is required")]
        public string SkillScore { get; set; }

        [Display(Name = "Form Teacher's Comment")]
        [Required(ErrorMessage = "Comment is required")]
        public string TeacherComment { get; set; }

        [Display(Name = "Date")]
        //[Required(ErrorMessage = "Date is required")]
        //[DataType(DataType.Date)]
        public DateTime? Date { get; set; }


    }

    public class ReportCard : GeneralSchool
    {
        public int ReportCardId { get; set; }
        public string TermName { get; set; }
        public string SessionName { get; set; }
        public int SchoolOpened { get; set; }
        public DateTime NextTermBegin { get; set; }
        public DateTime NextTermEnd { get; set; }
        public byte[] PrincipalSignature { get; set; }
    }


}


