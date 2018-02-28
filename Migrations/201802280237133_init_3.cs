namespace SurfergraphyApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init_3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Purchases", "ItemType", c => c.String());
            AddColumn("dbo.Purchases", "Sku", c => c.String());
            AddColumn("dbo.Purchases", "OriginalJson", c => c.String());
            AddColumn("dbo.Purchases", "Signature", c => c.String());
            DropColumn("dbo.Purchases", "ProductId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Purchases", "ProductId", c => c.String());
            DropColumn("dbo.Purchases", "Signature");
            DropColumn("dbo.Purchases", "OriginalJson");
            DropColumn("dbo.Purchases", "Sku");
            DropColumn("dbo.Purchases", "ItemType");
        }
    }
}
