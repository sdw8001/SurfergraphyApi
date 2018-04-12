namespace SurfergraphyApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class photobuyhistory1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PhotoBuyHistories", "Paid", c => c.Boolean(nullable: false));
            AddColumn("dbo.PhotoBuyHistories", "PaidDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PhotoBuyHistories", "PaidDate");
            DropColumn("dbo.PhotoBuyHistories", "Paid");
        }
    }
}
