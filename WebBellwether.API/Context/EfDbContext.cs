using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using WebBellwether.API.Entities;
using WebBellwether.API.Entities.IntegrationGames;
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
        public DbSet<IntegrationGameLanguage> IntegrationGameLanguages { get; set; }
        public DbSet<GameCategory> GameCategories { get; set; }
        public DbSet<GameCategoryLanguage> GameCategoryLanguages { get; set; }
        public DbSet<NumberOfPlayer> NumberOfPlayers { get; set; }
        public DbSet<NumberOfPlayerLanguage> NumberOfPlayerLanguages { get; set; }
        public DbSet<PaceOfPlay> PaceOfPlays { get; set; }
        public DbSet<PaceOfPlayLanguage> PaceOfPlayLanguages { get; set; }
        public DbSet<PreparationFun> PreparationFuns { get; set; }
        public DbSet<PreparationFunLanguage> PreparationFunLanguages { get; set; }
        public DbSet<GameFeature> GameFeatures { get; set; }
        public DbSet<GameFeatureLanguage> GameFeatureLanguages { get; set; }
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