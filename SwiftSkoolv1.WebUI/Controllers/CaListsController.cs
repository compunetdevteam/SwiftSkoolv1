using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using SwiftSkoolv1.Domain;
using SwiftSkoolv1.WebUI.Models;
using SwiftSkoolv1.WebUI.Services;
using SwiftSkoolv1.WebUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class CaListsController : BaseController
    {
        private readonly GradeRemark _myGradeRemark = new GradeRemark();

        public async Task<ActionResult> CreateCaView(string message)
        {
            ViewBag.TermName = new SelectList(Db.Terms.AsNoTracking(), "TermName", "TermName");
            ViewBag.SessionName = new SelectList(Db.Sessions.AsNoTracking(), "SessionName", "SessionName");

            if (User.IsInRole("Teacher"))
            {
                string name = User.Identity.GetUserName();
                var subjectList = Db.AssignSubjectTeachers.Include(i => i.Subject).AsNoTracking()
                    .Where(x => x.StaffName.Equals(name)).Select(x => x.Subject).Distinct().ToList();

                var subject = new List<Subject>();
                var classes = new List<Class>();

                var classList = Db.AssignSubjectTeachers.Include(i => i.Subject).AsNoTracking()
                    .Where(x => x.StaffName.Equals(name)).Select(x => x.ClassName).Distinct().ToList();
                foreach (var item in classList)
                {
                    classes.Add(Db.Classes.AsNoTracking().FirstOrDefault(x => x.FullClassName.Equals(item)));
                }

                subject.AddRange(subjectList);
                ViewBag.SubjectId = new SelectList(subject, "SubjectId", "SubjectName");
                ViewBag.ClassName = new SelectList(classes, "FullClassName", "FullClassName");
            }
            else
            {
                ViewBag.SubjectId = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
                ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            }
            ViewBag.Message = message;
            ViewBag.SetUpCount = 0;
            //return View(myCalist);
            return View();
        }

        // GET: CaLists
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Index")]
        public async Task<ActionResult> Index(CaSelectIndexVm model)
        {
            var studentClassName = await Db.Classes.AsNoTracking().Where(x => x.FullClassName.Equals(model.ClassName))
                                        .Select(s => s.ClassName).FirstOrDefaultAsync();
            var calist = await Db.CaLists.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool) &&
                                                    x.ClassName.Equals(model.ClassName)
                                                    && x.TermName.Equals(model.TermName)
                                                    && x.SubjectId.Equals(model.SubjectId)
                                                    && x.SessionName.Equals(model.SessionName))
                                                    .ToListAsync();
            var myCalist = new CaListIndexVm()
            {
                CaList = calist,
                CaSetUp = await Db.CaSetUps.Where(x => x.IsTrue.Equals(true)
                                        && x.SchoolId.Equals(userSchool)
                                        && x.ClassName.Equals(studentClassName)
                                        && x.TermName.Equals(model.TermName))
                                        .OrderBy(o => o.CaOrder).ToListAsync()

            };
            ViewBag.SetUpCount = await Db.CaSetUps.CountAsync(x => x.IsTrue.Equals(true)
                                                        && x.SchoolId.Equals(userSchool)
                                                        && x.TermName.Equals(model.TermName)
                                                        && x.ClassName.Equals(studentClassName));
            return View(myCalist);
        }

        // GET: CaLists/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var caList = await Db.CaLists.FindAsync(id);
            if (caList == null)
            {
                return HttpNotFound();
            }
            return View(caList);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "CreateCaView")]
        public async Task<ActionResult> CreateCaView(CaSelectIndexVm model)
        {
            var myCalist = await GenerateCaList(model);
            //return RedirectToAction("SelectIndex", myCalist.ToList());
            if (myCalist == null)
            {
                return RedirectToAction("CreateCaView", new { message = "Subject Not Assigned to Class" });
            }

            ViewBag.TermName = new SelectList(Db.Terms.AsNoTracking(), "TermName", "TermName");
            ViewBag.SessionName = new SelectList(Db.Sessions.AsNoTracking(), "SessionName", "SessionName");

            if (User.IsInRole("Teacher"))
            {
                string name = User.Identity.GetUserName();
                var subjectList = Db.AssignSubjectTeachers.AsNoTracking().Where(x => x.StaffName.Equals(name));
                ViewBag.SubjectId = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
                ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            }
            else
            {
                ViewBag.SubjectId = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
                ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            }
            ViewBag.SetUpCount = myCalist.Select(s => s.CaSetUpCount).FirstOrDefault();
            return View(myCalist.ToList());
        }

        private async Task<List<CaListVm>> GenerateCaList(CaSelectIndexVm model)
        {
            var students = await Db.AssignedClasses.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool)
                                           && x.ClassName.Equals(model.ClassName)
                                           && x.SessionName.Equals(model.SessionName)
                                           && x.TermName.Equals(model.TermName)).ToListAsync();

            var studentClassName = Db.Classes.AsNoTracking().Where(x => x.FullClassName.Equals(model.ClassName))
                                                .Select(s => s.ClassName).FirstOrDefault();
            var textStudent = students.Select(s => s.StudentId).FirstOrDefault();
            var subjectList = _query.GetStudentSubject(textStudent, userSchool, model.ClassName, model.TermName);

            var subject = subjectList.FirstOrDefault(x => x.SubjectId.Equals(model.SubjectId));
            if (subject != null)
            {
                var calist = Db.CaLists.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool) &&
                                                                  x.ClassName.Equals(model.ClassName)
                                                                  && x.TermName.Equals(model.TermName)
                                                                  && x.SubjectId.Equals(model.SubjectId)
                                                                  && x.SessionName.Equals(model.SessionName))
                    .ToList();
                var myCalist = new List<CaListVm>();
                if (calist.Any())
                {
                    foreach (var list in calist)
                    {
                        var ca = new CaListVm()
                        {
                            CaListId = list.CaListId,
                            StudentName = list.StudentName,
                            StudentId = list.StudentId,
                            SubjectId = model.SubjectId,
                            SubjectName = subject.SubjectName,
                            ClassName = list.ClassName,
                            TermName = list.TermName,
                            SessionName = list.SessionName,
                            CaSetUp = await Db.CaSetUps.Where(x => x.IsTrue.Equals(true)
                                                                   && x.SchoolId.Equals(userSchool)
                                                                   && x.ClassName.Equals(studentClassName)
                                                                   && x.TermName.Equals(model.TermName))
                                .OrderBy(o => o.CaOrder).ToListAsync(),

                            CaSetUpCount = await Db.CaSetUps.CountAsync(x => x.IsTrue.Equals(true)
                                                                             && x.SchoolId.Equals(userSchool)
                                                                             && x.TermName.Equals(model.TermName)
                                                                             && x.ClassName.Equals(studentClassName)),
                            FirstCa = list.FirstCa,
                            SecondCa = list.SecondCa,
                            ThirdCa = list.ThirdCa,
                            ForthCa = list.ForthCa,
                            FifthCa = list.FifthCa,
                            SixthCa = list.SixthCa,
                            SeventhCa = list.SeventhCa,
                            EightCa = list.EightCa,
                            NinthtCa = list.NinthtCa,
                            ExamCa = list.ExamCa
                        };
                        myCalist.Add(ca);
                    }
                }
                else
                {
                    foreach (var student in students)
                    {
                        var ca = new CaListVm()
                        {
                            StudentName = student.Student.FullName,
                            StudentId = student.Student.StudentId,
                            SubjectId = model.SubjectId,
                            SubjectName = subject.SubjectName,
                            ClassName = model.ClassName,
                            TermName = model.TermName,
                            SessionName = model.SessionName,
                            CaSetUp = await Db.CaSetUps.Where(x => x.IsTrue.Equals(true)
                                                                   && x.SchoolId.Equals(userSchool)
                                                                   && x.ClassName.Equals(studentClassName)
                                                                   && x.TermName.Equals(model.TermName))
                                .OrderBy(o => o.CaOrder).ToListAsync(),


                            CaSetUpCount = await Db.CaSetUps.CountAsync(x => x.IsTrue.Equals(true)
                                                                             && x.SchoolId.Equals(userSchool)
                                                                             && x.TermName.Equals(model.TermName)
                                                                             && x.ClassName.Equals(studentClassName))
                        };
                        myCalist.Add(ca);
                    }
                }
                return myCalist;
            }
            return null;

        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "DownloadCa")]
        public async Task DownloadCa(CaSelectIndexVm model)
        {
            //var facilityList = Db.Communications.AsNoTracking().ToList();
            var myCalist = await GenerateCaList(model);
            var setUpCount = myCalist.Select(s => s.CaSetUpCount).FirstOrDefault();
            char c1 = 'A';
            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Report");

            worksheet.Cells[$"{c1++}1"].Value = "Id";
            worksheet.Cells[$"{c1++}1"].Value = "Student Name";
            worksheet.Cells[$"{c1++}1"].Value = "Student Number";
            worksheet.Cells[$"{c1++}1"].Value = "Subject Name";
            worksheet.Cells[$"{c1++}1"].Value = "Term";
            worksheet.Cells[$"{c1++}1"].Value = "Session";

            var itemHeader = myCalist.Where(x => x.CaSetUpCount >= 1).Select(s => s.CaSetUp).FirstOrDefault();


            foreach (var caitem in itemHeader)
            {
                worksheet.Cells[$"{c1++}1"].Value = $"{caitem.CaCaption}({caitem.MaximumScore})";
            }

            int rowStart = 2;
            char c2 = 'A';

            for (var i = 0; i < myCalist.Count; i++)
            {

                worksheet.Cells[$"A{rowStart}"].Value = myCalist[i].CaListId;
                worksheet.Cells[$"B{rowStart}"].Value = myCalist[i].StudentName;
                worksheet.Cells[$"C{rowStart}"].Value = myCalist[i].StudentId;
                worksheet.Cells[$"D{rowStart}"].Value = myCalist[i].SubjectName;
                worksheet.Cells[$"E{rowStart}"].Value = "First";
                worksheet.Cells[$"F{rowStart}"].Value = myCalist[i].SessionName;

                if (setUpCount == 1)
                {
                    worksheet.Cells[$"G{rowStart}"].Value = myCalist[i].ExamCa;
                }
                else if (setUpCount == 2)
                {
                    worksheet.Cells[$"G{rowStart}"].Value = myCalist[i].FirstCa;
                    worksheet.Cells[$"H{rowStart}"].Value = myCalist[i].ExamCa;
                }
                else if (setUpCount == 3)
                {
                    worksheet.Cells[$"G{rowStart}"].Value = myCalist[i].FirstCa;
                    worksheet.Cells[$"H{rowStart}"].Value = myCalist[i].SecondCa;
                    worksheet.Cells[$"I{rowStart}"].Value = myCalist[i].ExamCa;
                }
                else if (setUpCount == 4)
                {
                    worksheet.Cells[$"G{rowStart}"].Value = myCalist[i].FirstCa;
                    worksheet.Cells[$"H{rowStart}"].Value = myCalist[i].SecondCa;
                    worksheet.Cells[$"I{rowStart}"].Value = myCalist[i].ThirdCa;
                    worksheet.Cells[$"J{rowStart}"].Value = myCalist[i].ExamCa;
                }
                else if (setUpCount == 5)
                {
                    worksheet.Cells[$"G{rowStart}"].Value = myCalist[i].FirstCa;
                    worksheet.Cells[$"H{rowStart}"].Value = myCalist[i].SecondCa;
                    worksheet.Cells[$"I{rowStart}"].Value = myCalist[i].ThirdCa;
                    worksheet.Cells[$"J{rowStart}"].Value = myCalist[i].ForthCa;
                    worksheet.Cells[$"K{rowStart}"].Value = myCalist[i].ExamCa;
                }
                else if (setUpCount == 6)
                {
                    worksheet.Cells[$"G{rowStart}"].Value = myCalist[i].FirstCa;
                    worksheet.Cells[$"H{rowStart}"].Value = myCalist[i].SecondCa;
                    worksheet.Cells[$"I{rowStart}"].Value = myCalist[i].ThirdCa;
                    worksheet.Cells[$"J{rowStart}"].Value = myCalist[i].ForthCa;
                    worksheet.Cells[$"K{rowStart}"].Value = myCalist[i].FifthCa;
                    worksheet.Cells[$"L{rowStart}"].Value = myCalist[i].ExamCa;
                }
                else if (setUpCount == 7)
                {
                    worksheet.Cells[$"G{rowStart}"].Value = myCalist[i].FirstCa;
                    worksheet.Cells[$"H{rowStart}"].Value = myCalist[i].SecondCa;
                    worksheet.Cells[$"I{rowStart}"].Value = myCalist[i].ThirdCa;
                    worksheet.Cells[$"J{rowStart}"].Value = myCalist[i].ForthCa;
                    worksheet.Cells[$"K{rowStart}"].Value = myCalist[i].FifthCa;
                    worksheet.Cells[$"L{rowStart}"].Value = myCalist[i].SixthCa;
                    worksheet.Cells[$"M{rowStart}"].Value = myCalist[i].ExamCa;
                }
                else if (setUpCount == 8)
                {
                    worksheet.Cells[$"G{rowStart}"].Value = myCalist[i].FirstCa;
                    worksheet.Cells[$"H{rowStart}"].Value = myCalist[i].SecondCa;
                    worksheet.Cells[$"I{rowStart}"].Value = myCalist[i].ThirdCa;
                    worksheet.Cells[$"J{rowStart}"].Value = myCalist[i].ForthCa;
                    worksheet.Cells[$"K{rowStart}"].Value = myCalist[i].FifthCa;
                    worksheet.Cells[$"L{rowStart}"].Value = myCalist[i].SixthCa;
                    worksheet.Cells[$"M{rowStart}"].Value = myCalist[i].SeventhCa;
                    worksheet.Cells[$"N{rowStart}"].Value = myCalist[i].ExamCa;
                }
                else if (setUpCount == 9)
                {
                    worksheet.Cells[$"G{rowStart}"].Value = myCalist[i].FirstCa;
                    worksheet.Cells[$"H{rowStart}"].Value = myCalist[i].SecondCa;
                    worksheet.Cells[$"I{rowStart}"].Value = myCalist[i].ThirdCa;
                    worksheet.Cells[$"J{rowStart}"].Value = myCalist[i].ForthCa;
                    worksheet.Cells[$"K{rowStart}"].Value = myCalist[i].FifthCa;
                    worksheet.Cells[$"L{rowStart}"].Value = myCalist[i].SixthCa;
                    worksheet.Cells[$"M{rowStart}"].Value = myCalist[i].SeventhCa;
                    worksheet.Cells[$"N{rowStart}"].Value = myCalist[i].EightCa;
                    worksheet.Cells[$"O{rowStart}"].Value = myCalist[i].ExamCa;
                }
                else if (setUpCount == 10)
                {
                    worksheet.Cells[$"G{rowStart}"].Value = myCalist[i].FirstCa;
                    worksheet.Cells[$"H{rowStart}"].Value = myCalist[i].SecondCa;
                    worksheet.Cells[$"I{rowStart}"].Value = myCalist[i].ThirdCa;
                    worksheet.Cells[$"J{rowStart}"].Value = myCalist[i].ForthCa;
                    worksheet.Cells[$"K{rowStart}"].Value = myCalist[i].FifthCa;
                    worksheet.Cells[$"L{rowStart}"].Value = myCalist[i].SixthCa;
                    worksheet.Cells[$"M{rowStart}"].Value = myCalist[i].SeventhCa;
                    worksheet.Cells[$"N{rowStart}"].Value = myCalist[i].EightCa;
                    worksheet.Cells[$"O{rowStart}"].Value = myCalist[i].NinthtCa;
                    worksheet.Cells[$"P{rowStart}"].Value = myCalist[i].ExamCa;
                }
                rowStart++;
            }
            var info = myCalist.FirstOrDefault();
            worksheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + $"{info.SubjectName}{info.ClassName}Result.xlsx");
            Response.BinaryWrite(package.GetAsByteArray());
            Response.End();

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCa(List<CaListVm> model)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in model)
                {
                    var myTotal = item.FirstCa + item.SecondCa + item.ThirdCa + item.ForthCa + item.FifthCa +
                                  item.SixthCa + item.SeventhCa + item.EightCa + item.NinthtCa + item.ExamCa;
                    var className = await Db.Classes.AsNoTracking().Where(x =>
                                    x.SchoolId.Equals(userSchool) && x.FullClassName.Equals(item.ClassName))
                                    .Select(x => x.ClassName).FirstOrDefaultAsync();
                    var caList = new CaList()
                    {
                        CaListId = item.CaListId,
                        StudentId = item.StudentId,
                        StudentName = item.StudentName,
                        TermName = item.TermName,
                        SessionName = item.SessionName,
                        SubjectId = item.SubjectId,
                        ClassName = item.ClassName,
                        FirstCa = item.FirstCa,
                        SecondCa = item.SecondCa,
                        ThirdCa = item.ThirdCa,
                        ForthCa = item.ForthCa,
                        FifthCa = item.FifthCa,
                        SixthCa = item.SixthCa,
                        SeventhCa = item.SeventhCa,
                        EightCa = item.EightCa,
                        NinthtCa = item.NinthtCa,
                        ExamCa = item.ExamCa,
                        Total = myTotal,
                        Grading = _myGradeRemark.Grading(myTotal, className, userSchool),
                        Remark = _myGradeRemark.Remark(myTotal, className, userSchool),

                        SchoolId = userSchool,
                    };
                    Db.CaLists.AddOrUpdate(caList);
                }
                await Db.SaveChangesAsync();
                return RedirectToAction("CreateCaView");
            }

            return RedirectToAction("CreateCaView", new { model = model });
        }

        // GET: CaLists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CaLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CaList caList)
        {
            if (ModelState.IsValid)
            {
                Db.CaLists.Add(caList);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(caList);
        }

        // GET: CaLists/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaList caList = await Db.CaLists.FindAsync(id);
            if (caList == null)
            {
                return HttpNotFound();
            }
            return View(caList);
        }

        // POST: CaLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CaList caList)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(caList).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(caList);
        }

        // GET: CaLists/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaList caList = await Db.CaLists.FindAsync(id);
            if (caList == null)
            {
                return HttpNotFound();
            }
            return View(caList);
        }

        // POST: CaLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CaList caList = await Db.CaLists.FindAsync(id);
            Db.CaLists.Remove(caList);
            await Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public JsonResult FirstCaValidation(List<double> FirstCa, List<double> ClassName, List<double> TermName, List<int> CaSetUpCount)
        {
            //bool myValue = ExamCa < 50;
            double maximumScore = 0;
            string firstCa = string.Empty;
            string className = string.Empty;
            string termName = string.Empty;
            string caSetUpCount = string.Empty;
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("firstca")) != null)
            {
                firstCa =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("firstca"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("classname")) != null)
            {
                className =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("classname"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("termname")) != null)
            {
                termName =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("termname"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("casetupcount")) != null)
            {
                caSetUpCount =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("casetupcount"))];
            }

            var checkedResult = GetValidation(className, 0, termName, caSetUpCount, maximumScore, firstCa);


            return Json(checkedResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SecondCaValidation(List<double> SecondCa, List<double> ClassName, List<double> TermName, List<int> CaSetUpCount)
        {
            //bool myValue = ExamCa < 50;
            double maximumScore = 0;
            string secondCa = string.Empty;
            string className = string.Empty;
            string termName = string.Empty;
            string caSetUpCount = string.Empty;
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("secondca")) != null)
            {
                secondCa =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("secondca"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("classname")) != null)
            {
                className =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("classname"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("termname")) != null)
            {
                termName =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("termname"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("casetupcount")) != null)
            {
                caSetUpCount =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("casetupcount"))];
            }

            var checkedResult = GetValidation(className, 1, termName, caSetUpCount, maximumScore, secondCa);


            return Json(checkedResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ThirdCaValidation(List<double> ThirdCa, List<double> ClassName, List<double> TermName, List<int> CaSetUpCount)
        {
            //bool myValue = ExamCa < 50;
            double maximumScore = 0;
            string thirdCa = string.Empty;
            string className = string.Empty;
            string termName = string.Empty;
            string caSetUpCount = string.Empty;
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("thirdca")) != null)
            {
                thirdCa =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("thirdca"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("classname")) != null)
            {
                className =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("classname"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("termname")) != null)
            {
                termName =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("termname"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("casetupcount")) != null)
            {
                caSetUpCount =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("casetupcount"))];
            }

            var checkedResult = GetValidation(className, 2, termName, caSetUpCount, maximumScore, thirdCa);


            return Json(checkedResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ForthCaValidation(List<double> ForthCa, List<double> ClassName, List<double> TermName, List<int> CaSetUpCount)
        {
            //bool myValue = ExamCa < 50;
            double maximumScore = 0;
            string forthCa = string.Empty;
            string className = string.Empty;
            string termName = string.Empty;
            string caSetUpCount = string.Empty;
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("forthca")) != null)
            {
                forthCa =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("forthca"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("classname")) != null)
            {
                className =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("classname"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("termname")) != null)
            {
                termName =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("termname"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("casetupcount")) != null)
            {
                caSetUpCount =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("casetupcount"))];
            }

            var checkedResult = GetValidation(className, 3, termName, caSetUpCount, maximumScore, forthCa);


            return Json(checkedResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FifthCaValidation(List<double> FifthCa, List<double> ClassName, List<double> TermName, List<int> CaSetUpCount)
        {
            //bool myValue = ExamCa < 50;
            double maximumScore = 0;
            string fifthCa = string.Empty;
            string className = string.Empty;
            string termName = string.Empty;
            string caSetUpCount = string.Empty;
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("fifthca")) != null)
            {
                fifthCa =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("fifthca"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("classname")) != null)
            {
                className =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("classname"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("termname")) != null)
            {
                termName =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("termname"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("casetupcount")) != null)
            {
                caSetUpCount =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("casetupcount"))];
            }

            var checkedResult = GetValidation(className, 4, termName, caSetUpCount, maximumScore, fifthCa);


            return Json(checkedResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SixthCaValidation(List<double> SixthCa, List<double> ClassName, List<double> TermName, List<int> CaSetUpCount)
        {
            //bool myValue = ExamCa < 50;
            double maximumScore = 0;
            string sixthCa = string.Empty;
            string className = string.Empty;
            string termName = string.Empty;
            string caSetUpCount = string.Empty;
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("sixthca")) != null)
            {
                sixthCa =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("sixthca"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("classname")) != null)
            {
                className =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("classname"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("termname")) != null)
            {
                termName =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("termname"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("casetupcount")) != null)
            {
                caSetUpCount =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("casetupcount"))];
            }

            var checkedResult = GetValidation(className, 5, termName, caSetUpCount, maximumScore, sixthCa);


            return Json(checkedResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SeventhCaValidation(List<double> SeventhCa, List<double> ClassName, List<double> TermName, List<int> CaSetUpCount)
        {
            //bool myValue = ExamCa < 50;
            double maximumScore = 0;
            string seventhCa = string.Empty;
            string className = string.Empty;
            string termName = string.Empty;
            string caSetUpCount = string.Empty;
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("seventhca")) != null)
            {
                seventhCa =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("seventhca"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("classname")) != null)
            {
                className =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("classname"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("termname")) != null)
            {
                termName =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("termname"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("casetupcount")) != null)
            {
                caSetUpCount =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("casetupcount"))];
            }

            var checkedResult = GetValidation(className, 6, termName, caSetUpCount, maximumScore, seventhCa);


            return Json(checkedResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EightCaValidation(List<double> EightCa, List<double> ClassName, List<double> TermName, List<int> CaSetUpCount)
        {
            //bool myValue = ExamCa < 50;
            double maximumScore = 0;
            string eightCa = string.Empty;
            string className = string.Empty;
            string termName = string.Empty;
            string caSetUpCount = string.Empty;
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("eightca")) != null)
            {
                eightCa =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("eightca"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("classname")) != null)
            {
                className =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("classname"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("termname")) != null)
            {
                termName =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("termname"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("casetupcount")) != null)
            {
                caSetUpCount =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("casetupcount"))];
            }

            var checkedResult = GetValidation(className, 7, termName, caSetUpCount, maximumScore, eightCa);


            return Json(checkedResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult NinthtCaValidation(List<double> NinthtCa, List<double> ClassName, List<double> TermName, List<int> CaSetUpCount)
        {
            //bool myValue = ExamCa < 50;
            double maximumScore = 0;
            string ninthtCa = string.Empty;
            string className = string.Empty;
            string termName = string.Empty;
            string caSetUpCount = string.Empty;
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("ninthtca")) != null)
            {
                ninthtCa =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("ninthtca"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("classname")) != null)
            {
                className =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("classname"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("termname")) != null)
            {
                termName =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("termname"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("casetupcount")) != null)
            {
                caSetUpCount =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("casetupcount"))];
            }

            var checkedResult = GetValidation(className, 8, termName, caSetUpCount, maximumScore, ninthtCa);


            return Json(checkedResult, JsonRequestBehavior.AllowGet);
        }


        private bool GetValidation(string className, int pointer, string termName, string caSetUpCount, double maximumScore,
            string inputScore)
        {
            var studentClassName = Db.Classes.AsNoTracking().Where(x => x.SchoolId.ToUpper().Trim().Equals(userSchool) && x.FullClassName.Equals(className))
                                        .Select(s => s.ClassName).FirstOrDefault();
            var setUps = Db.CaSetUps.AsNoTracking().Where(x => x.IsTrue.Equals(true)
                                                               && x.SchoolId.Equals(userSchool)
                                                               && x.ClassName.Equals(studentClassName)
                                                               && x.TermName.Equals(termName))
                .OrderBy(o => o.CaOrder).ToList();

            if (Convert.ToInt16(caSetUpCount) > 1)
            {
                maximumScore = setUps[pointer].MaximumScore;
            }

            if (Convert.ToDouble(inputScore) > maximumScore)
            {
                return false;
            }
            return true;
        }

        public JsonResult ExamCaValidation(List<double> ExamCa, List<double> ClassName, List<double> TermName, List<int> CaSetUpCount)
        {
            //bool myValue = ExamCa < 50;
            double maximumScore = 0;
            string examCa = string.Empty;
            string className = string.Empty;
            string termName = string.Empty;
            string caSetUpCount = string.Empty;
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("examca")) != null)
            {
                examCa =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("examca"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("classname")) != null)
            {
                className =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("classname"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("termname")) != null)
            {
                termName =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("termname"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("casetupcount")) != null)
            {
                caSetUpCount =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("casetupcount"))];
            }

            var studentClassName = Db.Classes.AsNoTracking().Where(x => x.FullClassName.Equals(className))
                .Select(s => s.ClassName).FirstOrDefault();

            var setUps = Db.CaSetUps.AsNoTracking().Where(x => x.IsTrue.Equals(true)
                                                               && x.SchoolId.Equals(userSchool)
                                                               && x.ClassName.Equals(studentClassName)
                                                               && x.TermName.Equals(termName))
                .OrderBy(o => o.CaOrder).ToList();

            if (Convert.ToInt16(caSetUpCount) == 1)
            {
                maximumScore = setUps[1].MaximumScore;
            }
            if (Convert.ToInt16(caSetUpCount) > 1)
            {
                maximumScore = setUps[Convert.ToInt16(caSetUpCount) - 1].MaximumScore;
            }


            if (Convert.ToDouble(examCa) > maximumScore)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);


            // return Json(false, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult UploadResult()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> UploadResult(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please Select a excel file <br/>";
                TempData["UserMessage"] = "Please Select a excel file.";
                TempData["Title"] = "Error.";

                return View("Index");
            }
            HttpPostedFileBase file = Request.Files["excelfile"];
            if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
            {
                double firstCa = 0, secondCa = 0, thirdCa = 0, forthCa = 0, fifthCa = 0, sixthCa = 0, seventhCa = 0,
                                        eightCa = 0, ninthCa = 0, examCa = 0;
                string lastrecord = "";
                int recordCount = 0;
                string message = "";
                string fileContentType = file.ContentType;
                byte[] fileBytes = new byte[file.ContentLength];
                var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                // Read data from excel file
                using (var package = new ExcelPackage(file.InputStream))
                {
                    var continiousAssesmentVmList = new List<CaListVm>();
                    ExcelValidation myExcel = new ExcelValidation();
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;
                    //int requiredField = 12;


                    for (int row = 2; row <= noOfRow; row++)
                    {
                        int caId = Convert.ToInt32(workSheet.Cells[row, 1].Value.ToString().Trim());
                        string studentName = workSheet.Cells[row, 2].Value.ToString().Trim();
                        string studentId = workSheet.Cells[row, 3].Value.ToString().Trim();
                        string subjectName = workSheet.Cells[row, 4].Value.ToString().Trim();
                        string termName = workSheet.Cells[row, 5].Value.ToString().Trim();
                        string sessionName = workSheet.Cells[row, 6].Value.ToString().Trim();
                        string className = await GetClassName(studentId, termName, sessionName);
                        var studentClassName = Db.Classes.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool) &&
                                                    x.FullClassName.Equals(className))
                                                    .Select(s => s.ClassName).FirstOrDefault();
                        var caSetup = await Db.CaSetUps.Where(x => x.IsTrue.Equals(true)
                                                && x.SchoolId.Equals(userSchool)
                                                && x.ClassName.Equals(studentClassName)
                                                && x.TermName.Equals(termName))
                                                .CountAsync();
                        var subjectId = await Db.Subjects.AsNoTracking()
                                            .Where(x => x.SchoolId.Equals(userSchool) &&
                                            x.SubjectName.ToUpper().Equals(subjectName.ToUpper()))
                                            .Select(s => s.SubjectId).FirstOrDefaultAsync();
                        var iscCaExist = Db.CaLists.AsNoTracking().Any(x => x.StudentId.Equals(studentId)
                                                    && x.ClassName.Equals(className) && x.SubjectId.Equals(subjectId)
                                                    && x.TermName.Equals(termName) && x.SessionName.Equals(sessionName));
                        if (iscCaExist)
                        {
                            ViewBag.ErrorInfo = "Record Already Exist, Please Download and Update again";
                            ViewBag.ErrorMessage = "You can add the same student record twice, please Update...";
                            return View("Error1");
                        }
                        if (caSetup == 2)
                        {
                            //var checkedResult = GetValidation(className, 8, termName, caSetup.ToString(), 0.0, ninthtCa);
                            firstCa = Convert.ToDouble(workSheet.Cells[row, 7].Value.ToString().Trim());
                            examCa = Convert.ToDouble(workSheet.Cells[row, 8].Value.ToString().Trim());
                        }
                        if (caSetup == 3)
                        {
                            firstCa = Convert.ToDouble(workSheet.Cells[row, 7].Value.ToString().Trim());
                            secondCa = Convert.ToDouble(workSheet.Cells[row, 8].Value.ToString().Trim());
                            examCa = Convert.ToDouble(workSheet.Cells[row, 9].Value.ToString().Trim());
                        }
                        if (caSetup == 4)
                        {
                            firstCa = Convert.ToDouble(workSheet.Cells[row, 7].Value.ToString().Trim());
                            secondCa = Convert.ToDouble(workSheet.Cells[row, 8].Value.ToString().Trim());
                            thirdCa = Convert.ToDouble(workSheet.Cells[row, 9].Value.ToString().Trim());
                            examCa = Convert.ToDouble(workSheet.Cells[row, 10].Value.ToString().Trim());
                        }
                        if (caSetup == 5)
                        {
                            firstCa = Convert.ToDouble(workSheet.Cells[row, 7].Value.ToString().Trim());
                            secondCa = Convert.ToDouble(workSheet.Cells[row, 8].Value.ToString().Trim());
                            thirdCa = Convert.ToDouble(workSheet.Cells[row, 9].Value.ToString().Trim());
                            forthCa = Convert.ToDouble(workSheet.Cells[row, 10].Value.ToString().Trim());
                            examCa = Convert.ToDouble(workSheet.Cells[row, 11].Value.ToString().Trim());
                        }
                        if (caSetup == 6)
                        {
                            firstCa = Convert.ToDouble(workSheet.Cells[row, 7].Value.ToString().Trim());
                            secondCa = Convert.ToDouble(workSheet.Cells[row, 8].Value.ToString().Trim());
                            thirdCa = Convert.ToDouble(workSheet.Cells[row, 9].Value.ToString().Trim());
                            forthCa = Convert.ToDouble(workSheet.Cells[row, 10].Value.ToString().Trim());
                            fifthCa = Convert.ToDouble(workSheet.Cells[row, 11].Value.ToString().Trim());
                            examCa = Convert.ToDouble(workSheet.Cells[row, 12].Value.ToString().Trim());
                        }
                        if (caSetup == 7)
                        {
                            firstCa = Convert.ToDouble(workSheet.Cells[row, 7].Value.ToString().Trim());
                            secondCa = Convert.ToDouble(workSheet.Cells[row, 8].Value.ToString().Trim());
                            thirdCa = Convert.ToDouble(workSheet.Cells[row, 9].Value.ToString().Trim());
                            forthCa = Convert.ToDouble(workSheet.Cells[row, 10].Value.ToString().Trim());
                            fifthCa = Convert.ToDouble(workSheet.Cells[row, 11].Value.ToString().Trim());
                            sixthCa = Convert.ToDouble(workSheet.Cells[row, 12].Value.ToString().Trim());
                            examCa = Convert.ToDouble(workSheet.Cells[row, 13].Value.ToString().Trim());
                        }
                        if (caSetup == 8)
                        {
                            firstCa = Convert.ToDouble(workSheet.Cells[row, 7].Value.ToString().Trim());
                            secondCa = Convert.ToDouble(workSheet.Cells[row, 8].Value.ToString().Trim());
                            thirdCa = Convert.ToDouble(workSheet.Cells[row, 9].Value.ToString().Trim());
                            forthCa = Convert.ToDouble(workSheet.Cells[row, 10].Value.ToString().Trim());
                            fifthCa = Convert.ToDouble(workSheet.Cells[row, 11].Value.ToString().Trim());
                            sixthCa = Convert.ToDouble(workSheet.Cells[row, 12].Value.ToString().Trim());
                            seventhCa = Convert.ToDouble(workSheet.Cells[row, 13].Value.ToString().Trim());
                            examCa = Convert.ToDouble(workSheet.Cells[row, 14].Value.ToString().Trim());
                        }
                        if (caSetup == 9)
                        {
                            firstCa = Convert.ToDouble(workSheet.Cells[row, 7].Value.ToString().Trim());
                            secondCa = Convert.ToDouble(workSheet.Cells[row, 8].Value.ToString().Trim());
                            thirdCa = Convert.ToDouble(workSheet.Cells[row, 9].Value.ToString().Trim());
                            forthCa = Convert.ToDouble(workSheet.Cells[row, 10].Value.ToString().Trim());
                            fifthCa = Convert.ToDouble(workSheet.Cells[row, 11].Value.ToString().Trim());
                            sixthCa = Convert.ToDouble(workSheet.Cells[row, 12].Value.ToString().Trim());
                            seventhCa = Convert.ToDouble(workSheet.Cells[row, 14].Value.ToString().Trim());
                            eightCa = Convert.ToDouble(workSheet.Cells[row, 15].Value.ToString().Trim());
                            examCa = Convert.ToDouble(workSheet.Cells[row, 16].Value.ToString().Trim());
                        }
                        if (caSetup == 10)
                        {
                            firstCa = Convert.ToDouble(workSheet.Cells[row, 7].Value.ToString().Trim());
                            secondCa = Convert.ToDouble(workSheet.Cells[row, 8].Value.ToString().Trim());
                            thirdCa = Convert.ToDouble(workSheet.Cells[row, 9].Value.ToString().Trim());
                            forthCa = Convert.ToDouble(workSheet.Cells[row, 10].Value.ToString().Trim());
                            fifthCa = Convert.ToDouble(workSheet.Cells[row, 11].Value.ToString().Trim());
                            sixthCa = Convert.ToDouble(workSheet.Cells[row, 12].Value.ToString().Trim());
                            seventhCa = Convert.ToDouble(workSheet.Cells[row, 14].Value.ToString().Trim());
                            eightCa = Convert.ToDouble(workSheet.Cells[row, 15].Value.ToString().Trim());
                            ninthCa = Convert.ToDouble(workSheet.Cells[row, 16].Value.ToString().Trim());
                            examCa = Convert.ToDouble(workSheet.Cells[row, 17].Value.ToString().Trim());
                        }

                        try
                        {
                            var myTotal = firstCa + secondCa + thirdCa + forthCa + fifthCa +
                                          sixthCa + seventhCa + eightCa + ninthCa + examCa;

                            var caList = new CaList()
                            {
                                CaListId = caId,
                                StudentId = studentId,
                                StudentName = studentName,
                                TermName = termName,
                                SessionName = sessionName,
                                SubjectId = subjectId,
                                ClassName = className,
                                FirstCa = firstCa,
                                SecondCa = secondCa,
                                ThirdCa = thirdCa,
                                ForthCa = forthCa,
                                FifthCa = fifthCa,
                                SixthCa = sixthCa,
                                SeventhCa = seventhCa,
                                EightCa = eightCa,
                                NinthtCa = ninthCa,
                                ExamCa = examCa,
                                Total = myTotal,
                                Grading = _myGradeRemark.Grading(myTotal, studentClassName, userSchool),
                                Remark = _myGradeRemark.Remark(myTotal, studentClassName, userSchool),

                                SchoolId = userSchool,
                            };
                            Db.CaLists.AddOrUpdate(caList);

                        }
                        catch (Exception ex)
                        {
                            ViewBag.ErrorInfo = "The programme code in the excel doesn't exist";
                            ViewBag.ErrorMessage = ex.Message;
                            return View("Error1");
                        }
                    }
                    await Db.SaveChangesAsync();

                }
                return RedirectToAction("CreateCaView");
            }

            ViewBag.Error = $"File type is Incorrect <br/>";
            return View("Index");
        }

        private async Task<string> GetClassName(string studentId, string termName, string sessionName)
        {
            var className = await Db.AssignedClasses.AsNoTracking().Where(
                                    x => x.StudentId.ToUpper().Equals(studentId.ToUpper())
                                     && x.TermName.ToUpper().Equals(termName.ToUpper())
                                     && x.SessionName.ToUpper().Equals(sessionName.ToUpper()))
                                     .Select(s => s.ClassName).FirstOrDefaultAsync();

            return className;
        }

        public async Task<ActionResult> SearchView()
        {
            ViewBag.TermName = new SelectList(Db.Terms.AsNoTracking(), "TermName", "TermName");
            ViewBag.SessionName = new SelectList(Db.Sessions.AsNoTracking(), "SessionName", "SessionName");

            if (User.IsInRole("Teacher"))
            {
                string name = User.Identity.GetUserName();
                var subjectList = Db.AssignSubjectTeachers.Include(i => i.Subject).AsNoTracking()
                    .Where(x => x.StaffName.Equals(name)).Select(x => x.Subject).Distinct().ToList();

                var subject = new List<Subject>();
                var classes = new List<Class>();

                var classList = Db.AssignSubjectTeachers.Include(i => i.Subject).AsNoTracking()
                    .Where(x => x.StaffName.Equals(name)).Select(x => x.ClassName).Distinct().ToList();
                foreach (var item in classList)
                {
                    classes.Add(Db.Classes.AsNoTracking().FirstOrDefault(x => x.FullClassName.Equals(item)));
                }

                subject.AddRange(subjectList);
                ViewBag.SubjectId = new SelectList(subject, "SubjectId", "SubjectName");
                ViewBag.ClassName = new SelectList(classes, "FullClassName", "FullClassName");
            }
            else
            {
                ViewBag.SubjectId = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
                ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            }

            ViewBag.SetUpCount = 0;
            //return View(myCalist);
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> DeleteCa(CaSelectIndexVm model)
        {
            var subjectName = await Db.AssignSubjects.AsNoTracking().Where(x => x.ClassName.Equals(model.ClassName))
                                    .ToListAsync();
            var myClasit = new List<CaList>();
            foreach (var subject in subjectName)
            {
                //var subjectId = await Db.Subjects.AsNoTracking().Where(x => x.SubjectName.Equals(subject.s))
                //    .FirstOrDefaultAsync();
                var calist = await Db.CaLists.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool) &&
                                        x.ClassName.Equals(model.ClassName)
                                        && x.TermName.Equals(model.TermName)
                                        && x.SubjectId.Equals(subject.SubjectId)
                                        && x.SessionName.Equals(model.SessionName))
                                        .ToListAsync();
                myClasit.AddRange(calist);
            }

            //var applicants = await _db.Applicants.GroupBy(i => i.ApplicantId)
            //    .Where(x => x.Count() > 1).Select(s => s.Key).ToListAsync();
            ViewBag.Message = $"{myClasit.Count} records deleted successfully";
            foreach (var ca in myClasit)
            {
                Db.Entry(ca).State = EntityState.Deleted;
            }
            await Db.SaveChangesAsync();


            return View(myClasit);
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
