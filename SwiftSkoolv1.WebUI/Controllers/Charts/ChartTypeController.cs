using HopeAcademySMS.ViewModel;
    using Newtonsoft.Json;
 
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
using HopeAcademySMS.Models.Charts;

namespace SwiftSkool.Controllers.Charts
{
    public class ChartTypesController : BaseController
    {

        public PartialViewResult ResultInfo(string studentNumber)
        {
            var resultInfoes = Db.ContinuousAssessments.Where(s => s.StudentId.Contains(studentNumber));
            return PartialView(resultInfoes);
        }
        public PartialViewResult Column()
        {
            //Below code can be used to include dynamic data in Chart. Check view page and uncomment the line "dataPoints: @Html.Raw(ViewBag.DataPoints)"
            //  ViewBag.DataPoints = JsonConvert.SerializeObject(DataService.GetRandomDataForCategoryAxis(10), _jsonSetting);

            return PartialView();
        }

        public PartialViewResult Line()
        {
            //Below code can be used to include dynamic data in Chart. Check view page and uncomment the line "dataPoints: @Html.Raw(ViewBag.DataPoints)"
            //ViewBag.DataPoints = JsonConvert.SerializeObject(DataService.GetRandomDataForNumericAxis(1000), _jsonSetting);

            return PartialView();
        }

        public PartialViewResult Bar()
        {
            //Below code can be used to include dynamic data in Chart. Check view page and uncomment the line "dataPoints: @Html.Raw(ViewBag.DataPoints)"
            //ViewBag.DataPoints = JsonConvert.SerializeObject(DataService.GetRandomDataForCategoryAxis(4), _jsonSetting);

            return PartialView();
        }

        public PartialViewResult Area(List<DataPoint> model)
        {
            //var dataPoints = new List<DataPoint>();
            //foreach (var item in model)
            //{
            //    var data = new DataPoint(item.X, item.Y);
            //    dataPoints.Add(data);
            //}
            //int value = 5;
            //for (int i = 0; i <= 5; i++)
            //{
            //    var data = new DataPoint(5, i);
            //    dataPoints.Add(data);
            //}
            //Below code can be used to include dynamic data in Chart. Check view page and uncomment the line "dataPoints: @Html.Raw(ViewBag.DataPoints)"
            ViewBag.DataPoints = JsonConvert.SerializeObject(model, _jsonSetting);

            return PartialView();
        }
        //public ActionResult Area()
        //{
        //    //Below code can be used to include dynamic data in Chart. Check view page and uncomment the line "dataPoints: @Html.Raw(ViewBag.DataPoints)"
        //    //ViewBag.DataPoints = JsonConvert.SerializeObject(DataService.GetRandomDataForCategoryAxis(10), _jsonSetting);

        //    return View();
        //}

        public PartialViewResult Pie(List<DataPoint> model, string title)
        {
            ViewBag.ChartTitle = title;
            //Below code can be used to include dynamic data in Chart. Check view page and uncomment the line "dataPoints: @Html.Raw(ViewBag.DataPoints)"
            ViewBag.DataPoints = JsonConvert.SerializeObject(model, _jsonSetting);

            return PartialView();
        }

        public PartialViewResult Doughnut()
        {
            //Below code can be used to include dynamic data in Chart. Check view page and uncomment the line "dataPoints: @Html.Raw(ViewBag.DataPoints)"
            //ViewBag.DataPoints = JsonConvert.SerializeObject(DataService.GetRandomDataForCategoryAxis(5), _jsonSetting);

            return PartialView();
        }

        public PartialViewResult Spline()
        {
            //Below code can be used to include dynamic data in Chart. Check view page and uncomment the line "dataPoints: @Html.Raw(ViewBag.DataPoints)"
            //ViewBag.DataPoints = JsonConvert.SerializeObject(DataService.GetRandomDataForCategoryAxis(9), _jsonSetting);

            return PartialView();
        }

        public PartialViewResult StepLine()
        {
            return PartialView();
        }

        public PartialViewResult SplineArea()
        {
            //Below code can be used to include dynamic data in Chart. Check view page and uncomment the line "dataPoints: @Html.Raw(ViewBag.DataPoints)"
            //ViewBag.DataPoints = JsonConvert.SerializeObject(DataService.GetRandomDataForNumericAxis(12), _jsonSetting);

            return PartialView();
        }

        public PartialViewResult Scatter()
        {
            //Below code can be used to include dynamic data in Chart. Check view page and uncomment the line "dataPoints: @Html.Raw(ViewBag.DataPoints)"
            //ViewBag.DataPoints = JsonConvert.SerializeObject(DataService.GetRandomDataForNumericAxis(50), _jsonSetting);

            return PartialView();
        }

