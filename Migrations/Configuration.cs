namespace WebShopping.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using WebShopping.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WebShopping.Models.StoreEntity>
    {
        private readonly bool _pendingMigrations;
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            var migrator = new DbMigrator(this);
            _pendingMigrations = migrator.GetPendingMigrations().Any();
        }

        protected override void Seed(StoreEntity context)
        {

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            
            try
            {
                // Your code...
                // Could also be before try if you know the exception occurs in SaveChanges
                if(context.Admins.ToList().Count == 0)
                {
                    context.Admins.AddOrUpdate(
                        new Admin()
                        {
                            Email = "masteradmin@gmail.com",
                            IsBoss = true,
                            Name = "Master",
                            Password = SimpleEncryptor.Encrypt("13589100sS"),
                            Logs = null,
                            LoginID = Guid.NewGuid().ToString()
                        }
                    );
                }
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
    }
}
