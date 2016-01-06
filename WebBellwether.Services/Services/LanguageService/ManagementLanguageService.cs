using System;
using System.Collections.Generic;
using System.Linq;
using WebBellwether.Models.Models.Translation;
using WebBellwether.Models.Results;
using WebBellwether.Repositories.Entities.Translations;
using WebBellwether.Services.Factories;
using WebBellwether.Services.Services.FileService;
using WebBellwether.Services.Utility;

namespace WebBellwether.Services.Services.LanguageService
{
    public interface IManagementLanguageService
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
    public class ManagementLanguageService : IManagementLanguageService
    {
        private readonly ILanguageFileService _languageFileService;

        public ManagementLanguageService(ILanguageFileService languageFileService)
        {
            _languageFileService = languageFileService;
        }
        public List<Language> GetLanguages(bool getAll = false)
        {
            var languages = getAll
                ? ModelMapper.Map<Language[], LanguageDao[]>(RepositoryFactory.Context.Languages.ToArray())
                : ModelMapper.Map<Language[], LanguageDao[]>(RepositoryFactory.Context.Languages.Where(x => x.IsPublic).ToArray());

            return languages.ToList();
        }
        private Language GetLanguageByName(string languageName)
        {
            return ModelMapper.Map<Language,LanguageDao>(RepositoryFactory.Context.Languages.FirstOrDefault(x => x.LanguageName.Equals(languageName)));
        }
        public Language GetLanguageById(int languageId)
        {
            return ModelMapper.Map<Language,LanguageDao>(RepositoryFactory.Context.Languages.FirstOrDefault(x => x.Id == languageId));
        }
        public ResultStateContainer PostLanguage(Language language)
        {
            try
            {
                //check is language exists 
                LanguageDao entity = ModelMapper.Map<LanguageDao,Language>(GetLanguageByName(language.LanguageName));
                if (entity != null)
                    return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.LanguageExists };
                //insert and return new id 
                entity = new LanguageDao { IsPublic = false, LanguageName = language.LanguageName, LanguageShortName = language.LanguageShortName };
                RepositoryFactory.Context.Languages.Add(entity);
                RepositoryFactory.Context.SaveChanges();
                entity = ModelMapper.Map<LanguageDao,Language>(GetLanguageByName(language.LanguageName));
                if (entity == null)
                    return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.LanguageExists };

