using SwiftSkoolv1.Domain;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class FeeTypesController : BaseController
    {
        // GET: FeeTypes
        public async Task<ActionResult> Index()
        {
            var feeTypes = Db.FeeTypes.Include(f => f.School);
            return View(await feeTypes.ToListAsync());
        }

        public async Task<ActionResult> GetIndex()
        {
            var v = Db.FeeTypes.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool))
                .Select(s => new
                {
                    s.ClassName,
                    s.TermName,
                    s.FeeCategory,
                    s.Amount,
                    s.Description,
                    s.FeeName,
                    s.FeeTypeId,
                    s.StudentType
                }).ToList();


            var data = v.ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);

        }

        // GET: FeeTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeeType feeType = await Db.FeeTypes.FindAsync(id);
            if (feeType == null)
            {
                return HttpNotFound();
            }
            return View(feeType);
        }


        public async Task<PartialViewResult> Save(int id)
        {
            var feeTypes = await Db.FeeTypes.FindAsync(id);
            ViewBag.TermName = new SelectList(_query.TermList(), "TermName", "TermName");

            ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "ClassName", "ClassName");
            var studentType = from StudentType s in Enum.GetValues(typeof(StudentType))
                              select new { ID = s, Name = s.ToString() };

            ViewBag.StudentType = new SelectList(studentType, "Name", "Name");
            var feeCategory = from FeeCategory s in Enum.GetValues(typeof(FeeCategory))
                              select new { ID = s, Name = s.ToString() };

            ViewBag.FeeCategory = new MultiSelectList(feeCategory, "Name", "Name");

            return PartialView(feeTypes);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(FeeType model)
        {
            bool status = false;
            string message = string.Empty;
            if (ModelState.IsValid)
            {

                if (model.FeeTypeId > 0)
                {
                    var feeType = await Db.FeeTypes.FindAsync(model.FeeTypeId);
                    if (feeType != null)
                    {
                        feeType.SchoolId = userSchool;
                        feeType.ClassName = model.ClassName;
                        feeType.FeeCategory = model.FeeCategory;
                        feeType.FeeName = model.FeeName;
                        feeType.TermName = model.TermName;
                        feeType.Amount = model.Amount;
                        feeType.AmountInWords = model.AmountInWords;
                        feeType.Description = model.Description;
                        feeType.StudentType = model.StudentType;
                        Db.Entry(feeType).State = EntityState.Modified;
                    }
                    await Db.SaveChangesAsync();
                    message = $"{model.FeeName} Updated Successfully...";
                    return new JsonResult { Data = new { status = true, message = message } };
                }
                else
                {

                    var feeType = new FeeType
                    {
                        ClassName = model.ClassName,
                        FeeCategory = model.FeeCategory,
                        FeeName = model.FeeName,
                        TermName = model.TermName,
                        SchoolId = userSchool,
                        Amount = model.Amount,
                        AmountInWords = model.AmountInWords,
                        Description = model.Description,
                        StudentType = model.StudentType
                    };
                    Db.FeeTypes.Add(feeType);
                    message = $" You have created {model.FeeName} Successfully.";
                    await Db.SaveChangesAsync();
                    return new JsonResult { Data = new { status = true, message = message } };
                }
            }
            return new JsonResult { Data = new { status = status, message = message } };
            //return View(subject);
        }


        #region Unused Code

        //// GET: FeeTypes/Create
        //public ActionResult Create()
        //{
        //    ViewBag.SchoolId = new SelectList(Db.Schools, "SchoolId", "Name");
        //    return View();
        //}

        //// POST: FeeTypes/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create(FeeType feeType)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Db.FeeTypes.Add(feeType);
        //        await Db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.SchoolId = new SelectList(Db.Schools, "SchoolId", "Name", feeType.SchoolId);
        //    return View(feeType);
        //}

        //// GET: FeeTypes/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    FeeType feeType = await Db.FeeTypes.FindAsync(id);
        //    if (feeType == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.SchoolId = new SelectList(Db.Schools, "SchoolId", "Name", feeType.SchoolId);
        //    return View(feeType);
        //}

        //// POST: FeeTypes/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit(FeeType feeType)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Db.Entry(feeType).State = EntityState.Modified;
        //        await Db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.SchoolId = new SelectList(Db.Schools, "SchoolId", "Name", feeType.SchoolId);
        //    return View(feeType);
        //} 
        #endregion

        // GET: FeeTypes/Delete/5
        public async Task<PartialViewResult> Delete(int id)
        {
            FeeType feeType = await Db.FeeTypes.FindAsync(id);

            return PartialView(feeType);
        }

        // POST: FeeTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            FeeType feeType = await Db.FeeTypes.FindAsync(id);
            if (feeType != null) Db.FeeTypes.Remove(feeType);
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
