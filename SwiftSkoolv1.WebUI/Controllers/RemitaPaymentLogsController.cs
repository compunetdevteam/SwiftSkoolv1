using SwiftSkoolv1.Domain;
using SwiftSkoolv1.WebUI.ViewModels.RemitaVm;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class RemitaPaymentLogsController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetIndex()
        {
            // dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            var data = await Db.RemitaPaymentLogs.AsNoTracking().Select(s => new
            {
                s.OrderId,
                s.Rrr,
                s.Amount,
                s.FeeCategory,
                s.PaymentName,
                s.TransactionMessage,
                s.RemitaPaymentLogId,
                s.PaymentDate,
            }).OrderByDescending(s => s.PaymentDate).ToListAsync();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ConfirmationPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ConfirmationPage(ConfirmRrr model)
        {
            var remitaParam = Db.RemitaFeeSettings.AsNoTracking()
                .FirstOrDefault(s => s.SchoolId.Equals(userSchool));
            var hash = _query.HashRemitedRePost(remitaParam.MerchantId, model.rrr, remitaParam.ApiKey);
            var url = Url.Action("ConfrimRrrPayment", "RemitaPaymentLogs", new { },
                protocol: Request.Url.Scheme);
            var remitaRePostVm = new RemitaRePostVm
            {
                rrr = model.rrr,
                merchantId = remitaParam.MerchantId,
                hash = hash,
                responseurl = url
            };
            return RedirectToAction("SubmitConfirmationPage", remitaRePostVm);
        }

        public ActionResult SubmitConfirmationPage(RemitaRePostVm model)
        {
            return View(model);
        }

        public ActionResult ConfrimRrrPayment(RemitaResponse model)
        {
            return View(model);
        }

        //public ActionResult ConfrimRrrPayment(string RRR, string orderID)
        //{

        //    var hashed = _query.HashRemitedValidate(orderID, RemitaConfigParams.APIKEY, RemitaConfigParams.MERCHANTID);
        //    string url = RemitaConfigParams.CHECKSTATUSURL + "/" + RemitaConfigParams.MERCHANTID + "/" + orderID + "/" + hashed + "/" + "orderstatus.reg";
        //    string jsondata = new WebClient().DownloadString(url);
        //    RemitaResponse result = JsonConvert.DeserializeObject<RemitaResponse>(jsondata);
        //    return View(result);
        //}
        // GET: RemitaPaymentLogs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RemitaPaymentLog remitaPaymentLog = await Db.RemitaPaymentLogs.FindAsync(id);
            if (remitaPaymentLog == null)
            {
                return HttpNotFound();
            }
            return View(remitaPaymentLog);
        }

        // GET: RemitaPaymentLogs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RemitaPaymentLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RemitaPaymentLogId,OrderId,Amount,Rrr,StatusCode,TransactionMessage,PaymentDate,PaymentName,PayerName")] RemitaPaymentLog remitaPaymentLog)
        {
            if (ModelState.IsValid)
            {
                Db.RemitaPaymentLogs.Add(remitaPaymentLog);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(remitaPaymentLog);
        }

        // GET: RemitaPaymentLogs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RemitaPaymentLog remitaPaymentLog = await Db.RemitaPaymentLogs.FindAsync(id);
            if (remitaPaymentLog == null)
            {
                return HttpNotFound();
            }
            return View(remitaPaymentLog);
        }

        // POST: RemitaPaymentLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RemitaPaymentLogId,OrderId,Amount,Rrr,StatusCode,TransactionMessage,PaymentDate,PaymentName,PayerName")] RemitaPaymentLog remitaPaymentLog)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(remitaPaymentLog).State = System.Data.Entity.EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(remitaPaymentLog);
        }

        // GET: RemitaPaymentLogs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RemitaPaymentLog remitaPaymentLog = await Db.RemitaPaymentLogs.FindAsync(id);
            if (remitaPaymentLog == null)
            {
                return HttpNotFound();
            }
            return View(remitaPaymentLog);
        }

        // POST: RemitaPaymentLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            RemitaPaymentLog remitaPaymentLog = await Db.RemitaPaymentLogs.FindAsync(id);
            Db.RemitaPaymentLogs.Remove(remitaPaymentLog);
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
