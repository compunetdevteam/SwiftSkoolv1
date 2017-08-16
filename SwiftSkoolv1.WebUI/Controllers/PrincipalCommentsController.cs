using PagedList;
using SwiftSkoolv1.Domain;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class PrincipalCommentsController : BaseController
    {
        // GET: PrincipalComments
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
            var assignedList = from s in Db.PrincipalComments select s;
            if (!String.IsNullOrEmpty(search))
            {
                assignedList = assignedList.Where(s => s.ClassName.ToUpper().Contains(search.ToUpper()));


            }
            else if (!String.IsNullOrEmpty(ClassName))
            {
                assignedList = assignedList.Where(s => s.ClassName.ToUpper().Equals(ClassName.ToUpper()));
                int myCount = await assignedList.CountAsync();
                if (myCount != 0)
                {
                    count = myCount;
                }
            }
            switch (sortOrder)
            {
                case "name_desc":
                    assignedList = assignedList.OrderByDescending(s => s.ClassName);
                    break;
                case "Date":
                    assignedList = assignedList.OrderBy(s => s.ClassName);
                    break;
                default:
                    assignedList = assignedList.OrderBy(s => s.ClassName);
                    break;
            }

            ViewBag.ClassName = new SelectList(Db.SchoolClasses.AsNoTracking(), "ClassCode", "ClassCode");
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

            //var v = Db.Subjects.Where(x => x.SchoolId != userSchool).Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToList();
            var v = Db.PrincipalComments.Where(x => x.SchoolId == userSchool).Select(s => new { s.Id, s.Remark, s.ClassName, s.MinimumGrade, s.MaximumGrade }).ToList();

            //var v = Db.Subjects.Where(x => x.SchoolId.Equals(userSchool)).Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToList();
            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            //{
            //    //v = v.OrderBy(sortColumn + " " + sortColumnDir);
            //    v = new List<Subject>(v.OrderBy(x => "sortColumn + \" \" + sortColumnDir"));
            //}
            if (!string.IsNullOrEmpty(search))
            {
                //v = v.OrderBy(sortColumn + " " + sortColumnDir);
                v = Db.PrincipalComments.Where(x => x.SchoolId.Equals(userSchool) && (x.ClassName.Equals(search) || x.Remark.Equals(search)))
                    .Select(s => new { s.Id, s.Remark, s.ClassName, s.MinimumGrade, s.MaximumGrade }).ToList();
            }
            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
            #endregion

            //return Json(new { data = await Db.Subjects.AsNoTracking().Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToListAsync() }, JsonRequestBehavior.AllowGet);
        }



        // GET: PrincipalComments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var principalComment = await Db.PrincipalComments.FindAsync(id);
            if (principalComment == null)
            {
                return HttpNotFound();
            }
            return View(principalComment);
        }

        // GET: PrincipalComments/Create
        public ActionResult Create()
        {
            ViewBag.ClassName = new SelectList(Db.SchoolClasses.AsNoTracking(), "ClassCode", "ClassCode");
            return View();
        }

        // POST: PrincipalComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,MinimumGrade,MaximumGrade,Remark,ClassName")] PrincipalCommentVm model)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in model.ClassName)
                {
                    var myGrade = await Db.PrincipalComments.CountAsync(x => x.MaximumGrade.Equals(model.MaximumGrade)
                                                                             && x.MinimumGrade.Equals(model.MinimumGrade)
                                                                             && x.ClassName.Equals(item));

                    if (myGrade >= 1)
                    {
                        TempData["UserMessage"] = "Principal Comment Already Exist in Database.";
                        TempData["Title"] = "Error.";
                        ViewBag.ClassName = new SelectList(Db.SchoolClasses, "ClassCode", "ClassCode");
                        return View(model);
                    }
                    var principalComment = new PrincipalComment()
                    {
                        MaximumGrade = model.MaximumGrade,
                        MinimumGrade = model.MinimumGrade,
                        Remark = model.Remark,
                        ClassName = item
                    };
                    Db.PrincipalComments.Add(principalComment);
                }
                await Db.SaveChangesAsync();
                ViewBag.ClassName = new SelectList(Db.SchoolClasses.AsNoTracking(), "ClassCode", "ClassCode");
                TempData["UserMessage"] = "Principal Comment Added Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: PrincipalComments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrincipalComment principalComment = await Db.PrincipalComments.FindAsync(id);
            if (principalComment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassName = new SelectList(Db.SchoolClasses.AsNoTracking(), "ClassCode", "ClassCode");
            return View(principalComment);
        }

        // POST: PrincipalComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,MinimumGrade,MaximumGrade,Remark,ClassName")] PrincipalComment principalComment)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(principalComment).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ClassName = new SelectList(Db.SchoolClasses.AsNoTracking(), "ClassCode", "ClassCode");
            return View(principalComment);
        }

        public async Task<PartialViewResult> Save(int id)
        {
            ViewBag.ClassName = new SelectList(Db.SchoolClasses.AsNoTracking(), "ClassCode", "ClassCode");
            var principalComment = await Db.PrincipalComments.FindAsync(id);
            return PartialView(principalComment);
        }

        // POST: Subjects/Save/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(PrincipalComment principalComment)
        {
            bool status = false;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (principalComment.Id > 0)
                {
                    principalComment.SchoolId = userSchool;
                    Db.Entry(principalComment).State = EntityState.Modified;
                    message = "Comment Updated Successfully...";
                }
                else
                {
                    principalComment.SchoolId = userSchool;
                    Db.PrincipalComments.Add(principalComment);
                    message = "Comment Created Successfully...";

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
            PrincipalComment principalComment = await Db.PrincipalComments.FindAsync(id);

            return PartialView(principalComment);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            bool status = false;
            string message = string.Empty;
            var principalComment = await Db.PrincipalComments.FindAsync(id);
            if (principalComment != null)
            {
                Db.PrincipalComments.Remove(principalComment);
                await Db.SaveChangesAsync();
                status = true;
                message = "Comment Deleted Successfully...";
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
