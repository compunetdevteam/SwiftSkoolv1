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
    public class AssignSubjectsController : BaseController
    {
        // GET: AssignSubjects
        //public async Task<ActionResult> Index(string sortOrder, string currentFilter, string search,
        //                                 string ClassName, string TermName, int? page)
        //{

        //    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        //    if (search == null)
        //    {
        //        page = 1;
        //    }
        //    else
        //    {
        //        search = currentFilter;
        //    }
        //    ViewBag.CurrentFilter = search;
        //    var assignedList = from s in Db.AssignSubjects select s;

        //    if (Request.IsAuthenticated && !User.IsInRole("SuperAdmin"))
        //    {
        //        assignedList = assignedList.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool));
        //    }
        //    if (!String.IsNullOrEmpty(search))
        //    {
        //        assignedList = assignedList.Where(s => s.ClassName.ToUpper().Contains(search.ToUpper())
        //                                               || s.Subject.SubjectName.ToUpper().Equals(search.ToUpper()));


        //    }
        //    else if (!String.IsNullOrEmpty(ClassName) && !String.IsNullOrEmpty(TermName))
        //    {
        //        assignedList = assignedList.Where(s => s.ClassName.ToUpper().Contains(ClassName.ToUpper())
        //                            && s.TermName.ToUpper().Equals(TermName.ToUpper()));

        //    }
        //    else if (!String.IsNullOrEmpty(ClassName) || !String.IsNullOrEmpty(TermName))
        //    {
        //        assignedList = assignedList.Where(s => s.ClassName.ToUpper().Contains(ClassName.ToUpper())
        //                                               || s.TermName.ToUpper().Equals(TermName.ToUpper()));

        //    }
        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            assignedList = assignedList.OrderByDescending(s => s.Subject.SubjectName);
        //            break;
        //        case "Date":
        //            assignedList = assignedList.OrderBy(s => s.Subject.SubjectName);
        //            break;
        //        default:
        //            assignedList = assignedList.OrderBy(s => s.ClassName);
        //            break;
        //    }
        //    int pageSize = 17;
        //    int pageNumber = (page ?? 1);

        //    ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
        //    ViewBag.TermName = new SelectList(Db.Terms.AsNoTracking(), "TermName", "TermName");

        //    return View(assignedList.ToPagedList(pageNumber, pageSize));
        //    //return View(await Db.AssignSubjects.ToListAsync());
        //}

        public async Task<ActionResult> Index()
        {
            ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            ViewBag.TermName = new SelectList(Db.Terms.AsNoTracking(), "TermName", "TermName");
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
            var className = string.Empty;


            var v = Db.AssignSubjects.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool))
                                .Select(s => new
                                {
                                    s.AssignSubjectId,
                                    s.ClassName,
                                    s.Subject.SubjectName,
                                    s.TermName
                                }).ToList();
            if (Request.IsAuthenticated && User.IsInRole("Student"))
            {
                var term = await Db.Terms.Where(x => x.ActiveTerm.Equals(true)).Select(x => x.TermName).FirstOrDefaultAsync();
                var session = await Db.Sessions.Where(x => x.ActiveSession.Equals(true)).Select(x => x.SessionName).FirstOrDefaultAsync();
                var studentId = User.Identity.GetUserId();
                var myClass = await Db.AssignedClasses.AsNoTracking().Where(x => x.TermName.Equals(term) && x.SessionName.Equals(session)
                                            && x.StudentId.Equals(studentId) && x.SchoolId.Equals(userSchool))
                    .Select(s => s.ClassName)
                    .FirstOrDefaultAsync();
                className = myClass;
                v = v.Where(x => x.ClassName.Equals(className)).ToList();
            }


            if (!string.IsNullOrEmpty(search))
            {
                //v = v.OrderBy(sortColumn + " " + sortColumnDir);
                v = Db.AssignSubjects.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool) && (x.Subject.SubjectName.Equals(search) || x.Class.ClassName.Equals(search)))
                    .Select(s => new { s.AssignSubjectId, s.ClassName, s.Subject.SubjectName, s.TermName }).ToList();
            }
            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
            #endregion

            //return Json(new { data = await Db.Subjects.AsNoTracking().Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToListAsync() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MyAssignedSubject()
        {
            return View();
        }

        public async Task<ActionResult> GetStudentIndex()
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
            var className = string.Empty;
            var term = await Db.Terms.Where(x => x.ActiveTerm.Equals(true)).Select(x => x.TermName).FirstOrDefaultAsync();
            var session = await Db.Sessions.Where(x => x.ActiveSession.Equals(true)).Select(x => x.SessionName).FirstOrDefaultAsync();
            var studentId = User.Identity.GetUserId();

            var v = Db.AssignSubjects.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool)
                                    && x.ClassName.Equals(className) && x.TermName.Equals(term))
                                    .Select(s => new
                                    {
                                        s.AssignSubjectId,
                                        s.ClassName,
                                        s.Subject.SubjectName,
                                        s.TermName
                                    }).ToList();
            if (Request.IsAuthenticated && User.IsInRole("Student"))
            {

                var myClass = await Db.AssignedClasses.AsNoTracking().Where(x => x.TermName.Equals(term) && x.SessionName.Equals(session)
                                                                                 && x.StudentId.Equals(studentId) && x.SchoolId.Equals(userSchool))
                    .Select(s => s.ClassName)
                    .FirstOrDefaultAsync();
                className = myClass;
                v = v.Where(x => x.ClassName.Equals(className) && x.TermName.Equals(term)).ToList();

            }

            if (!string.IsNullOrEmpty(search))
            {
                //v = v.OrderBy(sortColumn + " " + sortColumnDir);
                v = Db.AssignSubjects.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool) && (x.Subject.SubjectName.Equals(search) || x.Class.ClassName.Equals(search)))
                    .Select(s => new { s.AssignSubjectId, s.ClassName, s.Subject.SubjectName, s.TermName }).ToList();
            }
            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
            #endregion

            //return Json(new { data = await Db.Subjects.AsNoTracking().Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToListAsync() }, JsonRequestBehavior.AllowGet);
        }



        public async Task<PartialViewResult> Save(int id)
        {
            var assignSubject = await Db.AssignSubjects.FindAsync(id);
            ViewBag.SubjectId = new MultiSelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
            ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            ViewBag.TermName = new MultiSelectList(Db.Terms.AsNoTracking(), "TermName", "TermName");
            return PartialView(assignSubject);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(AssignSubjectViewModel model)
        {
            bool status = false;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                int counter = 0;
                string theClass = String.Empty;
                if (model.AssignSubjectId > 0)
                {
                    var assignSubject = await Db.AssignSubjects.FindAsync(model.AssignSubjectId);
                    if (assignSubject != null)
                    {
                        assignSubject.SchoolId = userSchool;
                        Db.Entry(assignSubject).State = EntityState.Modified;
                    }
                    message = "Class Updated Successfully...";
                }
                else
                {
                    foreach (var term in model.TermName)
                    {
                        foreach (var item in model.SubjectId)
                        {
                            var CA = Db.AssignSubjects.Where(x => x.ClassName.Equals(model.ClassName)
                                                                  && x.SubjectId.Equals(item) &&
                                                                  x.SchoolId.Equals(userSchool));
                            var countFromDb = await CA.CountAsync();
                            if (countFromDb >= 1)
                            {
                                message = $"Admin have already assigned {item} subject to {model.ClassName} Class";
                                return new JsonResult { Data = new { status = false, message = message } };
                            }
                            var assigSubject = new AssignSubject
                            {
                                ClassName = model.ClassName,
                                SubjectId = item,
                                TermName = term,
                                SchoolId = userSchool
                            };
                            Db.AssignSubjects.Add(assigSubject);
                            counter += 1;
                            theClass = model.ClassName;
                        }
                    }
                    await Db.SaveChangesAsync();
                    message = $" You have Assigned {counter} Subject(s)  to {theClass} Successfully.";

                    return new JsonResult { Data = new { status = true, message = message } };
                }
            }
            return new JsonResult { Data = new { status = status, message = message } };
            //return View(subject);
        }


        // GET: AssignSubjects/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var assignSubject = await Db.AssignSubjects.FindAsync(id);
            if (assignSubject == null)
            {
                return HttpNotFound();
            }
            return View(assignSubject);
        }

        // GET: AssignSubjects/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.SubjectId = new MultiSelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
            ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            ViewBag.TermName = new MultiSelectList(Db.Terms.AsNoTracking(), "TermName", "TermName");
            return View();
        }

        // POST: AssignSubjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AssignSubjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                int counter = 0;
                string theClass = String.Empty;
                if (model.SubjectId != null)
                {
                    foreach (var term in model.TermName)
                    {
                        foreach (var item in model.SubjectId)
                        {
                            var CA = Db.AssignSubjects.Where(x => x.ClassName.Equals(model.ClassName)
                                                                  && x.SubjectId.Equals(item) &&
                                                                  x.SchoolId.Equals(userSchool));
                            var countFromDb = await CA.CountAsync();
                            if (countFromDb >= 1)
                            {
                                TempData["UserMessage"] =
                                    $"Admin have already assigned {item} subject to {model.ClassName} Class";
                                TempData["Title"] = "Error.";
                                ViewBag.SubjectId = new MultiSelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
                                ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool),
                                    "FullClassName", "FullClassName");
                                ViewBag.TermName = new MultiSelectList(Db.Terms.AsNoTracking(), "TermName", "TermName");
                                return View(model);
                            }
                            var assigSubject = new AssignSubject
                            {
                                ClassName = model.ClassName,
                                SubjectId = item,
                                TermName = term,
                                SchoolId = userSchool
                            };
                            Db.AssignSubjects.Add(assigSubject);
                            counter += 1;
                            theClass = model.ClassName;
                        }
                    }
                    await Db.SaveChangesAsync();

                    TempData["UserMessage"] = $" You have Assigned {counter} Subject(s)  to {theClass} Successfully.";
                    TempData["Title"] = "Success.";
                    return RedirectToAction("Index", "AssignSubjects");
                }
            }

            ViewBag.SubjectId = new MultiSelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
            ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            ViewBag.TermName = new MultiSelectList(Db.Terms.AsNoTracking(), "TermName", "TermName");
            return View(model);
        }

        // GET: AssignSubjects/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignSubject assignSubject = await Db.AssignSubjects.FindAsync(id);
            if (assignSubject == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubjectId = new MultiSelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
            ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            ViewBag.TermName = new MultiSelectList(Db.Terms.AsNoTracking(), "TermName", "TermName");
            return View(assignSubject);
        }

        // POST: AssignSubjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AssignSubjectId,ClassName,SubjectName")] AssignSubject assignSubject)
        {
            if (ModelState.IsValid)
            {
                assignSubject.SchoolId = userSchool;
                Db.Entry(assignSubject).State = EntityState.Modified;
                await Db.SaveChangesAsync();

                TempData["UserMessage"] = "Assigned Subject Updated Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            ViewBag.SubjectId = new MultiSelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
            ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            ViewBag.TermName = new MultiSelectList(Db.Terms.AsNoTracking(), "TermName", "TermName");
            return View(assignSubject);
        }



        public async Task<PartialViewResult> Delete(int id)
        {
            var assignSubject = await Db.AssignSubjects.FindAsync(id);
            return PartialView(assignSubject);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            bool status = false;
            string message = string.Empty;
            var assignSubject = await Db.AssignSubjects.FindAsync(id);
            if (assignSubject != null)
            {
                Db.AssignSubjects.Remove(assignSubject);
                await Db.SaveChangesAsync();
                status = true;
                message = "Subject Deleted from Class Successfully...";
            }

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
