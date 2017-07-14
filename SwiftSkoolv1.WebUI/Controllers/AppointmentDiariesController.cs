using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SwiftSkool.Calender;
using SwiftSkool.Models;

namespace HopeAcademySMS.Controllers
{
    public class AppointmentDiariesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AppointmentDiaries
        public async Task<ActionResult> Index()
        {
            return View(await db.AppointmentDiary.ToListAsync());
        }

        // GET: AppointmentDiaries/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppointmentDiary appointmentDiary = await db.AppointmentDiary.FindAsync(id);
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
                db.AppointmentDiary.Add(appointmentDiary);
                await db.SaveChangesAsync();
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
            AppointmentDiary appointmentDiary = await db.AppointmentDiary.FindAsync(id);
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
                db.Entry(appointmentDiary).State = EntityState.Modified;
                await db.SaveChangesAsync();
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
            AppointmentDiary appointmentDiary = await db.AppointmentDiary.FindAsync(id);
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
            AppointmentDiary appointmentDiary = await db.AppointmentDiary.FindAsync(id);
            db.AppointmentDiary.Remove(appointmentDiary);
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
