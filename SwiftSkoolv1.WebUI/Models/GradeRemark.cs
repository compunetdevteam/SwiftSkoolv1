using System.Linq;

namespace SwiftSkoolv1.WebUI.Models
{
    public class GradeRemark
    {
        private readonly SwiftSkoolDbContext _db;

        public GradeRemark()
        {
            _db = new SwiftSkoolDbContext();
        }


        //private string GetschoolClass(string className)
        //{
        //    var myClass = _db.Classes.AsNoTracking().Where(x => x.FullClassName.Equals(className))
        //                        .Select(s => s.SchoolName).FirstOrDefault();
        //    return myClass;

        //}
        //public string GetschoolClassName(string className, string schoolId)
        //{
        //    var myClass = _db.Classes.AsNoTracking().Where(x => x.SchoolId.Equals(schoolId) &&
        //                        x.FullClassName.Equals(className)).Select(s => s.ClassName).FirstOrDefault();
        //    return myClass;

        //}


        // This can be private now
        public string Grading(double summaryTotal, string className, string schoolId)
        {
            //string myclassName = GetschoolClass(className);
            string gradeValue = "";
            int mySummaryTotal = (int)summaryTotal;

            // var myGrade = _db.Grades.AsNoTracking().Where(x => x.ClassName.Equals(myclassName)).ToList();
            var myGrade = _db.Grades.AsNoTracking().Where(x => x.SchoolId.Equals(schoolId) && x.ClassName.Equals(className)).ToList();
            foreach (var item in myGrade)
            {
                if (mySummaryTotal <= item.MaximumValue && mySummaryTotal >= item.MinimumValue)
                {
                    gradeValue = item.GradeName;
                }

            }
            return !string.IsNullOrEmpty(gradeValue) ? gradeValue : "Enter Value between 1 - 100";
            // return gradeValue;

        }


        public string Remark(double summaryTotal, string className, string schoolId)
        {
            //string myclassName = GetschoolClass(className);
            string remarkValue = "";

            int mySummaryTotal = (int)summaryTotal;
            //var myGrade = _db.Grades.AsNoTracking().Where(x => x.ClassName.Equals(myclassName)).ToList();
            var myGrade = _db.Grades.AsNoTracking().Where(x => x.SchoolId.Equals(schoolId) && x.ClassName.Equals(className)).ToList();
            foreach (var item in myGrade)
            {
                if (mySummaryTotal <= item.MaximumValue && mySummaryTotal >= item.MinimumValue)
                {
                    remarkValue = item.Remark;
                }
            }

            return !string.IsNullOrEmpty(remarkValue) ? remarkValue : "Enter Value between 1 - 100";
        }

        //public int GradingPoint(double summaryTotal, string className)
        //{
        //    string myclassName = GetschoolClass(className);
        //    int remarkValue = 0;
        //    int mySummaryTotal = (int)summaryTotal;
        //    //var myGrade = _db.Grades.AsNoTracking().Where(x => x.ClassName.Equals(myclassName)).ToList();
        //    var myGrade = _db.Grades.AsNoTracking().ToList();
        //    foreach (var item in myGrade)
        //    {
        //        if (mySummaryTotal <= item.MaximumValue && mySummaryTotal >= item.MinimumValue)
        //        {
        //            remarkValue = item.GradePoint;
        //        }
        //    }
        //    if (remarkValue == 0)
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        return remarkValue;

        //    }
        //}

        public string PrincipalRemark(double summaryTotal, string className, string schoolId)
        {
            // string myclassName = GetschoolClassName(className, schoolId);
            string remarkValue = "";

            //int mySummaryTotal = (int)summaryTotal;
            var myGrade = _db.PrincipalComments.AsNoTracking().Where(x => x.ClassName.Equals(className)
                            && x.SchoolId.Equals(schoolId)).ToList();
            foreach (var item in myGrade)
            {
                if (summaryTotal <= item.MaximumGrade && summaryTotal >= item.MinimumGrade)
                {
                    remarkValue = item.Remark;
                }
            }

            return !string.IsNullOrEmpty(remarkValue) ? remarkValue : "Enter Value between 0 - 100";

        }
    }
}
