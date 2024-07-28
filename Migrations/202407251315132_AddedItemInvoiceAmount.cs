namespace WebShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedItemInvoiceAmount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceItems", "Amount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InvoiceItems", "Amount");
        }
    }
}
