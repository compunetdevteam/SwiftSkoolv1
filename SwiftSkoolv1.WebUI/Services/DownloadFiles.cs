using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace SwiftSkoolv1.WebUI.Services
{
    public class DownloadFiles
    {
        public List<DownloadFileInformation> GetFiles()
        {
            List<DownloadFileInformation> lstFiles = new List<DownloadFileInformation>();
            DirectoryInfo dirInfo = new DirectoryInfo(HostingEnvironment.MapPath("~/MyFiles"));

            int i = 0;
            foreach (var item in dirInfo.GetFiles())
            {
                lstFiles.Add(new DownloadFileInformation()
                {
                    FileId = i + 1,
                    FileName = item.Name,
                    FilePath = dirInfo.FullName + @"\" + item.Name
                });
                i = i + 1;
            }
            return lstFiles;
        }

        public List<DownloadFileInformation> GetFiles(string studentId)
        {
            List<DownloadFileInformation> lstFiles = new List<DownloadFileInformation>();
            DirectoryInfo dirInfo = new DirectoryInfo(HostingEnvironment.MapPath("~/MyFiles"));

            int i = 0;
            foreach (var item in dirInfo.GetFiles().Where(x => x.Name.Contains(studentId)))
            {
                lstFiles.Add(new DownloadFileInformation()
                {
                    FileId = i + 1,
                    FileName = item.Name,
                    FilePath = dirInfo.FullName + @"\" + item.Name
                });
                i = i + 1;
            }
            return lstFiles;
        }
    }
}