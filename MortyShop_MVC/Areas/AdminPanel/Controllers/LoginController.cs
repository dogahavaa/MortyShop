using MortyShop_MVC.Areas.AdminPanel.Data;
using MortyShop_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MortyShop_MVC.Areas.AdminPanel.Controllers
{
    public class LoginController : Controller
    {
        MortyShopDB db = new MortyShopDB();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ManagerLoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                Manager m = db.Managers.FirstOrDefault(x => x.Username == loginModel.Username && x.Password == loginModel.Password);
                if (m != null)
                {
                    if (m.IsActive)
                    {
                        m.LastEntry = DateTime.Now;
                        if (loginModel.RememberMe)
                        {
                            HttpCookie cookie = new HttpCookie("ManagerCookie");
                            cookie["username"] = loginModel.Username;
                            cookie["password"] = loginModel.Password;
                            cookie.Expires = DateTime.Now.AddDays(10);
                            Response.Cookies.Add(cookie);
                        }
                        Session["ManagerSession"] = m;
                        return RedirectToAction("Index","Home");
                    }
                    else
                    {
                        ViewBag.mesaj = "Hesabınız askıya alınmıştır.";
                    }
                }
                else
                {
                    ViewBag.mesaj = "Kullanıcı bulunamadı";
                }
            }
            return View();
        }
        public ActionResult LogOut()
        {
            Session["ManagerSession"] = null;
            if (Request.Cookies["ManagerCookie"] != null)
            {
                HttpCookie SavedCookie = Request.Cookies["ManagerCookie"];
                SavedCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(SavedCookie);
            }

            return RedirectToAction("Index", "Login");
        }


    }
}