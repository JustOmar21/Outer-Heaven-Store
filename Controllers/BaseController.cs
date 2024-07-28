using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebShopping.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string language = Request.Cookies["language"] != null ? Request.Cookies["language"].Value : "1";
            CultureInfo.CurrentUICulture = language == "1" ? new CultureInfo("en") : new CultureInfo("ar");
            base.OnActionExecuting(filterContext);
        }
    }
}