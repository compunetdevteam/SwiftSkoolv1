using SwiftSkoolv1.Domain;
using System.Collections.Generic;

namespace SwiftSkoolv1.WebUI.ViewModels
{
    public class ReportVm
    {
        public string ClassName { get; set; }
        public string TermName { get; set; }
        public string SessionName { get; set; }
        public int NoOfStudentPerClass { get; set; }
        public int NoOfSubjectOffered { get; set; }
        public double AggregateScore { get; set; }
        public int AggregatePosition { get; set; }
        public double Average { get; set; }
        public double PercentageAverage { get; set; }
        public string OverAllGrade { get; set; }
        public string PrincipalComment { get; set; }

        public Student Student { get; set; }
        public List<ContinuousAssesmentVm> ContinuousAssesmentVms { get; set; }
        public List<AssignBehavior> AssignBehaviors { get; set; }
        public AssignBehavior AssignBehavior { get; set; }
        public List<string> BehaviorCategory { get; set; }

        public ReportCard ReportCard { get; set; }

        public List<CaSetUp> CaSetUp { get; set; }
        public int CaSetUpCount { get; set; }

        //public double GPA { get; set; }
        //public double TotalQualityPoint { get; set; }
        //public double TotalCreditUnit { get; set; }
        //public double GradePointAverage { get; set; }
        //public List<Result> Results { get; set; }
        //public List<ContinuousAssessment> Maths { get; set; }
        //public List<ContinuousAssessment> English { get; set; }

    }
}