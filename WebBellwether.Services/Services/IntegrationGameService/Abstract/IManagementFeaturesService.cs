using System.Collections.Generic;
using WebBellwether.Models.Models.IntegrationGame;
using WebBellwether.Models.Results;

namespace WebBellwether.Services.Services.IntegrationGameService.Abstract
{
    public interface IManagementFeaturesService
    {
        ResultMessage PutGameFeature(GameFeatureModel gameFeatureModel);
        ResultMessage PutGameFeatureDetail(GameFeatureDetailModel gameFeatureDetailModel);
        List<GameFeatureDetailModel> GetGameFeatureDetails(int language);
        List<GameFeatureModel> GetGameFeatuesModelWithDetails(int language);
        void BuildFeaturesDetailsTemplate(int language, List<GameFeatureDetailModel> gameFeatureDetailModels);
        bool CreateGameFeatures(int languageId);
    }
}
