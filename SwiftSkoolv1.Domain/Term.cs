using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SwiftSkoolv1.Domain;
using SwiftSkoolv1.Domain.CBT;

namespace SwiftSkool.Models
{
    public class Term
    {
        [Key]
        public int TermId { get; set; }

        [Index(IsUnique = true)]
        [StringLength(15)]
        public string TermName { get; set; }

        [Display(Name = "Current Term")]
        public bool ActiveTerm { get; set; }
        public virtual ICollection<CaSetUp> SchoolCas { get; set; }
        public ICollection<ExamLog> ExamLogs { get; set; }
        public ICollection<ExamSetting> ExamSettings { get; set; }
    }
}