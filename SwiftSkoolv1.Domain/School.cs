using SwiftSkool.ViewModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Web;

namespace SwiftSkool.Models
{
    public class School
    {
        [Key]
        [StringLength(25)]
        public string SchoolId { get; set; }

        [StringLength(45)]
        public string Name { get; set; }
        [StringLength(15)]
        public string Alias { get; set; }

        [StringLength(15)]
        public string Color { get; set; }

        [StringLength(35)]
        public string OwernshipType { get; set; }

        public DateTime? DateOfEstablishment { get; set; }

        [StringLength(75)]
        public string Address { get; set; }
        [StringLength(25)]
        public string LocalGovtArea { get; set; }


        public byte[] Logo { get; set; }
        public byte[] SchoolBanner { get; set; }

    }

    public class SchoolVm
    {

        public string SchoolId { get; set; }

        [Display(Name = "School FullName")]
        public string Name { get; set; }

        [Display(Name = "School Alias")]
        public string Alias { get; set; }

        [Display(Name = "School Primary Color")]
        public PopUp.ThemeColor Color { get; set; }


        public PopUp.OwershipType OwernshipType { get; set; }

        public DateTime? DateOfEstablishment { get; set; }

        public string Address { get; set; }

        [Display(Name = "Local Government Area")]
        public PopUp.LGA LocalGovtArea { get; set; }

        [Display(Name = "School Logo")]
        public byte[] Logo { get; set; }
        public byte[] SchoolBanner { get; set; }

        [Display(Name = "Upload School Logo")]
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
                    MemoryStream target = new MemoryStream();

                    if (value.InputStream == null)
                        return;

                    value.InputStream.CopyTo(target);
                    Logo = target.ToArray();
                }
                catch (Exception)
                {
                    //logger.Error(ex.Message);
                    //logger.Error(ex.StackTrace);
                }
            }
        }

        [Display(Name = "Upload School Banner")]
        [ValidateFile(ErrorMessage = "Please select a PNG/JPEG image smaller than 1MB")]
        [NotMapped]
        public HttpPostedFileBase BannerFile
        {
            get
            {
                return null;
            }

            set
            {
                try
                {
                    MemoryStream target = new MemoryStream();

                    if (value.InputStream == null)
                        return;

                    value.InputStream.CopyTo(target);
                    SchoolBanner = target.ToArray();
                }
                catch (Exception)
                {
                    //logger.Error(ex.Message);
                    //logger.Error(ex.StackTrace);
                }
            }
        }

    }
}