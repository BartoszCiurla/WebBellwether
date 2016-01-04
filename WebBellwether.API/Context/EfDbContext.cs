using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using WebBellwether.API.Entities;
using WebBellwether.API.Entities.IntegrationGame;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Entities.Joke;
using WebBellwether.API.Entities.Version;

namespace WebBellwether.API.Context
{
    public class EfDbContext : IdentityDbContext<IdentityUser>
    {
        public EfDbContext()
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