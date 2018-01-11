using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using SwiftSkoolv1.Domain;
using SwiftSkoolv1.WebUI.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SwiftSkoolv1.WebUI.Services;


namespace SwiftSkoolv1.WebUI.Controllers
{
    public class GuardiansController : BaseController
    {


        // GET: Guardians
        public ActionResult Index()
        {
            //var guardians = Db.Guardians.AsNoTracking().Include(g => g.Student);
            //var studentId = User.Identity.GetUserId();
            //if (Request.IsAuthenticated && User.IsInRole("Student"))
            //{
            //    return View(guardians.Where(x => x.SchoolId.Equals(userSchool) && x.StudentId.Equals(studentId)).ToList());
            //}
            //if (Request.IsAuthenticated && User.IsInRole("Admin"))
            //{
            //    return View(guardians.Where(x => x.SchoolId.Equals(userSchool)));
            //}

            //return View(await guardians.ToListAsync());
            return View();
        }

        public ActionResult GetIndex()
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
            var guardians = Db.Guardians.AsNoTracking().Include(g => g.Student);
            var studentId = User.Identity.GetUserId();
            if (Request.IsAuthenticated && User.IsInRole("Student"))
            {
                guardians = guardians.Where(x => x.SchoolId.Equals(userSchool) && x.StudentId.Equals(studentId));
            }
            if (Request.IsAuthenticated && User.IsInRole("Admin"))
            {
                guardians = guardians.Where(x => x.SchoolId.Equals(userSchool));
            }

            //var v = Db.Subjects.Where(x => x.SchoolId != userSchool).Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToList();
            var v = guardians.Where(x => x.SchoolId == userSchool).Select(s => new { s.GuardianId, s.StudentId, s.FullName, s.PhoneNumber, s.Relationship }).ToList();

            //var v = Db.Subjects.Where(x => x.SchoolId.Equals(userSchool)).Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToList();
            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            //{
            //    //v = v.OrderBy(sortColumn + " " + sortColumnDir);
            //    v = new List<Subject>(v.OrderBy(x => "sortColumn + \" \" + sortColumnDir"));
            //}
            if (!string.IsNullOrEmpty(search))
            {
                //v = v.OrderBy(sortColumn + " " + sortColumnDir);
                v = Db.Guardians.Where(x => x.SchoolId.Equals(userSchool) && (x.StudentId.Equals(search) || x.FullName.Equals(search)))
                    .Select(s => new { s.GuardianId, s.StudentId, s.FullName, s.PhoneNumber, s.Relationship }).ToList();
            }
            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
            #endregion

            //return Json(new { data = await Db.Subjects.AsNoTracking().Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToListAsync() }, JsonRequestBehavior.AllowGet);
        }

        public async Task<PartialViewResult> Save(int id)
        {
            if (id > 0)
            {
                var guardian = await Db.Guardians.FindAsync(id);
                var model = new GuardianViewModel()
                {
                    GuardianId = guardian.GuardianId,
                    StudentId = guardian.StudentId,
                    FirstName = guardian.FirstName,
                    MiddleName = guardian.MiddleName,
                    LastName = guardian.LastName,
                    Email = guardian.Email,
                    PhoneNumber = guardian.PhoneNumber,
                    Address = guardian.Address,
                    Occupation = guardian.Occupation,
                    LGAOforigin = guardian.LGAOforigin,
                    MotherName = guardian.MotherName,
                    MotherMaidenName = guardian.MotherMaidenName,

                };
                ViewBag.StudentId = new SelectList(await _query.StudentListAsync(userSchool), "StudentId", "FullName", guardian.StudentId);

                return PartialView(model);
            }
            ViewBag.StudentId = new SelectList(await _query.StudentListAsync(userSchool), "StudentId", "FullName");

            return PartialView();
        }

