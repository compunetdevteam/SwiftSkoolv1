using System;
using SwiftSkool.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkool.Controllers
{
    public class AssignFormTeacherToClassesController : BaseController
    {
        // GET: AssignFormTeacherToClasses
        public async Task<ActionResult> Index()
        {
            if (Request.IsAuthenticated && !User.IsInRole("SuperAdmin"))
            {
                return View(await Db.AssignFormTeacherToClasses.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool)).ToListAsync());
            }
            return View(await Db.AssignFormTeacherToClasses.AsNoTracking().ToListAsync());
        }

        public async Task<PartialViewResult> AddFormTeacher(string studentId)
        {
            //ViewBag.Username = studentId;
            ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            ViewBag.Username = new SelectList(await _query.StaffListAsync(userSchool), "Username", "Username");
            return PartialView("AddFormTeacher");
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

            var v = Db.AssignFormTeacherToClasses.Where(x => x.SchoolId.Equals(userSchool)).Select(s => new { s.Id,s.Username, s.ClassName }).ToList();
            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            //{
            //    //v = v.OrderBy(sortColumn + " " + sortColumnDir);
            //    v = new List<Subject>(v.OrderBy(x => "sortColumn + \" \" + sortColumnDir"));
            //}
            if (!string.IsNullOrEmpty(search))
            {
                //v = v.OrderBy(sortColumn + " " + sortColumnDir);
                v = Db.AssignFormTeacherToClasses.Where(x => x.SchoolId.Equals(userSchool) && (x.ClassName.Equals(search) || x.Username.Equals(search)))
                    .Select(s => new { s.Id, s.Username, s.ClassName }).ToList();
            }
            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
            #endregion

            //return Json(new { data = await Db.Subjects.AsNoTracking().Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToListAsync() }, JsonRequestBehavior.AllowGet);
        }



        public async Task<PartialViewResult> Save(int id)
        {
            var assignFormTeacherToClasses = await Db.AssignFormTeacherToClasses.FindAsync(id);
            return PartialView(assignFormTeacherToClasses);
        }

        // POST: Subjects/Save/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(AssignFormTeacherToClass assignFormTeacherToClasses)
        {
            bool status = false;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (assignFormTeacherToClasses.Id > 0)
                {
                    assignFormTeacherToClasses.SchoolId = userSchool;
                    Db.Entry(assignFormTeacherToClasses).State = EntityState.Modified;
                    message = "Class Updated Successfully...";
                }
                else
                {
                    assignFormTeacherToClasses.SchoolId = userSchool;
                    Db.AssignFormTeacherToClasses.Add(assignFormTeacherToClasses);
                    message = "Class Created Successfully...";
                }
                await Db.SaveChangesAsync();
                status = true;
            }
            return new JsonResult { Data = new { status = status, message = message } };
            //return View(subject);
        }


        // GET: AssignFormTeacherToClasses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignFormTeacherToClass assignFormTeacherToClass = await Db.AssignFormTeacherToClasses.FindAsync(id);
            if (assignFormTeacherToClass == null)
            {
                return HttpNotFound();
            }
            return View(assignFormTeacherToClass);
        }

        // GET: AssignFormTeacherToClasses/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            ViewBag.Username = new SelectList(await _query.StaffListAsync(userSchool), "Username", "Username");
            return View();
        }

        // POST: AssignFormTeacherToClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ClassName,Username")] AssignFormTeacherToClass assignFormTeacherToClass)
        {
            if (ModelState.IsValid)
            {
                Db.AssignFormTeacherToClasses.Add(assignFormTeacherToClass);
                await Db.SaveChangesAsync();

                return RedirectToAction("Index", "Staffs");
            }

            ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            ViewBag.Username = new SelectList(await _query.StaffListAsync(userSchool), "Username", "Username");
            return View(assignFormTeacherToClass);
        }

        // GET: AssignFormTeacherToClasses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignFormTeacherToClass assignFormTeacherToClass = await Db.AssignFormTeacherToClasses.FindAsync(id);
            if (assignFormTeacherToClass == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            ViewBag.Username = new SelectList(await _query.StaffListAsync(userSchool), "Username", "Username");
            return View(assignFormTeacherToClass);
        }

        // POST: AssignFormTeacherToClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ClassName,StaffName")] AssignFormTeacherToClass assignFormTeacherToClass)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(assignFormTeacherToClass).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            ViewBag.Username = new SelectList(await _query.StaffListAsync(userSchool), "Username", "Username");
            return View(assignFormTeacherToClass);
        }

        // GET: AssignFormTeacherToClasses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var assignFormTeacherToClass = await Db.AssignFormTeacherToClasses.FindAsync(id);
            if (assignFormTeacherToClass == null)
            {
                return HttpNotFound();
            }
            return View(assignFormTeacherToClass);
        }

        // POST: AssignFormTeacherToClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AssignFormTeacherToClass assignFormTeacherToClass = await Db.AssignFormTeacherToClasses.FindAsync(id);
            if (assignFormTeacherToClass != null) Db.AssignFormTeacherToClasses.Remove(assignFormTeacherToClass);
            await Db.SaveChangesAsync();
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
