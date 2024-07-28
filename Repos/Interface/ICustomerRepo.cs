using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebShopping.Models;

namespace WebShopping.Repos.Interface
{
    public interface ICustomerRepo
    {
        List<Customer> GetAll();
        Customer GetById(int id);
        HttpStatusCode Add(Customer customer);
        HttpStatusCode Update(Customer customer);
        HttpStatusCode Delete(int id);
    }
}
