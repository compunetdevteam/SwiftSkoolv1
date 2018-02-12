using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwiftSkoolv1.Domain.ClassRoom
{

    public class LessonNote : GeneralSchool
    {
        [Key, ForeignKey("Topic")]
        public int TopicId { get; set; }

        [DataType(DataType.MultilineText)]
        public string Note { get; set; }
        public virtual Topic Topic { get; set; }
    }
}
