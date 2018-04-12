namespace SurfergraphyApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class photo1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "TotalCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Photos", "TotalCount");
        }
    }
}
