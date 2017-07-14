using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwiftSkool.Models
{
    public class Session
    {
        public int SessionId { get; set; }

        [Display(Name = "Session Name")]
        [Required(ErrorMessage = "Session Name is required")]
        [Index(IsUnique = true)]
        [MaxLength(20)]
        public string SessionName { get; set; }

        [Display(Name = "Current Session")]
        public bool ActiveSession { get; set; }
    }

}