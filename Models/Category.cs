using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebShopping.CustomAttributes;

namespace WebShopping.Models
{
    public class Category
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Name", ResourceType = typeof(Resources.Resources))]
        [RegularExpression("^[A-Za-z]{3,}([ \'-][A-Za-z]{3,})*$", ErrorMessageResourceName = "NameVali", ErrorMessageResourceType = typeof(Resources.Resources))]
        [UniqueCategory]
        public string Name { get; set; }
        public virtual List<Item> Items { get; set; }
    }
}