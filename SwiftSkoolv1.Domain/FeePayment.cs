using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwiftSkoolv1.Domain
{
    public class FeePayment : GeneralSchool
    {
        public int FeePaymentId { get; set; }

        [Display(Name = "Student's Name")]
        [Required(ErrorMessage = "Student's Name is required")]
        public string StudentId { get; set; }

        [Index(IsUnique = true)]
        [MaxLength(50)]
        [Required]
        public string OrderId { get; set; }

        public string ReferenceNo { get; set; }
        public string FeeCategory { get; set; }

        [Display(Name = "Fees Name")]
        public string FeeName { get; set; }

        [Display(Name = "Term")]
        [Required(ErrorMessage = "Term is required")]
        public string Term { get; set; }

        [Display(Name = "Session")]
        [Required(ErrorMessage = "Session is required")]
        public string Session { get; set; }

        [Display(Name = "Amount Paid")]
        public decimal PaidFee { get; set; }


        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

        public bool Status { get; set; }

        public string PaymentStatus { get; set; }


        [Display(Name = "Date of Payment")]
        public DateTime Date { get; set; }


        public virtual Student Students { get; set; }
    }
}