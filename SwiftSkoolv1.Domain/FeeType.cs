namespace SwiftSkoolv1.Domain
{
    public class FeeType
    {
        public int FeeTypeId { get; set; }
        public string SchoolId { get; set; }
        public string ClassName { get; set; }
        public string TermName { get; set; }
        public string FeeCategory { get; set; }
        public string FeeName { get; set; }
        public string StudentType { get; set; }
        public decimal Amount { get; set; }
        public string AmountInWords { get; set; }
        public string Description { get; set; }
        public virtual School School { get; set; }

    }
}