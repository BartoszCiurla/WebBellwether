
namespace WebBellwether.API.Models.Translation
{
    public class TranslateLanguageKeysModel
    {
        public string CurrentLanguageShortName { get; set; }
        public int CurrentLanguageId { get; set; }
        public string TargetLangaugeShortName { get; set; }
        public int TargetLanguageId { get; set; }

    }
}
