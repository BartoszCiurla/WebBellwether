namespace WebBellwether.API.Models.IntegrationGame
{
    public class GameFeatureModel
    {
        public int Id { get; set; }//robi jako gamefeatureid czyli ten glowny 
        public string GameFeatureTemplateName { get; set; } // jako template zawsze angielski na twardo to zostanie przypisane 
        public string GameFeatureName { get; set; }
        public int LanguageId { get; set; }
    }
}