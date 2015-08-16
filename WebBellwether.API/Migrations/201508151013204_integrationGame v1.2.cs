namespace WebBellwether.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class integrationGamev12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IntegrationGameDetails", "IntegrationGameName", c => c.String());
            AddColumn("dbo.IntegrationGameDetails", "Language", c => c.String());
            AddColumn("dbo.IntegrationGameDetails", "LanguageId", c => c.Int(nullable: false));
            DropColumn("dbo.IntegrationGameDetailLanguages", "Language");
            DropColumn("dbo.IntegrationGameDetailLanguages", "LanguageId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.IntegrationGameDetailLanguages", "LanguageId", c => c.Int(nullable: false));
            AddColumn("dbo.IntegrationGameDetailLanguages", "Language", c => c.String());
            DropColumn("dbo.IntegrationGameDetails", "LanguageId");
            DropColumn("dbo.IntegrationGameDetails", "Language");
            DropColumn("dbo.IntegrationGameDetails", "IntegrationGameName");
        }
    }
}
