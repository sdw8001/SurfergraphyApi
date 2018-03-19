namespace SurfergraphyApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class member1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Members", "Grade", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Members", "Grade", c => c.String());
        }
    }
}
