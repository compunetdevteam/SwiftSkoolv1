using System.ComponentModel.DataAnnotations;

namespace SwiftSkool.Models
{
    public class Book : GeneralSchool
    {
        public int BookId { get; set; }

        [Key]
        [Display(Name = "Accession Number")]
        [Required(ErrorMessage = "Accession No is required")]
        [StringLength(25)]
        public string AccessionNo { get; set; }

        [Display(Name = "Book Title")]
        [Required(ErrorMessage = "Title is required")]
        [StringLength(50)]
        public string Title { get; set; }

        [Display(Name = "Book Author")]
        [Required(ErrorMessage = "Author is required")]
        [StringLength(50)]
        public string Author { get; set; }

        [Display(Name = "Joint Author")]
        [StringLength(50)]
        public string JointAuthor { get; set; }

        [Display(Name = "Book Subject")]
        [Required(ErrorMessage = "Book Subject is required")]
        [StringLength(30)]
        public string Subject { get; set; }

        [Display(Name = "Book's ISBN")]
        [StringLength(35)]
        [Required(ErrorMessage = "ISBN is required")]
        public string ISBN { get; set; }

        [Display(Name = "Book Edition")]
        [Required(ErrorMessage = "Edition is required")]
        public string Edition { get; set; }

        [Display(Name = "Book Publisher")]
        [Required(ErrorMessage = "Publisher is required")]
        [StringLength(35)]
        public string Publisher { get; set; }

        [Display(Name = "Place of Publish")]
        [StringLength(35)]
        public string PlaceOfPublish { get; set; }

        public virtual BookIssue BookIssue { get; set; }

    }
}