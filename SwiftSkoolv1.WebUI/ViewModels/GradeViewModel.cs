using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.WebUI.ViewModels
{
    public class GradeViewModel
    {
      
        public int GradeId { get; set; }

        [Display(Name = "Grade Name")]
        [Required(ErrorMessage = "Grade Name is required")]
        public string GradeName { get; set; }

        [Display(Name = "Score for Minimum Value")]
        [Range(1, 100)]
        [Required(ErrorMessage = "Minimum Value is required")]
        public int MinimumValue { get; set; }

        [Display(Name = "Score for Maximum Value")]
        [Range(1, 100)]
        [Required(ErrorMessage = "Maximum Value is required")]
        public int MaximumValue { get; set; }

        //[Display(Name = "Score for Grade Point")]
        //[Range(1, 9)]
        //[Required(ErrorMessage = "Maximum Value is required")]
        //public int GradePoint { get; set; }

        [Display(Name = "School Remark")]
        [Required(ErrorMessage = "Remark is required")]
        public string Remark { get; set; }

        // [Display(Name = "School Remark")]
        // [Required(ErrorMessage = "Remark is required")]
        public string[] ClassName { get; set; }

    }

}