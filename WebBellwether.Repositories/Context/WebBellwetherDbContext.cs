using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebBellwether.Repositories.Entities;
using WebBellwether.Repositories.Entities.Auth;
using WebBellwether.Repositories.Entities.IntegrationGame;
using WebBellwether.Repositories.Entities.Joke;
using WebBellwether.Repositories.Entities.Translations;
using WebBellwether.Repositories.Entities.Version;

namespace WebBellwether.Repositories.Context
{
    public class WebBellwetherDbContext :  IdentityDbContext<IdentityUser>
    {
        public WebBellwetherDbContext()
           : base("WebBellwether")
        {
            
        }
        //intergration games 
        public DbSet<IntegrationGameDao> IntegrationGames { get; set; }
        public DbSet<IntegrationGameDetailDao> IntegrationGameDetails { get; set; }
        public DbSet<IntegrationGameFeatureDao> IntegrationGameFeatures { get; set; }

        public DbSet<GameFeatureDao> GameFeatures { get; set; }
        public DbSet<GameFeatureLanguageDao> GameFeatureLanguages { get; set; }

        public DbSet<GameFeatureDetailDao> GameFeatureDetails { get; set; }
        public DbSet<GameFeatureDetailLanguageDao> GameFeatureDetailLanguages { get; set; }

        //intergration games 


        //jokes
        public DbSet<JokeDao> Jokes { get; set; }
        public DbSet<JokeDetailDao> JokeDetails { get; set; }
        public DbSet<JokeCategoryDao> JokeCategories { get; set; }
        public DbSet<JokeCategoryDetailDao> JokeCategoryDetails { get; set; }
        //jokes

        //translation    
        public DbSet<LanguageDao> Languages { get; set; }
        //translation

        //authentication
        public DbSet<ClientDao> Clients { get; set; }
        public DbSet<RefreshTokenDao> RefreshTokens { get; set; }
        //authentication

        //version
        public DbSet<LanguageVersionDao> LanguageVersions { get; set; }
        public DbSet<IntegrationGameVersionDao> IntegrationGameVersions { get; set; }
        public DbSet<JokeCategoryVersionDao> JokeCategoryVersions { get; set; }
        public DbSet<JokeVersionDao> JokeVersions { get; set; }
        //version
    }
}