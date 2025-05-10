using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MortyShop_MVC.Models;

namespace MortyShop_MVC.Controllers
{
    public class ProductController : Controller
    {
        MortyShopDB db = new MortyShopDB();
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Product p = db.Products.Find(id);
            if (p == null || p.IsDeleted == true || p.IsActive == false)
            {
                return RedirectToAction("NoProduct", "Product");
            }
            return View(p);
        }
    }
}