using SwiftSkool.Models;

namespace SwiftSkool.Calender
{
    public partial class AppointmentDiary : GeneralSchool
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int SomeImportantKey { get; set; }
        public System.DateTime DateTimeScheduled { get; set; }
        public int AppointmentLength { get; set; }
        public int StatusENUM { get; set; }
    }
}
