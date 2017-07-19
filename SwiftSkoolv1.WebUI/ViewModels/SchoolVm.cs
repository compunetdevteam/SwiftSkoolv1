using SwiftSkoolv1.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Web;

namespace SwiftSkoolv1.WebUI.ViewModels
{
    public class SchoolVm
    {
        [Required]
        public string SchoolId { get; set; }

        [Display(Name = "School Full Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "School Alias")]
        public string Alias { get; set; }

        [StringLength(35)]
        [Display(Name = "School Motto")]
        public string Motto { get; set; }

        [StringLength(35)]
        [Display(Name = "School Website")]
        public string SchoolWebsite { get; set; }

        [StringLength(35)]
        [Display(Name = "Country")]
        public string Country { get; set; }


        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "School Primary Color")]
        public ThemeColor Color { get; set; }

        public OwershipType OwernshipType { get; set; }

        public DateTime? DateOfEstablishment { get; set; }

        public string Address { get; set; }

        [Display(Name = "Local Government Area")]
        public LGA LocalGovtArea { get; set; }


        [Display(Name = "State")]
        public State State { get; set; }

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
                    var target = new MemoryStream();

                    if (value.InputStream == null)
                        return;

                    value.InputStream.CopyTo(target);
                    Logo = target.ToArray();
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
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
                    var target = new MemoryStream();

                    if (value.InputStream == null)
                        return;

                    value.InputStream.CopyTo(target);
                    SchoolBanner = target.ToArray();
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            }
        }

    }
}