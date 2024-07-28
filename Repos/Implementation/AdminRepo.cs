using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Data.Entity;
using WebShopping.Models;
using WebShopping.Repos.Interface;

namespace WebShopping.Repos.Implementation
{
    public class AdminRepo : IAdminRepo
    {
        private readonly StoreEntity context;
        public AdminRepo(StoreEntity storeEntity)
        {
            context = storeEntity;
        }
        public HttpStatusCode Add(Admin admin)
        {
            context.Admins.Add(admin);
            context.SaveChanges();
            return HttpStatusCode.OK;
        }

        public HttpStatusCode Delete(int id)
        {
            var admin = context.Admins.Find(id);
            if (admin is null) return HttpStatusCode.NotFound;
            if(admin.IsBoss == true) return HttpStatusCode.Forbidden;
            context.Admins.Remove(admin);
            context.SaveChanges();
            return HttpStatusCode.OK;
        }

        public List<Admin> GetAll()
        {
            return context.Admins.ToList();
        }

        public Admin GetById(int id)
        {
            return context.Admins.SingleOrDefault(ad=> ad.ID == id);
        }
    }
}