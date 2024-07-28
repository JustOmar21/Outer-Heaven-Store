using Backend.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebShopping.Models
{
    public class Invoice
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Date", ResourceType = typeof(Resources.Resources))]
        [PastDatesOnly]
        public DateTime Date { get; set; }
        [Required]
        [Display(Name = "Discount", ResourceType = typeof(Resources.Resources))]
        [Range(0, 100)]
        public int Discount { get; set; }
        [ForeignKey(nameof(Customer))]
        [Required]
        [Display(Name = "Customer", ResourceType = typeof(Resources.Resources))]
        public int? CustomerID { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<InvoiceItems> Items { get; set; }

    }
}