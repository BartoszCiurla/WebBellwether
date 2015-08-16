using WebBellwether.API.Entities.Translations;

namespace WebBellwether.API.Entities.IntegrationGame
{
    public class GameFeatureDetailLanguage
    {
        public int Id { get; set; }
        public virtual GameFeatureDetail GameFeatureDetail { get; set; }
        public string GameFeatureDetailName { get; set; }
        public virtual Language Language { get; set; }
    }
}