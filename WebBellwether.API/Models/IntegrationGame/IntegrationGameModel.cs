using System.Collections.Generic;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Models.Translation;

namespace WebBellwether.API.Models.IntegrationGame
{
    public class IntegrationGameModel
    {
        public int Id { get; set; } // global id 
        public int IntegrationGameId { get; set; }//id for translationh
        public string GameName { get; set; }
        public string GameDescription { get; set; }
        public bool TemporarySeveralTranslationDelete { get; set; }
        public LanguageDao Language { get; set; }
        public List<IntegrationGameDetailModel> IntegrationGameDetailModels { get; set; }
        public List<AvailableLanguage> GameTranslations { get; set; } 

    }
}