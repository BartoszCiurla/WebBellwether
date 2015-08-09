namespace WebBellwether.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Secret = c.String(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        ApplicationType = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        RefreshTokenLifeTime = c.Int(nullable: false),
                        AllowedOrigin = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GameCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GameCategories", t => t.GameCategory_Id)
                .Index(t => t.GameCategory_Id);
            
            CreateTable(
                "dbo.GameFeatureDetailLanguages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameFeatureDetailName = c.String(),
                        LanguageId = c.Int(nullable: false),
                        Language = c.String(),
                        GameFeatureDetail_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GameFeatureDetails", t => t.GameFeatureDetail_Id)
                .Index(t => t.GameFeatureDetail_Id);
            
            CreateTable(
                "dbo.GameFeatureDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameFeature_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GameFeatures", t => t.GameFeature_Id)
                .Index(t => t.GameFeature_Id);
            
            CreateTable(
                "dbo.GameFeatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GameFeatureLanguages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameFeatureName = c.String(),
                        LanguageId = c.Int(nullable: false),
                        Language = c.String(),
                        GameFeature_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GameFeatures", t => t.GameFeature_Id)
                .Index(t => t.GameFeature_Id);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IntegrationGames", t => t.IntegrationGame_Id)
                .Index(t => t.IntegrationGame_Id);
            
            CreateTable(
                "dbo.IntegrationGames",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageName = c.String(),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NumberOfPlayers", t => t.NumberOfPlayer_Id)
                .Index(t => t.NumberOfPlayer_Id);
            
            CreateTable(
                "dbo.NumberOfPlayers",
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PaceOfPlays", t => t.PaceOfPlay_Id)
                .Index(t => t.PaceOfPlay_Id);
            
            CreateTable(
                "dbo.PaceOfPlays",
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PreparationFuns", t => t.PreparationFun_Id)
                .Index(t => t.PreparationFun_Id);
            
            CreateTable(
                "dbo.PreparationFuns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RefreshTokens",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Subject = c.String(nullable: false, maxLength: 50),
                        ClientId = c.String(nullable: false, maxLength: 50),
                        IssuedUtc = c.DateTime(nullable: false),
                        ExpiresUtc = c.DateTime(nullable: false),
                        ProtectedTicket = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.PreparationFunLanguages", "PreparationFun_Id", "dbo.PreparationFuns");
            DropForeignKey("dbo.PaceOfPlayLanguages", "PaceOfPlay_Id", "dbo.PaceOfPlays");
            DropForeignKey("dbo.NumberOfPlayerLanguages", "NumberOfPlayer_Id", "dbo.NumberOfPlayers");
            DropForeignKey("dbo.IntegrationGameLanguages", "IntegrationGame_Id", "dbo.IntegrationGames");
            DropForeignKey("dbo.GameFeatureDetailLanguages", "GameFeatureDetail_Id", "dbo.GameFeatureDetails");
            DropForeignKey("dbo.GameFeatureLanguages", "GameFeature_Id", "dbo.GameFeatures");
            DropForeignKey("dbo.GameFeatureDetails", "GameFeature_Id", "dbo.GameFeatures");
            DropForeignKey("dbo.GameCategoryLanguages", "GameCategory_Id", "dbo.GameCategories");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.PreparationFunLanguages", new[] { "PreparationFun_Id" });
            DropIndex("dbo.PaceOfPlayLanguages", new[] { "PaceOfPlay_Id" });
            DropIndex("dbo.NumberOfPlayerLanguages", new[] { "NumberOfPlayer_Id" });
            DropIndex("dbo.IntegrationGameLanguages", new[] { "IntegrationGame_Id" });
            DropIndex("dbo.GameFeatureLanguages", new[] { "GameFeature_Id" });
            DropIndex("dbo.GameFeatureDetails", new[] { "GameFeature_Id" });
            DropIndex("dbo.GameFeatureDetailLanguages", new[] { "GameFeatureDetail_Id" });
            DropIndex("dbo.GameCategoryLanguages", new[] { "GameCategory_Id" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RefreshTokens");
            DropTable("dbo.PreparationFuns");
            DropTable("dbo.PreparationFunLanguages");
            DropTable("dbo.PaceOfPlays");
            DropTable("dbo.PaceOfPlayLanguages");
            DropTable("dbo.NumberOfPlayers");
            DropTable("dbo.NumberOfPlayerLanguages");
            DropTable("dbo.Languages");
            DropTable("dbo.IntegrationGames");
            DropTable("dbo.IntegrationGameLanguages");
            DropTable("dbo.GameFeatureLanguages");
            DropTable("dbo.GameFeatures");
            DropTable("dbo.GameFeatureDetails");
            DropTable("dbo.GameFeatureDetailLanguages");
            DropTable("dbo.GameCategoryLanguages");
            DropTable("dbo.GameCategories");
            DropTable("dbo.Clients");
        }
    }
}
