
namespace WebBellwether.API.Entities.Translations
{
    //tylko sama definicja jezyka 
    public class Language
    {
        public int Id { get; set; }
        public string LanguageName { get; set; }
        public string LanguageShortName { get; set; }
        public bool IsPublic { get; set; }
        public double LanguageVersion { get; set; }
    }
}