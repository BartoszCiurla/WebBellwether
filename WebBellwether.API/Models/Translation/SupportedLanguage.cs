using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBellwether.API.Models.Translation
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