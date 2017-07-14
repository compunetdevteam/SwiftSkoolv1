using SwiftSkoolv1.Domain;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class FeeCategoriesController : BaseController
    {

        // GET: FeeCategories
        public async Task<ActionResult> Index()
        {
            return View(await Db.FeeCategories.AsNoTracking().ToListAsync());
        }

        public async Task<FeeCategory> FindFeeCategoryAsync(int? id)
        {
            return await Db.FeeCategories.AsNoTracking().Where(fc => fc.FeeCategoryId.Equals(id))
                                              .SingleOrDefaultAsync();
        }

        // GET: FeeCategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var feeCategory = await FindFeeCategoryAsync(id);
            if (feeCategory == null)
            {
                return HttpNotFound();
            }
            return View(feeCategory);
        }

        // GET: FeeCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FeeCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateFeeCategoryVM feeCategory)
        {
            if (Db.FeeCategories.Any(fc => fc.CategoryName.Equals(feeCategory.CategoryName)))
            {
                ModelState.AddModelError("Error", "A Fee Category already exists with the name you have supplied or your or the name is blank! Please fill a valid name for the category!");
                return View(feeCategory);
            }

            if (ModelState.IsValid)
            {
                var model = new FeeCategory
                {
                    CategoryName = feeCategory.CategoryName,
                    CategoryDescription = feeCategory.CategoryDescription
                };

                Db.FeeCategories.Add(model);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(feeCategory);
        }

        // GET: FeeCategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var feeCategory = await FindFeeCategoryAsync(id);
            if (feeCategory == null)
            {
                return HttpNotFound();
            }
            return View(feeCategory);
        }

        // POST: FeeCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditFeeCategoryVM feeCategory)
        {
            if (string.IsNullOrWhiteSpace(feeCategory.CategoryName) || string.IsNullOrEmpty(feeCategory.CategoryName))
            {
                ModelState.AddModelError("Error", "You cannot have blank categories in the application!");
                return View(feeCategory);
            }
            if (ModelState.IsValid)
            {
                var model = await Db.FeeCategories.FindAsync(feeCategory.Id);

                Db.Entry(model).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(feeCategory);
        }

        // GET: FeeCategories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var feeCategory = await FindFeeCategoryAsync(id);
            if (feeCategory == null)
            {
                return HttpNotFound();
            }
            return View(feeCategory);
        }

        // POST: FeeCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var feeCategory = await FindFeeCategoryAsync(id);
            Db.FeeCategories.Remove(feeCategory);
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
