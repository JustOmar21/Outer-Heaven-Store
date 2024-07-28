using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebShopping.Models;

namespace WebShopping.CustomAttributes
{
    public class UniqueEmail : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            StoreEntity storeEntity = new StoreEntity();
            var Admin = validationContext.ObjectInstance as Admin;
            if (value is string)
            {
                string email = (string)value;
                bool isUnique = storeEntity.Admins.FirstOrDefault(ad => ad.Email == email && ad.ID != Admin.ID) is null;
                return isUnique ? ValidationResult.Success : new ValidationResult($"{Resources.Resources.EmailUnique}");
            }
            return new ValidationResult("The data type of this object is not string");
        }
    }
}