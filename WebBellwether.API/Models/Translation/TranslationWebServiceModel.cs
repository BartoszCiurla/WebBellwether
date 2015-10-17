using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBellwether.API.Models.Translation
{
    public class TranslationWebServiceModel
    {
        public string ServiceName { get; set; }
        public string ApiUrl { get; set; }
        public bool UseApiKey { get; set; }
        public string ApiKeyTemplate { get; set; }
        public string LanguageTemplate { get; set; }
        public string TextInputTemplate { get; set; }
        public bool IsPrimary { get; set; }
    }
}