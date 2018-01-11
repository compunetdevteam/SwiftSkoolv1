using SwiftSkoolv1.Domain;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class ReportCardSettingsController : BaseController
    {
        // GET: ReportCardSettings
        public async Task<ActionResult> Index()
        {
            return View(await Db.ReportCardSettings.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool)).ToListAsync());
        }

        // GET: ReportCardSettings/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportCardSetting reportCardSetting = await Db.ReportCardSettings.FindAsync(id);
            if (reportCardSetting == null)
            {
                return HttpNotFound();
            }
            return View(reportCardSetting);
        }

        // GET: ReportCardSettings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReportCardSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ReportCardSetting reportCardSetting)
        {
            if (ModelState.IsValid)
            {
                reportCardSetting.SchoolId = userSchool;
                //reportCardSetting.ResumptionDate = reportCardSetting.Date;
                Db.ReportCardSettings.Add(reportCardSetting);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(reportCardSetting);
        }

        // GET: ReportCardSettings/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportCardSetting reportCardSetting = await Db.ReportCardSettings.FindAsync(id);
            if (reportCardSetting == null)
            {
                return HttpNotFound();
            }
            return View(reportCardSetting);
        }

        // POST: ReportCardSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ReportCardSetting reportCardSetting)
        {
            if (ModelState.IsValid)
            {
                reportCardSetting.SchoolId = userSchool;
                Db.Entry(reportCardSetting).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(reportCardSetting);
        }

        // GET: ReportCardSettings/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportCardSetting reportCardSetting = await Db.ReportCardSettings.FindAsync(id);
            if (reportCardSetting == null)
            {
                return HttpNotFound();
            }
            return View(reportCardSetting);
        }

        // POST: ReportCardSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ReportCardSetting reportCardSetting = await Db.ReportCardSettings.FindAsync(id);
            if (reportCardSetting != null) Db.ReportCardSettings.Remove(reportCardSetting);
            await Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public async Task<ActionResult> RenderPrincipalSign(string schoolId)
        {
            var school = await Db.ReportCardSettings.AsNoTracking()
                            .Where(x => x.SchoolId.Equals(schoolId)).FirstOrDefaultAsync();

            byte[] photoBack = school.PrincipalSignature;

            return File(photoBack, "image/png");
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
