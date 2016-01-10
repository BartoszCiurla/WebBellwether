namespace WebBellwether.Models.ViewModels.IntegrationGame
{
    public class IntegrationGameDetailViewModel
    {
        public int Id { get; set; }// id for featuredetail
        public int GameFeatureId { get; set; } // game feature id 
        public int GameFeatureLanguageId { get; set; }
        public int GameFeatureDetailId { get; set; } // game feature detail id 
        public string GameFeatureName { get; set; }
        public string GameFeatureDetailName { get; set; }

    }
}