namespace WebBellwether.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gameFeatureVersion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GameFeatureVersionDaos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Version = c.Double(nullable: false),
                        Language_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Language", t => t.Language_Id)
                .Index(t => t.Language_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GameFeatureVersionDaos", "Language_Id", "dbo.Language");
            DropIndex("dbo.GameFeatureVersionDaos", new[] { "Language_Id" });
            DropTable("dbo.GameFeatureVersionDaos");
        }
    }
}
