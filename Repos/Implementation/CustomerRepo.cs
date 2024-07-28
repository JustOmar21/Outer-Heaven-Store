using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using WebShopping.Models;
using WebShopping.Repos.Interface;

namespace WebShopping.Repos.Implementation
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly StoreEntity context;
        public CustomerRepo(StoreEntity storeEntity)
        {
            context = storeEntity;
        }
        public HttpStatusCode Add(Customer customer)
        {
            context.Customers.Add(customer);
            context.SaveChanges();
            return HttpStatusCode.OK;
        }

        public HttpStatusCode Delete(int id)
        {
            var customer = GetById(id);
            if (customer is null) return HttpStatusCode.NotFound;
            context.Customers.Remove(customer);
            context.SaveChanges();
            return HttpStatusCode.OK;
        }

        public List<Customer> GetAll()
        {
            return context.Customers.ToList();
        }

        public Customer GetById(int id)
        {
            var customer = context.Customers.Include(cust=>cust.Invoices).SingleOrDefault(cust=> cust.ID == id);
            return customer;
        }

        public HttpStatusCode Update(Customer customer)
        {
            var customerFind = GetById(customer.ID);
            if (customerFind is null) return HttpStatusCode.NotFound;
            customerFind.Phone = customer.Phone;
            customerFind.Address = customer.Address;
            customerFind.Gender = customer.Gender;
            customerFind.Name = customer.Name;
            context.SaveChanges();
            return HttpStatusCode.OK;
        }
    }
}