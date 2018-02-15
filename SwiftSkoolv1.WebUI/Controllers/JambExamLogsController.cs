using SwiftSkoolv1.Domain.JambPractice;
using SwiftSkoolv1.WebUI.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class JambExamLogsController : BaseController
    {
        private SwiftSkoolDbContext Db = new SwiftSkoolDbContext();

        // GET: JambExamLogs
        public async Task<ActionResult> Index()
        {
            var jambExamLogs = Db.JambExamLogs.Include(j => j.JambSubject).Include(j => j.Student);
            return View(await jambExamLogs.ToListAsync());
        }

        // GET: JambExamLogs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JambExamLog jambExamLog = await Db.JambExamLogs.FindAsync(id);
            if (jambExamLog == null)
            {
                return HttpNotFound();
            }
            return View(jambExamLog);
        }

        // GET: JambExamLogs/Create
        public ActionResult Create()
        {
            ViewBag.JambSubjectId = new SelectList(Db.JambSubjects, "JambSubjectId", "SubjectCode");
            ViewBag.StudentId = new SelectList(Db.Students, "StudentId", "FirstName");
            return View();
        }

        // POST: JambExamLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "JambExamLogId,StudentId,JambSubjectId,Score,TotalScore,ExamTaken")] JambExamLog jambExamLog)
        {
            if (ModelState.IsValid)
            {
                Db.JambExamLogs.Add(jambExamLog);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.JambSubjectId = new SelectList(Db.JambSubjects, "JambSubjectId", "SubjectCode", jambExamLog.JambSubjectId);
            ViewBag.StudentId = new SelectList(Db.Students, "StudentId", "FirstName", jambExamLog.StudentId);
            return View(jambExamLog);
        }

        // GET: JambExamLogs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JambExamLog jambExamLog = await Db.JambExamLogs.FindAsync(id);
            if (jambExamLog == null)
            {
                return HttpNotFound();
            }
            ViewBag.JambSubjectId = new SelectList(Db.JambSubjects, "JambSubjectId", "SubjectCode", jambExamLog.JambSubjectId);
            ViewBag.StudentId = new SelectList(Db.Students, "StudentId", "FirstName", jambExamLog.StudentId);
            return View(jambExamLog);
        }

        // POST: JambExamLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "JambExamLogId,StudentId,JambSubjectId,Score,TotalScore,ExamTaken")] JambExamLog jambExamLog)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(jambExamLog).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.JambSubjectId = new SelectList(Db.JambSubjects, "JambSubjectId", "SubjectCode", jambExamLog.JambSubjectId);
            ViewBag.StudentId = new SelectList(Db.Students, "StudentId", "FirstName", jambExamLog.StudentId);
            return View(jambExamLog);
        }

        // GET: JambExamLogs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JambExamLog jambExamLog = await Db.JambExamLogs.FindAsync(id);
            if (jambExamLog == null)
            {
                return HttpNotFound();
            }
            return View(jambExamLog);
        }

        // POST: JambExamLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            JambExamLog jambExamLog = await Db.JambExamLogs.FindAsync(id);
            Db.JambExamLogs.Remove(jambExamLog);
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
