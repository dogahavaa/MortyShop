using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MortyShop_MVC.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Barkod numarası zorunludur.")]
        [StringLength(20, ErrorMessage = "Barkod numarası en fazla 20 karakter olabilir.")]
        public string BarcodeNo { get; set; }

        [Required(ErrorMessage = "Ürün adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Ürün adı en fazla 100 karakter olabilir.")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal Price { get; set; }
        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }
        public int BrandID { get; set; }
        [ForeignKey("BrandID")]
        public virtual Brand Brand { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreationTime { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public bool MonthlyPick { get; set; } = false;
        public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();

        /*
        1- çok satanlar
        2- en fazla görüntülenenler
        3- yeni ürünler
        4- daha önce bakmış olduğunuz ürünler
         */

    }
}