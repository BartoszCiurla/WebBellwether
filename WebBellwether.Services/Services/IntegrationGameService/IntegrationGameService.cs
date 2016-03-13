using System.Collections.Generic;
using System.Linq;
using WebBellwether.Models.ViewModels.IntegrationGame;
using WebBellwether.Services.Factories;

namespace WebBellwether.Services.Services.IntegrationGameService
{
    public interface IIntegrationGameService
    {
        DirectIntegrationGameViewModel[] GetIntegrationGames(int languageId);
        SimpleIntegrationGameViewModel[] GetSimpleIntegrationGames(int languageId);
    }
    public class IntegrationGameService : IIntegrationGameService
    {        
        public DirectIntegrationGameViewModel[] GetIntegrationGames(int languageId)
        {
            var games = new List<DirectIntegrationGameViewModel>();
            RepositoryFactory.Context.IntegrationGameDetails.Where(x => x.Language.Id == languageId).ToList().ForEach(x =>
            {
                var gameFeatureDetailsNames = GetGameFeatureDetailName(x.Id).ToArray();
                if (gameFeatureDetailsNames.Length == 4)
                games.Add(new DirectIntegrationGameViewModel()
                {
                    Id = x.IntegrationGame.Id,
                    IntegrationGameId = x.Id,
                    GameName = x.IntegrationGameName,
                    GameDescription = x.IntegrationGameDescription,
                    CategoryGame = gameFeatureDetailsNames[0],
                    PaceOfPlay = gameFeatureDetailsNames[1],
                    NumberOfPlayer = gameFeatureDetailsNames[2],
                    PreparationFun = gameFeatureDetailsNames[3],
                    LanguageId = x.Language.Id
                });
            });
            return games.ToArray();
        }

        public SimpleIntegrationGameViewModel[] GetSimpleIntegrationGames(int languageId)
        {
            var games = new List<SimpleIntegrationGameViewModel>();
            RepositoryFactory.Context.IntegrationGameDetails.Where(x=>x.Language.Id == languageId).ToList().ForEach(
                x =>
                {
                    games.Add(new SimpleIntegrationGameViewModel
                    {
                        Id = x.IntegrationGame.Id,
                        GameName =  x.IntegrationGameName,
                        GameDescription = x.IntegrationGameDescription,
                        LanguageId = x.Language.Id,
                        GameFeatures = GetGameFeatureDetailId(x.Id)
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

        private int[] GetGameFeatureDetailId(int integrationGameDetailId)
        {
            return
                RepositoryFactory.Context.IntegrationGameFeatures.Where(
                    x => x.IntegrationGameDetail.Id == integrationGameDetailId)
                    .OrderBy(x => x.GameFeatureLanguage.GameFeature.Id)
                    .Select(x => x.GameFeatureDetailLanguage.GameFeatureDetail.Id).ToArray();
        } 
    }
}