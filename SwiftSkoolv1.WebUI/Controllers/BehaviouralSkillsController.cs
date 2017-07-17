using SwiftSkoolv1.Domain;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class BehaviouralSkillsController : BaseController
    {


        // GET: BehaviouralSkills
        public async Task<ActionResult> Index()
        {
            var behaviouralSkills = Db.BehaviouralSkills.AsNoTracking().Include(b => b.BehaviorSkillCategories);
            return View(await behaviouralSkills.AsNoTracking().ToListAsync());
        }

        // GET: BehaviouralSkills/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var behaviouralSkill = await Db.BehaviouralSkills.FindAsync(id);
            if (behaviouralSkill == null)
            {
                return HttpNotFound();
            }
            return View(behaviouralSkill);
        }

        // GET: BehaviouralSkills/Create
        public ActionResult Create()
        {
            ViewBag.BehaviorSkillCategoryId = new SelectList(Db.BehaviorSkillCategories.AsNoTracking(), "Name", "Name");
            return View();
        }

        // POST: BehaviouralSkills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BehaviouralSkillId,SkillName,BehaviorSkillCategoryId")] BehaviouralSkill behaviouralSkill)
        {
            if (ModelState.IsValid)
            {
                Db.BehaviouralSkills.Add(behaviouralSkill);
                await Db.SaveChangesAsync();
                TempData["UserMessage"] = "Behavioral created Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            TempData["UserMessage"] = "Behavioral Not Created Successful";
            TempData["Title"] = "Error.";
            ViewBag.BehaviorSkillCategoryId = new SelectList(Db.BehaviorSkillCategories.AsNoTracking(), "Name", "Name", behaviouralSkill.BehaviorSkillCategoryId);
            return View(behaviouralSkill);
        }

        // GET: BehaviouralSkills/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var behaviouralSkill = await Db.BehaviouralSkills.FindAsync(id);
            if (behaviouralSkill == null)
            {

                return HttpNotFound();
            }
            ViewBag.BehaviorSkillCategoryId = new SelectList(Db.BehaviorSkillCategories.AsNoTracking(), "Name", "Name", behaviouralSkill.BehaviorSkillCategoryId);
            return View(behaviouralSkill);
        }

        // POST: BehaviouralSkills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BehaviouralSkillId,SkillName,BehaviorSkillCategoryId")] BehaviouralSkill behaviouralSkill)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(behaviouralSkill).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                TempData["UserMessage"] = "Behavioral Updated Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            TempData["UserMessage"] = "Behavioral Not Updated Successful";
            TempData["Title"] = "Error.";
            ViewBag.BehaviorSkillCategoryId = new SelectList(Db.BehaviorSkillCategories.AsNoTracking(), "BehaviorSkillCategoryId", "Name", behaviouralSkill.BehaviorSkillCategoryId);
            return View(behaviouralSkill);
        }

        // GET: BehaviouralSkills/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var behaviouralSkill = await Db.BehaviouralSkills.FindAsync(id);
            if (behaviouralSkill == null)
            {
                return HttpNotFound();
            }
            return View(behaviouralSkill);
        }

        // POST: BehaviouralSkills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var behaviouralSkill = await Db.BehaviouralSkills.FindAsync(id);
            if (behaviouralSkill != null) Db.BehaviouralSkills.Remove(behaviouralSkill);
            await Db.SaveChangesAsync();
            TempData["UserMessage"] = "Behavioral Deleted Successful";
            TempData["Title"] = "Error.";
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
