using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebShopping.Models
{
    public class Item
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Name", ResourceType = typeof(Resources.Resources))]
        public string Name { get; set; }
        [Display(Name = "Description", ResourceType = typeof(Resources.Resources))]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Image", ResourceType = typeof(Resources.Resources))]
        public string ImageURL { get; set; }
        [Required]
        [Display(Name = "Amount", ResourceType = typeof(Resources.Resources))]
        [Range(0, long.MaxValue)]
        public long Amount { get; set; }
        [Required]
        [Display(Name = "Price", ResourceType = typeof(Resources.Resources))]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        [Required]
        [ForeignKey(nameof(Category))]
        [Display(Name = "Category", ResourceType = typeof(Resources.Resources))]
        public int CategoryID { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<InvoiceItems> InvoicesItems { get; set; }

    }
}