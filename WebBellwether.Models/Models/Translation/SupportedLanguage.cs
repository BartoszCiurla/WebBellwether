namespace WebBellwether.Models.Models.Translation
{
    public class SupportedLanguage
    {
        public SupportedLanguage(string language,string code)
        {
            Language = language;
            Code = code;
        }
        public string Language { get; set; }
        public string Code { get; set; }
    }
}