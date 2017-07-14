using Microsoft.AspNet.Identity;
using SwiftSkool.ViewModel;
using SwiftSkoolv1.Domain;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using SwiftSkoolv1.WebUI.ViewModels;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class StaffsController : BaseController
    {
        // GET: Staffs
        public async Task<ActionResult> Index()
        {
            ViewData["ClassName"] = new SelectList(Db.Classes, "FullClassName", "FullClassName");
            return View(await Db.Staffs.AsNoTracking().ToListAsync());
        }

        public async Task<ActionResult> StaffDashboard()
        {

            StaffVM model = new StaffVM();

            var term = await Db.Terms.Where(x => x.ActiveTerm.Equals(true)).Select(x => x.TermName).FirstOrDefaultAsync();
            var session = await Db.Sessions.Where(x => x.ActiveSession.Equals(true)).Select(x => x.SessionName).FirstOrDefaultAsync();
            //get the ID of the logged in Staff


            var staffName = User.Identity.GetUserName();
            var TeacherAssignedClass = await Db.AssignFormTeacherToClasses.AsNoTracking().Where(x => x.Username.Equals(staffName)).Select(x => x.ClassName).FirstOrDefaultAsync();
            // var formTeacherAssignedClass = Db.AssignFormTeacherToClasses.Where(x => x.Username.Equals(staffName));

            var classAssignedToStudent = Db.AssignedClasses.Where(x => x.ClassName.Equals(TeacherAssignedClass)).Select(x => x.StudentId.Count());


            //number of male student in the class of the form Teacher
            var male = Db.AssignedClasses.AsNoTracking().Where(
                                x => x.TermName.Equals(term) && x.SessionName.Equals(session)
                                     && x.ClassName.Equals(TeacherAssignedClass)).Count(x => x.Student.Gender.Equals("MALE"));
            //number of female student in the class of the form Teacher
            var female = Db.AssignedClasses.AsNoTracking().Where(
                               x => x.TermName.Equals(term) && x.SessionName.Equals(session)
                                    && x.ClassName.Equals(TeacherAssignedClass)).Count(x => x.Student.Gender.Equals("FEMALE"));




            // var classIWasAssignedTo = Db.

            //get all the student in the class of a staff
            var studentInMyClass = Db.AssignedClasses.AsNoTracking()
                .Where(x => x.ClassName.Equals(TeacherAssignedClass))
                .Select(x => x.StudentId).ToList();
            var mylist = new List<Student>();

            foreach (var item in studentInMyClass)
            {
                var student = await Db.Students.Where(x => x.StudentId.Equals(item)).FirstOrDefaultAsync();
                mylist.Add(student);
            }

            model.StudentsInMyClass = mylist;
            model.ClassName = TeacherAssignedClass;
            model.MaleStudent = male;
            model.FemaleStudent = female;
            model.TotalStudentInClass = studentInMyClass.Count();


            return View(model);
        }

        public async Task<ActionResult> TeacherDashboard()
        {

            return View();
        }

        // GET: Staffs/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = await Db.Staffs.FindAsync(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            ViewData["ClassName"] = new SelectList(Db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            return View(staff);
        }

        //public async Task<ActionResult> StudentDetails(string id)
        //{
        //  //  var username = User.Identity.GetUserId();
        //    //var user = await _db.Users.AsNoTracking().Where(c => c.Id.Equals(username)).Select(c => c.Email).FirstOrDefaultAsync();
        //    //if (id == null)
        //    //{
        //    //    id = username;
        //    //}

        //    Student student = await Db.Students.FindAsync(id);
        //    if (student == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.Student = student;
        //    return PartialView("_StudentDetailForStaffView");
        //}

        // GET: Staffs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Salutation,FirstName,MiddleName,LastName,PhoneNumber,Email,Gender,Address,StateOfOrigin,Designation,StaffPassport,DateOfBirth,MaritalStatus,Qualifications")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                Db.Staffs.Add(staff);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(staff);
        }

        // GET: Staffs/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = await Db.Staffs.FindAsync(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Salutation,FirstName,MiddleName,LastName,PhoneNumber,Email,Gender,Address,StateOfOrigin,Designation,StaffPassport,DateOfBirth,MaritalStatus,Qualifications")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(staff).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(staff);
        }

        // GET: Staffs/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = await Db.Staffs.FindAsync(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Staff staff = await Db.Staffs.FindAsync(id);
            if (staff != null) Db.Staffs.Remove(staff);
            await Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> RenderImage(string id)
        {
            Staff staff = await Db.Staffs.FindAsync(id);

            byte[] photoBack = staff.StaffPassport;

            return File(photoBack, "image/png");
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
