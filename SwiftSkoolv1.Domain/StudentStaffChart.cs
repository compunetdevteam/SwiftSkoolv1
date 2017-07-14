using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HopeAcademySMS.Models
{
    public class StudentStaffChart
    {
        public int StudentStaffChartId { get; set; }
        public int TotalNumberOfMaleStudent { get; set; }
        public int TotalNumberOfFemaleStudent { get; set; }
        public int TotalNumberOfStaff { get; set; }
    }
}