using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShopping.Models;
using WebShopping.Repos.Implementation;
using WebShopping.Repos.Interface;

namespace WebShopping.App_Start
{
    public class AutofacConfig
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register Database
            builder.RegisterType<StoreEntity>().InstancePerRequest();

            // Register repos
            builder.RegisterType<CustomerRepo>().As<ICustomerRepo>();
            builder.RegisterType<CategoryRepo>().As<ICategoryRepo>();
            builder.RegisterType<ItemRepo>().As<IItemRepo>();
            builder.RegisterType<InvoiceRepo>().As<IInvoiceRepo>();
            builder.RegisterType<AdminRepo>().As<IAdminRepo>();
            builder.RegisterType<AccountRepo>().As<IAccountRepo>();

            // Build the container.
            var container = builder.Build();

            // Set the dependency resolver for MVC.
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}