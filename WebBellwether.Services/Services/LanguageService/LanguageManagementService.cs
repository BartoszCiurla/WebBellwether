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
        Language[] GetLanguages(bool getNotPublicLanguages = false);
        Language PostLanguage(Language language);
        bool FillLanguageFile(IEnumerable<string> languageValues, int langaugeId);
        IEnumerable<string> GetLanguageFileValue(int languageId);
        bool CreateLanguageFile(int newLanguageId);
        bool PutLanguageKey(LanguageKeyModel languageKey);
        string PublishLanguage(Language language);
        bool PutLanguage(Language language);
        bool DeleteLanguage(Language language);
        IEnumerable<LanguageFilePosition> GetLanguageFile(int languageId);
        Language GetLanguageById(int languageId);
    }
    public class LanguageManagementService : IManagementLanguageService
    {
        private readonly ILanguageFileService _languageFileService;

        public LanguageManagementService(ILanguageFileService languageFileService)
        {
            _languageFileService = languageFileService;
        }

        public Language[] GetLanguages(bool getNotPublicLanguages = false)
        {
            return getNotPublicLanguages
                ? ModelMapper.Map<Language[], LanguageDao[]>(RepositoryFactory.Context.Languages.ToArray())
                : ModelMapper.Map<Language[], LanguageDao[]>(RepositoryFactory.Context.Languages.Where(x => x.IsPublic).ToArray());
        }
     
        public Language GetLanguageById(int languageId)
        {
            return
                ModelMapper.Map<Language, LanguageDao>(
                    RepositoryFactory.Context.Languages.FirstOrDefault(x => x.Id == languageId));
        }
        public Language PostLanguage(Language language)
        {
            ValidateGetLanguageDaoByName(language.LanguageName);            
            var entity = ModelMapper.Map<LanguageDao, Language>(language);
            RepositoryFactory.Context.Languages.Add(entity);
            RepositoryFactory.Context.SaveChanges();
            entity = ModelMapper.Map<LanguageDao, Language>(ValidateGetLanguageByName(language.LanguageName));
            if (CreateLanguageFile(entity.Id))
                return ModelMapper.Map<Language, LanguageDao>(entity);
            RepositoryFactory.Context.Languages.Remove(entity);
            throw new Exception(ThrowMessage.LanguageFileNotExists.ToString());
        }

        public bool FillLanguageFile(IEnumerable<string> languageValues, int langaugeId)
        {
            var values = languageValues as string[] ?? languageValues.ToArray();
            bool fillLanguageResult = _languageFileService.FillFile(values.ToArray(), langaugeId);
            if (!fillLanguageResult)
                throw new Exception(ThrowMessage.ContentLanguageFilesAreNotCompatible.ToString());
            return true;
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

        public bool CreateLanguageFile(int newLanguageId)
        {
            if (newLanguageId == 0)
                throw new Exception(ThrowMessage.LanguageNotExists.ToString());
            return _languageFileService.CreateFile(newLanguageId);
        }
        public bool PutLanguageKey(LanguageKeyModel languageKey)
        {
            return _languageFileService.PutLanguageKey(languageKey);
        }
      
        public string PublishLanguage(Language language)
        {
            CheckLanguageExistsOnIrremovableList(language.Id);
            VerifyLanguageToPublish(language);
            LanguageDao entity = ValidateGetLanguageDaoById(language.Id);
            entity.IsPublic = language.IsPublic;
            RepositoryFactory.Context.SaveChanges();
            return language.IsPublic ? ThrowMessage.LanguageHasBeenPublished.ToString() : ThrowMessage.LanguageHasBeenNonpublic.ToString();
        }

        public bool PutLanguage(Language language)
        {
            LanguageDao entity = ValidateGetLanguageDaoById(language.Id);            
            entity.LanguageName = language.LanguageName;
            entity.LanguageShortName = language.LanguageShortName;
            RepositoryFactory.Context.SaveChanges();
            return true;
        }

        public bool DeleteLanguage(Language language)
        {
            CheckLanguageExistsOnIrremovableList(language.Id);
            LanguageDao entity = ValidateGetLanguageDaoById(language.Id);            
            RepositoryFactory.Context.Languages.Remove(entity);
            DeleteLanguageFromOtherStructure(language.Id);
            bool removeLanguageFileResult = _languageFileService.RemoveFile(language.Id);
            if (removeLanguageFileResult)
            {
                RepositoryFactory.Context.SaveChanges();
                return true;
            }
            throw new Exception(ThrowMessage.LanguageCanNotBeRemoved.ToString());
        }

        private void VerifyLanguageToPublish(Language language)
        {
            if (language.IsPublic)
            {
                if (_languageFileService.GetFileEmptyKeys(language.Id) > 0)
                    throw new Exception(ThrowMessage.EmptyKeysExists.ToString());
            }
            else
            {
                if (RepositoryFactory.Context.Languages.Count(x => x.IsPublic) == 1)
                    throw new Exception(ThrowMessage.OnlyOnePublicLanguage.ToString());
            }
        }

        private void CheckLanguageExistsOnIrremovableList(int languageId)
        {
            if (languageId <= 2) throw new ArgumentOutOfRangeException(ThrowMessage.LanguageCanNotBeRemoved.ToString());
        }

        private Language ValidateGetLanguageByName(string languageName)
        {
            var language = ModelMapper.Map<Language, LanguageDao>(
                    RepositoryFactory.Context.Languages.FirstOrDefault(x => x.LanguageName == languageName));
            if(language == null)
                ThrowLanguageNotExists();
            return language;

        }

        private LanguageDao ValidateGetLanguageDaoById(int languageid)
        {
            var language = RepositoryFactory.Context.Languages.FirstOrDefault(x => x.Id == languageid);
            if (language == null)
                ThrowLanguageNotExists();
            return language;
        }

        private void ValidateGetLanguageDaoByName(string languageName)
        {
            if (RepositoryFactory.Context.Languages.FirstOrDefault(x => x.LanguageName == languageName) != null)
                throw new Exception(ThrowMessage.LanguageExists.ToString());
        }

        private void ThrowLanguageNotExists()
        {
            throw new Exception(ThrowMessage.LanguageNotExists.ToString());
        }
        private void DeleteLanguageFromOtherStructure(int languageId)
        {
            DeleteLanguageFromJokes(languageId);
            DeleteLanguageFromIntegrationGames(languageId);
            DeleteLanguageFromVersions(languageId);
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
