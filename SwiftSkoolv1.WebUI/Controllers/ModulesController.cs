using SwiftSkoolv1.Domain.ClassRoom;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class ModulesController : BaseController
    {
        // GET: Modules
        public async Task<ActionResult> Index()
        {
            var modules = Db.Modules.Include(m => m.Class).Include(m => m.Subject).Include(m => m.Term);
            return View(await modules.ToListAsync());
        }

        // GET: Modules/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = await Db.Modules.FindAsync(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // GET: Modules/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.ClassId = new SelectList(await _query.ClassListAsync(userSchool), "ClassId", "FullClassName");
            ViewBag.SubjectId = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName");
            ViewBag.TermId = new SelectList(_query.TermList(), "TermId", "TermName");
            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Module module)
        {
            if (ModelState.IsValid)
            {
                module.SchoolId = userSchool;
                Db.Modules.Add(module);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ClassId = new SelectList(await _query.ClassListAsync(userSchool), "ClassId", "FullClassName", module.ClassId);
            ViewBag.SubjectId = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName", module.SubjectId);
            ViewBag.TermId = new SelectList(_query.TermList(), "TermId", "TermName", module.TermId);


            return View(module);
        }

        // GET: Modules/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = await Db.Modules.FindAsync(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassId = new SelectList(await _query.ClassListAsync(userSchool), "ClassId", "FullClassName", module.ClassId);
            ViewBag.SubjectId = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName", module.SubjectId);
            ViewBag.TermId = new SelectList(_query.TermList(), "TermId", "TermName", module.TermId);
            return View(module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Module module)
        {
            if (ModelState.IsValid)
            {
                module.SchoolId = userSchool;
                Db.Entry(module).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ClassId = new SelectList(await _query.ClassListAsync(userSchool), "ClassId", "FullClassName", module.ClassId);
            ViewBag.SubjectId = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectId", "SubjectName", module.SubjectId);
            ViewBag.TermId = new SelectList(_query.TermList(), "TermId", "TermName", module.TermId);
            return View(module);
        }

        // GET: Modules/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = await Db.Modules.FindAsync(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Module module = await Db.Modules.FindAsync(id);
            Db.Modules.Remove(module);
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
