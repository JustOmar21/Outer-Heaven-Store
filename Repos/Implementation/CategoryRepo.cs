using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Data.Entity;
using WebShopping.Models;
using WebShopping.Repos.Interface;

namespace WebShopping.Repos.Implementation
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly StoreEntity context;
        public CategoryRepo(StoreEntity context)
        {
            this.context = context;
        }

        public HttpStatusCode Add(Category category)
        {
            context.Categories.Add(category);
            context.SaveChanges();
            return HttpStatusCode.OK;
        }

        public HttpStatusCode Delete(int id)
        {
            var category = GetById(id);
            if (category is null) return HttpStatusCode.NotFound;
            var items = context.Items.Where(item => item.CategoryID == id).ToList();
            foreach(var item in items)
            {
                string fullPath = System.Web.HttpContext.Current.Server.MapPath("~" + item.ImageURL);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            context.Categories.Remove(category);
            context.SaveChanges();
            return HttpStatusCode.OK;
        }

        public List<Category> GetAll()
        {
            return context.Categories.ToList();
        }

        public Category GetById(int id)
        {
            return context.Categories.Find(id);
        }

        public Category GetByName(string name)
        {
            return context.Categories.FirstOrDefault(c => c.Name == name);
        }

        public HttpStatusCode Update(Category category)
        {
            var categoryFind = GetById(category.ID);
            if (categoryFind is null) return HttpStatusCode.NotFound;
            categoryFind.Name = category.Name;
            context.SaveChanges();
            return HttpStatusCode.OK;
        }

        public List<Category> GetAllWithItems()
        {
            return context.Categories.Include(cat => cat.Items).ToList();
        }
    }
}