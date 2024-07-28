using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace WebShopping.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
        public ActionResult ChangeLang(int langID)
        {
            CultureInfo.DefaultThreadCurrentUICulture = langID == 1 ? new CultureInfo("en") : new CultureInfo("ar");
            HttpCookie lang = new HttpCookie("language") { Expires = DateTime.Now.AddYears(2), Value=$"{langID}" };
            Response.SetCookie(lang);
            return Redirect(Request.UrlReferrer.ToString() ?? "/");
        }
    }
}