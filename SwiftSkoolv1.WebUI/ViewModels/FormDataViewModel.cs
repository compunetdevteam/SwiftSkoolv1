using SwiftSkool.Models;
using System.Collections.Generic;

namespace SwiftSkool.ViewModel
{
    public class FormDataViewModel
    {
        //public int Id { get; set; }
        //[Display(Name = "Student ID")]
        //[Required(ErrorMessage = "Your Student ID Number is required")]
        //[StringLength(15, ErrorMessage = "Your Student ID is too long")]
        //public string StudentId { get; set; }

        //[Display(Name = "Term")]
        //[Required(ErrorMessage = "Term is required")]
        //public PopUp.Term TermName { get; set; }

        //[Display(Name = "Session")]
        //[Required(ErrorMessage = "Session is required")]
        //public string SessionName { get; set; }

        //[Display(Name = "Class Name")]
        //[Required(ErrorMessage = "Class Name is required")]
        //public string ClassName { get; set; }

        //[Display(Name = "Honesty")]
        //public PopUp.Extra Honesty { get; set; }

        //[Display(Name = "Self Confidence")]
        //public PopUp.Extra SelfConfidence { get; set; }

        //[Display(Name = "Sociability")]
        //public PopUp.Extra Sociability { get; set; }

        //[Display(Name = "Punctuality")]
        //[Range(1, 3)]
        //public PopUp.Extra Punctuality { get; set; }

        //[Display(Name = "Neatness")]
        //public PopUp.Extra Neatness { get; set; }

        //[Display(Name = "Initiative")]
        //public PopUp.Extra Initiative { get; set; }

        //[Display(Name = "Organization")]
        //public PopUp.Extra Organization { get; set; }

        //[Display(Name = "Attentiveness In Class")]
        //public PopUp.Extra AttentivenessInClass { get; set; }

        //[Display(Name = "Honesty And Reliability")]
        //public PopUp.Extra HonestyAndReliability { get; set; }
        public AffectiveViewModel AffectiveViewModel { get; set; }
        public IEnumerable<AssignedClass> AssignedClasses { get; set; }
    }
}