namespace SwiftSkoolv1.Domain
{
    public class ParentEmailAddress : GeneralSchool
    {
        public int ParentEmailAddressId { get; set; }
        public string StudentId { get; set; }
        public string EmailAddress { get; set; }
        public bool IsActive { get; set; }

    }
}
