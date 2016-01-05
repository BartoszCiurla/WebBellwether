namespace WebBellwether.Models.Models.Translation
{
    public class Language
    {
        public int Id { get; set; }
        public string LanguageName { get; set; }
        public string LanguageShortName { get; set; }
        public bool IsPublic { get; set; }
    }
}
