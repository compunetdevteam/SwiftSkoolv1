using System;

namespace SwiftSkoolv1.Domain.Calender
{
    public class Event
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsHoliday { get; set; }
        public bool IsCommonToAll { get; set; }
        public string ThemeColor { get; set; }
        public byte IsFullDay { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}