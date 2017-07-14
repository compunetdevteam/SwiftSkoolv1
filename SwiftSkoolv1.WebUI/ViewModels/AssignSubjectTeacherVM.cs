using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.WebUI.ViewModels
{
    public class AssignSubjectTeacherVM
    {
        public int AssignTeacherToSubjectId { get; set; }

        [Display(Name = "Subject Name")]
        public int SubjectId { get; set; }

        [Display(Name = "Class Name")]
        public string[] ClassName { get; set; }

        [Display(Name = "Staff Name")]
        public string StaffName { get; set; }

    }
}