                //create language file if error delete language row in db and return error
                ResultStateContainer createFileLanguageResult = CreateLanguageFile(entity.Id);
                if (createFileLanguageResult.ResultState != ResultState.Success)
                {
                    //delete language in db 
                    RepositoryFactory.Context.Languages.Remove(entity);
                    return createFileLanguageResult;
                }
                //if lang inserted must return lang id ...
                return new ResultStateContainer { ResultState = ResultState.Success, ResultMessage = ResultMessage.LanguageAdded, ResultValue = entity };
            }
            catch (Exception)
            {
                return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.Error };
            }
        }

        public ResultStateContainer FillLanguageFile(IEnumerable<string> languageValues, int langaugeId)
        {
            try
            {
                var values = languageValues as string[] ?? languageValues.ToArray();
                bool fillLanguageResult = _languageFileService.FillFile(values.ToArray(), langaugeId);
                return fillLanguageResult ? new ResultStateContainer { ResultState = ResultState.Success, ResultMessage = ResultMessage.KeyValuesAreCorrectlyFilled } :
                 new ResultStateContainer { ResultState = ResultState.Failure, ResultValue = ResultMessage.ContentLanguageFilesAreNotCompatible };
            }
            catch (Exception e)
            {

                return new ResultStateContainer { ResultState = ResultState.Failure, ResultValue = e };
            }
        }
        public IEnumerable<string> GetLanguageFileValue(int languageId)
        {
            return _languageFileService.GetFileValues(languageId);
        }

        public IEnumerable<LanguageFilePosition> GetLanguageFile(int languageId)
        {
            var dictionaryJson = _languageFileService.GetFile(languageId);
            return dictionaryJson.Select(x => new LanguageFilePosition {Key = x.Key, Value = x.Value});
        }

        public ResultStateContainer CreateLanguageFile(int newLanguageId)
        {
            try
            {
                if (newLanguageId == 0)
                    return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.LanguageNotExists };
                bool createFileResult = _languageFileService.CreateFile(newLanguageId);
                return createFileResult?new ResultStateContainer { ResultState = ResultState.Success, ResultMessage = ResultMessage.LanguageFileAdded }:new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.LanguageFileNotExists };             
            }
            catch (Exception)
            {
                return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.Error };
            }
        }
        public ResultStateContainer PutLanguageKey(LanguageKeyModel languageKey)
        {
            try
            {
                bool putLanguageKeyResult = _languageFileService.PutLanguageKey(languageKey);
                return putLanguageKeyResult
                    ? new ResultStateContainer
                    {
                        ResultState = ResultState.Success,
                        ResultMessage = ResultMessage.LanguageKeyValueEdited
                    }
                    : new ResultStateContainer {ResultState = ResultState.Failure};
            }
            catch (Exception)
            {

                return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.Error };
            }
        }
        public ResultStateContainer PublishLanguage(Language language)
        {
            try
            {
                int emptyKey = 0;
                if (language.IsPublic) //public language
                {
                    emptyKey = _languageFileService.GetFileEmptyKeys(language.Id);
                    if (emptyKey > 0)
                        return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.EmptyKeysExists };
                }
                else //nonpublic language
                {
                    if (RepositoryFactory.Context.Languages.Count(x=>x.IsPublic) == 1)
                        return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.OnlyOnePublicLanguage };
                }
                LanguageDao entity = RepositoryFactory.Context.Languages.FirstOrDefault(x => x.Id == language.Id);
                if (entity == null)
                    return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.LanguageNotExists };
                entity.IsPublic = language.IsPublic;
                RepositoryFactory.Context.SaveChanges();
                return language.IsPublic ? new ResultStateContainer { ResultState = ResultState.Success, ResultMessage = ResultMessage.LanguageHasBeenPublished } : new ResultStateContainer { ResultState = ResultState.Success, ResultMessage = ResultMessage.LanguageHasBeenNonpublic };
            }
            catch (Exception)
            {

                return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.Error };
            }
        }

        public ResultStateContainer PutLanguage(Language language)
        {
            try
            {
                LanguageDao entity = RepositoryFactory.Context.Languages.FirstOrDefault(x => x.Id == language.Id);
                if (entity == null)
                    return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.LanguageNotExists };
                entity.LanguageName = language.LanguageName;
                entity.LanguageShortName = language.LanguageShortName;
                RepositoryFactory.Context.SaveChanges();
                return new ResultStateContainer { ResultState = ResultState.Success, ResultMessage = ResultMessage.LanguageEdited };
            }
            catch (Exception)
            {

                return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.Error };
            }
        }
        public ResultStateContainer DeleteLanguage(Language language)
        {
            try
            {
                if (language.Id == 1 | language.Id == 2) // i need list of irremovable languages ... 
                    return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.LanguageCanNotBeRemoved };
                LanguageDao entity =ModelMapper.Map<LanguageDao,Language>(GetLanguageById(language.Id));
                if (entity == null)
                    return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.LanguageNotExists };
                RepositoryFactory.Context.Languages.Remove(entity);
                ResultStateContainer deleteLanguageFromOtherStructure = DeleteLanguageFromOtherStructure(language.Id);
                if (deleteLanguageFromOtherStructure.ResultState != ResultState.Success)
                    return deleteLanguageFromOtherStructure;
                bool removeLanguageFileResult = _languageFileService.RemoveFile(language.Id);
                if (removeLanguageFileResult)
                {
                    RepositoryFactory.Context.SaveChanges();
                    return new ResultStateContainer { ResultState = ResultState.Success, ResultMessage = ResultMessage.LanguageRemoved };
                }
                return new ResultStateContainer {ResultState = ResultState.Failure,ResultMessage = ResultMessage.LanguageCanNotBeRemoved};                
            }
            catch (Exception)
            {
                return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.Error };
            }
        }

        private ResultStateContainer DeleteLanguageFromOtherStructure(int languageId)
        {
            try
            {
                DeleteLanguageFromJokes(languageId);
                DeleteLanguageFromIntegrationGames(languageId);
                DeleteLanguageFromVersions(languageId);
                return new ResultStateContainer { ResultState = ResultState.Success };
            }
            catch (Exception e)
            {

                return new ResultStateContainer { ResultState = ResultState.Failure, ResultValue = e };
            }
        }

        private void DeleteLanguageFromIntegrationGames(int languageId)
        {
            var integrationGameDetails =
                RepositoryFactory.Context.IntegrationGameDetails.Where(x => x.Language.Id == languageId);
            RepositoryFactory.Context.IntegrationGameDetails.RemoveRange(integrationGameDetails);

            var integrationGameFeatures =
                RepositoryFactory.Context.IntegrationGameFeatures.Where(
                    x => x.GameFeatureLanguage.Language.Id == languageId);
            RepositoryFactory.Context.IntegrationGameFeatures.RemoveRange(integrationGameFeatures);

            var gameFeatureDetails =
                RepositoryFactory.Context.GameFeatureDetailLanguages.Where(x => x.Language.Id == languageId);
            RepositoryFactory.Context.GameFeatureDetailLanguages.RemoveRange(gameFeatureDetails);

            var gameFeatureLanguages =
                RepositoryFactory.Context.GameFeatureLanguages.Where(x => x.Language.Id == languageId);
            RepositoryFactory.Context.GameFeatureLanguages.RemoveRange(gameFeatureLanguages);
        }

        private void DeleteLanguageFromJokes(int languageId)
        {
            var jokeCategoryDetails =
                RepositoryFactory.Context.JokeCategoryDetails.Where(x => x.Language.Id == languageId);
            RepositoryFactory.Context.JokeCategoryDetails.RemoveRange(jokeCategoryDetails);

            var jokeDetails = RepositoryFactory.Context.JokeDetails.Where(x => x.Language.Id == languageId);
            RepositoryFactory.Context.JokeDetails.RemoveRange(jokeDetails);                
        }

        private void DeleteLanguageFromVersions(int languageId)
        {
            var languageVersion = RepositoryFactory.Context.LanguageVersions.Where(x => x.Language.Id == languageId);
            RepositoryFactory.Context.LanguageVersions.RemoveRange(languageVersion);

            var integrationGameVersion =
                RepositoryFactory.Context.IntegrationGameVersions.Where(x => x.Language.Id == languageId);
            RepositoryFactory.Context.IntegrationGameVersions.RemoveRange(integrationGameVersion);

            var jokeCategoryVersion =
                RepositoryFactory.Context.JokeCategoryVersions.Where(x => x.Language.Id == languageId);
            RepositoryFactory.Context.JokeCategoryVersions.RemoveRange(jokeCategoryVersion);

            var jokeVersion = RepositoryFactory.Context.JokeVersions.Where(x => x.Language.Id == languageId);
            RepositoryFactory.Context.JokeVersions.RemoveRange(jokeVersion);
        }
    }
}
