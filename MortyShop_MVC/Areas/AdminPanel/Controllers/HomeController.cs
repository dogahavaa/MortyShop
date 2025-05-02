using MortyShop_MVC.Areas.AdminPanel.Filters;
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
        // GET: AdminPanel/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}