using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace SwiftSkoolv1.Domain
{
    public class HomePageSetUp
    {
        public string HomePagesetUpId { get; set; }
        public string Title { get; set; }

        public string DescriptiveText { get; set; }

        public string FileLocation { get; set; }

        [NotMapped]
        public HttpPostedFileBase File { get; set; }
    }
}