using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Results;
using WebBellwether.API.Models.Translation;
using System.IO;
using Newtonsoft.Json;
using WebBellwether.API.Repositories.Abstract;

namespace WebBellwether.API.Services.LanguageService
{
    public class ManagementLanguageService
    {
        private IAggregateRepositories _repository;
        private string destinationPlace;
        public ManagementLanguageService(IAggregateRepositories unitOfWork,string destPlace)
        {
            _repository = unitOfWork;
            destinationPlace = destPlace;
        } 
        public List<Language> GetLanguages()
        {
            return _repository.LanguageRepository.GetWithInclude(x=>x.IsPublic).ToList();
        }
        public List<Language> GetAllLanguages()
        {
            return _repository.LanguageRepository.Get().ToList();
        }
        private Language GetLanguageByName(string languageName)
        {
            return _repository.LanguageRepository.GetWithInclude(x => x.LanguageName.Equals(languageName)).FirstOrDefault();
        }
        public ResultStateContainer PostLanguage(Language language)
        {
            try
            {
                //check is language exists 
                Language entity = GetLanguageByName(language.LanguageName);
                if (entity != null)
                    return new ResultStateContainer { ResultState = ResultState.LanguageExists };
                //insert and return new id 
                entity = new Language { IsPublic = false, LanguageName = language.LanguageName, LanguageFlag = language.LanguageFlag, LanguageShortName = language.LanguageShortName };
                _repository.LanguageRepository.Insert(entity);
                _repository.Save();
                entity = GetLanguageByName(language.LanguageName);
                                  
               //3 majac id tworze plik jezeli sie nie uda usuwam poprzedni wpis zwracam blad z zapisu pliku

                //jezeli wszystko ok to moge kontynuować w interfejsie czyli masowe tlumaczenie ... 

               return new ResultStateContainer { ResultState = ResultState.Error };
            }
            catch (Exception e)
            {
                return new ResultStateContainer { ResultState = ResultState.Error };
            }
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
                int emptyKey = 0;
                if (language.IsPublic) //public language
                {
                    string jsonLocation = destinationPlace + language.Id + ".json";
                    string json = File.ReadAllText(jsonLocation);
                    dynamic jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    if (jsonObj == null)
                        return new ResultStateContainer { ResultState = ResultState.LanguageFileNotExists, };
                    foreach (var item in jsonObj)
                    {
                        if (item.Value.Length < 3)
                            emptyKey++;
                    }
                    if (emptyKey > 0)
                        return new ResultStateContainer { ResultState = ResultState.EmptyKeysExists};
                }
                else //nonpublic language
                {
                    if (_repository.LanguageRepository.GetWithInclude(x => x.IsPublic == true).Count() == 1)  //check how many languages is public 
                        return new ResultStateContainer { ResultState = ResultState.OnlyOnePublicLanguage };
                }
                Language entity = _repository.LanguageRepository.GetWithInclude(x => x.Id == language.Id).FirstOrDefault();
                if (entity == null)
                    return new ResultStateContainer { ResultState = ResultState.LanguageNotExists };
                entity.IsPublic = language.IsPublic;
                _repository.Save();
                return language.IsPublic == true ? new ResultStateContainer {ResultState = ResultState.LanguageHasBeenPublished } : new ResultStateContainer {ResultState = ResultState.LanguageHasBeenNonpublic };
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
                Language entity = _repository.LanguageRepository.GetWithInclude(x => x.Id == language.Id).FirstOrDefault();
                if (entity == null)
                    return new ResultStateContainer { ResultState = ResultState.LanguageNotExists };
                entity.LanguageFlag = language.LanguageFlag;
                entity.LanguageName = language.LanguageName;
                entity.LanguageShortName = language.LanguageShortName;
                _repository.Save();
                return new ResultStateContainer { ResultState = ResultState.LanguageEdited };
            }
            catch (Exception e)
            {

                return new ResultStateContainer { ResultState = ResultState.Error, Value = e };
            }
        }
    }
}
