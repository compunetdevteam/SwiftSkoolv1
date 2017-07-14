
using System;

namespace SwiftSkoolv1.Domain
{
    public class TimeTable
    {
        public int TimeTableId { get; set; }
        public int ClassId { get; set; }
        public int SubjectId { get; set; }
        public DayOfWeek Days { get; set; }
        public Duration StartDuration { get; set; }
        public Duration EndDuration { get; set; }

        public virtual Class Classes { get; set; }
        public virtual Subject Subjects { get; set; }
    }
}