using SwiftSkoolv1.Domain;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class AppointmentDiariesController : BaseController
    {
        // GET: AppointmentDiaries
        public async Task<ActionResult> Index()
        {
            return View(await Db.AppointmentDiary.ToListAsync());
        }

        // GET: AppointmentDiaries/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var appointmentDiary = await Db.AppointmentDiary.FindAsync(id);
            if (appointmentDiary == null)
            {
                return HttpNotFound();
            }
            return View(appointmentDiary);
        }

        // GET: AppointmentDiaries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AppointmentDiaries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Title,SomeImportantKey,DateTimeScheduled,AppointmentLength,StatusENUM")] AppointmentDiary appointmentDiary)
        {
            if (ModelState.IsValid)
            {
                Db.AppointmentDiary.Add(appointmentDiary);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(appointmentDiary);
        }

        // GET: AppointmentDiaries/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var appointmentDiary = await Db.AppointmentDiary.FindAsync(id);
            if (appointmentDiary == null)
            {
                return HttpNotFound();
            }
            return View(appointmentDiary);
        }

        // POST: AppointmentDiaries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Title,SomeImportantKey,DateTimeScheduled,AppointmentLength,StatusENUM")] AppointmentDiary appointmentDiary)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(appointmentDiary).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(appointmentDiary);
        }

        // GET: AppointmentDiaries/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var appointmentDiary = await Db.AppointmentDiary.FindAsync(id);
            if (appointmentDiary == null)
            {
                return HttpNotFound();
            }
            return View(appointmentDiary);
        }

        // POST: AppointmentDiaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var appointmentDiary = await Db.AppointmentDiary.FindAsync(id);
            Db.AppointmentDiary.Remove(appointmentDiary);
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
