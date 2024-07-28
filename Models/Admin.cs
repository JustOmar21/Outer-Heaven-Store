using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebShopping.CustomAttributes;

namespace WebShopping.Models
{
    public class Admin
    {
        public int ID { get; set; }

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
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, and one digit.")]
        public string Password { get; set; }

        [Required]
        public bool IsBoss { get; set; }
        public string LoginID { get; set; }

        public virtual List<Log> Logs { get; set; }
        
    }
}