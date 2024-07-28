using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebShopping.Models;
using WebShopping.Repos.Implementation;
using WebShopping.Repos.Interface;

namespace WebShopping.CustomAttributes
{
    public class UniqueCategory: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            StoreEntity context = new StoreEntity();
            var modelData = validationContext.ObjectInstance as Category;
            if (value is string)
            {
                string name = (string)value;
                bool isUnique = context.Categories.FirstOrDefault(cat=> cat.Name == name && cat.ID != modelData.ID) is null;
                return isUnique ? ValidationResult.Success : new ValidationResult($"{Resources.Resources.CatUnique}");
            }
            return new ValidationResult("The data type of this object is not string");
        }
    }
}