using System;
using Microsoft.AspNet.Identity;
using SwiftSkool.Models;
using SwiftSkool.ViewModel;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using SwiftSkoolv1.WebUI.ViewModels;

namespace SwiftSkool.Controllers
{
    public class ClassesController : BaseController
    {

        // GET: Classes
        public async Task<ActionResult> Index()
        {
            if (Request.IsAuthenticated && !User.IsInRole("SuperAdmin"))
            {
                return View(await Db.Classes.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool)).ToListAsync());
            }
            //var model = new List<ChartVm>();
            //for (int i = 0; i <= 5; i++)
            //{
            //    var data = new ChartVm
            //    {
            //        X = i,
            //        Y = 5
            //    };
            //    model.Add(data);
            //}
            //ViewBag.MyModel = model;
            return View(await Db.Classes.ToListAsync());
        }

        public async Task<ActionResult> FormTeacher()
        {
            var myForm = new FormDataViewModel();
            string username = User.Identity.GetUserName();
            var className = Db.AssignFormTeacherToClasses.Where(x => x.Username.Equals(username))
                                                .Select(s => s.ClassName)
                                                .FirstOrDefault();
            myForm.AssignedClasses = await Db.AssignedClasses.Where(x => x.ClassName.Equals(className) && x.TermName.Equals("First")
                                                          && x.SessionName.Equals("2016-2017")).ToListAsync();

            ViewBag.SubjectCode = new SelectList(Db.Subjects, "CourseName", "CourseName");
            ViewBag.StudentId = new SelectList(Db.Students, "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(Db.Sessions, "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(Db.Classes, "FullClassName", "FullClassName");
            // return View(await db.Classes.ToListAsync());
            return View(myForm);
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

            var v = Db.Classes.Where(x => x.SchoolId.Equals(userSchool)).Select(s => new { s.ClassId, s.ClassType, s.ClassName }).ToList();
            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            //{
            //    //v = v.OrderBy(sortColumn + " " + sortColumnDir);
            //    v = new List<Subject>(v.OrderBy(x => "sortColumn + \" \" + sortColumnDir"));
            //}
            if (!string.IsNullOrEmpty(search))
            {
                //v = v.OrderBy(sortColumn + " " + sortColumnDir);
                v = Db.Classes.Where(x => x.SchoolId.Equals(userSchool) && (x.ClassName.Equals(search) || x.ClassType.Equals(search)))
                    .Select(s => new { s.ClassId, s.ClassType, s.ClassName }).ToList();
            }
            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
            #endregion

            //return Json(new { data = await Db.Subjects.AsNoTracking().Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToListAsync() }, JsonRequestBehavior.AllowGet);
        }



        public async Task<PartialViewResult> Save(int id)
        {
            var classes = await Db.Classes.FindAsync(id);
            return PartialView(classes);
        }

        // POST: Subjects/Save/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(Class classes)
        {
            bool status = false;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (classes.ClassId > 0)
                {
                    classes.SchoolId = userSchool;
                    Db.Entry(classes).State = EntityState.Modified;
                    message = "Class Updated Successfully...";
                }
                else
                {
                    classes.SchoolId = userSchool;
                    Db.Classes.Add(classes);
                    message = "Class Created Successfully...";
                }
                await Db.SaveChangesAsync();
                status = true;
            }
            return new JsonResult { Data = new { status = status, message = message } };
            //return View(subject);
        }


        // GET: Classes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var @class = await Db.Classes.FindAsync(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            return View(@class);
        }

        // GET: Classes/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.SchoolName = new SelectList(_query.SchoolClassListAsync(), "ClassCode", "ClassCode");
            return View();
        }

        // POST: Classes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Class model)
        {
            if (ModelState.IsValid)
            {
                var myClass = Db.Classes.Where(x => x.SchoolId.Equals(userSchool) &&
                                                    x.FullClassName.Equals(model.FullClassName));

                if (myClass.Any())
                {
                    ViewBag.SchoolName = new SelectList(Db.SchoolClasses, "ClassCode", "ClassCode");
                    TempData["UserMessage"] = "Class Already Exist in Database";
                    TempData["Title"] = "Deleted.";
                    return View(model);
                }
                var @class = new Class()
                {
                    ClassType = model.ClassType.Trim().ToUpper(),
                    SchoolName = model.SchoolName.Trim(),
                    ClassLevel = model.ClassLevel,
                    SchoolId = userSchool
                };
                Db.Classes.Add(@class);
                await Db.SaveChangesAsync();
                TempData["UserMessage"] = "Class Added Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
                //db.Classes.Add(@class);
                //await db.SaveChangesAsync();
                //return RedirectToAction("Index");
            }
            ViewBag.SchoolName = new SelectList(Db.SchoolClasses, "ClassCode", "ClassCode");
            return View(model);
        }

        // GET: Classes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = await Db.Classes.FindAsync(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            ViewBag.SchoolName = new SelectList(Db.SchoolClasses, "ClassCode", "ClassCode");
            return View(@class);
        }

        // POST: Classes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ClassID,Name,ClassType")] Class model)
        {
            if (ModelState.IsValid)
            {
                var @class = new Class()
                {
                    ClassType = model.ClassType.Trim().ToUpper(),
                    SchoolName = model.SchoolName.Trim(),
                    ClassLevel = model.ClassLevel,
                    SchoolId = userSchool
                };
                Db.Entry(@class).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                TempData["UserMessage"] = "Class Updated Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            ViewBag.SchoolName = new SelectList(Db.SchoolClasses, "ClassCode", "ClassCode");
            return View(model);
        }




        // GET: Classes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = await Db.Classes.FindAsync(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            return View(@class);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Class @class = await Db.Classes.FindAsync(id);
            Db.Classes.Remove(@class);
            await Db.SaveChangesAsync();
            TempData["UserMessage"] = "Class Has Been Deleted";
            TempData["Title"] = "Deleted.";
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
    }
}
