namespace WebBellwether.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class langSpecifycation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LanguageSpecifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageName = c.String(),
                        LanguageFlag = c.String(),
                        YandexTranslateCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LanguageSpecifications");
        }
    }
}
