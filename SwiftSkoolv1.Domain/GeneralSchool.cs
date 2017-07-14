using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwiftSkoolv1.Domain
{
    public class GeneralSchool
    {
        //[Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(15)]
        [Index]
        public string SchoolId { get; set; }
    }
}