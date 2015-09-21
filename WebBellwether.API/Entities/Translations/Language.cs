
namespace WebBellwether.API.Entities.Translations
{
    //tylko sama definicja jezyka 
    public class Language
    {
        public int Id { get; set; }
        public string LanguageName { get; set; }
        public string LanguageShortName { get; set; }
        public string LanguageFlag { get; set; }
    }
}