        // POST: Subjects/Save/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(GuardianViewModel model)
        {
            bool status = false;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (model.GuardianId > 0)
                {
                    var guardian = await Db.Guardians.FindAsync(model.GuardianId);
                    if (guardian != null)
                    {
                        guardian.StudentId = model.StudentId;
                        guardian.Salutation = model.Salutation.ToString();
                        guardian.FirstName = model.FirstName;
                        guardian.MiddleName = model.MiddleName;
                        guardian.LastName = model.LastName;
                        guardian.Gender = model.Gender.ToString();
                        guardian.Email = model.Email;
                        guardian.PhoneNumber = model.PhoneNumber;
                        guardian.Address = model.Address;
                        guardian.Relationship = model.Relationship.ToString();
                        guardian.Occupation = model.Occupation;
                        guardian.Religion = model.Religion.ToString();
                        guardian.LGAOforigin = model.LGAOforigin;
                        guardian.StateOfOrigin = model.StateOfOrigin.ToString();
                        guardian.MotherName = model.MotherName;
                        guardian.MotherMaidenName = model.MotherMaidenName;
                        guardian.SchoolId = userSchool;
                    }
                    Db.Entry(guardian).State = EntityState.Modified;
                    message = "Guardian Updated Successfully...";
                }
                else
                {
                    if (Request.IsAuthenticated && !User.IsInRole("SuperAdmin"))
                    {
                        var guardian = new Guardian()
                        {
                            StudentId = model.StudentId,
                            Salutation = model.Salutation.ToString(),
                            FirstName = model.FirstName,
                            MiddleName = model.MiddleName,
                            LastName = model.LastName,
                            Gender = model.Gender.ToString(),
                            Email = model.Email,
                            PhoneNumber = model.PhoneNumber,
                            Address = model.Address,
                            Relationship = model.Relationship.ToString(),
                            Occupation = model.Occupation,
                            Religion = model.Religion.ToString(),
                            LGAOforigin = model.LGAOforigin,
                            StateOfOrigin = model.StateOfOrigin.ToString(),
                            MotherName = model.MotherName,
                            MotherMaidenName = model.MotherMaidenName,
                            SchoolId = userSchool
                        };
                        Db.Guardians.Add(guardian);
                        message = "Guardian Created Successfully...";
                    }
                }
                await Db.SaveChangesAsync();
                status = true;
            }
            return new JsonResult { Data = new { status = status, message = message } };
            //return View(subject);
        }

        // GET: Guardians/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guardian guardian = await Db.Guardians.FindAsync(id);
            if (guardian == null)
            {
                return HttpNotFound();
            }
            return View(guardian);
        }




        // GET: Guardians/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.StudentId = new SelectList(await _query.StudentListAsync(userSchool), "StudentId", "FullName");
            return View();
        }

        // POST: Guardians/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(GuardianViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request.IsAuthenticated && !User.IsInRole("SuperAdmin"))
                {
                    var guardian = new Guardian()
                    {
                        StudentId = model.StudentId,
                        Salutation = model.Salutation.ToString(),
                        FirstName = model.FirstName,
                        MiddleName = model.MiddleName,
                        LastName = model.LastName,
                        Gender = model.Gender.ToString(),
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        Address = model.Address,
                        Relationship = model.Relationship.ToString(),
                        Occupation = model.Occupation,
                        Religion = model.Religion.ToString(),
                        LGAOforigin = model.LGAOforigin,
                        StateOfOrigin = model.StateOfOrigin.ToString(),
                        MotherName = model.MotherName,
                        MotherMaidenName = model.MotherMaidenName,
                        SchoolId = userSchool
                    };
                    Db.Guardians.Add(guardian);
                    await Db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                ViewBag.Message = "Super Admin Cannot Create a Guardian";
                ViewBag.StudentId = new SelectList(Db.Students, "StudentId", "FullName", model.StudentId);
                return View(model);
            }

            ViewBag.StudentId = new SelectList(await _query.StudentListAsync(userSchool), "StudentId", "FullName", model.StudentId);
            return View(model);
        }

        // GET: Guardians/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guardian guardian = await Db.Guardians.FindAsync(id);
            if (guardian == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentId = new SelectList(await _query.StudentListAsync(userSchool), "StudentId", "FullName", guardian.StudentId);
            var model = new GuardianViewModel()
            {
                GuardianId = guardian.GuardianId,
                StudentId = guardian.StudentId,
                FirstName = guardian.FirstName,
                MiddleName = guardian.MiddleName,
                LastName = guardian.LastName,
                Email = guardian.Email,
                PhoneNumber = guardian.PhoneNumber,
                Address = guardian.Address,
                Occupation = guardian.Occupation,
                LGAOforigin = guardian.LGAOforigin,
                MotherName = guardian.MotherName,
                MotherMaidenName = guardian.MotherMaidenName,

            };
            return View(model);
        }

        // POST: Guardians/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "GuardianId,Salutation,FirstName,MiddleName,LastName,Gender,PhoneNumber,Email,Address,Occupation,Relationship,Religion,LGAOforigin,StateOfOrigin,MotherName,MotherMaidenName,UserName,FullName,StudentId")] GuardianViewModel model)
        {
            if (ModelState.IsValid)
            {
                var guardian = await Db.Guardians.FindAsync(model.GuardianId);
                if (guardian != null)
                {
                    guardian.StudentId = model.StudentId;
                    guardian.Salutation = model.Salutation.ToString();
                    guardian.FirstName = model.FirstName;
                    guardian.MiddleName = model.MiddleName;
                    guardian.LastName = model.LastName;
                    guardian.Gender = model.Gender.ToString();
                    guardian.Email = model.Email;
                    guardian.PhoneNumber = model.PhoneNumber;
                    guardian.Address = model.Address;
                    guardian.Relationship = model.Relationship.ToString();
                    guardian.Occupation = model.Occupation;
                    guardian.Religion = model.Religion.ToString();
                    guardian.LGAOforigin = model.LGAOforigin;
                    guardian.StateOfOrigin = model.StateOfOrigin.ToString();
                    guardian.MotherName = model.MotherName;
                    guardian.MotherMaidenName = model.MotherMaidenName;
                    guardian.SchoolId = userSchool;
                }
                Db.Entry(guardian).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.StudentId = new SelectList(await _query.StudentListAsync(userSchool), "StudentId", "FullName", model.StudentId);
            return View(model);
        }

        public async Task<PartialViewResult> Delete(int id)
        {
            var guardian = await Db.Guardians.FindAsync(id);
            return PartialView(guardian);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            bool status = false;
            string message = string.Empty;
            var guardian = await Db.Guardians.FindAsync(id);
            if (guardian != null)
            {
                Db.Guardians.Remove(guardian);
                await Db.SaveChangesAsync();
                status = true;
                message = "Guardian Deleted Successfully...";
            }
            return new JsonResult { Data = new { status = status, message = message } };
        }

        [AllowAnonymous]
        public PartialViewResult UpLoadGuardian()
        {
            return PartialView();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> UpLoadGuardian(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please Select a excel file <br/>";
                return View("UpLoadGuardian");
            }
            HttpPostedFileBase file = Request.Files["excelfile"];
            if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
            {
                string lastrecord = "";
                int recordCount = 0;
                string message = "";
                string fileContentType = file.ContentType;
                byte[] fileBytes = new byte[file.ContentLength];
                var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                // Read data from excel file
                using (var package = new ExcelPackage(file.InputStream))
                {
                    ExcelValidation myExcel = new ExcelValidation();
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;
                    int requiredField = 16;

                    string validCheck = myExcel.ValidateExcel(noOfRow, workSheet, requiredField);
                    if (!validCheck.Equals("Success"))
                    {
                        //string row = "";
                        //string column = "";
                        string[] ssizes = validCheck.Split(' ');
                        string[] myArray = new string[2];
                        for (int i = 0; i < ssizes.Length; i++)
                        {
                            myArray[i] = ssizes[i];
                            // myArray[i] = ssizes[];
                        }
                        string lineError = $"Line/Row number {myArray[0]}  and column {myArray[1]} is not rightly formatted, Please Check for anomalies ";
                        //ViewBag.LineError = lineError;
                        ViewBag.Message = lineError;
                        RedirectToAction("Index", "Guardians");
                    }

                    for (int row = 2; row <= noOfRow; row++)
                    {
                        try
                        {
                            string studentId = workSheet.Cells[row, 1].Value.ToString().Trim();
                            string salutation = workSheet.Cells[row, 2].Value.ToString().Trim();
                            string firstName = workSheet.Cells[row, 3].Value.ToString().Trim();
                            string middleName = workSheet.Cells[row, 4].Value.ToString().Trim();
                            string lastName = workSheet.Cells[row, 5].Value.ToString().Trim();
                            string email = workSheet.Cells[row, 6].Value.ToString().Trim();
                            string gender = workSheet.Cells[row, 7].Value.ToString().Trim();
                            string phoneNumber = workSheet.Cells[row, 8].Value.ToString().Trim();
                            string religion = workSheet.Cells[row, 9].Value.ToString().Trim();
                            string address = workSheet.Cells[row, 10].Value.ToString().Trim();
                            string occupation = workSheet.Cells[row, 11].Value.ToString().Trim();
                            string relationship = workSheet.Cells[row, 12].Value.ToString().Trim();
                            string lga = workSheet.Cells[row, 13].Value.ToString().Trim();
                            string state = workSheet.Cells[row, 14].Value.ToString().Trim();
                            string motherName = workSheet.Cells[row, 15].Value.ToString().Trim();
                            string mothermaidenName = workSheet.Cells[row, 16].Value.ToString().Trim();

                            var guardian = new Guardian()
                            {
                                StudentId = studentId,
                                Salutation = salutation.Trim(),
                                FirstName = firstName.Trim(),
                                MiddleName = middleName.Trim(),
                                LastName = lastName.Trim(),
                                Gender = gender.Trim(),
                                Address = address.Trim(),
                                PhoneNumber = phoneNumber.Trim(),
                                Email = email.Trim(),
                                Relationship = relationship.Trim(),
                                Occupation = occupation.Trim(),
                                Religion = religion,
                                LGAOforigin = lga,
                                StateOfOrigin = state,
                                MotherName = motherName,
                                MotherMaidenName = mothermaidenName,
                                SchoolId = userSchool
                            };
                            Db.Guardians.Add(guardian);
                            recordCount++;
                            lastrecord =
                                $"The last Updated record has the Last Name {lastName} and First Name {firstName} with Phone Number {phoneNumber}";

                        }
                        catch (Exception e)
                        {
                            ViewBag.ErrorMessage = $"Student Id doesn,t exist for row {row}{e.Message}";
                            return View("Error3");
                        }

                    }
                    try
                    {
                        await Db.SaveChangesAsync();
                        message = $"You have successfully Uploaded {recordCount} records...  and {lastrecord}";
                        ViewBag.Message = message;
                        return RedirectToAction("Index", "Guardians");
                    }
                    catch (Exception e)
                    {
                        ViewBag.ErrorMessage = $"Student Id in record doesn't exist. {e.Message}";
                        return View("Error3");
                    }

                }
            }
            ViewBag.Error = "File type is Incorrect <br/>";
            return View("UpLoadGuardian");
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
