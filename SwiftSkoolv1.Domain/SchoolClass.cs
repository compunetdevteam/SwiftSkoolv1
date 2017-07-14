using System.ComponentModel.DataAnnotations;

namespace SwiftSkool.Models
{
    public class SchoolClass
    {
        public int SchoolClassId { get; set; }

        [Display(Name = "School Class Code")]
        [Required(ErrorMessage = "School Class code is required")]
        // [Index(IsUnique = true)]
        [MaxLength(20)]
        public string ClassCode { get; set; }


        [Display(Name = "School Class Name")]
        [Required(ErrorMessage = "School Class name is required")]
        [StringLength(35)]
        public string ClassName { get; set; }

    }
}