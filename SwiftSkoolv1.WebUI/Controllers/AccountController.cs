using HopeAcademySMS.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OfficeOpenXml;
using SwiftSkool.Services;
using SwiftSkoolv1.Domain;
using SwiftSkoolv1.WebUI.Models;
using SwiftSkoolv1.WebUI.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private string validation = String.Empty;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> GuardianIndex()
        {
            var role = await Db.Roles.AsNoTracking().SingleOrDefaultAsync(m => m.Name == "Guardian");
            //var usersInRole = db.Users.Where(m => m.Roles.Any(r => r.RoleId == role.Id));
            return View(await UserManager.Users.AsNoTracking().Where(m => m.Roles.Any(r => r.RoleId == role.Id)).ToListAsync());
        }

        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            return View(await UserManager.Users.ToListAsync());
        }


        //// GET: /Account/Edit/1
        //public async Task<ActionResult> Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var user = await UserManager.FindByIdAsync(id);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    var userRoles = await UserManager.GetRolesAsync(user.Id);
        //    ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FullName");
        //    return View(new GuardianViewModel()
        //    {
        //        GuardianId = user.Id,
        //        FirstName = user.FirstName,
        //        LastName = user.LastName,
        //        PhoneNumber = user.PhoneNumber,
        //        Address = user.Address,
        //        Email = user.Email,
        //        //RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
        //        //{
        //        //    Selected = userRoles.Contains(x.Id),
        //        //    Text = x.Name,
        //        //    Value = x.Name
        //        //})
        //    });
        //}

        //// POST: /Users/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "Email,Id")] GuardianViewModel editUser)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await UserManager.FindByIdAsync(editUser.GuardianId);
        //        if (user == null)
        //        {
        //            return HttpNotFound();
        //        }

        //        user.UserName = editUser.Username;
        //        user.Email = editUser.Email;

        //        var userRoles = await UserManager.GetRolesAsync(user.Id);

        //        selectedRole = selectedRole ?? new string[] { };

        //        var result = await UserManager.AddUserToRolesAsync(user.Id, selectedRole.Except(userRoles).ToList<string>());

        //        if (!result.Succeeded)
        //        {
        //            ModelState.AddModelError("", result.Errors.First());
        //            return View();
        //        }
        //        result = await UserManager.RemoveUserFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToList<string>());

        //        if (!result.Succeeded)
        //        {
        //            ModelState.AddModelError("", result.Errors.First());
        //            return View();
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    ModelState.AddModelError("", "Something failed.");
        //    return View();
        //}


        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login4(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login4(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: 
            var user = await Db.Users.AsNoTracking().FirstOrDefaultAsync(c => c.Email.Equals(model.Email)
                                                    || c.UserName.Equals(model.Email)
                                                    || c.Id.Equals(model.Email));

            if (user == null)
            {
                return View("Error1");
            }
            var result = await SignInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("CustomDashborad", new { username = user.UserName });
                case SignInStatus.LockedOut:
                    return View("Lockout");

                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }


        public ActionResult CustomDashborad(string username)
        {
            if (User.IsInRole(RoleName.Admin))
            {
                TempData["UserMessage"] = $"Login Successful, Welcome {username}";
                TempData["Title"] = "Success.";
                return RedirectToAction("AdminDashBoard", "Home");
                // return RedirectToAction("AdminDashboard", "Home");
            }

            if (User.IsInRole(RoleName.Student))
            {
                //var model = await Db.Students.Where(x => x.StudentId.Equals(username)).FirstOrDefaultAsync();
                //model.IsLogin = true;
                //Db.Entry(model).State = EntityState.Modified;
                //await Db.SaveChangesAsync();

                //IdentityResult result = await UserManager.UpdateAsync(model);
                TempData["UserMessage"] = $"Login Successful, Welcome {username}";
                TempData["Title"] = "Success.";
                return RedirectToAction("Dashboard", "Students");
            }
            if (User.IsInRole(RoleName.Teacher))
            {
                TempData["UserMessage"] = $"Login Successful, Welcome {username}";
                TempData["Title"] = "Success.";
                return RedirectToAction("TeacherDashboard", "Staffs");
            }

            if (User.IsInRole(RoleName.FormTeacher))
            {
                TempData["UserMessage"] = $"Login Successful, Welcome {username}";
                TempData["Title"] = "Success.";
                return RedirectToAction("StaffDashboard", "Staffs");
            }

            if (User.IsInRole(RoleName.SuperAdmin))
            {
                TempData["UserMessage"] = $"Login Successful, Welcome {username}";
                TempData["Title"] = "Success.";
                return RedirectToAction("GeneralDashboard", "Home");
            }
            return RedirectToAction("Index4", "Home");
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        // GET: /Account/RegisterStaff
        [AllowAnonymous]
        public ActionResult RegisterStaff()
        {
            //ViewBag.Name = new SelectList(Db.Roles.AsNoTracking().ToList(), "Name", "Name").ToList();
            //ViewBag.Department = new SelectList(db.Departments.ToList(), "DeptCode", "DeptName");
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterStaff(StaffViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var staff = new Staff
                    {
                        Salutation = model.Salutation.ToString(),
                        FirstName = model.FirstName,
                        MiddleName = model.MiddleName,
                        LastName = model.LastName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        Address = model.Address,
                        MaritalStatus = model.MaritalStatus.ToString(),
                        DateOfBirth = model.DateOfBirth,
                        Designation = model.Designation,
                        Qualifications = model.Qualifications.ToString(),
                        StaffPassport = model.StaffPassport,
                        StateOfOrigin = model.StateOfOrigin.ToString(),
                        Gender = model.Gender.ToString(),
                        Password = model.Password,
                        SchoolId = userSchool,
                        StaffId = user.Id
                    };
                    Db.Staffs.Add(staff);
                    await Db.SaveChangesAsync();

                    //Assign Role to user Here 
                    await this.UserManager.AddToRoleAsync(user.Id, "Teacher");

                    //Disabled to avoid login in Automatically
                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    TempData["UserMessage"] = "Staff Has been Successfully Created.";
                    TempData["Title"] = "Success.";
                    return RedirectToAction("Index", "Staffs");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            ViewBag.Name = new SelectList(Db.Roles.AsNoTracking().ToList(), "Name", "Name").ToList();
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult UploadTeacher()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> UploadTeacher(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please Select a excel file <br/>";
                return View("Index");
            }
            else
            {
                HttpPostedFileBase file = Request.Files["excelfile"];
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    ExcelValidation myExcel = new ExcelValidation();
                    string lastrecord = "";
                    int recordCount = 0;
                    string message = "";
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                    // Read data from excel file
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        int requiredField = 12;

                        //string validCheck = ValidateExcel(noOfRow, workSheet);
                        string validCheck = myExcel.ValidateExcel(noOfRow, workSheet, requiredField);
                        if (validCheck.Equals("Fail"))
                        {
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

                            string salutation = workSheet.Cells[row, 1].Value.ToString().Trim();
                            string firstName = workSheet.Cells[row, 2].Value.ToString().Trim();
                            string middleName = workSheet.Cells[row, 3].Value.ToString().Trim();
                            string lastName = workSheet.Cells[row, 4].Value.ToString().Trim();
                            string phoneNumber = workSheet.Cells[row, 5].Value.ToString().Trim();
                            string email = workSheet.Cells[row, 6].Value.ToString().Trim();
                            string gender = workSheet.Cells[row, 7].Value.ToString().Trim();
                            string address = workSheet.Cells[row, 8].Value.ToString().Trim();
                            string stateOffOrigin = workSheet.Cells[row, 9].Value.ToString().Trim();
                            string designation = workSheet.Cells[row, 10].Value.ToString().Trim();
                            DateTime dateofBirth = DateTime.Parse(workSheet.Cells[row, 11].Value.ToString().Trim());
                            string maritalStatus = workSheet.Cells[row, 12].Value.ToString().Trim();
                            string qualification = workSheet.Cells[row, 13].Value.ToString().Trim();
                            string password = workSheet.Cells[row, 14].Value.ToString().Trim();
                            string username = firstName.Trim() + " " + lastName.Trim();

                            #region database operation

                            var user = new ApplicationUser { UserName = username, Email = email };
                            var result = await UserManager.CreateAsync(user, password);

                            if (result.Succeeded)
                            {
                                #region Creating New staff

                                try
                                {
                                    var staff = new Staff()
                                    {
                                        StaffId = user.Id,
                                        Salutation = salutation,
                                        FirstName = firstName,
                                        MiddleName = middleName,
                                        LastName = lastName,
                                        PhoneNumber = phoneNumber,
                                        Email = email,
                                        Gender = gender,
                                        Address = address,
                                        StateOfOrigin = stateOffOrigin,
                                        Designation = designation,
                                        DateOfBirth = dateofBirth,
                                        MaritalStatus = maritalStatus,
                                        Qualifications = qualification,
                                        Password = password,
                                        SchoolId = userSchool
                                    };
                                    Db.Staffs.Add(staff);
                                }
                                catch (Exception e)
                                {
                                    return View();
                                }

                                #endregion

                                //Assign Role to user Here 
                                await this.UserManager.AddToRoleAsync(user.Id, "Teacher");

                                recordCount++;
                                lastrecord = $"The last Updated record has the Last Name {lastName} and First Name {firstName} with staff phonenumber {phoneNumber}";

                            }
                            #endregion
                            await Db.SaveChangesAsync();
                            message = $"You have successfully Uploaded {recordCount} records...  and {lastrecord}";
                            TempData["UserMessage"] = message;
                            TempData["Title"] = "Success.";
                        }

                    }

                    return RedirectToAction("Index", "Staffs");
                }
                else
                {
                    ViewBag.Error = $"File type is Incorrect <br/>";
                    return View("Index");
                }

            }
        }

        private string ValidateExcel(int noOfRow, ExcelWorksheet workSheet)
        {
            for (int row = 2; row <= noOfRow; row++)
            {
                try
                {
                    string salutation = workSheet.Cells[row, 1].Value.ToString().Trim();
                    string firstName = workSheet.Cells[row, 2].Value.ToString().Trim();
                    string middleName = workSheet.Cells[row, 3].Value.ToString().Trim();
                    string lastName = workSheet.Cells[row, 4].Value.ToString().Trim();
                    string phoneNumber = workSheet.Cells[row, 5].Value.ToString().Trim();
                    string email = workSheet.Cells[row, 6].Value.ToString().Trim();
                    string gender = workSheet.Cells[row, 7].Value.ToString().Trim();
                    string address = workSheet.Cells[row, 8].Value.ToString().Trim();
                    string stateOffOrigin = workSheet.Cells[row, 9].Value.ToString().Trim();
                    string designation = workSheet.Cells[row, 10].Value.ToString().Trim();
                    DateTime dateofBirth = DateTime.Parse(workSheet.Cells[row, 11].Value.ToString().Trim());
                    string maritalStatus = workSheet.Cells[row, 12].Value.ToString().Trim();
                    string qualification = workSheet.Cells[row, 13].Value.ToString().Trim();
                    string password = workSheet.Cells[row, 14].Value.ToString().Trim();
                    string username = firstName.Trim() + " " + lastName.Trim();

                }
                catch (Exception e)
                {
                    validation = row.ToString();
                    return "Fail";
                }
            }
            return "Success";
        }


        [AllowAnonymous]
        public ActionResult UpLoadStudent()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> UpLoadStudent(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please Select a excel file <br/>";
                return View("UpLoadStudent");
            }
            else
            {
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
                        int requiredField = 13;

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
                            string studentId = workSheet.Cells[row, 1].Value.ToString().Trim();
                            string firstName = workSheet.Cells[row, 2].Value.ToString().Trim();
                            string middleName = workSheet.Cells[row, 3].Value.ToString().Trim();
                            string lastName = workSheet.Cells[row, 4].Value.ToString().Trim();
                            string gender = workSheet.Cells[row, 5].Value.ToString().Trim();
                            DateTime dateOfBirth = DateTime.Parse(workSheet.Cells[row, 6].Value.ToString().Trim());
                            string placeofBirth = workSheet.Cells[row, 7].Value.ToString().Trim();
                            string state = workSheet.Cells[row, 8].Value.ToString().Trim();
                            string religion = workSheet.Cells[row, 9].Value.ToString().Trim();
                            string tribe = workSheet.Cells[row, 10].Value.ToString().Trim();
                            DateTime addmision = DateTime.Parse(workSheet.Cells[row, 11].Value.ToString().Trim());
                            string phoneNumber = workSheet.Cells[row, 12].Value.ToString().Trim();
                            string password = workSheet.Cells[row, 13].Value.ToString().Trim();
                            string username = lastName.Trim() + " " + firstName.Trim();
                            try
                            {
                                var student = new Student()
                                {
                                    StudentId = studentId,
                                    FirstName = firstName,
                                    MiddleName = middleName,
                                    LastName = lastName,
                                    PhoneNumber = phoneNumber,
                                    Gender = gender,
                                    Religion = religion,
                                    PlaceOfBirth = placeofBirth,
                                    StateOfOrigin = state,
                                    Tribe = tribe,
                                    DateOfBirth = dateOfBirth,
                                    AdmissionDate = addmision,
                                    SchoolId = userSchool
                                };
                                Db.Students.Add(student);

                                recordCount++;
                                lastrecord =
                                    $"The last Updated record has the Last Name {lastName} and First Name {firstName} with Phone Number {phoneNumber}";

                                var user = new ApplicationUser
                                {
                                    Id = studentId,
                                    UserName = username,
                                    //Email = email.Trim(),
                                    PhoneNumber = phoneNumber.Trim(),

                                };
                                var result = await UserManager.CreateAsync(user, password);
                                if (result.Succeeded)
                                {
                                    //Assign Role to user Here 
                                    await this.UserManager.AddToRoleAsync(user.Id, "Student");
                                }
                            }
                            catch (Exception e)
                            {
                                message = $"You have successfully Uploaded {recordCount} records...  and {lastrecord}";
                                TempData["UserMessage"] = message + e.Message;
                                TempData["Title"] = "Success.";
                                return View("Error3");
                            }


                        }
                        await Db.SaveChangesAsync();
                        message = $"You have successfully Uploaded {recordCount} records...  and {lastrecord}";
                        TempData["UserMessage"] = message;
                        TempData["Title"] = "Success.";
                        return RedirectToAction("Index", "Students");
                    }
                }
                else
                {
                    ViewBag.Error = "File type is Incorrect <br/>";
                    return View("UploadStudent");
                }
            }
        }


        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult RegisterStudent()
        {
            //ViewBag.StudentId = new SelectList(Db.Students, "StudentId", "FullName");
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterStudent(StudentViewModel model)
        {

            if (ModelState.IsValid)
            {
                string studentUpdated = "";
                var user = new ApplicationUser
                {
                    Id = model.StudentId,
                    UserName = model.UserName,
                    //Email = model.Email.Trim(),
                    PhoneNumber = model.PhoneNumber.Trim(),
                    SchoolId = userSchool
                };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    #region Student
                    var student = new Student()
                    {
                        StudentId = model.StudentId,
                        FirstName = model.FirstName,
                        MiddleName = model.MiddleName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        Gender = model.Gender.ToString(),
                        Religion = model.Religion.ToString(),
                        DateOfBirth = model.DateOfBirth,
                        PlaceOfBirth = model.PlaceOfBirth,
                        StateOfOrigin = model.StateOfOrigin.ToString(),
                        Tribe = model.Tribe,
                        AdmissionDate = model.AdmissionDate,
                        StudentPassport = model.StudentPassport,
                        SchoolId = userSchool
                    };
                    Db.Students.Add(student);
                    TempData["UserMessage"] = "Student has been Added Successfully";
                    TempData["Title"] = "Success.";
                    await Db.SaveChangesAsync();
                    #endregion

                    //Assign Role to user Here 
                    await this.UserManager.AddToRoleAsync(user.Id, "Student");

                    // To avoid automatic login
                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");


                    return RedirectToAction("Index", "Students");
                }
                AddErrors(result);
            }
            ViewBag.StudentId = new SelectList(Db.Students.AsNoTracking(), "StudentId", "FullName");
            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.Class = new SelectList(Db.Classes.AsNoTracking(), "MyClassName", "MyClassName");
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Username, Email = model.Email, Id = model.StudentId };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    //Assign Role to user Here 
                    await this.UserManager.AddToRoleAsync(user.Id, "Student");

                    // To avoid automatic login
                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Dashboard", "Students");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        // GET: /Account/Register
        [CustomAuthorize(Roles = RoleName.SuperAdmin)]
        public ActionResult RegisterAdmin()
        {
            ViewBag.SchoolId = new SelectList(Db.Schools.AsNoTracking(), "SchoolId", "Name");
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [CustomAuthorize(Roles = RoleName.SuperAdmin)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterAdmin(RegisterAdminVm model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Username, Email = model.Email, SchoolId = model.SchoolId };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    //Assign Role to user Here 
                    await this.UserManager.AddToRoleAsync(user.Id, "Admin");

                    // To avoid automatic login
                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("AdminDashboard", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //GET: /Account/ConfirmEmail
        [
        AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login4");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login4", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Dashboard", "Students");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

        public ActionResult Details(string id)
        {
            throw new NotImplementedException();
        }

        // GET: /Users/Delete/5
        [AllowAnonymous]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Users/Delete/5
        [AllowAnonymous]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        private async Task DeleteUser(string id)
        {
            var user = UserManager.Users.FirstOrDefault(x => x.UserName.Equals(id));
            // var user = await UserManager.FindByIdAsync(id);
            var result = await UserManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", result.Errors.First());

            }
        }
    }
}