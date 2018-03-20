using System.Collections.Generic;

namespace SwiftSkoolv1.Domain.Objects
{
    public class Tag : GeneralSchool
    {

        public Tag()
        {
            this.Posts = new HashSet<Post>();
        }

        public int ID { get; set; }
        public string Name { get; set; }


        public virtual ICollection<Post> Posts { get; set; }
    }
}
