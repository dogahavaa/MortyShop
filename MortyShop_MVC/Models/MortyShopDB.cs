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

        public DbSet<ManagerRole> ManagerRoles { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
