using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBellwether.API.Models.Translation
{
    public class TranslateLanguageModel
    {
        public TranslateLanguageModel()
        {
            
        }

        public TranslateLanguageModel(string currentLanguageCode,string targetLanguageCode,IEnumerable<string> contentForTranslation)
        {
            CurrentLanguageCode = currentLanguageCode;
            TargetLanguageCode = targetLanguageCode;
            ContentForTranslation = contentForTranslation;
        }
        public string CurrentLanguageCode { get; set; }
        public string TargetLanguageCode { get; set; }
        public IEnumerable<string> ContentForTranslation { get; set; }
    }
}