using SwiftSkoolv1.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class CaSetUpsController : BaseController
    {
        // GET: CaSetUps
        public async Task<ActionResult> Index(int? ClassId, string TermName)
        {
            var caSetUps = Db.CaSetUps.Include(c => c.Class).Include(c => c.Term)
                                .Where(x => x.SchoolId.Equals(userSchool));
            if (ClassId != null && !string.IsNullOrEmpty(TermName))
            {
                caSetUps = caSetUps.Where(x => x.Term.TermName.Equals(TermName)
                                               && x.ClassId.Equals((int)ClassId));
            }
            else if (!string.IsNullOrEmpty(TermName))
            {
                caSetUps = caSetUps.Where(x => x.Term.TermName.Equals(TermName)
                                               || x.ClassId.Equals((int)ClassId));
            }

            ViewBag.ClassId = new SelectList(await _query.ClassListAsync(userSchool), "ClassId", "FullClassName");
            ViewBag.TermName = new SelectList(Db.Terms.AsNoTracking(), "TermName", "TermName");
            return View(await caSetUps.OrderBy(o => o.CaOrder).ToListAsync());
        }

        public async Task<ActionResult> SelectEdit()
        {
            ViewBag.ClassId = new SelectList(await _query.ClassListAsync(userSchool), "ClassId", "FullClassName");
            ViewBag.TermId = new SelectList(_query.TermList(), "TermName", "TermName");
            return View();
        }

        public async Task<ActionResult> EditAll(int ClassId, string TermId)
        {
            var caSetUps = Db.CaSetUps.Include(c => c.Class).Include(c => c.Term)
                            .Where(x => x.SchoolId.Equals(userSchool) && x.ClassId.Equals(ClassId)
                            && x.Term.TermName.Equals(TermId));
            ViewBag.ClassId = new SelectList(await _query.ClassListAsync(userSchool), "ClassId", "FullClassName");
            ViewBag.TermId = new SelectList(_query.TermList(), "TermId", "TermName");
            return View(await caSetUps.ToListAsync());
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAll(List<CaSetUp> model)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in model)
                {
                    //var caSetUp = new CaSetUp()
                    //{
                    //    CaSetUpId = item.CaSetUpId,
                    //    CaOrder = item.CaOrder,
                    //    IsTrue = item.IsTrue,
                    //    CACaption = item.CACaption,
                    //    MaximumScore = item.MaximumScore,
                    //    TermId = item.TermId,
                    //    SchoolClassId = item.SchoolClassId,
                    //    SchoolId = userSchool
                    //};

                    item.SchoolId = userSchool;
                    Db.Entry(item).State = EntityState.Modified;
                }
                await Db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            ViewBag.ClassId = new SelectList(await _query.ClassListAsync(userSchool), "ClassId", "FullClassName");
            ViewBag.TermId = new SelectList(_query.TermList(), "TermId", "TermName");
            return View(model);
        }

        // GET: CaSetUps/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var caSetUp = await Db.CaSetUps.FindAsync(id);
            if (caSetUp == null)
            {
                return HttpNotFound();
            }
            return View(caSetUp);
        }

        // GET: CaSetUps/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.ClassId = new SelectList(await _query.ClassListAsync(userSchool), "ClassId", "FullClassName");
            ViewBag.TermId = new SelectList(Db.Terms, "TermId", "TermName");
            return View();
        }

        // POST: CaSetUps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CaSetUpId,CaOrder,CACaption,MaximumScore,IsTrue,ClassId,TermId")] CaSetUp caSetUp)
        {
            if (ModelState.IsValid)
            {
                caSetUp.SchoolId = userSchool;
                Db.CaSetUps.Add(caSetUp);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ClassId = new SelectList(await _query.ClassListAsync(userSchool), "ClassId", "FullClassName");
            ViewBag.TermId = new SelectList(Db.Terms, "TermId", "TermName");
            return View(caSetUp);
        }

        // GET: CaSetUps/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaSetUp caSetUp = await Db.CaSetUps.FindAsync(id);
            if (caSetUp == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassId = new SelectList(await _query.ClassListAsync(userSchool), "ClassId", "FullClassName");
            ViewBag.TermId = new SelectList(Db.Terms, "TermId", "TermName");
            return View(caSetUp);
        }

        // POST: CaSetUps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CaSetUpId,CaOrder,CACaption,MaximumScore,IsTrue,ClassId,TermId")] CaSetUp caSetUp)
        {
            if (ModelState.IsValid)
            {
                caSetUp.SchoolId = userSchool;
                Db.Entry(caSetUp).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ClassId = new SelectList(await _query.ClassListAsync(userSchool), "ClassId", "FullClassName");
            ViewBag.TermId = new SelectList(Db.Terms, "TermId", "TermName");
            return View(caSetUp);
        }

        // GET: CaSetUps/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var caSetUp = await Db.CaSetUps.FindAsync(id);
            if (caSetUp == null)
            {
                return HttpNotFound();
            }
            return View(caSetUp);
        }

        // POST: CaSetUps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var caSetUp = await Db.CaSetUps.FindAsync(id);
            if (caSetUp != null) Db.CaSetUps.Remove(caSetUp);
            await Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<JsonResult> MaximumValidation(List<double> MaximumScore, List<int> CaSetUpId, List<int> ClassId, List<int> TermId)
        {
            //bool myValue = ExamCa < 50;
            string maximumScore = string.Empty;
            string classId = string.Empty;
            string termId = string.Empty;
            string caSetUpId = string.Empty;

            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("maximumscore")) != null)
            {
                maximumScore =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("maximumscore"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("classid")) != null)
            {
                classId =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("classid"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("termid")) != null)
            {
                termId =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("termid"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("casetupid")) != null)
            {
                caSetUpId =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("casetupid"))];
            }
            int newClassId = Convert.ToInt32(classId);
            int newTermId = Convert.ToInt32(termId);
            int newCastupId = Convert.ToInt32(caSetUpId);
            var setUpSum = Db.CaSetUps.AsNoTracking().Where(x => x.IsTrue.Equals(true)
                                                                && x.CaSetUpId != newCastupId
                                                               && x.SchoolId.Equals(userSchool)
                                                               && x.ClassId.Equals(newClassId)
                                                               && x.TermId.Equals(newTermId))
                                                                .Sum(s => s.MaximumScore);

            var totalValue = setUpSum + Convert.ToDouble(maximumScore);


            if (totalValue > 100)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            var caSetUp = await Db.CaSetUps.FindAsync(newCastupId);
            caSetUp.SchoolId = userSchool;
            caSetUp.MaximumScore = Convert.ToDouble(maximumScore);
            Db.Entry(caSetUp).State = EntityState.Modified;
            await Db.SaveChangesAsync();
            return Json(true, JsonRequestBehavior.AllowGet);


            // return Json(false, JsonRequestBehavior.AllowGet);
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
