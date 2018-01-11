using SwiftSkoolv1.Domain;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class SchoolClassesController : BaseController
    {


        // GET: SchoolClasses
        public ActionResult Index()
        {
            // return View(await Db.SchoolClasses.AsNoTracking().ToListAsync());
            return View();
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

            var v = await Db.SchoolClasses.Where(x => x.SchoolId.Equals(userSchool)).AsNoTracking().ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                //v = v.OrderBy(sortColumn + " " + sortColumnDir);
                v = Db.SchoolClasses.AsNoTracking().Where(x => x.ClassName.Equals(search) || x.ClassCode.Equals(search))
                    .ToList();
            }
            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
            #endregion

            //return Json(new { data = await Db.Subjects.AsNoTracking().Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToListAsync() }, JsonRequestBehavior.AllowGet);
        }



        public async Task<PartialViewResult> Save(int id)
        {
            var classes = await Db.SchoolClasses.FindAsync(id);
            return PartialView(classes);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(SchoolClass model)
        {
            bool status = false;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (model.SchoolClassId > 0)
                {
                    model.SchoolId = userSchool;
                    Db.Entry(model).State = EntityState.Modified;
                    message = "School Class Updated Successfully...";
                }
                else
                {
                    //var myClass = Db.SchoolClasses.Where(x => x.ClassCode.Equals(model.ClassCode));

                    //if (myClass.Any())
                    //{
                    //    return new JsonResult { Data = new { status = false, message = "School Class Already Exist in Database" } };
                    //}
                    model.SchoolId = userSchool;
                    Db.SchoolClasses.Add(model);
                }
                await Db.SaveChangesAsync();
                return new JsonResult { Data = new { status = true, message = "School Class Created Successfully..." } };

            }
            return new JsonResult { Data = new { status = false, message = "Data not Saved, Please try again" } };
            //return View(subject);
        }


        // GET: SchoolClasses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchoolClass schoolClass = await Db.SchoolClasses.FindAsync(id);
            if (schoolClass == null)
            {
                return HttpNotFound();
            }
            return View(schoolClass);
        }

        // GET: SchoolClasses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SchoolClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ClassCode,ClassName")] SchoolClass model)
        {
            if (ModelState.IsValid)
            {
                var myClass = await Db.SchoolClasses.AsNoTracking()
                                .CountAsync(x => x.ClassCode.Equals(model.ClassCode.Trim()));

                if (myClass >= 1)
                {
                    TempData["UserMessage"] = "School Class Already Exist in Database";
                    TempData["Title"] = "Error.";
                    return View(model);
                }
                var schoolClass = new SchoolClass()
                {
                    ClassName = model.ClassName.Trim(),
                    ClassCode = model.ClassCode.Trim(),
                    SchoolId = userSchool
                };

                Db.SchoolClasses.Add(schoolClass);

                await Db.SaveChangesAsync();
                TempData["UserMessage"] = "School Class Created Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            TempData["UserMessage"] = "Some Values are not entered Correctly";
            TempData["Title"] = "Error.";
            return View(model);
        }

        // GET: SchoolClasses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchoolClass schoolClass = await Db.SchoolClasses.FindAsync(id);
            if (schoolClass == null)
            {
                return HttpNotFound();
            }
            return View(schoolClass);
        }

        // POST: SchoolClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SchoolClass schoolClass)
        {
            if (ModelState.IsValid)
            {
                schoolClass.SchoolId = userSchool;
                Db.Entry(schoolClass).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                TempData["UserMessage"] = "School Class Updated Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            return View(schoolClass);
        }



        // GET: Subjects/Delete/5
        public async Task<PartialViewResult> Delete(int id)
        {

            var subject = await Db.SchoolClasses.FindAsync(id);

            return PartialView(subject);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            bool status = false;
            string message = string.Empty;
            var schoolClass = await Db.SchoolClasses.FindAsync(id);
            if (schoolClass != null)
            {
                Db.SchoolClasses.Remove(schoolClass);
                await Db.SaveChangesAsync();
                status = true;
                message = "School Class Deleted Successfully...";
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
