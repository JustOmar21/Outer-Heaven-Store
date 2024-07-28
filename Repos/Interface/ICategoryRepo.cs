using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebShopping.Models;

namespace WebShopping.Repos.Interface
{
    public interface ICategoryRepo
    {
        List<Category> GetAll();
        Category GetById(int id);
        Category GetByName(string name);
        List<Category> GetAllWithItems();
        HttpStatusCode Add(Category category);
        HttpStatusCode Update(Category category);
        HttpStatusCode Delete(int id);

    }
}
