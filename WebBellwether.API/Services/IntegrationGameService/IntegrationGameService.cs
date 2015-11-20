using System.Collections.Generic;
using System.Linq;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Models;
using WebBellwether.API.Models.IntegrationGame;
using WebBellwether.API.Results;
using WebBellwether.API.Services.IntegrationGameService.Abstract;
using WebBellwether.API.Repositories.Abstract;

namespace WebBellwether.API.Services.IntegrationGameService
{
    public class IntegrationGameService:IIntegrationGameService
    {
        private readonly IAggregateRepositories _repository;
        private readonly IManagementFeaturesService _managementFeaturesService;
        private readonly IManagementIntegrationGamesService _managementIntegrationGamesService;

        public IntegrationGameService(IAggregateRepositories repository)
        {
            _repository = repository;
            _managementFeaturesService = new ManagementFeaturesService(repository);
            _managementIntegrationGamesService = new ManagementIntegrationGamesService(repository);
        }

        public List<IntegrationGameModel> GetIntegrationGames(int language)
        {
            var games = new List<IntegrationGameModel>();
            var entity = _repository.IntegrationGameDetailRepository.GetWithInclude(x => x.Language.Id == language).ToList();
            entity.ForEach(x =>
            {
                games.Add(new IntegrationGameModel
                {
                    Id = x.IntegrationGame.Id,
                    Language = x.Language,
                    GameName = x.IntegrationGameName,
                    GameDescription = x.IntegrationGameDescription,
                    IntegrationGameDetailModels = FillGameDetailModel(x.Id) //i take id from integrationgamedetails
                });
            });
            return games;
        }
        public IntegrationGameModel GetGameTranslation(int gameId,int languageId)
        {
            return _managementIntegrationGamesService.GetGameTranslation(gameId,languageId);
        }

        public ResultStateContainer InsertIntegrationGame(NewIntegrationGameModel game)
        {
            ResultStateContainer result = _managementIntegrationGamesService.InsertIntegrationGame(game);
            if (result.ResultState == ResultState.Success && result.ResultMessage == ResultMessage.SeveralLanguageGameAdded)
                return result;
            else if (result.ResultState == ResultState.Success && result.ResultMessage == ResultMessage.GameAdded)
            {
                List<Language> languages = _repository.LanguageRepository.GetAll().ToList();
                //fill rest of data
                IntegrationGameModel tempGame = (IntegrationGameModel)result.ResultValue;
                tempGame.IntegrationGameDetailModels = FillGameDetailModel(tempGame.IntegrationGameId);
                tempGame.GameTranslations = FillAvailableTranslation(tempGame.Id, languages);
                result.ResultValue = tempGame;
                return result;
            }
            else return result;
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
            List<Language> languages = _repository.LanguageRepository.GetAll().ToList();
            var games = new List<IntegrationGameModel>();
            var entity = _repository.IntegrationGameDetailRepository.GetWithInclude(x => x.Language.Id == language).ToList();
            entity.ForEach(x =>
            {
                games.Add(new IntegrationGameModel
                {
                    Id = x.IntegrationGame.Id, // this is global id 
                    IntegrationGameId = x.Id, // id for translation
                    Language = x.Language,
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
            integrationGame.GameTranslations = FillAvailableTranslation(integrationGame.Id, _repository.LanguageRepository.GetAll().ToList());
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
        
       
        public List<AvailableLanguage> FillAvailableTranslation(int gameId, List<Language> allLanguages)
        {
            var translation = new List<AvailableLanguage>();
            _repository.IntegrationGameDetailRepository.GetWithInclude(x => x.IntegrationGame.Id == gameId).ToList().ForEach(z => translation.Add(new AvailableLanguage { Language = z.Language, HasTranslation = true }));
            allLanguages.ForEach(x =>
            {
                if (translation.FirstOrDefault(y => y.Language.Id == x.Id) == null)
                    translation.Add(new AvailableLanguage { Language = x, HasTranslation = false });
            });
            return translation;
        }
        public List<IntegrationGameDetailModel> FillGameDetailModel(int integrationGameDetailId)
        {
            List<IntegrationGameDetailModel> result = new List<IntegrationGameDetailModel>();
            _repository.IntegrationGameFeatureRepository.GetWithInclude(x => x.IntegrationGameDetail.Id == integrationGameDetailId).ToList().ForEach(
                z =>
                {
                    result.Add(new IntegrationGameDetailModel
                    {
                        Id = z.GameFeatureDetailLanguage.Id,
                        GameFeatureId = z.GameFeatureLanguage.GameFeature.Id,
                        GameFeatureLanguageId = z.GameFeatureLanguage.Id,
                        GameFeatureName = z.GameFeatureLanguage.GameFeatureName,
                        GameFeatureDetailId = z.GameFeatureDetailLanguage.GameFeatureDetail.Id,
                        GameFeatureDetailName = z.GameFeatureDetailLanguage.GameFeatureDetailName
                    });
                });
            return result;
        }       
    }
}