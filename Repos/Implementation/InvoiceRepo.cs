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
    public class InvoiceRepo : IInvoiceRepo
    {
        private readonly StoreEntity context;
        public InvoiceRepo(StoreEntity context)
        {
            this.context = context;
        }
        public HttpStatusCode Add(Invoice invoice)
        {
            context.Invoices.Add(invoice);
            context.SaveChanges();
            return HttpStatusCode.OK;
        }

        public HttpStatusCode AddItem(InvoiceItems invItem)
        {
            var item = context.Items.FirstOrDefault(it => it.ID == invItem.ItemID);
            var invoiceItemRecord = context.InvoicesItems.FirstOrDefault(it => it.ItemID == invItem.ItemID && it.InvoiceID == invItem.InvoiceID);
            if (invoiceItemRecord is null)
            {
                if(item.Amount >= invItem.Amount)
                {
                    var invoiceItem = new InvoiceItems() { Discount = invItem.Discount, ItemID = invItem.ItemID, InvoiceID = invItem.InvoiceID, Amount = invItem.Amount };
                    context.InvoicesItems.Add(invoiceItem);
                    item.Amount -= invItem.Amount;
                }
                else
                {
                    return HttpStatusCode.RequestEntityTooLarge;
                }
            }
            else
            {
                return HttpStatusCode.Forbidden;
            }
            context.SaveChanges();
            return HttpStatusCode.OK;
        }

        public HttpStatusCode DecreaseItem(int invoiceID, int itemID)
        {
            var item = context.Items.FirstOrDefault(it => it.ID == itemID);
            var invoiceItemRecord = context.InvoicesItems.FirstOrDefault(it => it.ItemID == itemID && it.InvoiceID == invoiceID);
            if (invoiceItemRecord is null)
            {
                return HttpStatusCode.NotFound;
            }
            else
            {
                if (invoiceItemRecord.Amount > 1)
                {
                    invoiceItemRecord.Amount--;
                    item.Amount++;
                }
                else
                {
                    item.Amount++;
                    return DeleteItem(invoiceID, itemID);
                }
            }
            context.SaveChanges();
            return HttpStatusCode.OK;
        }

        public HttpStatusCode Delete(int id)
        {
            var invoice = context.Invoices.Include(inv=>inv.Items.Select(x=>x.Item)).FirstOrDefault(it => it.ID == id);
            if (invoice is null) return HttpStatusCode.NotFound;
            foreach(var item in invoice.Items)
            {
                item.Item.Amount += item.Amount;
            }
            context.Invoices.Remove(invoice);
            context.SaveChanges();
            return HttpStatusCode.OK;
        }

        public HttpStatusCode DeleteItem(int invoiceID, int itemID)
        {
            var item = context.Items.FirstOrDefault(it => it.ID == itemID);
            var invoiceItemRecord = context.InvoicesItems.FirstOrDefault(it => it.ItemID == itemID && it.InvoiceID == invoiceID);
            if (invoiceItemRecord is null)
            {
                return HttpStatusCode.NotFound;
            }
            item.Amount += invoiceItemRecord.Amount;
            context.InvoicesItems.Remove(invoiceItemRecord);
            context.SaveChanges();
            return HttpStatusCode.OK;
        }

        public HttpStatusCode DiscountInvoice(int invoiceID, int discount)
        {
            var invoice = context.Invoices.FirstOrDefault(it => it.ID == invoiceID);
            if (invoice is null) return HttpStatusCode.NotFound;
            invoice.Discount = discount;
            context.SaveChanges();
            return HttpStatusCode.OK;
        }

        public HttpStatusCode DiscountItem(int invoiceID, int itemID , int discount)
        {
            var item = context.Items.FirstOrDefault(it => it.ID == itemID);
            var invoiceItemRecord = context.InvoicesItems.FirstOrDefault(it => it.ItemID == itemID && it.InvoiceID == invoiceID);
            if (invoiceItemRecord is null) return HttpStatusCode.NotFound;
            invoiceItemRecord.Discount = discount;
            context.SaveChanges();
            return HttpStatusCode.OK;
        }

        public List<Invoice> GetAll()
        {
            return context.Invoices.Include(inv=>inv.Customer).Include(inv=>inv.Items).ToList();
        }

        public List<Invoice> GetAllByUserId(int userID)
        {
            return context.Invoices.Where(inv => inv.CustomerID == userID).ToList();
        }
        public Invoice GetSingleInvoiceWItems(int invoiceID)
        {
            return context.Invoices.Include(inv => inv.Items.Select(x=>x.Item)).Include(inv=>inv.Customer).SingleOrDefault(inv => inv.ID == invoiceID);
        }
        public InvoiceItems GetInvoiceItemByID(int invoiceID, int itemID)
        {
            return context.InvoicesItems.Include(invItem=>invItem.Item).SingleOrDefault(invItem => invItem.ItemID == itemID && invItem.InvoiceID == invoiceID);
        }

        public Invoice GetById(int id)
        {
            return context.Invoices.Include(inv=> inv.Items).SingleOrDefault(inv =>  inv.ID == id);
        }

        public HttpStatusCode Update(Invoice invoice)
        {
            var invoiceRecord = GetById(invoice.ID);
            if(invoiceRecord is null) return HttpStatusCode.NotFound;
            invoiceRecord.Discount = invoice.Discount;
            invoiceRecord.Date = invoice.Date;
            invoiceRecord.CustomerID = invoice.CustomerID;
            context.SaveChanges();
            return HttpStatusCode.OK;
        }

        public HttpStatusCode UpdateItem(InvoiceItems invItem)
        {
            var item = context.Items.FirstOrDefault(it => it.ID == invItem.ItemID);
            var invoiceItemRecord = context.InvoicesItems.FirstOrDefault(it => it.ItemID == invItem.ItemID && it.InvoiceID == invItem.InvoiceID);
            if (invoiceItemRecord != null)
            {
                item.Amount += invoiceItemRecord.Amount;
                if (item.Amount >= invItem.Amount)
                {
                    invoiceItemRecord.Amount = invItem.Amount;
                    invoiceItemRecord.Discount = invItem.Discount;
                    item.Amount -= invItem.Amount;
                }
                else
                {
                    return HttpStatusCode.RequestEntityTooLarge;
                }
            }
            else
            {
                return HttpStatusCode.NotFound;
            }
            context.SaveChanges();
            return HttpStatusCode.OK;
        }
    }
}