using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace SwiftSkoolv1.Domain.ClassRoom
{
    public class TopicAssignment : GeneralSchool
    {
        public int TopicAssignmentId { get; set; }
        public int TopicId { get; set; }
        public string AssignmentTitle { get; set; }
        public string AssignmentDescription { get; set; }
        public DateTime? AssignmentDueDate { get; set; }
        public virtual Topic Topic { get; set; }
        public virtual ICollection<SubmitAssignment> SubmitAssignments { get; set; }
    }

    public class SubmitAssignment : GeneralSchool
    {
        public int SubmitAssignmentId { get; set; }
        public int TopicAssignmentId { get; set; }
        public string StudentId { get; set; }
        public string AssignmentBody { get; set; }
        public string AttachmentLocation { get; set; }

        [NotMapped]
        public HttpPostedFileBase File { get; set; }

        public virtual Student Student { get; set; }
        public virtual TopicAssignment TopicAssignment { get; set; }
        public virtual AssignmentReview AssignmentReview { get; set; }
    }

    public class AssignmentReview : GeneralSchool
    {
        [Key, ForeignKey("SubmitAssignment")]
        public int SubmitAssignmentId { get; set; }

        public string ReviewComment { get; set; }
        public string Rating { get; set; }
        public virtual SubmitAssignment SubmitAssignment { get; set; }
    }
}