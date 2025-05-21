using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MortyShop_MVC.Models;

namespace MortyShop_MVC.Controllers
{
    public class CategoryController : Controller
    {
        MortyShopDB db = new MortyShopDB();
        public ActionResult Index(int? categoryID)
        {
            if (categoryID == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Category mainCategory = db.Categories.Find(categoryID);
            if (mainCategory == null || mainCategory.IsDeleted == true)
            {
                return RedirectToAction("Index", "Home");
            }

            List<Category> subCategories = db.Categories.Where(x => x.UpCategoryID == categoryID).ToList();

            List<Product> products = db.Products.Where(x => x.CategoryID == categoryID && x.IsDeleted == false && x.IsActive == true).OrderByDescending(x => x.CreationTime).ToList();

            foreach (Category item in subCategories)
            {
                List<Product> subProducts = db.Products.Where(x => x.CategoryID == item.ID && x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.CreationTime).ToList();
                products.AddRange(subProducts);
            }
            CategoryViewModel viewModel = new CategoryViewModel();
            viewModel.MainCategory = mainCategory;
            viewModel.SubCategories = subCategories;
            viewModel.Products = products;

            return View(viewModel);
        }
    }
}