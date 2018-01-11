namespace SwiftSkoolv1.Domain.ClassRoom
{
    public class ClassRoomAnnoucement : GeneralSchool
    {
        public int ClassRoomAnnoucementId { get; set; }
        public int SubjectId { get; set; }
        public string AssignmentBody { get; set; }
        public string AnnouncementType { get; set; }
        public virtual Subject Subject { get; set; }
    }
}