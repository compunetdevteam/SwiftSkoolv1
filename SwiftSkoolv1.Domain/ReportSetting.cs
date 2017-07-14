namespace SwiftSkool.Models
{
    public class ReportSetting : GeneralSchool
    {
        public int ReportsettingId { get; set; }
        public bool TotalSubjectScore { get; set; }
        public bool SubjectPosition { get; set; }
        public bool ClassAverage { get; set; }
        public bool Average { get; set; }

        public bool OverAllGrade { get; set; }
        public bool SubjectHighest { get; set; }
        public bool SubjectLowest { get; set; }
        public bool AggregatePosition { get; set; }
        public bool TotalNumberInClass { get; set; }
        public bool TotalSubjectOfferedByStudent { get; set; }
        public bool SchoolActivityCalender { get; set; }

    }
}