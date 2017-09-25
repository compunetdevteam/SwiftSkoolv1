using SwiftSkoolv1.WebUI.Models;
using System;
using System.Linq;

namespace SwiftSkoolv1.WebUI.BusinessLogic
{

    public class ResultSummaryCmd
    {
        private readonly SwiftSkoolDbContext _db;
        private readonly GradeRemark _myGradeRemark;
        private readonly string _studentId;
        public readonly string _className;
        private readonly string _sessionName;
        private readonly string _schoolId;
        private readonly int _subjectId;

        public ResultSummaryCmd(string studentId, string sessionName, int subjectCode, string schoolId)
        {
            _db = new SwiftSkoolDbContext();
            _myGradeRemark = new GradeRemark();
            if (!string.IsNullOrEmpty(studentId) && !string.IsNullOrEmpty(sessionName)
                && !string.IsNullOrEmpty(schoolId))
            {
                _schoolId = schoolId.ToUpper().Trim();
                _sessionName = sessionName.Trim();
                _studentId = studentId.Trim();
                _className = GetClassName();
                _subjectId = subjectCode;
                ClassName = _className;
                FirstTermScore = GetFirstTermScore();
                FirstTermSubjectGrade = _myGradeRemark.Grading(FirstTermScore, _className, _schoolId).ToString();

                SecondTermScore = GetSecondTermScore();
                SecondTermSubjectGrade = _myGradeRemark.Grading(SecondTermScore, _className, _schoolId).ToString();

                ThirdTermScore = GetThirdTermScore();
                ThirdTermSubjectGrade = _myGradeRemark.Grading(ThirdTermScore, _className, _schoolId).ToString();

                FindSubjectPositionForFirstTerm();
                FindSubjectPositionForSecondTerm();
                FindSubjectPositionForThirdTerm();

                TotalScorePerStudent = FirstTermScore + SecondTermScore + ThirdTermScore;
                NoOfStudentPerClass = NumberOfStudentPerClass();
                NoOfSubjectOffered = SubjectOfferedByStudent();

                ClassAverage = Math.Round((TotalScorePerStudent / SubjectOfferedByStudent()), 2);
                //Average = Math.Round(CalculateAverage(studentId, className, term, sessionName), 2);
                //AggretateScore = Math.Round(SummaryTotalScorePerStudent(studentId, className, term, sessionName), 2);

                // NoOfSubjectOffered = SubjectOfferedByStudent(studentId);
                //FindSubjectPosition(studentId, subject, className, term, sessionName);
                //FindAggregatePosition(studentId, className, term, sessionName);
            }


        }

        public string ClassName { get; private set; }
        public double FirstTermScore { get; private set; }
        public int FirstTermSubjectPosition { get; private set; }
        public string FirstTermSubjectGrade { get; private set; }

        public double SecondTermScore { get; private set; }
        public int SecondTermSubjectPosition { get; private set; }
        public string SecondTermSubjectGrade { get; private set; }

        public double ThirdTermScore { get; private set; }
        public int ThirdTermSubjectPosition { get; private set; }
        public string ThirdTermSubjectGrade { get; private set; }



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
            get { return _myGradeRemark.Grading(ClassAverage, ClassName, _schoolId); }
            private set { }
        }


        public string SummaryRemark
        {
            get
            {
                return _myGradeRemark.Remark(ClassAverage, ClassName, _schoolId);
            }
            private set { }
        }

        public double TotalScorePerStudent { get; private set; }

        public int NoOfSubjectOffered { get; private set; }

        public int NoOfStudentPerClass { get; private set; }
        public double SummaryAverage { get; private set; }

        public double ClassAverage { get; private set; }


        #region Getting score for each Term
        private double GetFirstTermScore()
        {
            return _db.CaLists.AsNoTracking().Where(x => x.SchoolId.Equals(_schoolId) && x.StudentId.Equals(_studentId)
                                                        && x.TermName.ToUpper().Equals("FIRST")
                                                        && x.SessionName.Equals(_sessionName)
                                                        && x.ClassName.Equals(ClassName)
                                                        && x.SubjectId.Equals(_subjectId))
                                                        .Select(y => y.Total).FirstOrDefault();

        }


        private double GetSecondTermScore()
        {
            return _db.CaLists.AsNoTracking().Where(x => x.SchoolId.Equals(_schoolId) && x.StudentId.Equals(_studentId)
                                          && x.TermName.ToUpper().Equals("SECOND")
                                          && x.SessionName.Equals(_sessionName)
                                          && x.ClassName.Equals(_className)
                                          && x.SubjectId.Equals(_subjectId))
                                        .Select(y => y.Total).FirstOrDefault();

        }


