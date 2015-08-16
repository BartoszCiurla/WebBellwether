namespace WebBellwether.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class integrationGamev13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IntegrationGameDetails", "IntegrationGameDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.IntegrationGameDetails", "IntegrationGameDescription");
        }
    }
}
