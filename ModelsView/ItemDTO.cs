using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebShopping.Models;
using System.IO;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace WebShopping.ModelsView
{
    public class ItemDTO
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Name", ResourceType = typeof(Resources.Resources))]
        public string Name { get; set; }
        [Display(Name = "Description", ResourceType = typeof(Resources.Resources))]
        public string Description { get; set; }
        [Required]
        [Display(Name = "ImageFile", ResourceType = typeof(Resources.Resources))]
        public HttpPostedFileBase ImageFile { get; set; }
        [Required]
        [Display(Name = "Amount", ResourceType = typeof(Resources.Resources))]
        [Range(0, long.MaxValue)]
        public long Amount { get; set; }
        [Required]
        [Display(Name = "Price", ResourceType = typeof(Resources.Resources))]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        [Required]
        [Display(Name = "Category", ResourceType = typeof(Resources.Resources))]
        public int CategoryID { get; set; }

    }
}