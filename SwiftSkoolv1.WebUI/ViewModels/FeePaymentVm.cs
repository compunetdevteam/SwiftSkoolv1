using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.WebUI.ViewModels
{
    public class FeePaymentVm
    {
        [Display(Name = "Student's Name")]
        [Required(ErrorMessage = "Student's Name is required")]
        public string StudentId { get; set; }

        [Display(Name = "Fees Type")]
        public string FeeCategory { get; set; }


        [Display(Name = "Term Name")]
        [Required(ErrorMessage = "Term Name is required")]
        public string TermName { get; set; }

        [Display(Name = "Session")]
        [Required(ErrorMessage = "Session is required")]
        public string SessionName { get; set; }

        [Display(Name = "Amount Paid")]
        public decimal PaidFee { get; set; }


        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

    }

    public class FeeList
    {
        public string FeeTypeName { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}