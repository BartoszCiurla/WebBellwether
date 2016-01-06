using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using WebBellwether.Models.Models.Version;
using WebBellwether.Repositories.Entities.Translations;
using WebBellwether.Repositories.Entities.Version;
using WebBellwether.Services.Factories;

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
        public ClientVersionModel GetVersion(int languageId)
        {
            return new ClientVersionModel
            {
                LanguageVersion = RepositoryFactory.Context.LanguageVersions.Where(x => x.Language.Id == languageId).Max(x => x.Version),
                IntegrationGameVersion = RepositoryFactory.Context.IntegrationGameVersions.Where(x => x.Language.Id == languageId)
                    .Max(x => x.Version),
                JokeCategoryVersion = RepositoryFactory.Context.JokeCategoryVersions.Where(x => x.Language.Id == languageId)
                    .Max(x => x.Version),
                JokeVersion = RepositoryFactory.Context.JokeCategoryVersions.Where(x => x.Language.Id == languageId)
                    .Max(x => x.Version)
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

        public bool ChooseTargetAndFunction(VersionModel version, bool addOrRemove)
        {
            if (version.VersionTarget.Equals("language"))
                return addOrRemove ? AddLanguageVersion(version) : DeleteLanguageVersion(version);
            if (version.VersionTarget.Equals("integrationGame"))
                return addOrRemove ? AddIntegrationGameVersion(version) : DeleteIntegrationGameVersion(version);
            if (version.VersionTarget.Equals("jokeCategory"))
                return addOrRemove ? AddJokeCategoryVersion(version) : DeleteJokeCategoryVersion(version);
            if (version.VersionTarget.Equals("joke"))
                return addOrRemove ? AddJokeVersion(version) : DeleteJokeVersion(version);
            return false;
        }

        private bool DeleteLanguageVersion(VersionModel versionForDelete)
        {
            var entityToDelete =
                RepositoryFactory.Context.LanguageVersions.FirstOrDefault(
                    x =>
                        x.Language.Id.Equals(versionForDelete.LanguageId) &&
                        x.Version.Equals(versionForDelete.VersionNumber));
            if (entityToDelete == null)
                return false;
            RepositoryFactory.Context.LanguageVersions.Remove(entityToDelete);
            RepositoryFactory.Context.SaveChanges();
            return true;
        }

        private bool DeleteIntegrationGameVersion(VersionModel versionForDelete)
        {
            var entityToDelete =
                RepositoryFactory.Context.IntegrationGameVersions.FirstOrDefault(
                    x =>
                        x.Language.Id.Equals(versionForDelete.LanguageId) &&
                        x.Version.Equals(versionForDelete.VersionNumber));
            if (entityToDelete == null)
                return false;
            RepositoryFactory.Context.IntegrationGameVersions.Add(entityToDelete);
            RepositoryFactory.Context.SaveChanges();
            return true;
        }

        private bool DeleteJokeCategoryVersion(VersionModel versionForDelete)
        {
            var entityToDelete =
                RepositoryFactory.Context.JokeCategoryVersions.FirstOrDefault(
                    x =>
                        x.Language.Id.Equals(versionForDelete.LanguageId) &&
                                             x.Version.Equals(versionForDelete.VersionNumber));
            if (entityToDelete == null)
                return false;
            RepositoryFactory.Context.JokeCategoryVersions.Remove(entityToDelete);
            RepositoryFactory.Context.SaveChanges();
            return true;
        }

        private bool DeleteJokeVersion(VersionModel versionForDelete)
        {
            var entityTodelete = RepositoryFactory.Context.JokeVersions.FirstOrDefault(x=>x.Language.Id.Equals(versionForDelete.LanguageId) && x.Version.Equals(versionForDelete.VersionNumber));
            if (entityTodelete == null)
                return false;
            RepositoryFactory.Context.JokeVersions.Remove(entityTodelete);
            RepositoryFactory.Context.SaveChanges();
            return true;
        }
        private LanguageDao GetLanguage(int languageId)
        {
            return RepositoryFactory.Context.Languages.FirstOrDefault(x => x.Id == languageId);
        }
        private bool AddLanguageVersion(VersionModel languageVersion)
        {
            RepositoryFactory.Context.LanguageVersions.Add(new LanguageVersionDao
            {
                NumberOfItemsInFileLanguage = languageVersion.NumberOf,
                Version = languageVersion.VersionNumber,
                Language = GetLanguage(languageVersion.LanguageId)
            });
            return true;
        }

        private bool AddIntegrationGameVersion(VersionModel integrationGameVersion)
        {
            RepositoryFactory.Context.IntegrationGameVersions.Add(new IntegrationGameVersionDao
            {
                NumberOfIntegrationGames = integrationGameVersion.NumberOf,
                Version = integrationGameVersion.VersionNumber,
                Language = GetLanguage(integrationGameVersion.LanguageId)
            });
            RepositoryFactory.Context.SaveChanges();
            return true;
        }

        private bool AddJokeCategoryVersion(VersionModel jokeCategoryVersion)
        {
            RepositoryFactory.Context.JokeCategoryVersions.Add(new JokeCategoryVersionDao
            {
                NumberOfJokeCategory = jokeCategoryVersion.NumberOf,
                Version = jokeCategoryVersion.VersionNumber,
                Language = GetLanguage(jokeCategoryVersion.LanguageId)
            });
            RepositoryFactory.Context.SaveChanges();
            return true;
        }

        private bool AddJokeVersion(VersionModel jokeVersion)
        {
            RepositoryFactory.Context.JokeVersions.Add(new JokeVersionDao
            {
                NumberOfJokes = jokeVersion.NumberOf,
                Version = jokeVersion.VersionNumber,
                Language = GetLanguage(jokeVersion.LanguageId)
            });
            RepositoryFactory.Context.SaveChanges();           
            return true;
        }
        private CurrentVersionDetailStateModel FillCurrentVersionDetail(int languageId)
        {
            string _destinationPlace =
             @"E:\PRACA INŻYNIERSKA\WebBelwether New\WebBellwether\WebBellwether.Web\appData\translations\translation_";
            string languageFileLocation = $"{_destinationPlace}{languageId}{".json"}";
            string templateJson = File.ReadAllText(languageFileLocation);
            var dictionaryJson = JsonConvert.DeserializeObject<Dictionary<string, string>>(templateJson);
            return new CurrentVersionDetailStateModel
            {
                NumberOfItemsInFileLanguage = dictionaryJson.Count(),
                NumberOfIntegrationGames =
                    RepositoryFactory.Context.IntegrationGameDetails.Count(x => x.Language.Id == languageId),
                NumberOfJokes = RepositoryFactory.Context.JokeCategoryDetails.Count(x => x.Language.Id == languageId),
                NumberOfJokeCategory = RepositoryFactory.Context.JokeDetails.Count(x => x.Language.Id == languageId)
            };
        }

        private VersionDetailModel[] FillLanguageVersion(int languageId)
        {
            return
                RepositoryFactory.Context.LanguageVersions.Where(x => x.Language.Id == languageId)
                    .ToList()
                    .Select(
                        x =>
                            new VersionDetailModel
                            {
                                Id = x.Id,
                                NumberOf = x.NumberOfItemsInFileLanguage,
                                VersionNumber = x.Version
                            })
                    .ToArray();
        }

        private VersionDetailModel[] FillIntegrationGameVersion(int languageId)
        {
            return
                RepositoryFactory.Context.IntegrationGameVersions.Where(x => x.Language.Id == languageId)
                    .ToList()
                    .Select(x => new VersionDetailModel { Id = x.Id, NumberOf = x.NumberOfIntegrationGames, VersionNumber = x.Version })
                    .ToArray();
        }

        private VersionDetailModel[] FillJokeCategoryVersion(int languageId)
        {
            return
                RepositoryFactory.Context.JokeCategoryVersions.Where(x => x.Language.Id == languageId)
                    .ToList()
                    .Select(x => new VersionDetailModel { Id = x.Id, NumberOf = x.NumberOfJokeCategory, VersionNumber = x.Version })
                    .ToArray();
        }

        private VersionDetailModel[] FillJokeVersion(int languageId)
        {

            return
                RepositoryFactory.Context.JokeVersions.Where(x => x.Language.Id == languageId)
                    .ToList()
                    .Select(
                        x => new VersionDetailModel { Id = x.Id, NumberOf = x.NumberOfJokes, VersionNumber = x.Version })
                    .ToArray();
        }
    }
}
