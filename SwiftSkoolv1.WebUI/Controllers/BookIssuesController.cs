using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SwiftSkoolv1.Domain;
using SwiftSkoolv1.WebUI.Models;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class BookIssuesController : BaseController
    {
        private SwiftSkoolDbContext db = new SwiftSkoolDbContext();

        // GET: BookIssues
        public async Task<ActionResult> Index()
        {
            return View(await db.BookIssues.ToListAsync());
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
            var v = Db.BookIssues.Where(x => x.SchoolId == userSchool).Select(s => new {s.BookIssueId, s.StudentId, s.AccessionNo, s.IssueDate, s.DueDate,s.Status }).ToList();

            //var v = Db.Subjects.Where(x => x.SchoolId.Equals(userSchool)).Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToList();
            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            //{
            //    //v = v.OrderBy(sortColumn + " " + sortColumnDir);
            //    v = new List<Subject>(v.OrderBy(x => "sortColumn + \" \" + sortColumnDir"));
            //}
            if (!string.IsNullOrEmpty(search))
            {
                //v = v.OrderBy(sortColumn + " " + sortColumnDir);
                v = Db.BookIssues.Where(x => x.SchoolId.Equals(userSchool) && (x.StudentId.Equals(search) || x.AccessionNo.Equals(search) || x.IssueDate.Equals(search) || x.DueDate.Equals(search) || x.Status.Equals(search)))
                                          .Select(s => new { s.BookIssueId,  s.StudentId, s.AccessionNo, s.IssueDate, s.DueDate, s.Status }).ToList();
            }
            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
            #endregion

            //return Json(new { data = await Db.Subjects.AsNoTracking().Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToListAsync() }, JsonRequestBehavior.AllowGet);
        }



        // GET: BookIssue/Save/5
        public async Task<PartialViewResult> Save(int id)
        {
            var bookIssues = await Db.BookIssues.FindAsync(id);
            return PartialView(bookIssues);
        }

        // POST: Subjects/Save/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(BookIssue bookIssue)
        {
            bool status = false;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (bookIssue.BookIssueId > 0)
                {
                    bookIssue.SchoolId = userSchool;
                    Db.Entry(bookIssue).State = EntityState.Modified;
                    message = "Book Issue Updated Successfully...";
                }
                else
                {
                    bookIssue.SchoolId = userSchool;
                    Db.BookIssues.Add(bookIssue);
                    message = "Book Issue Created Successfully...";

                }
                await Db.SaveChangesAsync();
                status = true;
            }
            return new JsonResult { Data = new { status = status, message = message } };
            //return View(subject);
        }



        // GET: BookIssues/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookIssue bookIssue = await db.BookIssues.FindAsync(id);
            if (bookIssue == null)
            {
                return HttpNotFound();
            }
            return View(bookIssue);
        }

        // GET: BookIssues/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BookIssues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BookIssueId,StudentId,AccessionNo,IssueDate,DueDate,Status,SchoolId")] BookIssue bookIssue)
        {
            if (ModelState.IsValid)
            {
                db.BookIssues.Add(bookIssue);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(bookIssue);
        }

        // GET: BookIssues/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookIssue bookIssue = await db.BookIssues.FindAsync(id);
            if (bookIssue == null)
            {
                return HttpNotFound();
            }
            return View(bookIssue);
        }

        // POST: BookIssues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BookIssueId,StudentId,AccessionNo,IssueDate,DueDate,Status,SchoolId")] BookIssue bookIssue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookIssue).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(bookIssue);
        }

        public async Task<PartialViewResult> Delete(int id)
        {
            BookIssue bookIssue = await Db.BookIssues.FindAsync(id);

            return PartialView(bookIssue);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            bool status = false;
            string message = string.Empty;
            var bookIssue = await Db.BookIssues.FindAsync(id);
            if (bookIssue != null)
            {
                Db.BookIssues.Remove(bookIssue);
                await Db.SaveChangesAsync();
                status = true;
                message = "BookIssue Deleted Successfully...";
            }

            return new JsonResult { Data = new { status = status, message = message } };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
