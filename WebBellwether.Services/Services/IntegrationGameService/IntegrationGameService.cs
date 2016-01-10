using System.Collections.Generic;
using System.Linq;
using WebBellwether.Models.ViewModels.IntegrationGame;
using WebBellwether.Services.Factories;

namespace WebBellwether.Services.Services.IntegrationGameService
{
    public interface IIntegrationGameService
    {
        DirectIntegrationGameViewModel[] GetIntegrationGames(int languageId);
    }
    public class IntegrationGameService : IIntegrationGameService
    {        
        public DirectIntegrationGameViewModel[] GetIntegrationGames(int languageId)
        {
            var games = new List<DirectIntegrationGameViewModel>();
            RepositoryFactory.Context.IntegrationGameDetails.Where(x => x.Language.Id == languageId).ToList().ForEach(x =>
            {
                var gameFeatureDetailsNames = GetGameFeatureDetailName(x.Id).ToArray();
                games.Add(new DirectIntegrationGameViewModel()
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
            return games.ToArray();
        }
        private IEnumerable<string> GetGameFeatureDetailName(int integrationGameDetailId)
        {
            return
                RepositoryFactory.Context.IntegrationGameFeatures.Where(
                    x => x.IntegrationGameDetail.Id == integrationGameDetailId)
                    .OrderBy(x => x.GameFeatureLanguage.GameFeature.Id)
                    .Select(x => x.GameFeatureDetailLanguage.GameFeatureDetailName);                
        }
    }
}