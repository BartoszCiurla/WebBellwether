using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBellwether.API.Context
{
    public static class InitSeedEngine
    {
        public static void RushSeedIntegrationGame(EfDbContext context)
        {
            var languages = InitSeed.BuildLanguagesList().ToList();
            if (!context.Languages.Any())
                context.Languages.AddRange(languages);
            context.SaveChanges();
            var myLanguages = context.Languages.ToList();
            if (!context.GameCategories.Any())
                context.GameCategories.AddRange(InitSeed.BuildGameCategories(myLanguages));
            if (!context.NumberOfPlayers.Any())
                context.NumberOfPlayers.AddRange(InitSeed.BuildNumberOfPlayers(myLanguages));
            if (!context.PaceOfPlays.Any())
                context.PaceOfPlays.AddRange(InitSeed.BuildPaceOfPlays(myLanguages));
            if (!context.PreparationFuns.Any())
                context.PreparationFuns.AddRange(InitSeed.BuildPreparationFuns(myLanguages));
            if (!context.GameFeatures.Any())
                context.GameFeatures.AddRange(InitSeed.BuildGameFeatures(myLanguages));
        }
    }
}