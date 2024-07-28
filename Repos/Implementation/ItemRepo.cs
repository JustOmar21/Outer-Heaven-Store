using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Data.Entity;
using WebShopping.Models;
using WebShopping.ModelsView;
using WebShopping.Repos.Interface;
using System.Web;


namespace WebShopping.Repos.Implementation
{
    public class ItemRepo : IItemRepo
    {
        private readonly StoreEntity context;
        public ItemRepo(StoreEntity context)
        {
            this.context = context;
        }
        public HttpStatusCode Add(ItemDTO item)
        {
            if (item.ImageFile == null) return HttpStatusCode.NotFound;
            //if (!Int32.TryParse(item.CategoryID,out int CatID)) return HttpStatusCode.NotAcceptable;
            string fileExtension = Path.GetExtension(item.ImageFile.FileName).ToLower();
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
            if (allowedExtensions.Contains(fileExtension))
            {
                var maxFileSize = 4 * 1024 * 1024; // 4MB
                if (item.ImageFile.ContentLength > maxFileSize)
                {
                    return HttpStatusCode.RequestEntityTooLarge;
                }



                var fileName = $"IMG{Guid.NewGuid().ToString("N")}{fileExtension}";
                var path = $"/wwwroot/Images/";
                var savePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(path) + fileName);
                item.ImageFile.SaveAs(savePath);
                var itemRecord = new Item()
                {
                    Description = item.Description,
                    Amount = item.Amount,
                    CategoryID = item.CategoryID,
                    Name = item.Name,
                    ImageURL = path+fileName,
                    Price = item.Price
                };
                context.Items.Add(itemRecord);
                context.SaveChanges();
                return HttpStatusCode.OK;
            }
            else
            {
                return HttpStatusCode.UnsupportedMediaType;
            }
        }

        public HttpStatusCode Delete(int id)
        {
            var item = GetById(id);
            if (item is null) return HttpStatusCode.NotFound;
            context.Items.Remove(item);
            context.SaveChanges();
            string fullPath = System.Web.HttpContext.Current.Server.MapPath("~" + item.ImageURL);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            return HttpStatusCode.OK;
        }

        public List<Item> GetAll()
        {
            return context.Items.Include(it=>it.Category).ToList();
        }

        public Item GetById(int id)
        {
            return context.Items.Include(it=>it.Category).SingleOrDefault(item => item.ID == id);
        }

        public HttpStatusCode Update(ItemDTO item)
        {
            var itemFind = GetById(item.ID);
            if (itemFind is null) return HttpStatusCode.NotFound;
            itemFind.Name = item.Name;
            itemFind.Price = item.Price;
            itemFind.Amount = item.Amount;
            itemFind.Description = item.Description;
            itemFind.CategoryID = item.CategoryID;
            context.SaveChanges();
            return HttpStatusCode.OK;
        }

        public HttpStatusCode UpdatePhotos(UpdatePhotoDTO item)
        {
            var itemFind = GetById(item.ID);
            if (itemFind is null) return HttpStatusCode.NotFound;
            if (item.ImageFile == null) return HttpStatusCode.NotFound;
            //if (!Int32.TryParse(item.CategoryID,out int CatID)) return HttpStatusCode.NotAcceptable;
            string fileExtension = Path.GetExtension(item.ImageFile.FileName).ToLower();
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
            if (allowedExtensions.Contains(fileExtension))
            {
                var maxFileSize = 4 * 1024 * 1024; // 4MB
                if (item.ImageFile.ContentLength > maxFileSize)
                {
                    return HttpStatusCode.RequestEntityTooLarge;
                }



                var fileName = itemFind.ImageURL.Split('/').Last().ToString().Split('.').First().ToString() + fileExtension;
                var path = $"/wwwroot/Images/";
                var savePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(path) + fileName);
                item.ImageFile.SaveAs(savePath);
                context.SaveChanges();
                return HttpStatusCode.OK;
            }
            else
            {
                return HttpStatusCode.UnsupportedMediaType;
            }
        }
    }
}