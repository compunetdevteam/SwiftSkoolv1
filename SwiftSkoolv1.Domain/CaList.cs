using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SwiftSkool.Models
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

    public class CaListIndexVm
    {
        public List<CaList> CaList { get; set; }
        public List<CaSetUp> CaSetUp { get; set; }

    }

    public class CaSelectIndexVm
    {
        public string ClassName { get; set; }
        public string TermName { get; set; }
        public string SessionName { get; set; }
        public int SubjectId { get; set; }

    }
    public class CaListVm
    {
        public int CaListId { get; set; }
        public string StudentName { get; set; }
        public string StudentId { get; set; }
        public string TermName { get; set; }
        public string SessionName { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string ClassName { get; set; }
        [Remote("FirstCaValidation", "CaLists", ErrorMessage = "Score is greater than Expected Value", AdditionalFields = "ClassName,TermName,CaSetUpCount")]
        public double FirstCa { get; set; }

        [Remote("SecondCaValidation", "CaLists", ErrorMessage = "Score is greater than Expected Value", AdditionalFields = "ClassName,TermName,CaSetUpCount")]
        public double SecondCa { get; set; }
        [Remote("ThirdCaValidation", "CaLists", ErrorMessage = "Score is greater than Expected Value", AdditionalFields = "ClassName,TermName,CaSetUpCount")]
        public double ThirdCa { get; set; }

        [Remote("ForthCaValidation", "CaLists", ErrorMessage = "Score is greater than Expected Value", AdditionalFields = "ClassName,TermName,CaSetUpCount")]
        public double ForthCa { get; set; }
        [Remote("FifthCaValidation", "CaLists", ErrorMessage = "Score is greater than Expected Value", AdditionalFields = "ClassName,TermName,CaSetUpCount")]
        public double FifthCa { get; set; }
        [Remote("SixthCaValidation", "CaLists", ErrorMessage = "Score is greater than Expected Value", AdditionalFields = "ClassName,TermName,CaSetUpCount")]
        public double SixthCa { get; set; }
        [Remote("SeventhCaValidation", "CaLists", ErrorMessage = "Score is greater than Expected Value", AdditionalFields = "ClassName,TermName,CaSetUpCount")]
        public double SeventhCa { get; set; }
        [Remote("EightCaValidation", "CaLists", ErrorMessage = "Score is greater than Expected Value", AdditionalFields = "ClassName,TermName,CaSetUpCount")]
        public double EightCa { get; set; }
        [Remote("NinthtCaValidation", "CaLists", ErrorMessage = "Score is greater than Expected Value", AdditionalFields = "ClassName,TermName,CaSetUpCount")]
        public double NinthtCa { get; set; }

        [Remote("ExamCaValidation", "CaLists", ErrorMessage = "Score is greater than Expected Value", AdditionalFields = "ClassName,TermName,CaSetUpCount")]
        public double ExamCa { get; set; }
        //public double Total { get; set; }
        //public string Remark { get; set; }
        public List<CaSetUp> CaSetUp { get; set; }
        public int CaSetUpCount { get; set; }


    }
}