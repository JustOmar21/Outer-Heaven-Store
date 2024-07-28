using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebShopping.CustomAttributes;
using WebShopping.Models;

namespace WebShopping.ModelsView
{
    public class AdminRegister
    {

        [Required]
        [Display(Name = "Name", ResourceType = typeof(Resources.Resources))]
        [RegularExpression("^[A-Za-z]{3,}([ \'-][A-Za-z]{3,})*$", ErrorMessageResourceName = "NameVali", ErrorMessageResourceType = typeof(Resources.Resources))]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(Resources.Resources))]
        [UniqueEmail]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resources.Resources))]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessageResourceName = "PasswordVali", ErrorMessageResourceType = typeof(Resources.Resources))]
        public string Password { get; set; }
        [Required]
        [Display(Name = "NewPasswordAgain", ResourceType = typeof(Resources.Resources))]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessageResourceName = "PasswordMatch", ErrorMessageResourceType = typeof(Resources.Resources))]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessageResourceName = "PasswordVali", ErrorMessageResourceType = typeof(Resources.Resources))]
        public string ConfirmPassword { get; set; }
    }
}