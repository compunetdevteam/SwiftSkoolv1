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
    public class FeeCategoriesController : BaseController
    {

        // GET: FeeCategories
        public async Task<ActionResult> Index()
        {
            return View(await Db.FeeCategories.AsNoTracking().ToListAsync());
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
            var v = Db.FeeCategories.Where(x => x.SchoolId == userSchool).Select(s => new { s.FeeCategoryId, s.CategoryName, s.CategoryDescription }).ToList();

            //var v = Db.Subjects.Where(x => x.SchoolId.Equals(userSchool)).Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToList();
            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            //{
            //    //v = v.OrderBy(sortColumn + " " + sortColumnDir);
            //    v = new List<Subject>(v.OrderBy(x => "sortColumn + \" \" + sortColumnDir"));
            //}
            if (!string.IsNullOrEmpty(search))
            {
                //v = v.OrderBy(sortColumn + " " + sortColumnDir);
                v = Db.FeeCategories.Where(x => x.SchoolId.Equals(userSchool) && (x.CategoryName.Equals(search) || x.CategoryDescription.Equals(search)))
                    .Select(s => new { s.FeeCategoryId, s.CategoryName, s.CategoryDescription }).ToList();
            }
            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
            #endregion

            //return Json(new { data = await Db.Subjects.AsNoTracking().Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToListAsync() }, JsonRequestBehavior.AllowGet);
        }






        public async Task<FeeCategory> FindFeeCategoryAsync(int? id)
        {
            return await Db.FeeCategories.AsNoTracking().Where(fc => fc.FeeCategoryId.Equals(id))
                                              .SingleOrDefaultAsync();
        }

        // GET: FeeCategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var feeCategory = await FindFeeCategoryAsync(id);
            if (feeCategory == null)
            {
                return HttpNotFound();
            }
            return View(feeCategory);
        }

        // GET: FeeCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FeeCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FeeCategoryVm feeCategory)
        {
            if (Db.FeeCategories.Any(fc => fc.CategoryName.Equals(feeCategory.CategoryName)))
            {
                ModelState.AddModelError("Error", "A Fee Category already exists with " +
                                                  "the name you have supplied or your or the name is blank! " +
                                                  "Please fill a valid name for the category!");
                return View(feeCategory);
            }

            if (ModelState.IsValid)
            {
                var model = new FeeCategory
                {
                    CategoryName = feeCategory.CategoryName,
                    CategoryDescription = feeCategory.CategoryDescription
                };

                Db.FeeCategories.Add(model);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(feeCategory);
        }

        // GET: FeeCategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var feeCategory = await FindFeeCategoryAsync(id);
            if (feeCategory == null)
            {
                return HttpNotFound();
            }
            var model = new FeeCategoryVm
            {
                Id = feeCategory.FeeCategoryId,
                CategoryName = feeCategory.CategoryName,
                CategoryDescription = feeCategory.CategoryDescription
            };
            return View(model);
        }

        // POST: FeeCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(FeeCategoryVm feeCategory)
        {
            if (string.IsNullOrWhiteSpace(feeCategory.CategoryName) || string.IsNullOrEmpty(feeCategory.CategoryName))
            {
                ModelState.AddModelError("Error", "You cannot have blank categories in the application!");
                return View(feeCategory);
            }
            if (ModelState.IsValid)
            {
                var model = await Db.FeeCategories.FindAsync(feeCategory.Id);

                Db.Entry(model).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(feeCategory);
        }

        public async Task<PartialViewResult> Save(int id)
        {
            var feeCategory = await Db.FeeCategories.FindAsync(id);
            return PartialView(feeCategory);
        }

        // POST: Subjects/Save/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(FeeCategory feeCategory)
        {
            bool status = false;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (feeCategory.FeeCategoryId > 0)
                {
                    feeCategory.SchoolId = userSchool;
                    Db.Entry(feeCategory).State = EntityState.Modified;
                    message = "Fee Category Updated Successfully...";
                }
                else
                {
                    feeCategory.SchoolId = userSchool;
                    Db.FeeCategories.Add(feeCategory);
                    message = "Fee Category Created Successfully...";

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
            FeeCategory feeCategory = await Db.FeeCategories.FindAsync(id);

            return PartialView(feeCategory);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            bool status = false;
            string message = string.Empty;
            var feeCategory = await Db.FeeCategories.FindAsync(id);
            if (feeCategory != null)
            {
                Db.FeeCategories.Remove(feeCategory);
                await Db.SaveChangesAsync();
                status = true;
                message = "Fee Category Deleted Successfully...";
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
