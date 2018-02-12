using SwiftSkoolv1.Domain.ClassRoom;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class LessonNotesController : BaseController
    {

        // GET: LessonNotes
        public async Task<ActionResult> Index()
        {
            var lessonNotes = Db.LessonNotes.Include(l => l.Topic);
            return View(await lessonNotes.ToListAsync());
        }

        // GET: LessonNotes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LessonNote lessonNote = await Db.LessonNotes.FindAsync(id);
            if (lessonNote == null)
            {
                return HttpNotFound();
            }
            return View(lessonNote);
        }

        // GET: LessonNotes/Create
        public ActionResult Create()
        {
            ViewBag.TopicId = new SelectList(Db.Topics, "TopicId", "TopicName");
            return View();
        }

        // POST: LessonNotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(LessonNote lessonNote)
        {
            if (ModelState.IsValid)
            {
                lessonNote.SchoolId = userSchool;
                Db.LessonNotes.Add(lessonNote);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TopicId = new SelectList(Db.Topics, "TopicId", "TopicName", lessonNote.TopicId);
            return View(lessonNote);
        }

        // GET: LessonNotes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LessonNote lessonNote = await Db.LessonNotes.FindAsync(id);
            if (lessonNote == null)
            {
                return HttpNotFound();
            }
            ViewBag.TopicId = new SelectList(Db.Topics, "TopicId", "TopicName", lessonNote.TopicId);
            return View(lessonNote);
        }

        // POST: LessonNotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(LessonNote lessonNote)
        {
            if (ModelState.IsValid)
            {
                lessonNote.SchoolId = userSchool;
                Db.Entry(lessonNote).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TopicId = new SelectList(Db.Topics, "TopicId", "TopicName", lessonNote.TopicId);
            return View(lessonNote);
        }

        // GET: LessonNotes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LessonNote lessonNote = await Db.LessonNotes.FindAsync(id);
            if (lessonNote == null)
            {
                return HttpNotFound();
            }
            return View(lessonNote);
        }

        // POST: LessonNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            LessonNote lessonNote = await Db.LessonNotes.FindAsync(id);
            Db.LessonNotes.Remove(lessonNote);
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
