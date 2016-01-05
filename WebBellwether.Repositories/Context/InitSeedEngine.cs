using System.Linq;

namespace WebBellwether.Repositories.Context
{
    public static class InitSeedEngine
    {
        public static void RushSeedIntegrationGame(WebBellwetherDbContext context)
        {
            var languages = InitSeed.BuildLanguagesList().ToList();
            if (!context.Languages.Any())
                context.Languages.AddRange(languages);
            context.SaveChanges();
            var myLanguages = context.Languages.ToList();
            if (!context.GameFeatures.Any())
                context.GameFeatures.AddRange(InitSeed.BuildGameFeatures(myLanguages));
        }
    }
}