using SwiftSkoolv1.Domain;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class TermsController : BaseController
    {
        // GET: Terms
        public async Task<ActionResult> Index()
        {
            return View(await Db.Terms.ToListAsync());
        }

        public ActionResult GetIndex()
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

            var v = Db.Terms.AsNoTracking().ToList();


            if (!string.IsNullOrEmpty(search))
            {
                //v = v.OrderBy(sortColumn + " " + sortColumnDir);
                v = Db.Terms.AsNoTracking().Where(x => x.TermName.ToUpper().Equals(search.ToUpper())).ToList();
            }
            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
            #endregion

            //return Json(new { data = await Db.Subjects.AsNoTracking().Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToListAsync() }, JsonRequestBehavior.AllowGet);
        }


        public async Task<PartialViewResult> Save(int id)
        {
            var terms = await Db.Terms.FindAsync(id);
            return PartialView(terms);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(Term model)
        {
            bool status = false;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (model.TermId > 0)
                {
                    model.TermName = model.TermName.ToUpper();
                    Db.Entry(model).State = EntityState.Modified;
                    message = "Term Updated Successfully...";
                }
                else
                {
                    model.TermName = model.TermName.ToUpper();
                    Db.Terms.Add(model);
                    message = "Term Created Successfully...";
                }
                await Db.SaveChangesAsync();
                return new JsonResult { Data = new { status = true, message = message } };
            }
            return new JsonResult { Data = new { status = false, message = "Something went wrong" } };
            //return View(subject);
        }

        // GET: Terms/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Term term = await Db.Terms.FindAsync(id);
            if (term == null)
            {
                return HttpNotFound();
            }
            return View(term);
        }

        // GET: Terms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Terms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TermId,TermName,ActiveTerm")] Term term)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var myTerm = new Term()
                    {
                        TermName = term.TermName.Trim(),
                        ActiveTerm = term.ActiveTerm
                    };
                    Db.Terms.Add(myTerm);
                }
                catch (Exception e)
                {
                    TempData["UserMessage"] = $"Term Already Exist in Database {e.Message}";
                    TempData["Title"] = "Error.";
                    return View(term);
                }
                await Db.SaveChangesAsync();
                TempData["UserMessage"] = "Term Created Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }

            return View(term);
        }

        // GET: Terms/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Term term = await Db.Terms.FindAsync(id);
            if (term == null)
            {
                return HttpNotFound();
            }
            return View(term);
        }

        // POST: Terms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TermId,TermName,ActiveTerm")] Term term)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(term).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                TempData["UserMessage"] = "Term Updated Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            return View(term);
        }


        public async Task<PartialViewResult> Delete(int id)
        {
            var term = await Db.Terms.FindAsync(id);
            return PartialView(term);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            bool status = false;
            string message = string.Empty;
            var term = await Db.Terms.FindAsync(id);
            if (term != null)
            {
                Db.Terms.Remove(term);
                await Db.SaveChangesAsync();
                status = true;
                message = "Term Deleted Successfully...";
                return new JsonResult { Data = new { status = status, message = message } };
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
