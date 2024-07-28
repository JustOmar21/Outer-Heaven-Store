using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebShopping.Models;
using WebShopping.ModelsView;

namespace WebShopping.Repos.Interface
{
    public interface IItemRepo
    {
        List<Item> GetAll();
        Item GetById(int id);
        HttpStatusCode Add(ItemDTO item);
        HttpStatusCode Update(ItemDTO item);
        HttpStatusCode Delete(int id);
        HttpStatusCode UpdatePhotos(UpdatePhotoDTO item);
    }
}
