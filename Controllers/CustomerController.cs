using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebShopping.Filters;
using WebShopping.Models;
using WebShopping.Repos.Interface;

namespace WebShopping.Controllers
{
    [AuthorizeXS]
    public class CustomerController : BaseController
    {
        private readonly ICustomerRepo _customerRepo;
        public CustomerController(ICustomerRepo customerRepo)
        {
            _customerRepo = customerRepo;
        }

        public ActionResult Index()
        {
            var customers = _customerRepo.GetAll();
            return View(customers);
        }
        [HttpGet]
        [Route("Add")]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Customer customer)
        {
            if(ModelState.IsValid)
            {
                if (_customerRepo.Add(customer) == HttpStatusCode.OK)
                {
                    TempData["Added"] = $"{Resources.Resources.CusInsert}";
                }
                else
                {
                    TempData["Error"] = $"{Resources.Resources.CusInsertFail}";
                }
                return RedirectToAction("Index");
            }
            return View(customer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id = -1)
        {
            var result = _customerRepo.Delete(id);
            if (result == HttpStatusCode.OK)
            {
                TempData["Deleted"] = $"{Resources.Resources.CusDelete}";
            }
            else if (result == HttpStatusCode.NotFound)
            {
                TempData["Error"] = $"{Resources.Resources.CusNotFound}";
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
            var customer = _customerRepo.GetById(id);
            if(customer is null)
            {
                TempData["Error"] = $"{Resources.Resources.CusNotFound}";
                return RedirectToAction("Index");
            }
            return View(customer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (_customerRepo.Update(customer) == HttpStatusCode.OK)
                {
                    TempData["Added"] = $"{Resources.Resources.CusUpdate}";
                }
                else
                {
                    TempData["Error"] = $"{Resources.Resources.CusUpdateFail}";
                }
                return RedirectToAction("Index");
            }
            return View(customer);
        }
        [HttpGet]
        [Route("Invoices")]
        public ActionResult Invoices(int id = -1)
        {
            var customer = _customerRepo.GetById(id);
            if (customer is null)
            {
                TempData["Error"] = $"{Resources.Resources.CusNotFound}";
                return RedirectToAction("Index");
            }
            return View(customer);  
        }

    }
}