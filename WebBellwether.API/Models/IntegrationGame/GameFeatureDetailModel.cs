namespace WebBellwether.API.Models.IntegrationGame
{
    public class GameFeatureDetailModel
    {
        public int Id { get; set; }//gamefeaturedetaillanguageid
        public int GameFeatureDetailId { get; set; }//detail id 
        public int GameFeatureId { get; set; } 
        public string GameFeatureDetailName { get; set; }
        public string GameFeatureDetailTemplateName { get; set; }
        public string Language { get; set; }
        public int LanguageId { get; set; }
    }
}