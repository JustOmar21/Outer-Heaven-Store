using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebShopping.Filters;
using WebShopping.ModelsView;
using WebShopping.Repos.Interface;

namespace WebShopping.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountRepo _accountRepo;
        private readonly IAdminRepo _adminRepo;
        public AccountController(IAccountRepo accountRepo, IAdminRepo adminRepo)
        {
            this._accountRepo = accountRepo;
            this._adminRepo = adminRepo;
        }
        [HttpGet]
        public ActionResult Login()
        {
            if (Request.Cookies["Auth"] != null)
                return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login login)
        {
            if(ModelState.IsValid)
            {
                var admin = _accountRepo.Login(login);
                if(admin != null )
                {
                    HttpCookie auth = new HttpCookie("Auth") { Expires = DateTime.Now.AddHours(3) };
                    auth.Values.Add("ID", SimpleEncryptor.Encrypt($"{admin.ID}"));
                    auth.Values.Add("Boss",SimpleEncryptor.Encrypt(admin.IsBoss.ToString()));
                    auth.Values.Add("LoginID", SimpleEncryptor.Encrypt($"{admin.LoginID}"));
                    Response.SetCookie(auth);
                    return RedirectToAction("Index","Home");
                }
                
            }
            ModelState.AddModelError("", $"{Resources.Resources.PasswordLog}");
            return View(login);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            HttpCookie cookie = new HttpCookie("Auth") { Expires = DateTime.Now.AddYears(-3) };
            Response.SetCookie(cookie);
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [AuthorizeXS]
        [BossOnly]
        public ActionResult ResetPass(int adminID = -1)
        {
            var result = _accountRepo.ResetAdminPassword(adminID);
            if(result == HttpStatusCode.OK)
            {
                TempData["Added"] = $"{Resources.Resources.AdminPass}";
            }
            else
            {
                TempData["Error"] = $"{Resources.Resources.AdminNotFound}";
            }
            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        [AuthorizeXS]
        [BossOnly]
        public ActionResult ChangeName(int adminID = -1)
        {
            var admin = _adminRepo.GetById(adminID);
            if(admin == null)
            {
                TempData["Error"] = $"{Resources.Resources.AdminNotFound}";
                return RedirectToAction("Index", "Admin");
            }
            var newName = new AdminName() { ID = adminID ,Name = admin.Name};
            return View(newName);
        }
        [HttpPost]
        [AuthorizeXS]
        [BossOnly]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeName(AdminName adminName)
        {
            if(ModelState.IsValid)
            {
                var result = _accountRepo.ChangeName(adminName);
                if(result == HttpStatusCode.OK)
                {
                    TempData["Added"] = $"{Resources.Resources.NameChange}";
                }
                else
                {
                    TempData["Error"] = $"{Resources.Resources.AdminNotFound}";
                }
                return RedirectToAction("Index", "Admin");
            }
            return View(adminName);
        }

        [HttpGet]
        [AuthorizeXS]
        public ActionResult ChangePass(int adminID = -1)
        {
            var admin = _adminRepo.GetById(adminID);
            if (admin == null)
            {
                TempData["Error"] = $"{Resources.Resources.AdminNotFound}";
                return RedirectToAction("Index", "Home");
            }
            var newName = new AdminPassword() { ID = adminID };
            return View(newName);
        }
        [HttpPost]
        [AuthorizeXS]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePass(AdminPassword adminPassword)
        {
            if (ModelState.IsValid)
            {
                var result = _accountRepo.ChangePassword(adminPassword);
                if (result == HttpStatusCode.OK)
                {
                    TempData["Added"] = $"{Resources.Resources.PassChanged}";
                }
                else if(result == HttpStatusCode.Forbidden)
                {
                    ModelState.AddModelError("OldPassword",$"{Resources.Resources.WrongPass}");
                    return View(adminPassword);
                }
                else
                {
                    TempData["Error"] = $"{Resources.Resources.AdminNotFound}";
                }
                return RedirectToAction("Index", "Home");
            }
            return View(adminPassword);
        }
    }
}