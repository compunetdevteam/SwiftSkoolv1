using SwiftSkool.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwiftSkoolv1.Domain
{
    public class FeePayment : GeneralSchool
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FeePaymentId { get; set; }

        [Display(Name = "Student's Name")]
        [Required(ErrorMessage = "Student's Name is required")]
        [StringLength(25)]
        public string StudentId { get; set; }

        [Display(Name = "Fees Type")]
        [StringLength(25)]
        public string FeeName { get; set; }

        [Display(Name = "Term")]
        [Required(ErrorMessage = "Term is required")]
        [StringLength(15)]
        public string Term { get; set; }

        [Display(Name = "Session")]
        [Required(ErrorMessage = "Session is required")]
        [StringLength(15)]
        public string Session { get; set; }

        [Display(Name = "Amount Paid")]
        public decimal PaidFee { get; set; }


        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }


        [Display(Name = "Payment Method")]
        public PMode PaymentMode { get; set; }

        [Display(Name = "Date of Payment")]
        public DateTime Date { get; set; }


        [Display(Name = "Remaining Fee")]
        public decimal Remaining
        {
            get
            {
                return TotalAmount - PaidFee;
            }
            set { }
        }

        public virtual Student Students { get; set; }
        public virtual FeeType FeeType { get; set; }
    }
}