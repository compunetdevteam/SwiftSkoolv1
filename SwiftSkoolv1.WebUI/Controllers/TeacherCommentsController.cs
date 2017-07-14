using SwiftSkool.Models;
using SwiftSkool.ViewModel;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HopeAcademySMS.Controllers
{
    public class TeacherCommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TeacherComments
        public async Task<ActionResult> Index()
        {
            return View(await db.TeacherComments.ToListAsync());
        }

        // GET: TeacherComments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeacherComment teacherComment = await db.TeacherComments.FindAsync(id);
            if (teacherComment == null)
            {
                return HttpNotFound();
            }
            return View(teacherComment);
        }

        // GET: TeacherComments/Create
        public ActionResult Create()
        {
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(db.Sessions, "SessionName", "SessionName");
            return View();
        }

        // POST: TeacherComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TeacherComment model)
        {
            if (ModelState.IsValid)
            {
                TeacherComment comment = new TeacherComment()
                {
                    StudentId = model.StudentId,
                    TermName = model.TermName,
                    SessionName = model.SessionName,
                    Remark = model.Remark,
                    Date = model.Date.Date
                };
                db.TeacherComments.Add(comment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: TeacherComments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeacherComment teacherComment = await db.TeacherComments.FindAsync(id);
            if (teacherComment == null)
            {
                return HttpNotFound();
            }
            return View(teacherComment);
        }

        // POST: TeacherComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TeacherCommentId,StudentId,TermName,SessionName,Remark,Date")] TeacherComment teacherComment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teacherComment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(teacherComment);
        }

        // GET: TeacherComments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeacherComment teacherComment = await db.TeacherComments.FindAsync(id);
            if (teacherComment == null)
            {
                return HttpNotFound();
            }
            return View(teacherComment);
        }

        // POST: TeacherComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TeacherComment teacherComment = await db.TeacherComments.FindAsync(id);
            db.TeacherComments.Remove(teacherComment);
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
