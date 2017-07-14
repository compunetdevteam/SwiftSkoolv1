using HopeAcademySMS.Models;
using System;
using System.Collections.Generic;

namespace SwiftSkool.Models
{
    public class TimeTable
    {
        public int TimeTableId { get; set; }
        public List<Class> Classes { get; set; }
        public List<Subject> Subjects { get; set; }
        public DayOfWeek Days { get; set; }
        public Duration StartDuration { get; set; }
        public Duration EndDuration { get; set; }
    }
}