using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MortyShop_MVC.Models
{
    public class HomeViewModel
    {
        public List<Product> NewProducts { get; set; }
        public List<Product> BestSellers { get; set; }
        public List<Product> MonthlyPicks { get; set; }
    }
}