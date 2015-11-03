using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBellwether.API.Models.Translation
{
    public class LanguageKeyModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public int LanguageId { get; set; }
    }
}