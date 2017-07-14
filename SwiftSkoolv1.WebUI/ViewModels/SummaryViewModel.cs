using SwiftSkool.Models;
using System.Collections.Generic;

namespace SwiftSkool.ViewModel
{
    public class SummaryViewModel
    {
        public int NoOfStudentPerClass { get; set; }
        public Student Student { get; set; }
        public List<ContinuousAssessment> ContinuousAssessments { get; set; }
        public List<Result> Results { get; set; }
    }
}