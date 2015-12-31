using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Models.Translation;
using WebBellwether.API.Results;

namespace WebBellwether.API.Services.LanguageService.Abstract
{
    interface IManagementLanguageService
    {
        List<Language> GetLanguages(bool getAll = false);
        ResultStateContainer PostLanguage(Language language);
        ResultStateContainer FillLanguageFile(IEnumerable<string> languageValues, int langaugeId);
        IEnumerable<string> GetLanguageFileValue(int languageId);
        ResultStateContainer CreateLanguageFile(int newLanguageId);
        ResultStateContainer PutLanguageKey(LanguageKeyModel languageKey);
        ResultStateContainer PublishLanguage(Language language);
        ResultStateContainer PutLanguage(Language language);
        ResultStateContainer DeleteLanguage(Language language);
        IEnumerable<LanguageFilePosition> GetLanguageFile(int languageId);
        Language GetLanguageById(int languageId);
    }
}
