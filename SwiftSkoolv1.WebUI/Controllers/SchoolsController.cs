using SwiftSkoolv1.Domain;
using SwiftSkoolv1.WebUI.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    //[CustomAuthorize(Roles = RoleName.SuperAdmin)]
    public class SchoolsController : BaseController
    {
        // GET: Schools
        public async Task<ActionResult> Index()
        {
            return View(await Db.Schools.AsNoTracking().ToListAsync());
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

            var v = await Db.Schools.AsNoTracking().Select(s => new
            {
                s.SchoolId,
                s.Name,
                s.Alias,
                s.Address,
                s.OwernshipType,
            }).ToListAsync();
            if (User.IsInRole("Admin"))
            {
                v = v.Where(x => x.SchoolId.Equals(userSchool)).ToList();
            }

            if (!string.IsNullOrEmpty(search))
            {
                //v = v.OrderBy(sortColumn + " " + sortColumnDir);
                v = v.Where(x => x.Name.Equals(search) || x.Alias.Equals(search)
                                 || x.OwernshipType.Equals(search)).ToList();


            }

            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data },
                JsonRequestBehavior.AllowGet);

            #endregion

            //return Json(new { data = await Db.Subjects.AsNoTracking().Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToListAsync() }, JsonRequestBehavior.AllowGet);
        }
        public async Task<PartialViewResult> PartialDetails(string id)
        {
            //var user = await Db.Users.AsNoTracking().Where(c => c.Id.Equals(username)).Select(c => c.Email).FirstOrDefaultAsync();
            var school = await Db.Schools.FindAsync(id);

            return PartialView(school);
        }

        // GET: Schools/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                id = userSchool;
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
                // return new JsonResult { Data = new { status = true, message = "School Created Successfully" } };
            }

            return new JsonResult { Data = new { status = false, message = "Check your inputs and try again" } };
        }

        // GET: Schools/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                id = userSchool;
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
            var currentsch = await Db.Schools.FindAsync(model.SchoolId);
            if (ModelState.IsValid && currentsch != null)
            {
                currentsch.SchoolId = model.SchoolId;
                currentsch.Name = model.Name;
                currentsch.Alias = model.Alias;
                currentsch.Address = model.Address;
                currentsch.LocalGovtArea = model.LocalGovtArea.ToString();
                currentsch.Color = model.Color.ToString();
                currentsch.OwernshipType = model.OwernshipType.ToString();
                currentsch.DateOfEstablishment = model.DateOfEstablishment;
                currentsch.Logo = model.Logo;
                currentsch.SchoolBanner = model.SchoolBanner;

                Db.Entry(currentsch).State = EntityState.Modified;
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
        [AllowAnonymous]
        public async Task<ActionResult> RenderBanner(string schoolId)
        {
            var school = await Db.Schools.FindAsync(schoolId);

            byte[] photoBack = school.SchoolBanner;

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
