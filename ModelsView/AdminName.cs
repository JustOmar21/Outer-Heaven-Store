using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebShopping.ModelsView
{
    public class AdminName
    {
        [Required]
        public int ID { get; set; }
        [Display(Name = "Name", ResourceType = typeof(Resources.Resources))]
        [Required]
        [RegularExpression("^[A-Za-z]{3,}([ \'-][A-Za-z]{3,})*$", ErrorMessageResourceName = "NameVali", ErrorMessageResourceType = typeof(Resources.Resources))]
        public string Name { get; set; }
    }
}