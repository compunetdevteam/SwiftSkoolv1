using SwiftSkool.Services;
using SwiftSkoolv1.WebUI.Controllers;
using SwiftSkoolv1.WebUI.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;


namespace SwiftSkool.Controllers
{

    public class HomeController : BaseController
    {

        public async Task<ActionResult> Index()
        {
            ViewBag.PictureList = await Db.HomePageSetUps.AsNoTracking().CountAsync();
            return View(await Db.HomePageSetUps.ToListAsync());
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }
        public async Task<ActionResult> GeneralDashboard()
        {
            //get the total number of school that SwiftSkool support
            var numberOfSchools = await Db.Schools.AsNoTracking().CountAsync();

            //total students
            var students = await Db.Students.AsNoTracking().ToListAsync();

            //get the total number of student that SwiftSkool supports
            var numberOfStudent = students.Count();

            //total number of female student in the software
            var numberOfFemale = students.Count(x => x.Gender.ToLower().Equals("female") || x.Gender.ToLower().Equals("f"));

            //total number of male student in the application
            var numberOfMale = students.Count(x => x.Gender.ToLower().Equals("male") || x.Gender.ToLower().Equals("m"));


            var model = new GeneralDashboardVm();
            model.TotalNumberOfSchools = numberOfSchools;
            model.TotlaNumberOfStudents = numberOfStudent;
            model.MaleStudent = numberOfMale;
            model.FemaleStudent = numberOfFemale;

            //return new Rotativa.ViewAsPdf();
            return View(model);
        }

        public async Task<ActionResult> AdminDashboard()
        {
            int totalMaleStudent = await Db.Students.AsNoTracking().CountAsync(s => s.Gender.Equals("Male") && s.SchoolId.Equals(userSchool));
            int totalFemaleStudent = await Db.Students.AsNoTracking().CountAsync(s => s.Gender.Equals("Female") && s.SchoolId.Equals(userSchool));
            int active = await Db.Students.AsNoTracking().CountAsync(s => s.Active.Equals(true) && s.SchoolId.Equals(userSchool));
            int graduatedStudent = await Db.Students.AsNoTracking().CountAsync(s => s.IsGraduated.Equals(true) && s.SchoolId.Equals(userSchool));
            int totalStudent = await Db.Students.AsNoTracking().CountAsync(s => s.SchoolId.Equals(userSchool));
            int totalStaff = await Db.Staffs.AsNoTracking().CountAsync(s => s.SchoolId.Equals(userSchool));

            double val1 = totalMaleStudent * 100;
            double val2 = totalFemaleStudent * 100;

            double boysPercentage = Math.Round(val1 / totalStudent, 2);
            double femalePercentage = Math.Round(val2 / totalStudent, 2);

            var list = new List<DataPoint>
            {
                new DataPoint(boysPercentage, "Male","Male"),
                new DataPoint(femalePercentage, "Female","Female")

            };


            ViewBag.PiePoints = list;

            var dashboardVm = new AdminDashboardVm
            {
                MaleStudents = totalMaleStudent,
                FemaleStudents = totalFemaleStudent,
                MalePercentage = boysPercentage,
                FemalePercentage = femalePercentage,
                TotalStudents = totalStudent,
                TotalNumberOfStaff = totalStaff,
                DataPoint = list
            };

            return View(dashboardVm);
        }
        /*
         method to build chart on admin dashboard
             */


        /* 

         Get the total student in the class and display in the admin view

         */

        //public async Task<ActionResult> GetIndex()
        //{
        //    #region Server Side filtering
        //    //Get parameter for sorting from grid table
        //    // get Start (paging start index) and length (page size for paging)
        //    var draw = Request.Form.GetValues("draw").FirstOrDefault();
        //    var start = Request.Form.GetValues("start").FirstOrDefault();
        //    var length = Request.Form.GetValues("length").FirstOrDefault();
        //    //Get Sort columns values when we click on Header Name of column
        //    //getting column name
        //    var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
        //    //Soring direction(either desending or ascending)
        //    var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
        //    string search = Request.Form.GetValues("search[value]").FirstOrDefault();

