using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace SwiftSkoolv1.Domain
{
    public class ResultUpload : GeneralSchool
    {
        public int ResultUploadId { get; set; }
        public string StudentId { get; set; }
        public string SessionName { get; set; }
        public string TermName { get; set; }
        public string FilePath { get; set; }
        [NotMapped]
        public HttpPostedFileBase File { get; set; }

    }
}
