using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebShopping.Filters;
using WebShopping.Models;
using WebShopping.ModelsView;
using WebShopping.Repos.Implementation;
using WebShopping.Repos.Interface;

namespace WebShopping.Controllers
{
    [AuthorizeXS]
    public class ItemController : BaseController
    {
        private readonly IItemRepo _itemRepo;
        private readonly ICategoryRepo _categoryRepo;
        public ItemController(IItemRepo itemRepo, ICategoryRepo categoryRepo)
        {
            _itemRepo = itemRepo;
            _categoryRepo = categoryRepo;
        }

        public ActionResult Index()
        {
            return View(_itemRepo.GetAll());
        }
        [HttpGet]
        [Route("Add")]
        public ActionResult Add()
        {
            var categories = _categoryRepo.GetAll();
            if (categories == null || categories.Count == 0)
            {
                TempData["Error"] = $"{Resources.Resources.ItemCatWarning}";
                return RedirectToAction("Index");
            }
            ViewBag.Categories = categories;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ItemDTO item)
        {
            if (ModelState.IsValid)
            {
                var result = _itemRepo.Add(item);
                if ( result == HttpStatusCode.OK)
                {
                    TempData["Added"] = $"{Resources.Resources.ItemInsert}";
                }
                else if (result == HttpStatusCode.UnsupportedMediaType)
                {
                    ModelState.AddModelError("ImageFile", $"{Resources.Resources.ImgSupport}");
                    ViewBag.Categories = _categoryRepo.GetAll();
                    return View(item);
                }
                else if (result == HttpStatusCode.RequestEntityTooLarge)
                {
                    ModelState.AddModelError("ImageFile", $"{Resources.Resources.ImgLarge}");
                    ViewBag.Categories = _categoryRepo.GetAll();
                    return View(item);
                }
                else
                {
                    TempData["Error"] = $"{Resources.Resources.ItemInsertFail}";
                }
                return RedirectToAction("Index");
            }
            ViewBag.Categories = _categoryRepo.GetAll();
            return View(item);
        }
        [HttpGet]
        [Route("Edit")]
        public ActionResult Edit(int id = -1)
        {
            var Item = _itemRepo.GetById(id);
            if (Item is null)
            {
                TempData["Error"] = $"{Resources.Resources.ItemNotFound}";
                return RedirectToAction("Index");
            }
            var categories = _categoryRepo.GetAll();
            if (categories == null || categories.Count == 0)
            {
                TempData["Warning"] = $"{Resources.Resources.ItemCatWarning}";
                return RedirectToAction("Index");
            }
            ViewBag.Categories = categories;
            return View(Item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Item item)
        {
            ModelState["ImageURL"].Errors.Clear();
            if (ModelState.IsValid)
            {
                var itemDTO = new ItemDTO() { Amount = item.Amount, CategoryID = item.CategoryID, ID = item.ID, Description = item.Description, Name = item.Name, Price = item.Price };
                var result = _itemRepo.Update(itemDTO);
                if (result == HttpStatusCode.OK)
                {
                    TempData["Added"] = $"{Resources.Resources.ItemUpdate}";
                }
                else
                {
                    TempData["Error"] = $"{Resources.Resources.ItemUpdateFail}";
                }
                return RedirectToAction("Index");
            }
            ViewBag.Categories = _categoryRepo.GetAll();
            return View(item);
        }

        [HttpGet]
        [Route("UpdatePhoto")]
        public ActionResult UpdatePhoto(int id = -1)
        {
            var Item = _itemRepo.GetById(id);
            if (Item is null)
            {
                TempData["Error"] = $"{Resources.Resources.ItemNotFound}";
                return RedirectToAction("Index");
            }
            var itemPhoto = new UpdatePhotoDTO() { ID = id };
            return View(itemPhoto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePhoto(UpdatePhotoDTO item)
        {
            if (ModelState.IsValid)
            {
                var result = _itemRepo.UpdatePhotos(item);
                if (result == HttpStatusCode.OK)
                {
                    TempData["Added"] = $"{Resources.Resources.ItemPhotoUpdate}";
                }
                else if (result == HttpStatusCode.UnsupportedMediaType)
                {
                    ModelState.AddModelError("ImageFile", $"{Resources.Resources.ImgSupport}");
                    return View(item);
                }
                else if (result == HttpStatusCode.RequestEntityTooLarge)
                {
                    ModelState.AddModelError("ImageFile", $"{Resources.Resources.ImgLarge}");
                    return View(item);
                }
                else
                {
                    TempData["Error"] = $"{Resources.Resources.ItemPhotoUpdateFail}";
                }
                return RedirectToAction("Index");
            }
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id = -1)
        {
            var result = _itemRepo.Delete(id);
            if (result == HttpStatusCode.OK)
            {
                TempData["Deleted"] = $"{Resources.Resources.ItemDelete}";
            }
            else if (result == HttpStatusCode.NotFound)
            {
                TempData["Error"] = $"{Resources.Resources.ItemNotFound}";
            }
            else
            {
                TempData["Error"] = $"{Resources.Resources.ErrorEvent}";
            }
            return RedirectToAction("Index");
        }
    }
}