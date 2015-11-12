using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBellwether.API.UnitOfWork;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Results;
using WebBellwether.API.Models.Translation;
using System.IO;
using Newtonsoft.Json;

namespace WebBellwether.API.Services.LanguageService
{
    public class ManagementLanguageService
    {
        private LanguageUnitOfWork _unitOfWork;
        private string destinationPlace;
        public ManagementLanguageService(LanguageUnitOfWork unitOfWork,string destPlace)
        {
            _unitOfWork = unitOfWork;
            destinationPlace = destPlace;
        } 
        public List<Language> GetLanguages()
        {
            return _unitOfWork.LanguageRepository.Get().ToList();
        }
        public ResultStateContainer PutLanguageKey(LanguageKeyModel languageKey)
        {
            try
            {
                string jsonLocation = destinationPlace + languageKey.LanguageId + ".json";
                string json = File.ReadAllText(jsonLocation);
                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                jsonObj[languageKey.Key] = languageKey.Value;
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonLocation, output);
                return new ResultStateContainer { ResultState = ResultState.LanguageKeyValueEdited };
            }
            catch (Exception e)
            {

                return new ResultStateContainer { ResultState = ResultState.Error,Value = e };
            }
        }
        public ResultStateContainer PublishLanguage(Language language)
        {
            try
            {
                if (!language.IsPublic) //unpublic language
                {
                    if (_unitOfWork.LanguageRepository.GetWithInclude(x => x.IsPublic == true).Count() == 1)  //check how many languages is public 
                        return new ResultStateContainer { ResultState = ResultState.OnlyOnePublicLanguage };
                    //Language entity 

                }
                
                int emptyKey = 0;
                string jsonLocation = destinationPlace + language.Id + ".json";
                string json = File.ReadAllText(jsonLocation);
                //dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                dynamic jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                if (jsonObj == null)
                    return new ResultStateContainer { ResultState = ResultState.Error };//jeszcze nie wiem czy może być taki przypadek tzn brak pliku
                foreach (var item in jsonObj)
                {
                    if (item.Value.Length < 3)
                        emptyKey++;
                }
                // juz mi się nie chce w każdym razie gdy emptykey > 0 to lipa nie moge dodać 
                // w innym przypadku robie update btw jak wchodzi na 0 czyli nie chce publkować to tego nie sprawdzam ... tylko wale update 
                return new ResultStateContainer { ResultState = ResultState.Error };
                                
            }
            catch (Exception e)
            {

                return new ResultStateContainer { ResultState = ResultState.Error, Value = e };
            }
        }

        public ResultStateContainer PutLanguage(Language language)
        {
            try
            {
                Language entity = _unitOfWork.LanguageRepository.GetWithInclude(x => x.Id == language.Id).FirstOrDefault();
                if (entity == null)
                    return new ResultStateContainer { ResultState = ResultState.LanguageNotExists };
                entity.LanguageFlag = language.LanguageFlag;
                entity.LanguageName = language.LanguageName;
                entity.LanguageShortName = language.LanguageShortName;
                _unitOfWork.Save();
                return new ResultStateContainer { ResultState = ResultState.LanguageEdited };
            }
            catch (Exception e)
            {

                return new ResultStateContainer { ResultState = ResultState.Error, Value = e };
            }
        }
    }
}
