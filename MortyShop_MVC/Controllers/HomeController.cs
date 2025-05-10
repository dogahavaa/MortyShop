using MortyShop_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MortyShop_MVC.Controllers
{
    public class HomeController : Controller
    {
        MortyShopDB db = new MortyShopDB();
        // GET: Home
        public ActionResult Index()
        {
            HomeViewModel viewModel = new HomeViewModel();

            viewModel.NewProducts = db.Products.Where(x => x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.CreationTime).Take(4).ToList();

            
            return View(viewModel);
        }
    }
}