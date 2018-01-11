using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.Domain
{
    public class BookIssue : GeneralSchool
    {
        public int BookIssueId { get; set; }

        [Display(Name = "Student's Name")]
        [Required(ErrorMessage = "Student's Name is required")]
        public string StudentId { get; set; }

        [Display(Name = "Accession Number")]
        [Required(ErrorMessage = "Accession No is required")]
        public string AccessionNo { get; set; }

        [Display(Name = "Issued Date Number")]
        [Required(ErrorMessage = "Issued Date is Required")]
        public DateTime IssueDate { get; set; }

        [Display(Name = "Due Date Number")]
        [Required(ErrorMessage = "Issued Date is Required")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Book Status")]
        public Status Status { get; set; }

        public virtual ICollection<Book> Books { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}