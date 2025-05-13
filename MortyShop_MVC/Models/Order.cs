using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MortyShop_MVC.Models
{
    public class Order
    {
        public int ID { get; set; }
        public int MemberID { get; set; }
        [ForeignKey("MemberID")]
        public virtual Member Member { get; set; }

        [Required(ErrorMessage = "Sipariş numarası zorunludur.")]
        [StringLength(20, ErrorMessage = "Sipariş numarası en fazla 20 karakter olabilir.")]
        public string OrderNumber { get; set; }

        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Şehir zorunludur.")]
        [StringLength(50, ErrorMessage = "Şehir en fazla 50 karakter olabilir.")]
        public string City { get; set; }

        public string Status { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}