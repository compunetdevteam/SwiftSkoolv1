using SwiftSkoolv1.Domain;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class ParentEmailAddressesController : BaseController
    {
        // GET: ParentEmailAddresses
        public async Task<ActionResult> Index()
        {
            return View(await Db.ParentEmailAddresses.ToListAsync());
        }

        public async Task<ActionResult> GetIndex()
        {
            // dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            var data = await Db.ParentEmailAddresses.AsNoTracking().Select(s => new
            {
                s.StudentId,
                s.EmailAddress
            }).ToListAsync();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public async Task<PartialViewResult> Save(int id)
        {
            var pEmailAddress = await Db.ParentEmailAddresses.FindAsync(id);
            return PartialView(pEmailAddress);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(ParentEmailAddress model)
        {
            bool status = false;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (model.ParentEmailAddressId > 0)
                {
                    var pEmail = await Db.ParentEmailAddresses.FindAsync(model.ParentEmailAddressId);
                    if (pEmail != null)
                    {
                        pEmail.StudentId = model.StudentId;
                        pEmail.SchoolId = model.SchoolId;
                        pEmail.EmailAddress = model.EmailAddress;
                        Db.Entry(pEmail).State = EntityState.Modified;
                        await Db.SaveChangesAsync();
                        message = $"{model.StudentId} Updated Successfully...";
                        return new JsonResult { Data = new { status = true, message = message } };
                    }
                }
                else
                {
                    model.SchoolId = userSchool;
                    Db.ParentEmailAddresses.Add(model);
                    await Db.SaveChangesAsync();
                    message = $"{model.StudentId} Added Successfully.";
                    return new JsonResult { Data = new { status = true, message = message } };
                }
            }
            return new JsonResult { Data = new { status = status, message = message } };
            //return View(subject);
        }


        // GET: ParentEmailAddresses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParentEmailAddress parentEmailAddress = await Db.ParentEmailAddresses.FindAsync(id);
            if (parentEmailAddress == null)
            {
                return HttpNotFound();
            }
            return View(parentEmailAddress);
        }

        // GET: ParentEmailAddresses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ParentEmailAddresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ParentEmailAddressId,StudentId,EmailAddress,IsActive,SchoolId")] ParentEmailAddress parentEmailAddress)
        {
            if (ModelState.IsValid)
            {
                Db.ParentEmailAddresses.Add(parentEmailAddress);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(parentEmailAddress);
        }

        // GET: ParentEmailAddresses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParentEmailAddress parentEmailAddress = await Db.ParentEmailAddresses.FindAsync(id);
            if (parentEmailAddress == null)
            {
                return HttpNotFound();
            }
            return View(parentEmailAddress);
        }

        // POST: ParentEmailAddresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ParentEmailAddressId,StudentId,EmailAddress,IsActive,SchoolId")] ParentEmailAddress parentEmailAddress)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(parentEmailAddress).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(parentEmailAddress);
        }

        // GET: ParentEmailAddresses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParentEmailAddress parentEmailAddress = await Db.ParentEmailAddresses.FindAsync(id);
            if (parentEmailAddress == null)
            {
                return HttpNotFound();
            }
            return View(parentEmailAddress);
        }

        // POST: ParentEmailAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ParentEmailAddress parentEmailAddress = await Db.ParentEmailAddresses.FindAsync(id);
            Db.ParentEmailAddresses.Remove(parentEmailAddress);
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
