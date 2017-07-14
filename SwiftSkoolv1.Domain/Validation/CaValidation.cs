using System.ComponentModel.DataAnnotations;

namespace SwiftSkool.Models.Validation
{
    public class CaValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var caListVm = (CaListVm) validationContext.ObjectInstance;

            foreach (var caSetUp in caListVm.CaSetUp)
            {
                if (caListVm.CaSetUp.Count == 1)
                {
                    if (caListVm.ExamCa > caSetUp.MaximumScore)
                    {
                        return new ValidationResult($"Score cannot be greater than  {caSetUp.MaximumScore}");
                    }
                   
                }
            }
             return ValidationResult.Success;
        }
    }
}