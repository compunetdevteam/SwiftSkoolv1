using System.Collections.Generic;

namespace SwiftSkoolv1.Domain.ClassRoom
{
    public class Module : GeneralSchool
    {
        public int ModuleId { get; set; }
        public int SubjectId { get; set; }
        public int ClassId { get; set; }
        public int TermId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDescription { get; set; }
        public int ExpectedTime { get; set; }
        public virtual Subject Subject { get; set; }
        public Class Class { get; set; }
        public Term Term { get; set; }
        public virtual ICollection<Topic> Topics { get; set; }
    }
}