using System.Collections.Generic;
using System.Linq;
using WebBellwether.Models.Models.Translation;
using WebBellwether.Models.ViewModels.Version;
using WebBellwether.Repositories.Entities.Translations;
using WebBellwether.Repositories.Entities.Version;
using WebBellwether.Services.Factories;
using WebBellwether.Services.Services.FileService;
using WebBellwether.Services.Utility;

namespace WebBellwether.Services.Services.VersionService
{
    public interface IVersionService
    {
        VersionAggregateViewModel GetVersionDetailsForLanguage(int languageId);
        bool ChooseTargetAndFunction(VersionViewModel version, bool trueAddVersionFalseRemove);
        ClientVersionViewModel GetVersion(int languageId);
    }
    public class VersionService : IVersionService
    {
        private readonly ILanguageFileService _languageFileService;
        public VersionService(ILanguageFileService languageFileService)
        {
            _languageFileService = languageFileService;
        }
        public ClientVersionViewModel GetVersion(int languageId)
        {
            return new ClientVersionViewModel
            {
                Language = ModelMapper.Map<Language, LanguageDao>(RepositoryFactory.Context.Languages.FirstOrDefault(x => x.Id == languageId)),
                LanguageVersion = RepositoryFactory.Context.LanguageVersions.Where(x => x.Language.Id == languageId).Max(x => x.Version),
                IntegrationGameVersion = RepositoryFactory.Context.IntegrationGameVersions.Where(x => x.Language.Id == languageId)
                    .Max(x => x.Version),
                JokeCategoryVersion = RepositoryFactory.Context.JokeCategoryVersions.Where(x => x.Language.Id == languageId)
                    .Max(x => x.Version),
                JokeVersion = RepositoryFactory.Context.JokeCategoryVersions.Where(x => x.Language.Id == languageId)
                    .Max(x => x.Version),
                GameFeatureVersion = RepositoryFactory.Context.GameFeatureVersions.Where(x => x.Language.Id == languageId).Max(x => x.Version)
            };
        }

        public VersionAggregateViewModel GetVersionDetailsForLanguage(int languageId)
        {
            return new VersionAggregateViewModel
            {
                LanguageVersions = FillLanguageVersion(languageId),
                IntegrationGameVersions = FillIntegrationGameVersion(languageId),
                JokeCategoryVersions = FillJokeCategoryVersion(languageId),
                JokeVersions = FillJokeVersion(languageId),
                CurrentVersionStateModel = FillCurrentVersionDetail(languageId),
                GameFeatureVersions = FillGameFeatureVersion(languageId)
            };
        }

        public bool ChooseTargetAndFunction(VersionViewModel version, bool addOrRemove)
        {
            if (version.VersionTarget.Equals("language"))
                return addOrRemove ? AddLanguageVersion(version) : DeleteLanguageVersion(version);
            if (version.VersionTarget.Equals("integrationGame"))
                return addOrRemove ? AddIntegrationGameVersion(version) : DeleteIntegrationGameVersion(version);
            if (version.VersionTarget.Equals("jokeCategory"))
                return addOrRemove ? AddJokeCategoryVersion(version) : DeleteJokeCategoryVersion(version);
            if (version.VersionTarget.Equals("joke"))
                return addOrRemove ? AddJokeVersion(version) : DeleteJokeVersion(version);
            if (version.VersionTarget.Equals("gameFeature"))
                return addOrRemove ? AddGameFeatureVersion(version) : DeleteGameFeatureVersion(version);
            return false;
        }

        private LanguageDao GetLanguageById(int languageId)
        {
            return RepositoryFactory.Context.Languages.FirstOrDefault(x => x.Id == languageId);
        }

        private bool DeleteLanguageVersion(VersionViewModel versionForDelete)
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

        private bool DeleteGameFeatureVersion(VersionViewModel versionForDelete)
        {
            var entityToDelete =
                RepositoryFactory.Context.GameFeatureVersions.FirstOrDefault(
                    x =>
                        x.Language.Id.Equals(versionForDelete.LanguageId) &&
                        x.Version.Equals(versionForDelete.VersionNumber));
            if (entityToDelete == null)
                return false;
            RepositoryFactory.Context.GameFeatureVersions.Remove(entityToDelete);
            RepositoryFactory.Context.SaveChanges();
            return true;
        }

        private bool DeleteIntegrationGameVersion(VersionViewModel versionForDelete)
        {
            var entityToDelete =
                RepositoryFactory.Context.IntegrationGameVersions.FirstOrDefault(
                    x =>
                        x.Language.Id.Equals(versionForDelete.LanguageId) &&
                        x.Version.Equals(versionForDelete.VersionNumber));
            if (entityToDelete == null)
                return false;
            RepositoryFactory.Context.IntegrationGameVersions.Remove(entityToDelete);
            RepositoryFactory.Context.SaveChanges();
            return true;
        }

        private bool DeleteJokeCategoryVersion(VersionViewModel versionForDelete)
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

