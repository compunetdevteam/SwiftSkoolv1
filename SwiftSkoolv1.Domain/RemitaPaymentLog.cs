using System;

namespace SwiftSkoolv1.Domain
{
    public class RemitaPaymentLog
    {
        public int RemitaPaymentLogId { get; set; }
        public string SchoolId { get; set; }
        public string OrderId { get; set; }
        public string Amount { get; set; }
        public string Rrr { get; set; }
        public string StatusCode { get; set; }
        public string TransactionMessage { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentName { get; set; }
        public string FeeCategory { get; set; }
    }
}
