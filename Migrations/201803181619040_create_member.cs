namespace SurfergraphyApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_member : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        JoinType = c.String(),
                        LoginToken = c.String(),
                        PushToken = c.String(),
                        Name = c.String(),
                        ImageUrl = c.String(),
                        Grade = c.String(),
                        Wave = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        JoinDate = c.DateTime(nullable: false),
                        LoginDate = c.DateTime(nullable: false),
                        DeletedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Members");
        }
    }
}
