using MortyShop_MVC.Areas.AdminPanel.Filters;
using MortyShop_MVC.Areas.AdminPanel.Services;
using MortyShop_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MortyShop_MVC.Areas.AdminPanel.Controllers
{
    [ManagerLoginRequiredFilter]
    public class HomeController : Controller
    {
        MortyShopDB db = new MortyShopDB();
        XMLProductService xmlService = new XMLProductService();
        // GET: AdminPanel/Home
        public ActionResult Index()
        {
            List<TempProduct> tempProducts = db.TempProducts.Where(x => x.IsProcessed == false).ToList();

            return View(tempProducts);
        }

        public ActionResult ApproveProduct(int id)
        {
            xmlService.ApproveProduct(id);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult RejectProduct(int id)
        {
            xmlService.RejectProduct(id);
            return RedirectToAction("Index", "Home");
        }
    }
}