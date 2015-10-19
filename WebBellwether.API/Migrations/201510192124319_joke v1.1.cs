namespace WebBellwether.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jokev11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JokeDetails", "JokeCategoryDetail_Id", c => c.Int());
            CreateIndex("dbo.JokeDetails", "JokeCategoryDetail_Id");
            AddForeignKey("dbo.JokeDetails", "JokeCategoryDetail_Id", "dbo.JokeCategoryDetails", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JokeDetails", "JokeCategoryDetail_Id", "dbo.JokeCategoryDetails");
            DropIndex("dbo.JokeDetails", new[] { "JokeCategoryDetail_Id" });
            DropColumn("dbo.JokeDetails", "JokeCategoryDetail_Id");
        }
    }
}
