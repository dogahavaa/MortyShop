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

        //public ActionResult _DetailVariants(int? id)
        //{

        //}

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

            DetailModel model = new DetailModel();
            List<ProductVariant> ProductVariants = db.ProductVariants.Where(x => x.ProductID == p.ID).ToList();
            List<SelectListItem> variants = new List<SelectListItem>();

            foreach (ProductVariant item in ProductVariants)
            {
                variants.Add(
                    new SelectListItem 
                    {
                        Value = item.Variant.ID.ToString(), 
                        Text = item.Variant.VariantValue }
                    );
            }

            ViewBag.Variants = variants;
            model.ProductVariants = ProductVariants;
            model.Product = p;
            return View(model);
        }

        public ActionResult _ProductsByCategory(int? id, int? count = null)
        {
            if (id == null)
            {
                return null;
            }
            var query = db.Products.Where(x => x.CategoryID == id && x.IsDeleted == false && x.IsActive);

            if (count != null)
            {
                query = query.Take(count.Value);
            }

            List<Product> products = query.ToList();
            return PartialView(products);
        }

        
    }
}