using System.ComponentModel.DataAnnotations;
using SwiftSkoolv1.Domain;
//using static SwiftSkoolv1.Domain.PopUp;

namespace SwiftSkool.ViewModel
{
    public class AffectiveViewModel
    {
        public int Id { get; set; }

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

        [Display(Name = "Class Name")]
        [Required(ErrorMessage = "Class Name is required")]
        public string ClassName { get; set; }

        [Display(Name = "Honesty")]
        public Extra Honesty { get; set; }

        [Display(Name = "Self Confidence")]
        public Extra SelfConfidence { get; set; }

        [Display(Name = "Sociability")]
        public Extra Sociability { get; set; }

        [Display(Name = "Punctuality")]
        [Range(1, 3)]
        public Extra Punctuality { get; set; }

        [Display(Name = "Neatness")]
        public Extra Neatness { get; set; }

        [Display(Name = "Initiative")]
        public Extra Initiative { get; set; }

        [Display(Name = "Organization")]
        public Extra Organization { get; set; }

        [Display(Name = "Attendance In Class")]
        public Extra AttendanceInClass { get; set; }

        [Display(Name = "Honesty And Reliability")]
        public Extra HonestyAndReliability { get; set; }

    }

    public class PsycomotorViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Student ID")]
        public string StudentId { get; set; }

        [Display(Name = "Term Name")]
        public string TermName { get; set; }

        [Display(Name = "Session Name")]
        public string SessionName { get; set; }

        [Display(Name = "Class Name")]
        public string ClassName { get; set; }

        [Display(Name = "Sport")]
        public Extra Sports { get; set; }

        [Display(Name = "Extra Curricular Activity ")]
        public Extra ExtraCurricularActivity { get; set; }

        [Display(Name = "Assignment")]
        public Extra Assignment { get; set; }

        [Display(Name = "Helping Others")]
        public Extra HelpingOthers { get; set; }

        [Display(Name = "Manual Duty")]
        public Extra ManualDuty { get; set; }

        [Display(Name = "Level of Commitment")]
        public Extra LevelOfCommitment { get; set; }
    }

    //public class OtherSkillsViewModel
    //{
    //    public int Id { get; set; }
    //    [Display(Name = "Student ID")]
    //    [Required(ErrorMessage = "Your Student ID Number is required")]
    //    [StringLength(15, ErrorMessage = "Your Student ID is too long")]
    //    public string StudentId { get; set; }

    //    [Display(Name = "Term")]
    //    [Required(ErrorMessage = "Term is required")]
    //    public string TermName { get; set; }

    //    [Display(Name = "Session")]
    //    [Required(ErrorMessage = "Session is required")]
    //    public string SessionName { get; set; }

    //    [Display(Name = "Class Name")]
    //    [Required(ErrorMessage = "Class Name is required")]
    //    public string ClassName { get; set; }
    //    [Display(Name = "Team Work/Team Leading")]
    //    [Range(1, 3)]
    //    public int TeamWorkTeamLeading { get; set; }

    //    [Display(Name = "Physical Dexterity")]
    //    [Range(1, 3)]
    //    public int PhysicalDexterity { get; set; }

    //    [Display(Name = "Club And Societies")]
    //    [Range(1, 3)]
    //    public int ClubAndSocieties { get; set; }
    //    [Display(Name = "Artistic Or Musical Skills")]
    //    [Range(1, 3)]
    //    public int ArtisticOrMusicalSkills { get; set; }

    //    [Display(Name = "Lab And Workshop Skills")]
    //    [Range(1, 3)]
    //    public int LabAndWorkshopSkills { get; set; }

    //    [Display(Name = "Sports")]
    //    [Range(1, 3)]
    //    public int Sports { get; set; }
    //}
}