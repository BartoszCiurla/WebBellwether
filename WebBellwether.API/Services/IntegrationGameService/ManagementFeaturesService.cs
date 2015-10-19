using System.Collections.Generic;
using System.Linq;
using WebBellwether.API.Models.IntegrationGame;
using WebBellwether.API.Repositories;
using WebBellwether.API.Results;
using WebBellwether.API.Services.IntegrationGameService.Abstract;
using WebBellwether.API.UnitOfWork;

namespace WebBellwether.API.Services.IntegrationGameService
{
    public class ManagementFeaturesService : IManagementFeaturesService
    {
        private readonly IntegrationGameUnitOfWork _unitOfWork;

        public ManagementFeaturesService(IntegrationGameUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ResultState PutGameFeature(GameFeatureModel gameFeatureModel)
        {
            var entity =
               _unitOfWork.GameFeatureLanguageRepository.GetWithInclude(
                    x => x.GameFeature.Id == gameFeatureModel.Id && x.Language.Id == gameFeatureModel.LanguageId).SingleOrDefault();

            if (entity == null)
                return ResultState.GameFeatureNotExists;
            entity.GameFeatureName = gameFeatureModel.GameFeatureName;
            _unitOfWork.Save();
            return ResultState.GameFeatureEdited;
        }
        public ResultState PutGameFeatureDetail(GameFeatureDetailModel gameFeatureDetailModel)
        {
            var entity =
               _unitOfWork.GameFeatureDetailLanguageRepository.GetWithInclude(
                    x => x.Id == gameFeatureDetailModel.Id && x.Language.Id == gameFeatureDetailModel.LanguageId).SingleOrDefault();
            if (entity == null)
                return ResultState.GameFeatureDetailNotExists;
            entity.GameFeatureDetailName = gameFeatureDetailModel.GameFeatureDetailName;
            _unitOfWork.Save();
            return ResultState.GameFeatureDetailEdited;
        }


        public List<GameFeatureModel> GetGameFeatuesModelWithDetails(int language)
        {
            var gameFeaturesDetails = new List<GameFeatureModel>();
            _unitOfWork.GameFeatureLanguageRepository.GetWithInclude(x => x.Language.Id == language).ToList().ForEach(
                x =>
                {
                    gameFeaturesDetails.Add(new GameFeatureModel
                    {
                        Id = x.GameFeature.Id,
                        GameFeatureName = x.GameFeatureName,
                        GameFeatureDetailModels = new List<GameFeatureDetailModel>()
                    });
                });
            gameFeaturesDetails.ForEach(x =>
            {
                _unitOfWork.GameFeatureDetailLanguageRepository.GetWithInclude(
                    z => z.Language.Id == language && z.GameFeatureDetail.GameFeature.Id == x.Id).ToList().ForEach(
                        y =>
                        {
                            x.GameFeatureDetailModels.Add(new GameFeatureDetailModel { Id = y.Id, GameFeatureDetailId = y.GameFeatureDetail.Id, GameFeatureId = y.GameFeatureDetail.GameFeature.Id, GameFeatureDetailName = y.GameFeatureDetailName, Language = y.Language.LanguageName, LanguageId = y.Language.Id });
                        });                

            });
            return gameFeaturesDetails;
        }

        public List<GameFeatureDetailModel> GetGameFeatureDetails(int language)
        {
            var gameFeatureDetails = new List<GameFeatureDetailModel>();
            _unitOfWork.GameFeatureDetailLanguageRepository.GetWithInclude(x => x.Language.Id == language).ToList().ForEach(x =>
            {
                gameFeatureDetails.Add(new GameFeatureDetailModel { Id = x.Id, GameFeatureDetailId = x.GameFeatureDetail.Id, GameFeatureId = x.GameFeatureDetail.GameFeature.Id, GameFeatureDetailName = x.GameFeatureDetailName, Language = x.Language.LanguageName, LanguageId = x.Language.Id });
            });
            BuildFeaturesDetailsTemplate(language, gameFeatureDetails);

            return gameFeatureDetails;
        }
        public void BuildFeaturesDetailsTemplate(int language, List<GameFeatureDetailModel> gameFeatureDetailModels)
        {
            var checkIsExists = _unitOfWork.LanguageRepository.GetFirst(x => x.LanguageName == "English");
            if (checkIsExists != null)
            {
                int enId = checkIsExists.Id;
                if (enId != language) // then i build template features for edit 
                {
                    _unitOfWork.GameFeatureDetailLanguageRepository.GetWithInclude(x => x.Language.Id == enId)
                         .ToList()
                         .ForEach(x => gameFeatureDetailModels.ForEach(z =>
                         {
                             if (z.GameFeatureDetailId == x.GameFeatureDetail.Id)
                                 z.GameFeatureDetailTemplateName = x.GameFeatureDetailName;
                         }));
                }

            }
        }
    }
}
