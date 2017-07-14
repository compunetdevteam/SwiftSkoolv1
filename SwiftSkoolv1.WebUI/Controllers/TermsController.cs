using SwiftSkool.Models;
using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkool.Controllers
{
    public class TermsController : BaseController
    {
        // GET: Terms
        public async Task<ActionResult> Index()
        {
            return View(await Db.Terms.ToListAsync());
        }

        // GET: TermsIndex
        public async Task<ActionResult> TermsIndex()
        {
            return View();
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

        // GET: Terms/Delete/5
        public async Task<ActionResult> Delete(int? id)
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

        // POST: Terms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Term term = await Db.Terms.FindAsync(id);
            if (term != null) Db.Terms.Remove(term);
            await Db.SaveChangesAsync();
            TempData["UserMessage"] = "Term Deleted Successful";
            TempData["Title"] = "Error.";
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
