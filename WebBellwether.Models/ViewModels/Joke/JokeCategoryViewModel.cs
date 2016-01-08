using System.Collections.Generic;
using WebBellwether.Models.Models.Translation;

namespace WebBellwether.Models.ViewModels.Joke
{
    public class JokeCategoryViewModel
    {
        public int Id { get; set; }//global id
        public int JokeCategoryId { get; set; } //translate id 
        public string JokeCategoryName { get; set; }
        public int LanguageId { get; set; }
        public bool TemporarySeveralTranslationDelete { get; set; }
        public List<AvailableLanguage> JokeCategoryTranslations { get; set; }
    }
}
