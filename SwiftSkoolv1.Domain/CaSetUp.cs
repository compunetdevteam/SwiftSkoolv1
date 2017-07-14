using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SwiftSkool.Models
{
    public class CaSetUp : GeneralSchool
    {
        public int CaSetUpId { get; set; }
        public int CaOrder { get; set; }
        [StringLength(25)]
        public string CACaption { get; set; }

        [Remote("MaximumValidation", "CaSetups", ErrorMessage = "Total Ca is greater than 100", AdditionalFields = "CaSetUpId,ClassId,TermId")]
        public double MaximumScore { get; set; }

        public double CaPercentage { get; set; }
        /// <summary>
        /// This property is used for enabling Ca settings
        /// </summary>
        public bool IsTrue { get; set; }
        public int ClassId { get; set; }
        public int TermId { get; set; }
        public virtual Term Term { get; set; }
        public virtual Class Class { get; set; }

    }
}