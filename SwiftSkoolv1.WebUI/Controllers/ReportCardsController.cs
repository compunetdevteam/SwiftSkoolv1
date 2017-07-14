using SwiftSkool.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HopeAcademySMS.Controllers
{
    public class ReportCardsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ReportCards
        public async Task<ActionResult> Index()
        {
            return View(await db.ReportCards.ToListAsync());
        }

        // GET: ReportCards/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportCard reportCard = await db.ReportCards.FindAsync(id);
            if (reportCard == null)
            {
                return HttpNotFound();
            }
            return View(reportCard);
        }

        // GET: ReportCards/Create
        public ActionResult Create()
        {
            ViewBag.SessionName = new SelectList(db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.TermName = new SelectList(db.Terms.AsNoTracking(), "TermName", "TermName");
            return View();
        }

        // POST: ReportCards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ReportCardVm model)
        {
            if (ModelState.IsValid)
            {
                ReportCard reportCard = new ReportCard()
                {
                    TermName = model.TermName,
                    SessionName = model.SessionName,
                    SchoolOpened = model.SchoolOpened,
                    NextTermBegin = model.NextTermBegin.Date,
                    NextTermEnd = model.NextTermEnd.Date,
                    PrincipalSignature = model.PrincipalSignature
                };
                db.ReportCards.Add(reportCard);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SessionName = new SelectList(db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.TermName = new SelectList(db.Terms.AsNoTracking(), "TermName", "TermName");
            return View(model);
        }

        // GET: ReportCards/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportCard model = await db.ReportCards.FindAsync(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            ReportCardVm reportCard = new ReportCardVm
            {
                ReportCardId = model.ReportCardId,
                TermName = model.TermName,
                SessionName = model.SessionName,
                SchoolOpened = model.SchoolOpened,
                NextTermBegin = model.NextTermBegin.Date,
                NextTermEnd = model.NextTermEnd.Date,
                PrincipalSignature = model.PrincipalSignature
            };
            ViewBag.SessionName = new SelectList(db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.TermName = new SelectList(db.Terms.AsNoTracking(), "TermName", "TermName");
            return View(reportCard);
        }

        // POST: ReportCards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ReportCardVm model)
        {
            if (ModelState.IsValid)
            {
                ReportCard reportCard = new ReportCard()
                {
                    ReportCardId = model.ReportCardId,
                    TermName = model.TermName,
                    SessionName = model.SessionName,
                    SchoolOpened = model.SchoolOpened,
                    NextTermBegin = model.NextTermBegin.Date,
                    NextTermEnd = model.NextTermEnd.Date,
                    PrincipalSignature = model.PrincipalSignature
                };
                db.Entry(reportCard).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SessionName = new SelectList(db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.TermName = new SelectList(db.Terms.AsNoTracking(), "TermName", "TermName");
            return View(model);
        }

        // GET: ReportCards/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportCard reportCard = await db.ReportCards.FindAsync(id);
            if (reportCard == null)
            {
                return HttpNotFound();
            }
            return View(reportCard);
        }

        // POST: ReportCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ReportCard reportCard = await db.ReportCards.FindAsync(id);
            db.ReportCards.Remove(reportCard);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
