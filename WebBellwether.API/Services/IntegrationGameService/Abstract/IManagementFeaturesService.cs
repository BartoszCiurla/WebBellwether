using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBellwether.API.Models.IntegrationGame;
using WebBellwether.API.Results;

namespace WebBellwether.API.Services.IntegrationGameService.Abstract
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
