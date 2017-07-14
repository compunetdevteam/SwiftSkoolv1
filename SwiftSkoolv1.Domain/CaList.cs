using SwiftSkool.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SwiftSkoolv1.Domain
{
    public class CaList : GeneralSchool
    {
        private readonly GradeRemark _myGradeRemark;

        public CaList()
        {
            _myGradeRemark = new GradeRemark();
        }

        public int CaListId { get; set; }

        public int SubjectId { get; set; }

        [StringLength(45)]
        public string StudentName { get; set; }

        [StringLength(35)]
        public string StudentId { get; set; }
        [StringLength(15)]
        public string TermName { get; set; }
        [StringLength(25)]
        public string SessionName { get; set; }

        [StringLength(25)]
        public string ClassName { get; set; }
        public double FirstCa { get; set; }
        public double SecondCa { get; set; }
        public double ThirdCa { get; set; }
        public double ForthCa { get; set; }
        public double FifthCa { get; set; }
        public double SixthCa { get; set; }
        public double SeventhCa { get; set; }
        public double EightCa { get; set; }
        public double NinthtCa { get; set; }
        public double ExamCa { get; set; }
        public double Total
        {
            get
            {
                double sum = FirstCa + SecondCa + ThirdCa + ForthCa + FifthCa + SixthCa + SeventhCa + EightCa + NinthtCa + ExamCa;
                return sum;
            }
            private set { }
        }
        public string Grading
        {

            get
            {
                return _myGradeRemark.Grading(Total, ClassName, SchoolId);
            }
            private set { }

        }

        public string Remark
        {
            #region Checking grade

            get
            {
                return _myGradeRemark.Remark(Total, ClassName, SchoolId);
            }
            private set { }

            #endregion
        }

        public string StaffName { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual Student Student { get; set; }
    }

    
   
}