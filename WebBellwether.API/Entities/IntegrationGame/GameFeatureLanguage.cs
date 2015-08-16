using WebBellwether.API.Entities.Translations;

namespace WebBellwether.API.Entities.IntegrationGame
{
    public class GameFeatureLanguage
    {
        public int Id { get; set; }
        public string GameFeatureName { get; set; }
        public Language Language { get; set; }
        public virtual GameFeature GameFeature { get; set; }
    }
}