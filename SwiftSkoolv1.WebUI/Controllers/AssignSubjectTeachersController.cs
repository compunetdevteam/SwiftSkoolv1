using Microsoft.AspNet.Identity;
using PagedList;
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
            return View(assignedList.ToPagedList(pageNumber, pageSize));
            //return View(await Db.AssignSubjectTeachers.ToListAsync());
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
        public async Task<ActionResult> Edit([Bind(Include = "Id,SubjectId,ClassName,StaffName")] AssignSubjectTeacher assignSubjectTeacher)
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
        public async Task<ActionResult> Delete(int? id)
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

        // POST: AssignSubjectTeachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var assignSubjectTeacher = await Db.AssignSubjectTeachers.FindAsync(id);
            if (assignSubjectTeacher != null) Db.AssignSubjectTeachers.Remove(assignSubjectTeacher);
            await Db.SaveChangesAsync();
            TempData["UserMessage"] = $"Subject Teacher Successfully Deleted";
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
