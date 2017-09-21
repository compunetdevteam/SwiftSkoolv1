using HopeAcademySMS.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OfficeOpenXml;
using PagedList;
using SwiftSkoolv1.Domain;
using SwiftSkoolv1.WebUI.Models;
using SwiftSkoolv1.WebUI.Services;
using SwiftSkoolv1.WebUI.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

//using Excel = Microsoft.Office.Interop.Excel;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class StudentsController : BaseController
    {

        // GET: Students
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string search, int? page, string whatever, string gender)
        {
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

            //get the gender to the view
            ViewBag.Gender = gender;
            //var studentList = from s in Db.Students.AsNoTracking() select s;
            var studentList = Db.Students.AsNoTracking().Include(g => g.Guardian);
            if (Request.IsAuthenticated && !User.IsInRole("SuperAdmin"))
            {
                studentList = studentList.Where(x => x.SchoolId.Equals(userSchool));
            }
            if (User.IsInRole("Guardian"))
            {
                string name = User.Identity.GetUserName();
                var user = await Db.Users.AsNoTracking().Where(c => c.UserName.Equals(name)).Select(x => x.PhoneNumber).FirstOrDefaultAsync();

            }
            else
            {
                if (!String.IsNullOrEmpty(search))
                {
                    studentList = studentList.AsNoTracking().Where(s => s.LastName.ToUpper().Contains(search.ToUpper())
                                                         || s.FirstName.ToUpper().Contains(search.ToUpper())
                                                         || s.StudentId.ToUpper().Contains(search.ToUpper()));

                }
            }

            switch (sortOrder)
            {
                case "name_desc":
                    studentList = studentList.AsNoTracking().OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    studentList = studentList.AsNoTracking().OrderBy(s => s.FirstName);
                    break;
                default:
                    studentList = studentList.AsNoTracking().OrderBy(s => s.LastName);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.Message = whatever;
            return View(studentList.ToPagedList(pageNumber, pageSize));
            //return View(studentList.ToList());
        }




        public async Task<ActionResult> GetIndex(string gender)
        {
            #region Server Side filtering
            //Get parameter for sorting from grid table
            // get Start (paging start index) and length (page size for paging)
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns values when we click on Header Name of column
            //getting column name
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            //Soring direction(either desending or ascending)
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            string search = Request.Form.GetValues("search[value]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;


            /*
             * get the student base on the studen type eithr
             * male or female
             */



            //var v = Db.Subjects.Where(x => x.SchoolId != userSchool).Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToList();
            var v = Db.Students.Where(x => x.SchoolId == userSchool)
                .Select(s => new { s.StudentId, s.FullName, s.Gender }).ToList();

            //var v = Db.Subjects.Where(x => x.SchoolId.Equals(userSchool)).Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToList();
            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            //{
            //    //v = v.OrderBy(sortColumn + " " + sortColumnDir);
            //    v = new List<Subject>(v.OrderBy(x => "sortColumn + \" \" + sortColumnDir"));
            //}

            if (!string.IsNullOrWhiteSpace(gender))
            {
                v = Db.Students.Where(x => x.SchoolId == userSchool && x.Gender == gender)
                    .Select(s => new { s.StudentId, s.FullName, s.Gender }).ToList();
            }
            if (!string.IsNullOrEmpty(search))
            {
                //v = v.OrderBy(sortColumn + " " + sortColumnDir);
                v = Db.Students.Where(x => x.SchoolId.Equals(userSchool) &&
                                           (x.StudentId.Equals(search) || x.FullName.Equals(search)))
                    .Select(s => new { s.StudentId, s.FullName, s.Gender }).ToList();
            }

            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data },
                JsonRequestBehavior.AllowGet);

            #endregion

            //return Json(new { data = await Db.Subjects.AsNoTracking().Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToListAsync() }, JsonRequestBehavior.AllowGet);
        }




        public PartialViewResult ExcelUpload()
        {
            return PartialView();
        }
        public async Task<ActionResult> Dashboard()
        {
            StudentDashboardVm model = new StudentDashboardVm();
            var term = await Db.Terms.Where(x => x.ActiveTerm.Equals(true)).Select(x => x.TermName).FirstOrDefaultAsync();
            var session = await Db.Sessions.Where(x => x.ActiveSession.Equals(true)).Select(x => x.SessionName).FirstOrDefaultAsync();
            //get the ID of the logged in Student
            var studentId = User.Identity.GetUserId();

            //fetch the student info frm Db
            Student student = Db.Students.AsNoTracking().FirstOrDefault(x => x.StudentId == studentId);

            var myClass = await Db.AssignedClasses.AsNoTracking().Where(x => x.TermName.Equals(term) && x.SessionName.Equals(session)
                                                       && x.StudentId.Equals(studentId)).Select(s => s.ClassName)
                                                        .FirstOrDefaultAsync();

            var male = await Db.AssignedClasses.AsNoTracking().Where(x => x.TermName.Equals(term)
                                    && x.SessionName.Equals(session)
                                     && x.ClassName.Equals(myClass))
                                     .CountAsync(c => c.Student.Gender.ToUpper().Equals("MALE"));

            var female = await Db.AssignedClasses.AsNoTracking().Where(
                x => x.TermName.Equals(term) && x.SessionName.Equals(session)
                     && x.ClassName.Equals(myClass)).CountAsync(c => c.Student.Gender.ToUpper().Equals("FEMALE"));


            //var totalSubjectOffered = await _resultCommand.SubjectOfferedByStudent(studentId, term, session);

            var totalStudent = male + female;

            //   model.Subjects = await _resultCommand.NameOfSubjectOfferedByStudent();

            model.ClassName = myClass;
            model.MaleStudents = male;
            model.FemaleStudents = female;
            model.TotalStudents = totalStudent;
            model.Term = term;
            model.Session = session;

            //var assignedSubjects = await Db.AssignSubjects.Include(i => i.Subject).AsNoTracking()
            //                        .Where(x => x.ClassName.Equals(myClass)
            //                            && x.TermName.Equals(term)
            //                            && x.SchoolId.Equals(userSchool))
            //                            .Select(s => s.SubjectId).ToListAsync();
            //foreach (var subject in assignedSubjects)
            //{
            //    var subjectName = await Db.Subjects.AsNoTracking().Where(x => x.SubjectId.Equals(subject))
            //                        .Select(x => x.SubjectName).FirstOrDefaultAsync();
            //    model.Subjects.Add(subjectName);
            //}
            if (String.IsNullOrEmpty(myClass))
            {

            }
            var classAssinged = Db.AssignedClasses.FirstOrDefault(x => x.StudentId == studentId);

            ViewBag.NotAssinged = "Class has not been assigned to you yet";
            ViewBag.Class = student;





            // ViewBag.studentClass = student.AssignedClasses;
            return View(model);
        }


        //Get the Specific student result
        public async Task<ActionResult> GetStudentResult()
        {
            var student = User.Identity.GetUserId();
            var result = await Db.Results.AsNoTracking().Where(x => x.StudentId.Equals(student)).ToListAsync();

            return View(result);
        }
        // GET: Students/Details/5
        public async Task<ActionResult> Details(string id)
        {

            var username = User.Identity.GetUserId();
            //var user = await Db.Users.AsNoTracking().Where(c => c.Id.Equals(username)).Select(c => c.Email).FirstOrDefaultAsync();
            if (id == null)
            {
                id = username;
            }

            Student student = await Db.Students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }
        public async Task<PartialViewResult> PartialDetails(string id)
        {
            var username = User.Identity.GetUserId();
            //var user = await Db.Users.AsNoTracking().Where(c => c.Id.Equals(username)).Select(c => c.Email).FirstOrDefaultAsync();
            if (id == null)
            {
                id = username;
            }

            var student = await Db.Students.FindAsync(id);

            return PartialView(student);
        }

        public async Task<ActionResult> AssignClass()
        {
            ViewBag.ClassName = new SelectList(Db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            ViewBag.StudentId = new MultiSelectList(await _query.StudentListAsync(userSchool), "StudentId", "FullName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> AssignClass(AssignClassToStudentVm model)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in model.StudentId)
                {
                    var student = await Db.Students.FindAsync(item);

                    if (student != null)
                    {
                        student.CurrentClass = model.ClassName;
                        Db.Entry(student).State = EntityState.Modified;
                        await Db.SaveChangesAsync();
                    }
                }
            }
            return View();
        }

        // GET: Students/Create
        public async Task<ActionResult> Create()
        {
            //var role = await Db.Roles.AsNoTracking().SingleOrDefaultAsync(m => m.Name == "Guardian");
            //var usersInRole = Db.Users.AsNoTracking().Where(m => m.Roles.Any(r => r.RoleId == role.Id));
            //ViewBag.GuardianId = new SelectList(usersInRole, "Email", "UserName");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(StudentViewModel model)
        {
            string myemail = System.Configuration.ConfigurationManager.AppSettings["SchoolName"].ToString();
            string email = model.StudentId + "@" + myemail + ".com";
            var store = new UserStore<ApplicationUser>(Db);
            var manager = new UserManager<ApplicationUser>(store);
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { Id = model.StudentId, UserName = model.UserName, Email = email, PhoneNumber = model.PhoneNumber };

                manager.Create(user, model.Password);

                var student = new Student()
                {
                    StudentId = model.StudentId,
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Gender = model.Gender.ToString(),
                    Religion = model.Religion.ToString(),
                    DateOfBirth = model.DateOfBirth,
                    PlaceOfBirth = model.PlaceOfBirth,
                    StateOfOrigin = model.StateOfOrigin.ToString(),
                    Tribe = model.Tribe,
                    AdmissionDate = model.AdmissionDate,
                    StudentPassport = model.StudentPassport,
                    IsGraduated = false,
                    SchoolId = userSchool
                };
                Db.Students.Add(student);

                await Db.SaveChangesAsync();
                await manager.AddToRoleAsync(user.Id, "Student");


                TempData["UserMessage"] = "Student has been Added Successfully";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            };
            var role = await Db.Roles.AsNoTracking().SingleOrDefaultAsync(m => m.Name == "Guardian");
            var usersInRole = Db.Users.AsNoTracking().Where(m => m.Roles.Any(r => r.RoleId == role.Id));
            ViewBag.GuardianId = new SelectList(usersInRole, "Email", "UserName");
            return View(model);
        }

        // GET: Students/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                id = User.Identity.GetUserId();
            }
            Student student = await Db.Students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            var myStudent = new StudentEditViewModel
            {
                StudentId = student.StudentId,
                FirstName = student.FirstName,
                MiddleName = student.MiddleName,
                LastName = student.LastName,
                DateOfBirth = student.DateOfBirth,
                AdmissionDate = student.AdmissionDate,
                StudentPassport = student.StudentPassport,
                PlaceOfBirth = student.PlaceOfBirth,
                PhoneNumber = student.PhoneNumber,
                Tribe = student.Tribe
            };


            return View(myStudent);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(StudentEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var student = new Student(model.StudentId, model.GuardianId, model.FirstName, model.MiddleName, model.LastName,
                //                            model.Gender.ToString(), model.DateOfBirth, model.AdmissionDate,
                //
                Student student = await Db.Students.FindAsync(model.StudentId);

                if (student != null)
                {
                    student.StudentId = model.StudentId;
                    student.FirstName = model.FirstName;
                    student.MiddleName = model.MiddleName;
                    student.LastName = model.LastName;
                    student.PhoneNumber = model.PhoneNumber;
                    student.Gender = model.Gender.ToString().ToUpper();
                    student.Religion = model.Religion.ToString();
                    student.DateOfBirth = model.DateOfBirth;
                    student.PlaceOfBirth = model.PlaceOfBirth;
                    student.StateOfOrigin = model.StateOfOrigin.ToString();
                    student.Tribe = model.Tribe;
                    student.AdmissionDate = model.AdmissionDate;
                    student.StudentPassport = model.StudentPassport;
                    student.IsGraduated = false;

                    Db.Entry(student).State = EntityState.Modified;
                }
                await Db.SaveChangesAsync();
                TempData["UserMessage"] = "Student has been Updated Successfully";
                TempData["Title"] = "Success.";

                return RedirectToAction("Index");
            }

            var role = await Db.Roles.AsNoTracking().SingleOrDefaultAsync(m => m.Name == "Guardian");
            var usersInRole = Db.Users.AsNoTracking().Where(m => m.Roles.Any(r => r.RoleId == role.Id));
            ViewBag.GuardianId = new SelectList(usersInRole, "PhoneNumber", "UserName");
            return View(model);
        }

        // GET: Students/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await Db.Students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Student student = await Db.Students.FindAsync(id);
            if (student != null) Db.Students.Remove(student);
            TempData["UserMessage"] = "Student has been Deleted Successfully.";
            TempData["Title"] = "Deleted.";
            await Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<ActionResult> RenderImage(string studentId)
        {
            Student student = await Db.Students.FindAsync(studentId);

            byte[] photoBack = student.StudentPassport;

            return File(photoBack, "image/png");
        }

        public async Task<ActionResult> RenderSignature(string studentId)
        {
            ReportCard student = await Db.ReportCards.FindAsync(studentId);

            byte[] photoBack = student.PrincipalSignature;

            return File(photoBack, "image/png");
        }

        //public PartialViewResult GuardianInfo(string studentNumber)
        //{
        //    var GuardianInfoes = Db.Guardians.Include(p => p.Student).Where(s => s.GuardianEmail.Contains(studentNumber));
        //    return PartialView(GuardianInfoes);
        //}




        public PartialViewResult ResultInfo(string studentNumber)
        {
            var resultInfoes = Db.ContinuousAssessments.Where(s => s.StudentId.Contains(studentNumber));
            return PartialView(resultInfoes);
        }


        public ActionResult Calender()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public PartialViewResult NewCalender()
        {
            return PartialView();
        }
        public string Init()
        {
            bool rslt = Utils.InitialiseDiary();
            return rslt.ToString();
        }

        public void UpdateEvent(int id, string NewEventStart, string NewEventEnd)
        {
            DiaryEvent.UpdateDiaryEvent(id, NewEventStart, NewEventEnd);
        }


        public bool SaveEvent(string Title, string NewEventDate, string NewEventTime, string NewEventDuration)
        {
            return DiaryEvent.CreateNewEvent(Title, NewEventDate, NewEventTime, NewEventDuration);
        }

        public JsonResult GetDiarySummary(double start, double end)
        {
            var ApptListForDate = DiaryEvent.LoadAppointmentSummaryInDateRange(start, end);
            var eventList = from e in ApptListForDate
                            select new
                            {
                                id = e.ID,
                                title = e.Title,
                                start = e.StartDateString,
                                end = e.EndDateString,
                                someKey = e.SomeImportantKeyID,
                                allDay = false
                            };
            var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetDiaryEvents(double start, double end)
        //{
        //    var ApptListForDate = DiaryEvent.LoadAllAppointmentsInDateRange(start, end);
        //    var eventList = from e in ApptListForDate
        //                    select new
        //                    {
        //                        id = e.ID,
        //                        title = e.Title,
        //                        start = e.StartDateString,
        //                        end = e.EndDateString,
        //                        color = e.StatusColor,
        //                        className = e.ClassName,
        //                        someKey = e.SomeImportantKeyID,
        //                        allDay = false
        //                    };
        //    var rows = eventList.ToArray();
        //    return Json(rows, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult UpLoadStudent()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> UpLoadStudent(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please Select a excel file <br/>";
                return View("UpLoadStudent");
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
                    int requiredField = 13;

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
                        string firstName = workSheet.Cells[row, 2].Value.ToString().Trim();
                        string middleName = workSheet.Cells[row, 3].Value.ToString().Trim();
                        string lastName = workSheet.Cells[row, 4].Value.ToString().Trim();
                        string gender = workSheet.Cells[row, 5].Value.ToString().Trim();
                        DateTime dateOfBirth = DateTime.Parse(workSheet.Cells[row, 6].Value.ToString().Trim());
                        string placeofBirth = workSheet.Cells[row, 7].Value.ToString().Trim();
                        string state = workSheet.Cells[row, 8].Value.ToString().Trim();
                        string religion = workSheet.Cells[row, 9].Value.ToString().Trim();
                        string tribe = workSheet.Cells[row, 10].Value.ToString().Trim();
                        DateTime addmision = DateTime.Parse(workSheet.Cells[row, 11].Value.ToString().Trim());
                        string phoneNumber = workSheet.Cells[row, 12].Value.ToString().Trim();
                        try
                        {
                            var student = new Student()
                            {
                                StudentId = studentId,
                                FirstName = firstName,
                                MiddleName = middleName,
                                LastName = lastName,
                                PhoneNumber = phoneNumber,
                                Gender = gender.ToUpper(),
                                Religion = religion,
                                PlaceOfBirth = placeofBirth,
                                StateOfOrigin = state,
                                Tribe = tribe,
                                DateOfBirth = dateOfBirth,
                                AdmissionDate = addmision,
                                SchoolId = userSchool,
                            };
                            Db.Students.Add(student);

                            recordCount++;
                            lastrecord =
                                $"The last Updated record has the Last Name {lastName} and First Name {firstName} with Phone Number {phoneNumber}";


                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = ex.Message;
                            //message = $"You have successfully Uploaded {recordCount} records...  and {lastrecord}";
                            //TempData["UserMessage"] = message + e.Message;
                            //TempData["Title"] = "Success.";
                            return View("Error3");
                        }


                    }
                    await Db.SaveChangesAsync();
                    message = $"You have successfully Uploaded {recordCount} records...  and {lastrecord}";
                    TempData["UserMessage"] = message;
                    TempData["Title"] = "Success.";
                    return RedirectToAction("Index", "Students");
                }
            }
            ViewBag.Error = "File type is Incorrect <br/>";
            return View("UploadStudent");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //_resultCommand.Dispose();
                Db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