        public PartialViewResult Bubble()
        {
            return PartialView();
        }

        public PartialViewResult StackedColumn()
        {
            //Below code can be used to include dynamic data in Chart. Check view page and uncomment the line "dataPoints: @Html.Raw(ViewBag.DataPoints)"
            //ViewBag.DataPoints1 = JsonConvert.SerializeObject(DataService.GetRandomDataForCategoryAxis(5), _jsonSetting);
            //ViewBag.DataPoints2 = JsonConvert.SerializeObject(DataService.GetRandomDataForCategoryAxis(5), _jsonSetting);

            return PartialView();
        }

        public PartialViewResult StackedBar()
        {
            //Below code can be used to include dynamic data in Chart. Check view page and uncomment the line "dataPoints: @Html.Raw(ViewBag.DataPoints)"
            //ViewBag.DataPoints1 = JsonConvert.SerializeObject(DataService.GetRandomDataForCategoryAxis(9), _jsonSetting);
            //ViewBag.DataPoints2 = JsonConvert.SerializeObject(DataService.GetRandomDataForCategoryAxis(9), _jsonSetting);
            //ViewBag.DataPoints3 = JsonConvert.SerializeObject(DataService.GetRandomDataForCategoryAxis(9), _jsonSetting);
            //ViewBag.DataPoints4 = JsonConvert.SerializeObject(DataService.GetRandomDataForCategoryAxis(9), _jsonSetting);

            return PartialView();
        }

        public PartialViewResult StackedArea()
        {
            //Below code can be used to include dynamic data in Chart. Check view page and uncomment the line "dataPoints: @Html.Raw(ViewBag.DataPoints)"
            //ViewBag.DataPoints = JsonConvert.SerializeObject(DataService.GetRandomDataForDateTimeAxis(7), _jsonSetting);

            return PartialView();
        }

        public PartialViewResult StackedColumn100()
        {
            //Below code can be used to include dynamic data in Chart. Check view page and uncomment the line "dataPoints: @Html.Raw(ViewBag.DataPoints)"
            //ViewBag.DataPoints1 = JsonConvert.SerializeObject(DataService.GetRandomDataForCategoryAxis(5), _jsonSetting);
            //ViewBag.DataPoints2 = JsonConvert.SerializeObject(DataService.GetRandomDataForCategoryAxis(5), _jsonSetting);

            return PartialView();
        }

        public PartialViewResult StackedBar100()
        {
            //Below code can be used to include dynamic data in Chart. Check view page and uncomment the line "dataPoints: @Html.Raw(ViewBag.DataPoints)"
            //ViewBag.DataPoints1 = JsonConvert.SerializeObject(DataService.GetRandomDataForCategoryAxis(9), _jsonSetting);
            //ViewBag.DataPoints2 = JsonConvert.SerializeObject(DataService.GetRandomDataForCategoryAxis(9), _jsonSetting);
            //ViewBag.DataPoints3 = JsonConvert.SerializeObject(DataService.GetRandomDataForCategoryAxis(9), _jsonSetting);
            //ViewBag.DataPoints4 = JsonConvert.SerializeObject(DataService.GetRandomDataForCategoryAxis(9), _jsonSetting);
            //ViewBag.DataPoints5 = JsonConvert.SerializeObject(DataService.GetRandomDataForCategoryAxis(9), _jsonSetting);
            //ViewBag.DataPoints6 = JsonConvert.SerializeObject(DataService.GetRandomDataForCategoryAxis(9), _jsonSetting);
            //ViewBag.DataPoints7 = JsonConvert.SerializeObject(DataService.GetRandomDataForCategoryAxis(9), _jsonSetting);
            //ViewBag.DataPoints8 = JsonConvert.SerializeObject(DataService.GetRandomDataForCategoryAxis(9), _jsonSetting);

            return PartialView();
        }

        public PartialViewResult StackedArea100()
        {
            //Below code can be used to include dynamic data in Chart. Check view page and uncomment the line "dataPoints: @Html.Raw(ViewBag.DataPoints)"
            //ViewBag.DataPoints = JsonConvert.SerializeObject(DataService.GetRandomDataForNumericAxis(7), _jsonSetting);

            return PartialView();
        }

        public PartialViewResult StepArea()
        {
            //Below code can be used to include dynamic data in Chart. Check view page and uncomment the line "dataPoints: @Html.Raw(ViewBag.DataPoints)"
            //ViewBag.DataPoints = JsonConvert.SerializeObject(DataService.GetRandomDataForCategoryAxis(25), _jsonSetting);

            return PartialView();
        }

        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };

    }
}