using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MortyShop_MVC.Models
{
    public class DetailModel
    {
        public Product Product { get; set; }
        public List<ProductVariant> ProductVariants { get; set; }
    }
}