        private double GetThirdTermScore()
        {
            return _db.CaLists.AsNoTracking().Where(x => x.SchoolId.Equals(_schoolId) && x.StudentId.Equals(_studentId)
                                          && x.TermName.ToUpper().Equals("THIRD")
                                          && x.SessionName.Equals(_sessionName)
                                          && x.ClassName.Equals(_className)
                                          && x.SubjectId.Equals(_subjectId))
                .Select(y => y.Total).FirstOrDefault();

        }
        #endregion


        private int NumberOfStudentPerClass()
        {
            var studentPerClass = _db.AssignedClasses.AsNoTracking().Count(x => x.SchoolId.Equals(_schoolId) &&
                                                                x.ClassName.Equals(_className) &&
                                                                x.TermName.ToUpper().Equals("THIRD") &&
                                                                x.SessionName.Equals(_sessionName));
            return studentPerClass;
        }

        private int SubjectOfferedByStudent()
        {
            var subjectPerStudent = _db.AssignSubjects.AsNoTracking().Count(x => x.ClassName.Equals(_className));
            return subjectPerStudent;
        }

        private string GetClassName()
        {
            var className = _db.AssignedClasses.AsNoTracking().Where(x => x.SchoolId.Equals(_schoolId)
                                    && x.StudentId.Equals(_studentId) && x.TermName.ToUpper().Equals("THIRD"))
                                    .Select(y => y.ClassName).FirstOrDefault();
            return className;
        }



        #region Subjects positions
        private void FindSubjectPositionForFirstTerm()
        {

            var mySubjectPosition = _db.CaLists.AsNoTracking().Where(x => x.SchoolId.Equals(_schoolId) && x.SubjectId.Equals(_subjectId)
                                                                        && x.ClassName.Equals(_className)
                                                                        && x.SessionName.Equals(_sessionName)
                                                                        && x.TermName.ToUpper().Equals("FIRST")).ToList();
            //.OrderByDescending(y => y.FirstTermScore);
            var q = from s in mySubjectPosition
                    orderby s.Total descending
                    select new
                    {
                        Name = s.StudentId,
                        Rank = (from o in mySubjectPosition
                                where o.Total > s.Total
                                select o).Count() + 1
                    };

            foreach (var item in q.Where(s => s.Name.Equals(_studentId)))
            {
                FirstTermSubjectPosition = item.Rank;
            }

        }

        private void FindSubjectPositionForSecondTerm()
        {
            var mySubjectPosition = _db.CaLists.AsNoTracking().Where(x => x.SchoolId.Equals(_schoolId) && x.SubjectId.Equals(_subjectId)
                                                                          && x.ClassName.Equals(_className)
                                                                          && x.SessionName.Equals(_sessionName)
                                                                          && x.TermName.ToUpper().Equals("SECOND")).ToList();
            //.OrderByDescending(y => y.SecondTermScore);
            var q = from s in mySubjectPosition
                    orderby s.Total descending
                    select new
                    {
                        Name = s.StudentId,
                        Rank = (from o in mySubjectPosition
                                where o.Total > s.Total
                                select o).Count() + 1
                    };

            foreach (var item in q.Where(s => s.Name.Equals(_studentId)))
            {
                SecondTermSubjectPosition = item.Rank;
            }
        }

        private void FindSubjectPositionForThirdTerm()
        {
            var mySubjectPosition = _db.CaLists.AsNoTracking().Where(x => x.SchoolId.Equals(_schoolId) && x.SubjectId.Equals(_subjectId)
                                                                          && x.ClassName.Equals(_className)
                                                                          && x.SessionName.Equals(_sessionName)
                                                                          && x.TermName.ToUpper().Equals("THIRD")).ToList();
            // .OrderByDescending(y => y.ThirdTermScore);
            var q = from s in mySubjectPosition
                    orderby s.Total descending
                    select new
                    {
                        Name = s.StudentId,
                        Rank = (from o in mySubjectPosition
                                where o.Total > s.Total
                                select o).Count() + 1
                    };

            foreach (var item in q.Where(s => s.Name.Equals(_studentId)))
            {
                ThirdTermSubjectPosition = item.Rank;
            }
        }

        #endregion

    }
}