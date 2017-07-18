namespace SwiftSkoolv1.Domain
{
    public class ResultAvailability
    {
        public int ResultAvailabilityId { get; set; }
        public int TermId { get; set; }
        public int SessionId { get; set; }
        public bool MakeResultAvailable { get; set; }
        public virtual Term Term { get; set; }
        public virtual Session Session { get; set; }
    }
}
