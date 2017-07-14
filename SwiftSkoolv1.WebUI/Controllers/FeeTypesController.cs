using SwiftSkoolv1.Domain;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class FeeTypesController : BaseController
    {
        // GET: FeeTypes
        public async Task<ActionResult> Index()
        {
            if (Request.IsAuthenticated && !User.IsInRole("SuperAdmin"))
            {
                return View(await Db.FeeTypes.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool)).ToListAsync());
            }
            return View(await Db.FeeTypes.AsNoTracking().ToListAsync());
        }

        // GET: FeeTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var feeType = await Db.FeeTypes.FindAsync(id);
            if (feeType == null)
            {
                return HttpNotFound();
            }
            return View(feeType);
        }

        // GET: FeeTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FeeTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,FeeName,Description")] FeeType feeType)
        {
            if (ModelState.IsValid)
            {
                feeType.SchoolId = userSchool;
                Db.FeeTypes.Add(feeType);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(feeType);
        }

        // GET: FeeTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var feeType = await Db.FeeTypes.FindAsync(id);
            if (feeType == null)
            {
                return HttpNotFound();
            }
            return View(feeType);
        }

        // POST: FeeTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,FeeName,Description")] FeeType feeType)
        {
            if (ModelState.IsValid)
            {
                feeType.SchoolId = userSchool;
                Db.Entry(feeType).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(feeType);
        }

        // GET: FeeTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var feeType = await Db.FeeTypes.FindAsync(id);
            if (feeType == null)
            {
                return HttpNotFound();
            }
            return View(feeType);
        }

        // POST: FeeTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var feeType = await Db.FeeTypes.FindAsync(id);
            if (feeType != null) Db.FeeTypes.Remove(feeType);
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
