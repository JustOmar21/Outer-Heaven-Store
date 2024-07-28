using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebShopping.Models
{
    public enum Gender { Male , Female };
    public class Customer
    {
        public int ID { get; set; }
        [Required]
        [Display(Name="Name",ResourceType =typeof(Resources.Resources))]
        [RegularExpression("^[A-Za-z]{3,}([ \'-][A-Za-z]{3,})*$", ErrorMessageResourceName = "NameVali", ErrorMessageResourceType = typeof(Resources.Resources))]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Gender", ResourceType = typeof(Resources.Resources))]
        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }
        [Required]
        [Display(Name = "Address", ResourceType = typeof(Resources.Resources))]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Phone", ResourceType = typeof(Resources.Resources))]
        [RegularExpression(@"^(\+201|01|00201)[0-2,5]{1}[0-9]{8}$", ErrorMessageResourceName = "PhoneVali", ErrorMessageResourceType = typeof(Resources.Resources))]
        public string Phone { get; set; }
        public virtual List<Invoice> Invoices { get; set; }
    }
}