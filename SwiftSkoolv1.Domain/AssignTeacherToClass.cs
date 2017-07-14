namespace SwiftSkool.Models
{
    public class AssignFormTeacherToClass : GeneralSchool
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public string Username { get; set; }
    }

    public class AssignSubjectTeacher : GeneralSchool
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public string ClassName { get; set; }
        public string StaffName { get; set; }

        public virtual Subject Subject { get; set; }
    }
}