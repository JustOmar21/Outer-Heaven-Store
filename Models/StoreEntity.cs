using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace WebShopping.Models
{
    public class StoreEntity : DbContext
    {
        public StoreEntity() : base("Conn")
        {
            this.Database.Log = Console.Write;
        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItems> InvoicesItems { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Log> Log { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InvoiceItems>().HasKey(II => new { II.InvoiceID, II.ItemID });
            modelBuilder.Entity<Item>()
                .HasRequired(i=> i.Category)
                .WithMany(cat => cat.Items)
                .HasForeignKey(i => i.CategoryID)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Log>()
                .HasOptional(log => log.Admin)
                .WithMany(admin => admin.Logs)
                .HasForeignKey(log => log.AdminID);

            modelBuilder.Entity<Invoice>()
                .HasRequired(inv => inv.Customer)
                .WithMany(cus => cus.Invoices)
                .HasForeignKey(inv => inv.CustomerID)
                .WillCascadeOnDelete();

            modelBuilder.Entity<InvoiceItems>()
                .HasRequired(invItem => invItem.Invoice)
                .WithMany(inv => inv.Items)
                .HasForeignKey(invItem=> invItem.InvoiceID)
                .WillCascadeOnDelete();

            modelBuilder.Entity<InvoiceItems>()
                .HasRequired(invItem => invItem.Item)
                .WithMany(it => it.InvoicesItems)
                .HasForeignKey(invItem => invItem.ItemID)
                .WillCascadeOnDelete();




            modelBuilder.Conventions.Add(new DecimalPropertyConvention(18, 2));
        }
    }
}