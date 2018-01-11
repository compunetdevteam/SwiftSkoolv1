using SwiftSkoolv1.WebUI.ViewModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Web;

namespace SwiftSkoolv1.Domain
{
    public class ReportCardSetting : GeneralSchool
    {
        public int ReportCardSettingId { get; set; }

        [Display(Name = "Resumption Date")]
        [DataType(DataType.Date)]
        public DateTime ResumptionDate { get; set; }

        [Display(Name = "Closing Date")]
        [DataType(DataType.Date)]
        public DateTime ClosingDate { get; set; }

        [Range(1, 300)]
        public int NoOfTimesSchoolOpened { get; set; }

        public byte[] PrincipalSignature { get; set; }

        [Display(Name = "Upload Principal Signature")]
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

    public class AssignReportCard : GeneralSchool
    {
        public int AssignReportCardId { get; set; }
        public ReportCardType ReportCardType { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public Class Class { get; set; }

    }
}
