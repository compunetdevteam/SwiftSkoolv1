using System.ComponentModel.DataAnnotations.Schema;

namespace SwiftSkoolv1.Domain
{
    public class GeneralSchool
    {
        //[Required]
        [Column(TypeName = "VARCHAR")]
        [Index]
        public string SchoolId { get; set; }
    }
}