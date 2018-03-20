using System.ComponentModel.DataAnnotations;
using System.Web;

namespace SwiftSkoolv1.Domain
{
    public class Book : GeneralSchool
    {
        public int BookId { get; set; }

        [Display(Name = "Book Title")]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Display(Name = "Book Author")]
        [Required(ErrorMessage = "Author is required")]
        public string Author { get; set; }

        [Display(Name = "Book Subject")]
        [Required(ErrorMessage = "Book Subject is required")]
        public string SubjectName { get; set; }

        [Display(Name = "Class Name")]
        public string ClassName { get; set; }

        [Display(Name = "Book Edition")]
        public string Edition { get; set; }

        public string BookLocation { get; set; }


    }

    public class BookVm : GeneralSchool
    {
        public int BookId { get; set; }

        [Display(Name = "Book Title")]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Display(Name = "Book Author")]
        [Required(ErrorMessage = "Author is required")]
        public string Author { get; set; }

        [Display(Name = "Book Subject")]
        [Required(ErrorMessage = "Book Subject is required")]
        public string SubjectName { get; set; }

        [Display(Name = "Class Name")]
        public string[] ClassName { get; set; }

        [Display(Name = "Book Edition")]
        public string Edition { get; set; }

        public string BookLocation { get; set; }

        [Display(Name = "Upload Book")]
        public HttpPostedFileBase File { get; set; }



    }
}