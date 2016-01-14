namespace WebBellwether.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gfv2 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.GameFeatureVersionDaos", newName: "GameFeatureVersion");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.GameFeatureVersion", newName: "GameFeatureVersionDaos");
        }
    }
}
