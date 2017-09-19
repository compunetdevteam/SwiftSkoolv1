using HopeAcademySMS.Services;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using PagedList;
using SwiftSkoolv1.Domain;
using SwiftSkoolv1.WebUI.Controllers;
using SwiftSkoolv1.WebUI.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

//using Excel = Microsoft.Office.Interop.Excel;

namespace SwiftSkool.Controllers
{
    public class ContinuousAssessmentsController : BaseController
    {
        // GET: ContinuousAssessments
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string search, int? page,
            int? SubjectId, string ClassName, string TermName, string SessionName)
        {
            int count = 10;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (search != null)
            {

                page = 1;
            }
            else
            {
                search = currentFilter;
            }
            ViewBag.CurrentFilter = search;
            var assignedList = from s in Db.ContinuousAssessments select s;
            if (Request.IsAuthenticated && !User.IsInRole("SuperAdmin"))
            {
                assignedList = assignedList.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool));
            }
            if (User.IsInRole("Teacher"))
            {
                string name = User.Identity.GetUserName();
                //var user = Db.Guardians.Where(c => c.UserName.Equals(name)).Select(s => s.Email).FirstOrDefault();

                assignedList = assignedList.AsNoTracking().Where(x => x.StaffName.Equals(name));

                //return View(subjectName);
            }
            else
            {
                if (!String.IsNullOrEmpty(search))
                {
                    assignedList = assignedList.AsNoTracking().Where(s => s.StudentId.ToUpper().Contains(search.ToUpper())
                                                                 || s.ClassName.ToUpper().Contains(search.ToUpper())
                                                                 || s.TermName.ToUpper().Contains(search.ToUpper()));

                }
                else if (SubjectId != null && (!String.IsNullOrEmpty(ClassName)
                       && !String.IsNullOrEmpty(SessionName) && !String.IsNullOrEmpty(TermName)))
                {
                    assignedList = assignedList.AsNoTracking().Where(s => s.SubjectId.Equals(SubjectId)
                                               && s.ClassName.ToUpper().Equals(ClassName.ToUpper())
                                               && s.TermName.ToUpper().Equals(TermName.ToUpper())
                                               && s.SessionName.ToUpper().Equals(SessionName))
                                               .OrderBy(c => c.StudentId);
                    int myCount = await assignedList.CountAsync();
                    if (myCount != 0)
                    {
                        count = myCount;
                    }
                }
                else if (SubjectId != null || (!String.IsNullOrEmpty(ClassName)
                                                                || !String.IsNullOrEmpty(SessionName) ||
                                                                !String.IsNullOrEmpty(TermName)))
                {
                    assignedList = assignedList.AsNoTracking().Where(s => s.SubjectId.Equals(SubjectId)
                                                           || s.ClassName.ToUpper().Equals(ClassName.ToUpper())
                                                           || s.TermName.ToUpper().Equals(TermName.ToUpper())
                                                           || s.SessionName.ToUpper().Equals(SessionName));
                }

            }


            switch (sortOrder)
            {
                case "name_desc":
                    assignedList = assignedList.AsNoTracking().OrderByDescending(s => s.StudentId);
                    break;
                case "Date":
                    assignedList = assignedList.AsNoTracking().OrderBy(s => s.SessionName);
                    break;
                default:
                    assignedList = assignedList.AsNoTracking().OrderBy(s => s.ClassName);
                    break;
            }
            // ViewBag.SubjectCode = new SelectList(Db.Subjects, "CourseName", "CourseName");
            ViewBag.TermName = new SelectList(Db.Terms.AsNoTracking(), "TermName", "TermName");
            ViewBag.SessionName = new SelectList(Db.Sessions.AsNoTracking(), "SessionName", "SessionName");

            if (User.IsInRole("Teacher"))
            {
                string name = User.Identity.GetUserName();
                var subjectList = Db.AssignSubjectTeachers.AsNoTracking().Where(x => x.StaffName.Equals(name));
                ViewBag.SubjectId = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
                ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "ClassName", "ClassName");
            }
            else
            {
                ViewBag.SubjectId = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
                ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "ClassName", "ClassName");
            }
            int pageSize = count;
            int pageNumber = (page ?? 1);
            return View(assignedList.ToPagedList(pageNumber, pageSize));
            //return View(await Db.ContinuousAssessments.ToListAsync());
        }

        public async Task<ActionResult> StudentIndex(string id, int? page)
        {
            int count = 50;

            var username = User.Identity.GetUserId();
            //var user = await Db.Users.AsNoTracking().Where(c => c.Id.Equals(username)).Select(c => c.Email).FirstOrDefaultAsync();
            if (id == null)
            {
                id = username;
            }

            var assignedList = from s in Db.ContinuousAssessments select s;


            assignedList = assignedList.AsNoTracking().Where(s => s.StudentId.ToUpper().Contains(id.ToUpper()));

            int pageSize = count;
            int pageNumber = (page ?? 1);
            return View(assignedList.ToPagedList(pageNumber, pageSize));
            //return View(await Db.ContinuousAssessments.ToListAsync());
        }


        // GET: ContinuousAssessments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var continuousAssessment = await Db.ContinuousAssessments.FindAsync(id);
            if (continuousAssessment == null)
            {
                return HttpNotFound();
            }
            return View(continuousAssessment);
        }

        // GET: ContinuousAssessments/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.StudentId = new SelectList(await _query.StudentListAsync(userSchool), "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(_query.SessionList(), "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(Db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            ViewBag.TermName = new SelectList(_query.TermList(), "TermName", "TermName");
            ViewBag.StaffName = User.Identity.GetUserName();
            if (User.IsInRole("Teacher"))
            {
                string name = User.Identity.GetUserName();
                var assignedList = Db.AssignSubjectTeachers.Where(x => x.StaffName.Equals(name));
                ViewBag.SubjectId = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
            }
            else
            {
                ViewBag.SubjectId = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
            }
            return View();
        }

        // POST: ContinuousAssessments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ContinuousAssesmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var mysubjectCategory = Db.Subjects.Where(x => x.CourseCode.Equals(model.SubjectCode))
                //                        .Select(c => c.CategoriesId).FirstOrDefault();

                //var subjectName = Db.Subjects.Where(x => x.CourseCode.Equals(model.SubjectCode))
                //                        .Select(c => c.CourseName).FirstOrDefault();
                //var student = Db.AssignedClasses.Where(x => x.ClassName.Equals(model.ClassName)
                //                                               && x.TermName.Contains(model.TermName.ToString())
                //                                               && x.SessionName.Equals(model.SessionName));
                var CA = Db.ContinuousAssessments.AsNoTracking().Where(x => x.ClassName.Equals(model.ClassName)
                                                                  && x.TermName.Contains(model.TermName.ToString())
                                                                  && x.SessionName.Equals(model.SessionName)
                                                                  && x.StudentId.Equals(model.StudentId)
                                                                  && x.SubjectId.Equals(model.SubjectId));
                var countFromDb = await CA.CountAsync();
                if (countFromDb >= 1)
                {
                    ViewBag.StudentId = new SelectList(await _query.StudentListAsync(userSchool), "StudentID", "FullName");
                    ViewBag.SessionName = new SelectList(_query.SessionList(), "SessionName", "SessionName");
                    ViewBag.ClassName = new SelectList(Db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
                    ViewBag.TermName = new SelectList(_query.TermList(), "TermName", "TermName");
                    ViewBag.SubjectId = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");

                    TempData["UserMessage"] = "Record Already Exist in Database.";
                    TempData["Title"] = "Error.";
                    return View(model);
                }
                else
                {
                    var myContinuousAssessment = new ContinuousAssessment()
                    {
                        StudentId = model.StudentId,
                        SubjectId = model.SubjectId,
                        FirstTest = model.FirstTest,
                        SecondTest = model.SecondTest,
                        ThirdTest = model.ThirdTest,
                        ExamScore = model.ExamScore,
                        TermName = model.TermName,
                        SessionName = model.SessionName,
                        ClassName = model.ClassName,
                        StaffName = model.StaffName,
                        SchoolId = userSchool
                        //SubjectCategory = mysubjectCategory
                    };
                    Db.ContinuousAssessments.Add(myContinuousAssessment);
                }
                await Db.SaveChangesAsync();
                TempData["UserMessage"] = "Continuous Assessment Added Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            ViewBag.StudentId = new SelectList(await _query.StudentListAsync(userSchool), "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(_query.SessionList(), "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(Db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            ViewBag.TermName = new SelectList(_query.TermList(), "TermName", "TermName");
            ViewBag.SubjectId = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
            ViewBag.StaffName = User.Identity.GetUserName();

            return View(model);
        }

        // GET: ContinuousAssessments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContinuousAssessment continuousAssessment = await Db.ContinuousAssessments.FindAsync(id);
            if (continuousAssessment == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentId = new SelectList(await _query.StudentListAsync(userSchool), "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(_query.SessionList(), "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(Db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            ViewBag.TermName = new SelectList(_query.TermList(), "TermName", "TermName");
            ViewBag.SubjectId = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
            ViewBag.StaffName = User.Identity.GetUserName();
            var model = new ContinuousAssesmentViewModel()
            {
                ContinuousAssessmentId = continuousAssessment.ContinuousAssessmentId,
                StudentId = continuousAssessment.StudentId,
                FirstTest = continuousAssessment.FirstTest,
                SecondTest = continuousAssessment.SecondTest,
                ThirdTest = continuousAssessment.ThirdTest,
                ExamScore = continuousAssessment.ExamScore
            };
            return View(model);
        }

        // POST: ContinuousAssessments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ContinuousAssesmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var myContinuousAssessment = new ContinuousAssessment()
                {
                    ContinuousAssessmentId = model.ContinuousAssessmentId,
                    StudentId = model.StudentId,
                    SubjectId = model.SubjectId,
                    FirstTest = model.FirstTest,
                    SecondTest = model.SecondTest,
                    ThirdTest = model.ThirdTest,
                    ExamScore = model.ExamScore,
                    TermName = model.TermName.ToString(),
                    SessionName = model.SessionName,
                    ClassName = model.ClassName,
                    StaffName = model.StaffName,
                    SchoolId = userSchool
                    //SubjectCategory = mysubjectCategory
                };
                // Db.ContinuousAssessments.Add(myContinuousAssessment);
                Db.Entry(myContinuousAssessment).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                TempData["UserMessage"] = "Continuous Assessment Updated Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            ViewBag.StudentId = new SelectList(await _query.StudentListAsync(userSchool), "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(_query.SessionList(), "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(Db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            ViewBag.TermName = new SelectList(_query.TermList(), "TermName", "TermName");
            ViewBag.SubjectId = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
            ViewBag.StaffName = User.Identity.GetUserName();
            return View(model);
        }

        // GET: ContinuousAssessments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var continuousAssessment = await Db.ContinuousAssessments.FindAsync(id);
            if (continuousAssessment == null)
            {
                return HttpNotFound();
            }
            return View(continuousAssessment);
        }

        // POST: ContinuousAssessments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var continuousAssessment = await Db.ContinuousAssessments.FindAsync(id);
            if (continuousAssessment != null) Db.ContinuousAssessments.Remove(continuousAssessment);
            await Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
            }
            base.Dispose(disposing);
        }

        public PartialViewResult UploadResult()
        {
            //ViewBag.CourseName = new SelectList(Db.Courses, "CourseName", "CourseName");
            return PartialView();
        }

        [HttpPost]
        public async Task<ActionResult> UploadResult(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please Select a excel file <br/>";
                return View();
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
                    var currentSheet = package.Workbook.Worksheets;
                    foreach (var sheet in currentSheet)
                    {
                        ExcelValidation myExcel = new ExcelValidation();
                        //var workSheet = currentSheet.First();
                        var noOfCol = sheet.Dimension.End.Column;
                        var noOfRow = sheet.Dimension.End.Row;
                        int requiredField = 10;

                        string validCheck = myExcel.ValidateExcel(noOfRow, sheet, requiredField);
                        if (!validCheck.Equals("Success"))
                        {

                            string[] ssizes = validCheck.Split(' ');
                            string[] myArray = new string[2];
                            for (int i = 0; i < ssizes.Length; i++)
                            {
                                myArray[i] = ssizes[i];
                            }
                            string lineError = $"Please Check sheet {sheet}, Line/Row number {myArray[0]}  and column {myArray[1]} is not rightly formatted, Please Check for anomalies ";
                            //ViewBag.LineError = lineError;
                            TempData["UserMessage"] = lineError;
                            TempData["Title"] = "Error.";
                            return View();
                        }

                        for (int row = 2; row <= noOfRow; row++)
                        {
                            string studentId = sheet.Cells[row, 1].Value.ToString().ToUpper().Trim();
                            string subjectValue = sheet.Cells[row, 2].Value.ToString().ToUpper().Trim();
                            string termName = sheet.Cells[row, 7].Value.ToString().Trim().ToUpper();
                            string className = sheet.Cells[row, 10].Value.ToString().Trim().ToUpper();
                            string sessionName = sheet.Cells[row, 8].Value.ToString().Trim();

                            var subjectName = Db.Subjects.Where(x => x.SubjectCode.Equals(subjectValue))
                                .Select(c => c.SubjectId).FirstOrDefault();

                            var CA = Db.ContinuousAssessments.Where(x => x.ClassName.Equals(className)
                                                                          && x.TermName.Contains(termName)
                                                                          && x.SessionName.Equals(sessionName)
                                                                          && x.StudentId.Equals(studentId)
                                                                          && x.SubjectId.Equals(subjectName));
                            var countFromDb = await CA.CountAsync();
                            if (countFromDb >= 1)
                            {
                                return View("Error2");
                            }
                            var mycontinuousAssessment = new ContinuousAssessment
                            {
                                StudentId = studentId,
                                SubjectId = subjectName,
                                FirstTest = double.Parse(sheet.Cells[row, 3].Value.ToString().Trim()),
                                SecondTest = double.Parse(sheet.Cells[row, 4].Value.ToString().Trim()),
                                ThirdTest = double.Parse(sheet.Cells[row, 5].Value.ToString().Trim()),
                                ExamScore = double.Parse(sheet.Cells[row, 6].Value.ToString().Trim()),
                                TermName = termName,
                                SessionName = sessionName,
                                StaffName = sheet.Cells[row, 9].Value.ToString().Trim().ToUpper(),
                                ClassName = className,
                                SchoolId = userSchool
                                //SubjectCategory = mysubjectCategory
                            };
                            Db.ContinuousAssessments.Add(mycontinuousAssessment);

                            recordCount++;
                            lastrecord = $"The last Updated record has the Student Id {studentId} and Subject Name is {subjectName}. Please Confirm!!!";
                        }
                    }
                }
                await Db.SaveChangesAsync();
                message = $"You have successfully Uploaded {recordCount} records...  and {lastrecord}";
                TempData["UserMessage"] = message;
                TempData["Title"] = "Success.";
                ViewBag.TermName = new SelectList(Db.Terms.AsNoTracking(), "TermName", "TermName");
                ViewBag.SessionName = new SelectList(Db.Sessions.AsNoTracking(), "SessionName", "SessionName");

                if (User.IsInRole("Teacher"))
                {
                    string name = User.Identity.GetUserName();
                    var subjectList = Db.AssignSubjectTeachers.AsNoTracking().Where(x => x.StaffName.Equals(name));
                    ViewBag.SubjectCode = new SelectList(subjectList.AsNoTracking(), "SubjectName", "SubjectName");
                    ViewBag.ClassName = new SelectList(subjectList.AsNoTracking(), "ClassName", "ClassName");
                }
                else
                {
                    ViewBag.ClassName = new SelectList(Db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
                    ViewBag.SubjectCode = new SelectList(Db.Subjects.AsNoTracking(), "CourseName", "CourseName");
                }
                return View();
            }
            else
            {
                ViewBag.Error = "File type is Incorrect <br/>";
                return View();
            }
        }
    }
}

