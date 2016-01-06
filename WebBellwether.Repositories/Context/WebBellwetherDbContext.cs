using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebBellwether.Repositories.Entities.Auth;
using WebBellwether.Repositories.Entities.IntegrationGame;
using WebBellwether.Repositories.Entities.Joke;
using WebBellwether.Repositories.Entities.Translations;
using WebBellwether.Repositories.Entities.Version;

namespace WebBellwether.Repositories.Context
{
    public class WebBellwetherDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<IntegrationGameDao> IntegrationGames { get; set; }
        public DbSet<IntegrationGameDetailDao> IntegrationGameDetails { get; set; }
        public DbSet<IntegrationGameFeatureDao> IntegrationGameFeatures { get; set; }
        public DbSet<GameFeatureDao> GameFeatures { get; set; }
        public DbSet<GameFeatureLanguageDao> GameFeatureLanguages { get; set; }
        public DbSet<GameFeatureDetailDao> GameFeatureDetails { get; set; }
        public DbSet<GameFeatureDetailLanguageDao> GameFeatureDetailLanguages { get; set; }
        public DbSet<JokeDao> Jokes { get; set; }
        public DbSet<JokeDetailDao> JokeDetails { get; set; }
        public DbSet<JokeCategoryDao> JokeCategories { get; set; }
        public DbSet<JokeCategoryDetailDao> JokeCategoryDetails { get; set; }
        public DbSet<LanguageDao> Languages { get; set; }
        public DbSet<ClientDao> Clients { get; set; }
        public DbSet<RefreshTokenDao> RefreshTokens { get; set; }
        public DbSet<LanguageVersionDao> LanguageVersions { get; set; }
        public DbSet<IntegrationGameVersionDao> IntegrationGameVersions { get; set; }
        public DbSet<JokeCategoryVersionDao> JokeCategoryVersions { get; set; }
        public DbSet<JokeVersionDao> JokeVersions { get; set; }
        public WebBellwetherDbContext()
           : base("WebBellwether")
        {

        }
    }
}