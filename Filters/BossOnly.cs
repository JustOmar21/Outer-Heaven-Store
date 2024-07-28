using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebShopping.Models;

namespace WebShopping.Filters
{
    public class BossOnly : AuthorizeAttribute
    {
        private readonly StoreEntity context = new StoreEntity();
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authCookie = httpContext.Request.Cookies["Auth"];
            var userID = Convert.ToInt32(SimpleEncryptor.Decrypt(authCookie["ID"]));
            var admin = context.Admins.SingleOrDefault(ad => ad.ID == userID);
            return admin == null ? false : admin.IsBoss;
        }
    }
}