using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.Domain
{
    public class Book : GeneralSchool
    {
        public int BookId { get; set; }

        public int BookCategoryId { get; set; }

        [Key]
        [Display(Name = "Accession Number")]
        [Required(ErrorMessage = "Accession No is required")]
        public string AccessionNo { get; set; }

        [Display(Name = "Book Title")]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Display(Name = "Book Author")]
        [Required(ErrorMessage = "Author is required")]
        public string Author { get; set; }

        [Display(Name = "Joint Author")]
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

        public BookIssue BookIssue { get; set; }
        public BookCategory BookCategory { get; set; }

    }
}