using MortyShop_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace MortyShop_MVC.Areas.AdminPanel.Filters
{
    public class ManagerLoginRequiredFilterAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        MortyShopDB db = new MortyShopDB();
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (string.IsNullOrEmpty(Convert.ToString(filterContext.HttpContext.Session["ManagerSession"])))
            {
                if (filterContext.HttpContext.Request.Cookies["ManagerCookie"] != null)
                {
                    HttpCookie savedCookie = filterContext.HttpContext.Request.Cookies["ManagerCookie"];
                    string username = savedCookie.Values["username"];
                    string password = savedCookie.Values["password"];
                    Manager m = db.Managers.FirstOrDefault(x => x.Username == username && x.Password == password);
                    if (m != null)
                    {
                        if (m != null)
                        {
                            filterContext.HttpContext.Session["ManagerSession"] = m;
                        }
                    }
                }
                else
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                }
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectResult("~/AdminPanel/Login/Index");
            }
        }
    }
}