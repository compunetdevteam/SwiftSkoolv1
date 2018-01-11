using Microsoft.AspNet.Identity;
using SwiftSkoolv1.Domain;
using SwiftSkoolv1.WebUI.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class AssignSubjectTeachersController : BaseController
    {


        // GET: AssignSubjectTeachers
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string search,
                                        int? SubjectId, string ClassName, int? page)
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
            var assignedList = from s in Db.AssignSubjectTeachers select s;
            if (Request.IsAuthenticated && !User.IsInRole("SuperAdmin"))
            {
                assignedList = assignedList.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool));
            }
            if (User.IsInRole("Teacher"))
            {
                string name = User.Identity.GetUserName();
                //var user = Db.Guardians.Where(c => c.UserName.Equals(name)).Select(s => s.Email).FirstOrDefault();

                assignedList = assignedList.Where(x => x.StaffName.Equals(name));

                //return View(subjectName);
            }
            else
            {
                if (!String.IsNullOrEmpty(search))
                {
                    assignedList = assignedList.Where(s => s.Subject.SubjectName.ToUpper().Contains(search.ToUpper())
                                                           || s.ClassName.ToUpper().Contains(search.ToUpper()));


                }
                else if (SubjectId != null || !String.IsNullOrEmpty(ClassName))
                {
                    assignedList = assignedList.Where(s => s.SubjectId.Equals(SubjectId)
                                                           || s.ClassName.ToUpper().Contains(ClassName.ToUpper()));

                }
            }
            switch (sortOrder)
            {
                case "name_desc":
                    assignedList = assignedList.OrderByDescending(s => s.Subject.SubjectName);
                    break;
                case "Date":
                    assignedList = assignedList.OrderBy(s => s.Subject.SubjectName);
                    break;
                default:
                    assignedList = assignedList.OrderBy(s => s.ClassName);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            ViewBag.SubjectId = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
            ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            // ViewBag.StaffName = new SelectList(await _query.StaffListAsync(userSchool), "Username", "Username");
            var count = await assignedList.CountAsync();
            TempData["UserMessage"] = $"You Search result contains {count} Records ";
            TempData["Title"] = "Success.";
            return View(/*ToPagedList(pageNumber, pageSize*/);
            //return View(await Db.AssignSubjectTeachers.ToListAsync());
        }


        public ActionResult TeacherIndex()
        {
            return View();
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
            var v = Db.AssignSubjectTeachers.Where(x => x.SchoolId.Equals(userSchool)).Select(s => new { s.Id, s.StaffName, s.ClassName, s.Subject.SubjectName }).ToList();

            //var v = Db.Subjects.Where(x => x.SchoolId.Equals(userSchool)).Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToList();
            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            //{
            //    //v = v.OrderBy(sortColumn + " " + sortColumnDir);
            //    v = new List<Subject>(v.OrderBy(x => "sortColumn + \" \" + sortColumnDir"));
            //}
            var userId = User.Identity.GetUserName();
            if (User.IsInRole("Teacher"))
            {
                v = v.Where(x => x.StaffName.Equals(userId)).ToList();
            }
            if (!string.IsNullOrEmpty(search))
            {
                //v = v.OrderBy(sortColumn + " " + sortColumnDir);
                v = Db.AssignSubjectTeachers.Where(x => x.SchoolId.Equals(userSchool) && (x.StaffName.Equals(search) || x.Subject.SubjectName.Equals(search) || x.ClassName.Equals(search)))
                    .Select(s => new { s.Id, s.StaffName, s.ClassName, s.Subject.SubjectName }).ToList();
            }
            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
            #endregion

            //return Json(new { data = await Db.Subjects.AsNoTracking().Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToListAsync() }, JsonRequestBehavior.AllowGet);
        }



        public async Task<PartialViewResult> Save(int id)
        {
            ViewBag.SubjectId = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
            ViewBag.ClassName = new MultiSelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            ViewBag.StaffName = new SelectList(await _query.StaffListAsync(userSchool), "Username", "Username");
            return PartialView();
        }

        // POST: Subjects/Save/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(AssignSubjectTeacherVM model)
        {
            bool status = false;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (model.Id > 0)
                {
                    var assignSubjectTeacher = await Db.AssignSubjectTeachers.FindAsync(model.Id);
                    if (assignSubjectTeacher != null)
                    {
                        assignSubjectTeacher.SchoolId = userSchool;
                        assignSubjectTeacher.ClassName = model.ClassName[0];
                        assignSubjectTeacher.SubjectId = model.SubjectId;
                        assignSubjectTeacher.StaffName = model.StaffName;
                        Db.Entry(assignSubjectTeacher).State = EntityState.Modified;
                    }
                    message = "Subject Assigned to Teacher was Updated Successfully...";
                }
                else
                {
                    if (model.ClassName != null)
                    {
                        int counter = 0;
                        string theClass = "";
                        string theName = "";
                        foreach (var item in model.ClassName)
                        {

                            var countFromDb = await Db.AssignSubjectTeachers.AsNoTracking().CountAsync(x => x.ClassName.Equals(item)
                                                && x.SubjectId.Equals(model.SubjectId) && x.SchoolId.Equals(userSchool));
                            var subject = await Db.Subjects.FindAsync(model.SubjectId);

                            // var countFromDb = CA.Count();

                            if (countFromDb >= 1)
                            {
                                message = $"Admin have already assigned Teacher to  {subject.SubjectName} in {item} Class";
                                return new JsonResult { Data = new { status = false, message = message } };
                            }

                            var assigSubjectTeacher = new AssignSubjectTeacher()
                            {
                                ClassName = item,
                                SubjectId = model.SubjectId,
                                StaffName = model.StaffName,
                                SchoolId = userSchool

                            };
                            Db.AssignSubjectTeachers.Add(assigSubjectTeacher);
                            counter += 1;
                            theClass = subject.SubjectName;
                            theName = model.StaffName;
                        }
                        message = $" You have Assigned {theClass} Subject  to {theName} in {counter} class Successfully.";
                        await Db.SaveChangesAsync();
                    }


                }
                await Db.SaveChangesAsync();
                status = true;
            }
            return new JsonResult { Data = new { status = status, message = message } };
            //return View(subject);
        }



        // GET: AssignSubjectTeachers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var assignSubjectTeacher = await Db.AssignSubjectTeachers.FindAsync(id);
            if (assignSubjectTeacher == null)
            {
                return HttpNotFound();
            }
            return View(assignSubjectTeacher);
        }

        // GET: AssignSubjectTeachers/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.SubjectId = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
            ViewBag.ClassName = new MultiSelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            ViewBag.StaffName = new SelectList(await _query.StaffListAsync(userSchool), "Username", "Username");
            return View();
        }

        // POST: AssignSubjectTeachers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AssignSubjectTeacherVM model)
        {
            if (ModelState.IsValid)
            {
                if (model.ClassName != null)
                {
                    int counter = 0;
                    string theClass = "";
                    string theName = "";
                    foreach (var item in model.ClassName)
                    {

                        var countFromDb = await Db.AssignSubjectTeachers.AsNoTracking().CountAsync(x => x.ClassName.Equals(item)
                                                            && x.SubjectId.Equals(model.SubjectId));
                        var subject = await Db.Subjects.FindAsync(model.SubjectId);


                        // var countFromDb = CA.Count();

                        if (countFromDb >= 1)
                        {
                            TempData["UserMessage"] = $"Admin have already assigned Teacher to  {subject.SubjectName} in {item} Class";
                            TempData["Title"] = "Error.";
                            ViewBag.SubjectId = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
                            ViewBag.ClassName = new MultiSelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
                            ViewBag.StaffName = new SelectList(await _query.StaffListAsync(userSchool), "Username", "Username");
                            return View(model);
                        }

                        var assigSubjectTeacher = new AssignSubjectTeacher()
                        {
                            ClassName = item,
                            SubjectId = model.SubjectId,
                            StaffName = model.StaffName,
                            SchoolId = userSchool

                        };
                        Db.AssignSubjectTeachers.Add(assigSubjectTeacher);
                        counter += 1;
                        theClass = subject.SubjectName;
                        theName = model.StaffName;
                    }
                    TempData["UserMessage"] = $" You have Assigned {theClass} Subject  to {theName} in {counter} class Successfully.";
                    TempData["Title"] = "Success.";
                    await Db.SaveChangesAsync();
                }
                return RedirectToAction("Index", "AssignSubjectTeachers");
            }
            ViewBag.SubjectId = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
            ViewBag.ClassName = new MultiSelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            ViewBag.StaffName = new SelectList(await _query.StaffListAsync(userSchool), "Username", "Username");
            return View(model);
        }

        // GET: AssignSubjectTeachers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var assignSubjectTeacher = await Db.AssignSubjectTeachers.FindAsync(id);
            if (assignSubjectTeacher == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubjectId = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
            ViewBag.ClassName = new MultiSelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            ViewBag.StaffName = new SelectList(await _query.StaffListAsync(userSchool), "Username", "Username");
            return View(assignSubjectTeacher);
        }

        // POST: AssignSubjectTeachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AssignSubjectTeacher assignSubjectTeacher)
        {
            if (ModelState.IsValid)
            {
                assignSubjectTeacher.SchoolId = userSchool;
                Db.Entry(assignSubjectTeacher).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                TempData["UserMessage"] = $"Subject Teacher Updated Successfully Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            ViewBag.SubjectId = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
            ViewBag.ClassName = new MultiSelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            ViewBag.StaffName = new SelectList(await _query.StaffListAsync(userSchool), "Username", "Username");
            return View(assignSubjectTeacher);
        }

        // GET: AssignSubjectTeachers/Delete/5
        public async Task<PartialViewResult> Delete(int? id)
        {
            var assignSubjectTeacher = await Db.AssignSubjectTeachers.FindAsync(id);

            return PartialView(assignSubjectTeacher);
        }

        // POST: AssignSubjectTeachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            bool status = false;
            string message = string.Empty;
            var assignSubjectTeacher = await Db.AssignSubjectTeachers.FindAsync(id);
            if (assignSubjectTeacher != null) Db.AssignSubjectTeachers.Remove(assignSubjectTeacher);
            await Db.SaveChangesAsync();
            status = true;
            message = "Subject Deleted from Class Successfully...";
            return new JsonResult { Data = new { status = status, message = message } };

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
