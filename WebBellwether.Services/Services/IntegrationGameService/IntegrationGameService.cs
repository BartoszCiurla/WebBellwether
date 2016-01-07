using System.Collections.Generic;
using System.Linq;
using WebBellwether.Models.Models.IntegrationGame;
using WebBellwether.Models.Models.Translation;
using WebBellwether.Models.Results;
using WebBellwether.Repositories.Entities.Translations;
using WebBellwether.Services.Factories;
using WebBellwether.Services.Utility;

namespace WebBellwether.Services.Services.IntegrationGameService
{
    public interface IIntegrationGameService
    {
        IEnumerable<DirectIntegrationGameModel> GetIntegrationGames(int language);
        List<IntegrationGameModel> GetIntegrationGamesWithAvailableLanguages(int language);
        ResultMessage PutGameFeature(GameFeatureModel gameFeatureModel);
        ResultMessage PutGameFeatureDetail(GameFeatureDetailModel gameFeatureDetailModel);
        List<GameFeatureDetailModel> GetGameFeatureDetails(int language);
        List<AvailableLanguage> FillAvailableTranslation(int gameId, List<LanguageDao> allLanguages);
        List<IntegrationGameDetailModel> FillGameDetailModel(int integrationGameDetailId);
        ResultStateContainer InsertIntegrationGame(NewIntegrationGameModel game);
        List<GameFeatureModel> GetGameFeatures(int language);
        List<GameFeatureModel> GetGameFeatuesModelWithDetails(int language);
        ResultStateContainer PutIntegrationGame(IntegrationGameModel integrationGame);
        ResultStateContainer DeleteIntegratiomGame(IntegrationGameModel integrationGame);
        IntegrationGameModel GetGameTranslation(int gameId, int languageId);
        bool CreateGameFeatures(int languageId);
    }
    public class IntegrationGameService : IIntegrationGameService
    {
        private readonly IManagementFeaturesService _managementFeaturesService;
        private readonly IManagementIntegrationGamesService _managementIntegrationGamesService;

        public IntegrationGameService()
        {
            _managementFeaturesService = new ManagementFeaturesService();
            _managementIntegrationGamesService = new ManagementIntegrationGamesService();
        }

        public IEnumerable<DirectIntegrationGameModel> GetIntegrationGames(int language)
        {
            var games = new List<DirectIntegrationGameModel>();
            RepositoryFactory.Context.IntegrationGameDetails.Where(x => x.Language.Id == language).ToList().ForEach(x =>
            {
                var gameFeatureDetailsNames = GetGameFeatureDetailName(x.Id).ToArray();
                games.Add(new DirectIntegrationGameModel()
                {
                    Id = x.IntegrationGame.Id,
                    IntegrationGameId = x.Id,
                    GameName = x.IntegrationGameName,
                    GameDescription = x.IntegrationGameDescription,
                    CategoryGame = gameFeatureDetailsNames[0],
                    PaceOfPlay = gameFeatureDetailsNames[1],
                    NumberOfPlayer = gameFeatureDetailsNames[2],
                    PreparationFun = gameFeatureDetailsNames[3]
                });
            });
            return games;
        }

        private IEnumerable<string> GetGameFeatureDetailName(int integrationGameDetailId)
        {
            return
                RepositoryFactory.Context.IntegrationGameFeatures.Where(
                    x => x.IntegrationGameDetail.Id == integrationGameDetailId)
                    .OrderBy(x => x.GameFeatureLanguage.GameFeature.Id)
                    .Select(x => x.GameFeatureDetailLanguage.GameFeatureDetailName);                
        }

        public IntegrationGameModel GetGameTranslation(int gameId, int languageId)
        {
            return _managementIntegrationGamesService.GetGameTranslation(gameId, languageId);
        }

        public ResultStateContainer InsertIntegrationGame(NewIntegrationGameModel game)
        {
            ResultStateContainer result = _managementIntegrationGamesService.InsertIntegrationGame(game);
            if (result.ResultState == ResultState.Success && result.ResultMessage == ResultMessage.SeveralLanguageGameAdded)
                return result;
            if (result.ResultState == ResultState.Success && result.ResultMessage == ResultMessage.GameAdded)
            {
                List<LanguageDao> languages = RepositoryFactory.Context.Languages.ToList();
                //fill rest of data
                IntegrationGameModel tempGame = (IntegrationGameModel)result.ResultValue;
                tempGame.IntegrationGameDetailModels = FillGameDetailModel(tempGame.IntegrationGameId);
                tempGame.GameTranslations = FillAvailableTranslation(tempGame.Id, languages);
                result.ResultValue = tempGame;
                return result;
            }
            return result;
        }

