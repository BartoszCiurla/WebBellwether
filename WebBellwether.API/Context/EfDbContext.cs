using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using WebBellwether.API.Entities;
using WebBellwether.API.Entities.IntegrationGame;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Models;

namespace WebBellwether.API.Context
{
    public class EfDbContext : IdentityDbContext<IdentityUser>
    {
        public EfDbContext()
            : base("WebBellwether")
        {
            
        }
        //intergration games 
        public DbSet<IntegrationGame> IntegrationGames { get; set; }
        public DbSet<IntegrationGameDetail> IntegrationGameDetails { get; set; }
        public DbSet<IntegrationGameFeature> IntegrationGameFeatures { get; set; }

        public DbSet<GameFeature> GameFeatures { get; set; }
        public DbSet<GameFeatureLanguage> GameFeatureLanguages { get; set; }

        public DbSet<GameFeatureDetail> GameFeatureDetails { get; set; }
        public DbSet<GameFeatureDetailLanguage> GameFeatureDetailLanguages { get; set; }
  
        //intergration games 

        //translation    
        public DbSet<Language> Languages { get; set; } 
        //translation

        //authentication
        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        //authentication
    }
}