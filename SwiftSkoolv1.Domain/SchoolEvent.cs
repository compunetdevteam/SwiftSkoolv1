using System;

namespace SwiftSkoolv1.Domain
{
    public class SchoolEvent : GeneralSchool
    {
        public int SchoolEventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsHoliday { get; set; }
        public bool IsCommonToAll { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
