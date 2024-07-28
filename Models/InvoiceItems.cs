using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebShopping.Models
{
    public class InvoiceItems
    {
        [ForeignKey(nameof(Invoice))]
        [Required]
        [Display(Name = "ID", ResourceType = typeof(Resources.Resources))]
        public int InvoiceID { get; set; }

        [ForeignKey(nameof(Item))]
        [Required]
        [Display(Name = "Item", ResourceType = typeof(Resources.Resources))]
        public int? ItemID { get; set; }

        [Required]
        [Display(Name = "Discount", ResourceType = typeof(Resources.Resources))]
        [Range(0, 100)]
        public int Discount { get; set; }
        [Required]
        [Display(Name = "Amount", ResourceType = typeof(Resources.Resources))]
        [Range(1, 10,ErrorMessageResourceName ="AmountVali", ErrorMessageResourceType =typeof(Resources.Resources))]
        public int Amount { get; set; }

        public virtual Invoice Invoice { get; set; }
        public virtual Item Item { get; set; }
    }
}