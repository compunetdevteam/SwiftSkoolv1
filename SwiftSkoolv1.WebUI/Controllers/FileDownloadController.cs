using Microsoft.AspNet.Identity;
using SwiftSkoolv1.WebUI.Services;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class FileDownloadController : BaseController
    {
        // GET: /FileProcess/  

        DownloadFiles obj;
        public FileDownloadController()
        {
            obj = new DownloadFiles();
        }

        public ActionResult Index()
        {
            var studentId = User.Identity.GetUserId();
            var filesCollection = obj.GetFiles(studentId);
            return View(filesCollection);
        }

        public FileResult Download(string FileId)
        {
            var studentId = User.Identity.GetUserId();
            int CurrentFileID = Convert.ToInt32(FileId);
            var filesCol = obj.GetFiles(studentId);
            string CurrentFileName = (from fls in filesCol
                                      where fls.FileId == CurrentFileID
                                      select fls.FilePath).First();

            string contentType = string.Empty;

            if (CurrentFileName.Contains(".pdf"))
            {
                contentType = "application/pdf";
            }

            else if (CurrentFileName.Contains(".docx"))
            {
                contentType = "application/docx";
            }
            return File(CurrentFileName, contentType, CurrentFileName);
        }
    }
}
