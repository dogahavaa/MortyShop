using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MortyShop_MVC.Models;
using MortyShop_MVC.Areas.AdminPanel.Data;

namespace MortyShop_MVC.Areas.AdminPanel.Controllers
{
    public class ProductController : Controller
    {
        MortyShopDB db = new MortyShopDB();
        public ActionResult Index()
        {
            return View(db.Products.Where(x => x.IsDeleted == false));
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories.Where(x => x.IsActive == true && x.IsDeleted == false), "ID", "Name");
            ViewBag.BrandID = new SelectList(db.Brands.Where(x => x.IsActive == true && x.IsDeleted == false), "ID", "Name");
            VariantTypeF();
            return View();
        }

        [HttpPost]
        public ActionResult Create(ProductCreateViewModel model, HttpPostedFileBase image, string VariantTypes)
        {
            if (ModelState.IsValid)
            {
                bool isvalidimage = true;
                model.Product.CreationTime = DateTime.Now;
                model.Product.CategoryID = model.CategoryID;
                model.Product.BrandID = model.BrandID;
                model.VariantType = VariantTypes;

                if (image != null)
                {
                    FileInfo fi = new FileInfo(image.FileName);
                    string extension = fi.Extension;
                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                    {
                        string generateName = Guid.NewGuid().ToString() + extension;
                        model.Product.Image = generateName;
                        //kontrol et
                        image.SaveAs(Server.MapPath("~/Assets/Images/ProductImages/" + generateName));
                    }
                    else
                    {
                        isvalidimage = false;
                        ViewBag.message = "Eklenebilir dosya uzantısı sadece .jpg, .jpeg veya .png olabilir.";
                    }
                }
                else
                {
                    model.Product.Image = "noImage.png";
                }

                if (isvalidimage)
                {
                    db.Products.Add(model.Product);
                    db.SaveChanges();

                    if (!string.IsNullOrEmpty(model.VariantValue) && !string.IsNullOrEmpty(model.VariantType) && model.Stock >= 0)
                    {
                        Variant variant = new Variant();
                        variant.VariantValue = model.VariantValue;
                        variant.VariantType = model.VariantType;
                        db.Variants.Add(variant);
                        db.SaveChanges();

                        ProductVariant pv = new ProductVariant();
                        pv.VariantID = variant.ID;
                        pv.ProductID = model.Product.ID;
                        pv.Product = model.Product;
                        pv.Variant = variant;
                        pv.Stock = model.Stock;
                        db.ProductVariants.Add(pv);
                        db.SaveChanges();
                    }
                    TempData["message"] = "Ürün başarıyla eklendi";
                    return RedirectToAction("Index", "Product");
                }
            }
            ViewBag.CategoryID = new SelectList(db.Categories.Where(x => x.IsActive == true && x.IsDeleted == false), "ID", "Name");
            ViewBag.BrandID = new SelectList(db.Brands.Where(x => x.IsActive == true && x.IsDeleted == false), "ID", "Name");
            VariantTypeF();
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            Product p = db.Products.Find(id);
            if (p == null)
            {
                TempData["message"] = "Ürün bulunamadı.";
                return RedirectToAction("Index", "Product");
            }

            List<Category> categories = db.Categories.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
            List<SelectListItem> CategoryID = new List<SelectListItem>();
            foreach (Category category in categories)
            {
                CategoryID.Add(
                    new SelectListItem
                    {
                        Value = category.ID.ToString(),
                        Text = category.Name,
                        Selected = (category.ID == p.CategoryID)
                    });
            }

            List<Brand> brands = db.Brands.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
            List<SelectListItem> BrandID = new List<SelectListItem>();

            foreach (Brand brand in brands)
            {
                BrandID.Add(
                    new SelectListItem
                    {
                        Value = brand.ID.ToString(),
                        Text = brand.Name,
                        Selected = (brand.ID == p.BrandID)
                    });
            }
            ViewBag.CategoryID = CategoryID;
            ViewBag.BrandID = BrandID;
            return View(p);
        }

