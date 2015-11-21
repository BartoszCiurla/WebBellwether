﻿using System.Collections.Generic;
using WebBellwether.API.Models.Translation;

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
