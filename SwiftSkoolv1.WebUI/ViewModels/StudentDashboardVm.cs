using System.Collections.Generic;

namespace SwiftSkoolv1.WebUI.ViewModels
{
    public class StudentDashboardVm
    {
        public string ClassName { get; set; }
        public List<string> Subjects { get; set; }
        public int MaleStudents { get; set; }
        public int FemaleStudents { get; set; }
        public int TotalStudents { get; set; }
        public string FormTeacher { get; set; }
        public string Term { get; set; }
        public string Session { get; set; }
        public int TotalNumberOfStaff { get; set; }
        public double BoysPercentage { get; set; }
        public double FemalePercentage { get; set; }
        public int ActiveStudent { get; set; }
        public int GraduatedStudent { get; set; }
        public List<Student> ListStudents { get; set; }
        public List<AssignSubjectTeacher> SubjectTeachers { get; set; }
    }

    public class AdminDashboardVm
    {
        public string ClassName { get; set; }
        public List<string> Subjects { get; set; }
        public int MaleStudents { get; set; }
        public int FemaleStudents { get; set; }
        public int TotalStudents { get; set; }
        public string FormTeacher { get; set; }
        public string Term { get; set; }
        public string Session { get; set; }
    }
}