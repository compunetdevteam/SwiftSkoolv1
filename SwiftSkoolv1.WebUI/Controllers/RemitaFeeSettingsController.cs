using SwiftSkoolv1.Domain;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class RemitaFeeSettingsController : BaseController
    {
        // GET: RemitaFeeSettings
        public async Task<ActionResult> Index()
        {
            var remitaFeeSettings = Db.RemitaFeeSettings.Include(r => r.School);
            return View(await remitaFeeSettings.ToListAsync());
        }

        // GET: RemitaFeeSettings/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RemitaFeeSetting remitaFeeSetting = await Db.RemitaFeeSettings.FindAsync(id);
            if (remitaFeeSetting == null)
            {
                return HttpNotFound();
            }
            return View(remitaFeeSetting);
        }

        // GET: RemitaFeeSettings/Create
        public ActionResult Create()
        {
            ViewBag.SchoolId = new SelectList(Db.Schools, "SchoolId", "Name");
            return View();
        }

        // POST: RemitaFeeSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RemitaFeeSetting remitaFeeSetting)
        {
            if (ModelState.IsValid)
            {
                remitaFeeSetting.SchoolId = userSchool;
                Db.RemitaFeeSettings.Add(remitaFeeSetting);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.SchoolId = new SelectList(Db.Schools, "SchoolId", "Name", remitaFeeSetting.SchoolId);
            return View(remitaFeeSetting);
        }

        // GET: RemitaFeeSettings/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RemitaFeeSetting remitaFeeSetting = await Db.RemitaFeeSettings.FindAsync(id);
            if (remitaFeeSetting == null)
            {
                return HttpNotFound();
            }
            ViewBag.SchoolId = new SelectList(Db.Schools, "SchoolId", "Name", remitaFeeSetting.SchoolId);
            return View(remitaFeeSetting);
        }

        // POST: RemitaFeeSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RemitaFeeSetting remitaFeeSetting)
        {
            if (ModelState.IsValid)
            {
                remitaFeeSetting.SchoolId = userSchool;
                Db.Entry(remitaFeeSetting).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SchoolId = new SelectList(Db.Schools, "SchoolId", "Name", remitaFeeSetting.SchoolId);
            return View(remitaFeeSetting);
        }

        // GET: RemitaFeeSettings/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RemitaFeeSetting remitaFeeSetting = await Db.RemitaFeeSettings.FindAsync(id);
            if (remitaFeeSetting == null)
            {
                return HttpNotFound();
            }
            return View(remitaFeeSetting);
        }

        // POST: RemitaFeeSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            RemitaFeeSetting remitaFeeSetting = await Db.RemitaFeeSettings.FindAsync(id);
            Db.RemitaFeeSettings.Remove(remitaFeeSetting);
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
