using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBellwether.API.Models.Joke
{
    public class JokeCategoryModel
    {
        public int Id { get; set; }//global id
        public int JokeCategoryId { get; set; } //translate id 
        public string JokeCategoryName { get; set; }
        public int LanguageId { get; set; }
        public bool TemporarySeveralTranslationDelete { get; set; }
        public List<AvailableLanguage> JokeCategoryTranslations { get; set; }
    }
}
