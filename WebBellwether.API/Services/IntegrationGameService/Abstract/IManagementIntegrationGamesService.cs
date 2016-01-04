using System.Collections.Generic;
using WebBellwether.API.Entities.IntegrationGame;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Models.IntegrationGame;
using WebBellwether.API.Results;

namespace WebBellwether.API.Services.IntegrationGameService.Abstract
{
    public interface IManagementIntegrationGamesService
    {
        ResultStateContainer InsertSingleLanguageGame(NewIntegrationGameModel game);
        IntegrationGameDetailDao BuildIntegrationGameDetail(NewIntegrationGameModel game);
        LanguageDao GetLanguage(int id);
        List<GameFeatureModel> GetGameFeatures(int language);
        void BuildFeaturesTemplate(int language, List<GameFeatureModel> gameFeatures);
        List<IntegrationGameFeatureDao> GetGameFeatures(int[] features, int language);
        ResultStateContainer InsertSeveralLanguageGame(NewIntegrationGameModel game);
        ResultMessage CheckNewGameLanguage(NewIntegrationGameModel game);
        ResultStateContainer InsertIntegrationGame(NewIntegrationGameModel game);
        ResultStateContainer PutIntegrationGame(IntegrationGameModel integrationGame);
        ResultStateContainer DeleteIntegratiomGame(IntegrationGameModel integrationGame);
        IntegrationGameModel GetGameTranslation(int gameId,int languageId);
    }
}
