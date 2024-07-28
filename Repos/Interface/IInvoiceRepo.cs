using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebShopping.Models;

namespace WebShopping.Repos.Interface
{
    public interface IInvoiceRepo
    {
        List<Invoice> GetAll();
        Invoice GetById(int id);
        List<Invoice> GetAllByUserId(int userID);
        Invoice GetSingleInvoiceWItems(int invoiceID);
        InvoiceItems GetInvoiceItemByID(int invoiceID, int itemID);
        HttpStatusCode Add(Invoice invoice);
        HttpStatusCode AddItem(InvoiceItems invItem);
        HttpStatusCode DecreaseItem(int invoiceID , int itemID);
        HttpStatusCode DeleteItem(int invoiceID,int itemID);
        HttpStatusCode DiscountItem(int invoiceID,int itemID, int discount);
        HttpStatusCode DiscountInvoice(int invoiceID, int discount);
        HttpStatusCode UpdateItem(InvoiceItems invItem);
        HttpStatusCode Update(Invoice invoice);
        HttpStatusCode Delete(int id);

    }
}
