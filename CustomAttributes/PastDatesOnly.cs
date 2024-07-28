using System;
using System.ComponentModel.DataAnnotations;
using WebShopping.Resources;

namespace Backend.CustomValidation
{
    public class PastDatesOnly : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value is DateTime)
            {
                DateTime date = (DateTime)value;
                DateTime dateNow = DateTime.Now;
                if (dateNow.CompareTo((DateTime)value) <= 0)
                {
                    return new ValidationResult($"{Resources.DateValiPast} {(dateNow.ToString("MMMM dd yyyy hh:mm tt"))}");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                return new ValidationResult($"The value sent is not of DateType 'DateTime'");
            }
        }
    }
}
