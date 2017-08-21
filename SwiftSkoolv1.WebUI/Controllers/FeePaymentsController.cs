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
            var v = Db.FeePayments.Where(x => x.SchoolId == userSchool).Select(s => new
                                                                                    {
                                                                                        s.FeePaymentId, s.StudentId, s.FeeName, 
                                                                                         s.Term, s.Session, s.PaidFee,
                                                                                         s.TotalAmount, s.PaymentMode, s.Date, s.Remaining 
                                                                                    }).ToList();

            //var v = Db.Subjects.Where(x => x.SchoolId.Equals(userSchool)).Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToList();
            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            //{
            //    //v = v.OrderBy(sortColumn + " " + sortColumnDir);
            //    v = new List<Subject>(v.OrderBy(x => "sortColumn + \" \" + sortColumnDir"));
            //}
            if (!string.IsNullOrEmpty(search))
            {
                //v = v.OrderBy(sortColumn + " " + sortColumnDir);
                v = Db.FeePayments.Where(x => x.SchoolId.Equals(userSchool) && (x.StudentId.Equals(search) || x.FeeName.Equals(search) || x.Term.Equals(search) || x.Session.Equals(search)))
                                                                                    .Select(s => new {
                                                                                        s.FeePaymentId,
                                                                                        s.StudentId,
                                                                                        s.FeeName,
                                                                                        s.Term,
                                                                                        s.Session,
                                                                                        s.PaidFee,
                                                                                        s.TotalAmount,
                                                                                        s.PaymentMode,
                                                                                        s.Date,
                                                                                        s.Remaining
                                                                                    }).ToList();
            }
            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
            #endregion

            //return Json(new { data = await Db.Subjects.AsNoTracking().Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToListAsync() }, JsonRequestBehavior.AllowGet);
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

        // GET: Subjects/Save/5
        public async Task<PartialViewResult> Save(int id)
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

            var payments = await Db.FeePayments.FindAsync(id);
           

            return PartialView(payments);
        }

        // POST: Subjects/Save/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(FeePayment payment)
        {
            bool status = false;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (payment.FeePaymentId > 0)
                {
                    payment.SchoolId = userSchool;
                    Db.Entry(payment).State = EntityState.Modified;
                    message = "Payment Updated Successfully...";
                }
                else
                {
                    payment.SchoolId = userSchool;
                    Db.FeePayments.Add(payment);
                    message = "Payment Created Successfully...";

                }
                await Db.SaveChangesAsync();
                status = true;
            }
            return new JsonResult { Data = new { status = status, message = message } };
            //return View(subject);
        }

        // GET: Subjects/Delete/5
        public async Task<PartialViewResult> Delete(int id)
        {
            FeePayment payment = await Db.FeePayments.FindAsync(id);

            return PartialView(payment);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            bool status = false;
            string message = string.Empty;
            var payment = await Db.FeePayments.FindAsync(id);
            if (payment != null)
            {
                Db.FeePayments.Remove(payment);
                await Db.SaveChangesAsync();
                status = true;
                message = "Payment Deleted Successfully...";
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
