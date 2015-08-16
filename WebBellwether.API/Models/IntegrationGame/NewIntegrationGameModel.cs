using WebBellwether.API.Entities.Translations;

namespace WebBellwether.API.Models.IntegrationGame
{
    public class NewIntegrationGameModel
    {
        public int Id { get; set; }
        public string GameName { get; set; }
        public string GameDetails { get; set; }
        public Language Language { get; set; }
        public int[] Features { get; set; }
    }

}