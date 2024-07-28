using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebShopping.Filters;
using WebShopping.Models;
using WebShopping.Repos.Implementation;
using WebShopping.Repos.Interface;

namespace WebShopping.Controllers
{
    [AuthorizeXS]
    [BossOnly]
    public class AdminController : BaseController
    {
        private readonly IAdminRepo _adminRepo;
        public AdminController(IAdminRepo adminRepo)
        {
            _adminRepo = adminRepo;
        }

        public ActionResult Index()
        {
            return View(_adminRepo.GetAll());
        }

        [HttpGet]
        [Route("Add")]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Admin admin)
        {
            admin.IsBoss = false;
            admin.Password = SimpleEncryptor.Encrypt("13589100sS");
            ModelState["Password"].Errors.Clear();
            if (ModelState.IsValid)
            {
                if (_adminRepo.Add(admin) == HttpStatusCode.OK)
                {
                    TempData["Added"] = $"{Resources.Resources.AdminInsert}";
                }
                else
                {
                    TempData["Error"] = $"{Resources.Resources.AdminInsertFail}";
                }
                return RedirectToAction("Index");
            }
            return View(admin);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id = -1)
        {
            var result = _adminRepo.Delete(id);
            if (result == HttpStatusCode.OK)
            {
                TempData["Deleted"] = $"{Resources.Resources.AdminDelete}";
            }
            else if (result == HttpStatusCode.NotFound)
            {
                TempData["Error"] = $"{Resources.Resources.AdminNotFound}";
            }
            else if (result == HttpStatusCode.Forbidden)
            {
                TempData["Error"] = $"{Resources.Resources.DelBoss}";
            }
            else
            {
                TempData["Error"] = $"{Resources.Resources.ErrorEvent}";
            }
            return RedirectToAction("Index");
        }
    }
}