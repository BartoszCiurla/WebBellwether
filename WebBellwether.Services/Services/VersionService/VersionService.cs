using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using WebBellwether.Models.Models.Version;
using WebBellwether.Repositories.Entities.Translations;
using WebBellwether.Repositories.Entities.Version;
using WebBellwether.Repositories.Repositories;
using WebBellwether.Repositories.Repositories.Abstract;

namespace WebBellwether.Services.Services.VersionService
{
    public interface IVersionService
    {
        VersionAggregateModel GetVersionDetailsForLanguage(int languageId);
        bool ChooseTargetAndFunction(VersionModel version, bool trueAddVersionFalseRemove);
        ClientVersionModel GetVersion(int languageId);
    }
    public class VersionService : IVersionService
    {
        private readonly IAggregateRepositories _repository;
        public VersionService()
        {
            _repository = new AggregateRepositories();
        }

        public ClientVersionModel GetVersion(int languageId)
        {
            var languageVersion =
                _repository.LanguageVersionRepository.GetWithInclude(x => x.Language.Id == languageId)
                    .Max(x => x.Version);
            var integrationGameVersion =
                _repository.IntegrationGameVersionRepository.GetWithInclude(x => x.Language.Id == languageId)
                    .Max(x => x.Version);
            var jokeCategoryVersion =
                _repository.JokeCategoryVersionRepository.GetWithInclude(x => x.Language.Id == languageId)
                    .Max(x => x.Version);
            var jokeVersion =
                _repository.JokeVersionRepository.GetWithInclude(x => x.Language.Id == languageId).Max(x => x.Version);
            return new ClientVersionModel
            {
                LanguageVersion = languageVersion,
                IntegrationGameVersion = integrationGameVersion,
                JokeCategoryVersion = jokeCategoryVersion,
                JokeVersion = jokeVersion
            };
        }

        public VersionAggregateModel GetVersionDetailsForLanguage(int languageId)
        {
            return new VersionAggregateModel
            {
                LanguageVersions = FillLanguageVersion(languageId),
                IntegrationGameVersions = FillIntegrationGameVersion(languageId),
                JokeCategoryVersions = FillJokeCategoryVersion(languageId),
                JokeVersions = FillJokeVersion(languageId),
                CurrentVersionStateModel = FillCurrentVersionDetail(languageId)
            };
        }

        public bool ChooseTargetAndFunction(VersionModel version, bool trueAddVersionFalseRemove)
        {
            if (version.VersionTarget == "language")
                return trueAddVersionFalseRemove ? AddLanguageVersion(version):DeleteLanguageVersion(version);
            if (version.VersionTarget == "integrationGame")
                return trueAddVersionFalseRemove ? AddIntegrationGameVersion(version):DeleteIntegrationGameVersion(version);
            if (version.VersionTarget == "jokeCategory")
                return trueAddVersionFalseRemove ? AddJokeCategoryVersion(version) : DeleteJokeCategoryVersion(version);
            if (version.VersionTarget == "joke")
                return trueAddVersionFalseRemove ? AddJokeVersion(version) : DeleteJokeVersion(version);
            return false;
        }

        private bool DeleteLanguageVersion(VersionModel versionForDelete)
        {
            var entityToDelete =
                _repository.LanguageVersionRepository.GetWithInclude(x => x.Language.Id.Equals(versionForDelete.LanguageId) && x.Version.Equals(versionForDelete.VersionNumber)).FirstOrDefault();
            _repository.LanguageVersionRepository.Delete(entityToDelete);
            _repository.Save();
            return true;
        }

        private bool DeleteIntegrationGameVersion(VersionModel versionForDelete)
        {
            var entityToDelete =
                _repository.IntegrationGameVersionRepository.GetWithInclude(x=>x.Language.Id.Equals(versionForDelete.LanguageId) && x.Version.Equals(versionForDelete.VersionNumber))
                    .FirstOrDefault();
            _repository.IntegrationGameVersionRepository.Delete(entityToDelete);
            _repository.Save();
            return true;
        }

        private bool DeleteJokeCategoryVersion(VersionModel versionForDelete)
        {
            var entityToDelete =
                _repository.JokeCategoryVersionRepository.GetWithInclude(x => x.Language.Id.Equals(versionForDelete.LanguageId) && x.Version.Equals(versionForDelete.VersionNumber))
                    .FirstOrDefault();
            _repository.JokeCategoryVersionRepository.Delete(entityToDelete);
            _repository.Save();
            return true;
        }

