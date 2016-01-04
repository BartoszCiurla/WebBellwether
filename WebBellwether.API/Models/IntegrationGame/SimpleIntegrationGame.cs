
using WebBellwether.API.Entities.Translations;

namespace WebBellwether.API.Models.IntegrationGame
{
    public class SimpleIntegrationGame
    {
        public int Id { get; set; }
        public LanguageDao Language { get; set; }
    }
}