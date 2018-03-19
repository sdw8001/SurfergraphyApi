namespace SurfergraphyApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_photo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "Valid", c => c.Boolean(nullable: false));
            AddColumn("dbo.Photos", "Expired", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Photos", "Expired");
            DropColumn("dbo.Photos", "Valid");
        }
    }
}
