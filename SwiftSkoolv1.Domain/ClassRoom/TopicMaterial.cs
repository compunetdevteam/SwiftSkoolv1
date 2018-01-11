using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace SwiftSkoolv1.Domain.ClassRoom
{
    public class TopicMaterial : GeneralSchool
    {
        public int TopicMaterialId { get; set; }
        public int TopicId { get; set; }
        public FileType FileType { get; set; }

        [Display(Name = "Material Name")]
        [Required(ErrorMessage = "Material Name is required")]
        public string Name { get; set; }

        [Display(Name = "Note")]
        public string NoteBody { get; set; }

        [Display(Name = "Material Author")]
        [Required(ErrorMessage = "Material Author is required")]
        public string Author { get; set; }

        [Display(Name = " Short Description")]
        //[Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        public string FileLocation { get; set; }

        [NotMapped]
        public HttpPostedFileBase File { get; set; }

        public virtual Topic Topic { get; set; }
    }
}