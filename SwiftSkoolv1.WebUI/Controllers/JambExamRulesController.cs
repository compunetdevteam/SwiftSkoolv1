using SwiftSkoolv1.Domain.JambPractice;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class JambExamRulesController : BaseController
    {
        // GET: JambExamRules
        public async Task<ActionResult> Index()
        {
            var jambExamRules = Db.JambExamRules.Include(j => j.JambSubject);
            return View(await jambExamRules.ToListAsync());
        }

        // GET: JambExamRules/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JambExamRule jambExamRule = await Db.JambExamRules.FindAsync(id);
            if (jambExamRule == null)
            {
                return HttpNotFound();
            }
            return View(jambExamRule);
        }

        // GET: JambExamRules/Create
        public ActionResult Create()
        {
            ViewBag.JambSubjectId = new SelectList(Db.JambSubjects, "JambSubjectId", "SubjectName");
            return View();
        }

        // POST: JambExamRules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(JambExamRule jambExamRule)
        {
            if (ModelState.IsValid)
            {
                Db.JambExamRules.Add(jambExamRule);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.JambSubjectId = new SelectList(Db.JambSubjects, "JambSubjectId", "SubjectName", jambExamRule.JambSubjectId);
            return View(jambExamRule);
        }

        // GET: JambExamRules/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JambExamRule jambExamRule = await Db.JambExamRules.FindAsync(id);
            if (jambExamRule == null)
            {
                return HttpNotFound();
            }
            ViewBag.JambSubjectId = new SelectList(Db.JambSubjects, "JambSubjectId", "SubjectName", jambExamRule.JambSubjectId);
            return View(jambExamRule);
        }

        // POST: JambExamRules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(JambExamRule jambExamRule)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(jambExamRule).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.JambSubjectId = new SelectList(Db.JambSubjects, "JambSubjectId", "SubjectName", jambExamRule.JambSubjectId);
            return View(jambExamRule);
        }

        // GET: JambExamRules/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JambExamRule jambExamRule = await Db.JambExamRules.FindAsync(id);
            if (jambExamRule == null)
            {
                return HttpNotFound();
            }
            return View(jambExamRule);
        }

        // POST: JambExamRules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            JambExamRule jambExamRule = await Db.JambExamRules.FindAsync(id);
            if (jambExamRule != null) Db.JambExamRules.Remove(jambExamRule);
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
