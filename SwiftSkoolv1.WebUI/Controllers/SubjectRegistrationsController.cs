using PagedList;
using SwiftSkool.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkool.Controllers
{
    public class SubjectRegistrationsController : BaseController
    {
        // GET: SubjectRegistrations
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string search, string StudentId,
                                         string SessionName, string ClassName, string TermName, int? page)
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
            var assignedList = from s in Db.SubjectRegistrations select s;
            if (Request.IsAuthenticated && !User.IsInRole("SuperAdmin"))
            {
                assignedList = assignedList.Where(x => x.SchoolId.Equals(userSchool));
            }
            if (!String.IsNullOrEmpty(search))
            {
                assignedList = assignedList.Where(s => s.StudentId.ToUpper().Contains(search.ToUpper()));
                //|| s.ClassName.ToUpper().Contains(search.ToUpper())
                //|| s.TermName.ToUpper().Contains(search.ToUpper()));

            }
            else if (!String.IsNullOrEmpty(SessionName) || !String.IsNullOrEmpty(ClassName)
                            || !String.IsNullOrEmpty(StudentId) || !String.IsNullOrEmpty(TermName))
            {
                assignedList = assignedList.Where(s => s.StudentId.ToUpper().Contains(StudentId.ToUpper()));
                //          s.SessionName.Contains(SessionName)
                //  && s.ClassName.ToUpper().Contains(ClassName.ToUpper())
                //  && s.TermName.ToUpper().Contains(TermName.ToUpper())
                //);
            }
            switch (sortOrder)
            {
                case "name_desc":
                    assignedList = assignedList.OrderByDescending(s => s.StudentId);
                    break;
                case "Date":
                    assignedList = assignedList.OrderBy(s => s.Subject.SubjectCode);
                    break;
                default:
                    assignedList = assignedList.OrderBy(s => s.Subject.SubjectCode);
                    break;
            }
            int pageSize = 15;
            int pageNumber = (page ?? 1);

            //ViewBag.SessionName = new SelectList(_query.SessionList(), "SessionName", "SessionName");
            //ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            //ViewBag.TermName = new SelectList(_query.TermList(), "TermName", "TermName");
            ViewBag.StudentId = new SelectList(await _query.StudentListAsync(userSchool), "StudentID", "FullName");
            //var count = assignedList.Count();
            //TempData["UserMessage"] = $"You Search result contains {count} Records ";
            //TempData["Title"] = "Success.";
            return View(assignedList.ToPagedList(pageNumber, pageSize));
            //return View(await db.AssignedClasses.ToListAsync());
        }

        // GET: SubjectRegistrations/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var subjectRegistration = await Db.SubjectRegistrations.FindAsync(id);
            if (subjectRegistration == null)
            {
                return HttpNotFound();
            }
            return View(subjectRegistration);
        }

        // GET: SubjectRegistrations/Create
        public async Task<ActionResult> Create()
        {
            //ViewBag.SessionName = new SelectList(_query.SessionList(), "SessionName", "SessionName");
            //ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            //ViewBag.TermName = new SelectList(_query.TermList(), "TermName", "TermName");
            ViewBag.StudentId = new SelectList(await _query.StudentListAsync(userSchool), "StudentID", "FullName");
            ViewBag.SubjectId = new MultiSelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");

            return View();
        }

        // POST: SubjectRegistrations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,StudentId,ClassName,TermName,SessionName,SubjectId")] SubjectRegistrationVm model)
        {
            if (ModelState.IsValid)
            {
                int counter = 0;
                string theClass = string.Empty;
                foreach (var subject in model.SubjectId)
                {
                    var studentName = Db.Students.Where(x => x.StudentId.Equals(model.StudentId))
                                              .Select(s => s.FullName)
                                              .FirstOrDefault();
                    var countFromDb = Db.SubjectRegistrations.Count(x => x.StudentId.Equals(model.StudentId)
                                            && x.SubjectId.Equals(subject));
                    //   x.ClassName.Equals(model.ClassName)
                    //&& x.TermName.Equals(model.TermName.ToString())
                    //&& x.SessionName.Equals(model.SessionName)
                    //&& );

                    // var countFromDb = CA.Count();

                    if (countFromDb >= 1)
                    {
                        TempData["UserMessage"] = $"Admin have already assigned {subject} subject to this {studentName} Student";
                        TempData["Title"] = "Error.";
                        //ViewBag.SessionName = new SelectList(_query.SessionList(), "SessionName", "SessionName");
                        //ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
                        //ViewBag.TermName = new SelectList(_query.TermList(), "TermName", "TermName");
                        ViewBag.StudentId = new SelectList(await _query.StudentListAsync(userSchool), "StudentID", "FullName");
                        ViewBag.SubjectId = new MultiSelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
                        return View(model);
                    }

                    var mySubject = new SubjectRegistration()
                    {
                        StudentId = model.StudentId,
                        StudentName = studentName,
                        //ClassName = model.ClassName,
                        //TermName = model.TermName,
                        //SessionName = model.SessionName,
                        SubjectId = subject,
                        SchoolId = userSchool
                    };
                    Db.SubjectRegistrations.Add(mySubject);
                    counter += 1;
                    theClass = studentName;
                }

                await Db.SaveChangesAsync();
                TempData["UserMessage"] = $" You have Assigned {counter} Subject(s)  to {theClass} Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            //ViewBag.SessionName = new SelectList(_query.SessionList(), "SessionName", "SessionName");
            //ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            //ViewBag.TermName = new SelectList(_query.TermList(), "TermName", "TermName");
            ViewBag.StudentId = new SelectList(await _query.StudentListAsync(userSchool), "StudentID", "FullName");
            ViewBag.SubjectId = new MultiSelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");

            return View(model);
        }

        // GET: SubjectRegistrations/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var subjectRegistration = await Db.SubjectRegistrations.FindAsync(id);
            if (subjectRegistration == null)
            {
                return HttpNotFound();
            }
            //ViewBag.SessionName = new SelectList(_query.SessionList(), "SessionName", "SessionName");
            //ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            //ViewBag.TermName = new SelectList(_query.TermList(), "TermName", "TermName");
            ViewBag.StudentId = new SelectList(await _query.StudentListAsync(userSchool), "StudentID", "FullName");
            ViewBag.SubjectId = new MultiSelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
            return View(subjectRegistration);
        }

        // POST: SubjectRegistrations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,StudentId,ClassName,TermName,SessionName,SubjectName")] SubjectRegistration model)
        {
            if (ModelState.IsValid)
            {
                var studentName = Db.Students.Where(x => x.StudentId.Equals(model.StudentId))
                                              .Select(s => s.FullName)
                                              .FirstOrDefault();
                var subjectRegistration = new SubjectRegistration()
                {
                    StudentId = model.StudentId,
                    StudentName = studentName,
                    //ClassName = model.ClassName,
                    //TermName = model.TermName,
                    //SessionName = model.SessionName,
                    SubjectId = model.SubjectId,
                    SchoolId = userSchool
                };
                Db.Entry(subjectRegistration).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                TempData["UserMessage"] = "Subject Registration Updated Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            //ViewBag.SessionName = new SelectList(_query.SessionList(), "SessionName", "SessionName");
            //ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            //ViewBag.TermName = new SelectList(_query.TermList(), "TermName", "TermName");
            ViewBag.StudentId = new SelectList(await _query.StudentListAsync(userSchool), "StudentID", "FullName");
            ViewBag.SubjectId = new MultiSelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
            return View(model);
        }

        // GET: SubjectRegistrations/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var subjectRegistration = await Db.SubjectRegistrations.FindAsync(id);
            if (subjectRegistration == null)
            {
                return HttpNotFound();
            }
            return View(subjectRegistration);
        }

        // POST: SubjectRegistrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var subjectRegistration = await Db.SubjectRegistrations.FindAsync(id);
            if (subjectRegistration != null) Db.SubjectRegistrations.Remove(subjectRegistration);
            await Db.SaveChangesAsync();
            TempData["UserMessage"] = "Subject has been removed Successfully";
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
