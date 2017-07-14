namespace SwiftSkool.BusinessLogic
{
    public interface IResultCommandManager
    {
        int FindAggregatePosition(string studentId, string className, string term, string session);
        int FindSubjectPosition(string studentId, string subject, string className, string term, string session);
        int NumberOfStudentPerClass(string className, string term, string session);
        int SubjectOfferedByStudent(string studentId, string term);
        double TotalScorePerStudent(string studentId, string className, string term, string session);
       // double TotalScorePerSubject(string subject, string className, string term, string session);

        double CalculateClassAverage(string className, string term, string sessionName, string subject);

        double CalculateAverage(string studentId, string className, string term, string sessionName);
    }
}