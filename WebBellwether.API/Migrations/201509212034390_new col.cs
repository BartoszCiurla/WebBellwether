namespace WebBellwether.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newcol : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Languages", "LanguageShortName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Languages", "LanguageShortName");
        }
    }
}
