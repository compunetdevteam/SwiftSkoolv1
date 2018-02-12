using SwiftSkoolv1.Domain.ClassRoom;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class TopicsController : BaseController
    {
        // GET: Topics
        public async Task<ActionResult> Index()
        {
            var topics = Db.Topics.Include(t => t.LessonNote).Include(t => t.Module);
            return View(await topics.ToListAsync());
        }

        // GET: Topics/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = await Db.Topics.FindAsync(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // GET: Topics/Create
        public ActionResult Create()
        {
            ViewBag.TopicId = new SelectList(Db.LessonNotes, "TopicId", "Note");
            ViewBag.ModuleId = new SelectList(Db.Modules, "ModuleId", "ModuleName");
            return View();
        }

        // POST: Topics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Topic topic)
        {
            if (ModelState.IsValid)
            {
                topic.SchoolId = userSchool;
                Db.Topics.Add(topic);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TopicId = new SelectList(Db.LessonNotes, "TopicId", "Note", topic.TopicId);
            ViewBag.ModuleId = new SelectList(Db.Modules, "ModuleId", "ModuleName", topic.ModuleId);
            return View(topic);
        }

        // GET: Topics/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = await Db.Topics.FindAsync(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            ViewBag.TopicId = new SelectList(Db.LessonNotes, "TopicId", "Note", topic.TopicId);
            ViewBag.ModuleId = new SelectList(Db.Modules, "ModuleId", "ModuleName", topic.ModuleId);
            return View(topic);
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Topic topic)
        {
            if (ModelState.IsValid)
            {
                topic.SchoolId = userSchool;
                Db.Entry(topic).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TopicId = new SelectList(Db.LessonNotes, "TopicId", "Note", topic.TopicId);
            ViewBag.ModuleId = new SelectList(Db.Modules, "ModuleId", "ModuleName", topic.ModuleId);
            return View(topic);
        }

        // GET: Topics/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = await Db.Topics.FindAsync(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Topic topic = await Db.Topics.FindAsync(id);
            Db.Topics.Remove(topic);
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
