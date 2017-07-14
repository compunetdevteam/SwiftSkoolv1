using SwiftSkool.Models;
using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.Domain
{
    public class FeeCategory : GeneralSchool
    {
        public int FeeCategoryId { get; set; }
        [StringLength(25)]
        public string CategoryName { get; set; }

        [StringLength(45)]
        public string CategoryDescription { get; set; }
    }




}