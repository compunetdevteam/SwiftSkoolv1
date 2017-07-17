using SwiftSkoolv1.Domain;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class BooksController : BaseController
    {
        // GET: book
        public async Task<ActionResult> Index()
        {
            var book = Db.Books.Include(b => b.BookIssue);
            return View(await book.ToListAsync());
        }

        // GET: book/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var book = await Db.Books.FindAsync(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: book/Create
        public ActionResult Create()
        {
            // ViewBag.BookId = new SelectList(Db.BookIssues, "BookId", "StudentId");
            return View();
        }

        // POST: book/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AccessionNo,BookId,Title,Author,JointAuthor,Subject,ISBN,Edition,Publisher,PlaceOfPublish")] Book book)
        {
            if (ModelState.IsValid)
            {
                Db.Books.Add(book);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }/*C:\Users\DEV-PC\Source\Repos\SwiftSkool\HopeAcademySMS\Controllers\BooksController.cs*/

            ViewBag.BookId = new SelectList(Db.BookIssues, "BookId", "StudentId", book.BookId);
            return View(book);
        }

        // GET: book/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var book = await Db.Books.FindAsync(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookId = new SelectList(Db.BookIssues, "BookId", "StudentId", book.BookId);
            return View(book);
        }

        // POST: book/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AccessionNo,BookId,Title,Author,JointAuthor,Subject,ISBN,Edition,Publisher,PlaceOfPublish")] Book book)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(book).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BookId = new SelectList(Db.BookIssues, "BookId", "StudentId");
            return View(book);
        }

        // GET: book/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var book = await Db.Books.FindAsync(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var book = await Db.Books.FindAsync(id);
            Db.Books.Remove(book);
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
