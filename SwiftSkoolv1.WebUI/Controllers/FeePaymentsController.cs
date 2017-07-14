using SwiftSkoolv1.Domain;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class FeePaymentsController : BaseController
    {

        // GET: FeePayments
        public async Task<ActionResult> Index()
        {
            var feePayments = Db.FeePayments.Include(f => f.Students);
            if (Request.IsAuthenticated && !User.IsInRole("SuperAdmin"))
            {
                return View(Db.FeePayments.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool)));
            }
            ViewBag.Term = new SelectList(Db.Terms.AsNoTracking(), "TermName", "TermName");
            return View(await feePayments.ToListAsync());
        }

        public async Task<ActionResult> DebtorsList()
        {
            decimal value = 1m;
            var feePayments = Db.FeePayments.AsNoTracking()
                .Include(f => f.Students).Where(s => s.Remaining > value);

            ViewBag.Term = new SelectList(Db.Terms.AsNoTracking(), "TermName", "TermName");

            return View(await feePayments.ToListAsync());
        }

        // GET: FeePayments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var feePayment = await Db.FeePayments.AsNoTracking()
                .Where(f => f.FeePaymentId == id).SingleOrDefaultAsync(); //optimized details method
            if (feePayment == null)
            {
                return HttpNotFound();
            }
            return View(feePayment);
        }

        // GET: FeePayments/Create
        // Optimized Create method
        public ActionResult Create()
        {
            DateTime datetime = new DateTime();
            datetime = DateTime.Now.Date;
            ViewBag.Date = datetime.ToShortDateString();
            ViewBag.StudentId = new SelectList(Db.Students.AsNoTracking().Select(s => new
            {
                StudentID = s.StudentId,
                FullName = s.FullName
            }), "StudentID", "FullName");
            ViewBag.FeeName = new SelectList(Db.FeeTypes.AsNoTracking().Select(f => new
            {
                FeeName = f.FeeName
            }), "FeeName", "FeeName");
            ViewBag.Session = new SelectList(Db.Sessions.AsNoTracking().Select(s => new
            {
                SessionName = s.SessionName

            }), "SessionName", "SessionName");
            ViewBag.Term = new SelectList(Db.Terms.AsNoTracking().Select(t => new
            {
                TermName = t.TermName

            }), "TermName", "TermName");
            return View();
        }

        // POST: FeePayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,StudentId,FeeName,Term,Session,PaidFee,TotalAmount,PaymentMode,Date")] FeePayment feePayment)
        {
            if (ModelState.IsValid)
            {
                feePayment.SchoolId = userSchool;
                Db.FeePayments.Add(feePayment);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            DateTime datetime = new DateTime();
            datetime = DateTime.Now.Date;
            ViewBag.Date = datetime.ToShortDateString();
            ViewBag.StudentId = new SelectList(Db.Students.AsNoTracking(), "StudentID", "FullName");
            ViewBag.FeeName = new SelectList(Db.FeeTypes.AsNoTracking(), "FeeName", "FeeName");
            ViewBag.Session = new SelectList(Db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.Term = new SelectList(Db.Terms.AsNoTracking(), "TermName", "TermName");
            return View(feePayment);
        }

        // GET: FeePayments/Edit/5
        //optimized edit action
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var feePayment = await Db.FeePayments.AsNoTracking()
                        .Where(f => f.FeePaymentId == id).SingleOrDefaultAsync();
            if (feePayment == null)
            {
                return HttpNotFound();
            }
            DateTime datetime = new DateTime();
            datetime = DateTime.Now.Date;
            ViewBag.Date = datetime.ToShortDateString();
            ViewBag.StudentId = new SelectList(Db.Students, "StudentID", "FullName", feePayment.StudentId);
            ViewBag.FeeName = new SelectList(Db.FeeTypes, "FeeName", "FeeName");

            ViewBag.Session = new SelectList(Db.Sessions, "SessionName", "SessionName");
            return View(feePayment);
        }

        // POST: FeePayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,StudentId,FeeName,Term,Session,PaidFee,TotalAmount,PaymentMode,Date")] FeePayment feePayment)
        {
            if (ModelState.IsValid)
            {
                feePayment.SchoolId = userSchool;
                Db.Entry(feePayment).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            DateTime datetime = new DateTime();
            datetime = DateTime.Now.Date;
            ViewBag.Date = datetime.ToShortDateString();
            ViewBag.StudentId = new SelectList(Db.Students, "StudentID", "FullName", feePayment.StudentId);
            ViewBag.FeeName = new SelectList(Db.FeeTypes, "FeeName", "FeeName");

            ViewBag.Session = new SelectList(Db.Sessions, "SessionName", "SessionName");

            return View(feePayment);
        }

        // GET: FeePayments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeePayment feePayment = await Db.FeePayments.FindAsync(id);
            if (feePayment == null)
            {
                return HttpNotFound();
            }
            return View(feePayment);
        }

        // POST: FeePayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            FeePayment feePayment = await Db.FeePayments.FindAsync(id);
            Db.FeePayments.Remove(feePayment);
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