        private bool DeleteJokeVersion(VersionViewModel versionForDelete)
        {
            var entityTodelete = RepositoryFactory.Context.JokeVersions.FirstOrDefault(x => x.Language.Id.Equals(versionForDelete.LanguageId) && x.Version.Equals(versionForDelete.VersionNumber));
            if (entityTodelete == null)
                return false;
            RepositoryFactory.Context.JokeVersions.Remove(entityTodelete);
            RepositoryFactory.Context.SaveChanges();
            return true;
        }
        private bool AddLanguageVersion(VersionViewModel languageVersion)
        {
            RepositoryFactory.Context.LanguageVersions.Add(new LanguageVersionDao
            {
                NumberOfItemsInFileLanguage = languageVersion.NumberOf,
                Version = languageVersion.VersionNumber,
                Language = GetLanguageById(languageVersion.LanguageId)
            });
            RepositoryFactory.Context.SaveChanges();
            return true;
        }

        private bool AddGameFeatureVersion(VersionViewModel gameFeatureVersion)
        {
            RepositoryFactory.Context.GameFeatureVersions.Add(new GameFeatureVersionDao
            {
                Version = gameFeatureVersion.VersionNumber,
                Language = GetLanguageById(gameFeatureVersion.LanguageId)
            });
            RepositoryFactory.Context.SaveChanges();
            return true;
        }

        private bool AddIntegrationGameVersion(VersionViewModel integrationGameVersion)
        {
            RepositoryFactory.Context.IntegrationGameVersions.Add(new IntegrationGameVersionDao
            {
                NumberOfIntegrationGames = integrationGameVersion.NumberOf,
                Version = integrationGameVersion.VersionNumber,
                Language = GetLanguageById(integrationGameVersion.LanguageId)
            });
            RepositoryFactory.Context.SaveChanges();
            return true;
        }

        private bool AddJokeCategoryVersion(VersionViewModel jokeCategoryVersion)
        {
            RepositoryFactory.Context.JokeCategoryVersions.Add(new JokeCategoryVersionDao
            {
                NumberOfJokeCategory = jokeCategoryVersion.NumberOf,
                Version = jokeCategoryVersion.VersionNumber,
                Language = GetLanguageById(jokeCategoryVersion.LanguageId)
            });
            RepositoryFactory.Context.SaveChanges();
            return true;
        }

        private bool AddJokeVersion(VersionViewModel jokeVersion)
        {
            RepositoryFactory.Context.JokeVersions.Add(new JokeVersionDao
            {
                NumberOfJokes = jokeVersion.NumberOf,
                Version = jokeVersion.VersionNumber,
                Language = GetLanguageById(jokeVersion.LanguageId)
            });
            RepositoryFactory.Context.SaveChanges();
            return true;
        }
        private CurrentVersionDetailStateViewModel FillCurrentVersionDetail(int languageId)
        {
            return new CurrentVersionDetailStateViewModel
            {
                NumberOfItemsInFileLanguage = _languageFileService.GetFile(languageId).Count(),
                NumberOfIntegrationGames =
                    RepositoryFactory.Context.IntegrationGameDetails.Count(x => x.Language.Id == languageId),
                NumberOfJokes = RepositoryFactory.Context.JokeCategoryDetails.Count(x => x.Language.Id == languageId),
                NumberOfJokeCategory = RepositoryFactory.Context.JokeDetails.Count(x => x.Language.Id == languageId)
            };
        }

        private List<VersionDetailViewModel> FillLanguageVersion(int languageId)
        {
            return
                RepositoryFactory.Context.LanguageVersions.Where(x => x.Language.Id == languageId)
                    .ToList()
                    .Select(
                        x =>
                            new VersionDetailViewModel
                            {
                                Id = x.Id,
                                NumberOf = x.NumberOfItemsInFileLanguage,
                                VersionNumber = x.Version
                            }).ToList();

        }

        private List<VersionDetailViewModel> FillIntegrationGameVersion(int languageId)
        {
            return
                RepositoryFactory.Context.IntegrationGameVersions.Where(x => x.Language.Id == languageId)
                    .ToList()
                    .Select(x => new VersionDetailViewModel { Id = x.Id, NumberOf = x.NumberOfIntegrationGames, VersionNumber = x.Version })
                    .ToList();
        }

        private List<VersionDetailViewModel> FillJokeCategoryVersion(int languageId)
        {
            return
                RepositoryFactory.Context.JokeCategoryVersions.Where(x => x.Language.Id == languageId)
                    .ToList()
                    .Select(x => new VersionDetailViewModel { Id = x.Id, NumberOf = x.NumberOfJokeCategory, VersionNumber = x.Version })
                    .ToList();
        }

        private List<VersionDetailViewModel> FillJokeVersion(int languageId)
        {

            return
                RepositoryFactory.Context.JokeVersions.Where(x => x.Language.Id == languageId)
                    .ToList()
                    .Select(
                        x => new VersionDetailViewModel { Id = x.Id, NumberOf = x.NumberOfJokes, VersionNumber = x.Version })
                    .ToList();
        }

        private List<VersionDetailViewModel> FillGameFeatureVersion(int languageId)
        {
            return
                RepositoryFactory.Context.GameFeatureVersions.Where(x => x.Language.Id == languageId)
                    .ToList()
                    .Select(x => new VersionDetailViewModel { Id = x.Id, VersionNumber = x.Version })
                    .ToList();
        }
    }
}
