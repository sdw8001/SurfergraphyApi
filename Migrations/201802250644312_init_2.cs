namespace SurfergraphyApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init_2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LikePhotoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        PhotoId = c.Int(nullable: false),
                        PhotoBuyHistoryId = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LikePhotoes");
        }
    }
}
