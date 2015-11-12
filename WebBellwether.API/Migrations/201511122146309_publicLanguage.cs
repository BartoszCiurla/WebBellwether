namespace WebBellwether.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class publicLanguage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Languages", "IsPublic", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Languages", "IsPublic");
        }
    }
}
