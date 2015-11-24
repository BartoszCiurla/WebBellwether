namespace WebBellwether.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class flagdown : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Languages", "LanguageFlag");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Languages", "LanguageFlag", c => c.String());
        }
    }
}
