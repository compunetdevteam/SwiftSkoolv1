using SwiftSkoolv1.Domain;
using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class TeacherCommentsController : BaseController
    {


        // GET: TeacherComments
        public async Task<ActionResult> Index()
        {
            return View(await Db.TeacherComments.ToListAsync());
        }

        // GET: TeacherComments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeacherComment teacherComment = await Db.TeacherComments.FindAsync(id);
            if (teacherComment == null)
            {
                return HttpNotFound();
            }
            return View(teacherComment);
        }

        // GET: TeacherComments/Create
        public ActionResult Create()
        {
            ViewBag.StudentId = new SelectList(Db.Students, "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(Db.Sessions, "SessionName", "SessionName");
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
                    Date = DateTime.Now
                };
                Db.TeacherComments.Add(comment);
                await Db.SaveChangesAsync();
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
            TeacherComment teacherComment = await Db.TeacherComments.FindAsync(id);
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
        public async Task<ActionResult> Edit(TeacherComment teacherComment)
        {
            if (ModelState.IsValid)
            {
                teacherComment.Date = DateTime.Now;
                Db.Entry(teacherComment).State = EntityState.Modified;
                await Db.SaveChangesAsync();
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
            TeacherComment teacherComment = await Db.TeacherComments.FindAsync(id);
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
            TeacherComment teacherComment = await Db.TeacherComments.FindAsync(id);
            Db.TeacherComments.Remove(teacherComment);
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
