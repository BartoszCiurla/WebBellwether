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
using Newtonsoft.Json.Linq;

namespace WebBellwether.API.Services.LanguageService
{
    public class ManagementLanguageService
    {
        private IAggregateRepositories _repository;
        private string destinationPlace;
        public ManagementLanguageService(IAggregateRepositories repository, string destPlace)
        {
            _repository = repository;
            destinationPlace = destPlace;
        }
        public List<Language> GetLanguages(bool getAll = false)
        {
            return getAll? _repository.LanguageRepository.Get().ToList(): _repository.LanguageRepository.GetWithInclude(x => x.IsPublic).ToList();
        }
        private Language GetLanguageByName(string languageName)
        {
            return _repository.LanguageRepository.GetWithInclude(x => x.LanguageName.Equals(languageName)).FirstOrDefault();
        }
        private Language GetLanguageByID(int languageId)
        {
            return _repository.LanguageRepository.GetWithInclude(x => x.Id == languageId).FirstOrDefault();
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
                if(entity == null)
                    return new ResultStateContainer { ResultState = ResultState.LanguageExists };

                //create language file if error delete language row in db and return error
                ResultStateContainer createFileLanguageResult = CreateLanguageFile(entity.Id);
                if (createFileLanguageResult.ResultState != ResultState.LanguageFileAdded)
                {
                    //delete language in db 
                    _repository.LanguageRepository.Delete(entity);
                    return createFileLanguageResult;
                }                     
                //if lang inserted must return lang id ...
                return new ResultStateContainer { ResultState = ResultState.LanguageAdded , Value = entity };
            }
            catch (Exception e)
            {
                return new ResultStateContainer { ResultState = ResultState.Error,Value = e};
            }
        }

        private Dictionary<string, string> GetLanguageFileKeys()
        {
            Language templateLanguage = _repository.LanguageRepository.GetWithInclude(x => x.IsPublic).FirstOrDefault();
            if (templateLanguage == null)
                return null;
            string templateFileLocation = destinationPlace + templateLanguage.Id + ".json";
            string templateJson = File.ReadAllText(templateFileLocation);
            dynamic jsonTemplateObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(templateJson);
            if (jsonTemplateObj == null)
                return null;
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var item in jsonTemplateObj)
            {
                result.Add(item.Key, "");
            }
            return result;

        }

        public ResultStateContainer CreateLanguageFile(int newLanguageId)
        {
            try
            {
                if (newLanguageId == 0)
                    return new ResultStateContainer { ResultState = ResultState.LanguageNotExists };
                //take key for new file                
                Dictionary<string, string> languageFileKeys = GetLanguageFileKeys();
                if (languageFileKeys == null)
                    return new ResultStateContainer { ResultState = ResultState.LanguageFileNotExists };

                string newFileLocation = destinationPlace + newLanguageId + ".json";
                
                //create json object
                JObject jsonKeys = new JObject();
                foreach (var item in languageFileKeys)
                {
                    jsonKeys.Add(new JProperty(item.Key, item.Value));
                }

                //create new file
                using (StreamWriter file = File.CreateText(newFileLocation))

                //save keys to file
                using (JsonTextWriter writer = new JsonTextWriter(file))
                {
                    jsonKeys.WriteTo(writer);
                }               
                    return new ResultStateContainer { ResultState = ResultState.LanguageFileAdded };
            }
            catch (Exception e)
            {
                return new ResultStateContainer { ResultState = ResultState.Error, Value = e };
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

                return new ResultStateContainer { ResultState = ResultState.Error, Value = e };
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
                        return new ResultStateContainer { ResultState = ResultState.EmptyKeysExists };
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
                return language.IsPublic == true ? new ResultStateContainer { ResultState = ResultState.LanguageHasBeenPublished } : new ResultStateContainer { ResultState = ResultState.LanguageHasBeenNonpublic };
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
        private ResultStateContainer DeleteLanguageFile(int languageId)
        {
            try
            {
                string file = destinationPlace + languageId + ".json";
                if(File.Exists(file))
                {
                    File.Delete(file);
                    return new ResultStateContainer { ResultState = ResultState.LanguageFileRemoved };
                }
                return new ResultStateContainer { ResultState = ResultState.LanguageFileNotExists };
            }
            catch (Exception e)
            {

                return new ResultStateContainer { ResultState = ResultState.Error, Value = e };
            }
        }
        private ResultStateContainer DeleteIntegrationGames(int languageId)
        {
            //NOTE STILL DO NOT KNOW WHETHER TO OPERATE HERE SEPARATE WRITTEN OR SERVICE BECAUSE THIS WILL BE MUCH WILL THIS IS NOT SO SIMPLE
            try
            {
                return new ResultStateContainer { ResultState = ResultState.Error };
            }
            catch (Exception e)
            {

                return new ResultStateContainer { ResultState = ResultState.Error, Value = e };
            }
        }
        private ResultStateContainer DeleteJokes(int languageId)
        {
            //NOTE STILL DO NOT KNOW WHETHER TO OPERATE HERE SEPARATE WRITTEN OR SERVICE BECAUSE THIS WILL BE MUCH WILL THIS IS NOT SO SIMPLE
            try
            {
                return new ResultStateContainer { ResultState = ResultState.Error };
            }
            catch (Exception e)
            {

                return new ResultStateContainer { ResultState = ResultState.Error, Value = e };
            }
        }
        public ResultStateContainer DeleteLanguage(Language language)
        {
            try
            {
                //BETA VERSION NOT REMOVE OTHER STRUCTURE ... 
                if (language.Id == 1 | language.Id == 2)
                    return new ResultStateContainer { ResultState = ResultState.LanguageCanNotBeRemoved };
                Language entity = GetLanguageByID(language.Id);
                if (entity == null)
                    return new ResultStateContainer { ResultState = ResultState.LanguageNotExists };
                _repository.LanguageRepository.Delete(entity);

                ResultStateContainer deleteFileResult = DeleteLanguageFile(language.Id);
                if (deleteFileResult.ResultState != ResultState.LanguageFileRemoved)
                    return deleteFileResult;
                _repository.Save();
                return new ResultStateContainer { ResultState = ResultState.LanguageRemoved };
            }
            catch (Exception e)
            {

                return new ResultStateContainer { ResultState = ResultState.Error, Value = e };
            }
        }
    }
}
