using System.Collections.Generic;
using WebBellwether.API.Models.Translation;

namespace WebBellwether.API.Models.Joke
{
    public class JokeModel
    {
        public int Id { get; set; } //global id
        public int JokeId { get; set; } //id for translation
        public string JokeContent { get; set; }
        public int LanguageId { get; set; }
        public string JokeCategoryName { get; set; }
        public int JokeCategoryId { get; set; }
        public int JokeCategoryDetailId { get; set; }
        public bool TemporarySeveralTranslationDelete { get; set; }
        public List<AvailableLanguage> JokeTranslations { get; set; }
    }
}
