using System.Collections.Generic;
using SwiftSkoolv1.Domain;

namespace SwiftSkoolv1.WebUI.ViewModels
{
    public class SchoolFeeReciept
    {
        public Student Student { get; set; }
        public string FeeCategory { get; set; }

        public FeePayment SchoolFeePayment { get; set; }

        public List<FeeType> SchoolFeeTypes { get; set; }
    }
}