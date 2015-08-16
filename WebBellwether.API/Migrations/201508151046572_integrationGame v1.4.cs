namespace WebBellwether.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class integrationGamev14 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.IntegrationGameDetailLanguages", newName: "IntegrationGameFeatures");
            AddColumn("dbo.IntegrationGameDetails", "Language_Id", c => c.Int());
            CreateIndex("dbo.IntegrationGameDetails", "Language_Id");
            AddForeignKey("dbo.IntegrationGameDetails", "Language_Id", "dbo.Languages", "Id");
            DropColumn("dbo.IntegrationGameDetails", "Language");
            DropColumn("dbo.IntegrationGameDetails", "LanguageId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.IntegrationGameDetails", "LanguageId", c => c.Int(nullable: false));
            AddColumn("dbo.IntegrationGameDetails", "Language", c => c.String());
            DropForeignKey("dbo.IntegrationGameDetails", "Language_Id", "dbo.Languages");
            DropIndex("dbo.IntegrationGameDetails", new[] { "Language_Id" });
            DropColumn("dbo.IntegrationGameDetails", "Language_Id");
            RenameTable(name: "dbo.IntegrationGameFeatures", newName: "IntegrationGameDetailLanguages");
        }
    }
}
