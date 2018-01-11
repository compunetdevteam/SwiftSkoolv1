using Microsoft.Ajax.Utilities;
using PagedList;
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
    public class GradesController : BaseController
    {
        // GET: Grades
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string search, int? page,
           string ClassName)
        {
            int count = 10;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (search != null)
            {

                page = 1;
            }
            else
            {
                search = currentFilter;
            }
            ViewBag.CurrentFilter = search;
            var assignedList = from s in Db.Grades select s;
            if (Request.IsAuthenticated && !User.IsInRole("SuperAdmin"))
            {
                assignedList = assignedList.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool));
            }
            //if (!String.IsNullOrEmpty(search))
            //{
            //    assignedList = assignedList.Where(s => s.ClassName.ToUpper().Contains(search.ToUpper()));


            //}
            //else if (!String.IsNullOrEmpty(ClassName))
            //{
            //    assignedList = assignedList.Where(s => s.ClassName.ToUpper().Equals(ClassName.ToUpper()));
            //    int myCount = await assignedList.CountAsync();
            //    if (myCount != 0)
            //    {
            //        count = myCount;
            //    }
            //}
            switch (sortOrder)
            {
                case "name_desc":
                    assignedList = assignedList.OrderByDescending(s => s.GradeName);
                    break;
                case "Date":
                    assignedList = assignedList.OrderBy(s => s.Remark);
                    break;
                default:
                    assignedList = assignedList.OrderBy(s => s.GradeName);
                    break;
            }

            ViewBag.ClassName = new SelectList(_query.SchoolClassListAsync(userSchool), "ClassCode", "ClassCode");
            int pageSize = count;
            int pageNumber = (page ?? 1);
            return View(assignedList.ToPagedList(pageNumber, pageSize));
            //return View(await Db.ContinuousAssessments.ToListAsync());
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


            var v = Db.Grades.Where(x => x.SchoolId == userSchool).Select(g => new { g.GradeId, g.ClassName, g.GradeName, g.MinimumValue, g.MaximumValue, g.Remark }).ToList();


            if (!string.IsNullOrEmpty(search))
            {

                v = v.Where(x => x.ClassName.Equals(search) || x.GradeName.Equals(search) || x.Remark.Equals(search)).ToList();
            }
            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
            #endregion

            //return Json(new { data = await Db.Subjects.AsNoTracking().Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToListAsync() }, JsonRequestBehavior.AllowGet);
        }



        // GET: Grades/Create
        public ActionResult Create()
        {
            ViewBag.ClassName = new SelectList(_query.SchoolClassListAsync(userSchool), "ClassCode", "ClassCode");
            return View();
        }

        // POST: Grades/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(GradeViewModel model)
        {
            if (ModelState.IsValid)
            {

                var myGrade = await Db.Grades.CountAsync(x => x.GradeName.Trim().Equals(model.GradeName.Trim())
                                        && x.SchoolId.Equals(userSchool));

                if (myGrade >= 1)
                {
                    TempData["UserMessage"] = "Grade Already Exist in Database.";
                    TempData["Title"] = "Error.";
                    ViewBag.ClassName = new SelectList(Db.SchoolClasses.AsNoTracking(), "ClassCode", "ClassCode");
                    return View(model);
                }

                var grade = new Grade
                {
                    GradeName = model.GradeName.Trim().ToUpper(),
                    MinimumValue = model.MinimumValue,
                    MaximumValue = model.MaximumValue,
                    //GradePoint = model.GradePoint,
                    Remark = model.Remark,
                    SchoolId = userSchool
                    //ClassName = item
                };
                Db.Grades.Add(grade);

                await Db.SaveChangesAsync();
                TempData["UserMessage"] = "Grade Added Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            //ViewBag.ClassName = new SelectList(Db.SchoolClasses.AsNoTracking(), "ClassCode", "ClassCode");
            return View(model);
        }




        public async Task<PartialViewResult> Save(int id)
        {
            var grade = await Db.Grades.FindAsync(id);
            var className = Db.Classes.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool)).AsEnumerable().DistinctBy(x => x.ClassName).ToList();
            ViewBag.ClassName = new MultiSelectList(className, "ClassName", "ClassName");
            if (grade != null)
            {
                var gradevm = new GradeViewModel
                {
                    GradeId = grade.GradeId,
                    GradeName = grade.GradeName,
                    Remark = grade.Remark,
                    MinimumValue = grade.MinimumValue,
                    MaximumValue = grade.MaximumValue
                };
                return PartialView(gradevm);
            }
            return PartialView();
        }

        // POST: Subjects/Save/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(GradeViewModel model)
        {
            bool status = false;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (model.GradeId > 0)
                {
                    var grade = await Db.Grades.FindAsync(model.GradeId);
                    if (grade != null)
                    {
                        grade.SchoolId = userSchool;
                        grade.ClassName = model.ClassName[0];
                        grade.GradeName = model.GradeName;
                        grade.MaximumValue = model.MaximumValue;
                        grade.MinimumValue = model.MinimumValue;
                        grade.Remark = model.Remark;
                        Db.Entry(grade).State = EntityState.Modified;
                    }
                    message = "Grade Updated Successfully...";
                }
                else
                {
                    foreach (var className in model.ClassName)
                    {
                        var myGrade = await Db.Grades.CountAsync(x => x.GradeName.Trim().Equals(model.GradeName.Trim())
                                                        && x.ClassName.Equals(className) && x.SchoolId.Equals(userSchool));

                        if (myGrade >= 1)
                        {
                            TempData["UserMessage"] = "Grade Already Exist in Database.";
                            TempData["Title"] = "Error.";
                            ViewBag.ClassName = new SelectList(Db.SchoolClasses.AsNoTracking(), "ClassCode", "ClassCode");
                            return View(model);
                        }

                        var grade = new Grade
                        {
                            GradeName = model.GradeName.Trim().ToUpper(),
                            MinimumValue = model.MinimumValue,
                            MaximumValue = model.MaximumValue,
                            //GradePoint = model.GradePoint,
                            Remark = model.Remark,
                            SchoolId = userSchool,
                            ClassName = className
                        };
                        Db.Grades.Add(grade);
                        grade.SchoolId = userSchool;
                        Db.Grades.Add(grade);
                        message = "Grade Created Successfully...";
                    }


                }
                await Db.SaveChangesAsync();
                status = true;
            }
            return new JsonResult { Data = new { status = status, message = message } };
            //return View(Grade);
        }



        // GET: Grades/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var grade = await Db.Grades.FindAsync(id);
            if (grade == null)
            {
                return HttpNotFound();
            }
            var myGrade = new GradeViewModel
            {
                GradeName = grade.GradeName,
                GradeId = grade.GradeId,
                MinimumValue = grade.MinimumValue,
                MaximumValue = grade.MaximumValue,
                //GradePoint = grade.GradePoint,
                Remark = grade.Remark,


            };
            ViewBag.ClassName = new MultiSelectList(_query.SchoolClassListAsync(userSchool), "ClassCode", "ClassCode");
            return View(myGrade);
        }

        // POST: Grades/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(GradeViewModel model)
        {
            if (ModelState.IsValid)
            {
                //foreach (var item in model.ClassName)
                //{
                var grade = new Grade()
                {

                    GradeId = model.GradeId,
                    GradeName = model.GradeName.ToString(),
                    MinimumValue = model.MinimumValue,
                    MaximumValue = model.MaximumValue,
                    //GradePoint = model.GradePoint,
                    Remark = model.Remark,
                    SchoolId = userSchool
                    //ClassName = item
                };
                Db.Entry(grade).State = EntityState.Modified;
                //}
                await Db.SaveChangesAsync();
                TempData["UserMessage"] = "Grade Updated Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            ViewBag.ClassName = new SelectList(_query.SchoolClassListAsync(userSchool), "ClassCode", "ClassCode");
            return View(model);
        }



        // GET: Classes/Delete/5
        public async Task<PartialViewResult> Delete(int id)
        {
            Grade @class = await Db.Grades.FindAsync(id);
            return PartialView(@class);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            bool status = false;
            string message = string.Empty;
            var grade = await Db.Grades.FindAsync(id);
            if (grade != null)
            {
                Db.Grades.Remove(grade);
                await Db.SaveChangesAsync();
                status = true;
                message = "Subject Deleted Successfully...";
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
