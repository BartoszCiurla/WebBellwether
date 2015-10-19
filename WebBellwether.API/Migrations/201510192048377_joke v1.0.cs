namespace WebBellwether.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jokev10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JokeCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.JokeCategoryDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JokeCategoryName = c.String(),
                        JokeCategory_Id = c.Int(),
                        Language_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JokeCategories", t => t.JokeCategory_Id)
                .ForeignKey("dbo.Languages", t => t.Language_Id)
                .Index(t => t.JokeCategory_Id)
                .Index(t => t.Language_Id);
            
            CreateTable(
                "dbo.JokeDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JokeContent = c.String(),
                        Joke_Id = c.Int(),
                        Language_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Jokes", t => t.Joke_Id)
                .ForeignKey("dbo.Languages", t => t.Language_Id)
                .Index(t => t.Joke_Id)
                .Index(t => t.Language_Id);
            
            CreateTable(
                "dbo.Jokes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JokeDetails", "Language_Id", "dbo.Languages");
            DropForeignKey("dbo.JokeDetails", "Joke_Id", "dbo.Jokes");
            DropForeignKey("dbo.JokeCategoryDetails", "Language_Id", "dbo.Languages");
            DropForeignKey("dbo.JokeCategoryDetails", "JokeCategory_Id", "dbo.JokeCategories");
            DropIndex("dbo.JokeDetails", new[] { "Language_Id" });
            DropIndex("dbo.JokeDetails", new[] { "Joke_Id" });
            DropIndex("dbo.JokeCategoryDetails", new[] { "Language_Id" });
            DropIndex("dbo.JokeCategoryDetails", new[] { "JokeCategory_Id" });
            DropTable("dbo.Jokes");
            DropTable("dbo.JokeDetails");
            DropTable("dbo.JokeCategoryDetails");
            DropTable("dbo.JokeCategories");
        }
    }
}
