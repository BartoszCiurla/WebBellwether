using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBellwether.API.Models.Translation
{
    public class TranslateLanguageKeyModel
    {
        public string CurrentLanguageShortName { get; set; }
        public string TargetLangaugeShortName { get; set; }
        public string ContentToTranslate { get; set; }
    }
}
