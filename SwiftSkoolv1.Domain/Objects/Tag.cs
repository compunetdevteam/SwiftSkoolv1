using System.Collections.Generic;

namespace SwiftSkool.Models.Objects
{
    public class Tag
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