        public List<GameFeatureModel> GetGameFeatures(int language)
        {
            return _managementIntegrationGamesService.GetGameFeatures(language);

        }

        public List<GameFeatureModel> GetGameFeatuesModelWithDetails(int language)
        {
            return _managementFeaturesService.GetGameFeatuesModelWithDetails(language);
        }
        public List<IntegrationGameModel> GetIntegrationGamesWithAvailableLanguages(int language)
        {
            List<LanguageDao> languages = RepositoryFactory.Context.Languages.ToList();
            var gameDetailsDao =
                RepositoryFactory.Context.IntegrationGameDetails.Where(x => x.Language.Id == language).ToList();
            if (!gameDetailsDao.Any())
                return null;
            var games = new List<IntegrationGameModel>();
            gameDetailsDao.ForEach(x =>
            {
                games.Add(new IntegrationGameModel
                {
                    Id = x.IntegrationGame.Id, // this is global id 
                    IntegrationGameId = x.Id, // id for translation
                    Language = ModelMapper.Map<Language,LanguageDao>(x.Language), 
                    GameName = x.IntegrationGameName,
                    GameDescription = x.IntegrationGameDescription,
                    IntegrationGameDetailModels = FillGameDetailModel(x.Id), //i take id from integrationgamedetails
                    GameTranslations = FillAvailableTranslation(x.IntegrationGame.Id, languages)
                });
            });
            return games;
        }
        public ResultStateContainer DeleteIntegratiomGame(IntegrationGameModel integrationGame)
        {
            return _managementIntegrationGamesService.DeleteIntegratiomGame(integrationGame);
        }
        public ResultStateContainer PutIntegrationGame(IntegrationGameModel integrationGame)
        {
            integrationGame.GameTranslations = FillAvailableTranslation(integrationGame.Id,
                RepositoryFactory.Context.Languages.ToList());
            return _managementIntegrationGamesService.PutIntegrationGame(integrationGame);
        }

        public ResultMessage PutGameFeature(GameFeatureModel gameFeatureModel)
        {
            return _managementFeaturesService.PutGameFeature(gameFeatureModel);

        }
        public ResultMessage PutGameFeatureDetail(GameFeatureDetailModel gameFeatureDetailModel)
        {
            return _managementFeaturesService.PutGameFeatureDetail(gameFeatureDetailModel);
        }

        public List<GameFeatureDetailModel> GetGameFeatureDetails(int language)
        {
            return _managementFeaturesService.GetGameFeatureDetails(language);
        }


        public List<AvailableLanguage> FillAvailableTranslation(int gameId, List<LanguageDao> allLanguages)
        {
            var translation =
                RepositoryFactory.Context.IntegrationGameDetails.Where(x => x.IntegrationGame.Id == gameId).ToList()
                    .Select(
                        z =>
                            new AvailableLanguage
                            {
                                Language = ModelMapper.Map<Language,LanguageDao>(z.Language),                                    
                                HasTranslation = true
                            }).ToList();
            allLanguages.ForEach(x =>
            {
                if (translation.FirstOrDefault(y => y.Language.Id == x.Id) == null)
                    translation.Add(new AvailableLanguage { Language = ModelMapper.Map<Language,LanguageDao>(x), HasTranslation = false });
            });
            return translation;
        }
        public List<IntegrationGameDetailModel> FillGameDetailModel(int integrationGameDetailId)
        {
            List<IntegrationGameDetailModel> result =
                RepositoryFactory.Context.IntegrationGameFeatures.Where(
                    x => x.IntegrationGameDetail.Id == integrationGameDetailId)
                    .Select(z => new IntegrationGameDetailModel
                    {
                        Id = z.GameFeatureDetailLanguage.Id,
                        GameFeatureId = z.GameFeatureLanguage.GameFeature.Id,
                        GameFeatureLanguageId = z.GameFeatureLanguage.Id,
                        GameFeatureName = z.GameFeatureLanguage.GameFeatureName,
                        GameFeatureDetailId = z.GameFeatureDetailLanguage.GameFeatureDetail.Id,
                        GameFeatureDetailName = z.GameFeatureDetailLanguage.GameFeatureDetailName
                    }).ToList();
            return result;
        }
        public bool CreateGameFeatures(int languageId)
        {
            return _managementFeaturesService.CreateGameFeatures(languageId);
        }
    }
}