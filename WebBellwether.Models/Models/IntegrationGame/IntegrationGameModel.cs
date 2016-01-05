using System.Collections.Generic;
using WebBellwether.Models.Models.Translation;

namespace WebBellwether.Models.Models.IntegrationGame
{
    public class IntegrationGameModel
    {
        public int Id { get; set; } // global id 
        public int IntegrationGameId { get; set; }//id for translationh
        public string GameName { get; set; }
        public string GameDescription { get; set; }
        public bool TemporarySeveralTranslationDelete { get; set; }
        public Language Language { get; set; }
        public List<IntegrationGameDetailModel> IntegrationGameDetailModels { get; set; }
        public List<AvailableLanguage> GameTranslations { get; set; } 

    }
}