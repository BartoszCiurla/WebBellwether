using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBellwether.API.Entities.Translations;

namespace WebBellwether.API.Models
{
    public class AvailableLanguage
    {
        public bool HasTranslation { get; set; }
        public Language Language { get; set; }
    }
}