using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.Domain
{
    public class FeeType : GeneralSchool
    {
        public int Id { get; set; }
        [StringLength(25)]
        public string FeeName { get; set; }
        [StringLength(45)]
        public string Description { get; set; }
    }
}