using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SwiftSkool.Models
{
    public class SessionSubjectTotal : GeneralSchool
    {
        private readonly GradeRemark _myGradeRemark = new GradeRemark();
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private SessionSubjectTotal()
        {

        }

        public SessionSubjectTotal(string studentId, string className, string sessionName, int subjectCode)
        {
            if (!string.IsNullOrEmpty(className) && !string.IsNullOrEmpty(sessionName))
            {
                StudentId = studentId;
                ClassName = className;
                SessionName = sessionName;
                FirstTermScore = Math.Round(GetFirstTermScoreForSubject(studentId, sessionName, subjectCode, className), 2);
                SecondTermScore = Math.Round(GetSecondTermScoreForSubject(studentId, sessionName, subjectCode, className), 2);
                ThirdTermScore = Math.Round(GetThirdTermScoreForSubject(studentId, sessionName, subjectCode, className), 2);

            }
            else
            {
                throw new ArgumentException("Invalid parameter supplied, make sure required parameters are not empty!");
            }
        }
        public int SessionSubjectTotalId { get; set; }
        [StringLength(25)]
        public string StudentId { get; private set; }
        [StringLength(15)]
        public string ClassName { get; private set; }
        //[StringLength(35)]
        public int SubjectId { get; private set; }
        [StringLength(15)]
        public string SessionName { get; private set; }
        public double FirstTermScore { get; private set; }
        public double SecondTermScore { get; private set; }
        public double ThirdTermScore { get; private set; }

        public virtual Subject Subject { get; set; }

        public double SummaryTotal
        {
            get
            {
                double firstTerm = 0.3 * FirstTermScore;
                double secondTerm = 0.3 * SecondTermScore;
                double thirdTerm = 0.4 * ThirdTermScore;
                return Math.Round((firstTerm + secondTerm + thirdTerm), 2);
            }
            private set { }
        }

        public int WeightedScores
        {
            get
            {

                return Convert.ToInt32(FirstTermScore + SecondTermScore + ThirdTermScore);
            }
            private set { }

        }

        public string SummaryGrading
        {
            get
            {
                return _myGradeRemark.Grading(SummaryTotal, ClassName, SchoolId).ToString();
            }
            private set { }
        }


        public string SummaryRemark
        {
            get
            {
                return _myGradeRemark.Remark(SummaryTotal, ClassName, SchoolId).ToString();
            }
            private set { }
        }


        private double GetFirstTermScoreForSubject(string studentId, string sessionName, int subjectCode, string className)
        {
            var firstTermScore = _db.ContinuousAssessments.Where(x => x.StudentId.Equals(studentId)
                                                                     && x.TermName.Equals("First")
                                                                     && x.SessionName.Equals(sessionName)
                                                                     && x.ClassName.Equals(className));
            var newSubjectName = _db.Subjects.Where(x => x.SubjectId.Equals(subjectCode))
                .Select(c => c.SubjectName).FirstOrDefault();

            var myFirstScore = firstTermScore.Where(x => x.SubjectId.Equals(subjectCode))
                .Select(y => y.Total).FirstOrDefault();

            return myFirstScore;
            //}
        }


        private double GetSecondTermScoreForSubject(string studentId, string sessionName, int subjectCode,
            string className)
        {
            var secondTermScore = _db.ContinuousAssessments.Where(x => x.StudentId.Equals(studentId)
                                                                      && x.TermName.Equals("Second")
                                                                      && x.SessionName.Equals(sessionName)
                                                                      && x.ClassName.Equals(className));
            var newSubjectName = _db.Subjects.Where(x => x.SubjectId.Equals(subjectCode))
                .Select(c => c.SubjectName).FirstOrDefault();

            //var mysubjectCategory = db.Subjects.Where(x => x.CourseName.Equals(subjectCode))
            //    .Select(c => c.CategoriesId).FirstOrDefault();

            //if (mysubjectCategory == "Mathematics")
            //{
            //    SubjectName = "Mathematics";
            //}
            //else if (mysubjectCategory == "English")
            //{
            //    SubjectName = "English";
            //}
            //else
            //{
            //    SubjectName = newSubjectName;
            //}
            //var subjectCategory = secondTermScore.Where(x => x.SubjectCategory.Equals(mysubjectCategory));
            //var countSubjectCategory = subjectCategory.Count();
            //double myTotal = 0;
            //if (countSubjectCategory > 1)
            //{
            //    foreach (var item in subjectCategory)
            //    {
            //        myTotal += item.Total;
            //    }
            //    return myTotal / countSubjectCategory;

            //}
            //else
            //{
            var mySecondScore = secondTermScore.Where(x => x.SubjectId.Equals(subjectCode))
                .Select(y => y.Total).FirstOrDefault();

            return mySecondScore;
            // }


        }


        private double GetThirdTermScoreForSubject(string studentId, string sessionName, int subjectCode,
            string className)
        {

            var secondTermScore = _db.ContinuousAssessments.Where(x => x.StudentId.Equals(studentId)
                                                                      && x.TermName.Equals("Third")
                                                                      && x.SessionName.Equals(sessionName)
                                                                      && x.ClassName.Equals(className));

            var newSubjectName = _db.Subjects.Where(x => x.SubjectId.Equals(subjectCode))
              .Select(c => c.SubjectName).FirstOrDefault();

            //var mysubjectCategory = db.Subjects.Where(x => x.CourseName.Equals(subjectCode))
            //    .Select(c => c.CategoriesId).FirstOrDefault();
            //var subjectCategory = secondTermScore.Where(x => x.SubjectCategory.Equals(mysubjectCategory));

            //if (mysubjectCategory == "Mathematics")
            //{
            //    SubjectName = "Mathematics";
            //}
            //else if (mysubjectCategory == "English")
            //{
            //    SubjectName = "English";
            //}
            //else
            //{
            //    SubjectName = newSubjectName;
            //}
            //var countSubjectCategory = subjectCategory.Count();
            //double myTotal = 0;
            //if (countSubjectCategory > 1)
            //{
            //    foreach (var item in subjectCategory)
            //    {
            //        myTotal += item.Total;
            //    }
            //    return myTotal / countSubjectCategory;

            //}
            //else
            //{
            var mySecondScore = secondTermScore.Where(x => x.SubjectId.Equals(subjectCode))
                .Select(y => y.Total).FirstOrDefault();

            return mySecondScore;

            // }

        }

    }
}