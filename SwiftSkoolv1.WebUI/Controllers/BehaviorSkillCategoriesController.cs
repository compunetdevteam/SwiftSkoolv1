using SwiftSkool.Models;
using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HopeAcademySMS.Controllers
{
    public class BehaviorSkillCategoriesController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: BehaviorSkillCategories
        public async Task<ActionResult> Index()
        {
            return View(await _db.BehaviorSkillCategories.ToListAsync());
        }

        // GET: BehaviorSkillCategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BehaviorSkillCategory behaviorSkillCategory = await _db.BehaviorSkillCategories.FindAsync(id);
            if (behaviorSkillCategory == null)
            {
                return HttpNotFound();
            }
            return View(behaviorSkillCategory);
        }

        // GET: BehaviorSkillCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BehaviorSkillCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BehaviorSkillCategoryId,Name")] BehaviorSkillCategory behaviorSkillCategory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _db.BehaviorSkillCategories.Add(behaviorSkillCategory);
                    await _db.SaveChangesAsync();
                    TempData["UserMessage"] = "Behavioral Category created Successfully.";
                    TempData["Title"] = "Success.";
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    TempData["UserMessage"] = $"You have already created Behavioral Category {e.Message}";
                    TempData["Title"] = "Error.";
                    return View(behaviorSkillCategory);
                }

            }
            TempData["UserMessage"] = "Behavioral Category Not Assigned Successful";
            TempData["Title"] = "Error.";
            return View(behaviorSkillCategory);
        }

        // GET: BehaviorSkillCategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BehaviorSkillCategory behaviorSkillCategory = await _db.BehaviorSkillCategories.FindAsync(id);
            if (behaviorSkillCategory == null)
            {
                return HttpNotFound();
            }
            return View(behaviorSkillCategory);
        }

        // POST: BehaviorSkillCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BehaviorSkillCategoryId,Name")] BehaviorSkillCategory behaviorSkillCategory)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(behaviorSkillCategory).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                TempData["UserMessage"] = "Behavioral Category Updated Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            TempData["UserMessage"] = "Behavioral Category Not Updated Successful";
            TempData["Title"] = "Error.";
            return View(behaviorSkillCategory);
        }

        // GET: BehaviorSkillCategories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BehaviorSkillCategory behaviorSkillCategory = await _db.BehaviorSkillCategories.FindAsync(id);
            if (behaviorSkillCategory == null)
            {
                return HttpNotFound();
            }
            return View(behaviorSkillCategory);
        }

        // POST: BehaviorSkillCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            BehaviorSkillCategory behaviorSkillCategory = await _db.BehaviorSkillCategories.FindAsync(id);
            if (behaviorSkillCategory != null) _db.BehaviorSkillCategories.Remove(behaviorSkillCategory);
            await _db.SaveChangesAsync();
            TempData["UserMessage"] = "Behavioral Category has been Deleted Successful";
            TempData["Title"] = "Error.";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
