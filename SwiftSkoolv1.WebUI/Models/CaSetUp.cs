using SwiftSkoolv1.Domain;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Models
{
    public class CaSetUp : GeneralSchool
    {
        public int CaSetUpId { get; set; }
        public int CaOrder { get; set; }
        [StringLength(25)]
        public string CaCaption { get; set; }
        [Remote("ScoreValidation", "CaSetups", ErrorMessage = "Total CA Score is greater than 100", AdditionalFields = "CaSetUpId,ClassName,TermName")]
        public double MaximumScore { get; set; }

        [Remote("PercentageValidation", "CaSetups", ErrorMessage = "Total CA Percentage is greater than 100", AdditionalFields = "CaSetUpId,ClassName,TermName")]
        public double CaPercentage { get; set; }
        /// <summary>
        /// This property is used for enabling Ca settings
        /// </summary>
        public bool IsTrue { get; set; }
        public string ClassName { get; set; }
        public string TermName { get; set; }

    }


}