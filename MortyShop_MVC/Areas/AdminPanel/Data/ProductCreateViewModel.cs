using MortyShop_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MortyShop_MVC.Areas.AdminPanel.Data
{
    public class ProductCreateViewModel
    {
        public Product Product { get; set; }
        public int ProductVariantID { get; set; }
        public int CategoryID { get; set; }
        public int BrandID { get; set; }
        public string VariantType { get; set; }
        public string VariantValue { get; set; }
        public int Stock { get; set; }
    }
}