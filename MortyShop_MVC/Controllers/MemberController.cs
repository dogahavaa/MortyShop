using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MortyShop_MVC.Models;

namespace MortyShop_MVC.Controllers
{
    public class MemberController : Controller
    {
        MortyShopDB db = new MortyShopDB();
        // GET: Member
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Member model)
        {
            if (ModelState.IsValid)
            {
                if (db.Members.Any(x => x.Email == model.Email))
                {
                    ViewBag.message = "Bu E-Posta zaten kayıtlı.";
                    return View(model);
                }

                model.RegistrationDate = DateTime.Now;
                model.IsActive = true;
                model.IsDeleted = false;
                db.Members.Add(model);
                db.SaveChanges();
                TempData["message"] = "Kayıt işlemi başarılı.";
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(password))
            {
                ViewBag.message = "E-posta ve şifre alanları zorunludur.";
            }

            Member m = db.Members.FirstOrDefault(x => x.Email == email && x.Password == password);
            if (m != null)
            {
                if (m.IsDeleted)
                {
                    ViewBag.message = "Hesabınız silinmiş.";
                    return View();
                }
                if (!m.IsActive)
                {
                    ViewBag.message = "Hesabınız askıya alınmıştır.";
                    return View();
                }

                Session["Member"] = m;

                m.LastLoginDate = DateTime.Now;
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            ViewBag.message = "E-posta veya şifre hatalı.";
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}