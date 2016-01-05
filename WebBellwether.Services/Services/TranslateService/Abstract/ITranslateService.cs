using System.Collections.Generic;
using System.Threading.Tasks;
using WebBellwether.Models.Models.Translation;
using WebBellwether.Models.Results;

namespace WebBellwether.Services.Services.TranslateService.Abstract
{
    public interface ITranslateService
    {
        List<SupportedLanguage> GetListOfSupportedLanguages();
        string GetServiceName();
        Task<ResultStateContainer> GetLanguageTranslation(TranslateLanguageModel languageModel);

        Task<ResultStateContainer> GetAllLanguageKeysTranslations(TranslateLanguageModel languageModel);
    }
}
