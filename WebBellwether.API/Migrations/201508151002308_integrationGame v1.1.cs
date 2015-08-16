namespace WebBellwether.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class integrationGamev11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IntegrationGameDetailLanguages", "GameFeatureDetailLanguage_Id", c => c.Int());
            AddColumn("dbo.IntegrationGameDetailLanguages", "GameFeatureLanguage_Id", c => c.Int());
            CreateIndex("dbo.IntegrationGameDetailLanguages", "GameFeatureDetailLanguage_Id");
            CreateIndex("dbo.IntegrationGameDetailLanguages", "GameFeatureLanguage_Id");
            AddForeignKey("dbo.IntegrationGameDetailLanguages", "GameFeatureDetailLanguage_Id", "dbo.GameFeatureDetailLanguages", "Id");
            AddForeignKey("dbo.IntegrationGameDetailLanguages", "GameFeatureLanguage_Id", "dbo.GameFeatureLanguages", "Id");
            DropColumn("dbo.IntegrationGameDetailLanguages", "IntegrationGameDetailLanguageName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.IntegrationGameDetailLanguages", "IntegrationGameDetailLanguageName", c => c.String());
            DropForeignKey("dbo.IntegrationGameDetailLanguages", "GameFeatureLanguage_Id", "dbo.GameFeatureLanguages");
            DropForeignKey("dbo.IntegrationGameDetailLanguages", "GameFeatureDetailLanguage_Id", "dbo.GameFeatureDetailLanguages");
            DropIndex("dbo.IntegrationGameDetailLanguages", new[] { "GameFeatureLanguage_Id" });
            DropIndex("dbo.IntegrationGameDetailLanguages", new[] { "GameFeatureDetailLanguage_Id" });
            DropColumn("dbo.IntegrationGameDetailLanguages", "GameFeatureLanguage_Id");
            DropColumn("dbo.IntegrationGameDetailLanguages", "GameFeatureDetailLanguage_Id");
        }
    }
}
