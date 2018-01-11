using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using CaSetUp = SwiftSkoolv1.WebUI.Models.CaSetUp;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class CaSetUpsController : BaseController
    {
        // GET: CaSetUps
        public async Task<ActionResult> Index(string ClassId, string TermName)
        {
            var caSetUps = Db.CaSetUps.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool) && x.IsTrue.Equals(true));
            if (ClassId != null && !string.IsNullOrEmpty(TermName))
            {
                caSetUps = caSetUps.Where(x => x.TermName.Equals(TermName)
                                               && x.ClassName.Equals(ClassId));
            }
            else if (!string.IsNullOrEmpty(TermName))
            {
                caSetUps = caSetUps.Where(x => x.TermName.Equals(TermName)
                                               || x.ClassName.Equals(ClassId));
            }
            var className = Db.Classes.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool)).DistinctBy(x => x.ClassName).ToList();
            ViewBag.ClassName = new SelectList(className, "ClassName", "ClassName");
            //ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "ClassName", "ClassName");
            ViewBag.TermName = new SelectList(Db.Terms.AsNoTracking(), "TermName", "TermName");
            return View(await caSetUps.OrderBy(o => o.ClassName).ToListAsync());
        }


        public ActionResult GetIndex(string ClassName, string TermName)
        {
            #region Server Side filtering
            ////Get parameter for sorting from grid table
            //// get Start (paging start index) and length (page size for paging)
            //var draw = Request.Form.GetValues("draw").FirstOrDefault();
            //var start = Request.Form.GetValues("start").FirstOrDefault();
            //var length = Request.Form.GetValues("length").FirstOrDefault();
            ////Get Sort columns values when we click on Header Name of column
            ////getting column name
            //var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            ////Soring direction(either desending or ascending)
            //var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            //string search = Request.Form.GetValues("search[value]").FirstOrDefault();

            //int pageSize = length != null ? Convert.ToInt32(length) : 0;
            //int skip = start != null ? Convert.ToInt32(start) : 0;
            //int totalRecords = 0;

            ////var v = Db.Subjects.Where(x => x.SchoolId != userSchool).Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToList();
            //var v = Db.CaSetUps.AsNoTracking().Where(x => x.SchoolId == userSchool).ToList();

            ////var v = Db.Subjects.Where(x => x.SchoolId.Equals(userSchool)).Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToList();
            ////if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            ////{
            ////    //v = v.OrderBy(sortColumn + " " + sortColumnDir);
            ////    v = new List<Subject>(v.OrderBy(x => "sortColumn + \" \" + sortColumnDir"));
            ////}
            //if (!string.IsNullOrEmpty(search))
            //{
            //    //v = v.OrderBy(sortColumn + " " + sortColumnDir);
            //    v = Db.CaSetUps.Where(x => x.SchoolId.Equals(userSchool) &&
            //                               (x.CaCaption.Equals(search) || x.ClassName.Equals(search))).ToList();

            //}
            //if (!string.IsNullOrEmpty(ClassName) && !string.IsNullOrEmpty(TermName))
            //{
            //    v = Db.CaSetUps.Where(x => x.SchoolId.Equals(userSchool) &&
            //                               (x.ClassName.Equals(ClassName) && x.TermName.Equals(TermName))).ToList();
            //}
            //if (!string.IsNullOrEmpty(ClassName) || !string.IsNullOrEmpty(TermName))
            //{
            //    v = Db.CaSetUps.Where(x => x.SchoolId.Equals(userSchool) &&
            //                               (x.ClassName.Equals(ClassName) || x.TermName.Equals(TermName))).ToList();
            //}
            //totalRecords = v.Count();
            //var data = v.Skip(skip).Take(pageSize).ToList();

            //return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
            #endregion

            var v = Db.CaSetUps.AsNoTracking().Where(x => x.SchoolId == userSchool).ToList();
            if (!string.IsNullOrEmpty(ClassName) && !string.IsNullOrEmpty(TermName))
            {
                v = Db.CaSetUps.Where(x => x.SchoolId.Equals(userSchool) &&
                                           (x.ClassName.Equals(ClassName) && x.TermName.Equals(TermName))).ToList();
            }
            if (!string.IsNullOrEmpty(ClassName) || !string.IsNullOrEmpty(TermName))
            {
                v = Db.CaSetUps.Where(x => x.SchoolId.Equals(userSchool) &&
                                           (x.ClassName.Equals(ClassName) || x.TermName.Equals(TermName))).ToList();
            }

            return Json(new { data = v }, JsonRequestBehavior.AllowGet);
        }



        public async Task<PartialViewResult> Save(int id)
        {
            var className = Db.Classes.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool)).DistinctBy(x => x.ClassName).ToList();
            ViewBag.ClassId = new SelectList(className, "ClassName", "ClassName");
            ViewBag.TermId = new SelectList(Db.Terms, "TermId", "TermName");

            var caSetUp = await Db.CaSetUps.FindAsync(id);
            return PartialView(caSetUp);
        }

        // POST: Subjects/Save/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(CaSetUp caSetUp)
        {
            bool status = false;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (caSetUp.CaSetUpId > 0)
                {
                    caSetUp.SchoolId = userSchool;
                    Db.Entry(caSetUp).State = EntityState.Modified;
                    message = "CA Updated Successfully...";
                }
                else
                {
                    caSetUp.SchoolId = userSchool;
                    Db.CaSetUps.Add(caSetUp);
                    message = "CA Created Successfully...";

                }
                await Db.SaveChangesAsync();
                status = true;
            }
            return new JsonResult { Data = new { status = status, message = message } };
            //return View(subject);
        }




        public PartialViewResult SelectEdit()
        {
            var className = Db.Classes.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool)).DistinctBy(x => x.ClassName).ToList();
            ViewBag.ClassId = new SelectList(className, "ClassName", "ClassName");
            ViewBag.TermId = new SelectList(_query.TermList(), "TermName", "TermName");
            return PartialView();
        }

        public async Task<ActionResult> EditAll(string ClassId, string TermId)
        {

            var caSetUps = new List<CaSetUp>();

            var checkCa = Db.CaSetUps.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool)
                                                                && x.ClassName.Equals(ClassId) &&
                                                                x.TermName.Equals(TermId));
            if (checkCa.Any())
            {
                caSetUps = checkCa.ToList();
            }
            else
            {
                for (int i = 1; i < 11; i++)
                {
                    if (i == 1)
                    {
                        var caSetup = new CaSetUp
                        {
                            CaCaption = "1st Test",
                            CaOrder = i,
                            IsTrue = true,
                            TermName = TermId,
                            MaximumScore = 10.0,
                            CaPercentage = 10,
                            ClassName = ClassId
                        };
                        caSetUps.Add(caSetup);
                        caSetup.SchoolId = userSchool;
                        Db.CaSetUps.Add(caSetup);
                    }
                    else if (i == 2)
                    {
                        var caSetup = new CaSetUp
                        {
                            CaCaption = "2nd Test",
                            CaOrder = i,
                            IsTrue = true,
                            TermName = TermId,
                            MaximumScore = 10.0,
                            CaPercentage = 10,
                            ClassName = ClassId
                        };
                        caSetUps.Add(caSetup);
                        caSetup.SchoolId = userSchool;
                        Db.CaSetUps.Add(caSetup);
                    }
                    else if (i == 3)
                    {
                        var caSetup = new CaSetUp
                        {
                            CaCaption = "3rd Test",
                            CaOrder = i,
                            IsTrue = true,
                            TermName = TermId,
                            MaximumScore = 10.0,
                            CaPercentage = 10,
                            ClassName = ClassId
                        };
                        caSetUps.Add(caSetup);
                        caSetup.SchoolId = userSchool;
                        Db.CaSetUps.Add(caSetup);
                    }
                    else if (i == 10)
                    {
                        var caSetup = new CaSetUp
                        {
                            CaCaption = "Exam",
                            CaOrder = i,
                            IsTrue = true,
                            TermName = TermId,
                            MaximumScore = 70.0,
                            CaPercentage = 70,
                            ClassName = ClassId
                        };
                        caSetUps.Add(caSetup);
                        caSetup.SchoolId = userSchool;
                        Db.CaSetUps.Add(caSetup);
                    }
                    else
                    {
                        var caSetup = new CaSetUp
                        {
                            CaCaption = "",
                            CaOrder = i,
                            IsTrue = false,
                            TermName = TermId,
                            MaximumScore = 0,
                            CaPercentage = 0,
                            ClassName = ClassId
                        };
                        caSetUps.Add(caSetup);
                        caSetup.SchoolId = userSchool;
                        Db.CaSetUps.Add(caSetup);
                    }

                }
                await Db.SaveChangesAsync();
            }
            var className = Db.Classes.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool)).DistinctBy(x => x.ClassName).ToList();
            ViewBag.ClassId = new SelectList(className, "ClassName", "ClassName"); ViewBag.TermId = new SelectList(_query.TermList(), "TermName", "TermName");
            return View(caSetUps);
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
            var className = Db.Classes.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool)).DistinctBy(x => x.ClassName).ToList();
            ViewBag.ClassId = new SelectList(className, "ClassName", "ClassName"); ViewBag.TermId = new SelectList(_query.TermList(), "TermId", "TermName");
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
            var className = Db.Classes.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool)).DistinctBy(x => x.ClassName).ToList();
            ViewBag.ClassId = new SelectList(className, "ClassId", "ClassName");
            //ViewBag.ClassId = new SelectList(await _query.ClassListAsync(userSchool), "ClassId", "FullClassName");
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
            var className = Db.Classes.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool)).DistinctBy(x => x.ClassName).ToList();
            ViewBag.ClassId = new SelectList(className, "ClassId", "ClassName");
            //ViewBag.ClassId = new SelectList(await _query.ClassListAsync(userSchool), "ClassId", "FullClassName");
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
            var className = Db.Classes.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool)).DistinctBy(x => x.ClassName).ToList();
            ViewBag.ClassId = new SelectList(className, "ClassId", "ClassName");
            //ViewBag.ClassId = new SelectList(await _query.ClassListAsync(userSchool), "ClassId", "FullClassName");
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
            var className = Db.Classes.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool)).DistinctBy(x => x.ClassName).ToList();
            ViewBag.ClassId = new SelectList(className, "ClassId", "ClassName");
            //ViewBag.ClassId = new SelectList(await _query.ClassListAsync(userSchool), "ClassId", "FullClassName");
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
            bool status = false;
            string message = string.Empty;
            var caSetUp = await Db.CaSetUps.FindAsync(id);
            if (caSetUp != null)
            {
                Db.CaSetUps.Remove(caSetUp);
                await Db.SaveChangesAsync();
                status = true;
                message = "CASS Deleted Successfully...";
            }

            return new JsonResult { Data = new { status = status, message = message } };
        }
        public async Task<JsonResult> ScoreValidation(List<double> MaximumScore, List<int> CaSetUpId, List<string> ClassName, List<string> TermName)
        {
            //bool myValue = ExamCa < 50;
            string maximumScore = string.Empty;
            string className = string.Empty;
            string termName = string.Empty;
            string caSetUpId = string.Empty;

            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("maximumscore")) != null)
            {
                maximumScore =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("maximumscore"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("classname")) != null)
            {
                className =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("classname"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("termname")) != null)
            {
                termName =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("termname"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("casetupid")) != null)
            {
                caSetUpId =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("casetupid"))];
            }

            int newCastupId = Convert.ToInt32(caSetUpId);
            var setUpSum = Db.CaSetUps.AsNoTracking().Where(x => x.CaSetUpId != newCastupId
                                                               && x.SchoolId.Equals(userSchool)
                                                               && x.ClassName.Equals(className)
                                                               && x.TermName.Equals(termName))
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

        public async Task<JsonResult> PercentageValidation(List<double> CaPercentage, List<int> CaSetUpId, List<string> ClassName, List<string> TermName)
        {
            //bool myValue = ExamCa < 50;
            string caPercentage = string.Empty;
            string className = string.Empty;
            string termName = string.Empty;
            string caSetUpId = string.Empty;

            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("capercentage")) != null)
            {
                caPercentage =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("capercentage"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("classname")) != null)
            {
                className =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("classname"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("termname")) != null)
            {
                termName =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("termname"))];
            }
            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("casetupid")) != null)
            {
                caSetUpId =
                    Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("casetupid"))];
            }

            //int newCastupId = Convert.ToInt32(caSetUpId);
            //var setUpSum = Db.CaSetUps.AsNoTracking().Where(x => x.CaSetUpId != newCastupId
            //                                                     && x.SchoolId.Equals(userSchool)
            //                                                     && x.ClassName.Equals(className)
            //                                                     && x.TermName.Equals(termName))
            //    .Sum(s => s.CaPercentage);

            //var totalValue = setUpSum + Convert.ToDouble(caPercentage);

            int newCastupId = Convert.ToInt32(caSetUpId);
            var setUpSum = Db.CaSetUps.AsNoTracking().Where(x => x.CaSetUpId != newCastupId
                                    && x.SchoolId.Equals(userSchool)
                                    && x.ClassName.Equals(className)
                                    && x.TermName.Equals(termName))
                                    .Sum(s => s.CaPercentage);

            var totalValue = setUpSum + Convert.ToDouble(caPercentage);


            if (totalValue > 100)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            var caSetUp = await Db.CaSetUps.FindAsync(newCastupId);
            caSetUp.SchoolId = userSchool;
            caSetUp.CaPercentage = Convert.ToDouble(caPercentage);
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
