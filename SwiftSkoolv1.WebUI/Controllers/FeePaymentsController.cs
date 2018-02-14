using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using OfficeOpenXml;
using SwiftSkoolv1.Domain;
using SwiftSkoolv1.WebUI.ViewModels;
using SwiftSkoolv1.WebUI.ViewModels.RemitaVm;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class FeePaymentsController : BaseController
    {
        // GET: FeePayments
        public async Task<ActionResult> Index()
        {
            var studentId = User.Identity.GetUserId();
            var schoolFeePayments = Db.FeePayments.Include(s => s.Students);
            schoolFeePayments = schoolFeePayments.Where(x => x.StudentId.Equals(studentId)
                && x.Status.Equals(true));
            return View(await schoolFeePayments.ToListAsync());
        }

        public async Task<ActionResult> RemoveTransaction(string studentId)
        {
            var message = string.Empty;
            if (!string.IsNullOrEmpty(studentId))
            {
                var student = await Db.Students.AsNoTracking().Where(x => x.StudentId.Equals(studentId))
                                    .Select(x => x.StudentId).FirstOrDefaultAsync();

                var session = _query.CurrentSession();
                if (student != null)
                {
                    var transactions = await Db.FeePayments.AsNoTracking().Where(x =>
                        x.StudentId.Equals(student) && x.Session.Equals(session)
                        && x.FeeCategory.Equals(FeeCategory.School_Fee.ToString())).FirstOrDefaultAsync();
                    if (transactions != null)
                    {
                        var entry = Db.Entry(transactions);
                        if (entry.State == EntityState.Detached)
                            Db.FeePayments.Attach(transactions);
                        Db.FeePayments.Remove(transactions);
                        await Db.SaveChangesAsync();
                        message = "Message Removed Successfully";
                    }
                    else
                    {
                        message = "Student Transaction not found";
                    }

                }
                else
                {
                    message = "Student not found";
                }
            }
            else
            {
                message = "Student Id is empty";
            }

            ViewBag.Message = message;

            return View();
        }

        public async Task<ActionResult> GetSchoolFee(string gender)
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
            string feeCategory = FeeCategory.School_Fee.ToString();
            var sessionId = _query.CurrentSession();
            var schoolFee = await Db.FeePayments.Include(i => i.Students).Include(i => i.Session)
                                .AsNoTracking().Where(x => x.FeeCategory.Equals(feeCategory) &&
                                x.Status.Equals(true) && x.Session.Equals(sessionId)).ToListAsync();
            var v = schoolFee.Select(s => new
            {
                s.Students.StudentId,
                s.Students.FullName,
                s.Students.Gender,
                s.ReferenceNo,
                s.TotalAmount,
                Date = s.Date.ToString(),
                s.Students.PhoneNumber
            }).ToList();

            if (!string.IsNullOrEmpty(search))
            {
                v = v.Where(x => x.StudentId.Equals(search) || x.FullName.Contains(search)
                            || x.ReferenceNo.Equals(search)).ToList();
            }

            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data },
                JsonRequestBehavior.AllowGet);

            #endregion Server Side filtering
        }


        // GET: FeePayments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var schoolFeePayment = await Db.FeePayments.FindAsync(id);
            if (schoolFeePayment == null)
            {
                return HttpNotFound();
            }
            return View(schoolFeePayment);
        }


        [HttpGet]
        public ActionResult MakePayment(string message)
        {
            var feeCategory = from FeeCategory s in Enum.GetValues(typeof(FeeCategory))
                              select new { ID = s, Name = s.ToString() };

            ViewBag.FeeCategory = new SelectList(feeCategory, "Name", "Name");
            ViewBag.TermName = new SelectList(_query.TermList(), "TermName", "TermName");
            ViewBag.SessionName = new SelectList(Db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.StudentId = User.Identity.GetUserName();
            ViewBag.Message = message;
            return View();
        }


        // GET: FeePayments/Create
        public async Task<ActionResult> Create(FeePaymentVm model)
        {

            var studentId = User.Identity.GetUserId();
            var hasPayedList = await Db.FeePayments.AsNoTracking().Where(x => x.StudentId.Equals(studentId) &&
                                        x.Session.Equals(model.SessionName) && x.FeeCategory.Equals(model.FeeCategory))
                                        .FirstOrDefaultAsync();
            if (hasPayedList.Status.Equals(true))
            {
                return View("Index");
            }

            var student = await Db.Students.AsNoTracking()
                                .FirstOrDefaultAsync(s => s.StudentId.Equals(studentId));

            var paymentList = await GetPaymentList(model, student);

            // var latePayment = await GetLatePaymentList();

            var fullName = $"{student.LastName} {student.FirstName} {student.MiddleName}";
            var feeList = new List<FeeList>();
            //if (latePayment != null)
            //{
            //    int dateCompare1 = DateTime.Compare(latePayment.EndDate, DateTime.Now.Date);
            //    if (dateCompare1 > 0)
            //    {
            //        var lateFee = new FeeList
            //        {
            //            FeeTypeName = "Late Registration",
            //            Description = "Fee charges for Late registration",
            //            Amount = Convert.ToDecimal(latePayment.FinedAmount)
            //        };
            //        feeList.Add(lateFee);
            //    }
            //}

            feeList.AddRange(paymentList.Select(fee => new FeeList
            {
                FeeTypeName = fee.FeeName,
                Amount = fee.Amount,
                Description = fee.Description
            }));

            //System.Threading.Thread.Sleep(1);
            long milliseconds = DateTime.Now.Ticks;
            var url = Url.Action("ConfrimPayment", "FeePayments", new { }, protocol: Request.Url.Scheme);
            var remitaParam = GetServiceType(model.FeeCategory);


            if (hasPayedList.Status.Equals(false))
            {
                remitaParam = GetServiceType(hasPayedList.FeeCategory);

                var hashed = _query.HashRemitedValidate(hasPayedList.OrderId, remitaParam.ApiKey, remitaParam.MerchantId);
                string checkurl = RemitaConfigParams.CHECKSTATUSURL + "/" + remitaParam.MerchantId + "/" + hasPayedList.OrderId + "/" + hashed + "/" + "orderstatus.reg";
                string jsondata = new WebClient().DownloadString(checkurl);
                var result = JsonConvert.DeserializeObject<RemitaResponse>(jsondata);
                if (string.IsNullOrEmpty(result.Rrr))
                {
                    var entry = Db.Entry(hasPayedList);
                    if (entry.State == EntityState.Detached)
                        Db.FeePayments.Attach(hasPayedList);
                    Db.FeePayments.Remove(hasPayedList);
                    await Db.SaveChangesAsync();
                }
                else
                {
                    return RedirectToAction("ConfrimPayment", new { orderID = hasPayedList.OrderId });
                }
            }



            var confirmPaymentVm = new ConfirmPaymentVm
            {
                FeeLists = feeList,
                StudentName = fullName,
                StudentId = studentId,
                FeeCategory = model.FeeCategory,
                TotalAmount = paymentList.Sum(s => s.Amount),
                SessionName = model.SessionName,
                TermName = model.TermName,
                payerName = fullName,
                payerEmail = $"{fullName}@gamil.com",
                payerPhone = student.PhoneNumber,
                amt = paymentList.Sum(s => s.Amount).ToString(CultureInfo.InvariantCulture)
            };

            confirmPaymentVm.TotalAmount = paymentList.Sum(s => s.Amount);
            confirmPaymentVm.merchantId = remitaParam.MerchantId;
            confirmPaymentVm.orderId = $"{userSchool}{milliseconds}";
            confirmPaymentVm.responseurl = url;
            confirmPaymentVm.serviceTypeId = remitaParam.ServiceType;

            if (confirmPaymentVm.TotalAmount < 10)
            {
                return RedirectToAction("MakePayment",
                    new { message = "Payment is not currently set at the moment, Please try again..." });
            }

            return View(confirmPaymentVm);


        }

        public RemitaFeeSetting GetServiceType(string feeCategory)
        {
            string schoolId = userSchool;
            var remitaSetting = Db.RemitaFeeSettings.AsNoTracking().FirstOrDefault(x => x.SchoolId.Equals(schoolId)
                                && x.FeeCategory.Equals(feeCategory));
            return remitaSetting;
        }

        //private async Task<SchoolFeeSetting> GetLatePaymentList()
        //{
        //    var latePayment = await Db.SchoolFeeSettings.Where(x => x.SemesterId.Equals(semesterId)
        //                                                             && x.SessionId.Equals(sessionId) &&
        //                                                             x.IsActive.Equals(true)).FirstOrDefaultAsync();
        //    return latePayment;
        //}

        private async Task<List<FeeType>> GetPaymentList(FeePaymentVm model, Student student)
        {
            var className = _query.GetMyClass(userSchool, student.StudentId);
            var paymentList = await Db.FeeTypes.AsNoTracking()
                            .Where(x => x.FeeCategory.Equals(model.FeeCategory.ToString())
                            && x.StudentType.Equals(student.StudentStatus)
                            && x.TermName.Equals(model.TermName)
                            && x.ClassName.Equals(className)).ToListAsync();
            return paymentList;
        }

        // POST: FeePayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ConfirmPaymentVm model)
        {
            if (ModelState.IsValid)
            {

                var hasTransaction = await Db.FeePayments.AsNoTracking().Where(x => x.StudentId.Equals(model.StudentId)
                                                && x.Session.Equals(model.SessionName)
                                                && x.FeeCategory.Equals(model.FeeCategory))
                                                .ToListAsync();
                model.paymenttype = model.RemitaPaymentType.ToString().Replace("_", " ").ToLower();


                if (string.IsNullOrEmpty(model.payerEmail))
                {
                    model.payerEmail = $"{model.payerName}@compunet.edu";
                }
                if (string.IsNullOrEmpty(model.payerPhone))
                {
                    model.payerPhone = "070300000000";
                }
                var remitaParam = GetServiceType(model.FeeCategory);

                var schoolFeePayment = new FeePayment
                {
                    OrderId = model.orderId,
                    FeeCategory = model.FeeCategory,
                    Date = DateTime.Now,
                    Term = model.TermName,
                    Session = model.SessionName,
                    StudentId = model.StudentId,
                    PaidFee = model.TotalAmount,
                    TotalAmount = model.TotalAmount,


                };
                Db.FeePayments.Add(schoolFeePayment);
                var log = new RemitaPaymentLog
                {
                    OrderId = model.orderId,
                    FeeCategory = model.FeeCategory,
                    PaymentDate = DateTime.Now,
                    Amount = model.TotalAmount.ToString(),
                    PaymentName = model.StudentName

                };
                Db.RemitaPaymentLogs.Add(log);
                await Db.SaveChangesAsync();
                model.hash = _query.HashRemitaRequest(model.merchantId, model.serviceTypeId, model.orderId, model.amt, model.responseurl, remitaParam.ApiKey);
                return RedirectToAction("SubmitRemita", model);
            }
            return View(model);
        }


        [AllowAnonymous]
        public ActionResult SubmitRemita(ConfirmPaymentVm model)
        {
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult RetrySchoolFeePayment(string rrr)
        {
            var remitaParam = GetServiceType(rrr);

            var hashrrr = _query.HashRrrQuery(rrr, remitaParam.ApiKey, remitaParam.MerchantId);
            string posturl = RemitaConfigParams.CHECKSTATUSURL + "/" + remitaParam.MerchantId + "/" + rrr + "/" + hashrrr + "/" + "status.reg";
            string jsondata = new WebClient().DownloadString(posturl);
            var result = JsonConvert.DeserializeObject<RemitaResponse>(jsondata);
            if (result.Status.Equals("00") || result.Status.Equals("01"))
            {
                return RedirectToAction("ConfrimPayment", "FeePayments", new { RRR = result.Rrr, orderID = result.OrderId });
            }
            var url = Url.Action("ConfrimPayment", "FeePayments", new { }, protocol: Request.Url.Scheme);
            var hash = _query.HashRemitedRePost(remitaParam.MerchantId, rrr, remitaParam.ApiKey);

            var model = new RemitaRePostVm
            {
                rrr = rrr,
                merchantId = remitaParam.MerchantId,
                hash = hash,
                responseurl = url
            };
            return View(model);
        }




        [AllowAnonymous]
        public async Task<ActionResult> ConfrimPayment(string RRR, string orderID)
        {
            FeePayment schoofeepayment;
            RemitaResponse result = new RemitaResponse();
            if (string.IsNullOrEmpty(orderID))
            {
                schoofeepayment = await Db.FeePayments.AsNoTracking()
                    .Where(x => x.ReferenceNo.Equals(RRR))
                    .FirstOrDefaultAsync();
            }
            else
            {
                schoofeepayment = await Db.FeePayments.AsNoTracking()
                    .Where(x => x.OrderId.Equals(orderID.Trim()))
                    .FirstOrDefaultAsync();
            }
            if (schoofeepayment != null)
            {
                var remitaParam = GetServiceType(schoofeepayment.FeeCategory);

                if (schoofeepayment.Status.Equals(true))
                {

                    result.Message = schoofeepayment.PaymentStatus;
                    result.OrderId = schoofeepayment.OrderId;
                    result.Rrr = schoofeepayment.ReferenceNo;
                    result.Status = schoofeepayment.Status.ToString();
                    return RedirectToAction("ConfrimRrrPayment", "RemitaPaymentLogs", result);
                }
                var log = await Db.RemitaPaymentLogs.AsNoTracking().Where(x => x.OrderId.Equals(schoofeepayment.OrderId))
                                            .FirstOrDefaultAsync();

                var hashed = _query.HashRemitedValidate(schoofeepayment.OrderId, remitaParam.ApiKey, remitaParam.MerchantId);
                string url = RemitaConfigParams.CHECKSTATUSURL + "/" + remitaParam.MerchantId + "/" + schoofeepayment.OrderId + "/" + hashed + "/" + "orderstatus.reg";
                string jsondata = new WebClient().DownloadString(url);
                result = JsonConvert.DeserializeObject<RemitaResponse>(jsondata);

                if (result.Status.Equals("00") || result.Status.Equals("01"))
                {
                    schoofeepayment.Status = true;
                    schoofeepayment.PaymentStatus = result.Message;
                    schoofeepayment.ReferenceNo = result.Rrr;
                    Db.Entry(schoofeepayment).State = EntityState.Modified;

                    await UpdateStudentRecord(schoofeepayment.FeeCategory, schoofeepayment.StudentId);

                    _query.UpdateTransactionLog(log, result);
                    await Db.SaveChangesAsync();
                }
                else
                {
                    schoofeepayment.Status = false;
                    schoofeepayment.PaymentStatus = result.Message;
                    schoofeepayment.ReferenceNo = result.Rrr;
                    Db.Entry(schoofeepayment).State = EntityState.Modified;

                    _query.UpdateTransactionLog(log, result);
                    await Db.SaveChangesAsync();
                    return RedirectToAction("RetrySchoolFeePayment", new { rrr = result.Rrr });

                }

                return RedirectToAction("ConfrimRrrPayment", "RemitaPaymentLogs", result);
            }
            var message = $"There is no payment that has either the RRR {RRR} or" +
                            $" Order Id {orderID} for School fee or Acceptance Fee";
            return RedirectToAction("GetPaymentStatus", "RemitaServices", new { message = message });

        }



        private async Task UpdateStudentRecord(string schoofeepayment, string studentId)
        {
            if (schoofeepayment.Equals(FeeCategory.School_Fee.ToString()))
            {
                var student = await Db.Students.Where(x => x.StudentId.Equals(studentId))
                    .FirstOrDefaultAsync();
                student.StudentStatus = StudentStatus.Returning.ToString();
                Db.Entry(student).State = EntityState.Modified;
            }
        }


        //public async Task<ActionResult> PrintReceipt(int id)
        //{
        //    var schoolFee = await Db.FeePayments.Include(i => i.Session).AsNoTracking()
        //                        .Where(x => x.FeePaymentId.Equals(id)).FirstOrDefaultAsync();
        //    var schoolFeePayment = new SchoolFeeReciept();
        //    schoolFeePayment.Student = await Db.Students.AsNoTracking().Where(x => x.StudentId.Equals(schoolFee.StudentId))
        //                                    .FirstOrDefaultAsync();
        //    schoolFeePayment.FeeCategory = schoolFee.FeeCategory;
        //    schoolFeePayment.SchoolFeeTypes = await Db.FeeTypes.AsNoTracking().Where(x => x.FeeCategory.Equals(schoolFee.FeeCategory)
        //                                                    && x.StudentType.Equals(schoolFeePayment.Student.StudentStatus)
        //                                                    && x.Session.Equals(schoolFeePayment.Student.Indegine)
        //                                                    && x.Level.LevelId.Equals(schoolFeePayment.Student.Level.LevelId)).ToListAsync();

        //    schoolFeePayment.SchoolFeePayment = schoolFee;

        //    //return View(schoolFeePayment);
        //    return new ViewAsPdf(schoolFeePayment);
        //}

        //public async Task<ActionResult> SchoolFeePayment()
        //{
        //    ViewBag.SessionId = new SelectList(await Db.Sessions.AsNoTracking().ToListAsync(), "SessionId", "SessionName");
        //    ViewBag.DepartmentId = new SelectList(await Db.Departments.AsNoTracking().ToListAsync(), "DepartmentId", "DeptName");
        //    ViewBag.LevelId = new SelectList(await Db.Levels.AsNoTracking().ToListAsync(), "LevelId", "LevelName");
        //    var feeCategory = from FeeCategory s in Enum.GetValues(typeof(FeeCategory))
        //                      select new { ID = s, Name = s.ToString() };

        //    ViewBag.FeeCategoryId = new MultiSelectList(feeCategory, "Name", "Name");

        //    return View();
        //}


        //public async Task<ActionResult> SchoolFeeDefaulters()
        //{
        //    ViewBag.SessionId = new SelectList(await Db.Sessions.AsNoTracking().ToListAsync(), "SessionId", "SessionName");
        //    ViewBag.DepartmentId = new SelectList(await Db.Departments.AsNoTracking().ToListAsync(), "DepartmentId", "DeptName");
        //    ViewBag.LevelId = new SelectList(await Db.Levels.AsNoTracking().ToListAsync(), "LevelId", "LevelName");
        //    var feeCategory = from FeeCategory s in Enum.GetValues(typeof(FeeCategory))
        //                      select new { ID = s, Name = s.ToString() };

        //    ViewBag.FeeCategoryId = new SelectList(feeCategory, "Name", "Name");
        //    return View();
        //}

        //public async Task<ActionResult> GetStudentPayment(string FeeCategoryId, int? DepartmentId, int? LevelId, int? SessionId)
        //{
        //    if (SessionId == null)
        //    {
        //        SessionId = _query.GetCurrentSessionId();
        //    }
        //    var schoolFee = await Db.FeePayments.Include(i => i.Students).Include(i => i.Session)
        //                            .Include(i => i.Students.Programme.Department).Include(i => i.Students.Level)
        //                            .AsNoTracking().Where(x => x.FeeCategory.Equals(FeeCategoryId) &&
        //                            x.Status.Equals(true) && x.SessionId.Equals((int)SessionId)).ToListAsync();
        //    if (DepartmentId != null & LevelId != null)
        //    {
        //        schoolFee = schoolFee.Where(x => x.Students.Programme.Department.DepartmentId.Equals((int)DepartmentId)
        //                                         && x.Students.Level.LevelId.Equals((int)LevelId)
        //                                         && x.SessionId.Equals((int)SessionId)).ToList();
        //    }
        //    else if (DepartmentId != null)
        //    {
        //        schoolFee = schoolFee.Where(x => x.Students.Programme.Department.DepartmentId.Equals((int)DepartmentId)
        //                                       ).ToList();
        //    }
        //    else if (LevelId != null)
        //    {
        //        schoolFee = schoolFee.Where(x => x.Students.Level.LevelId.Equals((int)LevelId)).ToList();
        //    }

        //    var data = schoolFee.Select(s => new
        //    {
        //        s.Students.MatricNo,
        //        s.Students.FullName,
        //        s.Students.Gender,
        //        s.Students.Programme.Department.DeptName,
        //        s.Students.Programme.ProgrammeName,
        //        s.Students.Level.LevelName,
        //        s.Students.PhoneNumber
        //    }).ToList();
        //    return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        //}

        //public async Task<ActionResult> GetSchoolFeeDefaulter(string FeeCategoryId, int? DepartmentId, int? LevelId, int? SessionId)
        //{
        //    if (SessionId == null)
        //    {
        //        SessionId = _query.GetCurrentSessionId();
        //    }
        //    var defaulterList = new List<Student>();
        //    var studentsList = await Db.Students.Include(i => i.Level).Include(i => i.Programme.Department).AsNoTracking()
        //                            .Where(x => x.Active.Equals(true) && x.IsGraduated.Equals(false)).ToListAsync();

        //    var schoolFee = await Db.FeePayments.Include(i => i.Students).Include(i => i.Session)
        //                            .Include(i => i.Students.Programme.Department).Include(i => i.Students.Level)
        //                            .AsNoTracking().Where(x => x.FeeCategory.Equals(FeeCategoryId) &&
        //                            x.Status.Equals(true) && x.SessionId.Equals((int)SessionId)).ToListAsync();

        //    if (DepartmentId != null & LevelId != null)
        //    {
        //        schoolFee = schoolFee.Where(x => x.Students.Programme.Department.DepartmentId.Equals((int)DepartmentId)
        //                                         && x.Students.Level.LevelId.Equals((int)LevelId)
        //                                         && x.SessionId.Equals((int)SessionId)).ToList();
        //    }
        //    else if (DepartmentId != null)
        //    {
        //        schoolFee = schoolFee.Where(x => x.Students.Programme.Department.DepartmentId.Equals((int)DepartmentId)
        //        ).ToList();
        //    }
        //    else if (LevelId != null)
        //    {
        //        schoolFee = schoolFee.Where(x => x.Students.Level.LevelId.Equals((int)LevelId)).ToList();
        //    }
        //    foreach (var student in studentsList)
        //    {
        //        var defaulter = schoolFee.FirstOrDefault(x => x.StudentId.Equals(student.StudentId));
        //        if (defaulter == null)
        //        {
        //            defaulterList.Add(student);
        //        }
        //    }

        //    var data = defaulterList.Select(s => new
        //    {
        //        s.MatricNo,
        //        s.FullName,
        //        s.Gender,
        //        s.Programme.Department.DeptName,
        //        s.Programme.ProgrammeName,
        //        s.Level.LevelName,
        //        s.PhoneNumber
        //    }).ToList();
        //    return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        //}

        public async Task DownloadPendingPayment()
        {
            //var facilityList = Db.Communications.AsNoTracking().ToList();
            char c1 = 'A';
            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Report");


            worksheet.Cells[$"{c1++}1"].Value = "Student Id";
            worksheet.Cells[$"{c1++}1"].Value = "Student Name";
            worksheet.Cells[$"{c1++}1"].Value = "OrderId";
            worksheet.Cells[$"{c1++}1"].Value = "Fee Type";
            worksheet.Cells[$"{c1++}1"].Value = "Amount";
            worksheet.Cells[$"{c1++}1"].Value = "Status";
            worksheet.Cells[$"{c1++}1"].Value = "Status Message";


            var sessionId = _query.CurrentSession();
            var feepayments = await Db.FeePayments.AsNoTracking().Where(x => x.Status.Equals(false)
                                    && x.Session.Equals(sessionId)).ToListAsync();

            int rowStart = 2;
            char c2 = 'A';

            foreach (var feepayment in feepayments)
            {
                var student = await Db.Students.Where(x => x.StudentId.Equals(feepayment.StudentId))
                                    .Select(s => new { s.StudentId, s.FirstName, s.LastName }).FirstOrDefaultAsync();
                worksheet.Cells[$"A{rowStart}"].Value = student.StudentId;
                worksheet.Cells[$"B{rowStart}"].Value = student.LastName + student.FirstName;
                worksheet.Cells[$"C{rowStart}"].Value = feepayment.OrderId;
                worksheet.Cells[$"D{rowStart}"].Value = feepayment.FeeCategory;
                worksheet.Cells[$"E{rowStart}"].Value = feepayment.ReferenceNo;
                worksheet.Cells[$"F{rowStart}"].Value = feepayment.TotalAmount;
                worksheet.Cells[$"G{rowStart}"].Value = feepayment.Status;
                worksheet.Cells[$"H{rowStart}"].Value = feepayment.PaymentStatus;

                rowStart++;
            }
            // var info = results.FirstOrDefault();
            worksheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + $"PreDegreeExamResult.xlsx");
            Response.BinaryWrite(package.GetAsByteArray());
            Response.End();

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
