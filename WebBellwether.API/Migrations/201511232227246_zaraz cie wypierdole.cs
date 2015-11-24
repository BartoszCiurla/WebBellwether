namespace WebBellwether.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zarazciewypierdole : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.LanguageTranslationSpecifications");
        }
        
        public override void Down()
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
    }
}