        //    int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //    int skip = start != null ? Convert.ToInt32(start) : 0;
        //    int totalRecords = 0;

        //    //var v = Db.Subjects.Where(x => x.SchoolId != userSchool).Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToList();
        //    var v = Db.Students.Where(x => x.SchoolId == userSchool && x.Gender == "Male").Select(s => new { s.StudentId, s.FullName, s.Gender }).ToList();

        //    //var v = Db.Subjects.Where(x => x.SchoolId.Equals(userSchool)).Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToList();
        //    //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
        //    //{
        //    //    //v = v.OrderBy(sortColumn + " " + sortColumnDir);
        //    //    v = new List<Subject>(v.OrderBy(x => "sortColumn + \" \" + sortColumnDir"));
        //    //}
        //    if (!string.IsNullOrEmpty(search))
        //    {
        //        //v = v.OrderBy(sortColumn + " " + sortColumnDir);
        //        v = Db.Students.Where(x => x.SchoolId.Equals(userSchool) && (x.StudentId.Equals(search) || x.FullName.Equals(search)))
        //            .Select(s => new { s.StudentId, s.FullName, s.Gender }).ToList();
        //    }
        //    totalRecords = v.Count();
        //    var data = v.Skip(skip).Take(pageSize).ToList();

        //    return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
        //    #endregion

        //    //return Json(new { data = await Db.Subjects.AsNoTracking().Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToListAsync() }, JsonRequestBehavior.AllowGet);
        //}


        public ActionResult CharterColumn()
        {


            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();

            var totalNumberOfStudent = Db.Students.AsNoTracking();
            var totalMaleStudent = Db.Students.AsNoTracking().CountAsync(s => s.Gender.Equals("Male"));
            var totalFemaleStudent = Db.Students.AsNoTracking().CountAsync(s => s.Gender.Equals("Female"));
            var totalStaff = Db.Staffs.AsNoTracking().CountAsync();
            //var results = ;
            //totalNumberOfStudent.ToList().ForEach(rs => xValue.Add(rs.Gender);
            //totalNumberOfStudent.ToList().ForEach(rs => yValue.Add(rs.GrowthValue));
            xValue.Add("Male Student");
            xValue.Add("Female Student");
            xValue.Add("Staff");

            yValue.Add(totalMaleStudent);
            yValue.Add(totalFemaleStudent);
            yValue.Add(totalStaff);

            new Chart(width: 400, height: 200, theme: ChartTheme.Green)
                .AddTitle("Chart for Growth [Column Chart]")
                .AddSeries("Default", chartType: "Column", xValue: xValue, yValues: yValue).Write("bmp");
            return null;
        }

        //public ActionResult SchoolSetUp()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult SchoolSetUp(SetUpVm model)
        //{
        //    string _FileName = String.Empty;
        //    if (model.File?.ContentLength > 0)
        //    {
        //        _FileName = Path.GetFileName(model.File.FileName);
        //        string _path = HostingEnvironment.MapPath("~/Content/Images/") + _FileName;
        //        var directory = new DirectoryInfo(HostingEnvironment.MapPath("~/Content/Images/"));
        //        if (directory.Exists == false)
        //        {
        //            directory.Create();
        //        }
        //        model.File.SaveAs(_path);
        //    }


        //    //ViewBag.Message = "File upload failed!!";
        //    //return View(model);

        //    Configuration objConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
        //    AppSettingsSection objAppsettings = (AppSettingsSection)objConfig.GetSection("appSettings");
        //    //Edit
        //    if (objAppsettings != null)
        //    {
        //        objAppsettings.Settings["SchoolName"].Value = model.SchoolName;
        //        objAppsettings.Settings["SchoolTheme"].Value = model.SchoolTheme.ToString();
        //        if (!String.IsNullOrEmpty(_FileName))
        //        {
        //            objAppsettings.Settings["SchoolImage"].Value = _FileName;
        //        }
        //        objConfig.Save();
        //    }
        //    return View("Index");
        //}

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}