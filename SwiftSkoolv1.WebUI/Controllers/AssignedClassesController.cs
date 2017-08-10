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
    public class AssignedClassesController : BaseController
    {
        // GET: AssignedClasses
        public ActionResult Index(string sortOrder, string currentFilter, string search,
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
            var pageSize = 15;
            ViewBag.CurrentFilter = search;
            var assignedList = from s in Db.AssignedClasses select s;
            if (Request.IsAuthenticated && !User.IsInRole("SuperAdmin"))
            {
                assignedList = assignedList.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool));
            }

            if (User.IsInRole("Teacher"))
            {
                var name = User.Identity.GetUserName();
                //var user = Db.Guardians.Where(c => c.UserName.Equals(name)).Select(s => s.Email).FirstOrDefault();
                assignedList = assignedList.AsNoTracking().Where(x => x.ClassName.Equals(name) && x.SchoolId.Equals(userSchool));

                //return View(subjectName);
            }
            else
            {
                if (!String.IsNullOrEmpty(search))
                {
                    assignedList = assignedList.AsNoTracking().AsNoTracking().Where(s => s.StudentId.ToUpper().Contains(search.ToUpper())
                                                           || s.ClassName.ToUpper().Contains(search.ToUpper())
                                                           || s.TermName.ToUpper().Contains(search.ToUpper()));

                }
                else if (!String.IsNullOrEmpty(SessionName) || !String.IsNullOrEmpty(ClassName))
                {
                    assignedList = assignedList.AsNoTracking().Where(s => s.SessionName.Contains(SessionName)
                                                           && s.ClassName.ToUpper().Contains(ClassName.ToUpper())
                                                           && s.TermName.ToUpper().Contains(TermName.ToUpper()));
                    pageSize = assignedList.Count();
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

            var pageNumber = (page ?? 1);

            if (User.IsInRole("Teacher"))
            {
                var name = User.Identity.GetUserName();
                var subjectList = Db.AssignSubjectTeachers.AsNoTracking().Where(x => x.StaffName.Equals(name));
                ViewBag.ClassName = new SelectList(subjectList.AsNoTracking(), "ClassName", "ClassName");
            }
            else
            {
                ViewBag.ClassName = new SelectList(Db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            }
            ViewBag.SessionName = new SelectList(Db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.TermName = new SelectList(Db.Terms.AsNoTracking(), "TermName", "TermName");
            var count = assignedList.Count();
            TempData["Index"] = $"You Search result contains {count} Records ";
            TempData["Title"] = "Success.";
            return View(assignedList.AsNoTracking().ToPagedList(pageNumber, pageSize));
            //return View(await Db.AssignedClasses.ToListAsync());
        }


        public ActionResult GetIndex(string SessionName, string ClassName, string TermName)
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

            var v = Db.AssignedClasses.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool))
                .Select(s => new { s.AssignedClassId, s.ClassName, s.StudentName, s.StudentId, s.TermName, s.SessionName }).ToList();
            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            //{
            //    //v = v.OrderBy(sortColumn + " " + sortColumnDir);
            //    v = new List<Subject>(v.OrderBy(x => "sortColumn + \" \" + sortColumnDir"));
            //}
            if (!string.IsNullOrEmpty(search))
            {
                //v = v.OrderBy(sortColumn + " " + sortColumnDir);
                v = Db.AssignedClasses.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool) && (x.ClassName.Equals(search) || x.StudentName.Equals(search)))
                      .Select(s => new { s.AssignedClassId, s.ClassName, s.StudentName, s.StudentId, s.TermName, s.SessionName }).ToList();
            }
            else if (!String.IsNullOrEmpty(SessionName) || !String.IsNullOrEmpty(ClassName))
            {
                v = v.Where(s => s.SessionName.Equals(SessionName)
                                && s.ClassName.ToUpper().Equals(ClassName.ToUpper())
                                && s.TermName.ToUpper().Equals(TermName.ToUpper())).ToList();

            }
            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
            #endregion

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

        // POST: Subjects/Save/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                            var countFromDb = await Db.AssignedClasses.AsNoTracking().CountAsync(
                                x => x.TermName.Equals(model.TermName.ToString())
                                     && x.SessionName.Equals(model.SessionName)
                                     && x.StudentId.Equals(item));

                            if (countFromDb >= 1)
                            {
                                message = "You have already Assigned Class one or more of these student";
                                return new JsonResult { Data = new { status = false, message = message } };
                            }
                            var studentName = await Db.Students.AsNoTracking().Where(x => x.StudentId.Equals(item))
                                .Select(s => s.FullName)
                                .FirstOrDefaultAsync();
                            var assigClass = new AssignedClass
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
                        message = $"You have Assigned to {counter} Student(s) to {theClass} Successfully.";
                    }
                }
                await Db.SaveChangesAsync();
                status = true;
            }
            return new JsonResult { Data = new { status = status, message = message } };
            //return View(subject);
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

        // POST: AssignedClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                        var countFromDb = await Db.AssignedClasses.AsNoTracking().CountAsync(x => x.TermName.Equals(model.TermName.ToString())
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
                        else
                        {
                            var studentName = await Db.Students.AsNoTracking().Where(x => x.StudentId.Equals(item))
                                                .Select(s => s.FullName)
                                                .FirstOrDefaultAsync();
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
        public async Task<ActionResult> Edit(int? id)
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
            var myModel = new AssignedClassesViewModel();
            myModel.AssignedClassId = assignedClass.AssignedClassId;
            ViewBag.StudentId = new MultiSelectList(Db.Students.AsNoTracking(), "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(Db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(Db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            return View(myModel);
        }

        // POST: AssignedClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
