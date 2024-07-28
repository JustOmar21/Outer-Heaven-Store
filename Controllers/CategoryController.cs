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
    public class CategoryController : BaseController
    {
        private readonly ICategoryRepo _categoryRepo;
        public CategoryController(ICategoryRepo categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }
        public ActionResult Index()
        {
            return View(_categoryRepo.GetAll());
        }
        [HttpGet]
        [Route("Add")]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Category category)
        {
            if (ModelState.IsValid)
            {
                if (_categoryRepo.Add(category) == HttpStatusCode.OK)
                {
                    TempData["Added"] = $"{Resources.Resources.CategoryInsert}";
                }
                else
                {
                    TempData["Error"] = $"{Resources.Resources.CatInsertFail}";
                }
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id = -1)
        {
            var result = _categoryRepo.Delete(id);
            if (result == HttpStatusCode.OK)
            {
                TempData["Deleted"] = $"{Resources.Resources.CatDelete}";
            }
            else if (result == HttpStatusCode.NotFound)
            {
                TempData["Error"] = $"{Resources.Resources.CatNotFound}";
            }
            else
            {
                TempData["Error"] = $"{Resources.Resources.ErrorEvent}";
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id = -1)
        {
            var category = _categoryRepo.GetById(id);
            if (category is null)
            {
                TempData["Error"] = $"{Resources.Resources.CatNotFound}";
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                if (_categoryRepo.Update(category) == HttpStatusCode.OK)
                {
                    TempData["Added"] = $"{Resources.Resources.CatUpdate}";
                }
                else
                {
                    TempData["Error"] = $"{Resources.Resources.CatUpdateFail}";
                }
                return RedirectToAction("Index");
            }
            return View(category);
        }
    }
}