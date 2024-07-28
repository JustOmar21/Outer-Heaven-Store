using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebShopping.Models;

namespace WebShopping.Repos.Interface
{
    public interface IAdminRepo
    {
        List<Admin> GetAll();
        Admin GetById(int id);
        HttpStatusCode Add(Admin admin);
        HttpStatusCode Delete(int id);
    }
}
