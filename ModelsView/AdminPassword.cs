using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebShopping.ModelsView
{
    public class AdminPassword
    {
        [Required]
        public int ID { get; set; }
        [Required]
        [Display(Name = "OldPassword", ResourceType = typeof(Resources.Resources))]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Required]
        [Display(Name = "NewPassword", ResourceType = typeof(Resources.Resources))]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessageResourceName = "PasswordVali", ErrorMessageResourceType = typeof(Resources.Resources))]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        [Display(Name = "NewPasswordAgain", ResourceType = typeof(Resources.Resources))]
        [Compare(nameof(NewPassword), ErrorMessageResourceName ="PasswordMatch", ErrorMessageResourceType =typeof(Resources.Resources))]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessageResourceName = "PasswordVali", ErrorMessageResourceType = typeof(Resources.Resources))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}