        [HttpPost]
        public ActionResult Edit(Product p, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                bool isvalidimage = true;

                if (image != null)
                {
                    FileInfo fi = new FileInfo(image.FileName);
                    string extension = fi.Extension;
                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                    {
                        string generateName = Guid.NewGuid().ToString() + extension;
                        p.Image = generateName;
                        image.SaveAs(Server.MapPath("~/Assets/Images/ProductImages/" + generateName));
                    }
                    else
                    {
                        isvalidimage = false;
                        ViewBag.message = "Eklenebilir dosya uzantısı sadece .jpg, .jpeg veya .png olabilir.";
                    }
                }

                if (isvalidimage)
                {
                    db.Entry(p).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    TempData["message"] = "Ürün düzenlemesi başarılı";
                    return RedirectToAction("Index", "Product");
                }
            }
            ViewBag.CategoryID = new SelectList(db.Categories.Where(x => x.IsActive == true && x.IsDeleted == false), "ID", "Name");
            ViewBag.BrandID = new SelectList(db.Brands.Where(x => x.IsActive == true && x.IsDeleted == false), "ID", "Name");
            return View(p);
        }

        public ActionResult Change(int? id)
        {
            if (id != null)
            {
                Product p = db.Products.Find(id);
                if (p != null)
                {
                    p.IsActive = !p.IsActive;
                    db.SaveChanges();
                    TempData["message"] = "Durum değiştirildi.";
                }
            }
            return RedirectToAction("Index", "Product");
        }

        public ActionResult Destroy(int? id)
        {
            if (id != null)
            {
                Product p = db.Products.Find(id);
                if (p != null)
                {
                    db.Products.Remove(p);
                    db.SaveChanges();
                    TempData["message"] = "Ürün 'tamamen' silindi.";
                }
            }
            return RedirectToAction("Index", "Product");
        }

        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                Product p = db.Products.Find(id);
                if (p != null)
                {
                    p.IsDeleted = true;
                    p.IsActive = false;
                    db.SaveChanges();
                    TempData["message"] = "Ürün başarıyla silindi.";
                }
            }
            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        public ActionResult AddVariant(int? id)
        {
            if (id != null)
            {
                Product p = db.Products.Find(id);
                if (p == null)
                {
                    TempData["message"] = "Ürün bulunamadı";
                    return RedirectToAction("Index", "Product");
                }

                ProductCreateViewModel model = new ProductCreateViewModel();
                model.Product = p;
                VariantTypeF();
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Product");
            }
        }

        [HttpPost]
        public ActionResult AddVariant(ProductCreateViewModel model, string VariantTypes)
        {

            // Ürünü bul
            Product product = db.Products.Find(model.Product.ID);
            if (product == null)
            {
                TempData["message"] = "Ürün bulunamadı.";
                return RedirectToAction("Index", "Product");
            }

            model.VariantType = VariantTypes;

            // Varyasyon ve stok bilgisi ekle
            if (!string.IsNullOrEmpty(model.VariantValue) && !string.IsNullOrEmpty(model.VariantType) && model.Stock >= 0)
            {

                Variant variant = new Variant
                {
                    VariantType = model.VariantType,
                    VariantValue = model.VariantValue
                };
                db.Variants.Add(variant);
                db.SaveChanges();

                var productVariant = new ProductVariant
                {
                    VariantID = variant.ID,
                    ProductID = product.ID,
                    Stock = model.Stock
                };
                db.ProductVariants.Add(productVariant);
                db.SaveChanges();

                TempData["message"] = "Varyasyon başarıyla eklendi.";
                return RedirectToAction("Index", "Product");
            }
            else
            {
                TempData["message"] = "Lütfen varyasyon bilgilerini eksiksiz doldurun.";
            }
            VariantTypeF();
            return View(model);
        }

        public ActionResult Variants(int? id)
        {
            Product p = db.Products.Find(id);
            if (p == null)
            {
                TempData["message"] = "Ürün bulunamadı.";
                return RedirectToAction("Index", "Product");
            }

            List<ProductVariant> productVariants = db.ProductVariants.Where(x => x.ProductID == p.ID).ToList();

            List<ProductCreateViewModel> products = new List<ProductCreateViewModel>();

            foreach (ProductVariant pv in productVariants)
            {
                ProductCreateViewModel pcvm = new ProductCreateViewModel();
                pcvm.Product = p;
                pcvm.VariantType = pv.Variant.VariantType;
                pcvm.VariantValue = pv.Variant.VariantValue;
                pcvm.Stock = pv.Stock;
                products.Add(pcvm);
            }
            return View(products);
        }

        //public ActionResult VariantEdit()
        //{

        //}


        private void VariantTypeF()
        {
            List<SelectListItem> VariantTypes = new List<SelectListItem>();
            VariantTypes.Add(new SelectListItem
            {
                Value = "1",
                Text = "Beden"
            }
                );
            VariantTypes.Add(new SelectListItem
            {
                Value = "2",
                Text = "Numara"
            }
                );
            ViewBag.VariantTypes = VariantTypes;
        }

    }
}