namespace MortyShop_MVC.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using MortyShop_MVC.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<MortyShop_MVC.Models.MortyShopDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(MortyShop_MVC.Models.MortyShopDB context)
        {
            //context.ManagerRoles.AddOrUpdate(x => x.ID, new ManagerRole() { ID = 1, Role = "Admin" });

            //context.Managers.AddOrUpdate(x => x.ID, new Manager() { ID = 1, Name = "Doğa", Surname = "Hava", Email = "dogahava@gmail.com", Username = "dogahava", Password = "1234", RoleID = 1, IsActive = true });
        }
    }
}
