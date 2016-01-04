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
