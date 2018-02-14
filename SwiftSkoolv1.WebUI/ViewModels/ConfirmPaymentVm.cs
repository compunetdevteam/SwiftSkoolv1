using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.WebUI.ViewModels
{
    public class ConfirmPaymentVm : RemitaPostVm
    {
        [Display(Name = "Student's Name")]
        public string StudentId { get; set; }
        public string StudentName { get; set; }

        [Display(Name = "Fees Type")]
        public string FeeCategory { get; set; }

        [Display(Name = "Term Name")]
        [Required(ErrorMessage = "Semester is required")]
        public string TermName { get; set; }

        [Display(Name = "Session")]
        [Required(ErrorMessage = "Session is required")]
        public string SessionName { get; set; }

        public int PaymentId { get; set; }

        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

        public RemitaPaymentType RemitaPaymentType { get; set; }

        public List<FeeList> FeeLists { get; set; }
    }
    public enum RemitaPaymentType
    {
        MasterCard = 1, Visa, Verve, PocketMoni, POS, BANK_BRANCH, BANK_INTERNET, REMITA_PAY, RRRGEN
    }
    public class RemitaPostVm
    {
        public string apiKey { get; set; }
        public string payerName { get; set; }
        public string payerEmail { get; set; }
        public string payerPhone { get; set; }
        public string orderId { get; set; }
        public string merchantId { get; set; }
        public string serviceTypeId { get; set; }
        public string responseurl { get; set; }
        public string amt { get; set; }
        public string hash { get; set; }
        public string paymenttype { get; set; }

    }
}