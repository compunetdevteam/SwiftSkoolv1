//using PagedList;
//using SwiftSkool.BusinessLogic;
//using SwiftSkool.Models;
//using SwiftSkool.ViewModel;
//using System;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Threading.Tasks;
//using System.Web.Mvc;

//namespace SwiftSkool.Controllers
//{
//    public class ResultsController : Controller
//    {
//        private readonly ResultCommandManager _resultCommand;
//        private readonly ApplicationDbContext _db;

//        public ResultsController()
//        {
//            _resultCommand = new ResultCommandManager();
//            _db = new ApplicationDbContext();
//        }

//        //public ResultsController(IResultCommandManager resultCommand, ApplicationDbContext db)
//        //{
//        //    ResultCommand = resultCommand;
//        //    //_db = db;
//        //}


//        // GET: Results
//        //public async Task<ActionResult> Index()
//        //{
//        //    return View(await _db.Results.ToListAsync());
//        //}

//        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string search, int? page,
//           int? SubjectCode, string ClassName, string TermName, string SessionName)
//        {
//            int count = 10;
//            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
//            if (search != null)
//            {

//                page = 1;
//            }
//            else
//            {
//                search = currentFilter;
//            }
//            ViewBag.CurrentFilter = search;
//            var assignedList = from s in _db.Results select s;
//            if (!String.IsNullOrEmpty(search))
//            {
//                assignedList = assignedList.Where(s => s.StudentId.ToUpper().Contains(search.ToUpper())
//                                                             || s.ClassName.ToUpper().Contains(search.ToUpper())
//                                                             || s.Term.ToUpper().Contains(search.ToUpper()));

//            }
//            else if (SubjectCode != null && (!String.IsNullOrEmpty(ClassName)
//                   && !String.IsNullOrEmpty(SessionName) && !String.IsNullOrEmpty(TermName)))
//            {
//                assignedList = assignedList.Where(s => s.SubjectName.Equals(SubjectCode)
//                                           && s.ClassName.ToUpper().Equals(ClassName.ToUpper())
//                                           && s.Term.ToUpper().Equals(TermName.ToUpper())
//                                           && s.SessionName.ToUpper().Equals(SessionName))
//                                           .OrderBy(c => c.StudentId);
//                int myCount = await assignedList.CountAsync();
//                if (myCount != 0)
//                {
//                    count = myCount;
//                }

//            }
//            else if (SubjectCode != null || (!String.IsNullOrEmpty(ClassName)
//                                                            || !String.IsNullOrEmpty(SessionName) ||
//                                                            !String.IsNullOrEmpty(TermName)))
//            {
//                assignedList = assignedList.Where(s => s.SubjectName.Equals(SubjectCode)
//                                                       || s.ClassName.ToUpper().Equals(ClassName.ToUpper())
//                                                       || s.Term.ToUpper().Equals(TermName.ToUpper())
//                                                       || s.SessionName.ToUpper().Equals(SessionName));
//            }


//            switch (sortOrder)
//            {
//                case "name_desc":
//                    assignedList = assignedList.OrderByDescending(s => s.StudentId);
//                    break;
//                case "Date":
//                    assignedList = assignedList.OrderBy(s => s.SessionName);
//                    break;
//                default:
//                    assignedList = assignedList.OrderBy(s => s.ClassName);
//                    break;
//            }
//            int pageSize = count;
//            int pageNumber = (page ?? 1);

//            ViewBag.SubjectCode = new SelectList(_db.Subjects.AsNoTracking(), "SubjectId", "SubjectName");
//            ViewBag.TermName = new SelectList(_db.Terms.AsNoTracking(), "TermName", "TermName");
//            ViewBag.SessionName = new SelectList(_db.Sessions.AsNoTracking(), "SessionName", "SessionName");
//            ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
//            return View(assignedList.ToPagedList(pageNumber, pageSize));
//            //return View(await db.ContinuousAssessments.ToListAsync());
//        }

//        //// GET: Results/Details/5
//        //public ActionResult Details(int id)
//        //{
//        //    return View();
//        //}

