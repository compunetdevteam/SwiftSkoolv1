using SwiftSkoolv1.Domain;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class AssignReportCardsController : BaseController
    {

        // GET: AssignReportCards
        public async Task<ActionResult> Index()
        {
            var assignReportCards = Db.AssignReportCards.Include(a => a.Class).AsNoTracking()
                            .Where(x => x.SchoolId.Equals(userSchool));
            return View(await assignReportCards.ToListAsync());
        }

        // GET: AssignReportCards/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignReportCard assignReportCard = await Db.AssignReportCards.FindAsync(id);
            if (assignReportCard == null)
            {
                return HttpNotFound();
            }
            return View(assignReportCard);
        }

        // GET: AssignReportCards/Create
        public async Task<ActionResult> Create()
        {
            var classList = await _query.ClassListAsync(userSchool);
            ViewBag.ClassId = new SelectList(classList, "ClassId", "ClassName");
            return View();
        }

        // POST: AssignReportCards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AssignReportCard assignReportCard)
        {
            if (ModelState.IsValid)
            {
                assignReportCard.SchoolId = userSchool;
                Db.AssignReportCards.Add(assignReportCard);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var classList = await _query.ClassListAsync(userSchool);
            ViewBag.ClassId = new SelectList(classList, "ClassId", "ClassName");
            return View(assignReportCard);
        }

        // GET: AssignReportCards/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignReportCard assignReportCard = await Db.AssignReportCards.FindAsync(id);
            if (assignReportCard == null)
            {
                return HttpNotFound();
            }
            var classList = await _query.ClassListAsync(userSchool);
            ViewBag.ClassId = new SelectList(classList, "ClassId", "ClassName");
            return View(assignReportCard);
        }

        // POST: AssignReportCards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AssignReportCard assignReportCard)
        {
            if (ModelState.IsValid)
            {
                assignReportCard.SchoolId = userSchool;
                Db.Entry(assignReportCard).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var classList = await _query.ClassListAsync(userSchool);
            ViewBag.ClassId = new SelectList(classList, "ClassId", "ClassName");
            return View(assignReportCard);
        }

        // GET: AssignReportCards/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignReportCard assignReportCard = await Db.AssignReportCards.FindAsync(id);
            if (assignReportCard == null)
            {
                return HttpNotFound();
            }
            return View(assignReportCard);
        }

        // POST: AssignReportCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AssignReportCard assignReportCard = await Db.AssignReportCards.FindAsync(id);
            Db.AssignReportCards.Remove(assignReportCard);
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
