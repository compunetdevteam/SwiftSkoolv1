using System;
using System.Linq;

namespace SwiftSkool.Models
{
    public class ReportSummary : GeneralSchool
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private readonly GradeRemark _myGradeRemark = new GradeRemark();
        private ReportSummary()
        {

        }

        public ReportSummary(string studentId, string className, string sessionName, int subjectCode)
        {
            if (!string.IsNullOrEmpty(studentId) && !string.IsNullOrEmpty(className)
                        && !string.IsNullOrEmpty(sessionName))
            {
                StudentId = studentId;
                ClassName = className;
                SessionName = sessionName;
                SubjectName = subjectCode;
                FirstTermScore = GetFirstTermScore(studentId, sessionName, subjectCode, className);


                FirstTermSubjectGrade = _myGradeRemark.Grading(FirstTermScore, className, SchoolId).ToString();

                SecondTermScore = GetSecondTermScore(studentId, sessionName, subjectCode, className);
                SecondTermSubjectGrade = _myGradeRemark.Grading(SecondTermScore, className, SchoolId).ToString();

                ThirdTermScore = GetThirdTermScore(studentId, sessionName, subjectCode, className);
                ThirdTermSubjectGrade = _myGradeRemark.Grading(ThirdTermScore, className, SchoolId).ToString();

                FindSubjectPositionForFirstTerm(studentId, subjectCode, className, sessionName);
                FindSubjectPositionForSecondTerm(studentId, subjectCode, className, sessionName);
                FindSubjectPositionForThirdTerm(studentId, subjectCode, className, sessionName);

                TotalScorePerStudent = SummaryTotalScorePerStudent(studentId, className, sessionName);
                NoOfStudentPerClass = NumberOfStudentPerClass(className, sessionName);
                NoOfSubjectOffered = SubjectOfferedByStudent(studentId);

                FirstTermGpa = Math.Round(GetFirstTermGpa(studentId, sessionName, subjectCode, className), 2);
                SecondTermGpa = Math.Round(GetSecondTermGpa(studentId, sessionName, subjectCode, className), 2);
                ThirdTermGpa = Math.Round(GetThirdTermGpa(studentId, sessionName, subjectCode, className), 2);


                ClassAverage = Math.Round(CalculateAverage(studentId, className, sessionName), 2);
                //Average = Math.Round(CalculateAverage(studentId, className, term, sessionName), 2);
                //AggretateScore = Math.Round(SummaryTotalScorePerStudent(studentId, className, term, sessionName), 2);

                // NoOfSubjectOffered = SubjectOfferedByStudent(studentId);
                //FindSubjectPosition(studentId, subject, className, term, sessionName);
                //FindAggregatePosition(studentId, className, term, sessionName);
            }
            else
            {

            }

        }

        public void UpdateResultSummary(string studentId, string className, string sessionName, int subjectCode)
        {
            if (!string.IsNullOrEmpty(studentId) && !string.IsNullOrEmpty(className)
                        && !string.IsNullOrEmpty(sessionName))
            {
                StudentId = studentId;
                ClassName = className;
                SubjectName = subjectCode;
                SessionName = sessionName;

                FirstTermScore = GetFirstTermScore(studentId, sessionName, subjectCode, className);

                FirstTermSubjectGrade = _myGradeRemark.Grading(FirstTermScore, ClassName, SchoolId).ToString();

                SecondTermScore = GetSecondTermScore(studentId, sessionName, subjectCode, className);
                SecondTermSubjectGrade = _myGradeRemark.Grading(SecondTermScore, ClassName, SchoolId).ToString();

                ThirdTermScore = GetThirdTermScore(studentId, sessionName, subjectCode, className);
                ThirdTermSubjectGrade = _myGradeRemark.Grading(ThirdTermScore, ClassName, SchoolId).ToString();

                FindSubjectPositionForFirstTerm(studentId, subjectCode, className, sessionName);
                FindSubjectPositionForSecondTerm(studentId, subjectCode, className, sessionName);
                FindSubjectPositionForThirdTerm(studentId, subjectCode, className, sessionName);
            }
            else
            {

            }

        }

        private double CalculateAverage(string studentId, string className, string sessionName)
        {
            double scorePerstudent = SummaryTotalScorePerStudent(studentId, className, sessionName);
            int subjectOffered = SubjectOfferedByStudent(studentId);
            return scorePerstudent / subjectOffered;
        }

