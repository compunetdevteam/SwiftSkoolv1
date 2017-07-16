using SwiftSkoolv1.Domain;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class SubjectsController : BaseController
    {
        // GET: Subjects
        public async Task<ActionResult> Index()
        {
            //if (Request.IsAuthenticated && !User.IsInRole("SuperAdmin"))
            //{
            //    return View(Db.Subjects.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool)));
            //}
            //return View(await Db.Subjects.AsNoTracking().ToListAsync());
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
            var v = Db.Subjects.Where(x => x.SchoolId == userSchool).Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToList();

            //var v = Db.Subjects.Where(x => x.SchoolId.Equals(userSchool)).Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToList();
            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            //{
            //    //v = v.OrderBy(sortColumn + " " + sortColumnDir);
            //    v = new List<Subject>(v.OrderBy(x => "sortColumn + \" \" + sortColumnDir"));
            //}
            if (!string.IsNullOrEmpty(search))
            {
                //v = v.OrderBy(sortColumn + " " + sortColumnDir);
                v = Db.Subjects.Where(x => x.SchoolId.Equals(userSchool) && (x.SubjectName.Equals(search) || x.SubjectCode.Equals(search)))
                                    .Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToList();
            }
            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
            #endregion

            //return Json(new { data = await Db.Subjects.AsNoTracking().Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToListAsync() }, JsonRequestBehavior.AllowGet);
        }

        // GET: Subjects/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = await Db.Subjects.FindAsync(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // GET: Subjects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SubjectId,SubjectCode,SubjectName")] Subject subject)
        {

            if (ModelState.IsValid)
            {

                string message = string.Empty;
                var mysubject = Db.Subjects.Where(x => x.SchoolId.Equals(userSchool) &&
                                                    x.SubjectCode.ToUpper().Trim().Equals(subject.SubjectCode.ToUpper().Trim()));

                if (mysubject.Any())
                {
                    message = "Duplicate Subject code";
                    return new JsonResult { Data = new { status = false, message = message } };
                }

                subject.SchoolId = userSchool;
                Db.Subjects.Add(subject);
                await Db.SaveChangesAsync();

                message = "Subject Created Successfully.";
                return new JsonResult { Data = new { status = true, message = message } };
                //TempData["UserMessage"] = "Subject Created Successfully.";
                //TempData["Title"] = "Success.";
                //return RedirectToAction("Index");
            }

            return new JsonResult { Data = new { status = false, message = "Not Saved" } };
        }

        // GET: Subjects/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var subject = await Db.Subjects.FindAsync(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SubjectId,SubjectCode,SubjectName")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                subject.SchoolId = userSchool;
                Db.Entry(subject).State = EntityState.Modified;
                await Db.SaveChangesAsync();

                TempData["UserMessage"] = "Subject Saved Updated Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            return View(subject);
        }

        // GET: Subjects/Save/5
        public async Task<PartialViewResult> Save(int id)
        {
            var subject = await Db.Subjects.FindAsync(id);
            return PartialView(subject);
        }

        // POST: Subjects/Save/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(Subject subject)
        {
            bool status = false;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (subject.SubjectId > 0)
                {
                    subject.SchoolId = userSchool;
                    Db.Entry(subject).State = EntityState.Modified;
                    message = "Subject Updated Successfully...";
                }
                else
                {
                    subject.SchoolId = userSchool;
                    Db.Subjects.Add(subject);
                    message = "Subject Created Successfully...";

                }
                await Db.SaveChangesAsync();
                status = true;
            }
            return new JsonResult { Data = new { status = status, message = message } };
            //return View(subject);
        }

        // GET: Subjects/Delete/5
        public async Task<PartialViewResult> Delete(int id)
        {

            Subject subject = await Db.Subjects.FindAsync(id);

            return PartialView(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            bool status = false;
            string message = string.Empty;
            var subject = await Db.Subjects.FindAsync(id);
            if (subject != null)
            {
                Db.Subjects.Remove(subject);
                await Db.SaveChangesAsync();
                status = true;
                message = "Subject Deleted Successfully...";
            }

            return new JsonResult { Data = new { status = status, message = message } };
        }

        //// GET: Subjects/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Subject subject = await Db.Subjects.FindAsync(id);
        //    if (subject == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(subject);
        //}

        //// POST: Subjects/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    var subject = await Db.Subjects.FindAsync(id);
        //    if (subject != null) Db.Subjects.Remove(subject);
        //    await Db.SaveChangesAsync();
        //    TempData["UserMessage"] = "Subject Deleted Successfully.";
        //    TempData["Title"] = "Deleted.";
        //    return RedirectToAction("Index");
        //}

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
