using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace MortyShop_MVC.Models
{
    public partial class MortyShopDB : DbContext
    {
        public MortyShopDB()
            : base("name=MortyShopDB")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
