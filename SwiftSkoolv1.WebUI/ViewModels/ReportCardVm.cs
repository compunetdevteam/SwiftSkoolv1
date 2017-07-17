using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Web;

namespace SwiftSkoolv1.WebUI.ViewModels
{
    public class ReportCardVm
    {
        public int ReportCardId { get; set; }


        [Display(Name = "Term")]
        [Required(ErrorMessage = "Term")]
        public string TermName { get; set; }

        [Display(Name = "Session Name")]
        [Required(ErrorMessage = "Session Name is required")]
        public string SessionName { get; set; }

        [Display(Name = "No of Times School Opened")]
        [Required(ErrorMessage = "Session Name is required")]
        public int SchoolOpened { get; set; }

        [Display(Name = "Next Term Begin")]
        [Required(ErrorMessage = "Next Term Begin is required")]
        [DataType(DataType.Date)]
        public DateTime NextTermBegin { get; set; }

        [Display(Name = "Next Term End")]
        [Required(ErrorMessage = "Next Term End is required")]
        [DataType(DataType.Date)]
        public DateTime NextTermEnd { get; set; }

        public byte[] PrincipalSignature { get; set; }

        [Display(Name = "Upload A Principal's Signature")]
        [ValidateFile(ErrorMessage = "Please select a PNG/JPEG image smaller than 1MB")]
        [NotMapped]
        public HttpPostedFileBase File
        {
            get
            {
                return null;
            }

            set
            {
                try
                {
                    var target = new MemoryStream();

                    if (value.InputStream == null)
                        return;

                    value.InputStream.CopyTo(target);
                    PrincipalSignature = target.ToArray();
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            }
        }
    }
}