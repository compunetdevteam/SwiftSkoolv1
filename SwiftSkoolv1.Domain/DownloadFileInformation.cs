using System.ComponentModel.DataAnnotations;

namespace SwiftSkool.Models
{
    public class DownloadFileInformation
    {
        [Key]
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}