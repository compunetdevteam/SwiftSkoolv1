using System.Collections.Generic;
using System.Web.Mvc;
using SwiftSkoolv1.Domain;


namespace SwiftSkoolv1.WebUI.ViewModels
{
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
}