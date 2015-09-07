using System.Collections.Generic;

namespace WebBellwether.API.Models.IntegrationGame.templates
{
    public class GameFeatureTemplate
    {
        public int Id { get; set; }//robi jako gamefeatureid czyli ten glowny 
        public string GameFeatureName { get; set; }
        public List<GameFeatureDetailTemplate> GameFeatureDetailTemplates { get; set; }
    }
}