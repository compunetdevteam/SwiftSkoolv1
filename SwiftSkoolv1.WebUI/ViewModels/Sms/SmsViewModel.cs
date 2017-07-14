using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.WebUI.ViewModels.Sms
{
    public class SmsViewModel
    {
        public string ClassName { get; set; }

        public string Term { get; set; }

        public string Session { get; set; }

        public string SenderId { get; set; }

        [Display(Name = "Message")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}