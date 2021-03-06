﻿using HopeAcademySMS.Services;
using OfficeOpenXml;
using SwiftSkool.Models;
using SwiftSkoolv1.WebUI.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SwiftSkoolv1.Domain;
using SwiftSkoolv1.WebUI.ViewModels;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class GuardiansController : BaseController
    {


        // GET: Guardians
        public async Task<ActionResult> Index()
        {
            var guardians = Db.Guardians.AsNoTracking().Include(g => g.Student);
            if (Request.IsAuthenticated && !User.IsInRole("SuperAdmin"))
            {
                return View(guardians.Where(x => x.SchoolId.Equals(userSchool)));
            }
            return View(await guardians.ToListAsync());
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

        // GET: Guardians/Delete/5
        public async Task<ActionResult> Delete(string id)
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

        // POST: Guardians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Guardian guardian = await Db.Guardians.FindAsync(id);
            Db.Guardians.Remove(guardian);
            await Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult UpLoadGuardian()
        {
            return View();
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
                        TempData["UserMessage"] = lineError;
                        TempData["Title"] = "Error.";
                        return View();
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
                                MotherMaidenName = mothermaidenName
                            };
                            Db.Guardians.Add(guardian);
                            recordCount++;
                            lastrecord =
                                $"The last Updated record has the Last Name {lastName} and First Name {firstName} with Phone Number {phoneNumber}";

                        }
                        catch (Exception e)
                        {
                            return View("Error3");
                        }
                        await Db.SaveChangesAsync();
                        message = $"You have successfully Uploaded {recordCount} records...  and {lastrecord}";
                        TempData["UserMessage"] = message;
                        TempData["Title"] = "Success.";
                    }

                    return RedirectToAction("Index", "Guardians");
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
