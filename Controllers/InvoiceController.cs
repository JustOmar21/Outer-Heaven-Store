using Microsoft.Ajax.Utilities;
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
    public class InvoiceController : BaseController
    {
        private readonly IInvoiceRepo _invoiceRepo;
        private readonly ICustomerRepo _customerRepo;
        private readonly IItemRepo _itemRepo;
        private readonly ICategoryRepo _categoryRepo;
        public InvoiceController(IInvoiceRepo invoiceRepo, ICustomerRepo customerRepo, IItemRepo itemRepo, ICategoryRepo categoryRepo)
        {
            this._invoiceRepo = invoiceRepo;
            this._customerRepo = customerRepo;
            this._itemRepo = itemRepo;
            _categoryRepo = categoryRepo;
        }
        public ActionResult Index()
        {
            return View(_invoiceRepo.GetAll());
        }
        public ActionResult Details(int invoiceID = -1)
        {
            var invoice = _invoiceRepo.GetSingleInvoiceWItems(invoiceID);
            if(invoice is null)
            {
                TempData["Error"] = $"{Resources.Resources.InvNotFound}";
                return RedirectToAction("Index");
            }
            return View(invoice);
        }
        [HttpGet]
        [Route("Add")]
        public ActionResult Add()
        {
            var customers = _customerRepo.GetAll();
            if(customers.Count()==0)
            {
                TempData["Error"] = $"{Resources.Resources.InvNoCustomers}";
                return RedirectToAction("Index");
            }
            ViewBag.Customers = _customerRepo.GetAll();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                if (_invoiceRepo.Add(invoice) == HttpStatusCode.OK)
                {
                    TempData["Added"] = $"{Resources.Resources.InvInsert}";
                }
                else
                {
                    TempData["Error"] = $"{Resources.Resources.InvInsertFail}";
                }
                return RedirectToAction("Index");
            }
            ViewBag.Customers = _customerRepo.GetAll();
            return View(invoice);
        }
        [HttpGet]
        public ActionResult Edit(int id = -1)
        {
            var invoice = _invoiceRepo.GetById(id);
            if (invoice is null)
            {
                TempData["Error"] = $"{Resources.Resources.InvNotFound}";
                return RedirectToAction("Index");
            }
            ViewBag.Customers = _customerRepo.GetAll();
            return View(invoice);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                if (_invoiceRepo.Update(invoice) == HttpStatusCode.OK)
                {
                    TempData["Added"] = $"{Resources.Resources.InvUpdate}";
                }
                else
                {
                    TempData["Error"] = $"{Resources.Resources.InvUpdateFail}";
                }
                return RedirectToAction("Index");
            }
            ViewBag.Customers = _customerRepo.GetAll();
            return View(invoice);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id = -1)
        {
            var result = _invoiceRepo.Delete(id);
            if (result == HttpStatusCode.OK)
            {
                TempData["Deleted"] = $"{Resources.Resources.InvDelete}";
            }
            else if (result == HttpStatusCode.NotFound)
            {
                TempData["Error"] = $"{Resources.Resources.InvNotFound}";
            }
            else
            {
                TempData["Error"] = $"{Resources.Resources.ErrorEvent}";
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("AddItem")]
        public ActionResult AddItem(int invoiceID = -1)
        {
            var invoice = _invoiceRepo.GetById(invoiceID);
            if (invoice == null)
            {
                TempData["Error"] = $"{Resources.Resources.InvNotFound}";
                return RedirectToAction("Index");
            }
            var itemsList = _itemRepo.GetAll();
            if (itemsList.Count() == 0)
            {
                TempData["Error"] = $"{Resources.Resources.InvItemNoItems}";
                return RedirectToAction("Index");
            }
            var invItem = new InvoiceItems() { InvoiceID =  invoiceID , Amount = 0 , Discount = 0, ItemID = null };
            var categories = _categoryRepo.GetAllWithItems();
            long items = 0;
            foreach(var cat in categories)
            {
                items += cat.Items.Count();
            }
            if(items == 0)
            {
                TempData["Error"] = $"{Resources.Resources.NoItemWarning}";
                RedirectToAction("Details", new { invoiceID = invoiceID });
            }
            ViewBag.Categories = categories;
            return View(invItem);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddItem(InvoiceItems invoice)
        {
            if (ModelState.IsValid)
            {
                var result = _invoiceRepo.AddItem(invoice);
                if ( result == HttpStatusCode.OK)
                {
                    TempData["Added"] = $"{Resources.Resources.InvItemInsert}";
                }
                else if ( result == HttpStatusCode.RequestEntityTooLarge)
                {
                    var item = _itemRepo.GetById((int)invoice.ItemID);
                    if(item is null)
                    {
                        TempData["Error"] = $"{Resources.Resources.ItemNotFound}";
                        RedirectToAction("Details", new { invoiceID = invoice.InvoiceID });
                    }
                    ViewBag.Categories = _categoryRepo.GetAllWithItems();
                    ModelState.AddModelError("Amount", $"{Resources.Resources.ItemAmount} {item.Amount} {Resources.Resources.Units}");
                    return View(invoice);
                }
                else if( result == HttpStatusCode.Forbidden)
                {
                    TempData["Error"] = $"{Resources.Resources.InvItemExists}";
                    return RedirectToAction("Details", new { invoiceID = invoice.InvoiceID });
                }
                return RedirectToAction("Details",new {invoiceID = invoice.InvoiceID});
            }
            ViewBag.Categories = _categoryRepo.GetAllWithItems();
            return View(invoice);
        }
        [HttpGet]
        [Route("EditItem")]
        public ActionResult EditItem(int invoiceID = -1, int itemID = -1)
        {
            var invoiceItem = _invoiceRepo.GetInvoiceItemByID(invoiceID, itemID);
            if (invoiceItem == null)
            {
                TempData["Error"] = $"{Resources.Resources.InvItemNotFound}";
                return RedirectToAction("Details", new { invoiceID });
            }
            ViewBag.Categories = _categoryRepo.GetAllWithItems();
            return View(invoiceItem);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditItem(InvoiceItems invoice)
        {
            var invoiceItem = _invoiceRepo.GetInvoiceItemByID(invoice.InvoiceID,(int)invoice.ItemID);
            if (invoiceItem == null) {
                TempData["Error"] = $"{Resources.Resources.InvItemNotFound}";
                return RedirectToAction("Details", new { invoice.InvoiceID });
            }
            invoice.Item = invoiceItem.Item;
            if (ModelState.IsValid)
            {
                var result = _invoiceRepo.UpdateItem(invoice);
                if (result == HttpStatusCode.OK)
                {
                    TempData["Added"] = $"{Resources.Resources.InvItemUpdate}";
                }
                else if (result == HttpStatusCode.RequestEntityTooLarge)
                {
                    var item = _itemRepo.GetById((int)invoice.ItemID);
                    if (item is null)
                    {
                        TempData["Error"] = $"{Resources.Resources.ItemNotFound}";
                        return RedirectToAction("Details", new { invoiceID = invoice.InvoiceID });
                    }
                    ViewBag.Categories = _categoryRepo.GetAllWithItems();
                    ModelState.AddModelError("Amount", $"{Resources.Resources.ItemAmount} {item.Amount} {Resources.Resources.Units}");
                    return View(invoice);
                }
                else if (result == HttpStatusCode.NotFound)
                {
                    TempData["Error"] = $"{Resources.Resources.InvItemNotFound}";
                    return RedirectToAction("Details", new { invoiceID = invoice.InvoiceID });
                }
                return RedirectToAction("Details", new { invoiceID = invoice.InvoiceID });
            }
            ViewBag.Categories = _categoryRepo.GetAllWithItems();
            return View(invoice);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(int invoiceID = -1, int itemID = -1)
        {
            var result = _invoiceRepo.DeleteItem(invoiceID,itemID);
            if (result == HttpStatusCode.OK)
            {
                TempData["Deleted"] = $"{Resources.Resources.InvItemDelete}";
            }
            else if (result == HttpStatusCode.NotFound)
            {
                TempData["Error"] = $"{Resources.Resources.InvItemNotFound}";
            }
            else
            {
                TempData["Error"] = $"{Resources.Resources.ErrorEvent}";
            }
            return RedirectToAction("Details", new { invoiceID });

        }

    }
}