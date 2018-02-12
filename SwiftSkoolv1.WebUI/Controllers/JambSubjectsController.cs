using SwiftSkoolv1.Domain.JambPractice;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class JambSubjectsController : BaseController
    {
        // GET: JambSubjects
        public async Task<ActionResult> Index()
        {
            return View(await Db.JambSubjects.ToListAsync());
        }

        // GET: JambSubjects/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JambSubject jambSubject = await Db.JambSubjects.FindAsync(id);
            if (jambSubject == null)
            {
                return HttpNotFound();
            }
            return View(jambSubject);
        }

        // GET: JambSubjects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: JambSubjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "JambSubjectId,SubjectCode,SubjectName")] JambSubject jambSubject)
        {
            if (ModelState.IsValid)
            {
                Db.JambSubjects.Add(jambSubject);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(jambSubject);
        }

        // GET: JambSubjects/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JambSubject jambSubject = await Db.JambSubjects.FindAsync(id);
            if (jambSubject == null)
            {
                return HttpNotFound();
            }
            return View(jambSubject);
        }

        // POST: JambSubjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "JambSubjectId,SubjectCode,SubjectName")] JambSubject jambSubject)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(jambSubject).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(jambSubject);
        }

        // GET: JambSubjects/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JambSubject jambSubject = await Db.JambSubjects.FindAsync(id);
            if (jambSubject == null)
            {
                return HttpNotFound();
            }
            return View(jambSubject);
        }

        // POST: JambSubjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            JambSubject jambSubject = await Db.JambSubjects.FindAsync(id);
            Db.JambSubjects.Remove(jambSubject);
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
