using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Enfo.Repository.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NonNegativeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((decimal) value >= decimal.Zero)
            {
                return ValidationResult.Success;
            }

            if (validationContext is null)
            {
                return new ValidationResult("Value must not be negative.");
            }

            return new ValidationResult(
                $"{validationContext.DisplayName} must not be negative.",
                new List<string>() {validationContext.MemberName});
        }
    }
}