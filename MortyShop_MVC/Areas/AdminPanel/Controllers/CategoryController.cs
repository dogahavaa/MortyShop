using MortyShop_MVC.Areas.AdminPanel.Filters;
using MortyShop_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MortyShop_MVC.Areas.AdminPanel.Controllers
{
    [ManagerLoginRequiredFilter]
    public class CategoryController : Controller
    {
        MortyShopDB db = new MortyShopDB();
        // GET: AdminPanel/Category
        public ActionResult Index()
        {
            return View(db.Categories.Where(x => x.IsDeleted == false).ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            DDLCategoryID();
            return View();
        }
        [HttpPost]
        public ActionResult Create(Category model, string DDLSecenekleri)
        {
            DDLCategoryID();
            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrEmpty(DDLSecenekleri))
                    {
                        model.UpCategoryID = Convert.ToInt32(DDLSecenekleri);
                    }
                    db.Categories.Add(model);
                    db.SaveChanges();
                    TempData["message"] = "Kategori başarıyla eklendi";
                    return RedirectToAction("Index", "Category");
                }
                catch
                {
                    ViewBag.message = "Beklenmeyen bir hata oluştu.";
                }
            }
            return View(model);
        }
        
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                TempData["message"] = "Kategori bulunamadı.";
                return RedirectToAction("Index", "Category");
            }

            //Kategoriyi bul ve üst kategori id'sini fonksiyona gönder.
            DDLCategoryID(category.UpCategoryID);

            return View(category);
        }
        [HttpPost]
        public ActionResult Edit(Category model, string DDLSecenekleri)
        {
            if (ModelState.IsValid)
            {
                Category c = db.Categories.Find(model.ID);
                if (c == null)
                {
                    TempData["message"] = "Kategori bulunamadı.";
                    return RedirectToAction("Index", "Category");
                }
                c.Name = model.Name;
                c.IsActive = model.IsActive;
                c.IsDeleted = model.IsDeleted;

                if (!string.IsNullOrEmpty(DDLSecenekleri))
                {
                    c.UpCategoryID = Convert.ToInt32(DDLSecenekleri);
                }
                else
                {
                    c.UpCategoryID = null;
                }
                db.SaveChanges();
                TempData["message"] = "Kategori başarıyla güncellendi.";
                return RedirectToAction("Index", "Category");
            }
            return View(model);
        }

        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                Category c = db.Categories.Find(id);
                if (c != null)
                {
                    c.IsDeleted = true;
                    c.IsActive = false;
                    db.SaveChanges();
                    TempData["message"] = "Kategori başarıyla silindi.";
                }
            }
            return RedirectToAction("Index", "Category");
        }
        public ActionResult Change(int? id)
        {
            if (id != null)
            {
                Category c = db.Categories.Find(id);
                if (c != null)
                {
                    c.IsActive = !c.IsActive;
                    db.SaveChanges();
                    TempData["message"] = "Durum değiştirildi.";
                }
            }
            return RedirectToAction("Index", "Category");
        }
        public ActionResult Destroy(int? id)
        {
            if (id != null)
            {
                Category c = db.Categories.Find(id);
                if (c != null)
                {
                    db.Categories.Remove(c);
                    db.SaveChanges();
                    TempData["message"] = "Kategori 'tamamen' silindi.";
                }
            }
            return RedirectToAction("Index", "Category");
        }

        public ActionResult List(int? id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                TempData["message"] = "Kategori bulunamadı.";
                return RedirectToAction("Index", "Category");
            }


            //Alt kategorileri aldık
            List<Category> categories = db.Categories.Where(x => category.ID == x.UpCategoryID && x.IsDeleted == false).ToList();

            if (categories.Count > 0)
            {
                List<Product> products = new List<Product>();
                foreach (Category c in categories) // Her bir kategorinin idsi ile ürün kategori idsini eşleştirerek products listesine ekledik
                {
                    List<Product> temp = db.Products.Where(x => x.CategoryID == c.ID && x.IsDeleted == false).ToList();
                    products.AddRange(temp);
                }
                return View(products); //Gönderdik ve oldu babba
            }
            return View(db.Products.Where(x => x.CategoryID == category.ID && x.IsDeleted == false)); //Hiç alt kategorisi yoksa direkt bu çalışacak
        }


        private void DDLCategoryID(int? selectedValue = null)
        {
            List<Category> mainCategories = db.Categories.Where(x => x.IsDeleted == false && x.UpCategoryID == null).ToList();

            List<SelectListItem> ddlSecenekleri = new List<SelectListItem>();
            ddlSecenekleri.Add(new SelectListItem
            {
                Value = "",
                Text = "Yok",
                Selected = (ddlSecenekleri == null) // Parantez içi true gelirse seçili olmuş olacak. Yani yok
            });

            foreach (Category category in mainCategories)
            {
                ddlSecenekleri.Add(
                    new SelectListItem
                    {
                        Value = category.ID.ToString(),
                        Text = category.Name,
                        // Eğer selectedValue bu kategorinin ID'sine eşit ise true gelsin yani seçili olsun.
                        Selected = (selectedValue != null && selectedValue == category.ID)
                    }
                );
            }

            //Oluşturulan seçenekleri Viewbag ile gönder.
            ViewBag.DDLSecenekleri = ddlSecenekleri;
        }
    }
}