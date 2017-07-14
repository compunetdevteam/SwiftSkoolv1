using SwiftSkool.Models;
using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.Domain
{
    public class CaSetUp : GeneralSchool
    {
        public int CaSetUpId { get; set; }
        public int CaOrder { get; set; }
        [StringLength(25)]
        public string CaCaption { get; set; }
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