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
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class CaListsController : BaseController
    {
        private GradeRemark _myGradeRemark = new GradeRemark();

        public async Task<ActionResult> CreateCaView()
        {
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
            //var myCalist = new CaListIndexVm()
            //{
            //    CaList = await Db.CaLists.ToListAsync(),
            //    CaSetUp = Db.CaSetUps.Where(x => x.IsTrue.Equals(true)
            //                && x.SchoolId.Equals(userSchool)).OrderBy(o => o.CaOrder).ToList()
            //};
            ViewBag.SetUpCount = 0;
            //return View(myCalist);
            return View();
        }

        // GET: CaLists
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Index")]
        public async Task<ActionResult> Index(CaSelectIndexVm model)
        {
            var studentClassName = Db.Classes.AsNoTracking().Where(x => x.FullClassName.Equals(model.ClassName))
                                        .Select(s => s.ClassName).FirstOrDefault();
            var calist = Db.CaLists.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool) &&
                                                    x.ClassName.Equals(model.ClassName)
                                                    && x.TermName.Equals(model.TermName)
                                                    && x.SubjectId.Equals(model.SubjectId)
                                                    && x.SessionName.Equals(model.SessionName))
                                                    .ToList();
            var myCalist = new CaListIndexVm()
            {
                CaList = calist,
                CaSetUp = Db.CaSetUps.Where(x => x.IsTrue.Equals(true)
                                                 && x.SchoolId.Equals(userSchool)
                                                 && x.ClassName.Equals(studentClassName)
                                                 && x.TermName.Equals(model.TermName))
                    .OrderBy(o => o.CaOrder).ToList(),

            };
            ViewBag.SetUpCount = Db.CaSetUps.Count(x => x.IsTrue.Equals(true)
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
            var subject = await Db.Subjects.FindAsync(model.SubjectId);
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
                        CaSetUp = Db.CaSetUps.Where(x => x.IsTrue.Equals(true)
                                                         && x.SchoolId.Equals(userSchool)
                                                         && x.ClassName.Equals(studentClassName)
                                                         && x.TermName.Equals(model.TermName))
                                                        .OrderBy(o => o.CaOrder).ToList(),

                        CaSetUpCount = Db.CaSetUps.Count(x => x.IsTrue.Equals(true)
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
                        CaSetUp = Db.CaSetUps.Where(x => x.IsTrue.Equals(true)
                                                         && x.SchoolId.Equals(userSchool)
                                                         && x.ClassName.Equals(studentClassName)
                                                         && x.TermName.Equals(model.TermName))
                                                        .OrderBy(o => o.CaOrder).ToList(),


                        CaSetUpCount = Db.CaSetUps.Count(x => x.IsTrue.Equals(true)
                                                                             && x.SchoolId.Equals(userSchool)
                                                                             && x.TermName.Equals(model.TermName)
                                                                             && x.ClassName.Equals(studentClassName)),
                    };
                    myCalist.Add(ca);
                }
            }
            return myCalist;
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
            //foreach (var item in model)
            //{
            //    if (item.ExamCa > 50)
            //    {
            //        ModelState.AddModelError("ExamCa", "Exam Score is greater");
            //        var myModel = new CaSelectIndexVm
            //        {
            //            ClassName = item.ClassName,
            //            SubjectId = item.SubjectId,
            //            TermName = item.TermName,
            //            SessionName = item.SessionName

            //        };

            //        RedirectToAction("CreateCaView", new { model = myModel });
            //    }
            //}

            if (ModelState.IsValid)
            {
                foreach (var item in model)
                {
                    var myTotal = item.FirstCa + item.SecondCa + item.ThirdCa + item.ForthCa + item.FifthCa +
                                  item.SixthCa + item.SecondCa + item.EightCa + item.NinthtCa + item.ExamCa;
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
                        Grading = _myGradeRemark.Grading(myTotal, item.ClassName, userSchool),
                        Remark = _myGradeRemark.Remark(myTotal, item.ClassName, userSchool),

                        SchoolId = userSchool,
                    };
                    Db.CaLists.AddOrUpdate(caList);
                }
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
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
        public async Task<ActionResult> Create([Bind(Include = "CaListId,StudentId,TermName,SessionName,SubjectCode,ClassName,FirstCa,SecondCa,ThirdCa,ForthCa,FifthCa,FseventhCa,Eight,NinthtCa,ExamCa")] CaList caList)
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
        public async Task<ActionResult> Edit([Bind(Include = "CaListId,StudentId,TermName,SessionName,SubjectCode,ClassName,FirstCa,SecondCa,ThirdCa,ForthCa,FifthCa,FseventhCa,Eight,NinthtCa,ExamCa")] CaList caList)
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
