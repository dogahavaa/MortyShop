using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MortyShop_MVC.Models
{
    public class CategoryViewModel
    {
        public Category MainCategory { get; set; }
        public List<Category> SubCategories { get; set; }
        public List<Product> Products { get; set; }
    }
}