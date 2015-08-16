namespace WebBellwether.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GameCategoryLanguages", "GameCategory_Id", "dbo.GameCategories");
            DropForeignKey("dbo.NumberOfPlayerLanguages", "NumberOfPlayer_Id", "dbo.NumberOfPlayers");
            DropForeignKey("dbo.PaceOfPlayLanguages", "PaceOfPlay_Id", "dbo.PaceOfPlays");
            DropForeignKey("dbo.PreparationFunLanguages", "PreparationFun_Id", "dbo.PreparationFuns");
            DropIndex("dbo.GameCategoryLanguages", new[] { "GameCategory_Id" });
            DropIndex("dbo.NumberOfPlayerLanguages", new[] { "NumberOfPlayer_Id" });
            DropIndex("dbo.PaceOfPlayLanguages", new[] { "PaceOfPlay_Id" });
            DropIndex("dbo.PreparationFunLanguages", new[] { "PreparationFun_Id" });
            DropTable("dbo.GameCategories");
            DropTable("dbo.GameCategoryLanguages");
            DropTable("dbo.NumberOfPlayerLanguages");
            DropTable("dbo.NumberOfPlayers");
            DropTable("dbo.PaceOfPlayLanguages");
            DropTable("dbo.PaceOfPlays");
            DropTable("dbo.PreparationFunLanguages");
            DropTable("dbo.PreparationFuns");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PreparationFuns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PreparationFunLanguages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PreparationFunName = c.String(),
                        LanguageId = c.Int(nullable: false),
                        Language = c.String(),
                        PreparationFun_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PaceOfPlays",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PaceOfPlayLanguages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PaceOfPlayName = c.String(),
                        LanguageId = c.Int(nullable: false),
                        Language = c.String(),
                        PaceOfPlay_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NumberOfPlayers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NumberOfPlayerLanguages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumberOfPlayerName = c.String(),
                        LanguageId = c.Int(nullable: false),
                        Language = c.String(),
                        NumberOfPlayer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GameCategoryLanguages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameCategoryName = c.String(),
                        LanguageId = c.Int(nullable: false),
                        Language = c.String(),
                        GameCategory_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GameCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.PreparationFunLanguages", "PreparationFun_Id");
            CreateIndex("dbo.PaceOfPlayLanguages", "PaceOfPlay_Id");
            CreateIndex("dbo.NumberOfPlayerLanguages", "NumberOfPlayer_Id");
            CreateIndex("dbo.GameCategoryLanguages", "GameCategory_Id");
            AddForeignKey("dbo.PreparationFunLanguages", "PreparationFun_Id", "dbo.PreparationFuns", "Id");
            AddForeignKey("dbo.PaceOfPlayLanguages", "PaceOfPlay_Id", "dbo.PaceOfPlays", "Id");
            AddForeignKey("dbo.NumberOfPlayerLanguages", "NumberOfPlayer_Id", "dbo.NumberOfPlayers", "Id");
            AddForeignKey("dbo.GameCategoryLanguages", "GameCategory_Id", "dbo.GameCategories", "Id");
        }
    }
}
