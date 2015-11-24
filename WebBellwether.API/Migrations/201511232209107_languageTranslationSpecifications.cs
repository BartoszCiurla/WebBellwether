namespace WebBellwether.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class languageTranslationSpecifications : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LanguageTranslationSpecifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageName = c.String(),
                        LanguageFlag = c.String(),
                        YandexTranslationCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LanguageTranslationSpecifications");
        }
    }
}
