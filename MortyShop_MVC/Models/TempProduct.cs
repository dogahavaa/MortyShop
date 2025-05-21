using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MortyShop_MVC.Models
{
    public class TempProduct
    {
        public int ID { get; set; }
        public string BarcodeNo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; }
        public int BrandID { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool IsNew { get; set; } // Yeni ürün mü yoksa güncelleme mi
        public bool IsDeleted { get; set; } // Silinecek ürün mü
        public bool IsProcessed { get; set; } // İşlem yapıldı mı
        public virtual Category Category { get; set; }
        public virtual Brand Brand { get; set; }
    }
}