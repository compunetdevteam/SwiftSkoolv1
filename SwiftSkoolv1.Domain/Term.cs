using SwiftSkoolv1.Domain.CBT;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwiftSkoolv1.Domain
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
        public ICollection<ResultAvailability> ResultAvailabilities { get; set; }
    }
}