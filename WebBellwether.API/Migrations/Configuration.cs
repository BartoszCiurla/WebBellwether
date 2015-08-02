using WebBellwether.API.Context;

namespace WebBellwether.API.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WebBellwether.API.Context.EfDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebBellwether.API.Context.EfDbContext context)
        {
            InitSeedEngine.RushSeedIntegrationGame(context);
        }
    }
}
