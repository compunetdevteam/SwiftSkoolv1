using System.ComponentModel.DataAnnotations;

namespace SwiftSkool.Models
{
    public class FeeCategory : GeneralSchool
    {
        public int FeeCategoryId { get; set; }
        [StringLength(25)]
        public string CategoryName { get; set; }

        [StringLength(45)]
        public string CategoryDescription { get; set; }
    }


    public class CreateFeeCategoryVM
    {
        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }

        public string CategoryDescription { get; set; }

    }

    public class EditFeeCategoryVM
    {
        public string CategoryName { get; set; }

        public string CategoryDescription { get; set; }

        public int Id { get; set; }
    }
}