        private bool DeleteJokeVersion(VersionModel versionForDelete)
        {
            var entityTodelete =
                _repository.JokeVersionRepository.GetWithInclude(x => x.Language.Id.Equals(versionForDelete.LanguageId) && x.Version.Equals(versionForDelete.VersionNumber)).FirstOrDefault();
            _repository.JokeVersionRepository.Delete(entityTodelete);
            _repository.Save();
            return true;
        }
        private LanguageDao GetLanguage(int languageId)
        {
            return _repository.LanguageRepository.GetWithInclude(x => x.Id == languageId).FirstOrDefault();
        }
        private bool AddLanguageVersion(VersionModel languageVersion)
        {
            _repository.LanguageVersionRepository.Insert(new LanguageVersionDao { NumberOfItemsInFileLanguage = languageVersion.NumberOf, Version = languageVersion.VersionNumber, Language = GetLanguage(languageVersion.LanguageId) });
            _repository.Save();
            return true;
        }

        private bool AddIntegrationGameVersion(VersionModel integrationGameVersion)
        {
            _repository.IntegrationGameVersionRepository.Insert(new IntegrationGameVersionDao { NumberOfIntegrationGames = integrationGameVersion.NumberOf, Version = integrationGameVersion.VersionNumber, Language = GetLanguage(integrationGameVersion.LanguageId) });
            _repository.Save();
            return true;
        }

        private bool AddJokeCategoryVersion(VersionModel jokeCategoryVersion)
        {
            _repository.JokeCategoryVersionRepository.Insert(new JokeCategoryVersionDao {NumberOfJokeCategory = jokeCategoryVersion.NumberOf,Version = jokeCategoryVersion.VersionNumber,Language = GetLanguage(jokeCategoryVersion.LanguageId)});
            _repository.Save();
            return true;
        }

        private bool AddJokeVersion(VersionModel jokeVersion)
        {
            _repository.JokeVersionRepository.Insert(new JokeVersionDao {NumberOfJokes = jokeVersion.NumberOf,Version = jokeVersion.VersionNumber,Language = GetLanguage(jokeVersion.LanguageId)});
            _repository.Save();
            return true;
        }
        private CurrentVersionDetailStateModel FillCurrentVersionDetail(int languageId)
        {
            var result = new CurrentVersionDetailStateModel();
            //to jest chwilowe rozwiązanie jutro idzie pełen reforge 
            string _destinationPlace =
                @"E:\PRACA INŻYNIERSKA\WebBelwether New\WebBellwether\WebBellwether.Web\appData\translations\translation_";
            string languageFileLocation = $"{_destinationPlace}{languageId}{".json"}";
            string templateJson = File.ReadAllText(languageFileLocation);
            var dictionaryJson = JsonConvert.DeserializeObject<Dictionary<string, string>>(templateJson);
            result.NumberOfItemsInFileLanguage = dictionaryJson.Count();
            result.NumberOfIntegrationGames =
                _repository.IntegrationGameDetailRepository.GetWithInclude(x => x.Language.Id == languageId).Count();
            result.NumberOfJokeCategory =
                _repository.JokeCategoryDetailRepository.GetWithInclude(x => x.Language.Id == languageId).Count();
            result.NumberOfJokes =
                _repository.JokeDetailRepository.GetWithInclude(x => x.Language.Id == languageId).Count();
            return result;
        }

        private IEnumerable<VersionDetailModel> FillLanguageVersion(int languageId)
        {
            var result = new List<VersionDetailModel>();
            _repository.LanguageVersionRepository.GetWithInclude(x => x.Language.Id == languageId).ToList().ForEach(x =>
              {
                  result.Add(new VersionDetailModel { VersionNumber = x.Version, NumberOf = x.NumberOfItemsInFileLanguage,Id = x.Id});
              });
            return result;
        }

        private IEnumerable<VersionDetailModel> FillIntegrationGameVersion(int languageId)
        {
            var result = new List<VersionDetailModel>();
            _repository.IntegrationGameVersionRepository.GetWithInclude(x => x.Language.Id == languageId).ToList().ForEach(
                x =>
                {
                    result.Add(new VersionDetailModel { VersionNumber = x.Version, NumberOf = x.NumberOfIntegrationGames,Id = x.Id});
                });
            return result;
        }

        private IEnumerable<VersionDetailModel> FillJokeCategoryVersion(int languageId)
        {
            var result = new List<VersionDetailModel>();
            _repository.JokeCategoryVersionRepository.GetWithInclude(x => x.Language.Id == languageId).ToList().ForEach(
                x =>
                {
                    result.Add(new VersionDetailModel { VersionNumber = x.Version, NumberOf = x.NumberOfJokeCategory, Id = x.Id });
                });
            return result;
        }

        private IEnumerable<VersionDetailModel> FillJokeVersion(int languageId)
        {
            var result = new List<VersionDetailModel>();
            _repository.JokeVersionRepository.GetWithInclude(x => x.Language.Id == languageId).ToList().ForEach(x =>
              {
                  result.Add(new VersionDetailModel { VersionNumber = x.Version, NumberOf = x.NumberOfJokes, Id = x.Id});
              });
            return result;
        }
    }
}
