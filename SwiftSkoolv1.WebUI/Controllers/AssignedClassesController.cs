using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using SwiftSkoolv1.Domain;
using SwiftSkoolv1.WebUI.Services;
using SwiftSkoolv1.WebUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class AssignedClassesController : BaseController
    {
        public async Task<ActionResult> Index()
        {
            if (User.IsInRole("Teacher"))
            {
                string name = User.Identity.GetUserName();

                var classes = new List<Class>();

                var classList = Db.AssignSubjectTeachers.Include(i => i.Subject).AsNoTracking()
                    .Where(x => x.StaffName.Equals(name)).Select(x => x.ClassName).Distinct().ToList();
                foreach (var item in classList)
                {
                    classes.Add(Db.Classes.AsNoTracking().FirstOrDefault(x => x.FullClassName.Equals(item)));
                }

                ViewBag.ClassName = new SelectList(classes, "FullClassName", "FullClassName");
            }
            else
            {
                ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            }
            ViewBag.TermName = new SelectList(Db.Terms.AsNoTracking(), "TermName", "TermName");
            return View();
        }

        public ActionResult GetIndex(string ClassName, string TermName)
        {
            #region Server Side filtering

            var v = Db.AssignedClasses.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool))
                .Select(s => new { s.AssignedClassId, s.ClassName, s.StudentName, s.StudentId, s.TermName, s.SessionName }).ToList();

            if (!String.IsNullOrEmpty(TermName) || !String.IsNullOrEmpty(ClassName))
            {
                v = v.Where(s => s.TermName.Equals(TermName)
                                && s.ClassName.ToUpper().Equals(ClassName.ToUpper())).ToList();
            }

            return Json(new { data = v }, JsonRequestBehavior.AllowGet);

            #endregion Server Side filtering

            //return Json(new { data = await Db.Subjects.AsNoTracking().Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToListAsync() }, JsonRequestBehavior.AllowGet);
        }

        public async Task<PartialViewResult> Save(int id)
        {
            // var assignedClass = await Db.AssignedClasses.FindAsync(id);
            ViewBag.StudentId = new MultiSelectList(await _query.StudentListAsync(userSchool), "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(Db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            ViewBag.TermName = new SelectList(Db.Terms.AsNoTracking(), "TermName", "TermName");
            //var assignClassVm = new AssignedClassesViewModel
            //{
            //    AssignedClassId = assignedClass.AssignedClassId,

            //};
            return PartialView();
        }

        // POST: Subjects/Save/5 To protect from overposting attacks, please enable the specific
        // properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(AssignedClassesViewModel model)
        {
            var status = false;
            var message = string.Empty;
            if (ModelState.IsValid)
            {
                if (model.AssignedClassId > 0)
                {
                    var studentName = await Db.Students.AsNoTracking().Where(x => x.StudentId.Equals(model.StudentId))
                        .Select(s => s.FullName)
                        .FirstOrDefaultAsync();
                    var assigClass = new AssignedClass()
                    {
                        AssignedClassId = model.AssignedClassId,
                        StudentName = studentName,
                        StudentId = model.StudentId.ToString(),
                        ClassName = model.ClassName,
                        TermName = model.TermName.ToString(),
                        SessionName = model.SessionName,
                        SchoolId = userSchool
                    };
                    Db.Entry(assigClass).State = EntityState.Modified;
                    await Db.SaveChangesAsync();
                    message = "Student Class Updated Successfully.";
                }
                else
                {
                    if (model.StudentId != null)
                    {
                        int counter = 0;
                        string theClass = "";
                        foreach (var item in model.StudentId)
                        {
                            var countFromDb = await Db.AssignedClasses.AsNoTracking().FirstOrDefaultAsync(
                                                x => x.SchoolId.Equals(userSchool)
                                                && x.TermName.Equals(model.TermName.ToString())
                                                && x.SessionName.Equals(model.SessionName)
                                                && x.StudentId.Equals(item));

                            if (countFromDb != null)
                            {
                                var studentFound = await Db.Students.FindAsync(item);
                                message = $"You have already Assigned ( {countFromDb.ClassName} ) Class to this student ( {studentFound?.FullName} )";
                                return new JsonResult { Data = new { status = false, message = message } };
                            }
                            var student = await Db.Students.AsNoTracking().Where(x => x.StudentId.Equals(item))
                                                .FirstOrDefaultAsync();
                            var assigClass = new AssignedClass
                            {
                                StudentId = item,
                                ClassName = model.ClassName,
                                TermName = model.TermName,
                                SessionName = model.SessionName,
                                StudentName = student.FirstName,
                                SchoolId = userSchool
                            };
                            Db.AssignedClasses.Add(assigClass);
                            counter += 1;
                            theClass = model.ClassName;
                        }
                        message = $"You have Assigned to {counter} Student(s) to {theClass} Successfully.";
                        await Db.SaveChangesAsync();
                        return new JsonResult { Data = new { status = true, message = message } };
                    }
                }
            }
            return new JsonResult { Data = new { status = status, message = message } };
            //return View(subject);
        }

        public async Task<ActionResult> MyClassMate()
        {
            var term = await Db.Terms.Where(x => x.ActiveTerm.Equals(true)).Select(x => x.TermName).FirstOrDefaultAsync();
            var session = await Db.Sessions.Where(x => x.ActiveSession.Equals(true)).Select(x => x.SessionName).FirstOrDefaultAsync();
            var studentId = User.Identity.GetUserId();
            var myClass = await Db.AssignedClasses.AsNoTracking().Where(x => x.TermName.Equals(term) && x.SessionName.Equals(session)
                                    && x.StudentId.Equals(studentId) && x.SchoolId.Equals(userSchool))
                                    .Select(s => s.ClassName).FirstOrDefaultAsync();
            var myClassmate = await Db.AssignedClasses.AsNoTracking().Where(x => x.TermName.Equals(term)
                                                    && x.SessionName.Equals(session) && x.SchoolId.Equals(userSchool)
                                                    && x.ClassName.Equals(myClass))
                                                    .Select(s => s.Student).ToListAsync();

            return View(myClassmate);
        }

        // GET: AssignedClasses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignedClass assignedClass = await Db.AssignedClasses.FindAsync(id);
            if (assignedClass == null)
            {
                return HttpNotFound();
            }
            return View(assignedClass);
        }

        // GET: AssignedClasses/Create
        public async Task<ActionResult> Create()
        {
            if (User.IsInRole("Teacher"))
            {
                string name = User.Identity.GetUserName();
                var subjectList = Db.AssignSubjectTeachers.AsNoTracking().Where(x => x.StaffName.Equals(name));
                ViewBag.ClassName = new SelectList(subjectList.AsNoTracking(), "ClassName", "ClassName");
            }
            else
            {
                ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            }

            ViewBag.StudentId = new MultiSelectList(await _query.StudentListAsync(userSchool), "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(_query.SessionList(), "SessionName", "SessionName");
            // ViewBag.ClassName = new SelectList(Db.Classes, "FullClassName", "FullClassName");
            ViewBag.TermName = new SelectList(_query.TermList(), "TermName", "TermName");
            return View();
        }

        // POST: AssignedClasses/Create To protect from overposting attacks, please enable the
        // specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AssignedClassesViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.StudentId != null)
                {
                    int counter = 0;
                    string theClass = "";
                    foreach (var item in model.StudentId)
                    {
                        var countFromDb = await Db.AssignedClasses.AsNoTracking().CountAsync(x => x.SchoolId.Equals(userSchool)
                                                            && x.TermName.Equals(model.TermName.ToString())
                                                              && x.SessionName.Equals(model.SessionName)
                                                              && x.StudentId.Equals(item));

                        if (countFromDb >= 1)
                        {
                            TempData["UserMessage"] = "You have already Assigned Class these student";
                            TempData["Title"] = "Error.";
                            ViewBag.StudentId = new MultiSelectList(Db.Students.AsNoTracking(), "StudentID", "FullName");
                            ViewBag.SessionName = new SelectList(Db.Sessions.AsNoTracking(), "SessionName", "SessionName");
                            ViewBag.ClassName = new SelectList(Db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
                            ViewBag.TermName = new SelectList(Db.Terms.AsNoTracking(), "TermName", "TermName");
                            return View(model);
                        }
                        var studentName = await Db.Students.AsNoTracking().Where(x => x.StudentId.Equals(item))
                                            .Select(s => s.FullName).FirstOrDefaultAsync();
                        var assigClass = new AssignedClass()
                        {
                            StudentId = item,
                            ClassName = model.ClassName,
                            TermName = model.TermName,
                            SessionName = model.SessionName,
                            StudentName = studentName,
                            SchoolId = userSchool
                        };
                        Db.AssignedClasses.Add(assigClass);
                        counter += 1;
                        theClass = model.ClassName;
                    }
                    await Db.SaveChangesAsync();
                    TempData["UserMessage"] = $"You have Assigned to {counter} Student(s) to {theClass} Successfully.";
                    TempData["Title"] = "Success.";
                    return RedirectToAction("Index", "AssignedClasses");
                }
                return RedirectToAction("Index");
            }

            ViewBag.StudentId = new MultiSelectList(Db.Students.AsNoTracking(), "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(Db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(Db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            ViewBag.TermName = new SelectList(Db.Terms.AsNoTracking(), "TermName", "TermName");
            return View(model);
        }

        // GET: AssignedClasses/Edit/5
        public async Task<PartialViewResult> Edit(int? id)
        {
            if (id == null)
            {
                return PartialView();
            }
            AssignedClass assignedClass = await Db.AssignedClasses.FindAsync(id);

            var myModel = new AssignedClassesViewModel();
            myModel.AssignedClassId = assignedClass.AssignedClassId;
            ViewBag.StudentId = new MultiSelectList(Db.Students.AsNoTracking(), "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(Db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(Db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            return PartialView(myModel);
        }

        // POST: AssignedClasses/Edit/5 To protect from overposting attacks, please enable the
        // specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AssignedClassesViewModel assignedClass)
        {
            if (ModelState.IsValid)
            {
                var studentName = await Db.Students.AsNoTracking().Where(x => x.StudentId.Equals(assignedClass.StudentId))
                                   .Select(s => s.FullName)
                                   .FirstOrDefaultAsync();
                var assigClass = new AssignedClass()
                {
                    AssignedClassId = assignedClass.AssignedClassId,
                    StudentId = assignedClass.StudentId.ToString(),
                    ClassName = assignedClass.ClassName,
                    TermName = assignedClass.TermName.ToString(),
                    SessionName = assignedClass.SessionName,
                    SchoolId = userSchool
                };
                Db.Entry(assigClass).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                TempData["UserMessage"] = "Student Class Updated Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            ViewBag.StudentId = new MultiSelectList(Db.Students.AsNoTracking(), "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(Db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(Db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            ViewBag.TermName = new SelectList(Db.Terms.AsNoTracking(), "TermName", "TermName");
            return View(assignedClass);
        }

        // GET: AssignedClasses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignedClass assignedClass = await Db.AssignedClasses.FindAsync(id);
            if (assignedClass == null)
            {
                return HttpNotFound();
            }
            return View(assignedClass);
        }

        // POST: AssignedClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AssignedClass assignedClass = await Db.AssignedClasses.FindAsync(id);
            if (assignedClass != null) Db.AssignedClasses.Remove(assignedClass);
            await Db.SaveChangesAsync();
            TempData["UserMessage"] = "You have removed Student from Class";
            TempData["Title"] = "Deleted.";
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public PartialViewResult AssignClassUpload()
        {
            return PartialView();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> AssignClassUpload(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please Select a excel file <br/>";
                return View("AssignClassUpload");
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
                        ViewBag.Message = lineError;
                        RedirectToAction("Index", "AssignedClasses");
                    }

                    for (int row = 2; row <= noOfRow; row++)
                    {
                        try
                        {
                            string studentId = workSheet.Cells[row, 1].Value.ToString().Trim();
                            string classname = workSheet.Cells[row, 2].Value.ToString().Trim();
                            string termName = workSheet.Cells[row, 3].Value.ToString().Trim();
                            string sessionName = workSheet.Cells[row, 4].Value.ToString().Trim();

                            var countFromDb = await Db.AssignedClasses.AsNoTracking().CountAsync(x => x.SchoolId.Equals(userSchool)
                                                                                                      && x.TermName.Equals(termName.ToString())
                                                                                                      && x.SessionName.Equals(sessionName)
                                                                                                      && x.StudentId.Equals(studentId));

                            if (countFromDb >= 1)
                            {
                                TempData["UserMessage"] = "";
                                TempData["Title"] = "Error.";

                                ViewBag.ErrorMessage = $"You have already Assigned Class to the student in row {row} of the excel";
                                return View("Error3");
                            }

                            var assigClass = new AssignedClass()
                            {
                                StudentId = studentId,
                                ClassName = classname,
                                TermName = termName,
                                SessionName = sessionName,
                                SchoolId = userSchool
                            };
                            Db.AssignedClasses.Add(assigClass);
                        }
                        catch (Exception e)
                        {
                            ViewBag.ErrorMessage = $"Error Saving Record for row {row}{e.Message}";
                            return View("Error3");
                        }
                    }
                    try
                    {
                        await Db.SaveChangesAsync();
                        message = $"You have successfully Uploaded {recordCount} records...  and {lastrecord}";
                        ViewBag.Message = message;
                        return RedirectToAction("Index", "AssignedClasses");
                    }
                    catch (Exception e)
                    {
                        ViewBag.ErrorMessage = $"Student Id in record doesn't exist. {e.Message}";
                        return View("Error3");
                    }
                }
            }
            ViewBag.Error = "File type is Incorrect <br/>";
            return View("AssignClassUpload");
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