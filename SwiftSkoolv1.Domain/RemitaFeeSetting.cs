namespace SwiftSkoolv1.Domain
{
    public class RemitaFeeSetting
    {
        public int RemitaFeeSettingId { get; set; }
        public string FeeCategory { get; set; }
        public string SchoolId { get; set; }
        public string ServiceType { get; set; }
        public string MerchantId { get; set; }
        public string ApiKey { get; set; }
        public virtual School School { get; set; }
    }
}
