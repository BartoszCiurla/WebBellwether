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
                "dbo.GameFeatureDetailLanguages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameFeatureDetailName = c.String(),
                        GameFeatureDetail_Id = c.Int(),
                        Language_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GameFeatureDetails", t => t.GameFeatureDetail_Id)
                .ForeignKey("dbo.Languages", t => t.Language_Id)
                .Index(t => t.GameFeatureDetail_Id)
                .Index(t => t.Language_Id);
            
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
                        GameFeature_Id = c.Int(),
                        Language_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GameFeatures", t => t.GameFeature_Id)
                .ForeignKey("dbo.Languages", t => t.Language_Id)
                .Index(t => t.GameFeature_Id)
                .Index(t => t.Language_Id);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IntegrationGameDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IntegrationGameName = c.String(),
                        IntegrationGameDescription = c.String(),
                        IntegrationGame_Id = c.Int(),
                        Language_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IntegrationGames", t => t.IntegrationGame_Id)
                .ForeignKey("dbo.Languages", t => t.Language_Id)
                .Index(t => t.IntegrationGame_Id)
                .Index(t => t.Language_Id);
            
            CreateTable(
                "dbo.IntegrationGames",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IntegrationGameFeatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameFeatureDetailLanguage_Id = c.Int(),
                        GameFeatureLanguage_Id = c.Int(),
                        IntegrationGameDetail_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GameFeatureDetailLanguages", t => t.GameFeatureDetailLanguage_Id)
                .ForeignKey("dbo.GameFeatureLanguages", t => t.GameFeatureLanguage_Id)
                .ForeignKey("dbo.IntegrationGameDetails", t => t.IntegrationGameDetail_Id)
                .Index(t => t.GameFeatureDetailLanguage_Id)
                .Index(t => t.GameFeatureLanguage_Id)
                .Index(t => t.IntegrationGameDetail_Id);
            
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
            DropForeignKey("dbo.IntegrationGameDetails", "Language_Id", "dbo.Languages");
            DropForeignKey("dbo.IntegrationGameFeatures", "IntegrationGameDetail_Id", "dbo.IntegrationGameDetails");
            DropForeignKey("dbo.IntegrationGameFeatures", "GameFeatureLanguage_Id", "dbo.GameFeatureLanguages");
            DropForeignKey("dbo.IntegrationGameFeatures", "GameFeatureDetailLanguage_Id", "dbo.GameFeatureDetailLanguages");
            DropForeignKey("dbo.IntegrationGameDetails", "IntegrationGame_Id", "dbo.IntegrationGames");
            DropForeignKey("dbo.GameFeatureDetailLanguages", "Language_Id", "dbo.Languages");
            DropForeignKey("dbo.GameFeatureDetailLanguages", "GameFeatureDetail_Id", "dbo.GameFeatureDetails");
            DropForeignKey("dbo.GameFeatureLanguages", "Language_Id", "dbo.Languages");
            DropForeignKey("dbo.GameFeatureLanguages", "GameFeature_Id", "dbo.GameFeatures");
            DropForeignKey("dbo.GameFeatureDetails", "GameFeature_Id", "dbo.GameFeatures");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.IntegrationGameFeatures", new[] { "IntegrationGameDetail_Id" });
            DropIndex("dbo.IntegrationGameFeatures", new[] { "GameFeatureLanguage_Id" });
            DropIndex("dbo.IntegrationGameFeatures", new[] { "GameFeatureDetailLanguage_Id" });
            DropIndex("dbo.IntegrationGameDetails", new[] { "Language_Id" });
            DropIndex("dbo.IntegrationGameDetails", new[] { "IntegrationGame_Id" });
            DropIndex("dbo.GameFeatureLanguages", new[] { "Language_Id" });
            DropIndex("dbo.GameFeatureLanguages", new[] { "GameFeature_Id" });
            DropIndex("dbo.GameFeatureDetails", new[] { "GameFeature_Id" });
            DropIndex("dbo.GameFeatureDetailLanguages", new[] { "Language_Id" });
            DropIndex("dbo.GameFeatureDetailLanguages", new[] { "GameFeatureDetail_Id" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RefreshTokens");
            DropTable("dbo.IntegrationGameFeatures");
            DropTable("dbo.IntegrationGames");
            DropTable("dbo.IntegrationGameDetails");
            DropTable("dbo.Languages");
            DropTable("dbo.GameFeatureLanguages");
            DropTable("dbo.GameFeatures");
            DropTable("dbo.GameFeatureDetails");
            DropTable("dbo.GameFeatureDetailLanguages");
            DropTable("dbo.Clients");
        }
    }
}
