namespace WebBellwether.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Client",
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
                "dbo.GameFeatureDetailLanguage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameFeatureDetailName = c.String(),
                        GameFeatureDetail_Id = c.Int(),
                        Language_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GameFeatureDetail", t => t.GameFeatureDetail_Id)
                .ForeignKey("dbo.Language", t => t.Language_Id)
                .Index(t => t.GameFeatureDetail_Id)
                .Index(t => t.Language_Id);
            
            CreateTable(
                "dbo.GameFeatureDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameFeature_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GameFeature", t => t.GameFeature_Id)
                .Index(t => t.GameFeature_Id);
            
            CreateTable(
                "dbo.GameFeature",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GameFeatureLanguage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameFeatureName = c.String(),
                        GameFeature_Id = c.Int(),
                        Language_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GameFeature", t => t.GameFeature_Id)
                .ForeignKey("dbo.Language", t => t.Language_Id)
                .Index(t => t.GameFeature_Id)
                .Index(t => t.Language_Id);
            
            CreateTable(
                "dbo.Language",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageName = c.String(),
                        LanguageShortName = c.String(),
                        IsPublic = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IntegrationGameDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IntegrationGameName = c.String(),
                        IntegrationGameDescription = c.String(),
                        IntegrationGame_Id = c.Int(),
                        Language_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IntegrationGame", t => t.IntegrationGame_Id)
                .ForeignKey("dbo.Language", t => t.Language_Id)
                .Index(t => t.IntegrationGame_Id)
                .Index(t => t.Language_Id);
            
            CreateTable(
                "dbo.IntegrationGame",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IntegrationGameFeature",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameFeatureDetailLanguage_Id = c.Int(),
                        GameFeatureLanguage_Id = c.Int(),
                        IntegrationGameDetail_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GameFeatureDetailLanguage", t => t.GameFeatureDetailLanguage_Id)
                .ForeignKey("dbo.GameFeatureLanguage", t => t.GameFeatureLanguage_Id)
                .ForeignKey("dbo.IntegrationGameDetail", t => t.IntegrationGameDetail_Id)
                .Index(t => t.GameFeatureDetailLanguage_Id)
                .Index(t => t.GameFeatureLanguage_Id)
                .Index(t => t.IntegrationGameDetail_Id);
            
            CreateTable(
                "dbo.IntegrationGameVersion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Version = c.Double(nullable: false),
                        NumberOfIntegrationGames = c.Int(nullable: false),
                        Language_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Language", t => t.Language_Id)
                .Index(t => t.Language_Id);
            
            CreateTable(
                "dbo.JokeCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.JokeCategoryDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JokeCategoryName = c.String(),
                        JokeCategory_Id = c.Int(),
                        Language_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JokeCategory", t => t.JokeCategory_Id)
                .ForeignKey("dbo.Language", t => t.Language_Id)
                .Index(t => t.JokeCategory_Id)
                .Index(t => t.Language_Id);
            
            CreateTable(
                "dbo.JokeCategoryVersion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Version = c.Double(nullable: false),
                        NumberOfJokeCategory = c.Int(nullable: false),
                        Language_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Language", t => t.Language_Id)
                .Index(t => t.Language_Id);
            
            CreateTable(
                "dbo.JokeDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JokeContent = c.String(),
                        Joke_Id = c.Int(),
                        JokeCategoryDetail_Id = c.Int(),
                        Language_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Joke", t => t.Joke_Id)
                .ForeignKey("dbo.JokeCategoryDetail", t => t.JokeCategoryDetail_Id)
                .ForeignKey("dbo.Language", t => t.Language_Id)
                .Index(t => t.Joke_Id)
                .Index(t => t.JokeCategoryDetail_Id)
                .Index(t => t.Language_Id);
            
            CreateTable(
                "dbo.Joke",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.JokeVersion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Version = c.Double(nullable: false),
                        NumberOfJokes = c.Int(nullable: false),
                        Language_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Language", t => t.Language_Id)
                .Index(t => t.Language_Id);
            
            CreateTable(
                "dbo.LanguageVersion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Version = c.Double(nullable: false),
                        NumberOfItemsInFileLanguage = c.Int(nullable: false),
                        Language_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Language", t => t.Language_Id)
                .Index(t => t.Language_Id);
            
            CreateTable(
                "dbo.RefreshToken",
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
            DropForeignKey("dbo.LanguageVersion", "Language_Id", "dbo.Language");
            DropForeignKey("dbo.JokeVersion", "Language_Id", "dbo.Language");
            DropForeignKey("dbo.JokeDetail", "Language_Id", "dbo.Language");
            DropForeignKey("dbo.JokeDetail", "JokeCategoryDetail_Id", "dbo.JokeCategoryDetail");
            DropForeignKey("dbo.JokeDetail", "Joke_Id", "dbo.Joke");
            DropForeignKey("dbo.JokeCategoryVersion", "Language_Id", "dbo.Language");
            DropForeignKey("dbo.JokeCategoryDetail", "Language_Id", "dbo.Language");
            DropForeignKey("dbo.JokeCategoryDetail", "JokeCategory_Id", "dbo.JokeCategory");
            DropForeignKey("dbo.IntegrationGameVersion", "Language_Id", "dbo.Language");
            DropForeignKey("dbo.IntegrationGameDetail", "Language_Id", "dbo.Language");
            DropForeignKey("dbo.IntegrationGameFeature", "IntegrationGameDetail_Id", "dbo.IntegrationGameDetail");
            DropForeignKey("dbo.IntegrationGameFeature", "GameFeatureLanguage_Id", "dbo.GameFeatureLanguage");
            DropForeignKey("dbo.IntegrationGameFeature", "GameFeatureDetailLanguage_Id", "dbo.GameFeatureDetailLanguage");
            DropForeignKey("dbo.IntegrationGameDetail", "IntegrationGame_Id", "dbo.IntegrationGame");
            DropForeignKey("dbo.GameFeatureDetailLanguage", "Language_Id", "dbo.Language");
            DropForeignKey("dbo.GameFeatureDetailLanguage", "GameFeatureDetail_Id", "dbo.GameFeatureDetail");
            DropForeignKey("dbo.GameFeatureLanguage", "Language_Id", "dbo.Language");
            DropForeignKey("dbo.GameFeatureLanguage", "GameFeature_Id", "dbo.GameFeature");
            DropForeignKey("dbo.GameFeatureDetail", "GameFeature_Id", "dbo.GameFeature");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.LanguageVersion", new[] { "Language_Id" });
            DropIndex("dbo.JokeVersion", new[] { "Language_Id" });
            DropIndex("dbo.JokeDetail", new[] { "Language_Id" });
            DropIndex("dbo.JokeDetail", new[] { "JokeCategoryDetail_Id" });
            DropIndex("dbo.JokeDetail", new[] { "Joke_Id" });
            DropIndex("dbo.JokeCategoryVersion", new[] { "Language_Id" });
            DropIndex("dbo.JokeCategoryDetail", new[] { "Language_Id" });
            DropIndex("dbo.JokeCategoryDetail", new[] { "JokeCategory_Id" });
            DropIndex("dbo.IntegrationGameVersion", new[] { "Language_Id" });
            DropIndex("dbo.IntegrationGameFeature", new[] { "IntegrationGameDetail_Id" });
            DropIndex("dbo.IntegrationGameFeature", new[] { "GameFeatureLanguage_Id" });
            DropIndex("dbo.IntegrationGameFeature", new[] { "GameFeatureDetailLanguage_Id" });
            DropIndex("dbo.IntegrationGameDetail", new[] { "Language_Id" });
            DropIndex("dbo.IntegrationGameDetail", new[] { "IntegrationGame_Id" });
            DropIndex("dbo.GameFeatureLanguage", new[] { "Language_Id" });
            DropIndex("dbo.GameFeatureLanguage", new[] { "GameFeature_Id" });
            DropIndex("dbo.GameFeatureDetail", new[] { "GameFeature_Id" });
            DropIndex("dbo.GameFeatureDetailLanguage", new[] { "Language_Id" });
            DropIndex("dbo.GameFeatureDetailLanguage", new[] { "GameFeatureDetail_Id" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RefreshToken");
            DropTable("dbo.LanguageVersion");
            DropTable("dbo.JokeVersion");
            DropTable("dbo.Joke");
            DropTable("dbo.JokeDetail");
            DropTable("dbo.JokeCategoryVersion");
            DropTable("dbo.JokeCategoryDetail");
            DropTable("dbo.JokeCategory");
            DropTable("dbo.IntegrationGameVersion");
            DropTable("dbo.IntegrationGameFeature");
            DropTable("dbo.IntegrationGame");
            DropTable("dbo.IntegrationGameDetail");
            DropTable("dbo.Language");
            DropTable("dbo.GameFeatureLanguage");
            DropTable("dbo.GameFeature");
            DropTable("dbo.GameFeatureDetail");
            DropTable("dbo.GameFeatureDetailLanguage");
            DropTable("dbo.Client");
        }
    }
}
