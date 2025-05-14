using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MortyShop_MVC.Models
{
    public class Cart
    {
        public int ID { get; set; }

        public int ProductVariantID { get; set; }
        [ForeignKey("ProductVariantID")]
        public virtual ProductVariant ProductVariant { get; set; }

        public int MemberID { get; set; }

        [ForeignKey("MemberID")]

        public virtual Member Member { get; set; }

        public int Quantity { get; set; }
    }
}