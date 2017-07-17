using SwiftSkool.Services;
using SwiftSkoolv1.WebUI.Controllers;
using SwiftSkoolv1.WebUI.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;


namespace SwiftSkool.Controllers
{

    public class HomeController : BaseController
    {
        ///private readonly SwiftSkoolDbContext Db = new SwiftSkoolDbContext();

        //public ActionResult Index()
        //{
        //    int totalMaleStudent = Db.Students.Count(s => s.Gender.Equals("Male"));
        //    int totalFemaleStudent = Db.Students.Count(s => s.Gender.Equals("Female"));
        //    int totalStudent = Db.Students.Count();
        //    int totalStaff = Db.Staffs.Count();

        //    double val1 = totalMaleStudent * 100;
        //    double val2 = totalFemaleStudent * 100;

        //    double boysPercentage = Math.Round(val1 / totalStudent, 2);
        //    double femalePercentage = Math.Round(val2 / totalStudent, 2);

        //    ViewBag.MaleStudent = totalMaleStudent;
        //    ViewBag.Femalestudent = totalFemaleStudent;
        //    ViewBag.TotalStudent = totalStudent;
        //    ViewBag.TotalStaff = totalStaff;
        //    ViewBag.BoysPercentage = boysPercentage;
        //    ViewBag.FemalePercentage = femalePercentage;
        //    return View();
        //}
        public async Task<ActionResult> Index()
        {
            ViewBag.PictureList = await Db.HomePageSetUps.AsNoTracking().CountAsync();
            return View(await Db.HomePageSetUps.ToListAsync());
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            //return new Rotativa.ViewAsPdf();
            return View();
        }
        public ActionResult GeneralDashboard()
        {
            ViewBag.Message = "Your application description page.";

            //return new Rotativa.ViewAsPdf();
            return View();
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



            StudentDashboardVm dashboardVm = new StudentDashboardVm();
            dashboardVm.MaleStudents = totalMaleStudent;
            dashboardVm.FemaleStudents = totalFemaleStudent;
            dashboardVm.TotalStudents = totalStudent;
            dashboardVm.TotalNumberOfStaff = totalStaff;
            ViewBag.BoysPercentage = boysPercentage;
            ViewBag.FemalePercentage = femalePercentage;
            dashboardVm.ActiveStudent = active;
            dashboardVm.GraduatedStudent = graduatedStudent;
            return View();
        }
        /*
         method to build chart on admin dashboard
             */

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

            yValue.Add(totalNumberOfStudent);
            yValue.Add(totalMaleStudent);
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