using SwiftSkoolv1.Domain;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class BehaviorSkillCategoriesController : BaseController
    {
        // GET: BehaviorSkillCategories
        public ActionResult Index()
        {
            //return View(await Db.BehaviorSkillCategories.ToListAsync());
            return View();
        }

        public ActionResult GetIndex()
        {
            #region Server Side filtering
            //Get parameter for sorting from grid table
            // get Start (paging start index) and length (page size for paging)
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns values when we click on Header Name of column
            //getting column name
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            //Soring direction(either desending or ascending)
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            string search = Request.Form.GetValues("search[value]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var v = Db.BehaviorSkillCategories.Where(x => x.SchoolId == userSchool).Select(s => new { s.BehaviorSkillCategoryId, s.Name }).ToList();

            if (!string.IsNullOrEmpty(search))
            {
                v = Db.BehaviorSkillCategories.Where(x => x.SchoolId.Equals(userSchool) && (x.Name.Equals(search)))
                    .Select(s => new { s.BehaviorSkillCategoryId, s.Name }).ToList();
            }
            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
            #endregion

            //return Json(new { data = await Db.Subjects.AsNoTracking().Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToListAsync() }, JsonRequestBehavior.AllowGet);
        }

        // GET: BehaviorSkillCategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var behaviorSkillCategory = await Db.BehaviorSkillCategories.FindAsync(id);
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
                    Db.BehaviorSkillCategories.Add(behaviorSkillCategory);
                    await Db.SaveChangesAsync();
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
            var behaviorSkillCategory = await Db.BehaviorSkillCategories.FindAsync(id);
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
                Db.Entry(behaviorSkillCategory).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                TempData["UserMessage"] = "Behavioral Category Updated Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            TempData["UserMessage"] = "Behavioral Category Not Updated Successful";
            TempData["Title"] = "Error.";
            return View(behaviorSkillCategory);
        }


        public async Task<PartialViewResult> Save(int id)
        {
            var behaviorSkill = await Db.BehaviorSkillCategories.FindAsync(id);
            return PartialView(behaviorSkill);
        }

        // POST: Subjects/Save/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(BehaviorSkillCategory behaviorskill)
        {
            bool status = false;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (behaviorskill.BehaviorSkillCategoryId > 0)
                {
                    behaviorskill.SchoolId = userSchool;
                    Db.Entry(behaviorskill).State = EntityState.Modified;
                    message = "Behavioral skill Updated Successfully...";
                }
                else
                {
                    behaviorskill.SchoolId = userSchool;
                    Db.BehaviorSkillCategories.Add(behaviorskill);
                    message = "Behavioral skill Created Successfully...";

                }
                await Db.SaveChangesAsync();
                status = true;
                return new JsonResult { Data = new { status = status, message = message } };
            }
            return new JsonResult { Data = new { status = status, message = message } };
            //return View(subject);
        }


        // GET: BehaviorSkillCategories/Delete/5
        // GET: Subjects/Delete/5
        public async Task<PartialViewResult> Delete(int id)
        {
            BehaviorSkillCategory behavoralskill = await Db.BehaviorSkillCategories.FindAsync(id);

            return PartialView(behavoralskill);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            bool status = false;
            string message = string.Empty;
            var behavior = await Db.BehaviorSkillCategories.FindAsync(id);
            if (behavior != null)
            {
                Db.BehaviorSkillCategories.Remove(behavior);
                await Db.SaveChangesAsync();
                status = true;
                message = "Behavioral skill Category Deleted Successfully...";
            }

            return new JsonResult { Data = new { status = status, message = message } };
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
