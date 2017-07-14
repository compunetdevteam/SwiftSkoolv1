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
    public class PrincipalCommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PrincipalComments
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string search, int? page,
           string ClassName)
        {
            int count = 10;
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
            var assignedList = from s in db.PrincipalComments select s;
            if (!String.IsNullOrEmpty(search))
            {
                assignedList = assignedList.Where(s => s.ClassName.ToUpper().Contains(search.ToUpper()));


            }
            else if (!String.IsNullOrEmpty(ClassName))
            {
                assignedList = assignedList.Where(s => s.ClassName.ToUpper().Equals(ClassName.ToUpper()));
                int myCount = await assignedList.CountAsync();
                if (myCount != 0)
                {
                    count = myCount;
                }
            }
            switch (sortOrder)
            {
                case "name_desc":
                    assignedList = assignedList.OrderByDescending(s => s.ClassName);
                    break;
                case "Date":
                    assignedList = assignedList.OrderBy(s => s.ClassName);
                    break;
                default:
                    assignedList = assignedList.OrderBy(s => s.ClassName);
                    break;
            }

            ViewBag.ClassName = new SelectList(db.SchoolClasses.AsNoTracking(), "ClassCode", "ClassCode");
            int pageSize = count;
            int pageNumber = (page ?? 1);
            return View(assignedList.ToPagedList(pageNumber, pageSize));
            //return View(await db.ContinuousAssessments.ToListAsync());
        }


        // GET: PrincipalComments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrincipalComment principalComment = await db.PrincipalComments.FindAsync(id);
            if (principalComment == null)
            {
                return HttpNotFound();
            }
            return View(principalComment);
        }

        // GET: PrincipalComments/Create
        public ActionResult Create()
        {
            ViewBag.ClassName = new SelectList(db.SchoolClasses.AsNoTracking(), "ClassCode", "ClassCode");
            return View();
        }

        // POST: PrincipalComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,MinimumGrade,MaximumGrade,Remark,ClassName")] PrincipalCommentVm model)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in model.ClassName)
                {
                    var myGrade = await db.PrincipalComments.CountAsync(x => x.MaximumGrade.Equals(model.MaximumGrade)
                                                                  && x.MinimumGrade.Equals(model.MinimumGrade)
                                                                  && x.ClassName.Equals(item));

                    if (myGrade >= 1)
                    {
                        TempData["UserMessage"] = "Principal Comment Already Exist in Database.";
                        TempData["Title"] = "Error.";
                        ViewBag.ClassName = new SelectList(db.SchoolClasses, "ClassCode", "ClassCode");
                        return View(model);
                    }
                    var principalComment = new PrincipalComment()
                    {
                        MaximumGrade = model.MaximumGrade,
                        MinimumGrade = model.MinimumGrade,
                        Remark = model.Remark,
                        ClassName = item
                    };
                    db.PrincipalComments.Add(principalComment);
                }
                await db.SaveChangesAsync();
                ViewBag.ClassName = new SelectList(db.SchoolClasses.AsNoTracking(), "ClassCode", "ClassCode");
                TempData["UserMessage"] = "Principal Comment Added Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: PrincipalComments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrincipalComment principalComment = await db.PrincipalComments.FindAsync(id);
            if (principalComment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassName = new SelectList(db.SchoolClasses.AsNoTracking(), "ClassCode", "ClassCode");
            return View(principalComment);
        }

        // POST: PrincipalComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,MinimumGrade,MaximumGrade,Remark,ClassName")] PrincipalComment principalComment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(principalComment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ClassName = new SelectList(db.SchoolClasses.AsNoTracking(), "ClassCode", "ClassCode");
            return View(principalComment);
        }

        // GET: PrincipalComments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrincipalComment principalComment = await db.PrincipalComments.FindAsync(id);
            if (principalComment == null)
            {
                return HttpNotFound();
            }
            return View(principalComment);
        }

        // POST: PrincipalComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PrincipalComment principalComment = await db.PrincipalComments.FindAsync(id);
            if (principalComment != null) db.PrincipalComments.Remove(principalComment);
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
