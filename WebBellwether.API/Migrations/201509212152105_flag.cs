namespace WebBellwether.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class flag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Languages", "LanguageFlag", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Languages", "LanguageFlag");
        }
    }
}
