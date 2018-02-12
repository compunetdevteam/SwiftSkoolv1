using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using SwiftSkoolv1.Domain;
using SwiftSkoolv1.WebUI.Services;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class ResultUploadsController : BaseController
    {

        // GET: ResultUploads
        public async Task<ActionResult> Index()
        {
            if (User.IsInRole("Student"))
            {
                var student = User.Identity.GetUserId();
                var studentResult = Db.ResultUploads.AsNoTracking().Where(x => x.StudentId.Equals(student)
                            && x.SchoolId.Equals(userSchool)).ToListAsync();
                return View(await studentResult);
            }
            else
            {
                var studentResult = Db.ResultUploads.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool))
                                    .ToListAsync();
                return View(await studentResult);
            }
        }

        // GET: ResultUploads/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResultUpload resultUpload = await Db.ResultUploads.FindAsync(id);

            if (resultUpload == null)
            {
                return HttpNotFound();
            }
            DirectoryInfo dirInfo = new DirectoryInfo(HostingEnvironment.MapPath("~/MyFiles"));
            var fullPath = dirInfo.GetFiles().FirstOrDefault(x => x.Name.Contains(resultUpload.FilePath));
            string CurrentFileName = dirInfo.FullName + @"\" + fullPath;
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
            return View(resultUpload);
        }

        // GET: ResultUploads/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ResultUploads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ResultUpload resultUpload)
        {
            if (ModelState.IsValid)
            {
                Db.ResultUploads.Add(resultUpload);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(resultUpload);
        }

        // GET: ResultUploads/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResultUpload resultUpload = await Db.ResultUploads.FindAsync(id);
            if (resultUpload == null)
            {
                return HttpNotFound();
            }
            return View(resultUpload);
        }

        // POST: ResultUploads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ResultUpload resultUpload)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(resultUpload).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(resultUpload);
        }

        // GET: ResultUploads/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResultUpload resultUpload = await Db.ResultUploads.FindAsync(id);
            if (resultUpload == null)
            {
                return HttpNotFound();
            }
            return View(resultUpload);
        }

        // POST: ResultUploads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ResultUpload resultUpload = await Db.ResultUploads.FindAsync(id);
            Db.ResultUploads.Remove(resultUpload);
            await Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ActionResult UploadResult()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> UploadResult(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please Select a excel file <br/>";
                return View("UploadResult");
            }
            HttpPostedFileBase file = Request.Files["excelfile"];
            if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
            {
                string lastrecord = "";
                int recordCount = 0;
                string message = "";
                string fileContentType = file.ContentType;
                byte[] fileBytes = new byte[file.ContentLength];
                var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                // Read data from excel file
                using (var package = new ExcelPackage(file.InputStream))
                {
                    ExcelValidation myExcel = new ExcelValidation();
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;
                    int requiredField = 4;

                    string validCheck = myExcel.ValidateExcel(noOfRow, workSheet, requiredField);
                    if (!validCheck.Equals("Success"))
                    {
                        //string row = "";
                        //string column = "";
                        string[] ssizes = validCheck.Split(' ');
                        string[] myArray = new string[2];
                        for (int i = 0; i < ssizes.Length; i++)
                        {
                            myArray[i] = ssizes[i];
                            // myArray[i] = ssizes[];
                        }
                        string lineError = $"Line/Row number {myArray[0]}  and column {myArray[1]} is not rightly formatted, Please Check for anomalies ";
                        //ViewBag.LineError = lineError;
                        TempData["UserMessage"] = lineError;
                        TempData["Title"] = "Error.";
                        return View();
                    }

                    for (int row = 2; row <= noOfRow; row++)
                    {
                        string studentId = workSheet.Cells[row, 1].Value.ToString().Trim();
                        string termName = workSheet.Cells[row, 2].Value.ToString().Trim();
                        string sessionName = workSheet.Cells[row, 3].Value.ToString().Trim();
                        string filePath = workSheet.Cells[row, 4].Value.ToString().Trim();

                        try
                        {
                            var resultUpload = new ResultUpload()
                            {
                                StudentId = studentId,
                                TermName = termName,
                                SessionName = sessionName,
                                FilePath = filePath,
                                SchoolId = userSchool,
                            };
                            Db.ResultUploads.Add(resultUpload);

                            recordCount++;
                            lastrecord =
                                $"The last Updated record has the Last Name {studentId} and Term Name {termName} with Session Name {sessionName}";


                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = ex.Message;
                            return View("Error3");
                        }

                    }
                    await Db.SaveChangesAsync();
                    message = $"You have successfully Uploaded {recordCount} records...  and {lastrecord}";
                    TempData["UserMessage"] = message;
                    TempData["Title"] = "Success.";
                    return RedirectToAction("Index", "ResultUploads");
                }
            }
            ViewBag.Error = "File type is Incorrect <br/>";
            return View("UploadResult");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
