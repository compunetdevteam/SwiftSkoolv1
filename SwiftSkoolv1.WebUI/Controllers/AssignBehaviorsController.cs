using PagedList;
using SwiftSkool.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HopeAcademySMS.Controllers
{
    public class AssignBehaviorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AssignBehaviors
        public ActionResult Index(string sortOrder, string currentFilter, string search, string StudentId,
                                         string SessionName, string TermName, int? page)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (search != null)
            {

                page = 1;
            }
            else
            {
                search = currentFilter;
            }
            ViewBag.CurrentFilter = search;
            var assignedList = from s in db.AssignBehaviors select s;
            if (!String.IsNullOrEmpty(search))
            {
                assignedList = assignedList.AsNoTracking().Where(s => s.StudentId.ToUpper().Contains(search.ToUpper())
                                                     || s.SessionName.ToUpper().Contains(search.ToUpper())
                                                     || s.TermName.ToUpper().Contains(search.ToUpper()));

            }
            else if (!String.IsNullOrEmpty(SessionName) || !String.IsNullOrEmpty(StudentId)
                                || !String.IsNullOrEmpty(TermName))
            {
                assignedList = assignedList.AsNoTracking().Where(s => s.SessionName.Contains(SessionName)
                                                    && s.TermName.ToUpper().Contains(TermName.ToUpper())
                                                    && s.StudentId.ToUpper().Contains(StudentId.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    assignedList = assignedList.AsNoTracking().OrderByDescending(s => s.StudentId);
                    break;
                case "Date":
                    assignedList = assignedList.AsNoTracking().OrderBy(s => s.StudentId);
                    break;
                default:
                    assignedList = assignedList.AsNoTracking().OrderBy(s => s.StudentId);
                    break;
            }
            int pageSize = 15;
            int pageNumber = (page ?? 1);

            ViewBag.BehaviouralSkillId = new MultiSelectList(db.BehaviouralSkills.AsNoTracking(), "SkillName", "SkillName");
            ViewBag.SessionName = new SelectList(db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.TermName = new SelectList(db.Terms.AsNoTracking(), "TermName", "TermName");
            ViewBag.StudentId = new SelectList(db.Students.AsNoTracking(), "StudentID", "FullName");
            //var count = assignedList.Count();
            //TempData["UserMessage"] = $"You Search result contains {count} Records ";
            //TempData["Title"] = "Success.";
            return View(assignedList.AsNoTracking().ToPagedList(pageNumber, pageSize));
            //return View(await db.AssignedClasses.ToListAsync());
        }

        // GET: AssignBehaviors/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignBehavior assignBehavior = await db.AssignBehaviors.FindAsync(id);
            if (assignBehavior == null)
            {
                return HttpNotFound();
            }
            return View(assignBehavior);
        }

        // GET: AssignBehaviors/Create
        public ActionResult Create()
        {
            ViewBag.BehaviouralSkillId = new MultiSelectList(db.BehaviouralSkills.AsNoTracking(), "SkillName", "SkillName");
            ViewBag.SessionName = new SelectList(db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.TermName = new SelectList(db.Terms.AsNoTracking(), "TermName", "TermName");
            ViewBag.StudentId = new SelectList(db.Students.AsNoTracking(), "StudentID", "FullName");
            return View();
        }

        // POST: AssignBehaviors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AssignBehaviorVm model)
        {
            if (ModelState.IsValid)
            {
                //string[] ssizes = model.SkillScore.Split(new[] { ' ', ',', '-' });
                string[] ssizes = model.SkillScore.Trim().Split(',');
                if (ssizes.Length != model.BehaviouralSkillId.Length)
                {
                    string lineError = $"The number of Behaviors you entered is {model.BehaviouralSkillId.Length}  and the score Entered is {ssizes.Length}. " +
                                       $"You should assign the same number of Score to the behavior you selected ";
                    //ViewBag.LineError = lineError;
                    TempData["UserMessage"] = lineError;
                    TempData["Title"] = "Error.";
                    ViewBag.BehaviouralSkillId = new MultiSelectList(db.BehaviouralSkills.AsNoTracking(), "SkillName", "SkillName");
                    ViewBag.SessionName = new SelectList(db.Sessions.AsNoTracking(), "SessionName", "SessionName");
                    ViewBag.TermName = new SelectList(db.Terms.AsNoTracking(), "TermName", "TermName");
                    ViewBag.StudentId = new SelectList(db.Students.AsNoTracking(), "StudentID", "FullName");
                    return View(model);
                }
                for (int i = 0; i < model.BehaviouralSkillId.Length; i++)
                {
                    string my = model.BehaviouralSkillId[i];
                    var behavioCategory = await db.BehaviouralSkills.Where(c => c.SkillName.Equals(my))
                                         .Select(x => x.BehaviorSkillCategoryId).FirstOrDefaultAsync();
                    AssignBehavior assignBehavior = new AssignBehavior()
                    {
                        StudentId = model.StudentId,
                        SessionName = model.SessionName,
                        TermName = model.TermName,
                        TeacherComment = model.TeacherComment,
                        SkillScore = ssizes[i].Trim(),
                        BehaviouralSkillId = model.BehaviouralSkillId[i],
                        BehaviorCategory = behavioCategory,
                        NoOfAbsence = model.NoOfAbsence,
                        Date = DateTime.Now

                    };
                    db.AssignBehaviors.Add(assignBehavior);
                }

                await db.SaveChangesAsync();
                TempData["UserMessage"] = "Behavior Added Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }

            ViewBag.BehaviouralSkillId = new MultiSelectList(db.BehaviouralSkills.AsNoTracking(), "SkillName", "SkillName", model.BehaviouralSkillId);
            ViewBag.SessionName = new SelectList(db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.TermName = new SelectList(db.Terms.AsNoTracking(), "TermName", "TermName");
            ViewBag.StudentId = new SelectList(db.Students.AsNoTracking(), "StudentID", "FullName");
            return View(model);
        }

        // GET: AssignBehaviors/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignBehavior model = await db.AssignBehaviors.FindAsync(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            AssignBehaviorVm assignBehavior = new AssignBehaviorVm
            {
                AssignBehaviorId = model.AssignBehaviorId,
                StudentId = model.StudentId,
                SessionName = model.SessionName,
                TermName = model.TermName,
                TeacherComment = model.TeacherComment,
                SkillScore = model.SkillScore,
                NoOfAbsence = model.NoOfAbsence,
                Date = DateTime.Now
            };

            //ViewBag.BehaviouralSkillId = new MultiSelectList(db.BehaviouralSkills, "BehaviouralSkillId", "SkillName", assignBehavior.BehaviouralSkillId);
            ViewBag.BehaviouralSkillId = new MultiSelectList(db.BehaviouralSkills.AsNoTracking(), "SkillName", "SkillName");
            ViewBag.SessionName = new SelectList(db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.TermName = new SelectList(db.Terms.AsNoTracking(), "TermName", "TermName");
            return View(assignBehavior);
        }

        // POST: AssignBehaviors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AssignBehaviorVm model)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < model.BehaviouralSkillId.Length; i++)
                {
                    string my = model.BehaviouralSkillId[i];
                    var behavioCategory = await db.BehaviouralSkills.Where(c => c.SkillName.Equals(my))
                            .Select(x => x.BehaviorSkillCategoryId).FirstOrDefaultAsync();
                    AssignBehavior assignBehavior = new AssignBehavior()
                    {
                        AssignBehaviorId = model.AssignBehaviorId,
                        StudentId = model.StudentId,
                        SessionName = model.SessionName,
                        TermName = model.TermName,
                        TeacherComment = model.TeacherComment,
                        SkillScore = model.SkillScore,
                        BehaviouralSkillId = model.BehaviouralSkillId[i],
                        BehaviorCategory = behavioCategory,
                        NoOfAbsence = model.NoOfAbsence,
                        Date = DateTime.Now

                    };
                    db.Entry(assignBehavior).State = EntityState.Modified;
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.BehaviouralSkillId = new MultiSelectList(db.BehaviouralSkills.AsNoTracking(), "SkillName", "SkillName");
            ViewBag.SessionName = new SelectList(db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.TermName = new SelectList(db.Terms.AsNoTracking(), "TermName", "TermName");
            return View(model);
        }

        // GET: AssignBehaviors/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignBehavior assignBehavior = await db.AssignBehaviors.FindAsync(id);
            if (assignBehavior == null)
            {
                return HttpNotFound();
            }
            return View(assignBehavior);
        }

        // POST: AssignBehaviors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AssignBehavior assignBehavior = await db.AssignBehaviors.FindAsync(id);
            if (assignBehavior != null) db.AssignBehaviors.Remove(assignBehavior);
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
