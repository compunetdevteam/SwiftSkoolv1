using SwiftSkool.Models;
using SwiftSkool.Services;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkool.Controllers
{
    [CustomAuthorize(Roles = RoleName.SuperAdmin)]
    public class SchoolsController : BaseController
    {
        // GET: Schools
        public async Task<ActionResult> Index()
        {
            return View(await Db.Schools.AsNoTracking().ToListAsync());
        }

        // GET: Schools/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var school = await Db.Schools.FindAsync(id);
            if (school == null)
            {
                return HttpNotFound();
            }
            return View(school);
        }

        // GET: Schools/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Schools/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SchoolVm model)
        {
            if (ModelState.IsValid)
            {
                var school = new School
                {
                    SchoolId = model.SchoolId,
                    Name = model.Name,
                    Alias = model.Alias,
                    Address = model.Address,
                    LocalGovtArea = model.LocalGovtArea.ToString(),
                    Color = model.Color.ToString(),
                    OwernshipType = model.OwernshipType.ToString(),
                    DateOfEstablishment = model.DateOfEstablishment,
                    Logo = model.Logo,
                    SchoolBanner = model.SchoolBanner
                };
                Db.Schools.Add(school);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Schools/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = await Db.Schools.FindAsync(id);

            if (model == null)
            {
                return HttpNotFound();
            }
            var school = new SchoolVm()
            {
                SchoolId = model.SchoolId,
                Name = model.Name,
                Alias = model.Alias,
                Address = model.Address,
                DateOfEstablishment = model.DateOfEstablishment,
                //Color = model.Color.ToString(),
                Logo = model.Logo,
                SchoolBanner = model.SchoolBanner
            };

            return View(school);
        }

        // POST: Schools/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SchoolVm model)
        {

            if (ModelState.IsValid)
            {
                var school = new School
                {
                    SchoolId = model.SchoolId,
                    Name = model.Name,
                    Alias = model.Alias,
                    Address = model.Address,
                    LocalGovtArea = model.LocalGovtArea.ToString(),
                    Color = model.Color.ToString(),
                    OwernshipType = model.OwernshipType.ToString(),
                    DateOfEstablishment = model.DateOfEstablishment,
                    Logo = model.Logo,
                    SchoolBanner = model.SchoolBanner
                };
                Db.Entry(school).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Schools/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var school = await Db.Schools.FindAsync(id);
            if (school == null)
            {
                return HttpNotFound();
            }
            return View(school);
        }

        // POST: Schools/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var school = await Db.Schools.FindAsync(id);
            if (school != null) Db.Schools.Remove(school);
            await Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public async Task<ActionResult> RenderImage(string studentId)
        {
            var school = await Db.Schools.FindAsync(studentId);

            byte[] photoBack = school.Logo;

            return File(photoBack, "image/png");
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