        //private double CalculateClassAverage(string className, string term, string sessionName, string subject)
        //{
        //    var scorePerSubject = SummaryTotalScorePerSubject(subject, className, term, sessionName);
        //    var studentInClass = NumberOfStudentPerClass(className, term, sessionName, subject);
        //    return scorePerSubject / studentInClass;
        //}

        public int ReportSummaryId { get; set; }

        public string StudentId { get; private set; }
        public int SubjectName { get; private set; }
        public string ClassName { get; private set; }
        public string SessionName { get; private set; }

        public double FirstTermScore { get; private set; }
        public int FirstTermSubjectPosition { get; private set; }
        public string FirstTermSubjectGrade { get; private set; }

        public double SecondTermScore { get; private set; }
        public int SecondTermSubjectPosition { get; private set; }
        public string SecondTermSubjectGrade { get; private set; }

        public double ThirdTermScore { get; private set; }
        public int ThirdTermSubjectPosition { get; private set; }
        public string ThirdTermSubjectGrade { get; private set; }

        public double FirstTermGpa { get; private set; }
        public double SecondTermGpa { get; private set; }
        public double ThirdTermGpa { get; private set; }

        public double Cgpa
        {
            get
            {
                return Math.Round((FirstTermGpa + SecondTermGpa + ThirdTermGpa) / 3, 2);
            }
            private set { }
        }

