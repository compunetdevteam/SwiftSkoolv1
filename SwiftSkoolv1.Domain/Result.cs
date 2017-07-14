using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SwiftSkool.Models
{
    public class Result : GeneralSchool
    {
        #region contructors and methods
        // private ApplicationDbContext db = new ApplicationDbContext();

        //private Result()
        //{

        //}

        //public Result(string studentId, string className, string term, string sessionName, string subject)
        //{
        //    if (!string.IsNullOrEmpty(studentId) && !string.IsNullOrEmpty(className) && !string.IsNullOrEmpty(term)
        //        && !string.IsNullOrEmpty(sessionName) && !string.IsNullOrEmpty(subject))
        //    {
        //        StudentId = studentId;
        //        ClassName = className;
        //        Term = term;
        //        SessionName = sessionName;
        //        SubjectName = subject;
        //        ClassAverage = Math.Round(CalculateClassAverage(className, term, sessionName, subject), 2);
        //        Average = Math.Round(CalculateAverage(studentId, className, term, sessionName), 2);
        //        AggretateScore = Math.Round(TotalScorePerStudent(studentId, className, term, sessionName), 2);
        //        NoOfStudentPerClass = NumberOfStudentPerClass(className, term, sessionName);
        //        //var TotalScorePerSubject = this.TotalScorePerSubject(SubjectName, className, term, sessionName);
        //        NoOfSubjectOffered = SubjectOfferedByStudent(studentId, term);
        //        FindSubjectPosition(studentId, subject, className, term, sessionName);
        //        FindAggregatePosition(studentId, className, term, sessionName);
        //    }
        //    else
        //    {
        //        throw new ArgumentException("Invalid parameter supplied, make sure required parameters are not empty!");
        //    }

        //}

        //public void Updateresult(string studentId, string className, string term, string sessionName, string subject)
        //{
        //    if (!string.IsNullOrEmpty(studentId) && !string.IsNullOrEmpty(className) && !string.IsNullOrEmpty(term)
        //        && !string.IsNullOrEmpty(sessionName) && !string.IsNullOrEmpty(subject))
        //    {
        //        StudentId = studentId;
        //        ClassName = className;
        //        Term = term;
        //        SessionName = sessionName;
        //        SubjectName = subject;
        //        ClassAverage = Math.Round(CalculateClassAverage(className, term, sessionName, subject), 2);
        //        Average = Math.Round(CalculateAverage(studentId, className, term, sessionName), 2);
        //        AggretateScore = Math.Round(TotalScorePerStudent(studentId, className, term, sessionName), 2);
        //        NoOfStudentPerClass = NumberOfStudentPerClass(className, term, sessionName);
        //        NoOfSubjectOffered = SubjectOfferedByStudent(studentId, term);
        //        FindSubjectPosition(studentId, subject, className, term, sessionName);
        //        FindAggregatePosition(studentId, className, term, sessionName);

        //    }
        //    else
        //    {
        //        throw new ArgumentException("Invalid parameter supplied, make sure required parameters are not empty!");
        //    }
        //} 
        #endregion

        public Result()
        {

        }
        public int ResultId { get; set; }
        [StringLength(25)]
        public string StudentId { get; set; }
        [StringLength(25)]
        public string ClassName { get; set; }
        [StringLength(25)]
        public string Term { get; set; }
        [StringLength(15)]
        public string SessionName { get; set; }
       // [StringLength(35)]
        public int SubjectName { get; set; }

        public double SubjectHighest { get; set; }
        public double SubjectLowest { get; set; }
        public int SubjectPosition { get; set; }
        public double AggretateScore { get; set; }

        public double Average { get; set; }

        public double ClassAverage { get; set; }

        public double TotalQualityPoint { get; set; }

        public double TotalCreditUnit { get; set; }

        public double GradePointAverage { get; set; }

        public double GPA
        {
            get
            {
                return Math.Round((TotalQualityPoint / TotalCreditUnit), 2);
            }
            private set { }


        }
        public virtual ICollection<Student> Students { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual ICollection<AssignedClass> AssignedClasses { get; set; }
        public virtual ICollection<AssignSubject> AssignSubject { get; set; }
        public virtual ICollection<ContinuousAssessment> ContinuousAssessments { get; set; }

    }
}