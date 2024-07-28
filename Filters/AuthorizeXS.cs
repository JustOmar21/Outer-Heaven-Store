using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebShopping.Models;
using WebShopping.Repos.Interface;

namespace WebShopping.Filters
{
    public class AuthorizeXS : AuthorizeAttribute
    {
        private readonly StoreEntity context = new StoreEntity();
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authCookie = httpContext.Request.Cookies["Auth"];
            if ( authCookie != null)
            {
                var userID = Convert.ToInt32(SimpleEncryptor.Decrypt(authCookie["ID"]));
                var LoginID = SimpleEncryptor.Decrypt(authCookie["LoginID"]);
                var admin = context.Admins.AsNoTracking().SingleOrDefault(ad => ad.ID == userID);
                if ( admin == null ) { return false; }
                if (LoginID == admin.LoginID)
                {
                    return true;
                }
                else
                {
                    HttpCookie cookie = new HttpCookie("Auth") { Expires = DateTime.Now.AddYears(-3) };
                    httpContext.Response.SetCookie(cookie);
                    return false;
                }
            }
            else
            {
                return false;
            }    
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
        }
    }
}