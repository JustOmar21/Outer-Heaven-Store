using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebShopping.ModelsView
{
    public class UpdatePhotoDTO
    {
        [Required]
        public int ID { get; set; }

        [Required]
        [Display(Name = "ImageFile", ResourceType = typeof(Resources.Resources))]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}