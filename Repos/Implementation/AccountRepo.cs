using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;
using WebShopping.Models;
using WebShopping.ModelsView;
using WebShopping.Repos.Interface;

namespace WebShopping.Repos.Implementation
{
    public class AccountRepo : IAccountRepo
    {
        private readonly StoreEntity context;
        public AccountRepo(StoreEntity context)
        {
            this.context = context;
        }
        public HttpStatusCode AddLog(Log log)
        {
            throw new NotImplementedException();
        }

        public HttpStatusCode ChangeName(AdminName name)
        {
            var admin = context.Admins.Find(name.ID);
            if (admin is null) return HttpStatusCode.NotFound;
            admin.Name = name.Name;
            context.SaveChanges();
            return HttpStatusCode.OK;
        }

        public HttpStatusCode ChangePassword(AdminPassword password)
        {
            var admin = context.Admins.Find(password.ID);
            if (admin is null) return HttpStatusCode.NotFound;
            if (password.OldPassword != SimpleEncryptor.Decrypt(admin.Password)) return HttpStatusCode.Forbidden;
            admin.Password = SimpleEncryptor.Encrypt(password.NewPassword);
            context.SaveChanges();
            return HttpStatusCode.OK;
        }

        public List<Log> GetLogs()
        {
            throw new NotImplementedException();
        }

        public Admin Login(Login login)
        {
            var admin = context.Admins.SingleOrDefault(ad=> ad.Email == login.Email);
            if (admin is null) return null;
            admin = SimpleEncryptor.Decrypt(admin.Password) == login.Password ? admin : null;
            if (admin is null) return null;
            admin.LoginID = Guid.NewGuid().ToString();
            context.SaveChanges();
            admin = context.Admins.SingleOrDefault(ad => ad.Email == login.Email);
            return admin;
        }

        public HttpStatusCode Logout()
        {
            throw new NotImplementedException();
        }

        public HttpStatusCode ResetAdminPassword(int adminID)
        {
            var admin = context.Admins.Find(adminID);
            if (admin is null) return HttpStatusCode.NotFound;
            admin.Password = SimpleEncryptor.Encrypt("13589100sS");
            context.SaveChanges();
            return HttpStatusCode.OK;
        }

        public HttpStatusCode ResetAllAdminsPasswords()
        {
            throw new NotImplementedException();
        }
    }
}