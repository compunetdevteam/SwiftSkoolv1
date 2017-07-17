//using System.Threading.Tasks;
//using System.Web.Mvc;

//namespace SwiftSkoolv1.WebUI.Controllers
//{
//    public class SummaryScoreController : BaseController
//    {

//        // GET: SummaryScore
//        public async Task<ActionResult> Index()
//        {
//            return View(await Db.SessionSubjectTotals.ToListAsync());
//        }

//        // GET: SummaryScore/Details/5
//        //public ActionResult Details(int id)
//        //{
//        //    return View();
//        //}

//        // GET: SummaryScore/Create
//        public ActionResult Create()
//        {
//            ViewBag.SubjectCode = new SelectList(Db.Subjects, "SubjectId", "SubjectName");
//            ViewBag.SessionName = new SelectList(Db.Sessions, "SessionName", "SessionName");
//            ViewBag.ClassName = new SelectList(Db.Classes, "FullClassName", "FullClassName");
//            return View();
//        }

//        // POST: SummaryScore/Create
//        [HttpPost]
//        public async Task<ActionResult> Create(SummaryScoreViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                var student = Db.AssignedClasses.AsNoTracking().Where(x => x.ClassName.Equals(model.ClassName) && x.TermName.Contains("Third")
//                                                        && x.SessionName.Equals(model.SessionName)).ToList();
//                //var mysubjectCategory = Db.Subjects.Where(x => x.CourseName.Equals(model.SubjectCode))
//                //                                .Select(c => c.CategoriesId).FirstOrDefault();
//                //var newSubjectName = Db.Subjects.AsNoTracking().Where(x => x.SubjectId.Equals(model.SubjectId))
//                //.Select(c => c.SubjectName).FirstOrDefault();
//                foreach (var listStudent in student)
//                {
//                    string studentNumber = listStudent.StudentId;
//                    var CA = Db.SessionSubjectTotals.Where(x => x.ClassName.Equals(model.ClassName)
//                                                                && x.SessionName.Equals(model.SessionName)
//                                                                // && (x.SubjectName.Equals(mysubjectCategory)
//                                                                && x.SubjectId.Equals(model.SubjectId)
//                                                                && x.StudentId.Equals(studentNumber));
//                    var countFromDb = CA.Count();
//                    if (countFromDb >= 1)
//                    {
//                        return View("Error3");
//                    }
//                    var resultSummary = new SessionSubjectTotal(studentNumber, model.ClassName, model.SessionName, model.SubjectId);

//                    Db.SessionSubjectTotals.Add(resultSummary);
//                }
//                await Db.SaveChangesAsync();
//                return RedirectToAction("Index");
//            };
//            return View(model);
//        }

//        //// GET: SummaryScore/Edit/5
//        //public ActionResult Edit(int id)
//        //{
//        //    return View();
//        //}

//        //// POST: SummaryScore/Edit/5
//        //[HttpPost]
//        //public ActionResult Edit(int id, FormCollection collection)
//        //{
//        //    try
//        //    {
//        //        // TODO: Add update logic here

//        //        return RedirectToAction("Index");
//        //    }
//        //    catch
//        //    {
//        //        return View();
//        //    }
//        //}

//        //// GET: SummaryScore/Delete/5
//        //public ActionResult Delete(int id)
//        //{
//        //    return View();
//        //}

//        //// POST: SummaryScore/Delete/5
//        //[HttpPost]
//        //public ActionResult Delete(int id, FormCollection collection)
//        //{
//        //    try
//        //    {
//        //        // TODO: Add delete logic here

//        //        return RedirectToAction("Index");
//        //    }
//        //    catch
//        //    {
//        //        return View();
//        //    }
//        //}
//    }
//}
