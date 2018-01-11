using System.Collections.Generic;

namespace SwiftSkoolv1.Domain
{
    public class BookCategory : GeneralSchool
    {
        public int BookCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
