using WebBellwether.API.Entities.Translations;

namespace WebBellwether.API.Models.Translation
{
    public class AvailableLanguage
    {
        public bool HasTranslation { get; set; }
        public LanguageDao Language { get; set; }
    }
}