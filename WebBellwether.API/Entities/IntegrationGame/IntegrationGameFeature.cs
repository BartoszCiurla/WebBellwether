namespace WebBellwether.API.Entities.IntegrationGame
{
    public class IntegrationGameFeature
    {
        public int Id { get; set; }
        public virtual IntegrationGameDetail IntegrationGameDetail { get; set; }
        public virtual GameFeatureLanguage GameFeatureLanguage { get; set; }
        public virtual GameFeatureDetailLanguage GameFeatureDetailLanguage { get; set; }
    }
}