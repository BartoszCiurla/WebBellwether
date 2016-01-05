using System.Collections.Generic;
using WebBellwether.Models.Models.IntegrationGame;
using WebBellwether.Models.Models.Translation;
using WebBellwether.Models.Results;
using WebBellwether.Repositories.Entities.Translations;

namespace WebBellwether.Services.Services.IntegrationGameService.Abstract
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
        IntegrationGameModel GetGameTranslation(int gameId,int languageId);
        bool CreateGameFeatures(int languageId);

    }
}