        //public double SummaryTotal
        //{
        //    get
        //    {
        //        double firstTerm = 0.3 * FirstTermScore;
        //        double secondTerm = 0.3 * SecondTermScore;
        //        double thirdTerm = 0.4 * ThirdTermScore;
        //        return firstTerm + secondTerm + thirdTerm;
        //    }
        //    private set { }
        //}

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
                return _myGradeRemark.Grading(ClassAverage, ClassName, SchoolId).ToString();
            }
            private set { }
        }


        public string SummaryRemark
        {
            get
            {
                return _myGradeRemark.Remark(ClassAverage, ClassName, SchoolId).ToString();
            }
            private set { }
        }

        public double TotalScorePerStudent { get; private set; }

        public int SummaryPosition { get; private set; }

        public int NoOfSubjectOffered { get; private set; }

        public int NoOfStudentPerClass { get; private set; }
        public double SummaryAverage { get; private set; }

        public double ClassAverage { get; private set; }


        #region Getting score for each Term
        private double GetFirstTermScore(string studentId, string sessionName, int subjectCode, string className)
        {
            return _db.ContinuousAssessments.Where(x => x.StudentId.Equals(studentId)
                                                        && x.TermName.ToUpper().Equals("FIRST")
                                                        && x.SessionName.Equals(sessionName)
                                                        && x.ClassName.Equals(className)
                                                        && x.SubjectId.Equals(subjectCode))
                                                        .Select(y => y.Total).FirstOrDefault();

        }


        private double GetSecondTermScore(string studentId, string sessionName, int subjectCode,
            string className)
        {
            return _db.ContinuousAssessments.Where(x => x.StudentId.Equals(studentId)
                                                       && x.TermName.ToUpper().Equals("SECOND")
                                                       && x.SessionName.Equals(sessionName)
                                                       && x.ClassName.Equals(className)
                                                       && x.SubjectId.Equals(subjectCode))
                                                       .Select(y => y.Total).FirstOrDefault();

        }


        private double GetThirdTermScore(string studentId, string sessionName, int subjectCode,
            string className)
        {

            return _db.ContinuousAssessments.Where(x => x.StudentId.Equals(studentId)
                                                        && x.TermName.ToUpper().Equals("THIRD")
                                                        && x.SessionName.Equals(sessionName)
                                                        && x.ClassName.Equals(className)
                                                        && x.SubjectId.Equals(subjectCode))
                                                        .Select(y => y.Total).FirstOrDefault();

        }
        #endregion


        private double SummaryTotalScorePerStudent(string studentId, string className, string session)
        {
            var summaryTotalSum = _db.SessionSubjectTotals.Where(x => x.StudentId.Equals(studentId) && x.ClassName.Equals(className)
                                                               && x.SessionName.Equals(session))
                                                               .Sum(y => y.WeightedScores);
            return summaryTotalSum;
        }

        private int NumberOfStudentPerClass(string className, string session)
        {
            var studentPerClass = _db.AssignedClasses.Count(x => x.ClassName.Equals(className) &&
                                                                x.TermName.Equals("Third") &&
                                                                x.SessionName.Equals(session));
            return studentPerClass;
        }

        private int SubjectOfferedByStudent(string studentId)
        {
            var className = _db.AssignedClasses.Where(x => x.StudentId.Equals(studentId) && x.TermName.Equals("Third"))
                                .Select(y => y.ClassName)
                                .FirstOrDefault();


            var subjectPerStudent = _db.AssignSubjects.Count(x => x.ClassName.Equals(className));
            return subjectPerStudent;
        }

        #region CommentedCode


        //private int SubjectOfferedByStudent(string studentId)
        //{
        //    var className =
        //        db.AssignedClasses.Where(x => x.StudentId.Equals(studentId))
        //            .Select(y => y.ClassName)
        //            .FirstOrDefault();


        //    var subjectPerStudent = db.AssignSubjects.Count(x => x.ClassName.Equals(className));
        //    return subjectPerStudent;
        //}

        //private double SummaryTotalScorePerSubject(string subject, string className, string term, string session)
        //{
        //    var sumPerSubject = db.ContinuousAssessments.Where(x => x.SubjectCode.Equals(subject)
        //                                                            && x.ClassName.Equals(className)
        //                                                            && x.TermName.Equals(term) &&
        //                                                            x.SessionName.Equals(session)).Sum(y => y.SummaryTotal);
        //    return sumPerSubject;
        //}


        //private int NumberOfStudentPerClass(string className, string term, string session, string subject)
        //{
        //    var studentPerClass = db.ContinuousAssessments.Count(x => x.ClassName.Equals(className) &&
        //                                                        x.TermName.Equals(term) &&
        //                                                        x.SessionName.Equals(session) &&
        //                                                        x.SubjectCode.Equals(subject));
        //    return studentPerClass;
        //}
        //private int NumberOfStudentPerClass(string className, string term, string session)
        //{
        //    var studentPerClass = db.AssignedClasses.Count(x => x.ClassName.Equals(className) &&
        //                                                        x.TermName.Equals(term) &&
        //                                                        x.SessionName.Equals(session));
        //    return studentPerClass;
        //}

        //public void FindAggregatePosition(string className, string term, string session)
        //{
        //    var myAggregatePosition = db.Results.Where(x => x.ClassName.Equals(className) &&
        //                                                             x.Term.Equals(term) &&
        //                                                             x.SessionName.Equals(session))
        //                                                        .OrderByDescending(y => y.AggretateScore);
        //} 
        #endregion

        #region Subjects positions
        private void FindSubjectPositionForFirstTerm(string studentId, int subject, string className, string session)
        {

            var mySubjectPosition = _db.SessionSubjectTotals.Where(x => x.SubjectId.Equals(subject) &&
                                                                            x.ClassName.Equals(className) &&
                                                                            x.SessionName.Equals(session));
            //.OrderByDescending(y => y.FirstTermScore);
            var q = from s in mySubjectPosition
                    orderby s.FirstTermScore descending
                    select new
                    {
                        Name = s.StudentId,
                        Rank = (from o in mySubjectPosition
                                where o.FirstTermScore > s.FirstTermScore
                                select o).Count() + 1
                    };

            foreach (var item in q.Where(s => s.Name.Equals(studentId)))
            {
                FirstTermSubjectPosition = item.Rank;
            }

        }

        private void FindSubjectPositionForSecondTerm(string studentId, int subject, string className, string session)
        {
            var mySubjectPosition = _db.SessionSubjectTotals.Where(x => x.SubjectId.Equals(subject) &&
                                                                        x.ClassName.Equals(className) &&
                                                                        x.SessionName.Equals(session));
            //.OrderByDescending(y => y.SecondTermScore);
            var q = from s in mySubjectPosition
                    orderby s.SecondTermScore descending
                    select new
                    {
                        Name = s.StudentId,
                        Rank = (from o in mySubjectPosition
                                where o.SecondTermScore > s.SecondTermScore
                                select o).Count() + 1
                    };

            foreach (var item in q.Where(s => s.Name.Equals(studentId)))
            {
                SecondTermSubjectPosition = item.Rank;
            }
        }

        private void FindSubjectPositionForThirdTerm(string studentId, int subject, string className, string session)
        {
            var mySubjectPosition = _db.SessionSubjectTotals.Where(x => x.SubjectId.Equals(subject) &&
                                                                        x.ClassName.Equals(className) &&
                                                                        x.SessionName.Equals(session));
            // .OrderByDescending(y => y.ThirdTermScore);
            var q = from s in mySubjectPosition
                    orderby s.ThirdTermScore descending
                    select new
                    {
                        Name = s.StudentId,
                        Rank = (from o in mySubjectPosition
                                where o.ThirdTermScore > s.ThirdTermScore
                                select o).Count() + 1
                    };

            foreach (var item in q.Where(s => s.Name.Equals(studentId)))
            {
                ThirdTermSubjectPosition = item.Rank;
            }
        }

        #endregion
        private void FindAggregatePosition(string studentId, string className, string term, string session)
        {
            var resultPosition = _db.ReportSummarys.Where(x => x.StudentId.Equals(studentId)
                                                                && x.ClassName.Equals(className)
                                                                && x.SessionName.Equals(session));
            //.OrderByDescending(y => y.WeightedScores);
            var q = from s in resultPosition
                    orderby s.WeightedScores descending
                    select new
                    {
                        Name = s.StudentId,
                        Rank = (from o in resultPosition
                                where o.WeightedScores > s.WeightedScores
                                select o).Count() + 1
                    };

            foreach (var item in q.Where(s => s.Name.Equals(studentId)))
            {
                ThirdTermSubjectPosition = item.Rank;
            }
        }

        //public async Task<ActionResult> SubjectPostion(string CourseName, string level, string term, string session)
        //{
        //    var grades = db.Grades.Include(g => g.Course).Include(g => g.Student)
        //                    .Where(s => s.CourseCode.Equals(CourseName) && s.Student.MyClassName.Equals(level)
        //                        && s.TermName.Equals(term) && s.SessionName.Equals(session))
        //                    .OrderByDescending(s => s.SummaryTotal);
        //    if (grades == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    int myposition = 0;
        //    double? mySummaryTotal = 0;
        //    foreach (var item in grades)
        //    {
        //        Grade grade = db.Grades.Find(item.GradeID);
        //        {
        //            if (mySummaryTotal == item.SummaryTotal)
        //            {
        //                grade.SubjectPosition = myposition;
        //            }
        //            else
        //            {
        //                grade.SubjectPosition = ++myposition;
        //            }
        //            mySummaryTotal = item.SummaryTotal;
        //        };
        //        db.Entry(grade).State = EntityState.Modified;

        //        //db.SubjectPositions.Add(grade);
        //    }
        //    await db.SaveChangesAsync();
        //    ViewBag.CourseName = new SelectList(db.Courses, "CourseCode", "CourseName");
        //    return View(await grades.ToListAsync());
        //    //db.Entry(grade).State = EntityState.Modified;
        //    //await db.SaveChangesAsync();
        //}



        private double GetFirstTermGpa(string studentId, string sessionName, int subjectCode, string className)
        {
            return _db.Results.Where(x => x.StudentId.Equals(studentId)
                                                         && x.Term.ToUpper().Equals("FIRST")
                                                         && x.SessionName.Equals(sessionName)
                                                         && x.ClassName.Equals(className))
                                                    .Select(c => c.GPA).FirstOrDefault();

        }
        private double GetSecondTermGpa(string studentId, string sessionName, int subjectCode, string className)
        {
            return _db.Results.Where(x => x.StudentId.Equals(studentId)
                                                         && x.Term.ToUpper().Equals("SECOND")
                                                         && x.SessionName.Equals(sessionName)
                                                         && x.ClassName.Equals(className))
                                                    .Select(c => c.GPA).FirstOrDefault();

        }

        private double GetThirdTermGpa(string studentId, string sessionName, int subjectCode, string className)
        {
            return _db.Results.Where(x => x.StudentId.Equals(studentId)
                                                         && x.Term.ToUpper().Equals("THIRD")
                                                         && x.SessionName.Equals(sessionName)
                                                         && x.ClassName.Equals(className))
                                                    .Select(c => c.GPA).FirstOrDefault();

        }


    }
}