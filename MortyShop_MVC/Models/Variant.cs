using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MortyShop_MVC.Models
{
    public class Variant
    {
        public int ID { get; set; }
        public string VariantType { get; set; }
        public string VariantValue { get; set; }
        public virtual List<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
    }
}