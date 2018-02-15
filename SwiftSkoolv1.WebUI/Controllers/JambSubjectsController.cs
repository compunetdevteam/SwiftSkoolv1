using SwiftSkoolv1.Domain.JambPractice;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class JambSubjectsController : BaseController
    {
        // GET: JambSubjects
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetIndex()
        {
            // dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            var data = await Db.JambSubjects.AsNoTracking().Select(s => new
            {
                s.SubjectCode,
                s.SubjectName,
                s.JambSubjectId
            }).ToListAsync();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public async Task<PartialViewResult> Save(int id)
        {
            var jambSubject = await Db.JambSubjects.FindAsync(id);
            return PartialView(jambSubject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(JambSubject model)
        {
            bool status = false;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (model.JambSubjectId > 0)
                {
                    var jambSubject = await Db.JambSubjects.FindAsync(model.JambSubjectId);
                    if (jambSubject != null)
                    {
                        jambSubject.JambSubjectId = model.JambSubjectId;
                        jambSubject.SubjectName = model.SubjectName;
                        jambSubject.SubjectCode = model.SubjectCode;
                        Db.Entry(jambSubject).State = EntityState.Modified;
                        await Db.SaveChangesAsync();
                        message = $"{model.SubjectName} Updated Successfully...";
                        return new JsonResult { Data = new { status = true, message = message } };
                    }
                }
                else
                {
                    Db.JambSubjects.Add(model);
                    await Db.SaveChangesAsync();
                    message = $"{model.SubjectName} Added Successfully.";
                    return new JsonResult { Data = new { status = true, message = message } };
                }
            }
            return new JsonResult { Data = new { status = status, message = message } };
            //return View(subject);
        }




        // GET: JambSubjects/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JambSubject jambSubject = await Db.JambSubjects.FindAsync(id);
            if (jambSubject == null)
            {
                return HttpNotFound();
            }
            return View(jambSubject);
        }

        // GET: JambSubjects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: JambSubjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(JambSubject jambSubject)
        {
            if (ModelState.IsValid)
            {
                Db.JambSubjects.Add(jambSubject);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(jambSubject);
        }

        // GET: JambSubjects/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JambSubject jambSubject = await Db.JambSubjects.FindAsync(id);
            if (jambSubject == null)
            {
                return HttpNotFound();
            }
            return View(jambSubject);
        }

        // POST: JambSubjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "JambSubjectId,SubjectCode,SubjectName")] JambSubject jambSubject)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(jambSubject).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(jambSubject);
        }

        // GET: JambSubjects/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JambSubject jambSubject = await Db.JambSubjects.FindAsync(id);
            if (jambSubject == null)
            {
                return HttpNotFound();
            }
            return View(jambSubject);
        }

        // POST: JambSubjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            JambSubject jambSubject = await Db.JambSubjects.FindAsync(id);
            Db.JambSubjects.Remove(jambSubject);
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