//        // GET: Results/Create
//        public ActionResult Create()
//        {
//            ViewBag.SubjectCode = new SelectList(_db.Subjects.AsNoTracking(), "SubjectId", "SubjectName");
//            ViewBag.TermName = new SelectList(_db.Terms.AsNoTracking(), "TermName", "TermName");
//            //ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FullName");
//            ViewBag.SessionName = new SelectList(_db.Sessions.AsNoTracking(), "SessionName", "SessionName");
//            ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
//            return View();
//        }

//        // POST: Results/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Create(ResultViewModel model)
//        {
//            if (ModelState.IsValid)
//            {

//                var student = await _db.AssignedClasses.Where(x => x.ClassName.Equals(model.ClassName.Trim())
//                                                            && x.TermName.ToUpper().Trim().Equals(model.TermName.ToUpper().Trim())
//                                                            && x.SessionName.ToUpper().Trim().Equals(model.SessionName.ToUpper().Trim())).ToListAsync();

//                foreach (var listStudent in student)
//                {
//                    string studentNumber = listStudent.StudentId;
//                    var cA = await _db.Results.CountAsync(x => x.ClassName.ToUpper().Trim().Equals(model.ClassName.ToUpper().Trim())
//                                                                 && x.Term.ToUpper().Trim().Equals(model.TermName.ToUpper().Trim())
//                                                                 && x.SessionName.ToUpper().Trim().Equals(model.SessionName.ToUpper().Trim())
//                                                                 && x.SubjectName.Equals(model.SubjectCode)
//                                                                 && x.StudentId.ToUpper().Trim().Equals(studentNumber.ToUpper().Trim()));

//                    if (cA >= 1)
//                    {
//                        TempData["UserMessage"] = "Result has Already been created.";
//                        TempData["Title"] = "Error.";
//                        ViewBag.SubjectCode = new SelectList(_db.Subjects.AsNoTracking(), "SubjectId", "SubjectName");
//                        ViewBag.TermName = new SelectList(_db.Terms.AsNoTracking(), "TermName", "TermName");
//                        //ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FullName");
//                        ViewBag.SessionName = new SelectList(_db.Sessions.AsNoTracking(), "SessionName", "SessionName");
//                        ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
//                        return View(model);
//                    }
//                    else
//                    {
//                        //var result = new Result(studentNumber, model.ClassName, model.TermName.ToString(),
//                        //    model.SessionName, model.SubjectCode);
//                        try
//                        {
//                            var result = new Result
//                            {
//                                StudentId = studentNumber,
//                                ClassName = model.ClassName,
//                                Term = model.TermName,
//                                SubjectName = model.SubjectCode,
//                                SessionName = model.SessionName,
//                                ClassAverage = Math.Round(await _resultCommand.CalculateClassAverage(model.ClassName, model.TermName, model.SessionName, model.SubjectCode), 2),
//                                Average = Math.Round(await _resultCommand.CalculateAverage(studentNumber, model.ClassName, model.TermName, model.SessionName), 2),
//                                SubjectPosition = _resultCommand.FindSubjectPosition(studentNumber, model.SubjectCode, model.ClassName, model.TermName, model.SessionName),
//                                AggretateScore = Math.Round(await _resultCommand.TotalScorePerStudent(studentNumber, model.ClassName, model.TermName, model.SessionName), 2),
//                                // TotalQualityPoint = Math.Round(await _resultCommand.TotalQualityPoint(studentNumber, model.ClassName, model.TermName, model.SessionName), 2),
//                                TotalCreditUnit = Math.Round(await _resultCommand.TotalcreditUnit(model.ClassName), 2),
//                                SubjectHighest = Math.Round(await _resultCommand.SubjectHighest(model.SubjectCode, model.ClassName, model.TermName, model.SessionName), 2),
//                                SubjectLowest = Math.Round(await _resultCommand.SubjectLowest(model.SubjectCode, model.ClassName, model.TermName, model.SessionName), 2)
//                            };
//                            _db.Results.Add(result);
//                        }
//                        catch (Exception e)
//                        {
//                            TempData["UserMessage"] = $"Result setup Not Complete..The class is {model.ClassName} Student Id is {studentNumber} , Term is {model.TermName}, Session is" +
//                                                      $"{model.SessionName} and Subject Code {model.SubjectCode} Please ensure that you have properly setup the necessary things to create a result.{e.Message}";
//                            TempData["Title"] = "Error.";
//                            ViewBag.SubjectCode = new SelectList(_db.Subjects.AsNoTracking(), "SubjectId", "SubjectName");
//                            ViewBag.TermName = new SelectList(_db.Terms.AsNoTracking(), "TermName", "TermName");
//                            //ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FullName");
//                            ViewBag.SessionName = new SelectList(_db.Sessions.AsNoTracking(), "SessionName", "SessionName");
//                            ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
//                            return View(model);
//                        }

