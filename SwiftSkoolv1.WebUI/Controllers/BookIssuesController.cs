using SwiftSkoolv1.Domain;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class BookIssuesController : BaseController
    {
        // GET: BookIssues
        public async Task<ActionResult> Index()
        {
            return View(await Db.BookIssues.ToListAsync());
        }

        // GET: BookIssues/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var bookIssue = await Db.BookIssues.FindAsync(id);
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
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,StudentId,AccessionNo,IssueDate,DueDate,Status")] BookIssue bookIssue)
        {
            if (ModelState.IsValid)
            {
                Db.BookIssues.Add(bookIssue);
                await Db.SaveChangesAsync();
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
            BookIssue bookIssue = await Db.BookIssues.FindAsync(id);
            if (bookIssue == null)
            {
                return HttpNotFound();
            }
            return View(bookIssue);
        }

        // POST: BookIssues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,StudentId,AccessionNo,IssueDate,DueDate,Status")] BookIssue bookIssue)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(bookIssue).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(bookIssue);
        }

        // GET: BookIssues/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var bookIssue = await Db.BookIssues.FindAsync(id);
            if (bookIssue == null)
            {
                return HttpNotFound();
            }
            return View(bookIssue);
        }

        // POST: BookIssues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var bookIssue = await Db.BookIssues.FindAsync(id);
            if (bookIssue != null) Db.BookIssues.Remove(bookIssue);
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
