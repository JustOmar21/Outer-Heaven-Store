namespace WebShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBInitalVersion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        IsBoss = c.Boolean(nullable: false),
                        LoginID = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Action = c.String(nullable: false),
                        Created = c.DateTime(nullable: false),
                        AdminID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Admins", t => t.AdminID)
                .Index(t => t.AdminID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        ImageURL = c.String(nullable: false),
                        Amount = c.Long(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.InvoiceItems",
                c => new
                    {
                        InvoiceID = c.Int(nullable: false),
                        ItemID = c.Int(nullable: false),
                        Discount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.InvoiceID, t.ItemID })
                .ForeignKey("dbo.Invoices", t => t.InvoiceID, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.ItemID, cascadeDelete: true)
                .Index(t => t.InvoiceID)
                .Index(t => t.ItemID);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Discount = c.Int(nullable: false),
                        CustomerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customers", t => t.CustomerID, cascadeDelete: true)
                .Index(t => t.CustomerID);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Gender = c.Int(nullable: false),
                        Address = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InvoiceItems", "ItemID", "dbo.Items");
            DropForeignKey("dbo.InvoiceItems", "InvoiceID", "dbo.Invoices");
            DropForeignKey("dbo.Invoices", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.Items", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.Logs", "AdminID", "dbo.Admins");
            DropIndex("dbo.Invoices", new[] { "CustomerID" });
            DropIndex("dbo.InvoiceItems", new[] { "ItemID" });
            DropIndex("dbo.InvoiceItems", new[] { "InvoiceID" });
            DropIndex("dbo.Items", new[] { "CategoryID" });
            DropIndex("dbo.Logs", new[] { "AdminID" });
            DropTable("dbo.Customers");
            DropTable("dbo.Invoices");
            DropTable("dbo.InvoiceItems");
            DropTable("dbo.Items");
            DropTable("dbo.Categories");
            DropTable("dbo.Logs");
            DropTable("dbo.Admins");
        }
    }
}
