namespace SurfergraphyApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init_4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Purchases", "userId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Purchases", "userId");
        }
    }
}
