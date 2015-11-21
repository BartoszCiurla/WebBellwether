using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBellwether.API.Models.Translation;

namespace WebBellwether.API.Services.TranslateService.Abstract
{
    public interface ITranslateService
    {
        List<SupportedLanguage> GetListOfSupportedLanguages();
        string GetServiceName();
    }
}
