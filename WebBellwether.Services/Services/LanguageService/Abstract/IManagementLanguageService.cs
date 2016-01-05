using System.Collections.Generic;
using WebBellwether.Models.Models.Translation;
using WebBellwether.Models.Results;
using WebBellwether.Repositories.Entities.Translations;

namespace WebBellwether.Services.Services.LanguageService.Abstract
{
    interface IManagementLanguageService
    {
        List<LanguageDao> GetLanguages(bool getAll = false);
        ResultStateContainer PostLanguage(LanguageDao language);
        ResultStateContainer FillLanguageFile(IEnumerable<string> languageValues, int langaugeId);
        IEnumerable<string> GetLanguageFileValue(int languageId);
        ResultStateContainer CreateLanguageFile(int newLanguageId);
        ResultStateContainer PutLanguageKey(LanguageKeyModel languageKey);
        ResultStateContainer PublishLanguage(LanguageDao language);
        ResultStateContainer PutLanguage(LanguageDao language);
        ResultStateContainer DeleteLanguage(LanguageDao language);
        IEnumerable<LanguageFilePosition> GetLanguageFile(int languageId);
        LanguageDao GetLanguageById(int languageId);
    }
}
