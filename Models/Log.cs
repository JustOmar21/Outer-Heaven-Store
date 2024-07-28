using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebShopping.Models
{
    public class Log
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Action", ResourceType = typeof(Resources.Resources))]
        public string Action { get; set; }
        [Required]
        [Display(Name = "Date", ResourceType = typeof(Resources.Resources))]
        public DateTime Created { get; set; }
        [ForeignKey("Admin")]
        [Display(Name = "Admin", ResourceType = typeof(Resources.Resources))]
        public int? AdminID { get; set; }
        

        public virtual Admin Admin { get; set; }
    }
}