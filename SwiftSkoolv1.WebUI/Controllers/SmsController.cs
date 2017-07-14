using SwiftSkool.Models;
using SwiftSkool.Services;
using SwiftSkoolv1.WebUI.ViewModels.Sms;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class SmsController : BaseController
    {

        private readonly SmsServiceTemp _smsService;

        public SmsController()
        {
            _smsService = new SmsServiceTemp();
        }

        //public ActionResult Index()
        //{
        //    ViewData["Message"] = "Welcome to SMS Web Application!";

        //    return View();
        //}

        //public ActionResult About()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<ActionResult> SendSMS()
        {
            //SMS sms = new SMS();
            ViewBag.Term = new SelectList(_query.TermList(), "TermName", "TermName");
            ViewBag.Session = new SelectList(_query.SessionList(), "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            return View();
        }

        [HttpPost]
        public ActionResult SendSMS(SmsViewModel model)
        {

            if (ModelState.IsValid)
            {
                var studentList = Db.AssignedClasses.Where(x => x.ClassName.Equals(model.ClassName) &&
                                                                x.TermName.Equals(model.Term.ToString()) &&
                                                                x.SessionName.Equals(model.Session) &&
                                                                x.SchoolId.Equals(userSchool))
                                                                .Select(y => y.StudentId).ToList();
                foreach (var student in studentList)
                {
                    var guardianNumber =
                        Db.Students.Where(s => s.StudentId.Equals(student)).Select(x => x.PhoneNumber).ToList();
                    foreach (var guardian in guardianNumber)
                    {
                        //var guardianContact = Db.Guardians.Where(x => x.GuardianEmail.Equals(guardian))
                        //                        .Select(y => y.PhoneNumber).FirstOrDefault();

                        SMS sms = new SMS()
                        {
                            SenderId = model.SenderId,
                            Message = model.Message,
                            Numbers = guardian
                        };
                        try
                        {
                            bool isSuccess = false;
                            string errMsg = null;
                            string response = _smsService.Send(sms); //Send sms

                            string code = _smsService.GetResponseMessage(response, out isSuccess, out errMsg);

                            if (!isSuccess)
                            {
                                ModelState.AddModelError("", errMsg);
                            }
                            else
                            {
                                ViewBag.Message = "Message was successfully sent.";
                            }
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", ex.Message);
                        }
                    }

                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult SendToStaff()
        {
            //SMS sms = new SMS();
            // ViewBag.ContactGroup = new SelectList(Db.Classes, "FullClassName", "FullClassName");
            return View();
        }

        [HttpPost]
        public ActionResult SendToStaff(SendToStaffViewModel model)
        {

            if (ModelState.IsValid)
            {
                var staffList = Db.Staffs.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool))
                    .Select(x => x.PhoneNumber);
                foreach (var staffNumber in staffList)
                {
                    SMS sms = new SMS()
                    {
                        SenderId = model.SenderId,
                        Message = model.Message,
                        Numbers = staffNumber
                    };
                    try
                    {
                        bool isSuccess = false;
                        string errMsg = null;
                        string response = _smsService.Send(sms); //Send sms

                        string code = _smsService.GetResponseMessage(response, out isSuccess, out errMsg);

                        if (!isSuccess)
                        {
                            ModelState.AddModelError("", errMsg);
                        }
                        else
                        {
                            ViewBag.Message = "Message was successfully sent.";
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }


                }
            }

            return View(model);
        }


        [HttpGet]
        public ActionResult SendtoAllStudent()
        {
            //SMS sms = new SMS();
            ViewBag.Term = new SelectList(_query.TermList(), "TermName", "TermName");
            ViewBag.Session = new SelectList(_query.SessionList(), "SessionName", "SessionName");
            // ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            return View();
        }

        [HttpPost]
        public ActionResult SendtoAllStudent(SendToAllViewModel model)
        {

            if (ModelState.IsValid)
            {
                var studentList = Db.Students.Where(x => x.Active.Equals(true) &&
                                                         x.IsGraduated.Equals(false)
                                                         && x.SchoolId.Equals(userSchool))
                    .Select(y => y.PhoneNumber).ToList();
                foreach (var student in studentList)
                {
                    //var guardianList = Db.Guardians.Where(x => x.StudentId.Equals(student))
                    //                                .Select(y => y.GuardianEmail).ToList();
                    //foreach (var guardian in guardianList)
                    //{
                    //    var guardianContact = Db.Guardians.Where(x => x.GuardianEmail.Equals(guardian))
                    //                            .Select(y => y.PhoneNumber).FirstOrDefault();

                    SMS sms = new SMS()
                    {
                        SenderId = model.SenderId,
                        Message = model.Message,
                        Numbers = student
                    };
                    try
                    {
                        bool isSuccess = false;
                        string errMsg = null;
                        string response = _smsService.Send(sms); //Send sms

                        string code = _smsService.GetResponseMessage(response, out isSuccess, out errMsg);

                        if (!isSuccess)
                        {
                            ModelState.AddModelError("", errMsg);
                        }
                        else
                        {
                            ViewBag.Message = "Message was successfully sent.";
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }

            }
            return View(model);
        }
    }
}