//                    }
//                }
//                await _db.SaveChangesAsync();
//                TempData["UserMessage"] = $"Result Created Successfully for all Students in {model.ClassName} offering {model.SubjectCode}";
//                TempData["Title"] = "Success.";
//                return RedirectToAction("Index");
//            }
//            return View(model);
//        }

//        // GET: Results/Edit/5
//        public async Task<ActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            var result = await _db.Results.FindAsync(id);
//            if (result == null)
//            {
//                return HttpNotFound();
//            }
//            var myStudent = new ResultViewModel()
//            {
//                ResultId = result.ResultId,
//                StudentId = result.StudentId
//            };
//            ViewBag.SubjectCode = new SelectList(_db.Subjects.AsNoTracking(), "SubjectId", "SubjectName");
//            //ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FullName");
//            ViewBag.SessionName = new SelectList(_db.Sessions.AsNoTracking(), "SessionName", "SessionName");
//            ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
//            ViewBag.TermName = new SelectList(_db.Terms.AsNoTracking(), "TermName", "TermName");
//            return View(myStudent);
//        }

//        // POST: Results/Edit/5
//        [HttpPost]
//        public async Task<ActionResult> Edit(ResultViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                string studentNumber = model.StudentId;

//                var result = new Result
//                {
//                    StudentId = studentNumber,
//                    ClassName = model.ClassName,
//                    Term = model.TermName,
//                    SubjectName = model.SubjectCode,
//                    SessionName = model.SessionName,
//                    ClassAverage = Math.Round(await _resultCommand.CalculateClassAverage(model.ClassName, model.TermName, model.SessionName, model.SubjectCode), 2),
//                    Average = Math.Round(await _resultCommand.CalculateAverage(studentNumber, model.ClassName, model.TermName, model.SessionName), 2),
//                    SubjectPosition = _resultCommand.FindSubjectPosition(studentNumber, model.SubjectCode, model.ClassName, model.TermName, model.SessionName),
//                    AggretateScore = Math.Round(await _resultCommand.TotalScorePerStudent(studentNumber, model.ClassName, model.TermName, model.SessionName), 2),
//                    // TotalQualityPoint = Math.Round(await _resultCommand.TotalQualityPoint(studentNumber, model.ClassName, model.TermName, model.SessionName), 2),
//                    TotalCreditUnit = Math.Round(await _resultCommand.TotalcreditUnit(model.ClassName), 2),
//                    SubjectHighest = Math.Round(await _resultCommand.SubjectHighest(model.SubjectCode, model.ClassName, model.TermName, model.SessionName), 2),
//                    SubjectLowest = Math.Round(await _resultCommand.SubjectLowest(model.SubjectCode, model.ClassName, model.TermName, model.SessionName), 2)
//                };

//                _db.Entry(result).State = EntityState.Modified;
//                await _db.SaveChangesAsync();
//                return RedirectToAction("Index");
//            }

//            return View(model);
//            // return View();
//        }

//        // GET: Results/Delete/5
//        //public ActionResult Delete(int id)
//        //{
//        //    return View();
//        //}

//        //// POST: Results/Delete/5
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
