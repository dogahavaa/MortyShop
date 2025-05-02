using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MortyShop_MVC.Models
{
    public class ManagerRole
    {
        public int ID { get; set; }
        public string Role { get; set; }
        public virtual ICollection<Manager> Managers { get; set; }
    }
}