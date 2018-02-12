using System.Collections.Generic;

namespace SwiftSkoolv1.Domain.ClassRoom
{
    public class Topic : GeneralSchool
    {
        public int TopicId { get; set; }
        public int ModuleId { get; set; }
        public string TopicName { get; set; }
        public int ExpectedTime { get; set; }
        public virtual Module Module { get; set; }
        public virtual LessonNote LessonNote { get; set; }
        public virtual ICollection<TopicMaterial> TopicMaterials { get; set; }
        public virtual ICollection<TopicAssignment> TopicAssignments { get; set; }

    }
}