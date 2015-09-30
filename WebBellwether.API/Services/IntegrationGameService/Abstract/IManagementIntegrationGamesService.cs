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
        IntegrationGameDetail BuildIntegrationGameDetail(NewIntegrationGameModel game);
        Language GetLanguage(int id);
        List<GameFeatureModel> GetGameFeatures(int language);
        void BuildFeaturesTemplate(int language, List<GameFeatureModel> gameFeatures);
        List<IntegrationGameFeature> GetGameFeatures(int[] features, int language);
        ResultStateContainer InsertSeveralLanguageGame(NewIntegrationGameModel game);
        ResultState CheckNewGameLanguage(NewIntegrationGameModel game);
        ResultStateContainer InsertIntegrationGame(NewIntegrationGameModel game);
        ResultState PutIntegrationGame(IntegrationGameModel integrationGame);
    }
}
