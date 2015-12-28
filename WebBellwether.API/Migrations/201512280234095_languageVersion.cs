namespace WebBellwether.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class languageVersion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Languages", "LanguageVersion", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Languages", "LanguageVersion");
        }
    }
}
