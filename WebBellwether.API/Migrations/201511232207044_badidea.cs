namespace WebBellwether.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class badidea : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.LanguageSpecifications");
        }
        
        public override void Down()
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
    }
}
