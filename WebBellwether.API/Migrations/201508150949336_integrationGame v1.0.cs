namespace WebBellwether.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class integrationGamev10 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.IntegrationGameLanguages", "IntegrationGame_Id", "dbo.IntegrationGames");
            DropIndex("dbo.IntegrationGameLanguages", new[] { "IntegrationGame_Id" });
            CreateTable(
                "dbo.IntegrationGameDetailLanguages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Language = c.String(),
                        LanguageId = c.Int(nullable: false),
                        IntegrationGameDetailLanguageName = c.String(),
                        IntegrationGameDetail_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IntegrationGameDetails", t => t.IntegrationGameDetail_Id)
                .Index(t => t.IntegrationGameDetail_Id);
            
            CreateTable(
                "dbo.IntegrationGameDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IntegrationGame_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IntegrationGames", t => t.IntegrationGame_Id)
                .Index(t => t.IntegrationGame_Id);
            
            DropTable("dbo.IntegrationGameLanguages");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.IntegrationGameLanguages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameName = c.String(),
                        GameDetails = c.String(),
                        GameCategoryId = c.Int(nullable: false),
                        GameCategory = c.String(),
                        PaceOfPlayId = c.Int(nullable: false),
                        PaceOfPlay = c.String(),
                        NumberOfPlayerId = c.Int(nullable: false),
                        NumberOfPlayer = c.String(),
                        PreparationFunId = c.Int(nullable: false),
                        PreparationFun = c.String(),
                        LanguageId = c.Int(nullable: false),
                        Language = c.String(),
                        IntegrationGame_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.IntegrationGameDetailLanguages", "IntegrationGameDetail_Id", "dbo.IntegrationGameDetails");
            DropForeignKey("dbo.IntegrationGameDetails", "IntegrationGame_Id", "dbo.IntegrationGames");
            DropIndex("dbo.IntegrationGameDetails", new[] { "IntegrationGame_Id" });
            DropIndex("dbo.IntegrationGameDetailLanguages", new[] { "IntegrationGameDetail_Id" });
            DropTable("dbo.IntegrationGameDetails");
            DropTable("dbo.IntegrationGameDetailLanguages");
            CreateIndex("dbo.IntegrationGameLanguages", "IntegrationGame_Id");
            AddForeignKey("dbo.IntegrationGameLanguages", "IntegrationGame_Id", "dbo.IntegrationGames", "Id");
        }
    }
}
