using HopeAcademySMS.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OfficeOpenXml;
using PagedList;
using Rotativa;
using SwiftSkoolv1.Domain;
using SwiftSkoolv1.WebUI.BusinessLogic;
using SwiftSkoolv1.WebUI.Models;
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

//using Excel = Microsoft.Office.Interop.Excel;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class StudentsController : BaseController
    {

        private readonly GradeRemark _gradeRemark;
        private ResultCommandManager _resultCommand;

        public StudentsController()
        {
            _gradeRemark = new GradeRemark();
        }

        // GET: Students
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string search, int? page, string whatever)
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
                // studentList = studentList.Where(s => s.GPhoneNumber.ToUpper().Equals(user.ToUpper()));

                // var studentId = studentList.Select(x => x.StudentId).FirstOrDefault();
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

        public async Task<ActionResult> GetIndex()
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

            //var v = Db.Subjects.Where(x => x.SchoolId != userSchool).Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToList();
            var v = Db.Students.Where(x => x.SchoolId == userSchool).Select(s => new { s.StudentId, s.FullName, s.Gender }).ToList();

            //var v = Db.Subjects.Where(x => x.SchoolId.Equals(userSchool)).Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToList();
            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            //{
            //    //v = v.OrderBy(sortColumn + " " + sortColumnDir);
            //    v = new List<Subject>(v.OrderBy(x => "sortColumn + \" \" + sortColumnDir"));
            //}
            if (!string.IsNullOrEmpty(search))
            {
                //v = v.OrderBy(sortColumn + " " + sortColumnDir);
                v = Db.Students.Where(x => x.SchoolId.Equals(userSchool) && (x.StudentId.Equals(search) || x.FullName.Equals(search)))
                    .Select(s => new { s.StudentId, s.FullName, s.Gender }).ToList();
            }
            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
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
            Student student = Db.Students.FirstOrDefault(x => x.StudentId == studentId);

            var myClass = await Db.AssignedClasses.AsNoTracking().Where(x => x.TermName.Equals(term) && x.SessionName.Equals(session)
                                                       && x.StudentId.Equals(studentId)).Select(s => s.ClassName)
                                                        .FirstOrDefaultAsync();

            var male = await Db.AssignedClasses.AsNoTracking().Where(
                                x => x.TermName.Equals(term) && x.SessionName.Equals(session)
                                     && x.ClassName.Equals(myClass)).CountAsync(c => c.Student.Gender.ToUpper().Equals("MALE"));

            var female = await Db.AssignedClasses.AsNoTracking().Where(
                x => x.TermName.Equals(term) && x.SessionName.Equals(session)
                     && x.ClassName.Equals(myClass)).CountAsync(c => c.Student.Gender.ToUpper().Equals("FEMALE"));


            //var totalSubjectOffered = await _resultCommand.SubjectOfferedByStudent(studentId, term, session);

            var totalStudent = male + female;

            model.Subjects = await _resultCommand.NameOfSubjectOfferedByStudent();

            model.ClassName = myClass;
            model.MaleStudents = male;
            model.FemaleStudents = female;
            model.TotalStudents = totalStudent;

            model.Term = term;
            model.Session = session;
            // model.Subjects = totalSubjectOffered;
            //foreach (var students in male)
            //{
            //    Db.Students.FirstOrDefault(x => x.StudentId == studentId);
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
                    student.Gender = model.Gender.ToString();
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

        #region Result Display
        public async Task<ActionResult> PrintSecondTerm(string id, string term, string sessionName)
        {

            _resultCommand = new ResultCommandManager(id, term, sessionName, userSchool);
            var reportModel = new ReportVm();
            var newCalist = new List<ContinuousAssesmentVm>();
            foreach (var ca in _resultCommand._studentCa)
            {
                var caVm = new ContinuousAssesmentVm
                {
                    SubjectName = ca.Subject.SubjectName,
                    SubjectPosition = _resultCommand.FindSubjectPosition(ca.SubjectId),
                    SubjectHighest = _resultCommand.SubjectHighest(ca.SubjectId),
                    SubjectLowest = _resultCommand.SubjectLowest(ca.SubjectId),
                    ClassAverage = await _resultCommand.CalculateClassAverage(ca.SubjectId),
                    FirstCa = ca.FirstCa,
                    SecondCa = ca.SecondCa,
                    ThirdCa = ca.ThirdCa,
                    ForthCa = ca.ForthCa,
                    FifthCa = ca.FifthCa,
                    SixthCa = ca.SixthCa,
                    SeventhCa = ca.SeventhCa,
                    EightCa = ca.EightCa,
                    NinthtCa = ca.NinthtCa,
                    ExamCa = ca.ExamCa,
                    Total = ca.Total,
                    Grading = ca.Grading,
                    Remark = ca.Remark,
                    StaffName = ca.StaffName
                };
                newCalist.Add(caVm);

            }
            reportModel.ContinuousAssesmentVms = newCalist;

            reportModel.NoOfStudentPerClass = await _resultCommand.NumberOfStudentPerClass();
            reportModel.NoOfSubjectOffered = await _resultCommand.SubjectOfferedByStudent();
            reportModel.AggregateScore = _resultCommand.TotalScorePerStudent();
            reportModel.Average = await _resultCommand.CalculateAverage();
            reportModel.OverAllGrade = _gradeRemark.Grading(reportModel.Average, _resultCommand._className, userSchool);





            var myOtherSkills = await Db.Psychomotors.AsNoTracking().Where(s => s.StudentId.Contains(id)
                                                          && s.TermName.Contains(term)
                                                          && s.SessionName.Contains(sessionName)
                                                          && s.ClassName.Equals(_resultCommand._className))
                                                         .Select(c => c.Id).FirstOrDefaultAsync();





            reportModel.BehaviorCategory = await Db.BehaviorSkillCategories.AsNoTracking()
                                                    .Where(s => s.SchoolId.Equals(userSchool))
                                                    .Select(x => x.Name).ToListAsync();
            reportModel.AssignBehaviors = await Db.AssignBehaviors.Where(s => s.SchoolId.Equals(userSchool)
                                                                    && s.StudentId.Contains(id)
                                                                     && s.TermName.Contains(term)
                                                                     && s.SessionName.Contains(sessionName)).ToListAsync();


            reportModel.AssignBehavior = reportModel.AssignBehaviors.FirstOrDefault();

            reportModel.ReportCard = await Db.ReportCards.FirstOrDefaultAsync(x => x.SchoolId.Equals(userSchool)
                                                && x.TermName.ToUpper().Equals(term)
                                                && x.SessionName.Equals(sessionName));


            //ViewBag.Class = 
            reportModel.PrincipalComment = _gradeRemark.PrincipalRemark(reportModel.Average, _resultCommand._className, userSchool);
            reportModel.TermName = term;
            reportModel.SessionName = sessionName;
            reportModel.ClassName = _resultCommand._className;
            reportModel.Student = await Db.Students.FindAsync(id);
            reportModel.CaSetUp = await Db.CaSetUps.AsNoTracking().Where(x => x.IsTrue.Equals(true)
                                                         && x.Class.FullClassName.Equals(_resultCommand._className))
                                                        .OrderBy(o => o.CaOrder).ToListAsync();
            reportModel.CaSetUpCount = reportModel.CaSetUp.Count();

            var myAggregateList = new List<AggregateList>();

            var classMate = Db.AssignedClasses.AsNoTracking().Where(x => x.ClassName.Equals(_resultCommand._className))
                                                    .Select(s => s.StudentId).ToList();
            foreach (var student in classMate)
            {
                var aggregateList = new AggregateList()
                {
                    Score = await Db.CaLists.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool) && x.StudentId.Equals(student)
                                                    && x.ClassName.Equals(_resultCommand._className)
                                                    && x.TermName.Equals(term) && x.SessionName.Equals(sessionName))
                                                .SumAsync(s => s.Total),
                    StudentId = student
                };
                myAggregateList.Add(aggregateList);
            }

            reportModel.AggregatePosition = _resultCommand.FindAggregatePosition(myAggregateList);

            //return View(reportModel);

            return new ViewAsPdf("PrintSecondTerm", reportModel);
        }


        public async Task<ActionResult> SummaryResult(string id, string sessionName)
        {
            var cModel = new CummulativeReportVm();
            var summaryCaList = new List<SummaryCa>();
            cModel.Student = await Db.Students.FindAsync(id);
            var noOfStudentInClass = 0;
            var className = string.Empty;

            var subjectOffered = await GetSubjectId(cModel.Student.CurrentClass, cModel.Student.StudentId);
            foreach (var subject in subjectOffered)
            {
                var resultSummaryCmd = new ResultSummaryCmd(id, sessionName, subject, userSchool);
                var summaryCa = new SummaryCa
                {
                    FirstTermScore = resultSummaryCmd.FirstTermScore,
                    FirstTermGrade = resultSummaryCmd.FirstTermSubjectGrade,
                    FirstTermPosition = resultSummaryCmd.FirstTermSubjectPosition,
                    SecondTermScore = resultSummaryCmd.SecondTermScore,
                    SecondTermPosition = resultSummaryCmd.FirstTermSubjectPosition,
                    SeondTermGrade = resultSummaryCmd.SecondTermSubjectGrade,
                    ThirdTermScore = resultSummaryCmd.ThirdTermScore,
                    ThirdTermGrade = resultSummaryCmd.ThirdTermSubjectGrade,
                    ThirdTermPosition = resultSummaryCmd.ThirdTermSubjectPosition,
                    SubjectGrade = resultSummaryCmd.SummaryGrading,
                    WeightedScore = resultSummaryCmd.WeightedScores,
                    SubjectRemark = resultSummaryCmd.SummaryRemark,
                    SubjectAverage = resultSummaryCmd.ClassAverage
                };
                noOfStudentInClass = resultSummaryCmd.NoOfStudentPerClass;
                className = resultSummaryCmd.ClassName;

                summaryCaList.Add(summaryCa);
            }

            cModel.SummaryCas = summaryCaList;
            cModel.NoOfSubjectOffered = subjectOffered.Count();
            cModel.NoOfStudentPerClass = noOfStudentInClass;
            cModel.SessionName = sessionName;
            cModel.ClassName = className;
            cModel.AggregateScore = cModel.SummaryCas.Sum(s => s.WeightedScore);


            return View(cModel);

        }

        #endregion
        public async Task<List<int>> GetSubjectId(string _className, string _studentId)
        {
            var subjectAssigned = await Db.AssignSubjects.AsNoTracking().Where(c => c.SchoolId.ToUpper().Trim().Equals(userSchool)
                                                        && c.ClassName.ToUpper().Trim().Equals(_className))
                                                        .Select(s => s.Subject.SubjectId).ToListAsync();
            var subjectregistration = await Db.SubjectRegistrations.AsNoTracking().Where(x => x.SchoolId.ToUpper().Trim().Equals(userSchool)
                                                        && x.StudentId.ToUpper().Trim().Equals(_studentId.ToUpper().Trim()))
                                                        .Select(s => s.Subject.SubjectId).ToListAsync();
            if (subjectregistration.Count > 1)
            {
                return subjectregistration;
            }
            return subjectAssigned;
            //var noOfSubjectPerStudent = _db.AssignSubjects.Count(x => x.ClassName.Equals(className));

        }
        //public async Task<ActionResult> PrintSecondTerm(string FileId)
        //{
        //    DownloadFiles obj = new DownloadFiles();
        //    string NewFileName = FileId + ".pdf";
        //    var filesCol = obj.GetFiles();
        //    string CurrentFileName = (from fls in filesCol
        //                              where fls.FileName == NewFileName
        //                              select fls.FilePath).First();

        //    string contentType = string.Empty;

        //    if (CurrentFileName.Contains(".pdf"))
        //    {
        //        contentType = "application/pdf";
        //    }

        //    else if (CurrentFileName.Contains(".docx"))
        //    {
        //        contentType = "application/docx";
        //    }
        //    return File(CurrentFileName, contentType, CurrentFileName);
        //}


        //public ActionResult PrintTest(string id, string term, string sessionName)
        //{


        //    var className = Db.AssignedClasses.Where(x => x.StudentId.Equals(id) && x.TermName.ToUpper().Trim().Equals(term.ToUpper().Trim())
        //                                             && x.SessionName.ToUpper().Trim().Equals(sessionName.ToUpper().Trim()))
        //                                        .Select(y => y.ClassName)
        //                                        .FirstOrDefault();
        //    string subject = "MATHEMATICS";

        //    // var className = "JSS1 A";

        //    ViewBag.Subject = _resultCommand.SubjectOfferedByStudent(id, term, sessionName);
        //    var sumPerSubject = Db.ContinuousAssessments.Where(x => x.SubjectCode.ToUpper().Trim().Equals(subject.ToUpper().Trim())
        //                                                           && x.ClassName.ToUpper().Trim().Equals(className.ToUpper().Trim())
        //                                                            && x.TermName.ToUpper().Trim().Equals(term.ToUpper().Trim())
        //                                                    && x.SessionName.ToUpper().Trim().Equals(sessionName.ToUpper().Trim()))
        //                                                    .Sum(y => y.Total);
        //    double classAverage = _resultCommand.CalculateClassAverage(className, term, sessionName, subject.ToUpper().Trim());
        //    var studentPerClass = Db.AssignedClasses.Count(x => x.ClassName.ToUpper().Trim().Equals(className.ToUpper().Trim())
        //                                                        && x.TermName.ToUpper().Trim().Equals(term.ToUpper().Trim())
        //                                                     && x.SessionName.ToUpper().Trim().Equals(sessionName.ToUpper().Trim()));

        //    double average = _resultCommand.CalculateAverage(id, className, term, sessionName);
        //    double totalScore = _resultCommand.TotalScorePerStudent(id, className, term, sessionName);
        //    // return Math.Round(sumPerSubject, 2);
        //    ViewBag.Term = term;
        //    ViewBag.Session = sessionName;
        //    ViewBag.ClassName = className;
        //    ViewBag.SubjectTotal = Math.Round(sumPerSubject, 2);

        //    ViewBag.ClassAverage = classAverage;
        //    ViewBag.StudentPerClass = studentPerClass;
        //    ViewBag.Average = average;
        //    ViewBag.TotalScore = totalScore;

        //    return View();

        //}
        //public ActionResult PrintSummaryReport(string id, string sessionName)
        //{
        //    var summary = new SummaryReportViewModel()
        //    {
        //        Results = Db.Results.Where(s => s.StudentId.Contains(id)
        //                                && s.SessionName.Contains(sessionName)).ToList(),
        //        ReportSummaries = Db.ReportSummarys.Where(s => s.StudentId.Equals(id)
        //                                            && s.SessionName.Equals(sessionName)).ToList()
        //    };
        //    //foreach (var item in studentResults.Where(c => c.))
        //    //{

        //    //}
        //    return View(summary);
        //}

        public PartialViewResult ResultInfo(string studentNumber)
        {
            var resultInfoes = Db.ContinuousAssessments.Where(s => s.StudentId.Contains(studentNumber));
            return PartialView(resultInfoes);
        }
        //public PartialViewResult ResultRemplate(string studentNumber, string term, string sessionName)
        //{
        //    var GuardianInfoes = Db.ContinuousAssessments.Include(p => p.Student).Where(s => s.StudentId.Contains(studentNumber)
        //                                            && s.TermName.Contains(term) && s.SessionName.Contains(sessionName));
        //    return PartialView(GuardianInfoes);
        //}

        //public PartialViewResult RenderRemplate(string studentNumber, string term, string sessionName, string subjectcode)
        //{
        //    var myResult = Db.Results.Where(s => s.StudentId.Contains(studentNumber)
        //                                            && s.Term.Contains(term)
        //                                            && s.SessionName.Contains(sessionName)
        //                                            && s.SubjectName.Contains(subjectcode));
        //    return PartialView(myResult);
        //}


        //public ActionResult Pdf()
        //{
        //    var pdf = Db.Students.ToList();
        //    return new PdfResult(pdf, "Pdf");
        //}

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
            else
            {
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
                            string password = workSheet.Cells[row, 13].Value.ToString().Trim();
                            string username = lastName.Trim() + " " + firstName.Trim();
                            try
                            {
                                var student = new Student()
                                {
                                    StudentId = studentId,
                                    FirstName = firstName,
                                    MiddleName = middleName,
                                    LastName = lastName,
                                    PhoneNumber = phoneNumber,
                                    Gender = gender,
                                    Religion = religion,
                                    PlaceOfBirth = placeofBirth,
                                    StateOfOrigin = state,
                                    Tribe = tribe,
                                    DateOfBirth = dateOfBirth,
                                    AdmissionDate = addmision
                                };
                                Db.Students.Add(student);

                                recordCount++;
                                lastrecord =
                                    $"The last Updated record has the Last Name {lastName} and First Name {firstName} with Phone Number {phoneNumber}";
                            }
                            catch (Exception e)
                            {
                                message = $"You have successfully Uploaded {recordCount} records...  and {lastrecord}";
                                TempData["UserMessage"] = message;
                                TempData["Title"] = "Success.";
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
                else
                {
                    ViewBag.Error = "File type is Incorrect <br/>";
                    return View("UploadStudent");
                }
            }
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
