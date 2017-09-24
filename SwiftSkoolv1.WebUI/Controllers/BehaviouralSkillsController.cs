using SwiftSkoolv1.Domain;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class BehaviouralSkillsController : BaseController
    {


        // GET: BehaviouralSkills
        //public async Task<ActionResult> Index()
        //{
        //    var behaviouralSkills = Db.BehaviouralSkills.AsNoTracking().Include(b => b.BehaviorSkillCategories);
        //    return View(await behaviouralSkills.AsNoTracking().ToListAsync());
        //}
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetIndex()
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

            //var v = Db.Subjects.Where(x => x.SchoolId != userSchool).Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToList();
            var v = Db.BehaviouralSkills.Where(x => x.SchoolId == userSchool).Select(s => new { s.BehaviouralSkillId, s.SkillName, s.BehaviorSkillCategoryId }).ToList();

            //var v = Db.Subjects.Where(x => x.SchoolId.Equals(userSchool)).Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToList();
            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            //{
            //    //v = v.OrderBy(sortColumn + " " + sortColumnDir);
            //    v = new List<Subject>(v.OrderBy(x => "sortColumn + \" \" + sortColumnDir"));
            //}
            if (!string.IsNullOrEmpty(search))
            {
                //v = v.OrderBy(sortColumn + " " + sortColumnDir);
                v = Db.BehaviouralSkills.Where(x => x.SchoolId.Equals(userSchool) && (x.SkillName.Equals(search)))
                    .Select(s => new { s.BehaviouralSkillId, s.SkillName, s.BehaviorSkillCategoryId }).ToList();
            }
            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
            #endregion

            //return Json(new { data = await Db.Subjects.AsNoTracking().Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToListAsync() }, JsonRequestBehavior.AllowGet);
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


        public async Task<PartialViewResult> Save(int id)
        {
            ViewBag.BehaviorSkillCategoryId = new SelectList(Db.BehaviorSkillCategories.AsNoTracking()
                            .Where(x => x.SchoolId.Equals(userSchool)), "Name", "Name");
            var behaviouralSkill = await Db.BehaviouralSkills.FindAsync(id);
            return PartialView(behaviouralSkill);
        }

        // POST: Subjects/Save/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(BehaviouralSkill behaviouralSkill)
        {
            bool status = false;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (behaviouralSkill.BehaviouralSkillId > 0)
                {
                    behaviouralSkill.SchoolId = userSchool;
                    Db.Entry(behaviouralSkill).State = EntityState.Modified;
                    message = "Behavioural skill Updated Successfully...";
                }
                else
                {
                    behaviouralSkill.SchoolId = userSchool;
                    Db.BehaviouralSkills.Add(behaviouralSkill);
                    message = "Behavioural skill Created Successfully...";

                }
                await Db.SaveChangesAsync();
                status = true;
            }
            return new JsonResult { Data = new { status = status, message = message } };
            //return View(subject);
        }



        // GET: BehaviouralSkills/Delete/5
        public async Task<PartialViewResult> Delete(int id)
        {
            BehaviouralSkill behaviouralSkill = await Db.BehaviouralSkills.FindAsync(id);

            return PartialView(behaviouralSkill);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            bool status = false;
            string message = string.Empty;
            var behaviouralSkill = await Db.BehaviouralSkills.FindAsync(id);
            if (behaviouralSkill != null)
            {
                Db.BehaviouralSkills.Remove(behaviouralSkill);
                await Db.SaveChangesAsync();
                status = true;
                message = "Behavioural Skill Deleted Successfully...";
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
