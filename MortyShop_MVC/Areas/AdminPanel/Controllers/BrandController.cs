using MortyShop_MVC.Areas.AdminPanel.Filters;
using MortyShop_MVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MortyShop_MVC.Areas.AdminPanel.Controllers
{
    [ManagerLoginRequiredFilter]
    public class BrandController : Controller
    {
        
        MortyShopDB db = new MortyShopDB();

        public ActionResult Index()
        {
            return View(db.Brands.Where(x => x.IsDeleted == false));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Brand model, HttpPostedFileBase logo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool isvalidimage = true;
                    if (logo != null)
                    {
                        FileInfo fi = new FileInfo(logo.FileName);
                        string extension = fi.Extension;
                        if (extension == ".jpg" || extension == ".jpeg" ||extension == ".png")
                        {
                            string generateName = Guid.NewGuid().ToString() + extension;
                            model.Logo = generateName;
                            logo.SaveAs(Server.MapPath("~/Assets/Images/BrandImages/"+generateName));
                        }
                        else
                        {
                            isvalidimage = false;
                            ViewBag.message = "Eklenebilir dosya uzantısı sadece .jpg, .jpeg veya .png olabilir.";
                        }
                    }
                    else
                    {
                        model.Logo = "noImage.png";
                    }
                    if (isvalidimage)
                    {
                        db.Brands.Add(model);
                        db.SaveChanges();
                        TempData["message"] = "Marka başarıyla eklendi";
                        return RedirectToAction("Index", "Brand");
                    }
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
            if (id == null)
            {
                TempData["message"] = "Marka bulunamadı.";
                return RedirectToAction("Index", "Brand");
            }

            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                TempData["message"] = "Marka bulunamadı.";
                return RedirectToAction("Index", "Brand");
            }

            if (brand.IsDeleted)
            {
                TempData["message"] = "Marka silinmiş";
                return RedirectToAction("Index", "Brand");
            }
            return View(brand);
        }

        [HttpPost]
        public ActionResult Edit(Brand model, HttpPostedFileBase logo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool isvalidimage = true;
                    if (logo != null)
                    {
                        FileInfo fi = new FileInfo(logo.FileName);
                        string ext = fi.Extension;
                        if (ext == ".jpg" ||ext == ".jpeg" || ext == ".png" )
                        {
                            string name = Guid.NewGuid().ToString() + ext;
                            model.Logo = name;
                            logo.SaveAs(Server.MapPath("~/Assets/Images/BrandImages/" + name));
                        }
                        else
                        {
                            isvalidimage = false;
                            ViewBag.message = "Eklenebilir dosya uzantısı sadece .jpg, .jpeg veya .png olabilir.";
                        }
                    }
                    if (isvalidimage)
                    {
                        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        TempData["message"] = "Marka başarıyla güncellendi.";
                        return RedirectToAction("Index", "Brand");
                    }
                }
                catch 
                {
                    ViewBag.message = "Beklenmedik bir hata oluştu";
                }
            }
            return View(model);
        }

        public ActionResult Delete(int? id)
        {
            if(id == null)
            {
                TempData["message"] = "Marka bulunamadı";
                return RedirectToAction("Index", "Brand");
            }
            Brand b = db.Brands.Find(id);
            if (b == null)
            {
                TempData["message"] = "Marka bulunamadı";
                return RedirectToAction("Index", "Brand");
            }
            b.IsDeleted = true;
            b.IsActive = false;
            db.SaveChanges();
            TempData["message"] = "Marka başarıyla silindi.";
            return RedirectToAction("Index", "Brand");
        }

        public ActionResult Destroy(int? id)
        {
            if(id == null)
            {
                TempData["message"] = "Marka bulunamadı";
                return RedirectToAction("Index", "Brand");
            }
            Brand b = db.Brands.Find(id);
            if (b == null)
            {
                TempData["message"] = "Marka bulunamadı";
                return RedirectToAction("Index", "Brand");
            }
            db.Brands.Remove(b);
            db.SaveChanges();
            TempData["message"] = "Marka 'tamamen' silindi.";
            return RedirectToAction("Index", "Brand");
        }

        public ActionResult Change(int? id)
        {
            if (id == null)
            {
                TempData["message"] = "Marka bulunamadı";
                return RedirectToAction("Index", "Brand");
            }
            Brand b = db.Brands.Find(id);
            if (b == null)
            {
                TempData["message"] = "Marka bulunamadı";
                return RedirectToAction("Index", "Brand");
            }
            b.IsActive = !b.IsActive;
            db.SaveChanges();
            TempData["message"] = "Durum değiştirildi.";
            return RedirectToAction("Index", "Brand");
        }

        public ActionResult List(int? id)
        {
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                TempData["message"] = "Marka bulunamadı.";
                return RedirectToAction("Index", "Brand");
            }

            return View(db.Products.Where(x => x.BrandID == brand.ID && x.IsDeleted == false)); 
        }
    }
}